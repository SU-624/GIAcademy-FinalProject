using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [Header("튜토리얼용")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;
    [SerializeField] private Image m_FadeOutImg;
    [SerializeField] private GameObject m_ClassBlock1;
    [SerializeField] private GameObject m_ClassBlock2;

    public GameObject EventStartButton;

    public Narrative StartNarrative;

    int checkMonth = 0;       // 현재 Date랑 Week랑 바뀌는 시간이 달라 Date는 바꼈지만 Week는 바뀌지않아 발생하는 오류를 잡기 위해 만든 변수

    bool IsComeInUpdate = false;
    private List<ClassAlramScript> m_ClassAlramScriptList = new List<ClassAlramScript>(AllOriginalJsonData.Instance.OriginalClassAlramScriptData);

    private int m_TutorialCount;
    private int m_ScriptCount;

    private float m_LastTime;
    public void PopUpAcademySettingPanel(PopUpUI academypanel)
    {
        academypanel.TurnOnUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(2340, 1080, true);

        if (Json.Instance.UseLoadingData == false)
        {
            StartNarrative.gameObject.SetActive(true);
            StartCoroutine(StartNarrative.NarrativeImgChange());
            StartCoroutine(StartNarrative.NarrativeTextChange());
            m_TutorialCount = 0;
            m_ScriptCount = 0;
            PlayerInfo.Instance.IsFirstClassSettingPdEnd = false;
            PlayerInfo.Instance.IsUpgradePossible = false;
            PlayerInfo.Instance.IsUpgradeTutorialEnd = false;
        }
        else if (Json.Instance.UseLoadingData)
        {
            checkMonth = AllInOneData.Instance.PlayerData.Month;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            checkMonth = 1;
        }
        if (!PlayerInfo.Instance.IsFirstClassSetting)
        {
            m_ClassBlock1.SetActive(false);
            m_ClassBlock2.SetActive(false);
        }
        if (PlayerInfo.Instance.IsFirstAcademySetting && PlayerInfo.Instance.IsFirstClassSetting)
        {
            if (m_TutorialCount == 0)
            {
                Time.timeScale = 0;
                m_TutorialCount++;
                StartCoroutine(FadeIn());
            }
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (m_TutorialCount == 2)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 3)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 4)
                {
                    //_popUpClassAlramPanel.TurnOnUI();
                    PlayerInfo.Instance.IsFirstClassSettingPdEnd = true;
                    m_TutorialCount++;
                    m_PDAlarm.SetActive(false);

                    _SetClassData.MakeClass();
                    _SetClassData.MakeProfessor();
                    _SetClassData.InitData();
                    _popUpClassPanel.TurnOnUI();
                }
            }

#elif UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0); 
                if (PlayerInfo.Instance.IsFirstClassSetting && touch.phase == TouchPhase.Ended)
                {
                    if (m_TutorialCount == 2)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 3)
                    {
                        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
                        m_TutorialCount++;
                    }
                    else if (m_TutorialCount == 4)
                    {
                        _popUpClassAlramPanel.TurnOnUI();
                        PlayerInfo.Instance.IsFirstClassSettingPdEnd = true;
                        m_TutorialCount++;
                        m_PDAlarm.SetActive(false);
                        StartCoroutine(ClassAlramPanel());
                    }
                }
            }
#endif

        }
        // 월 바뀌었을 때 -> 여기 else 에 안들어오네 이거 확인하자
        if (GameTime.Instance.FlowTime.NowDay == 1 && GameTime.Instance.FlowTime.NowWeek == 1 && checkMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            if (checkMonth != 0 && IsComeInUpdate == false && GameTime.Instance.FlowTime.NowMonth != 2 && !PlayerInfo.Instance.IsFirstClassSetting)
            {
                _popUpClassAlramPanel.TurnOnUI();
                StartCoroutine(ClassAlramPanel());
                IsComeInUpdate = true;
            }
            if (checkMonth == 12)       // 일 년 주기 끝나고 1월로 설정
            {
                checkMonth = 1;
            }
            else
            {
                checkMonth += 1;
            }
        }

        if (GameTime.Instance.FlowTime.NowDay == 3)
        {
            IsComeInUpdate = false;
        }
    }

    IEnumerator FadeIn()
    {
        m_LastTime = Time.realtimeSinceStartup;
        while (m_FadeOutImg.color.a > 0f)
        {
            float deltaTime = Time.realtimeSinceStartup - m_LastTime;
            m_FadeOutImg.color = new Color(m_FadeOutImg.color.r, m_FadeOutImg.color.g, m_FadeOutImg.color.b, m_FadeOutImg.color.a - (deltaTime / 0.5f));
            m_LastTime = Time.realtimeSinceStartup;
            yield return null;
        }
        m_FadeOutImg.gameObject.SetActive(false);

        m_TutorialPanel.SetActive(true);
        m_PDAlarm.SetActive(true);
        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
        m_ScriptCount++;
        m_TutorialCount++;
    }

    private IEnumerator ClassAlramPanel()
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

    private string FindScriptToMonth()
    {
        int randomIndex = Random.Range(0, 2);

        int firstConditionScriptIndex = m_ClassAlramScriptList.FindIndex(x => x.Month == GameTime.Instance.FlowTime.NowMonth);

        return m_ClassAlramScriptList[randomIndex + firstConditionScriptIndex].Text;
    }
}
