using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using Coffee.UIExtensions;

public struct SaveSelectClassInfoData
{
    public Class m_SelectClassDataSave;
    public ProfessorStat m_SelectProfessorDataSave;
    public string m_Week;
}

// 수업을 만들 때 이미지 교체를 위한 enum클래스
enum PartName
{
    GameDesigner,
    Art,
    Programming,
    Commonm,
    Special
}

/// <summary>
/// 새로 바뀐 수업 선택 팝업창에 따른 수업선택 구조 변경 중...
/// 프리팹 단위로 관리할 수 있게 변경 중(UI구조 변경으로 인해 나중에 할 작업을 지금 시작,,!)
/// 
/// 23.02.13 Ocean / 23.04.20 Ocean
/// </summary>
public class SelectClass : MonoBehaviour
{
    public Color m_ClassStatTextColor;
    [SerializeField] private GameObject m_ClassPrefab;
    [SerializeField] private GameObject m_ProfessorPrefab;
    [SerializeField] private GameObject m_CheckClassPrefab;

    [SerializeField] private Button m_OpenClassButton;

    [SerializeField] private ClassController m_SelectClass;                                                     // 원하는 수업의 세부내용을 보여주기 위해

    [SerializeField] private TextMeshProUGUI m_InGameUICurrentMoney;

    [SerializeField] private Sprite[] m_ClassPartImage;
    [SerializeField] private Sprite[] m_StatImage;
    [SerializeField] private Sprite[] m_ClassPanelPartImage;
    [SerializeField] private Sprite[] m_WeekButtonSprite;
    [SerializeField] private Sprite m_FullTimeProfessor;
    [SerializeField] private Sprite m_AdjunctProfessor;

    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;
    [SerializeField] private Button m_NextButton;

    [SerializeField] private Image m_FadeOutImg;

    public SetClassPanel m_SetClassPanel;
    public CheckSelecteClass m_CheckClassPanel;

    public List<string> m_SelectClassButtonName = new List<string>();                                           // 내가 클릭한 버튼에 최종적으로 선택한 수업을 넣어주기 위해 만든 리스트

    public SaveSelectClassInfoData m_SaveData = new SaveSelectClassInfoData();
    public SaveSelectClassInfoData m_TempSaveData = new SaveSelectClassInfoData();
    public static List<SaveSelectClassInfoData> m_GameDesignerData = new List<SaveSelectClassInfoData>();
    public static List<SaveSelectClassInfoData> m_ArtData = new List<SaveSelectClassInfoData>();
    public static List<SaveSelectClassInfoData> m_ProgrammingData = new List<SaveSelectClassInfoData>();

    public EachClass m_NowClass = new EachClass();                                                              // 내가 현재 가지고 있는 수업들

    private ProfessorStat _ClickProfessorData;
    private Class _ClickClassData;
    private Class _prevClickClassData;
    private GameObject _prevClass;
    private GameObject _prevProfessor;
    private List<IncreaseFee> m_ClassFeeList = new List<IncreaseFee>();

    public int m_WeekClassIndex = 0;                                                                            // 2이 되면 1,2주차 수업 모두 선택한거니 완료창으로 넘겨주기
    public int m_WeekProfessorIndex = 0;                                                                        // 2이 되면 1,2주차 교수 모두 선택한거니 완료창으로 넘겨주기
    public int m_SaveDataIndex = 0;                                                                             // 저장할 데이터들의 인덱스를 바꿔주기 위한 변수. 0과 1만 사용한다.
    public int m_TotalMoney = 0;                                                                                // SelectPanel에서 내가 선택한 수업들의 총 수업료를 계산해주기 위한 변수
    public int m_TotalHealth = 0;

    private int m_ClassStack = 0;                                                                               // 0~2까지는 1주차 기획,아트, 플밍 3~5까지는 2주차 기획,아트,플밍 강사, 수업 정보
    private int[] m_StatNum = new int[5];
    private string[] m_StatName = new string[5];
    private int[] m_AllInfoStatNum = new int[5];
    private string[] m_AllInfoStatName = new string[5];
    private string m_Week;                                                                                      // 수업 수정을 눌렀을 때 내가 누른 수업이 몇 주차 수업인지 담아주는 변수

    private string m_ModifyProfessorString;                                                                     // 수업을 다 선택한 후 수정 버튼을 눌러서 해당 학과의 수업을 수정하려는데 교수나 수업 둘 중 하나만 선택할 수 있으니 기존의 정보를 남겨두기 위한 예외처리 변수

    private string m_ModifyClassString;                                                                         // 수업을 다 선택한 후 수정 버튼을 눌러서 해당 학과의 수업을 수정하려는데 교수나 수업 둘 중 하나만 선택할 수 있으니 기존의 정보를 남겨두기 위한 예외처리 변수
    private string _currentMoney;
    private int m_CurrentMoney = 0;                                                                             // SelectPanel에서 내가 현재 보유하고 있는 재화를 띄워주기 위한 변수
    private int[] m_ClassMoney = new int[6];                                                                    // 클릭한 수업들의 수업료를 배열에 저장하여 배열의 요소들을 모두 더해주는 형식
    private int[] m_ClassHealth = new int[6];
    private bool _isOpenyearTrue = false;
    private bool _isOpenMonthTrue = false;
    private bool m_IsChangeWeekend = false;                                                                     // 1,2 주차 수업선택을 모두 하고 완료를 누를 때 다시 false로 바꿔주기
    private bool m_IsMoneyOK = false;
    private bool m_IsHealthOK = false;

    private int m_TutorialCount;
    private int m_ScriptCount;

    private bool m_IsTutorialStart;

    #region _구조체 형식의 리스트 인덱스를 바꾸기 위한 함수

    public void ChangeListIndex(List<SaveSelectClassInfoData> _tempList, int _index, SaveSelectClassInfoData _saveData)
    {
        SaveSelectClassInfoData _temp = _tempList[_index];
        _temp = _saveData;
        _tempList[_index] = _temp;
    }

    #endregion
    
    private void Update()
    {
        _currentMoney = m_InGameUICurrentMoney.text.Replace(",", "");

        m_CurrentMoney = int.Parse(_currentMoney);

        if (m_SetClassPanel.CurrentMoney != null)
        {
            m_SetClassPanel.CurrentMoney.text = string.Format("{0:#,0}", m_CurrentMoney);
            m_CheckClassPanel.SetCurrentMoney(string.Format("{0:#,0}", m_CurrentMoney));
        }

        if (PlayerInfo.Instance.IsFirstAcademySetting && PlayerInfo.Instance.IsFirstClassSetting &&
            PlayerInfo.Instance.IsFirstClassSettingPdEnd && !m_IsTutorialStart)
        {
            m_TutorialPanel.SetActive(true);
            m_PDAlarm.SetActive(false);
            m_BlackScreen.SetActive(true);
            m_Unmask.gameObject.SetActive(true);
            m_Unmask.fitTarget = m_SetClassPanel.PartWeek.GetComponent<RectTransform>();
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(400, 0, 0);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_NextButton.gameObject.SetActive(true);
            m_ScriptCount++;
            m_TutorialCount++;
            m_IsTutorialStart = true;
            //#if UNITY_EDITOR || UNITY_EDITOR_WIN
            //            if (Input.GetMouseButtonDown(0))
            //            {
            //                TutorialContinue();
            //            }
            //#elif UNITY_ANDROID
            //            if (Input.touchCount == 1)
            //            {
            //                Touch touch = Input.GetTouch(0); 
            //                if (touch.phase == TouchPhase.Ended)
            //                {
            //                    TutorialContinue();
            //                    ClickEventManager.Instance.Sound.PlayIconTouch();
            //                }
            //            }
            //#endif
        }
    }

