using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 2023. 01. 10 Mang
/// 
/// 이벤트, 수업 등 일정 시간에 발생할 모든 이벤트를 시간 체크 후 발생 시켜 줄 스크립트
/// </summary>
public class ClockScheduler : MonoBehaviour
{
    [SerializeField] PopUpUI _popUpAcademySettingPanel;
    [SerializeField] GameObject FirstClassAlarmPanel;
    [SerializeField] private SelectClass _SetClassData;
    [SerializeField] PopUpUI _popUpClassAlramPanel;
    [SerializeField] PopUpUI _popUpClassPanel;
    [SerializeField] PopOffUI _popOffClassAlramPanel;
    [SerializeField] TextMeshProUGUI _ClassAlramPanelScript;
    [SerializeField] PopUpUI _PopUpEventPanel;
    [SerializeField] PopUpUI _TempEventPanel;
    [SerializeField] GameObject _tempEventPanelforText;

    [Space(5f)]
    [Header("수업 선택 튜토리얼용")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;

    public GameObject EventStartButton;

    public Narrative StartNarrative;


    int checkMonth = 0;       // 현재 Date랑 Week랑 바뀌는 시간이 달라 Date는 바꼈지만 Week는 바뀌지않아 발생하는 오류를 잡기 위해 만든 변수

    bool isAlreadySetEvent = false;

    bool IsResetEventList = true;

    bool IsComeInUpdate = false;
    bool FirstIsComeInUpdate = false;
    private List<ClassAlramScript> m_ClassAlramScriptList = new List<ClassAlramScript>(AllOriginalJsonData.Instance.OriginalClassAlramScriptData);
    private string _ClassAlramScript;

    private int m_TutorialCount;

    public void PopUpAcademySettingPanel(PopUpUI academypanel)
    {
        academypanel.TurnOnUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(2340, 1080, true);
        if (Json.Instance.IsSavedDataExists == false)
        {
            // _popUpClassPanel.TurnOnUI();
            // _popUpAcademySettingPanel.TurnOnUI();
            StartNarrative.gameObject.SetActive(true);
            StartCoroutine(StartNarrative.NarrativeImgChange());
            StartCoroutine(StartNarrative.NarrativeTextChange());
            //StartCoroutine(StartNarrative.NarrativeContinue());
            m_TutorialCount = 0;
            PlayerInfo.Instance.IsFirstClassSettingPDEnd = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInfo.Instance.IsFirstAcademySetting && PlayerInfo.Instance.IsFirstClassSetting)
        {
            if (m_TutorialCount == 0)
            {
                m_TutorialPanel.SetActive(true);
                m_PDAlarm.SetActive(true);
                m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                m_TutorialCount++;
                IsComeInUpdate = true;
                Time.timeScale = 0;
            }
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (m_TutorialCount == 1)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 2)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 3)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 4)
                {
                    _popUpClassAlramPanel.TurnOnUI();
                    PlayerInfo.Instance.IsFirstClassSettingPDEnd = true;
                    m_TutorialCount++;
                    m_PDAlarm.SetActive(false);
                    StartCoroutine(ClassAlramPanel());
                }
            }

#elif UNITY_ANDROID
            if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
            {
                Touch touch = Input.GetTouch(0); 
                if (PlayerInfo.Instance.IsFirstClassSetting && touch.phase == TouchPhase.Ended)
                {
                    if (m_TutorialCount == 1)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 2)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 3)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_TutorialCount];
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 4)
                    {
                        _popUpClassAlramPanel.TurnOnUI();
                        PlayerInfo.Instance.IsFirstClassSettingPDEnd = true;
                        m_TutorialCount++;
                        StartCoroutine(ClassAlramPanel());
                    }
                }
            }