    private void TutorialContinue()
    {
        //if (m_TutorialCount == 0)
        //{
        //    m_TutorialPanel.SetActive(true);
        //    m_PDAlarm.SetActive(false);
        //    m_BlackScreen.SetActive(true);
        //    m_Unmask.gameObject.SetActive(true);
        //    m_Unmask.fitTarget = m_SetClassPanel.PartWeek.GetComponent<RectTransform>();
        //    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(400, 0, 0);
        //    m_TutorialTextImage.gameObject.SetActive(true);
        //    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
        //    m_ScriptCount++;
        //    m_TutorialCount++;
        //}
        if (m_TutorialCount == 1)
        {
            m_Unmask.fitTarget = m_SetClassPanel.MoneyRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-450, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 2)
        {
            m_SetClassPanel.ProfessorScrollView.vertical = false;
            m_Unmask.fitTarget = m_SetClassPanel.ProfessorListRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(500, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 3)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 4)
        {
            m_SetClassPanel.ProfessorListRect.gameObject.SetActive(false);
            m_Unmask.fitTarget =
                m_SetClassPanel.ProfessorPrefabParent.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 6)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 7)
        {
            m_SetClassPanel.ClassListRect.gameObject.SetActive(false);
            m_Unmask.fitTarget = m_SetClassPanel.ClassPrefabParent.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 9)
        {
            m_Unmask.fitTarget = m_SetClassPanel.CompleteButton.gameObject.GetComponent<RectTransform>();
            m_SetClassPanel.CompleteButton.enabled = true;
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-200, 150, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 11)
        {
            m_TutorialPanel.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 14)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 15)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 16)
        {
            m_Unmask.gameObject.SetActive(false);
            m_BlackScreen.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialPanel.SetActive(false);
            m_TutorialCount++;
            PlayerInfo.Instance.IsFirstClassSetting = false;
            GameTime.Instance.AlarmControl.AlarmMessageQ.Enqueue("1학기가 시작되었습니다.");
            Time.timeScale = 1;
            m_IsTutorialStart = false;
        }
    }

    private void Start()
    {
        m_TutorialCount = 0;
        m_ScriptCount = 3;
        m_GameDesignerData.Capacity = 2;
        m_ArtData.Capacity = 2;
        m_ProgrammingData.Capacity = 2;
        InitClass();
        InitDifficultyClassFee();
        m_SetClassPanel.TotalMoney(Color.white, "0");
        m_NextButton.onClick.AddListener(TutorialContinue);
        m_NextButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_IsTutorialStart = false;
    }

    // 수업 선택 패널이 켜지자 마자 띄워줄 수업들을 각 리스트에 넣어준다.
    private void InitClass()
    {
        for (int i = 0; i < m_SelectClass.classData.Count; i++)
        {
            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Art &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.ArtClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.GameDesigner &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.GameDesignerClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Programming &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.ProgrammingClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.None &&
                m_SelectClass.classData.ElementAt(i).Value.m_UnLockClass)
            {
                m_NowClass.NoneClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }
        }
    }

    public void InitData()
    {
        m_GameDesignerData.Clear();
        m_ArtData.Clear();
        m_ProgrammingData.Clear();
        string _money = (int.Parse(m_InGameUICurrentMoney.text.Replace(",", "")) - m_TotalMoney).ToString();
        m_InGameUICurrentMoney.text = string.Format("{0:#,0}", _money);

        for (int i = 0; i < 2; i++)
        {
            m_GameDesignerData.Add(new SaveSelectClassInfoData());
            m_ArtData.Add(new SaveSelectClassInfoData());
            m_ProgrammingData.Add(new SaveSelectClassInfoData());
        }
    }

    // 학원 랭킹에 따른 수업료 상승폭
    private void InitDifficultyClassFee()
    {
        m_ClassFeeList.Add(new IncreaseFee(Rank.SSS, 6f, 6f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.SS, 4.5f, 4.5f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.S, 4.5f, 4.5f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.A, 3f, 3f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.B, 3f, 3f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.C, 1.5f, 1.5f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.D, 1.5f, 1.5f));
        m_ClassFeeList.Add(new IncreaseFee(Rank.E, 1f, 1));
        m_ClassFeeList.Add(new IncreaseFee(Rank.F, 1f, 1));
    }

    private Sprite SetStatImage(string _statName)
    {
        Sprite _stat = null;

        switch (_statName)
        {
            case "통찰":
            {
                _stat = m_StatImage[(int)AbilityType.Insight];
            }
            break;

            case "집중":
            {
                _stat = m_StatImage[(int)AbilityType.Concentration];
            }
            break;

            case "감각":
            {
                _stat = m_StatImage[(int)AbilityType.Sense];
            }
            break;

            case "기술":
            {
                _stat = m_StatImage[(int)AbilityType.Technique];
            }
            break;

            case "재치":
            {
                _stat = m_StatImage[(int)AbilityType.Wit];
            }
            break;
        }

        return _stat;
    }

    // 스탯이 몇 개인지에 따라 수업 프리팹의 스탯들을 셋팅해주는 함수
    private void SetClassStats(int _statCount, GameObject _classPrefab)
    {
        if (_statCount == 5)
        {
            _classPrefab.GetComponent<ClassPrefab>().AllStat.SetActive(true);
            _classPrefab.GetComponent<ClassPrefab>().AllStatText.text = m_StatNum[0].ToString();
            _classPrefab.GetComponent<ClassPrefab>().AllStatText.color = Color.black;
        }
        else
        {
            for (int i = 0; i < _statCount; i++)
            {
                _classPrefab.GetComponent<ClassPrefab>().Stat[i].SetActive(true);
                _classPrefab.GetComponent<ClassPrefab>().StatText[i].text = m_StatNum[i].ToString();
                _classPrefab.GetComponent<ClassPrefab>().StatText[i].color = Color.black;
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                _classPrefab.GetComponent<ClassPrefab>().StatImage[i].sprite = _statSprite;
            }
        }
    }

    ///
    // 스탯이 몇 개인지에 따라 교수 정보가 있다면 교수의 강의력만큼 수업 프리팹의 스탯들을 셋팅해주는 함수
    private void SetClassStatsToProfessorPower(ProfessorStat _professor, int _statCount, GameObject _classPrefab)
    {
        float _professorPower = Professor.Instance.m_StatMagnification[_professor.m_ProfessorPower];
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

        Color _textColor = _professorPower != 0 ? m_ClassStatTextColor : Color.black;

        if (_statCount == 5)
        {
            double _stat = Math.Round(m_StatNum[0] * _professorPower);

            _classPrefab.GetComponent<ClassPrefab>().AllStat.SetActive(true);
            _classPrefab.GetComponent<ClassPrefab>().AllStatText.text = _stat.ToString();
            _classPrefab.GetComponent<ClassPrefab>().AllStatText.color = _textColor;
        }
        else
        {
            for (int i = 0; i < _statCount; i++)
            {
                double _stat = Math.Round(m_StatNum[i] * _professorPower);

                _classPrefab.GetComponent<ClassPrefab>().Stat[i].SetActive(true);
                _classPrefab.GetComponent<ClassPrefab>().StatText[i].text = _stat.ToString();
                _classPrefab.GetComponent<ClassPrefab>().StatText[i].color = _textColor;
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                _classPrefab.GetComponent<ClassPrefab>().StatImage[i].sprite = _statSprite;
            }
        }
    }

    /// 
    // 스탯이 몇 개인지에 따라 선택이 완료된 수업 프리팹의 스탯들을 셋팅해주는 함수
    private void SetComplelteClassStats(int _statCount, GameObject _completePrefab)
    {
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

        if (_statCount == 5)
        {
            //double _stat = Math.Round(m_StatNum[0]  * _increase);

            _completePrefab.GetComponent<CompleteClassPrefab>().AllStat.SetActive(true);
            _completePrefab.GetComponent<CompleteClassPrefab>().AllStatText.text = m_StatNum[0].ToString();
        }
        else
        {
            for (int i = 0; i < _statCount; i++)
            {
                //double _stat = Math.Round(m_StatNum[0] * _increase);

                _completePrefab.GetComponent<CompleteClassPrefab>().Stat[i].SetActive(true);
                _completePrefab.GetComponent<CompleteClassPrefab>().StatText[i].text = m_StatNum[i].ToString();
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                _completePrefab.GetComponent<CompleteClassPrefab>().StatImage[i].sprite = _statSprite;
            }
        }
    }

    // 수업의 등장 조건을 판별하는 함수
    private void DiscriminateClassOpenDate(List<Class> _class, int _index)
    {
        _isOpenyearTrue = false;
        _isOpenMonthTrue = false;

        if (_class[_index].m_OpenYear.Contains(",") && _class[_index].m_OpenMonth.Contains(","))
        {
            string[] _openYear = _class[_index].m_OpenYear.Split(",");
            string[] _openMonth = _class[_index].m_OpenMonth.Split(",");

            for (int x = 0; x < _openYear.Length; x++)
            {
                if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                {
                    _isOpenyearTrue = true;
                    break;
                }
            }

            for (int x = 0; x < _openMonth.Length; x++)
            {
                if (_openMonth[x] == GameTime.Instance.FlowTime.NowMonth.ToString())
                {
                    _isOpenMonthTrue = true;
                    break;
                }
            }
        }
        else if (_class[_index].m_OpenYear.Contains(",") && !_class[_index].m_OpenMonth.Contains(","))
        {
            string[] _openYear = _class[_index].m_OpenYear.Split(",");

            for (int x = 0; x < _openYear.Length; x++)
            {
                if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                {
                    _isOpenyearTrue = true;
                    break;
                }
            }

            if (_class[_index].m_OpenMonth == "0" ||
                _class[_index].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth.ToString())
            {
                _isOpenMonthTrue = true;
            }
        }
        else
        {
            if (_class[_index].m_OpenMonth.Contains(","))
            {
                string[] _openMonth = _class[_index].m_OpenMonth.Split(",");

                for (int x = 0; x < _openMonth.Length; x++)
                {
                    if (_openMonth[x] == GameTime.Instance.FlowTime.NowMonth.ToString())
                    {
                        _isOpenMonthTrue = true;
                        break;
                    }
                }
            }
            else if (_class[_index].m_OpenMonth == "0" ||
                     _class[_index].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth.ToString())
            {
                _isOpenMonthTrue = true;
            }

            if (_class[_index].m_OpenYear.Contains(","))
            {
                string[] _openYear = _class[_index].m_OpenYear.Split(",");

                for (int x = 0; x < _openYear.Length; x++)
                {
                    if (_openYear[x] == GameTime.Instance.FlowTime.NowYear.ToString())
                    {
                        _isOpenyearTrue = true;
                        break;
                    }
                }
            }
            else if (_class[_index].m_OpenYear == "0" ||
                     _class[_index].m_OpenYear == GameTime.Instance.FlowTime.NowYear.ToString())
            {
                _isOpenyearTrue = true;
            }
        }
    }

    private void InitStatArr()
    {
        for (int i = 0; i < 5; i++)
        {
            m_StatNum[i] = 0;
            m_StatName[i] = "";
            m_AllInfoStatNum[i] = 0;
            m_AllInfoStatName[i] = "";
        }
    }

    // 공통 수업은 항상 모든 학과에 만들어준다
    private void MakeCommonClass()
    {
        for (int i = 0; i < m_NowClass.NoneClass.Count; i++)
        {
            DiscriminateClassOpenDate(m_NowClass.NoneClass, i);

            if (_isOpenyearTrue && _isOpenMonthTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                _classPrefab.name = m_NowClass.NoneClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                // 이 부분을 공통과 학과 특수로 구별해서 바꿔주기
                if (m_NowClass.NoneClass[i].m_ClassStatType == ClassType.Commonm)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Commonm];
                }
                else if (m_NowClass.NoneClass[i].m_ClassStatType == ClassType.Special)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Special];
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.NoneClass[i].m_ClassName;

                InitStatArr();
                FindStat(m_NowClass.NoneClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = string.Format("{0:#,0}", m_NowClass.NoneClass[i].m_Money);
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text = m_NowClass.NoneClass[i].m_Health.ToString();
            }
        }
    }

    // 수업 선택 패널은 순서대로 1주차 기획 아트 플밍, 2주차 기획 아트 플밍으로 나온다. 이 함수는 처음 화면을 만들 함수이다
    public void MakeClass()
    {
        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);

        for (int i = 0; i < m_NowClass.GameDesignerClass.Count; i++)
        {
            DiscriminateClassOpenDate(m_NowClass.GameDesignerClass, i);

            if (_isOpenMonthTrue && _isOpenyearTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                _classPrefab.name = m_NowClass.GameDesignerClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Class)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GameDesigner];
                }
                else if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Special)
                {
                    _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Special];
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.GameDesignerClass[i].m_ClassName;

                InitStatArr();
                FindStat(m_NowClass.GameDesignerClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = string.Format("{0:#,0}", m_NowClass.GameDesignerClass[i].m_Money);
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text = m_NowClass.GameDesignerClass[i].m_Health.ToString();
            }
        }

        MakeCommonClass();
    }

    public void MakeProfessor()
    {
        for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
        {
            GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

            _professorPrefab.name = Professor.Instance.GameManagerProfessor[i].m_ProfessorName;
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

            _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                Professor.Instance.GameManagerProfessor[i].m_ProfessorName;

            if (Professor.Instance.GameManagerProfessor[i].m_ProfessorSet == "전임")
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
            }
            else
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
            }

            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                Professor.Instance.GameManagerProfessor[i].m_ProfessorHealth.ToString();
            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                Professor.Instance.GameManagerProfessor[i].m_ProfessorPassion.ToString();

            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                Professor.Instance.GameManagerProfessor[i].m_ProfessorSkills[0];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                Professor.Instance.GameManagerProfessor[i].m_ProfessorSkills[1];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                Professor.Instance.GameManagerProfessor[i].m_TeacherProfileImg;
            _professorPrefab.GetComponent<ProfessorPrefab>().professorLevelText.text =
                "Lv. " + Professor.Instance.GameManagerProfessor[i].m_ProfessorPower.ToString();
        }
    }

    public void MakeOtherClass()
    {
        // 기획반은 선택됐고 아트반은 선택이 안됐을 때 
        if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
            m_ArtData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            m_SetClassPanel.SetBackButtonActive(true);


            m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);

            for (int i = 0; i < m_NowClass.ArtClass.Count; i++)
            {
                DiscriminateClassOpenDate(m_NowClass.ArtClass, i);

                if (_isOpenMonthTrue && _isOpenyearTrue)
                {
                    GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                    _classPrefab.name = m_NowClass.ArtClass[i].m_ClassName;
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                    if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Class)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Art];
                    }
                    else if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Special)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Special];
                    }

                    _classPrefab.GetComponent<ClassPrefab>().ClassName.text = m_NowClass.ArtClass[i].m_ClassName;

                    InitStatArr();
                    FindStat(m_NowClass.ArtClass, i);

                    int StatCount = 0;

                    for (int j = 0; j < 5; j++)
                    {
                        if (m_StatNum[j] != 0 && m_StatName[j] != "")
                        {
                            StatCount += 1;
                        }
                    }

                    SetClassStats(StatCount, _classPrefab);

                    _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = string.Format("{0:#,0}", m_NowClass.ArtClass[i].m_Money);
                    _classPrefab.GetComponent<ClassPrefab>().HealthText.text =
                        m_NowClass.ArtClass[i].m_Health.ToString();
                }
            }

            MakeCommonClass();
        }
        // 기획반과 아트반 모두 선택됐는데 플밍이 선택이 안됐을 때
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            m_SetClassPanel.SetBackButtonActive(true);

            m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);

            for (int i = 0; i < m_NowClass.ProgrammingClass.Count; i++)
            {
                DiscriminateClassOpenDate(m_NowClass.ProgrammingClass, i);

                if (_isOpenMonthTrue && _isOpenyearTrue)
                {
                    GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                    _classPrefab.name = m_NowClass.ProgrammingClass[i].m_ClassName;
                    _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                    if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Class)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Programming];
                    }
                    else if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Special)
                    {
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite =
                            m_ClassPartImage[(int)PartName.Special];
                    }

                    _classPrefab.GetComponent<ClassPrefab>().ClassName.text =
                        m_NowClass.ProgrammingClass[i].m_ClassName;
                    InitStatArr();
                    FindStat(m_NowClass.ProgrammingClass, i);

                    int StatCount = 0;

                    for (int j = 0; j < 5; j++)
                    {
                        if (m_StatNum[j] != 0 && m_StatName[j] != "")
                        {
                            StatCount += 1;
                        }
                    }

                    SetClassStats(StatCount, _classPrefab);

                    _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = string.Format("{0:#,0}", m_NowClass.ProgrammingClass[i].m_Money);
                    _classPrefab.GetComponent<ClassPrefab>().HealthText.text =
                        m_NowClass.ProgrammingClass[i].m_Health.ToString();
                }
            }

            MakeCommonClass();
        }
        // 1주차 기획 아트 플밍 모두 선택했을 때
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave != null)
        {
            MakeClass();
            m_WeekClassIndex++;
        }
    }

    public void MakeOtherProfessor()
    {
        if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
            m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave == null)
        {
            for (int i = 0; i < Professor.Instance.ArtProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                _professorPrefab.name = Professor.Instance.ArtProfessor[i].m_ProfessorName;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                    Professor.Instance.ArtProfessor[i].m_ProfessorName;

                if (Professor.Instance.ArtProfessor[i].m_ProfessorSet == "전임")
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
                }
                else
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
                }

                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                    Professor.Instance.ArtProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                    Professor.Instance.ArtProfessor[i].m_ProfessorPassion.ToString();

                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                    Professor.Instance.ArtProfessor[i].m_ProfessorSkills[0];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                    Professor.Instance.ArtProfessor[i].m_ProfessorSkills[1];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                    Professor.Instance.ArtProfessor[i].m_TeacherProfileImg;
                _professorPrefab.GetComponent<ProfessorPrefab>().professorLevelText.text =
                "Lv. " + Professor.Instance.ArtProfessor[i].m_ProfessorPower.ToString();
            }
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ProgrammingData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            MakeProfessor();
            m_WeekProfessorIndex++;
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null &&
                 m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            for (int i = 0; i < Professor.Instance.ProgrammingProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                _professorPrefab.name = Professor.Instance.ProgrammingProfessor[i].m_ProfessorName;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text =
                    Professor.Instance.ProgrammingProfessor[i].m_ProfessorName;

                if (Professor.Instance.ProgrammingProfessor[i].m_ProfessorSet == "전임")
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
                }
                else
                {
                    _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
                }

                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                    Professor.Instance.ProgrammingProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                    Professor.Instance.ProgrammingProfessor[i].m_ProfessorPassion.ToString();

                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                    Professor.Instance.ProgrammingProfessor[i].m_ProfessorSkills[0];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                    Professor.Instance.ProgrammingProfessor[i].m_ProfessorSkills[1];
                _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                    Professor.Instance.ProgrammingProfessor[i].m_TeacherProfileImg;
                _professorPrefab.GetComponent<ProfessorPrefab>().professorLevelText.text =
                "Lv. " + Professor.Instance.ProgrammingProfessor[i].m_ProfessorPower.ToString();
            }
        }
    }

    // 수정하기 버튼을 눌렀을 때 해당하는 패널의 값들을 변경시켜주기 위한 함수
    private void SetStatAllInfo(List<SaveSelectClassInfoData> _tempList, int _index)
    {
        InitStatArr();

        m_SetClassPanel.SetBackButtonActive(false);

        m_SetClassPanel.SetClassName(_tempList[_index].m_SelectClassDataSave.m_ClassName);
        m_SetClassPanel.SetProfessorInfo(_tempList[_index].m_SelectProfessorDataSave.m_ProfessorName,
            _tempList[_index].m_SelectProfessorDataSave.m_TeacherProfileImg);

        int _tempindex = SelectedClassDataStatIndex(_tempList, _index);
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);
        float _magnification = Professor.Instance.m_StatMagnification[_tempList[_index].m_SelectProfessorDataSave.m_ProfessorPower];
        double _increaseStat = Math.Round(_increase * _magnification);

        for (int j = 0; j < _tempindex;)
        {
            if (_tempList[_index].m_SelectClassDataSave.m_Insight > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Insight < 0)
            {

                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Insight * (int)_increaseStat;
                m_StatName[j] = "통찰";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Concentration > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Concentration < 0)
            {
                double _concentration = Math.Round(_increase * _magnification);

                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Concentration * (int)_increaseStat;
                m_StatName[j] = "집중";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Sense > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Sense < 0)
            {
                double _sense = Math.Round(_increase * _magnification);

                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Sense * (int)_increaseStat;
                m_StatName[j] = "감각";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Technique > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Technique < 0)
            {
                double _technique = Math.Round(_increase * _magnification);

                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Technique * (int)_increaseStat;
                m_StatName[j] = "기술";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Wit > 0 || _tempList[_index].m_SelectClassDataSave.m_Wit < 0)
            {
                double _wit = Math.Round(_increase * _magnification);

                m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Wit * (int)_increaseStat;
                m_StatName[j] = "재치";
                j++;
            }
        }

        int StatCount = 0;

        for (int i = 0; i < 5; i++)
        {
            if (m_StatNum[i] != 0 && m_StatName[i] != "")
            {
                StatCount += 1;
            }
        }

        if (StatCount == 5)
        {
            foreach (GameObject _stat in m_SetClassPanel.AllInfoPanelClassStat)
            {
                _stat.SetActive(false);
            }

            m_SetClassPanel.SetStatsActive(true);
            m_SetClassPanel.SetAllStatText(m_StatNum[0].ToString());
        }
        else
        {
            m_SetClassPanel.SetStatsActive(false);

            for (int i = 0; i < StatCount; i++)
            {
                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(true);
                m_SetClassPanel.AllInfoPanelClassStatText[i].text = m_StatNum[i].ToString();
                Sprite _statSprite = SetStatImage(m_StatName[i]);
                m_SetClassPanel.AllInfoPanelClassStatImage[i].sprite = _statSprite;
            }
        }

        m_SetClassPanel.SetClassUseHealth(_tempList[_index].m_SelectClassDataSave.m_Health.ToString());
    }

    // checkPanel에 내가 선택한 수업들의 정보를 띄워주기 위해 CompleteClassPrefab을 셋팅해주는 함수
    private void SetStatCheckClassPrefabInfo(GameObject _obj, List<SaveSelectClassInfoData> _tempList, int _index)
    {
        // CompleteClassPrefab의 수업 이름과 교수 이름 변경 부분.
        _obj.GetComponent<CompleteClassPrefab>().ClassName.text = _tempList[_index].m_SelectClassDataSave.m_ClassName;
        _obj.GetComponent<CompleteClassPrefab>().ProfessorName.text = _tempList[_index].m_SelectProfessorDataSave.m_ProfessorName;
        _obj.GetComponent<CompleteClassPrefab>().ProfessorImage.sprite = _tempList[_index].m_SelectProfessorDataSave.m_TeacherProfileImg;

        int _tempindex = SelectedClassDataStatIndex(_tempList, _index);
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);
        float _magnification = Professor.Instance.m_StatMagnification[_tempList[_index].m_SelectProfessorDataSave.m_ProfessorPower];

        for (int j = 0; j < _tempindex;)
        {
            if (_tempList[_index].m_SelectClassDataSave.m_Insight > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Insight < 0)
            {
                double _insight = Math.Round(_increase * _tempList[_index].m_SelectClassDataSave.m_Insight * _magnification);
                m_StatNum[j] = (int)_insight;
                m_StatName[j] = "통찰";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Concentration > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Concentration < 0)
            {
                double _concentration = Math.Round(_increase * _tempList[_index].m_SelectClassDataSave.m_Concentration * _magnification);
                m_StatNum[j] = (int)_concentration;
                m_StatName[j] = "집중";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Sense > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Sense < 0)
            {
                double _sense = Math.Round(_increase * _tempList[_index].m_SelectClassDataSave.m_Sense * _magnification);
                m_StatNum[j] = (int)_sense;
                m_StatName[j] = "감각";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Technique > 0 ||
                _tempList[_index].m_SelectClassDataSave.m_Technique < 0)
            {
                double _technique = Math.Round(_increase * _tempList[_index].m_SelectClassDataSave.m_Technique * _magnification);
                m_StatNum[j] = (int)_technique;
                m_StatName[j] = "기술";
                j++;
            }

            if (_tempList[_index].m_SelectClassDataSave.m_Wit > 0 || _tempList[_index].m_SelectClassDataSave.m_Wit < 0)
            {
                double _wit = Math.Round(_increase * _tempList[_index].m_SelectClassDataSave.m_Wit * _magnification);
                m_StatNum[j] = (int)_wit;
                m_StatName[j] = "재치";
                j++;
            }
        }

        int StatCount = 0;

        for (int j = 0; j < 5; j++)
        {
            if (m_StatNum[j] != 0 && m_StatName[j] != "")
            {
                StatCount += 1;
            }
        }

        SetComplelteClassStats(StatCount, _obj);

        _obj.GetComponent<CompleteClassPrefab>().MoneyText.text = string.Format("{0:#,0}", _tempList[_index].m_SelectClassDataSave.m_Money);
        _obj.GetComponent<CompleteClassPrefab>().HealthText.text = _tempList[_index].m_SelectClassDataSave.m_Health.ToString();
    }

    private int SelectedClassDataStatIndex(List<SaveSelectClassInfoData> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Insight != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Concentration != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Sense != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Technique != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int UnSelectedClassDataStatIndex(List<Class> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_Insight != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Concentration != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Sense != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Technique != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int UnSelectedClickClassDataStatIndex(Class _tempList)
    {
        int _index = 0;

        if (_tempList.m_Insight != 0)
        {
            _index++;
        }

        if (_tempList.m_Concentration != 0)
        {
            _index++;
        }

        if (_tempList.m_Sense != 0)
        {
            _index++;
        }

        if (_tempList.m_Technique != 0)
        {
            _index++;
        }

        if (_tempList.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    // 1주는 0, 2주는 1 이고 기획반은 1, 아트반은 2, 플밍반은 3이다.
    private void MakeTotalInfo(List<SaveSelectClassInfoData> _tempList, int _partIndex)
    {
        string _1WeekClassName = _tempList[0].m_SelectClassDataSave.m_ClassName;
        string _2WeekClassName = _tempList[1].m_SelectClassDataSave.m_ClassName;
        string _1WeekClassMoney = string.Format("{0:#,0}", _tempList[0].m_SelectClassDataSave.m_Money);
        string _2WeekClassMoney = string.Format("{0:#,0}", _tempList[1].m_SelectClassDataSave.m_Money);

        switch (_partIndex)
        {
            case 1:
            {
                m_CheckClassPanel.SetGameDesignerData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney, _2WeekClassMoney);
            }
            break;

            case 2:
            {
                m_CheckClassPanel.SetArtData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney, _2WeekClassMoney);
            }
            break;

            case 3:
            {
                m_CheckClassPanel.SetProgrammingData(_1WeekClassName, _2WeekClassName, _1WeekClassMoney,
                    _2WeekClassMoney);
            }
            break;
        }
    }

    private int CalculateTotalMoneyCheckPanel()
    {
        int _totalMoney;

        _totalMoney = m_ArtData[0].m_SelectClassDataSave.m_Money + m_ArtData[1].m_SelectClassDataSave.m_Money +
                      m_ProgrammingData[0].m_SelectClassDataSave.m_Money +
                      m_ProgrammingData[1].m_SelectClassDataSave.m_Money
                      + m_GameDesignerData[0].m_SelectClassDataSave.m_Money +
                      m_GameDesignerData[1].m_SelectClassDataSave.m_Money;

        return _totalMoney;
    }

    private void ModifyButton()
    {
        m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
        m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

        m_SetClassPanel.PopUpSelectClassPanel();
        // 수정완료버튼 활성화
        m_SetClassPanel.SetModifiyButton(true);

        m_CheckClassPanel.PopOffCheckClassPanel();

        if (m_SetClassPanel.ClassPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectClassObject();
        }

        if (m_SetClassPanel.ProfessorPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectPanelProfessorObject();
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        GameObject _parentObj = _currentObj.transform.parent.gameObject;

        // 수정 버튼을 눌렀을 때 m_SaveData가 비어있을 수 있으니 내가 클릭한 오브젝트의 정보로 다시 채워주기
        Class _tempData;

        m_ModifyClassString = _parentObj.GetComponent<CompleteClassPrefab>().ClassName.text;

        m_SelectClass.classData.TryGetValue(m_ModifyClassString, out _tempData);

        ProfessorStat _tempProfessor;

        m_ModifyProfessorString = _parentObj.GetComponent<CompleteClassPrefab>().ProfessorName.text;

        Professor.Instance.SelectProfessor.professorData.TryGetValue(m_ModifyProfessorString, out _tempProfessor);

        if (m_SaveData.m_SelectClassDataSave == null || m_SaveData.m_SelectClassDataSave != _tempData)
        {
            m_SaveData.m_SelectClassDataSave = _tempData;
        }

        if (m_SaveData.m_SelectProfessorDataSave == null || m_SaveData.m_SelectProfessorDataSave != _tempProfessor)
        {
            m_SaveData.m_SelectProfessorDataSave = _tempProfessor;
        }

        if (m_Week == "1주차")
        {
            m_SetClassPanel.SetWeekText("1주차");
            m_SaveDataIndex = 0;
            ResetSelectPanel(_tempData, _tempProfessor);
            // 1주차에 선택했던 수업을 보여준다
            m_SetClassPanel._1WeekPanel.SetActive(true);
            m_SetClassPanel._2WeekPanel.SetActive(false);
            m_SetClassPanel.SetDownButton(true);
            m_SetClassPanel.SetUpButton(false);
        }
        else
        {
            m_SetClassPanel.SetWeekText("2주차");
            m_SaveDataIndex = 1;
            ResetSelectPanel(_tempData, _tempProfessor);
            // 2주차에 선택했던 수업을 보여준다
            m_SetClassPanel._1WeekPanel.SetActive(false);
            m_SetClassPanel._2WeekPanel.SetActive(true);
            m_SetClassPanel.SetDownButton(false);
            m_SetClassPanel.SetUpButton(true);
        }
    }

    // 선택 패널 정보 셋팅해주기
    private void SetToSeletPanel(List<Class> _nowClass, List<ProfessorStat> _nowProfessor, string _partName,
        List<SaveSelectClassInfoData> _data)
    {
        MakeCommonClass();

        for (int i = 0; i < _nowClass.Count; i++)
        {
            DiscriminateClassOpenDate(_nowClass, i);

            if (_isOpenyearTrue && _isOpenMonthTrue)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_SetClassPanel.ClassPrefabParent);
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

                _classPrefab.name = _nowClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);
                InitStatArr();
                FindStat(_nowClass, i);

                int StatCount = 0;

                for (int j = 0; j < 5; j++)
                {
                    if (m_StatNum[j] != 0 && m_StatName[j] != "")
                    {
                        StatCount += 1;
                    }
                }

                SetClassStats(StatCount, _classPrefab);

                switch (_partName)
                {
                    case "기획":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GameDesigner];
                    }
                    break;

                    case "아트":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.Art];
                    }
                    break;

                    case "플밍":
                    {
                        m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);
                        _classPrefab.GetComponent<ClassPrefab>().PartImage.sprite = m_ClassPartImage[(int)PartName.GameDesigner];
                    }
                    break;
                }

                _classPrefab.GetComponent<ClassPrefab>().ClassName.text = _nowClass[i].m_ClassName;

                _classPrefab.GetComponent<ClassPrefab>().MoneyText.text = string.Format("{0:#,0}", _nowClass[i].m_Money);
                _classPrefab.GetComponent<ClassPrefab>().HealthText.text = _nowClass[i].m_Health.ToString();
            }
        }

        for (int i = 0; i < _nowProfessor.Count; i++)
        {
            GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_SetClassPanel.ProfessorPrefabParent);
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

            _professorPrefab.name = _nowProfessor[i].m_ProfessorName;
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

            _professorPrefab.GetComponent<ProfessorPrefab>().professorName.text = _nowProfessor[i].m_ProfessorName;

            if (_nowProfessor[i].m_ProfessorSet == "전임")
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_FullTimeProfessor;
            }
            else
            {
                _professorPrefab.GetComponent<ProfessorPrefab>().professorPosition.sprite = m_AdjunctProfessor;
            }

            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentHealth.text =
                _nowProfessor[i].m_ProfessorHealth.ToString();
            _professorPrefab.GetComponent<ProfessorPrefab>().CurrentPassion.text =
                _nowProfessor[i].m_ProfessorPassion.ToString();

            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName.text =
                _nowProfessor[i].m_ProfessorSkills[0];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillInfo.text =
                _nowProfessor[i].m_ProfessorSkills[1];
            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorProfileImg.sprite =
                _nowProfessor[i].m_TeacherProfileImg;
            _professorPrefab.GetComponent<ProfessorPrefab>().professorLevelText.text =
                "Lv. " + Professor.Instance.GameManagerProfessor[i].m_ProfessorPower.ToString();

            switch (_partName)
            {
                case "기획":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.GameDesigner]);
                }
                break;

                case "아트":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Art]);
                }
                break;

                case "플밍":
                {
                    m_SetClassPanel.SetPartImage(m_ClassPanelPartImage[(int)StudentType.Programming]);
                }
                break;
            }

            //if (_nowProfessor[i].m_ProfessorSkills != null)
            //{
            //    for (int j = 0; j < 3;)
            //    {
            //        if (_nowProfessor[i].m_ProfessorSkills[j] != "")
            //        {
            //            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillUnLockImage[j].SetActive(false);
            //            _professorPrefab.GetComponent<ProfessorPrefab>().ProfessorSkillName[j].text = _nowProfessor[i].m_ProfessorSkills[j];

            //            j++;
            //        }
            //        else
            //        {
            //            j = 3;
            //        }
            //    }
            //}
        }

        if (m_SaveDataIndex == 0)
        {
            // 1주
            SetStatAllInfo(_data, 0);
        }
        else
        {
            // 2주
            SetStatAllInfo(_data, 1);
        }
    }

    private void ResetSelectPanel(Class _tempClass, ProfessorStat _tempProfessor)
    {
        if (_tempClass.m_ClassType == StudentType.Art)
        {
            SetToSeletPanel(m_NowClass.ArtClass, Professor.Instance.ArtProfessor, "아트", m_ArtData);
        }
        else if (_tempClass.m_ClassType == StudentType.GameDesigner)
        {
            SetToSeletPanel(m_NowClass.GameDesignerClass, Professor.Instance.GameManagerProfessor, "기획",
                m_GameDesignerData);
        }
        else if (_tempClass.m_ClassType == StudentType.Programming)
        {
            SetToSeletPanel(m_NowClass.ProgrammingClass, Professor.Instance.ProgrammingProfessor, "플밍",
                m_ProgrammingData);
        }
        else if (_tempClass.m_ClassType == StudentType.None)
        {
            if (_tempProfessor.m_ProfessorType == StudentType.GameDesigner)
            {
                SetToSeletPanel(m_NowClass.GameDesignerClass, Professor.Instance.GameManagerProfessor, "기획",
                    m_GameDesignerData);
            }
            else if (_tempProfessor.m_ProfessorType == StudentType.Art)
            {
                SetToSeletPanel(m_NowClass.ArtClass, Professor.Instance.ArtProfessor, "아트", m_ArtData);
            }
            else if (_tempProfessor.m_ProfessorType == StudentType.Programming)
            {
                SetToSeletPanel(m_NowClass.ProgrammingClass, Professor.Instance.ProgrammingProfessor, "플밍",
                    m_ProgrammingData);
            }
        }
    }

    public void ModifyCompleteButton()
    {
        m_SetClassPanel.SetModifiyButton(false);

        if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
        {
            m_ArtData.Add(m_SaveData);
            ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
            m_ArtData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
        {
            m_GameDesignerData.Add(m_SaveData);
            ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
            m_GameDesignerData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
        {
            m_ProgrammingData.Add(m_SaveData);
            ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
            m_ProgrammingData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.None)
        {
            if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
            {
                m_GameDesignerData.Add(m_SaveData);
                ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                m_GameDesignerData.RemoveAt(2);
            }
            else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
            {
                m_ArtData.Add(m_SaveData);
                ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                m_ArtData.RemoveAt(2);
            }
            else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
            {
                m_ProgrammingData.Add(m_SaveData);
                ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                m_ProgrammingData.RemoveAt(2);
            }
        }

        // 모든 학과의 수업을 선택했으면 내가 선택한 정보들을 한번에 띄워 줄 UI를 띄워야한다.
        if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
                                                       && m_GameDesignerData[1].m_SelectClassDataSave != null)
        {
            m_SetClassPanel.PopOffSelectClassPanel();
            m_CheckClassPanel.PopUpCheckClassPanel();

            m_CheckClassPanel.SetTotalMoney(string.Format("{0:#,0}", CalculateTotalMoneyCheckPanel()));

            m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
            m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);

            MakeCheckClass();
        }
    }

    public void MakeCheckClass()
    {
        GameObject _currentButton = EventSystem.current.currentSelectedGameObject;

        _currentMoney = m_InGameUICurrentMoney.text.Replace(",", "");
        m_CurrentMoney = int.Parse(_currentMoney);

        m_CheckClassPanel.SetCurrentMoney(string.Format("{0:#,0}", m_CurrentMoney));

        if (_currentButton.name != "2WeekButton")
        {
            _currentButton = m_CheckClassPanel._1WeekButton.gameObject;
            m_Week = "1주차";
            m_CheckClassPanel._1WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[1];
            m_CheckClassPanel._2WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[2];
        }
        else
        {
            m_Week = "2주차";
            m_CheckClassPanel._1WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[0];
            m_CheckClassPanel._2WeekButton.GetComponent<Image>().sprite = m_WeekButtonSprite[3];
        }

        if (m_CheckClassPanel.CompleteClassPrefabParent.childCount > 0)
        {
            m_CheckClassPanel.DestroyCheckClassObject();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject _checkClassPrefab = Instantiate(m_CheckClassPrefab, m_CheckClassPanel.CompleteClassPrefabParent);

            _checkClassPrefab.GetComponent<CompleteClassPrefab>().ModifiyButton.onClick.AddListener(ModifyButton);

            InitStatArr();

            switch (i)
            {
                case 0:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_GameDesignerData, 0);
                        MakeTotalInfo(m_GameDesignerData, 1);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_GameDesignerData, 1);
                        MakeTotalInfo(m_GameDesignerData, 1);
                    }
                }
                break;

                case 1:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ArtData, 0);
                        MakeTotalInfo(m_ArtData, 2);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ArtData, 1);
                        MakeTotalInfo(m_ArtData, 2);
                    }
                }
                break;

                case 2:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ProgrammingData, 0);
                        MakeTotalInfo(m_ProgrammingData, 3);
                    }
                    else
                    {
                        SetStatCheckClassPrefabInfo(_checkClassPrefab, m_ProgrammingData, 1);
                        MakeTotalInfo(m_ProgrammingData, 3);
                    }
                }
                break;
            }
        }
    }

    public void ClickCompleteButton()
    {
        // 순서대로 보유재화부족과 강사의 체력 부족, 내가 선택한 총 수업료가 보유 재화를 넘어갔을 때 뜨는 경고이다.
        if (m_IsHealthOK == true && m_IsMoneyOK == true)
        {
            _currentMoney = m_SetClassPanel.m_TotalMoney.text.Replace(",", "");

            int _totalMoney = int.Parse(_currentMoney);

            if (_totalMoney > m_CurrentMoney)
            {
                m_SetClassPanel.SetWarningText("재화가 부족합니다.");
                return;
            }

            if (m_SaveData.m_SelectClassDataSave != null && m_SaveData.m_SelectProfessorDataSave != null)
            {
                if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
                {
                    m_ArtData.Add(m_SaveData);
                    ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                    m_ArtData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
                {
                    m_GameDesignerData.Add(m_SaveData);
                    ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                    m_GameDesignerData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
                {
                    m_ProgrammingData.Add(m_SaveData);
                    ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                    m_ProgrammingData.RemoveAt(2);

                    // 모든 학과의 수업을 선택했으면 내가 선택한 정보들을 한번에 띄워 줄 UI를 띄워야한다.
                    if (m_ArtData[1].m_SelectClassDataSave != null
                        && m_ProgrammingData[1].m_SelectClassDataSave != null
                        && m_GameDesignerData[1].m_SelectClassDataSave != null)
                    {

                        m_SetClassPanel.SetBackButtonActive(false);

                        m_CheckClassPanel.PopUpCheckClassPanel();
                        m_SetClassPanel.PopOffSelectClassPanel();

                        m_CheckClassPanel.SetTotalMoney(string.Format("{0:#,0}", CalculateTotalMoneyCheckPanel()));

                        m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
                        m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);


                        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 12)
                        {
                            m_Unmask.gameObject.SetActive(true);
                            m_Unmask.fitTarget =
                                m_CheckClassPanel._StartButton.gameObject.GetComponent<RectTransform>();
                            m_CheckClassPanel._StartButton.onClick.AddListener(ContinueTutorial);
                            m_TutorialTextImage.gameObject.SetActive(false);
                            m_TutorialArrowImage.gameObject.SetActive(true);
                            m_TutorialArrowImage.transform.position =
                                m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                            m_TutorialPanel.SetActive(true);
                            m_TutorialCount++;
                        }

                        MakeCheckClass();
                    }
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.None)
                {
                    if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                    {
                        m_GameDesignerData.Add(m_SaveData);
                        ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                        m_GameDesignerData.RemoveAt(2);
                    }
                    else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                    {
                        m_ArtData.Add(m_SaveData);
                        ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                        m_ArtData.RemoveAt(2);
                    }
                    else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                    {
                        m_ProgrammingData.Add(m_SaveData);
                        ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                        m_ProgrammingData.RemoveAt(2);

                        // 모든 학과의 수업을 선택했으면 내가 선택한 정보들을 한번에 띄워 줄 UI를 띄워야한다.
                        if (m_ArtData[1].m_SelectClassDataSave != null
                            && m_ProgrammingData[1].m_SelectClassDataSave != null
                            && m_GameDesignerData[1].m_SelectClassDataSave != null)
                        {
                            m_CheckClassPanel.PopUpCheckClassPanel();
                            m_SetClassPanel.PopOffSelectClassPanel();

                            m_CheckClassPanel.SetTotalMoney(string.Format("{0:#,0}", CalculateTotalMoneyCheckPanel()));

                            m_CheckClassPanel._1WeekButton.onClick.AddListener(MakeCheckClass);
                            m_CheckClassPanel._2WeekButton.onClick.AddListener(MakeCheckClass);

                            if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 12)
                            {
                                m_Unmask.gameObject.SetActive(true);
                                m_Unmask.fitTarget =
                                    m_CheckClassPanel._StartButton.gameObject.GetComponent<RectTransform>();
                                m_CheckClassPanel._StartButton.onClick.AddListener(ContinueTutorial);
                                m_TutorialTextImage.gameObject.SetActive(false);
                                m_TutorialArrowImage.gameObject.SetActive(true);
                                m_TutorialArrowImage.transform.position =
                                    m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                                m_TutorialPanel.SetActive(true);
                                m_TutorialCount++;
                            }

                            MakeCheckClass();
                        }
                    }
                }

                // 다음 학과로 넘어가니까 저장할 데이터를 담아둘 곳을 비워준다.
                InitStatArr();

                m_SetClassPanel.DestroySelectClassObject();
                m_SetClassPanel.DestroySelectPanelProfessorObject();

                InitAllInfoStat();

                m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
                m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

                MakeOtherClass();
                MakeOtherProfessor();

                if (m_WeekClassIndex == 1 && m_WeekProfessorIndex == 1 && m_IsChangeWeekend == false)
                {
                    m_IsChangeWeekend = true;
                    m_SetClassPanel.SetWeekText("2주차");
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                    m_SaveDataIndex++;
                }

                m_SaveData.m_SelectClassDataSave = null;
                m_SaveData.m_SelectProfessorDataSave = null;
                _ClickProfessorData = null;
                _ClickClassData = null;

                // 수업 선택이 완료 됐으면 증가시키기
                if (m_ClassStack < 5)
                {
                    m_ClassStack++;
                }
            }
        }
        else if (m_IsMoneyOK == false)
        {
            m_SetClassPanel.SetWarningText("재화가 부족합니다.");
        }
        else if (m_IsHealthOK == false)
        {
            m_SetClassPanel.SetWarningText("체력이 부족합니다.");
        }
        else if (m_IsMoneyOK == false && m_IsHealthOK == false)
        {
            m_SetClassPanel.SetWarningText("재화와 체력이 부족합니다.");
        }

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 10)
        {
            m_Unmask.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            //m_TutorialTextImage.transform.position = new Vector3(2340f / 2f, 1080f / 2f, 0);
            m_TutorialTextImage.transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // 수업이 끝난 후 수업 선택창을 비워주기 위한 함수
    public void InitSelecteClass()
    {
        // 다음 학과로 넘어가니까 저장할 데이터를 담아둘 곳을 비워준다.
        InitStatArr();
        //InitData();

        m_SetClassPanel.DestroySelectClassObject();
        m_SetClassPanel.DestroySelectPanelProfessorObject();

        InitAllInfoStat();

        InitTotalInfo(m_SetClassPanel._1WeekPanel);
        InitTotalInfo(m_SetClassPanel._2WeekPanel);

        //MakeOtherClass();
        //MakeOtherProfessor();
        m_SetClassPanel.SetWeekText("1주차");

        m_SetClassPanel._2WeekPanel.SetActive(false);
        m_SetClassPanel._1WeekPanel.SetActive(true);
        m_SetClassPanel.SetDownButton(false);
        m_SetClassPanel.SetUpButton(false);

        m_IsChangeWeekend = false;

        for (int i = 0; i < 6; i++)
        {
            m_ClassMoney[i] = 0;
            m_ClassHealth[i] = 0;
        }

        // 총 수업료
        m_SetClassPanel.TotalMoney(Color.white, "0");
        //m_SetClassPanel.TotalHealth("0");
        m_SaveData.m_SelectClassDataSave = null;
        m_SaveData.m_SelectProfessorDataSave = null;
        _ClickProfessorData = null;
        _ClickClassData = null;

        m_WeekClassIndex = 0;
        m_WeekProfessorIndex = 0;
        m_SaveDataIndex = 0;
        m_ClassStack = 0;
    }

    // 현재 수업의 스탯이 다 차있는게 아니라면 0이 아닌 3가지 스탯만 띄워줘야한다.
    private void FindStat(List<Class> _nowClass, int _index)
    {
        int _tempindex = UnSelectedClassDataStatIndex(_nowClass, _index);
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

        for (int j = 0; j < _tempindex;)
        {
            if (_nowClass[_index].m_Insight > 0 || _nowClass[_index].m_Insight < 0)
            {
                double _insight = Math.Round(_nowClass[_index].m_Insight * _increase);
                m_StatNum[j] = (int)_insight;
                m_StatName[j] = "통찰";
                j++;
            }

            if (_nowClass[_index].m_Concentration > 0 || _nowClass[_index].m_Concentration < 0)
            {
                double _concentration = Math.Round(_nowClass[_index].m_Concentration * _increase);
                m_StatNum[j] = (int)_concentration;

                m_StatName[j] = "집중";
                j++;
            }

            if (_nowClass[_index].m_Sense > 0 || _nowClass[_index].m_Sense < 0)
            {
                double _sense = Math.Round(_nowClass[_index].m_Sense * _increase);
                m_StatNum[j] = (int)_sense;
                m_StatName[j] = "감각";
                j++;
            }

            if (_nowClass[_index].m_Technique > 0 || _nowClass[_index].m_Technique < 0)
            {
                double _technique = Math.Round(_nowClass[_index].m_Technique * _increase);
                m_StatNum[j] = (int)_technique;
                m_StatName[j] = "기술";
                j++;
            }

            if (_nowClass[_index].m_Wit > 0 || _nowClass[_index].m_Wit < 0)
            {
                double _wit = Math.Round(_nowClass[_index].m_Wit * _increase);
                m_StatNum[j] = (int)_wit;
                m_StatName[j] = "재치";
                j++;
            }
        }
    }

    private void FindClickClassStat(Class _nowClass)
    {
        int _tempindex = UnSelectedClickClassDataStatIndex(_nowClass);
        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, true);

        for (int j = 0; j < _tempindex;)
        {
            if (_nowClass.m_Insight > 0 || _nowClass.m_Insight < 0)
            {
                double _insight = Math.Round(_nowClass.m_Insight * _increase);
                m_StatNum[j] = (int)_insight;
                m_StatName[j] = "통찰";
                j++;
            }

            if (_nowClass.m_Concentration > 0 || _nowClass.m_Concentration < 0)
            {
                double _concentration = Math.Round(_nowClass.m_Concentration * _increase);
                m_StatNum[j] = (int)_concentration;
                m_StatName[j] = "집중";
                j++;
            }

            if (_nowClass.m_Sense > 0 || _nowClass.m_Sense < 0)
            {
                double _sense = Math.Round(_nowClass.m_Sense * _increase);
                m_StatNum[j] = (int)_sense;
                m_StatName[j] = "감각";
                j++;
            }

            if (_nowClass.m_Technique > 0 || _nowClass.m_Technique < 0)
            {
                double _technique = Math.Round(_nowClass.m_Technique * _increase);
                m_StatNum[j] = (int)_technique;
                m_StatName[j] = "기술";
                j++;
            }

            if (_nowClass.m_Wit > 0 || _nowClass.m_Wit < 0)
            {
                double _wit = Math.Round(_nowClass.m_Wit * _increase);
                m_StatNum[j] = (int)_wit;
                m_StatName[j] = "재치";
                j++;
            }
        }
    }

    // 뒤로가기를 눌렀을 때 옆에 뜨는 정보들도 같이 없애주기 위한 코드
    private void RestPanel(GameObject _panel, int _index)
    {
        _panel.transform.GetChild(_index).gameObject.SetActive(false);
        _panel.transform.GetChild(_index).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        _panel.transform.GetChild(_index).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    }

    // 수업을 선택했을 때 바로바로 총 금액이 바뀔 수 있게 해주는 함수
    private void SaveMoneySelectPaenl(GameObject _Weekpanel, int _2WeekPanelIndex)
    {
        m_TotalMoney = 0;

        for (int i = 0; i < _Weekpanel.transform.childCount - 1; i++)
        {
            if (_Weekpanel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                string _tempGameDesignerMoney = _Weekpanel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text;
                _tempGameDesignerMoney = _tempGameDesignerMoney.Replace(",", "");
                m_ClassMoney[i + _2WeekPanelIndex] = int.Parse(_tempGameDesignerMoney);
            }
        }
    }

    private void SaveHealthSelectPanel(GameObject _weekPanel, int _2weekPanelIndex, int health)
    {
        m_TotalHealth = 0;

        for (int i = 0; i < _weekPanel.transform.childCount - 1; i++)
        {
            if (_weekPanel.transform.GetChild(i + 1).gameObject.activeSelf)
            {
                m_ClassHealth[i + _2weekPanelIndex] = health;
            }
        }
    }

    private void InitAllInfoStat()
    {
        // 체력과 돈
        m_SetClassPanel.SetClassUseHealth("");

        // 수업이름
        m_SetClassPanel.SetClassName("");

        // 교사이름
        m_SetClassPanel.SetProfessorInfo("", null);

        // 스탯들
        for (int i = 0; i < 3; i++)
        {
            if (m_SetClassPanel.AllInfoPanelClassAllStat.activeSelf)
            {
                m_SetClassPanel.AllInfoPanelClassAllStat.SetActive(false);
                m_SetClassPanel.AllInfoPanelClassAllStatText.text = "";
                return;
            }

            if (m_SetClassPanel.AllInfoPanelClassStat[i].activeSelf == true)
            {
                m_SetClassPanel.AllInfoPanelClassStat[i].SetActive(false);
                m_SetClassPanel.AllInfoPanelClassStatText[i].text = "";
            }
        }
    }

    // 수업 클릭했을 때 클릭한 수업의 정보를 AllInfoPanel에 띄워주기 위한 함수
    private void ClickClass()
    {
        #region _버튼을 클릭했을 때 체크 스프라이트 켜주기

        if (_prevClass != null)
        {
            _prevClass.GetComponent<ClassPrefab>().CheckImage.SetActive(false);

            InitStatArr();
            FindClickClassStat(_prevClickClassData);

            int StatCount = 0;

            for (int j = 0; j < 5; j++)
            {
                if (m_StatNum[j] != 0 && m_StatName[j] != "")
                {
                    StatCount += 1;
                }
            }

            SetClassStats(StatCount, _prevClass);
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        _prevClass = _currentObj;
        _currentObj.GetComponent<ClassPrefab>().CheckImage.SetActive(true);
        #endregion

        m_SelectClass.classData.TryGetValue(_currentObj.name, out _ClickClassData);

        int _compareMoney = int.Parse(m_InGameUICurrentMoney.text.Replace(",", ""));
        int _compareHealth = _ClickClassData.m_Health;

        m_SetClassPanel.SetClassName(_ClickClassData.m_ClassName);
        m_SetClassPanel.SetClassUseHealth(_ClickClassData.m_Health.ToString());

        float _increase = SetIncreaseClassFeeOrStat(PlayerInfo.Instance.CurrentRank, false);

        double _money = Math.Round(_ClickClassData.m_Money * _increase);

        // 현재 내가 소지하고 있는 돈보다 수업료가 더 비싸면 경고 띄워주기
        if (_compareMoney < _ClickClassData.m_Money)
        {

            m_IsMoneyOK = false;
            m_SetClassPanel.TotalMoney(Color.red, string.Format("{0:#,0}", _money));
        }
        else
        {
            m_IsMoneyOK = true;
            m_SetClassPanel.TotalMoney(Color.white, string.Format("{0:#,0}", _money));
        }

        _prevClickClassData = _ClickClassData;

        if (_ClickProfessorData != null)
        {
            if (_compareHealth > _ClickProfessorData.m_ProfessorHealth)
            {
                m_IsHealthOK = false;
                m_SetClassPanel.SetProfessorHealth(Color.red);
            }
            else
            {
                m_IsHealthOK = true;
                m_SetClassPanel.SetProfessorHealth(Color.white);
            }

            InitStatArr();
            FindClickClassStat(_ClickClassData);
            int StatCount = 0;

            for (int j = 0; j < 5; j++)
            {
                if (m_StatNum[j] != 0 && m_StatName[j] != "")
                {
                    StatCount += 1;
                }
            }

            SetClassStatsToProfessorPower(_ClickProfessorData, StatCount, _currentObj);
        }

        if (m_SetClassPanel.WeekText.text == "1주차")
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                m_SetClassPanel.Set1WeekGameDesignerPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                m_SetClassPanel.Set1WeekArtPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                m_SetClassPanel.Set1WeekProgrammingPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                {
                    m_SetClassPanel.Set1WeekGameDesignerPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                {
                    m_SetClassPanel.Set1WeekArtPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                {
                    m_SetClassPanel.Set1WeekProgrammingPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
            }

            m_SaveData.m_Week = "1";
            SaveMoneySelectPaenl(m_SetClassPanel._1WeekPanel, 0);
            SaveHealthSelectPanel(m_SetClassPanel._1WeekPanel, 0, _ClickClassData.m_Health);
        }
        else
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                m_SetClassPanel._1WeekPanel.SetActive(false);
                m_SetClassPanel._2WeekPanel.SetActive(true);
                m_SetClassPanel.SetUpButton(true);
                m_SetClassPanel.Set2WeekGameDesignerPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                m_SetClassPanel.Set2WeekArtPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                m_SetClassPanel.Set2WeekProgrammingPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.GameDesigner])
                {
                    m_SetClassPanel.Set2WeekGameDesignerPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Art])
                {
                    m_SetClassPanel.Set2WeekArtPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
                else if (m_SetClassPanel.PartImage.sprite == m_ClassPanelPartImage[(int)StudentType.Programming])
                {
                    m_SetClassPanel.Set2WeekProgrammingPanel(true, _ClickClassData.m_ClassName, string.Format("{0:#,0}", _ClickClassData.m_Money));
                }
            }

            m_SaveData.m_Week = "2";
            SaveMoneySelectPaenl(m_SetClassPanel._2WeekPanel, 3);
            SaveHealthSelectPanel(m_SetClassPanel._2WeekPanel, 3, _ClickClassData.m_Health);
        }

        for (int i = 0; i < m_ClassMoney.Length; i++)
        {
            m_TotalMoney += m_ClassMoney[i];
        }

        for (int i = 0; i < m_ClassHealth.Length; i++)
        {
            m_TotalHealth += m_ClassHealth[i];
        }

        double increaseMoney = Math.Round(m_TotalMoney * _increase);
        m_TotalMoney = (int)increaseMoney;

        if (m_TotalMoney > m_CurrentMoney)
        {
            m_SetClassPanel.TotalMoney(Color.red, string.Format("{0:#,0}", m_TotalMoney));
        }
        else
        {
            m_SetClassPanel.TotalMoney(Color.white, string.Format("{0:#,0}", m_TotalMoney));
        }

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 8)
        {
            m_SetClassPanel.ClassScrollView.vertical = true;
            m_Unmask.fitTarget = m_SetClassPanel.CompleteButton.gameObject.GetComponent<RectTransform>();
            m_SetClassPanel.CompleteButton.enabled = false;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 300, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }

        m_SaveData.m_SelectClassDataSave = _ClickClassData;
    }

    private void ClickProfessor()
    {
        #region _버튼을 클릭했을 때 하이라이트가 남아있게 하기

        if (_prevProfessor != null)
        {
            _prevProfessor.GetComponent<ProfessorPrefab>().CheckImage.SetActive(false);
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        _prevProfessor = _currentObj;

        _currentObj.GetComponent<ProfessorPrefab>().CheckImage.SetActive(true);

        #endregion

        Professor.Instance.SelectProfessor.professorData.TryGetValue(_currentObj.name, out _ClickProfessorData);

        m_SetClassPanel.SetProfessorInfo(_ClickProfessorData.m_ProfessorName, _ClickProfessorData.m_TeacherProfileImg);

        if (_ClickClassData != null)
        {
            int _compareHealth = _ClickClassData.m_Health;

            if (_compareHealth > _ClickProfessorData.m_ProfessorHealth)
            {
                m_IsHealthOK = false;
                m_SetClassPanel.SetProfessorHealth(Color.red);
            }
            else
            {
                m_IsHealthOK = true;
                m_SetClassPanel.SetProfessorHealth(Color.white);
            }

            InitStatArr();
            FindClickClassStat(_ClickClassData);

            int StatCount = 0;

            for (int j = 0; j < 5; j++)
            {
                if (m_StatNum[j] != 0 && m_StatName[j] != "")
                {
                    StatCount += 1;
                }
            }

            SetClassStatsToProfessorPower(_ClickProfessorData, StatCount, _prevClass);

            // 내가 선택한 교수가 있다면 그 버튼을 선택한 상태로 만들어준다.
            //for (int i = 0; i < m_SetClassPanel.ClassPrefabParent.childCount; i++)
            //{
            //    if (m_SetClassPanel.ClassPrefabParent.GetChild(i).name == _ClickClassData.m_ClassName)
            //    {
            //        m_SetClassPanel.ClassPrefabParent.GetChild(i).GetComponent<ClassPrefab>().CheckImage.SetActive(true);
            //        break;
            //    }
            //}
        }

        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 5)
        {
            m_Unmask.fitTarget = m_SetClassPanel.ClassListRect;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-250, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
            m_SetClassPanel.ProfessorScrollView.vertical = true;
            m_SetClassPanel.ClassScrollView.vertical = false;
            m_NextButton.gameObject.SetActive(true);
        }

        m_SaveData.m_SelectProfessorDataSave = _ClickProfessorData;
    }

    // 뒤로가기 버튼을 눌렀을 때 새로 수업과 교수의 데이터를 만들어주기 위해 이전의 정보들을 지워준다.
    private void RemovalBeforData(List<SaveSelectClassInfoData> _data, int _removeIndex)
    {
        SaveSelectClassInfoData _ClearData = new SaveSelectClassInfoData();

        if (m_SetClassPanel.ClassPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectClassObject();
        }

        if (m_SetClassPanel.ProfessorPrefabParent.childCount > 0)
        {
            m_SetClassPanel.DestroySelectPanelProfessorObject();
        }

        InitStatArr();

        if (m_WeekClassIndex == 1 && m_WeekProfessorIndex == 1 && m_IsChangeWeekend == false)
        {
            m_IsChangeWeekend = true;
            m_SetClassPanel.SetWeekText("2주차");
            m_SetClassPanel._2WeekPanel.SetActive(true);
            m_SaveDataIndex++;
        }

        m_SaveData.m_SelectClassDataSave = null;
        m_SaveData.m_SelectProfessorDataSave = null;
        _ClickProfessorData = null;
        _ClickClassData = null;

        _data.Add(_ClearData);
        ChangeListIndex(_data, _removeIndex, _ClearData);
        _data.RemoveAt(2);
    }

    // 뒤로가기 버튼을 눌렀을 때 내가 이전에 선택했던 정보를 잠시 저장해두는 방식
    private void BackSaveData(List<SaveSelectClassInfoData> _data, int _index)
    {
        m_TempSaveData.m_SelectClassDataSave = _data[_index].m_SelectClassDataSave;
        m_TempSaveData.m_SelectProfessorDataSave = _data[_index].m_SelectProfessorDataSave;
        m_TempSaveData.m_Week = _data[_index].m_Week;
    }

    /// TODO : 내가 1주차 패널을 보고있을 때 2주차 수업선택에서 뒤로가기 버튼을 누르면 바로 2주차 패널 보여주기
    // 내가 현재 있던 곳에서 이전으로 돌려줘야한다.
    private void CheckClassToBackButton()
    {
        switch (m_ClassStack)
        {
            case 1: // 1주차 아트, 1주차 기획으로 셋팅해놔야함
            {
                ResetSelectPanel(m_GameDesignerData[0].m_SelectClassDataSave,
                    m_GameDesignerData[0].m_SelectProfessorDataSave);
                BackSaveData(m_GameDesignerData, 0);
                RemovalBeforData(m_GameDesignerData, 0);

                RestPanel(m_SetClassPanel._1WeekPanel, 2);

                MakeClass();
                MakeProfessor();

                // 위에서 다 비워준 뒤 다시 내가 이전에 선택했던 정보를 넣어준다.
                m_SaveData = m_TempSaveData;
            }
            break;

            case 2: // 1주차 플밍
            {
                ResetSelectPanel(m_ArtData[0].m_SelectClassDataSave, m_ArtData[0].m_SelectProfessorDataSave);
                BackSaveData(m_ArtData, 0);

                RemovalBeforData(m_ArtData, 0);
                RestPanel(m_SetClassPanel._1WeekPanel, 3);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
            break;

            case 3: // 2주차 기획
            {
                // 아래 6가지는 2주차에서 다시 1주차로 넘어가기 때문에 여기서만 해주는 것
                m_WeekClassIndex = 0;
                m_WeekProfessorIndex = 0;
                m_IsChangeWeekend = false;
                m_SetClassPanel.SetWeekText("1주차");
                m_SetClassPanel._1WeekPanel.SetActive(true);
                m_SetClassPanel._2WeekPanel.SetActive(false);
                m_SetClassPanel.SetUpButton(false);
                m_SaveDataIndex = 0;

                // 만약 1주차 패널을 보는중인데 2주차 기획에서 뒤로가기 버튼을 누르면 아래 화살표 버튼을 없애줘야한다.
                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel.SetDownButton(false);
                }

                ResetSelectPanel(m_ProgrammingData[0].m_SelectClassDataSave,
                    m_ProgrammingData[0].m_SelectProfessorDataSave);
                BackSaveData(m_ProgrammingData, 0);

                RemovalBeforData(m_ProgrammingData, 0);
                RestPanel(m_SetClassPanel._2WeekPanel, 1);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
            break;

            case 4: // 2주차 아트
            {
                ResetSelectPanel(m_GameDesignerData[1].m_SelectClassDataSave,
                    m_GameDesignerData[1].m_SelectProfessorDataSave);
                BackSaveData(m_GameDesignerData, 1);

                RemovalBeforData(m_GameDesignerData, 1);

                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel.SetDownButton(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                }

                RestPanel(m_SetClassPanel._2WeekPanel, 2);

                MakeClass();
                MakeProfessor();
                m_SetClassPanel.SetBackButtonActive(true);
                m_SaveData = m_TempSaveData;
            }
            break;

            case 5: // 2주차 플밍
            {
                ResetSelectPanel(m_ArtData[1].m_SelectClassDataSave, m_ArtData[1].m_SelectProfessorDataSave);
                BackSaveData(m_ArtData, 1);

                RemovalBeforData(m_ArtData, 1);

                if (m_SetClassPanel._1WeekPanel.activeSelf)
                {
                    m_SetClassPanel._1WeekPanel.SetActive(false);
                    m_SetClassPanel.SetDownButton(false);
                    m_SetClassPanel._2WeekPanel.SetActive(true);
                    m_SetClassPanel.SetUpButton(true);
                }

                RestPanel(m_SetClassPanel._2WeekPanel, 3);

                MakeOtherClass();
                MakeOtherProfessor();

                m_SaveData = m_TempSaveData;
            }
            break;
        }
    }

    // SelectPanel에 있는 1주차와 2주차의 정보들을 초기화 시켜주는 함수
    public void InitTotalInfo(GameObject _panel)
    {
        for (int i = 0; i < _panel.transform.childCount - 1; i++)
        {
            if (_panel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                _panel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                _panel.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
        }
    }

    // 뒤로가기 버튼을 눌렀을 때 실행할 함수
    public void ClickBackButton()
    {
        CheckClassToBackButton();

        m_SetClassPanel.m_ClassPrefab.anchoredPosition = new Vector2(7f, 0f);
        m_SetClassPanel.m_ProfessorPrefab.anchoredPosition = new Vector2(7f, 0f);

        if (m_ClassStack > 0)
        {
            m_ClassStack--;
        }
    }

    // 숨김 버튼을 눌렀을 때
    public void ClickHideButton()
    {
        //// 확인하는 화면이었다면 CheckPanel을 꺼준다.
        if (m_CheckClassPanel.gameObject.activeSelf == true)
        {
            m_CheckClassPanel.gameObject.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
        else if (m_SetClassPanel.gameObject.activeSelf == true)
        {
            m_SetClassPanel.gameObject.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
    }

    // 수업열기를 눌렀을 때 수업 선택중이었다면 수업 선택창으로, 수업 확인중이었다면 확인창으로, 수정중이었다면 수정하는 창 열어주기
    public void OpenClassButton()
    {
        if (m_ArtData[1].m_SelectClassDataSave != null && m_GameDesignerData[1].m_SelectClassDataSave != null &&
            m_ProgrammingData[1].m_SelectClassDataSave != null
            && m_SetClassPanel.ModifiyButton.gameObject.activeSelf == true)
        {
            m_SetClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
            m_SetClassPanel.SetModifiyButton(true);
        }
        else if (m_ArtData[1].m_SelectClassDataSave != null && m_GameDesignerData[1].m_SelectClassDataSave != null &&
                 m_ProgrammingData[1].m_SelectClassDataSave != null)
        {
            m_CheckClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
        else
        {
            m_SetClassPanel.gameObject.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
    }

    public float SetIncreaseClassFeeOrStat(Rank _myAcademyRank, bool _isStat)
    {
        if (!_isStat)
        {
            foreach (IncreaseFee list in m_ClassFeeList)
            {
                if (list.MyAcademyRank == _myAcademyRank)
                {
                    return list.RisingFee;
                }
            }
        }
        else
        {
            foreach (IncreaseFee list in m_ClassFeeList)
            {
                if (list.MyAcademyRank == _myAcademyRank)
                {
                    return list.RisingStat;
                }
            }
        }

        return 0;
    }

    private void ContinueTutorial()
    {
        if (PlayerInfo.Instance.IsFirstClassSetting && m_TutorialCount == 13)
        {
            Time.timeScale = 0;
            m_PDAlarm.SetActive(true);
            m_Unmask.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(true);
            m_BlackScreen.SetActive(false);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().ClassTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }
}