#endif

        }
        // 월 바뀌었을 때 -> 여기 else 에 안들어오네 이거 확인하자
        if (GameTime.Instance.FlowTime.NowDay == 1 && GameTime.Instance.FlowTime.NowWeek == 1 && checkMonth != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowMonth != 2)
        {
            //if (checkMonth == 0 && FirstIsComeInUpdate == false)       // 맨 처음에만 불리도록
            //{
            //    FirstIsComeInUpdate = true;

            //    checkMonth = 3;

            //    // _popUpClassPanel.TurnOnUI();
            //}
            //else
            if (checkMonth != 0 && IsComeInUpdate == false)
            {
                _popUpClassAlramPanel.TurnOnUI();
                StartCoroutine(ClassAlramPanel());
                IsComeInUpdate = true;
            }
            else if (checkMonth == 0 && PlayerInfo.Instance.IsFirstAcademySetting && IsComeInUpdate == false)
            {
                //_popUpClassAlramPanel.TurnOnUI();
                //StartCoroutine(ClassAlramPanel());
                //IsComeInUpdate = true;
            }
        }

        if (GameTime.Instance.IsGameMode == true)
        {
            AutoEventPopUp();           // 정해진 날마다 자동으로 이벤트 창 팝업
        }

        // 월 바뀌었을 때
        if (GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1 && IsResetEventList == false)
        {
            if (checkMonth == 12)       // 일 년 주기 끝나고 1월로 설정
            {
                checkMonth = 1;
            }
            else
            {
                checkMonth += 1;
            }

            Debug.Log("나의 현 월 이벤트리스트 싹 비우기");
            isAlreadySetEvent = false;
            //EventSchedule.Instance.ResetPossibleCount();
            //SwitchEventList.Instance.ReturnEventPrefabToPool();
            IsResetEventList = true;
            // SwitchEventList.Instance.IsSetEventList = false;
            // SwitchEventList.Instance.PrevIChoosedEvent.Clear();

            IsComeInUpdate = false;
        }

        if (GameTime.Instance.FlowTime.NowWeek != 1 && GameTime.Instance.FlowTime.NowDay != 1)
        {
            IsResetEventList = false;
        }
    }

    // 월마다 이벤트 창이 자동으로 뜨게 하기
    public void AutoEventPopUp()
    {
        //foreach (var thisMonthEvent in SwitchEventList.Instance.MyEventList)
        //for (int i = 0; i < SwitchEventList.Instance.ThisMonthMySelectEventList.Count; i++)
        //{
        //   // if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].IsPopUp == false)
        //   // {
        //   //     if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[1] == GameTime.Instance.FlowTime.NowMonth)
        //   //     {
        //   //         if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[2] == GameTime.Instance.FlowTime.NowWeek)
        //   //         {
        //   //             if (SwitchEventList.Instance.ThisMonthMySelectEventList[i].SuddenEventDate[3] == GameTime.Instance.FlowTime.NowDay)
        //   //             {
        //   //                 _tempEventPanelforText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SwitchEventList.Instance.ThisMonthMySelectEventList[i].EventClassName + " 이벤트 발생!";
        //   //                 _TempEventPanel.TurnOnUI();
        //   //                 SwitchEventList.Instance.ThisMonthMySelectEventList[i].IsPopUp = true;
        //   //             }
        //   //         }
        //   //     }
        //   // }
        //}
    }

    public IEnumerator ClassAlramPanel()
    {
        _ClassAlramPanelScript.text = FindScriptToMonth();

        yield return new WaitUntil(() =>
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                _SetClassData.MakeClass();
                _SetClassData.MakeProfessor();
                _SetClassData.InitData();
                _popUpClassPanel.TurnOnUI();
                _popOffClassAlramPanel.TurnOffUI();
                return true;
            }
            else
            {
                return false;
            }
        });
    }

    public void PopUpEventStartButton()
    {
        // 월 -> 3, 6, 9, 12월 일때 분기별 수업 후 이벤트 진행 되도록

        EventStartButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameTime.Instance.FlowTime.NowMonth.ToString() + "월";
        _PopUpEventPanel.TurnOnUI();

        isAlreadySetEvent = true;

        InGameTest.Instance._isSelectClassNotNull = false;
    }

    private string FindScriptToMonth()
    {
        int randomIndex = Random.Range(0, 2);

        int firstConditionScriptIndex = m_ClassAlramScriptList.FindIndex(x => x.Month == GameTime.Instance.FlowTime.NowMonth);

        return m_ClassAlramScriptList[randomIndex + firstConditionScriptIndex].Text;
    }
}
