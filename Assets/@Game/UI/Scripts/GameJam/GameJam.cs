using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct SaveGameJamData
{
    public Student m_StudentData;
    public GameJamInfo m_GameJamInfoData;
    public GameJamData m_GameJamData;
}

struct GenreValuePair
{
    public GenreStat genre;
    public int value;
}

// �������� �����ϰ� ���� �����ؾ� �� ������
public struct GameJamData
{
    public double m_Score;
    public string m_Genre;
    public string m_Rank;
    public string m_GameName;
    public int m_Awareness;
    public int m_PracticalTalent;
    public int m_Management;
    public int m_MakeYear;
    public int m_MakeMonth;
    public double m_Funny;
    public double m_Perfection;
    public double m_Graphic;
    public double m_StudnetGenre;
    public double m_TotalGenreScore;
    public List<Student> m_GM;
    public List<Student> m_Art;
    public List<Student> m_Programming;
}

/// <summary>
/// �������� ������� �� ���� �������� �����߳� � �л����� �����뿡 ������ �� �ϴ� 
/// �������� ������ִ� Ŭ����
/// 
/// 2023.03.06 Ocean
/// </summary>
public class GameJam : MonoBehaviour
{
    public delegate void ActivateGameShowButtonEventHandler(bool activate);
    public static event ActivateGameShowButtonEventHandler OnActivateButtonHandler;

    public delegate void DataChangedEventHandler();
    public static event DataChangedEventHandler DataChangedEvent;

    public delegate void SkillConditionDataChangedEventHandler();
    public static event SkillConditionDataChangedEventHandler SkillConditionDataChangedEvent;

    public static string[] GenreNameList = new string[] { "�׼�", "�ùķ��̼�", "��庥��", "����", "RPG", "����", "����", "������" };
    public static string[] AbilityNameList = new string[] { "����", "����", "����", "���", "��ġ" };

    public int[] RiseGMStatList = new int[5];                                               // ��޿� ���� ��Ʈ�� ��� ����
    public int[] RiseArtStatList = new int[5];                                              // 
    public int[] RiseProgrammingStatList = new int[5];                                      //

    public int[] GMRequirementStatList = new int[5];
    public int[] ArtRequirementStatList = new int[5];
    public int[] ProgrammingRequirementStatList = new int[5];

    private List<GameJamInfo> m_DummyGameJamList = new List<GameJamInfo>();                 // ���������͸� ����ִ� ����Ʈ �߿��� ���ǿ� �ش��ϴ� ������ ��� ����ִ� ����Ʈ
    private List<GameJamInfo> m_FixedGameJam = new List<GameJamInfo>();
    private List<GameJamInfo> m_RandomGameJam = new List<GameJamInfo>();

    private Dictionary<string, int> m_GameJamEntryCount = new Dictionary<string, int>();    // �� �����뿡 �� ���� �����ߴ��� 
    private Dictionary<string, int> m_NeedGMStat = new Dictionary<string, int>();           // �ʼ� ������ �̸��� ���� �����ص־��Ѵ�.
    private Dictionary<string, int> m_NeedArtStat = new Dictionary<string, int>();
    private Dictionary<string, int> m_NeedProgrammingStat = new Dictionary<string, int>();

    private SaveGameJamData m_SaveGameJamData = new SaveGameJamData();                      // �������� ���� �� ������ �����͵�

    private List<SaveGameJamData> m_GMStudentData = new List<SaveGameJamData>();            // ���õ� �л����� �����͵��� ���� ����Ʈ
    private List<SaveGameJamData> m_ArtStudentData = new List<SaveGameJamData>();
    private List<SaveGameJamData> m_ProgrammingStudentData = new List<SaveGameJamData>();
    private List<SaveGameJamData> m_ToBeRunning = new List<SaveGameJamData>();
    private List<GameDifficulty> m_GameJamDifficultiesList = new List<GameDifficulty>();    // ������ ���̵��� �Ǻ����ֱ� ���� ������ �־� �� ����Ʈ
    private List<GameJamReward> m_GameJamRewardList = new List<GameJamReward>();

    public static Dictionary<int, List<SaveGameJamData>> m_GameJamHistory = new Dictionary<int, List<SaveGameJamData>>();       // �������� �ѹ� ������ �Ǹ� ���⿡ ����.                      // �ѹ� �˶��� �� ���� ������ �����ÿ� �˶��� �ȶ��.
    private Queue<Action> eventQueue = new Queue<Action>();                                                                     // �������� ����ǰ� ���� ���Ӽ ������ �غ� ������Ѵ�. timescale�� 0�� �� �̺�Ʈ�� �����ϴ� �ȵż� Update������ timescale�� 0 ���� Ŭ �� ��������ִ°ɷ� ����
    private Queue<Coroutine> m_ToRunningGameJam = new Queue<Coroutine>();

    [SerializeField] private Sprite m_NotEntry;
    [SerializeField] private Sprite m_EntryComplete;
    [SerializeField] private Sprite m_Entry;
    [SerializeField] private Sprite m_SatisfyStat;
    [SerializeField] private Sprite m_DissatisfactionStat;
    [SerializeField] private Sprite m_UpArrow;
    [SerializeField] private Sprite m_DownArrow;
    [SerializeField] private Sprite[] m_RequirementStats;
    [SerializeField] private Sprite[] m_GenreSprite;

    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_GameJamButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;


    private Sprite m_NoneArrow;
    private bool m_IsGameJamEventCreation;                                                  // ������ �����ϸ� �������� �ѹ��� ���� �� �ְ� ���ִ� bool��
    private bool m_IsGameJamStart;
    private int m_Year;
    private int m_Month;
    private int m_Week;
    private string m_PartName;                                                              // �����뿡�� �л����� ������ �� �����Ʈ �л����� Ȯ�����ִ� ����, ��� �������� ������ �ٽ� ��ȹ���� �ٲ��ֱ�
    private string _warningMessage;
    private int m_Requirement1 = 0;
    private int m_Requirement2 = 0;
    private int[] m_Requirement1List = new int[3];                                          // ������ �л����� ������ �ִ� �ʼ� ������ ����
    private int[] m_Requirement2List = new int[3];
    private int[] m_RequirementStatList1 = new int[3];
    private int[] m_RequirementStatList2 = new int[3];

    private float m_TimerText;
    private int m_MonthLimit;                                                               // �Ѵ޿� 2���ۿ� ������ �� ������
    private int m_EnteryCount;                                                              // �Ȱ��� �����뿡 �� �� ���� �ߴ��� 
    private double Funny;
    private double Graphic;
    private double Perfection;
    private double _genreBonusScore;
    private double m_GenreScore;
    private double m_TotalGenreScore;
    private string _rank;                                                                   // �̹� �����뿡�� ���� ��ũ
    private double _average;                                                                // �̹� ������ ��� ����
    private string _genreName;                                                              // ���� ������ �帣�� �������ֱ� ���� ��� �־�δ� ����
    private string _gameJameName;                                                           // ���̸���Ʈ�� �ִ� ��� �� ������ �����ߴ��� �����صδ� ����
    private int _selectGameWeek;
    private string _rankButtonName;                                                         // ������ ���� ������ ��ũ ��ư�� ������ ��ư�� �̸��� �־��ִ� ����
    private string _genreButtonName;                                                        // ������ ���� ������ �帣 ��ư�� ������ ��ư�� �̸��� �־��ִ� ����

    private int m_TutorialCount;
    private int m_ScriptCount;
    private int m_DifficultyMiddle;
    private int m_DifficultyHigh;

    public GameObject m_SliderBarArlam;
    public GameObject m_GameJamArlam;
    public GameObject m_SecondContentGameJamArlam;
    public GameObject m_GameJamPrefab;
    public GameObject m_StudentPrefab;
    public GameObject m_GameJamListPrefab;
    public GameJamSelect_Panel m_GameJamPanel;
    public GameJamResult m_GameJamResultPanel;
    public FinalGameJamResult m_FinalGameJamResultPanel;
    public GameJamList m_GameJamListPanel;
    public GameJamTimer m_Timer;                                                            // �������� ���� �� �� ������ Ÿ�̸�
    public SlideMenuPanel m_Slider;
    public PopUpUI m_PopUpResultPanel;
    public PopOffUI m_PopOffResultPanel;

    public PopUpUI m_OnNoticePanel;
    public PopOffUI m_OffNoticePanel;

    private void Start()
    {
        m_IsGameJamEventCreation = false;
        m_NoneArrow = null;
        m_PartName = "��ȹ";
        m_MonthLimit = 2;
        m_GameJamHistory.Clear();
        InitDifficulyList();
        InitRewardList();
        ClassifyGameJamData();

        m_TutorialCount = 0;
        m_ScriptCount = 0;
        m_DifficultyMiddle = 10;
        m_DifficultyHigh = 20;
    }

    private void Update()
    {
        if (m_Month != GameTime.Instance.FlowTime.NowMonth)
        {
            m_IsGameJamEventCreation = false;
            m_MonthLimit = 2;
            m_DummyGameJamList.Clear();
            m_GameJamPanel.DestroyGameJamListObj();
        }

        if (eventQueue.Count > 0 && Time.timeScale >= 1f)
        {
            eventQueue.Dequeue()?.Invoke();
            m_IsGameJamStart = false;
        }

        // �� �ָ��� ������ �׸� �ش��ϴ� ���ǿ� �����ϴ°� �ִ��� Ȯ���Ѵ�.
        if (m_Week != GameTime.Instance.FlowTime.NowWeek)
        {
            m_Year = GameTime.Instance.FlowTime.NowYear;
            m_Month = GameTime.Instance.FlowTime.NowMonth;
            m_Week = GameTime.Instance.FlowTime.NowWeek;

            if (m_ToBeRunning.Count != 0 && !m_IsGameJamStart)
            {
                for (int i = 0; i < m_ToBeRunning.Count; i++)
                {
                    if (m_ToBeRunning[i].m_GameJamInfoData.m_GjamMonth == m_Month && m_ToBeRunning[i].m_GameJamInfoData.m_GjamWeek == m_Week)
                    {
                        m_SaveGameJamData = m_ToBeRunning[i];
                        //for (int j = 0; j < m_ToBeRunning.Count; j++)
                        //{
                        //    if (m_ToBeRunning[j].m_GameJamInfoData.m_GjamWeek == m_Week)
                        //    {
                        //    }
                        //}

                        if (m_SaveGameJamData.m_GameJamInfoData.m_GjamWeek == 1 || m_SaveGameJamData.m_GameJamInfoData.m_GjamWeek == 2)
                        {
                            m_SaveGameJamData.m_GameJamInfoData.m_GjamTime = 10;

                            StartCoroutine(CheckClassStart(9f));
                            m_IsGameJamStart = true;

                        }
                        else
                        {
                            m_SaveGameJamData.m_GameJamInfoData.m_GjamTime = 100;
                            StartCoroutine(CheckClassStart(90f));
                            m_IsGameJamStart = true;
                        }
                    }
                }
            }

        }

        if (!m_IsGameJamEventCreation && PlayerInfo.Instance.IsFirstClassEnd)
        {
            #region _���� ����� ������ ����Ǯ ����� ���
            //if (m_GameJamButton.GetComponent<Button>().interactable == false)
            //{
            //    m_GameJamButton.GetComponent<Button>().interactable = true;
            //}

            //m_Year = GameTime.Instance.FlowTime.NowYear;
            //m_Month = GameTime.Instance.FlowTime.NowMonth;
            //m_Week = GameTime.Instance.FlowTime.NowWeek;

            //int difficuly = CheckDifficulties(PlayerInfo.Instance.m_CurrentRank);

            //for (int i = 0; i < m_GameJamDataList.Count; i++)
            //{
            //    // -2�� ���� ¦���⵵�� ���� ������ �˶� �߰� �ϱ�
            //    if (m_GameJamDataList[i].m_GjamYear == -2 &&
            //        m_Year % 2 == 0 &&
            //        m_GameJamDataList[i].m_GjamMonth - 1 == m_Month && difficuly == m_GameJamDataList[i].m_GjamAI_ID)
            //    {
            //        m_DummyGameJamList.Add(m_GameJamDataList[i]);

            //        if (!m_GameJamHistory.ContainsKey(m_GameJamDataList[i].m_GjamID))
            //        {
            //            //m_GameJamHistory.Add(m_GameJamDataList[i].m_GjamID, m_GameJamDataList[i].m_GjamName);
            //            SetActiveAlram(true);
            //        }
            //        m_IsGameJamEventCreation = true;
            //    }
            //    // -1�̸� Ȧ���⵵�� ����
            //    else if (m_GameJamDataList[i].m_GjamYear == -1 &&
            //        m_Year % 2 != 0 &&
            //        m_GameJamDataList[i].m_GjamMonth - 1 == m_Month && difficuly == m_GameJamDataList[i].m_GjamAI_ID)
            //    {
            //        m_DummyGameJamList.Add(m_GameJamDataList[i]);

            //        if (!m_GameJamHistory.ContainsKey(m_GameJamDataList[i].m_GjamID))
            //        {
            //            //m_GameJamHistory.Add(m_GameJamDataList[i].m_GjamID, m_GameJamDataList[i].m_GjamName);
            //            SetActiveAlram(true);
            //        }
            //        m_IsGameJamEventCreation = true;
            //    }
            //    // 0�� �ų� �����ϴ°Ŵϱ� �ų� �ش��ϴ� �޿� �˶��ֱ�
            //    else if (m_GameJamDataList[i].m_GjamYear == 0 &&
            //        m_GameJamDataList[i].m_GjamMonth - 1 == m_Month && difficuly == m_GameJamDataList[i].m_GjamAI_ID)
            //    {
            //        m_DummyGameJamList.Add(m_GameJamDataList[i]);

            //        if (!m_GameJamHistory.ContainsKey(m_GameJamDataList[i].m_GjamID))
            //        {
            //            //m_GameJamHistory.Add(m_GameJamDataList[i].m_GjamID, m_GameJamDataList[i].m_GjamName);
            //            SetActiveAlram(true);
            //        }
            //        m_IsGameJamEventCreation = true;
            //    }
            //}
            #endregion

            if (m_GameJamButton.GetComponent<Button>().interactable == false)
            {
                m_GameJamButton.GetComponent<Button>().interactable = true;
            }

            m_Year = GameTime.Instance.FlowTime.NowYear;
            m_Month = GameTime.Instance.FlowTime.NowMonth;
            m_Week = GameTime.Instance.FlowTime.NowWeek;

            m_IsGameJamEventCreation = true;

            BringFixedGameJamData();
            BringRandomGameJamData();

            if (m_DummyGameJamList.Count != 0)
            {
                MakeGameJamList();
                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 0 && PlayerInfo.Instance.IsFirstAcademySetting)
                {
                    Time.timeScale = 0;

                    m_TutorialPanel.SetActive(true);
                    m_PDAlarm.SetActive(true);
                    m_Unmask.gameObject.SetActive(false);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
        }
#if UNITY_EDITOR
        if (PlayerInfo.Instance.IsFirstGameJam && Input.GetMouseButtonDown(0))
        {
            if (m_TutorialCount == 1)
            {
                m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 2)
            {
                m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 3)
            {
                m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 4)
            {
                if (m_FoldButton.GetComponent<PopUpUI>().isSlideMenuPanelOpend == false)
                {
                    m_FoldButton.GetComponent<PopUpUI>().AutoSlideMenuUI();
                }

                m_BlackScreen.SetActive(true);
                m_PDAlarm.SetActive(false);
                m_Unmask.gameObject.SetActive(true);
                m_Unmask.fitTarget = m_GameJamButton.GetComponent<RectTransform>();
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 6)
            {
                m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = false;
                m_Unmask.fitTarget = m_GameJamPanel.m_GameJamParent.GetChild(0).GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 130, 0);
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 8)
            {
                m_Unmask.fitTarget = m_GameJamPanel.m_SetStartButton.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 10)
            {
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 11)
            {
                m_Unmask.fitTarget = m_GameJamPanel.GenreRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 12)
            {
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 13)
            {
                m_Unmask.fitTarget = m_GameJamPanel.RewardAwarenessRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-600, 0, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 14)
            {
                m_Unmask.fitTarget = m_GameJamPanel.DateRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 15)
            {
                m_Unmask.fitTarget = m_GameJamPanel.HealthPassionRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 16)
            {
                m_Unmask.fitTarget = m_GameJamPanel.ExpectedGenreRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 17)
            {
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 18)
            {
                m_Unmask.fitTarget = m_GameJamPanel.ExpectedSuccessRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 19)
            {
                m_Unmask.fitTarget = m_GameJamPanel.PartInfoRect.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
            else if (m_TutorialCount == 20)
            {
                m_Unmask.fitTarget = m_GameJamPanel.GMButton.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 250, 0);
                m_TutorialCount++;
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
        {
            Touch touch = Input.GetTouch(0); 
            if (PlayerInfo.Instance.IsFirstGameJam && touch.phase == TouchPhase.Ended)
            {
                if (m_TutorialCount == 1)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 2)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 3)
                {
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 4)
                {
                    if (m_FoldButton.GetComponent<PopUpUI>().isSlideMenuPanelOpend == false)
                    {
                        m_FoldButton.GetComponent<PopUpUI>().AutoSlideMenuUI();
                    }

                    m_BlackScreen.SetActive(true);
                    m_PDAlarm.SetActive(false);
                    m_Unmask.gameObject.SetActive(true);
                    m_Unmask.fitTarget = m_GameJamButton.GetComponent<RectTransform>();
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 6)
                {
                    m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = false;
                    m_Unmask.fitTarget = m_GameJamPanel.m_GameJamParent.GetChild(0).GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 130, 0);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 8)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.m_SetStartButton.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 10)
                {
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 11)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.GenreRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 12)
                {
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 13)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.RewardAwarenessRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-600, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 14)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.DateRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 15)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.HealthPassionRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 16)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.ExpectedGenreRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 17)
                {
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 18)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.ExpectedSuccessRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 19)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.PartInfoRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
                else if (m_TutorialCount == 20)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.GMButton.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(true);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 250, 0);
                    m_TutorialCount++;
                }
            }
        }

#endif
    }

    private void InitDifficulyList()
    {
        // ��
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.SSS, 100));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.SS, 100));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.S, 100));

        // ��
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.A, 200));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.B, 200));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.C, 200));

        // ��
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.D, 300));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.E, 300));
        m_GameJamDifficultiesList.Add(new GameDifficulty(Rank.F, 300));
    }

    private void InitRewardList()
    {
        m_GameJamRewardList.Add(new GameJamReward(100, "AAA", 500000));
        m_GameJamRewardList.Add(new GameJamReward(100, "AA", 350000));
        m_GameJamRewardList.Add(new GameJamReward(100, "A", 250000));
        m_GameJamRewardList.Add(new GameJamReward(100, "B", 150000));
        m_GameJamRewardList.Add(new GameJamReward(100, "�̿ϼ�", 0));


        m_GameJamRewardList.Add(new GameJamReward(200, "AAA", 400000));
        m_GameJamRewardList.Add(new GameJamReward(200, "AA", 300000));
        m_GameJamRewardList.Add(new GameJamReward(200, "A", 200000));
        m_GameJamRewardList.Add(new GameJamReward(200, "B", 100000));
        m_GameJamRewardList.Add(new GameJamReward(200, "�̿ϼ�", 0));


        m_GameJamRewardList.Add(new GameJamReward(300, "AAA", 350000));
        m_GameJamRewardList.Add(new GameJamReward(300, "AA", 250000));
        m_GameJamRewardList.Add(new GameJamReward(300, "A", 150000));
        m_GameJamRewardList.Add(new GameJamReward(300, "B", 80000));
        m_GameJamRewardList.Add(new GameJamReward(300, "�̿ϼ�", 0));
    }

    // �ϴ� ������ ������ �����͸� �����ش�.
    private void ClassifyGameJamData()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalGameJamData.Count; i++)
        {
            if (AllOriginalJsonData.Instance.OriginalGameJamData[i].m_Fix)
            {
                m_FixedGameJam.Add(AllOriginalJsonData.Instance.OriginalGameJamData[i]);
            }
            else
            {
                m_RandomGameJam.Add(AllOriginalJsonData.Instance.OriginalGameJamData[i]);
            }
        }
    }

    // �����Ͱ� �ٲ� �� ��������� �̺�Ʈ���� ť�� �־��ش�. 
    public void EnqueueDataChangedEvent()
    {
        eventQueue.Enqueue(() => DataChangedEvent?.Invoke());
        eventQueue.Enqueue(() => SkillConditionDataChangedEvent?.Invoke());
    }

    // ���� ������ ������ Ǯ���� ��¥�� �´� �������� ������ �� ��ī���� ���̵��� ���� ������ �ٲ��ش�.
    private void BringFixedGameJamData()
    {
        int difficuly = CheckDifficulties(PlayerInfo.Instance.m_CurrentRank);

        for (int i = 0; i < m_FixedGameJam.Count; i++)
        {
            if (m_FixedGameJam[i].m_GjamYear == -2 &&
                    m_Year % 2 == 0 &&
                    m_FixedGameJam[i].m_GjamMonth - 1 == m_Month)
            {
                m_DummyGameJamList.Add(m_FixedGameJam[i]);

                if (!m_GameJamHistory.ContainsKey(m_FixedGameJam[i].m_GjamID))
                {
                    SetActiveAlram(true);
                }
            }
            // -1�̸� Ȧ���⵵�� ����
            else if (m_FixedGameJam[i].m_GjamYear == -1 &&
                m_Year % 2 != 0 &&
                m_FixedGameJam[i].m_GjamMonth - 1 == m_Month)
            {
                m_DummyGameJamList.Add(m_FixedGameJam[i]);

                if (!m_GameJamHistory.ContainsKey(m_FixedGameJam[i].m_GjamID))
                {
                    SetActiveAlram(true);
                }
            }
            // 0�� �ų� �����ϴ°Ŵϱ� �ų� �ش��ϴ� �޿� �˶��ֱ�
            else if (m_FixedGameJam[i].m_GjamYear == 0 &&
                m_FixedGameJam[i].m_GjamMonth - 1 == m_Month)
            {
                m_DummyGameJamList.Add(m_FixedGameJam[i]);

                if (!m_GameJamHistory.ContainsKey(m_FixedGameJam[i].m_GjamID))
                {
                    SetActiveAlram(true);
                }
            }
        }

        if (m_DummyGameJamList.Count == 0)
        {
            return;
        }

        switch (difficuly)
        {
            // ��
            case 100:
            {
                // �Ѱ��� ���̵��� ��, �� �ϳ��� �����ϰ�
                int _randomNum = UnityEngine.Random.Range(0, m_DummyGameJamList.Count);
                int _difficurly = _randomNum == 0 || _randomNum == 1 ? m_DifficultyMiddle : 0;

                Dictionary<string, int> m_GMNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatGM.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                m_DummyGameJamList[_randomNum].m_GjamNeedStatGM = m_GMNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 || _randomNum == 1 ? 200 : 300;
                // ������ �� ���� ������ �����ش�.
                for (int i = 0; i < 2; i++)
                {
                    if (i != _randomNum)
                    {
                        Dictionary<string, int> m_MyDifficurlyGMNeedStat = m_DummyGameJamList[i].m_GjamNeedStatGM.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));
                        Dictionary<string, int> m_MyDifficurlyArtNeedStat = m_DummyGameJamList[i].m_GjamNeedStatArt.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));
                        Dictionary<string, int> m_MyDifficurlyProgrammingNeedStat = m_DummyGameJamList[i].m_GjamNeedStatProgramming.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));

                        m_DummyGameJamList[i].m_GjamNeedStatGM = m_MyDifficurlyGMNeedStat;
                        m_DummyGameJamList[i].m_GjamNeedStatArt = m_MyDifficurlyArtNeedStat;
                        m_DummyGameJamList[i].m_GjamNeedStatProgramming = m_MyDifficurlyProgrammingNeedStat;
                        m_DummyGameJamList[_randomNum].m_GjamAI_ID = 100;
                    }
                }
            }
            break;

            // ��
            case 200:
            {
                int _listCount = m_DummyGameJamList.Count;

                for (int i = 0; i < 3; i++)
                {
                    int _randomNum = UnityEngine.Random.Range(0, _listCount);
                    int _difficurly = _randomNum == 0 ? m_DifficultyHigh : _randomNum == 1 ? m_DifficultyMiddle : 0;

                    Dictionary<string, int> m_GMNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatGM.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                    Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                    Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                    m_DummyGameJamList[_randomNum].m_GjamNeedStatGM = m_GMNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 ? 100 : _randomNum == 1 ? 200 : 300;
                    GameShow.Swap(m_DummyGameJamList, _randomNum, m_DummyGameJamList.Count - 1);
                    _listCount -= 1;
                }
            }
            break;

            // ��
            case 300:
            {
                int _randomNum = UnityEngine.Random.Range(0, m_DummyGameJamList.Count);
                int _difficurly = _randomNum == 0 || _randomNum == 1 ? m_DifficultyMiddle : m_DifficultyHigh;

                if (m_DummyGameJamList[_randomNum].m_GjamName == "�׼��谨" && m_Year == 1 && m_Month == 3)
                {
                    _randomNum = UnityEngine.Random.Range(1, m_DummyGameJamList.Count);
                    _difficurly = _randomNum == 1 || _randomNum == 2 ? m_DifficultyMiddle : m_DifficultyHigh;
                }

                Dictionary<string, int> m_GMNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatGM.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                m_DummyGameJamList[_randomNum].m_GjamNeedStatGM = m_GMNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 || _randomNum == 1 ? 200 : 300;
            }
            break;
        }
    }

    // ���� ������ ������ Ǯ���� 2���� �̾� ���̵��� �߻������� ���Ƿ� �������ش�.
    private void BringRandomGameJamData()
    {
        List<GameJamInfo> m_MonthRandomList = new List<GameJamInfo>();

        for (int i = 0; i < m_RandomGameJam.Count; i++)
        {
            if (m_RandomGameJam[i].m_GjamMonth - 1 == m_Month)
            {
                m_MonthRandomList.Add(m_RandomGameJam[i]);
            }
        }

        int _listCount = m_MonthRandomList.Count;
        int _weekend = 5;

        for (int i = 0; i < 2; i++)
        {
            int randomDifficuly = UnityEngine.Random.Range(0, _listCount);
            int randomWeekwnd = UnityEngine.Random.Range(1, _weekend);
            int difficuly = randomDifficuly == 0 ? m_DifficultyHigh : randomDifficuly == 1 ? m_DifficultyMiddle : 0;

            Dictionary<string, int> m_GMNeedStat = m_MonthRandomList[randomDifficuly].m_GjamNeedStatGM.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));
            Dictionary<string, int> m_ArtNeedStat = m_MonthRandomList[randomDifficuly].m_GjamNeedStatArt.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));
            Dictionary<string, int> m_ProgrammingNeedStat = m_MonthRandomList[randomDifficuly].m_GjamNeedStatProgramming.ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));

            m_MonthRandomList[randomDifficuly].m_GjamNeedStatGM = m_GMNeedStat;
            m_MonthRandomList[randomDifficuly].m_GjamNeedStatArt = m_ArtNeedStat;
            m_MonthRandomList[randomDifficuly].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
            m_MonthRandomList[randomDifficuly].m_GjamYear = m_Year;
            //m_RandomGameJam[randomDifficuly].m_GjamMonth = m_Month + 1;
            m_MonthRandomList[randomDifficuly].m_GjamWeek = randomWeekwnd;
            m_DummyGameJamList.Add(m_MonthRandomList[randomDifficuly]);

            if (!m_GameJamHistory.ContainsKey(m_MonthRandomList[randomDifficuly].m_GjamID))
            {
                SetActiveAlram(true);
            }

            GameShow.Swap(m_MonthRandomList, randomDifficuly, m_MonthRandomList.Count - 1);
            _listCount -= 1;
            _weekend -= 1;
        }
    }

    //�������� �ѹ��̶� �����ϸ� ���Ӽ� ��ư�� Ȱ��ȭ �����ش�.
    private void OnDataChanged()
    {
        bool activateGameShowButton = CheckGameJamData();

        if (OnActivateButtonHandler != null)
        {
            OnActivateButtonHandler(activateGameShowButton);
        }
    }

    // �� ��ī������ ��޿� ���� ���̵��� ��ȯ���ִ� �Լ�
    private int CheckDifficulties(Rank _myAcademyRank)
    {
        foreach (GameDifficulty gameDifficulty in m_GameJamDifficultiesList)
        {
            if (gameDifficulty.MyRank == _myAcademyRank)
            {
                return gameDifficulty.Difficulty;
            }
        }

        return 0;
    }

    // ������ �����Ͱ� �ִ��� ������ Ȯ���ϴ� �Լ�
    private bool CheckGameJamData()
    {
        if (m_GameJamHistory.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // �ڷΰ��� ��ư�� ������ �� �л����� �����ߴ� üũ ǥ�ø� �� �����ְ� ���� �������� ���ư��� �� �ٽ� �� �����ִ� ������ ������ ����ش�.
    public void ClickBackButton()
    {
        m_GameJamPanel.BackButton();

        FirstGameJamIfoToRecruitNoticeInfo();

        m_GameJamPanel.m_GMName.text = "";
        m_GameJamPanel.m_ArtName.text = "";
        m_GameJamPanel.m_ProgrammingName.text = "";
    }

    // Ȩ��ư�� ������ �� �˶�ǥ�ø� ���ֱ� ���� �Լ�
    public void SetActiveAlram(bool _isFirstAlram = false)
    {
        m_SliderBarArlam.SetActive(_isFirstAlram);
        m_GameJamArlam.SetActive(_isFirstAlram);
        m_SecondContentGameJamArlam.SetActive(_isFirstAlram);
    }

    // Ȩ��ư�� ������ ���� ȭ������ ���ٸ� ���� �����ϰ� �ִ� �����鵵 �ʱ�ȭ ��������Ѵ�.
    // �������� �� �ϰ� ���� ������ ���� �� �Լ� ����ϱ�
    public void ClickHomeButtonYes()
    {
        // ������ ó���� ��� �ؾ��� �� ����̴�..
        m_SaveGameJamData.m_GameJamData.m_GameName = "";
        m_SaveGameJamData.m_GameJamInfoData = null;
        m_SaveGameJamData.m_StudentData = null;

        m_SaveGameJamData.m_GameJamData.m_Awareness = 0;
        m_SaveGameJamData.m_GameJamData.m_Management = 0;
        m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 0;
        m_SaveGameJamData.m_GameJamData.m_GameName = "";

        SetActiveAlram(false);
    }

    // �ϴܿ� �� �а� ��ư�� ������ �� �а����� �л����� ����� �Լ�
    private void MakeStudent(List<Student> _list, StudentType _type)
    {
        m_GameJamPanel.DestroyStudentObj();

        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].m_StudentStat.m_StudentType == _type)
            {
                GameObject _studentInfo = Instantiate(m_StudentPrefab);
                m_GameJamPanel.MoveObj(_studentInfo, m_GameJamPanel.m_StudentInfoParent);
                _studentInfo.name = _list[i].m_StudentStat.m_StudentName;
                _studentInfo.GetComponent<Image>().sprite = _list[i].StudentProfileImg;
                _studentInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _list[i].m_StudentStat.m_NumberOfEntries.ToString();

                if (_list[i].m_StudentStat.m_NumberOfEntries < 1)
                {
                    _studentInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
                }

                FindCheckStudent(_list, i, _type, _studentInfo);
                _studentInfo.GetComponent<Button>().onClick.AddListener(ClickStudentButton);
            }
        }
    }

    private void FindStat(List<Student> _list, StudentType _type, string _name)
    {
        int _tempIndex = 0;

        for (int i = 0; i < AbilityNameList.Length; i++)
        {
            if (AbilityNameList[i] == _name)
            {
                _tempIndex = i;
                break;
            }
        }

        _list.Sort((x, y) =>
        {
            if (x.m_StudentStat.m_NumberOfEntries > y.m_StudentStat.m_NumberOfEntries) return -1;
            else if (x.m_StudentStat.m_NumberOfEntries < y.m_StudentStat.m_NumberOfEntries) return 1;
            else if (x.m_StudentStat.m_AbilityAmountList[_tempIndex] > y.m_StudentStat.m_AbilityAmountList[_tempIndex]) return -1;
            else if (x.m_StudentStat.m_AbilityAmountList[_tempIndex] < y.m_StudentStat.m_AbilityAmountList[_tempIndex]) return 1;
            else return 0;
        });

        MakeStudent(_list, _type);
    }

    // �л��� �ʼ� ������ ���� ū ������� �����ϴ� �Լ�. ���� �ʼ� ������ �� ����� �� ū ģ���� �������� �ϱ� ���� �񱳸� �Ѵ�.
    private void SortList(StudentType _type)
    {
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        Dictionary<string, int> _dict = null;
        if (_type == StudentType.GameDesigner) _dict = m_NeedGMStat;
        else if (_type == StudentType.Art) _dict = m_NeedArtStat;
        else if (_type == StudentType.Programming) _dict = m_NeedProgrammingStat;

        FindHigherOfTheTwoStat(_dict, _type, _studentList);
    }

    // �л��� �� �ش� ������ ���� ū ������� �����ϱ� ���� �Լ�
    private void FindHigherOfTheTwoStat(Dictionary<string, int> _dic, StudentType _type, List<Student> _list)
    {
        // �ʼ� ������ �� ���� ��
        if (_dic.Count > 1)
        {
            int _bigger = _dic.ElementAt(0).Value > _dic.ElementAt(1).Value ? 0 : 1;
            string _statName = _dic.ElementAt(_bigger).Key;
            FindStat(_list, _type, _statName);
        }
        else
        {
            string _statName = _dic.ElementAt(0).Key;

            FindStat(_list, _type, _statName);
        }
    }

    // ������ư�� ������ �� ó������� �ϴ� ��
    public void ClickEntryButton()
    {
        if (m_GameJamPanel.m_GetStartButtonText.text == "��û �Ұ�")
        {
            // ��� �޼����� ����ش�.
            _warningMessage = "���� �� �� �ִ� Ƚ���� �ʰ��Ͽ����ϴ�!\n�������� ���������.";
            m_GameJamPanel.SetActiveEntryCountWarningPanel(true, _warningMessage);
            StartCoroutine(SetEntryWarningPanel());
        }
        else if (m_GameJamPanel.m_GetStartButtonText.text == "���� ��û �Ϸ�")
        {
            // �ƹ��͵� �ȶ��.
        }
        else
        {
            if (m_ToBeRunning.Count != 0)
            {
                if (m_ToBeRunning[0].m_GameJamInfoData.m_GjamWeek == _selectGameWeek && m_ToBeRunning[0].m_GameJamInfoData.m_GjamMonth - 1 == m_Month)
                {
                    _warningMessage = "�̹� �ش� �ֿ� �����ϴ� �������� �ֽ��ϴ�!";

                    m_GameJamPanel.SetActiveEntryCountWarningPanel(true, _warningMessage);
                    StartCoroutine(SetEntryWarningPanel());
                }
                else
                {
                    SetActiveAlram(false);

                    for (int i = 0; i < m_DummyGameJamList.Count; i++)
                    {
                        if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                        {
                            m_SaveGameJamData.m_GameJamInfoData = m_DummyGameJamList[i];
                        }
                    }
                    m_GameJamPanel.SetRecruitNoticeActive(false);
                    m_GameJamPanel.SetSelectActive(true);
                    m_GameJamPanel.ResetSlider();
                    SortList(StudentType.GameDesigner);
                    m_GameJamPanel.SetActivePartSelectedCheckBox(true, false, false);
                    // �����ݾ׸�ŭ ���ֱ�

                    if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 9)
                    {
                        m_Unmask.fitTarget = m_GameJamPanel.SelectStudentRect.GetComponent<RectTransform>();
                        m_TutorialArrowImage.gameObject.SetActive(false);
                        m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                        m_TutorialTextImage.gameObject.SetActive(true);
                        m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(700, 0, 0);
                        m_ScriptCount++;
                        m_TutorialCount++;
                    }
                }
            }
            else
            {
                SetActiveAlram(false);

                for (int i = 0; i < m_DummyGameJamList.Count; i++)
                {
                    if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                    {
                        m_SaveGameJamData.m_GameJamInfoData = m_DummyGameJamList[i];
                    }
                }
                m_GameJamPanel.SetRecruitNoticeActive(false);
                m_GameJamPanel.SetSelectActive(true);
                m_GameJamPanel.ResetSlider();
                SortList(StudentType.GameDesigner);
                m_GameJamPanel.SetActivePartSelectedCheckBox(true, false, false);
                // �����ݾ׸�ŭ ���ֱ�

                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 9)
                {
                    m_Unmask.fitTarget = m_GameJamPanel.SelectStudentRect.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(700, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
        }
    }

    // ��Ʈ���� ������ �� �ִ� �л����� ����� �����ش�.
    public void ClickPartButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        m_PartName = _currentObj.name;

        switch (m_PartName)
        {
            case "GMButton":
            {
                SortList(StudentType.GameDesigner);
                m_GameJamPanel.SetActivePartSelectedCheckBox(true, false, false);
            }
            break;

            case "ArtButton":
            {
                SortList(StudentType.Art);
                m_GameJamPanel.SetActivePartSelectedCheckBox(false, true, false);
            }
            break;

            case "ProgrammingButton":
            {
                SortList(StudentType.Programming);
                m_GameJamPanel.SetActivePartSelectedCheckBox(false, false, true);
            }
            break;
        }

        if (PlayerInfo.Instance.IsFirstGameJam && (m_TutorialCount == 21 || m_TutorialCount == 23 || m_TutorialCount == 25))
        {
            m_Unmask.fitTarget = m_GameJamPanel.m_StudentInfoParent.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }
    }

    // ���� ������ �����ߴ� �л��̸� üũǥ�ø� ������Ѵ�.
    private void FindCheckStudent(List<Student> _list, int _index, StudentType _type, GameObject _student)
    {
        switch (_type)
        {
            case StudentType.GameDesigner:
            {
                if (_list[_index].m_StudentStat.m_StudentName == m_GameJamPanel.m_GMName.text)
                {
                    _student.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            break;
            case StudentType.Art:
            {
                if (_list[_index].m_StudentStat.m_StudentName == m_GameJamPanel.m_ArtName.text)
                {
                    _student.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            break;
            case StudentType.Programming:
            {
                if (_list[_index].m_StudentStat.m_StudentName == m_GameJamPanel.m_ProgrammingName.text)
                {
                    _student.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            break;
        }

    }

    private int FindStudent(StudentType _type)
    {
        int _sliderValue = 0;

        for (int i = 0; i < (int)StudentType.Count; i++)
        {
            if ((int)_type == i)
            {
                _sliderValue = m_Requirement1List[i] + m_Requirement2List[i];
                break;
            }
        }

        return _sliderValue;
    }
    // �л��� Ŭ������ �� �ش� �л��� ������ üũ �̹����� ����ֱ� ���� �Լ�
    public void ClickStudentButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        // ���� �����ִ� üũ �ڽ��� �� ���ش�.
        for (int i = 0; i < m_GameJamPanel.m_StudentInfoParent.childCount; i++)
        {
            m_GameJamPanel.m_StudentInfoParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_currentObj.name == _studentList[i].m_StudentStat.m_StudentName)
            {
                if (_studentList[i].m_StudentStat.m_NumberOfEntries >= 1)
                {
                    _currentObj.transform.GetChild(0).gameObject.SetActive(true);

                    if (_studentList[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {

                        if (m_GMStudentData != null)
                        {
                            m_GMStudentData.Clear();
                        }

                        m_SaveGameJamData.m_StudentData = _studentList[i];

                        m_GMStudentData.Add(m_SaveGameJamData);

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_GMNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.GMStudentPanel, true);

                        string _name = _studentList[i].m_StudentStat.m_StudentName;
                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedGMStat);
                        //m_GameJamPanel.SetRequirmentStatImage(StudentType.GameDesigner, GMRequirementStatList);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.GameDesigner, _name, _health, _passion,
                           _studentList[i].m_StudentStat.m_AbilityAmountList, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.GameDesigner);

                        m_GameJamPanel.ChangeSlider(StudentType.GameDesigner, _sliderValue);

                        int _needValue = 0;

                        if (m_NeedGMStat.Count > 1)
                        {
                            _needValue = m_NeedGMStat.ElementAt(0).Value + m_NeedGMStat.ElementAt(1).Value;

                        }
                        else
                        {
                            _needValue = m_NeedGMStat.ElementAt(0).Value;

                        }
                        m_GameJamPanel.ChangeSliderFillSprite(StudentType.GameDesigner, _sliderValue, _needValue);
                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_ArtNone, m_GameJamPanel.m_ProgrammingNone);
                    }
                    else if (_studentList[i].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        if (m_ArtStudentData != null)
                        {
                            m_ArtStudentData.Clear();
                        }

                        m_SaveGameJamData.m_StudentData = _studentList[i];

                        m_ArtStudentData.Add(m_SaveGameJamData);

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_ArtNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.ArtStudentPanel, true);

                        string _name = _studentList[i].m_StudentStat.m_StudentName;
                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedArtStat);
                        //m_GameJamPanel.SetRequirmentStatImage(StudentType.Art, ArtRequirementStatList);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.Art, _name, _health, _passion,
                           _studentList[i].m_StudentStat.m_AbilityAmountList, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.Art);

                        m_GameJamPanel.ChangeSlider(StudentType.Art, _sliderValue);

                        int _needValue = 0;

                        if (m_NeedArtStat.Count > 1)
                        {
                            _needValue = m_NeedArtStat.ElementAt(0).Value + m_NeedArtStat.ElementAt(1).Value;

                        }
                        else
                        {
                            _needValue = m_NeedArtStat.ElementAt(0).Value;

                        }
                        m_GameJamPanel.ChangeSliderFillSprite(StudentType.Art, _sliderValue, _needValue);
                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_GMNone, m_GameJamPanel.m_ProgrammingNone);
                    }
                    else if (_studentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
                    {
                        if (m_ProgrammingStudentData != null)
                        {
                            m_ProgrammingStudentData.Clear();
                        }

                        m_SaveGameJamData.m_StudentData = _studentList[i];

                        m_ProgrammingStudentData.Add(m_SaveGameJamData);

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_ProgrammingNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.ProgrammingStudentPanel, true);

                        string _name = _studentList[i].m_StudentStat.m_StudentName;
                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedProgrammingStat);
                        //m_GameJamPanel.SetRequirmentStatImage(StudentType.Programming, ProgrammingRequirementStatList);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.Programming, _name, _health, _passion,
                            _studentList[i].m_StudentStat.m_AbilityAmountList, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.Programming);

                        m_GameJamPanel.ChangeSlider(StudentType.Programming, _sliderValue);

                        int _needValue = 0;

                        if (m_NeedProgrammingStat.Count > 1)
                        {
                            _needValue = m_NeedProgrammingStat.ElementAt(0).Value + m_NeedProgrammingStat.ElementAt(1).Value;

                        }
                        else
                        {
                            _needValue = m_NeedProgrammingStat.ElementAt(0).Value;

                        }
                        m_GameJamPanel.ChangeSliderFillSprite(StudentType.Programming, _sliderValue, _needValue);

                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_GMNone, m_GameJamPanel.m_ArtNone);
                    }
                }
                else
                {
                    // �̹� ������ �л��̴� �ȵȴٴ� ��� ����ֱ�
                    _currentObj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        if (m_GameJamPanel.m_GMNone.activeSelf == false && m_GameJamPanel.m_ArtNone.activeSelf == false &&
            m_GameJamPanel.m_ProgrammingNone.activeSelf == false)
        {
            DecideGenre();
            DeterminesRank();
        }

        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 22)
        {
            m_Unmask.fitTarget = m_GameJamPanel.ArtButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 250, 0);
            m_TutorialCount++;
        }
        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 24)
        {
            m_Unmask.fitTarget = m_GameJamPanel.ProgrammingButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 250, 0);
            m_TutorialCount++;
        }
        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 26)
        {
            m_Unmask.fitTarget = m_GameJamPanel.SelectCompleteButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }
    }

    public void ClickStartButton()
    {
        if (m_GMStudentData.Count > 0 && m_ArtStudentData.Count > 0 && m_ProgrammingStudentData.Count > 0)
        {
            m_GameJamPanel.ClickSelectCompleteButton();

            if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 27)
            {
                m_Unmask.fitTarget = m_GameJamPanel.ParticipationYesButton.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 300, 0);
                m_TutorialCount++;
            }
        }
    }

    // �������� �����ϴ� ��ư�� ������ �������� ����Ǹ� Ÿ�̸Ӱ� �����ȴ�. 
    public void SelectCompleteButton()
    {
        int _temp = m_SaveGameJamData.m_GameJamInfoData.m_GjamTime;
        string _name = m_SaveGameJamData.m_GameJamInfoData.m_GjamName;

        if (m_GameJamEntryCount.ContainsKey(_name))
        {
            m_EnteryCount++;
            m_GameJamEntryCount[_name] = m_EnteryCount;
        }
        else
        {
            m_GameJamEntryCount.Add(_name, 1);
        }

        if (_temp < 60)
        {
            m_Timer.StartGameJam("0", _temp.ToString());

        }
        else
        {
            m_Timer.StartGameJam((_temp % 60).ToString(), _temp.ToString());
        }

        //m_GameJamIDList.Add(m_SaveGameJamData.m_GameJamInfoData.m_GjamID);
        m_GMStudentData[0].m_StudentData.m_StudentStat.m_NumberOfEntries -= 1;
        m_ArtStudentData[0].m_StudentData.m_StudentStat.m_NumberOfEntries -= 1;
        m_ProgrammingStudentData[0].m_StudentData.m_StudentStat.m_NumberOfEntries -= 1;

        m_SaveGameJamData.m_GameJamData.m_GM.Add(m_GMStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_Art.Add(m_ArtStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_Programming.Add(m_ProgrammingStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_MakeYear = GameTime.Instance.FlowTime.NowYear;
        m_SaveGameJamData.m_GameJamData.m_MakeMonth = GameTime.Instance.FlowTime.NowMonth;

        m_ToBeRunning.Add(m_SaveGameJamData);

        //m_GameAndStudentData = m_SaveGameJamData;

        m_MonthLimit -= 1;
        PlayerInfo.Instance.m_MyMoney -= m_SaveGameJamData.m_GameJamInfoData.m_EntryFee;
        MonthlyReporter.Instance.m_NowMonth.ExpensesActivity += m_SaveGameJamData.m_GameJamInfoData.m_EntryFee;
        //SetRecruitNoticeInfoContent();
        //StartCoroutine(GameJamScheduler());
    }

    private IEnumerator CheckClassStart(float second)
    {
        yield return new WaitUntil(() =>
        {
            if (InGameTest.Instance.m_ClassState == ClassState.Studying)
            {
                StartCoroutine(GameJamScheduler(second));
                return true;
            }
            else
            {
                return false;
            }

        });
    }

    private IEnumerator GameJamScheduler(float second)
    {
        m_Timer.SetActiveSelf(true);

        m_TimerText = second;

        while (m_TimerText > 0)
        {
            m_TimerText -= Time.deltaTime;
            string _min = Mathf.Floor(m_TimerText / 60).ToString("00");
            string _second = (m_TimerText % 60).ToString("00");
            m_Timer.ChangeText(_min, _second);
            m_Timer.ChangeImageColor(new Color(1, 1, 1, 1));
            yield return null;

            if (m_TimerText <= 0.5)
            {
                m_Timer.SetActiveSelf(false);

                //m_ResultPanel.SetActive(true);
            }
            else if (m_TimerText <= 5)
            {
                m_Timer.ChangeImageColor(new Color(255, 0, 0, 255));
            }
        }

        if (m_TimerText <= 0)
        {
            m_PopUpResultPanel.TurnOnUI();
        }
    }

    // Ÿ�̸Ӱ� �� ������ ���� ���â�� �������Ѵ�.
    public void ClickResultPanelButtn()
    {
        m_GameJamResultPanel.SetResultPanelMoneyAndSpecialPoint();

        string _gmName = m_GMStudentData[0].m_StudentData.m_StudentStat.m_StudentName;
        string _artName = m_ArtStudentData[0].m_StudentData.m_StudentStat.m_StudentName;
        string _programmingName = m_ProgrammingStudentData[0].m_StudentData.m_StudentStat.m_StudentName;

        SetGameJamNeedStatPanel(StudentType.GameDesigner);
        SetGameJamNeedStatPanel(StudentType.Art);
        SetGameJamNeedStatPanel(StudentType.Programming);

        m_GameJamResultPanel.ChangeResultStudentInfo(StudentType.GameDesigner, _gmName);
        m_GameJamResultPanel.ChangeResultStudentInfo(StudentType.Art, _artName);
        m_GameJamResultPanel.ChangeResultStudentInfo(StudentType.Programming, _programmingName);

        SetGameJamResultInfo();

        m_PopOffResultPanel.TurnOffUI();
    }

    // ��� ���� �ߴ� ������ ������ �����ͼ� �������â�� ����ش�.
    public void SetFinalGameJamPanel()
    {
        m_GameJamResultPanel.TurnOffPanel();

        // SaveGameJamData gameJamData = m_GameJamHistory[m_SaveGameJamData.m_GameJamInfoData.m_GjamID].Last();
        CalculateFinalGameJamResult();
        int _awareness = m_SaveGameJamData.m_GameJamData.m_Awareness;
        int _TalentDevelopment = m_SaveGameJamData.m_GameJamData.m_PracticalTalent;
        int _management = m_SaveGameJamData.m_GameJamData.m_Management;

        int _prevAwereness = PlayerInfo.Instance.m_Awareness;
        int _prevTalentDevelopment = PlayerInfo.Instance.m_TalentDevelopment;
        int _prevManagement = PlayerInfo.Instance.m_Management;

        int _finalAwereness = _prevAwereness + _awareness;
        int _finalTalentDevelopment = _prevTalentDevelopment + _TalentDevelopment;
        int _finalManagement = _prevManagement + _management;

        Sprite _awarenessArrow = _awareness > 0 ? m_UpArrow : _awareness == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _TalentDevelopmentArrow = _TalentDevelopment > 0 ? m_UpArrow : _TalentDevelopment == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _managementArrow = _management > 0 ? m_UpArrow : _management == 0 ? m_NoneArrow : m_DownArrow;

        Sprite _FinalawarenessArrow = _finalAwereness - _prevAwereness > 0 ? m_UpArrow : _finalAwereness - _prevAwereness == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _FinalTalentDevelopmentArrow = _finalTalentDevelopment - _prevTalentDevelopment > 0 ? m_UpArrow : _finalTalentDevelopment - _prevTalentDevelopment == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _FinalmanagementArrow = _finalManagement - _prevManagement > 0 ? m_UpArrow : _finalManagement - _prevManagement == 0 ? m_NoneArrow : m_DownArrow;

        string _gameJamName = m_GameJamResultPanel._changeGameName;

        if (_gameJamName == "")
        {
            string GjamName = m_SaveGameJamData.m_GameJamInfoData.m_GjamName;
            _gameJamName = GjamName + m_GameJamEntryCount[GjamName].ToString(); ;
        }

        string _date = GameTime.Instance.FlowTime.NowYear + "�� " + GameTime.Instance.FlowTime.NowMonth + "�� " + GameTime.Instance.FlowTime.NowWeek + "���� ���";
        m_FinalGameJamResultPanel.SetPreInfo(_prevAwereness.ToString(), _prevTalentDevelopment.ToString(), _prevManagement.ToString());
        m_FinalGameJamResultPanel.SetChangeInfo(_awareness.ToString(), _TalentDevelopment.ToString(), _management.ToString());
        m_FinalGameJamResultPanel.SetChangeInfoArrowImage(_awarenessArrow, _TalentDevelopmentArrow, _managementArrow);
        m_FinalGameJamResultPanel.SetSlider(_finalAwereness, _finalTalentDevelopment, _finalManagement);
        m_FinalGameJamResultPanel.SetFinalInfo(_finalAwereness.ToString(), _finalTalentDevelopment.ToString(), _finalManagement.ToString());
        m_FinalGameJamResultPanel.SetFinalInfoArrowImage(_FinalawarenessArrow, _FinalTalentDevelopmentArrow, _FinalmanagementArrow);

        m_SaveGameJamData.m_GameJamData.m_GameName = _gameJamName;
        m_SaveGameJamData.m_GameJamData.m_GM.Add(m_GMStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_Art.Add(m_ArtStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_Programming.Add(m_ProgrammingStudentData[0].m_StudentData);
        m_SaveGameJamData.m_GameJamData.m_Genre = _genreName;
        m_SaveGameJamData.m_GameJamData.m_Rank = _rank;
        m_SaveGameJamData.m_GameJamData.m_Funny = Funny;
        m_SaveGameJamData.m_GameJamData.m_Graphic = Graphic;
        m_SaveGameJamData.m_GameJamData.m_Perfection = Perfection;
        m_SaveGameJamData.m_GameJamData.m_TotalGenreScore = m_TotalGenreScore;

        // �����͸� ������ �� �̹� �� �� �����غô� �̺�Ʈ�� �ش� ���̵� ���� ����Ʈ�� ���� ����. �ƴ϶�� ���ο� ����Ʈ ���� ���̵�� �����ϱ�
        if (m_GameJamHistory.ContainsKey(m_SaveGameJamData.m_GameJamInfoData.m_GjamID))
        {
            m_GameJamHistory[m_SaveGameJamData.m_GameJamInfoData.m_GjamID].Add(m_SaveGameJamData);
        }
        else
        {
            List<SaveGameJamData> m_GameJamDataList = new List<SaveGameJamData>();
            m_GameJamDataList.Add(m_SaveGameJamData);
            m_GameJamHistory.Add(m_SaveGameJamData.m_GameJamInfoData.m_GjamID, m_GameJamDataList);
            OnDataChanged();
        }

        EnqueueDataChangedEvent();

        SetRiseStat(_rank, GMRequirementStatList, RiseGMStatList);
        SetRiseStat(_rank, ArtRequirementStatList, RiseArtStatList);
        SetRiseStat(_rank, ProgrammingRequirementStatList, RiseProgrammingStatList);

        ChangeStudentData(m_GMStudentData[0].m_StudentData.m_StudentStat.m_StudentName, RiseGMStatList);
        ChangeStudentData(m_ArtStudentData[0].m_StudentData.m_StudentStat.m_StudentName, RiseArtStatList);
        ChangeStudentData(m_ProgrammingStudentData[0].m_StudentData.m_StudentStat.m_StudentName, RiseProgrammingStatList);

        PlayerInfo.Instance.m_Awareness = _finalAwereness;
        PlayerInfo.Instance.m_Management = _finalManagement;
        PlayerInfo.Instance.m_TalentDevelopment = _finalTalentDevelopment;

        /// �������� ���� ����, �, ����缺 ���� ����
        MonthlyReporter.Instance.m_NowMonth.ManagementScore = _finalManagement;
        MonthlyReporter.Instance.m_NowMonth.FamousScore = _finalAwereness;
        MonthlyReporter.Instance.m_NowMonth.TalentDevelopmentScore = _finalTalentDevelopment;

        int reward = FindRewardToDifficulty(m_SaveGameJamData.m_GameJamInfoData.m_GjamAI_ID, m_SaveGameJamData.m_GameJamData.m_Rank);

        m_FinalGameJamResultPanel.SetResultPanel(InGameUI.Instance.m_nowAcademyName.text, _gameJamName, _date, reward.ToString());

        if (_rank != "�̿ϼ�")
        {
            PlayerInfo.Instance.m_MyMoney += reward;
            MonthlyReporter.Instance.m_NowMonth.IncomeActivity += reward;
        }
        int index = m_ToBeRunning.FindIndex(x => x.m_GameJamInfoData.m_GjamWeek == m_SaveGameJamData.m_GameJamInfoData.m_GjamWeek);
        m_ToBeRunning.RemoveAt(index);
        //m_GameJamResultPanel.InitChangeName();
    }

    // ������ ���� �濵���� ����ϴ� �Լ�
    private void CalculateFinalGameJamResult()
    {
        if (_rank == "�̿ϼ�")
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = -10;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 0;
            m_SaveGameJamData.m_GameJamData.m_Management = 0;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 50)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 2;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 0;
            m_SaveGameJamData.m_GameJamData.m_Management = 0;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 80)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 4;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 0;
            m_SaveGameJamData.m_GameJamData.m_Management = 0;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 110)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 6;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 1;
            m_SaveGameJamData.m_GameJamData.m_Management = 0;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 140)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 8;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 2;
            m_SaveGameJamData.m_GameJamData.m_Management = 1;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 170)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 10;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 3;
            m_SaveGameJamData.m_GameJamData.m_Management = 2;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 200)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 12;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 4;
            m_SaveGameJamData.m_GameJamData.m_Management = 3;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 230)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 14;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 5;
            m_SaveGameJamData.m_GameJamData.m_Management = 4;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 260)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 16;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 6;
            m_SaveGameJamData.m_GameJamData.m_Management = 5;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 290)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 18;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 7;
            m_SaveGameJamData.m_GameJamData.m_Management = 6;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 320)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 20;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 8;
            m_SaveGameJamData.m_GameJamData.m_Management = 7;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 350)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 22;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 9;
            m_SaveGameJamData.m_GameJamData.m_Management = 8;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 380)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 24;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 10;
            m_SaveGameJamData.m_GameJamData.m_Management = 9;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 410)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 26;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 11;
            m_SaveGameJamData.m_GameJamData.m_Management = 10;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 440)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 28;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 12;
            m_SaveGameJamData.m_GameJamData.m_Management = 11;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 470)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 30;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 13;
            m_SaveGameJamData.m_GameJamData.m_Management = 12;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 500)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 32;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 14;
            m_SaveGameJamData.m_GameJamData.m_Management = 13;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 530)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 34;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 15;
            m_SaveGameJamData.m_GameJamData.m_Management = 14;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 560)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 36;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 16;
            m_SaveGameJamData.m_GameJamData.m_Management = 15;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 590)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 38;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 17;
            m_SaveGameJamData.m_GameJamData.m_Management = 16;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 620)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 40;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 18;
            m_SaveGameJamData.m_GameJamData.m_Management = 17;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 650)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 42;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 19;
            m_SaveGameJamData.m_GameJamData.m_Management = 18;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 680)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 44;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 20;
            m_SaveGameJamData.m_GameJamData.m_Management = 19;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 710)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 46;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 21;
            m_SaveGameJamData.m_GameJamData.m_Management = 20;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 740)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 48;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 22;
            m_SaveGameJamData.m_GameJamData.m_Management = 21;
        }
        else if (m_SaveGameJamData.m_GameJamData.m_Score <= 999)
        {
            m_SaveGameJamData.m_GameJamData.m_Awareness = 50;
            m_SaveGameJamData.m_GameJamData.m_PracticalTalent = 23;
            m_SaveGameJamData.m_GameJamData.m_Management = 22;
        }
    }

    // �䱸�ϴ� ������ ù��°. �ϳ��ۿ� ������ �� �Լ��� �����ϸ� �ȴ�
    private void FindPartFirstRequirementStat(string _statName, List<Student> _list, int _index)
    {
        for (int i = 0; i < AbilityNameList.Length; i++)
        {
            if (_statName == AbilityNameList[i])
            {
                m_Requirement1List[(int)_list[_index].m_StudentStat.m_StudentType] = _list[_index].m_StudentStat.m_AbilityAmountList[i];
                break;
            }
        }
    }

    // �䱸�ϴ� ������ �ι�°.
    private void FindPartSecondRequirementStat(string _statName, List<Student> _list, int _index)
    {
        for (int i = 0; i < AbilityNameList.Length; i++)
        {
            if (_statName == AbilityNameList[i])
            {
                m_Requirement2List[(int)_list[_index].m_StudentStat.m_StudentType] = _list[_index].m_StudentStat.m_AbilityAmountList[i];
                break;
            }
        }
    }

    // �л��� ������ �ִ� ������ �ʼ� ������ �����ش�.
    private void StudentNeedStat(List<Student> _list, int _index, Dictionary<string, int> _dic)
    {
        string _statName1 = _dic.ElementAt(0).Key;
        int _statNum1 = _dic.ElementAt(0).Value;

        if (_dic.Count > 1)
        {
            string _statName2 = _dic.ElementAt(1).Key;
            int _statNum2 = _dic.ElementAt(1).Value;

            if (_statNum1 > _statNum2)
            {
                FindPartFirstRequirementStat(_statName1, _list, _index);

                FindPartSecondRequirementStat(_statName2, _list, _index);
            }
            else
            {
                FindPartFirstRequirementStat(_statName2, _list, _index);

                FindPartSecondRequirementStat(_statName1, _list, _index);
            }
        }
        else
        {
            FindPartFirstRequirementStat(_statName1, _list, _index);
        }
    }

    // �ʼ� ������ ������ ���� �������� ���ְ� ���ش�.
    private void SetReauirementIcon(Image _icon1, Image _icon2, Dictionary<string, int> _dummyDic)
    {
        if (_dummyDic.Count > 1)
        {
            _icon1.gameObject.SetActive(true);
            _icon2.gameObject.SetActive(true);

            ChangeRequirementStatImage(_dummyDic.ElementAt(0).Key, _icon1);
            ChangeRequirementStatImage(_dummyDic.ElementAt(1).Key, _icon2);
        }
        else
        {
            _icon1.gameObject.SetActive(true);
            _icon2.gameObject.SetActive(false);

            ChangeRequirementStatImage(_dummyDic.ElementAt(0).Key, _icon1);
        }
    }

    private Sprite FindGenreSprite(string _genre)
    {
        switch (_genre)
        {
            case "����":
            {
                return m_GenreSprite[0];
            }

            case "�ùķ��̼�":
            {
                return m_GenreSprite[1];
            }

            case "����":
            {
                return m_GenreSprite[2];
            }

            case "��庥��":
            {
                return m_GenreSprite[3];
            }

            case "RPG":
            {
                return m_GenreSprite[4];
            }

            case "������":
            {
                return m_GenreSprite[5];
            }

            case "�׼�":
            {
                return m_GenreSprite[6];
            }

            case "����":
            {
                return m_GenreSprite[7];
            }
        }

        return null;
    }

    // �ʼ� ������ ������ ���� ���� �̹����� �־��ش�.
    private void ChangeRequirementStatImage(string _statName, Image _statImgae)
    {
        switch (_statName)
        {
            case "����":
            {
                _statImgae.sprite = m_RequirementStats[(int)AbilityType.Insight];
                _statImgae.rectTransform.sizeDelta = new Vector2(50, 50);
            }
            break;

            case "����":
            {
                _statImgae.sprite = m_RequirementStats[(int)AbilityType.Concentration];
                _statImgae.rectTransform.sizeDelta = new Vector2(60, 50);
            }
            break;

            case "����":
            {
                _statImgae.sprite = m_RequirementStats[(int)AbilityType.Sense];
                _statImgae.rectTransform.sizeDelta = new Vector2(60, 50);
            }
            break;

            case "���":
            {
                _statImgae.sprite = m_RequirementStats[(int)AbilityType.Technique];
                _statImgae.rectTransform.sizeDelta = new Vector2(50, 50);
            }
            break;

            case "��ġ":
            {
                _statImgae.sprite = m_RequirementStats[(int)AbilityType.Wit];
                _statImgae.rectTransform.sizeDelta = new Vector2(50, 50);
            }
            break;
        }
    }

    private void SetRecruitContent(int _index, string _entryFee)
    {
        _gameJameName = m_DummyGameJamList[_index].m_GjamName;

        string _name = m_DummyGameJamList[_index].m_GjamName + "������";
        string _subName = m_DummyGameJamList[_index].m_GjamDetailInfo;
        string _reward = FindRewardToDifficulty(m_DummyGameJamList[_index].m_GjamAI_ID).ToString();
        string _awareness = "2 ~ 50";
        string _date = m_DummyGameJamList[_index].m_GjamMonth.ToString() + "�� " + m_DummyGameJamList[_index].m_GjamWeek.ToString() + "����";
        string _health = m_DummyGameJamList[_index].m_StudentHealth.ToString();
        string _passion = m_DummyGameJamList[_index].m_StudentPassion.ToString();
        Sprite _maingenre = FindGenreSprite(m_DummyGameJamList[_index].m_GjamMainGenre);
        Sprite _subGenre = FindGenreSprite(m_DummyGameJamList[_index].m_GjamSubGenre);
        ClassifyPartNeedStat("��ȹ", m_DummyGameJamList[_index].m_GjamNeedStatGM);
        ClassifyPartNeedStat("��Ʈ", m_DummyGameJamList[_index].m_GjamNeedStatArt);
        ClassifyPartNeedStat("�ù�", m_DummyGameJamList[_index].m_GjamNeedStatProgramming);

        m_GameJamPanel.ChangeRecruitNoticeInfo(_name, _subName, _reward, _awareness, _date, _health, _passion, _entryFee);
        m_GameJamPanel.SetAwarenessPlusMinus(_awareness, m_UpArrow, m_DownArrow);
        m_GameJamPanel.SetHealthPlusMinus(_health, m_UpArrow, m_DownArrow);
        m_GameJamPanel.SetPassionPlusMinus(_passion, m_UpArrow, m_DownArrow);
        m_GameJamPanel.ChangeGenreSprite(_maingenre, _subGenre);
        SetReauirementIcon(m_GameJamPanel.GMRequirementStatIcon1, m_GameJamPanel.GMRequirementStatIcon2, m_NeedGMStat);
        SetReauirementIcon(m_GameJamPanel.ArtRequirementStatIcon1, m_GameJamPanel.ArtRequirementStatIcon2, m_NeedArtStat);
        SetReauirementIcon(m_GameJamPanel.ProgrammingRequirementStatIcon1, m_GameJamPanel.ProgrammingRequirementStatIcon2, m_NeedProgrammingStat);

        for (int j = 0; j < 5; j++)
        {
            GMRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatGM[AbilityNameList[j]];
            ArtRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatArt[AbilityNameList[j]];
            ProgrammingRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatProgramming[AbilityNameList[j]];
        }
    }

    // ������ �г��� ���� �� ���� ó�� �����ִ� ������ ����Ʈ �� �����ִ� �������� �Ѵ�.
    public void FirstGameJamIfoToRecruitNoticeInfo()
    {
        if (m_DummyGameJamList.Count == 0)
        {
            //m_GameJamPanel.m_GameJamParent.GetChild(0).GetComponent<Button>().Select();

            if (m_GameJamPanel.GameJamCanvas.activeSelf)
            {
                m_GameJamPanel.SetActiveGameJamCanvas(false);
            }
            m_Slider.SetWarningPanel.SetActive(true);

            StartCoroutine(SetWarningPanel());
        }
        else if (m_MonthLimit > 0)
        {
            if (!m_GameJamPanel.GameJamCanvas.activeSelf)
            {
                m_GameJamPanel.SetActiveGameJamCanvas(true);
                m_Slider.SetWarningPanel.SetActive(false);
            }
            //SetWarningPanel();

            if (m_ToBeRunning.Count == 0 || m_DummyGameJamList[0].m_GjamName != m_ToBeRunning[0].m_GameJamInfoData.m_GjamName)
            {
                m_GameJamPanel.m_GameJamParent.GetChild(0).GetComponent<Button>().Select();

                string _entryFee = m_DummyGameJamList[0].m_EntryFee.ToString();
                string _money = m_GameJamPanel.SetCurrentMoney();

                SetRecruitContent(0, _entryFee);

                // �����ϰ� �ִ� �ݾ��� �����⺸�� ������ ���������� ���� �ٲ��ֱ�
                if (int.Parse(_entryFee) > int.Parse(_money))
                {
                    m_GameJamPanel.ChangeColorMoneyText(new Color(255, 0, 0, 255));
                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_NotEntry);
                }
                else
                {
                    m_GameJamPanel.ChangeColorMoneyText(new Color(0, 0, 0, 255));
                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                }

                m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                m_GameJamPanel.ChangeStartButtonName("����");
            }
            else
            {
                m_GameJamPanel.m_GameJamParent.GetChild(0).GetComponent<Button>().Select();

                string _entryFee = "-";

                SetRecruitContent(0, _entryFee);

                m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_EntryComplete);
                m_GameJamPanel.ChangeStartButtonName("���� ��û �Ϸ�");
            }

            m_SaveGameJamData.m_GameJamData.m_GM = new List<Student>();
            m_SaveGameJamData.m_GameJamData.m_Art = new List<Student>();
            m_SaveGameJamData.m_GameJamData.m_Programming = new List<Student>();

            if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 5)
            {
                m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = false;
                m_Unmask.fitTarget = m_GameJamPanel.RecruitNoticePanel.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(false);
                m_TutorialTextImage.gameObject.SetActive(true);
                m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(700, -200, 0);
                m_ScriptCount++;
                m_TutorialCount++;
            }
        }
        else // �̹� �Ѵ޿� 2�� �����뿡 �������� ��� �����Ұ��� ����������
        {
            if (!m_GameJamPanel.GameJamCanvas.activeSelf)
            {
                m_GameJamPanel.SetActiveGameJamCanvas(true);
                m_Slider.SetWarningPanel.SetActive(false);
            }

            // �̹��� ���� Ƚ���� �� ��ٰ� ��� ����ֱ�
            for (int i = 0; i < m_ToBeRunning.Count; i++)
            {
                if (m_ToBeRunning[i].m_GameJamInfoData.m_GjamName == m_DummyGameJamList[0].m_GjamName)
                {
                    string _entryFee = "-";

                    SetRecruitContent(0, _entryFee);

                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_EntryComplete);
                    m_GameJamPanel.ChangeStartButtonName("���� ��û �Ϸ�");
                    return;
                }
                else
                {
                    string _entryFee = "-";

                    SetRecruitContent(0, _entryFee);

                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_NotEntry);
                    m_GameJamPanel.ChangeStartButtonName("��û �Ұ�");
                }
                break;
            }

            //SetWarningPanel();

            //string _entryFee = "-";

            //SetRecruitContent(0, _entryFee);

            //m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_NotEntry);
            //m_GameJamPanel.ChangeStartButtonName("��û �Ұ�");

        }
    }

    IEnumerator SetWarningPanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        m_Slider.SetWarningPanel.SetActive(false);
    }

    IEnumerator SetEntryWarningPanel()
    {
        _warningMessage = "���� �� �� �ִ� Ƚ���� �ʰ��Ͽ����ϴ�!\n�������� ���������.";
        yield return new WaitForSecondsRealtime(3f);
        m_GameJamPanel.SetActiveEntryCountWarningPanel(false, _warningMessage);
    }

    private void MakeGameJamList()
    {
        for (int i = 0; i < m_DummyGameJamList.Count; i++)
        {
            GameObject _gameJamPrefab = Instantiate(m_GameJamPrefab);
            m_GameJamPanel.MoveObj(_gameJamPrefab, m_GameJamPanel.m_GameJamParent);

            _gameJamPrefab.name = m_DummyGameJamList[i].m_GjamName;

            _gameJamPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_DummyGameJamList[i].m_GjamName;
            _gameJamPrefab.GetComponent<Button>().onClick.AddListener(MakeRecruitNoticeInfo);
        }
    }

    // ������ ������� ��������� ����� �����ִ� �Լ�
    public void MakePreGameJamList()
    {
        m_GameJamListPanel.DestroyObj();

        for (int i = 0; i < m_GameJamHistory.Count; i++)
        {
            // ���� ���̵��� �������� �� �� ������ ����Ʈ�� ���� �������� ������ �װ� �� ���鼭 ���������Ѵ�.
            if (m_GameJamHistory.ElementAt(i).Value.Count > 1)
            {
                for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                {
                    GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                    m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                    string _gameName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                    string _genreName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre;
                    string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank;
                    Sprite _genre = FindGenreSprite(_genreName);

                    _gameJamListPrefab.name = _gameName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

                    GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.gameObject;
                    //m_GameJamListPanel.changePrefabContent(_gameName, _genreName, _rank);
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(() => MakePreGameJamInfo(_gameList));
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(m_GameJamListPanel.ClickGameJamPrefab);
                }
            }
            else
            {
                GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                string _gameName = m_GameJamHistory.ElementAt(i).Value[0].m_GameJamData.m_GameName;
                string _genreName = m_GameJamHistory.ElementAt(i).Value[0].m_GameJamData.m_Genre;
                string _rank = m_GameJamHistory.ElementAt(i).Value[0].m_GameJamData.m_Rank;
                Sprite _genre = FindGenreSprite(_genreName);

                _gameJamListPrefab.name = _gameName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

                GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.gameObject;
                //m_GameJamListPanel.changePrefabContent(_gameName, _genreName, _rank);
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(() => MakePreGameJamInfo(_gameList));
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(m_GameJamListPanel.ClickGameJamPrefab);
            }
        }
    }

    // ���� ��ư�� �� ������ �������ִ� �Լ�
    private void MakeRecruitNoticeInfo()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        _gameJameName = _currentObj.name;

        /// TODO : ������ ��ư�� ������ ���� ToBeRunning�� ����־ Argument Out Over Range�� �߻��Ѵ�.
        if (m_MonthLimit > 0)
        {
            if (m_ToBeRunning.Count == 0)
            {
                for (int i = 0; i < m_DummyGameJamList.Count; i++)
                {
                    if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                    {
                        string _entryFee = m_DummyGameJamList[i].m_EntryFee.ToString();
                        string _money = m_GameJamPanel.SetCurrentMoney();


                        SetRecruitContent(i, _entryFee);

                        // �����ϰ� �ִ� �ݾ��� �����⺸�� ������ ���������� ���� �ٲ��ֱ�
                        if (int.Parse(_entryFee) > int.Parse(_money))
                        {
                            m_GameJamPanel.ChangeColorMoneyText(new Color(255, 0, 0, 255));
                            m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_NotEntry);
                        }
                        else
                        {
                            m_GameJamPanel.ChangeColorMoneyText(new Color(0, 0, 0, 255));
                            m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                        }

                        m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                        m_GameJamPanel.ChangeStartButtonName("����");
                    }
                }
                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 7)
                {
                    m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = true;
                    m_Unmask.fitTarget = m_GameJamPanel.RecruitNoticeInfoPanel.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-1600, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
            else
            {
                for (int i = 0; i < m_ToBeRunning.Count; i++)
                {
                    if (m_ToBeRunning[i].m_GameJamInfoData.m_GjamName != _gameJameName)
                    {
                        for (int j = 0; j < m_DummyGameJamList.Count; j++)
                        {
                            if (_gameJameName == m_DummyGameJamList[j].m_GjamName)
                            {
                                string _entryFee = m_DummyGameJamList[j].m_EntryFee.ToString();
                                string _money = m_GameJamPanel.SetCurrentMoney();
                                _selectGameWeek = m_DummyGameJamList[j].m_GjamWeek;

                                SetRecruitContent(j, _entryFee);

                                // �����ϰ� �ִ� �ݾ��� �����⺸�� ������ ���������� ���� �ٲ��ֱ�
                                if (int.Parse(_entryFee) > int.Parse(_money))
                                {
                                    m_GameJamPanel.ChangeColorMoneyText(new Color(1, 0, 0, 1));
                                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_NotEntry);
                                }
                                else
                                {
                                    m_GameJamPanel.ChangeColorMoneyText(new Color(0, 0, 0, 1));
                                    m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                                }
                                m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                                m_GameJamPanel.ChangeStartButtonName("����");
                            }
                        }
                    }
                    else
                    {
                        string _entryFee = "-";

                        SetRecruitContent(i, _entryFee);

                        m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_EntryComplete);
                        m_GameJamPanel.ChangeStartButtonName("���� ��û �Ϸ�");
                    }
                }
            }
        }
        else
        {
            // �̹��� ���� Ƚ���� �� ��ٰ� ��� ����ֱ�
            for (int i = 0; i < m_ToBeRunning.Count; i++)
            {
                for (int j = 0; j < m_DummyGameJamList.Count; j++)
                {
                    if (m_ToBeRunning[i].m_GameJamInfoData.m_GjamName == _gameJameName)
                    {
                        string _entryFee = "-";

                        SetRecruitContent(i, _entryFee);

                        m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_EntryComplete);
                        m_GameJamPanel.ChangeStartButtonName("���� ��û �Ϸ�");
                        return;
                    }
                    else if (_gameJameName == m_DummyGameJamList[j].m_GjamName)
                    {
                        string _entryFee = "-";

                        SetRecruitContent(j, _entryFee);

                        m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_NotEntry);
                        m_GameJamPanel.ChangeStartButtonName("��û �Ұ�");
                    }
                }
            }
        }
    }

    // ���� ������ �����ִ� �гο��� ���� �������� � �������� �帣���� �������ִ� �Լ�
    private void MakePreGameJamInfo(GameObject _obj)
    {
        EventSystem.current.SetSelectedGameObject(_obj);
        string _gameName = _obj.name;

        for (int i = 0; i < m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (_gameName == m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName)
                {
                    //m_GameJamListPanel.ClickGameJamPrefab();
                    string _name = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                    string _year = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamInfoData.m_GjamYear.ToString();
                    string _month = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamInfoData.m_GjamMonth.ToString();

                    string _gmName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GM[0].m_StudentStat.m_StudentName;
                    string _artName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Art[0].m_StudentStat.m_StudentName;
                    string _programmingName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Programming[0].m_StudentStat.m_StudentName;
                    Sprite _gmProfile = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GM[0].StudentProfileImg;
                    Sprite _artProfile = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Art[0].StudentProfileImg;
                    Sprite _programmingProfile = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Programming[0].StudentProfileImg;
                    Sprite _genreSprite = FindGenreSprite(m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre);
                    string _concept = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamInfoData.m_GjamConcept;
                    string _funny = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Funny.ToString();
                    string _graphic = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Graphic.ToString();
                    string _perfection = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Perfection.ToString();
                    string _totalGenreScore = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_TotalGenreScore.ToString();
                    string _score = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Score.ToString();
                    string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank;

                    if (_year == "0")
                    {
                        _year = GameTime.Instance.FlowTime.NowYear.ToString();
                    }

                    m_GameJamListPanel.ChangeGameInfo(_year + "���� " + _month + "��", _name, _name);
                    m_GameJamListPanel.ChangeStudentInfo(_gmName, _artName, _programmingName, _gmProfile, _artProfile, _programmingProfile);
                    m_GameJamListPanel.ChangeGameJamScorePanel(_concept, _funny, _graphic, _perfection, _totalGenreScore, _score, _rank);  /// �л� �帣������ �߰�����
                    m_GameJamListPanel.ChangeGenreSprite(_genreSprite);
                    m_GameJamListPanel.ChangeMVPImageGameListPanel(Funny, Graphic, Perfection);


                    //int _value = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GM[0].

                    int _gmValue = FindStudent(StudentType.GameDesigner);
                    int _artValue = FindStudent(StudentType.Art);
                    int _programmingValue = FindStudent(StudentType.Programming);

                    int _gmNeedValue = m_NeedGMStat.ElementAt(0).Value + m_NeedGMStat.ElementAt(1).Value;
                    int _artNeedValue = m_Requirement1List[1] + m_Requirement2List[1];
                    int _programmingNeedValue = m_Requirement1List[2] + m_Requirement2List[2];


                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.GameDesigner, _gmValue, _gmNeedValue);
                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.Art, _artValue, _artNeedValue);
                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.Programming, _programmingValue, _programmingNeedValue);
                    //m_GameJamListPanel.SetPreGameStatSliderValue(_gmValue, _artValue, _programmingValue);
                    //m_GameJamListPanel.SetSliderBar(_gmNeedValue, _artNeedValue, _programmingNeedValue);
                    SetReauirementIcon(m_GameJamListPanel.GMRequiredStatImage1, m_GameJamListPanel.GMRequiredStatImage2, m_NeedGMStat);
                    SetReauirementIcon(m_GameJamListPanel.ArtRequiredStatImage1, m_GameJamListPanel.ArtRequiredStatImage2, m_NeedProgrammingStat);
                    SetReauirementIcon(m_GameJamListPanel.ProgrammingRequiredStatImage1, m_GameJamListPanel.ProgrammingRequiredStatImage2, m_NeedArtStat);

                    if (_gmValue >= _gmNeedValue)
                    {
                        m_GameJamListPanel.SetGMPreGameStatSliderColor(m_SatisfyStat);
                    }
                    else
                    {
                        m_GameJamListPanel.SetGMPreGameStatSliderColor(m_DissatisfactionStat);
                    }

                    if (_artValue >= _artNeedValue)
                    {
                        m_GameJamListPanel.SetArtPreGameStatSliderColor(m_SatisfyStat);
                    }
                    else
                    {
                        m_GameJamListPanel.SetArtPreGameStatSliderColor(m_DissatisfactionStat);
                    }

                    if (_programmingValue >= _programmingNeedValue)
                    {
                        m_GameJamListPanel.SetProgrammingPreGameStatSliderColor(m_SatisfyStat);
                    }
                    else
                    {
                        m_GameJamListPanel.SetProgrammingPreGameStatSliderColor(m_DissatisfactionStat);
                    }
                }
            }
        }
    }

    // �а����� �ʿ��� �ʼ� ���ȵ��� ��ųʸ��� �־��ִ� �Լ��̴�.
    private void ClassifyPartNeedStat(string _name, Dictionary<string, int> _info)
    {
        int _tempIndex = 0;

        if (_name == "��ȹ")
        {
            m_NeedGMStat.Clear();
        }
        else if (_name == "��Ʈ")
        {
            m_NeedArtStat.Clear();

        }
        if (_name == "�ù�")
        {
            m_NeedProgrammingStat.Clear();
        }

        for (int i = 0; i < 5; i++)
        {
            int _value = _info.ElementAt(i).Value;
            string _statName = _info.ElementAt(i).Key;

            if (_value > 0)
            {
                if (_name == "��ȹ")
                {
                    m_NeedGMStat.Add(_statName, _value);
                }
                else if (_name == "��Ʈ")
                {
                    m_NeedArtStat.Add(_statName, _value);

                }
                if (_name == "�ù�")
                {
                    m_NeedProgrammingStat.Add(_statName, _value);
                }
                _tempIndex++;
            }
        }

        if (_name == "��ȹ")
        {
            FindOutRequiredStatCount(_name, _tempIndex, m_NeedGMStat);
        }
        else if (_name == "��Ʈ")
        {
            FindOutRequiredStatCount(_name, _tempIndex, m_NeedArtStat);
        }
        else if (_name == "�ù�")
        {
            FindOutRequiredStatCount(_name, _tempIndex, m_NeedProgrammingStat);
        }
    }

    // �ʿ��� �ʼ� ������ �� ������ �� �������� ���� ����ó���� ���ָ� ������ �гο� �������ش�.
    private void FindOutRequiredStatCount(string _name, int _tempIndex, Dictionary<string, int> _stat)
    {
        if (_stat.Count == 1)
        {
            m_GameJamPanel.SetPartsNeedStat(_name, _stat.ElementAt(0).Value, _tempIndex);
            m_Requirement1 = _stat.ElementAt(0).Value;
        }
        else if (_stat.Count > 1)
        {
            if (_stat.ElementAt(0).Value < _stat.ElementAt(1).Value)
            {
                m_Requirement1 = _stat.ElementAt(1).Value;
                m_Requirement2 = _stat.ElementAt(0).Value + _stat.ElementAt(1).Value;
                m_GameJamPanel.SetPartsNeedStat(_name, m_Requirement1, _tempIndex, m_Requirement2);
            }
            else
            {
                m_Requirement1 = _stat.ElementAt(0).Value;
                m_Requirement2 = _stat.ElementAt(0).Value + _stat.ElementAt(1).Value;
                m_GameJamPanel.SetPartsNeedStat(_name, m_Requirement1, _tempIndex, m_Requirement2);
            }
        }
    }

    // ������ ���â�� �� ������ ������ִ� �Լ�
    public void SetGameJamResultInfo()
    {
        List<Student> _studentList = ObjectManager.Instance.m_StudentList;

        string _academyName = PlayerInfo.Instance.m_AcademyName;
        string _gameName = m_SaveGameJamData.m_GameJamInfoData.m_GjamName;
        string _placeHolder = _gameName + m_GameJamEntryCount[_gameName].ToString();
        string _concept = m_SaveGameJamData.m_GameJamInfoData.m_GjamConcept;
        Sprite _genreImage = FindGenreSprite(_genreName);

        Funny = 0;
        Graphic = 0;
        Perfection = 0;
        m_TotalGenreScore = 0;

        CalculateStat(_studentList, m_GMStudentData[0].m_GameJamInfoData.m_GjamNeedStatGM, m_GMStudentData, StudentType.GameDesigner);

        CalculateStat(_studentList, m_ArtStudentData[0].m_GameJamInfoData.m_GjamNeedStatArt, m_ArtStudentData, StudentType.Art);

        CalculateStat(_studentList, m_ProgrammingStudentData[0].m_GameJamInfoData.m_GjamNeedStatProgramming, m_ProgrammingStudentData, StudentType.Programming);

        double _score = m_GenreScore + _genreBonusScore + Funny + Graphic + Perfection;
        m_TotalGenreScore = m_GenreScore + _genreBonusScore;

        _score = Math.Truncate(_score);
        m_TotalGenreScore = Math.Truncate(m_TotalGenreScore);

        m_SaveGameJamData.m_GameJamData.m_Score = _score;

        CalculateRank(_score);

        m_GameJamResultPanel.SetPlusMinus(StudentType.GameDesigner, _rank, m_GMStudentData, GMRequirementStatList);
        m_GameJamResultPanel.SetPlusMinus(StudentType.Art, _rank, m_ArtStudentData, ArtRequirementStatList);
        m_GameJamResultPanel.SetPlusMinus(StudentType.Programming, _rank, m_ProgrammingStudentData, ProgrammingRequirementStatList);

        SetReauirementIcon(m_GameJamResultPanel.GMRequirementStat1, m_GameJamResultPanel.GMRequirementStat2, m_NeedGMStat);
        SetReauirementIcon(m_GameJamResultPanel.ArtRequirementStat1, m_GameJamResultPanel.ArtRequirementStat2, m_NeedArtStat);
        SetReauirementIcon(m_GameJamResultPanel.ProgrammingRequirementStat1, m_GameJamResultPanel.ProgrammingRequirementStat2, m_NeedProgrammingStat);

        SetReauirementIcon(m_GameJamResultPanel.NeedStatPanelGMStatIcon1, m_GameJamResultPanel.NeedStatPanelGMStatIcon2, m_NeedGMStat);
        SetReauirementIcon(m_GameJamResultPanel.NeedStatPanelArtStatIcon1, m_GameJamResultPanel.NeedStatPanelArtStatIcon2, m_NeedArtStat);
        SetReauirementIcon(m_GameJamResultPanel.NeedStatPanelProgrammingStatIcon1, m_GameJamResultPanel.NeedStatPanelProgrammingStatIcon2, m_NeedProgrammingStat);
        m_GameJamResultPanel.ChangeGenreSprite(_genreImage);

        if (_rank == "�̿ϼ�")
        {
            m_GameJamResultPanel.SetInfo(_academyName, _placeHolder, _concept, "-", "-", "-", "-", _rank, "-");
        }
        else
        {
            m_GameJamResultPanel.SetInfo(_academyName, _placeHolder, _concept, Funny.ToString(), Graphic.ToString(), Perfection.ToString(), _score.ToString(), _rank, m_TotalGenreScore.ToString());
        }

        m_GameJamResultPanel.ChangeMVPImageResultPanel(Funny, Graphic, Perfection);
    }

    // ������ ���â �� �ϳ��� ���� �����ߴ� �л����� ������ �����ִ� �г��� �������ִ� �Լ��̴�.
    private void SetGameJamNeedStatPanel(StudentType _type)
    {
        if (StudentType.GameDesigner == _type)
        {
            int _sliderValue = FindStudent(StudentType.GameDesigner);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.GameDesigner, _sliderValue, m_NeedGMStat);

            int _needValue = 0;

            if (m_NeedGMStat.Count > 1)
            {
                _needValue = m_NeedGMStat.ElementAt(0).Value + m_NeedGMStat.ElementAt(1).Value;
                //m_GameJamResultPanel.FillGMRequirementSlider(_sliderValue, _needValue);

            }
            else
            {
                _needValue = m_NeedGMStat.ElementAt(0).Value;
                //m_GameJamResultPanel.FillGMRequirementSlider(_sliderValue, _needValue);

            }
            m_GameJamResultPanel.ChangeColorFillImage(StudentType.GameDesigner, _sliderValue, _needValue, m_SatisfyStat, m_DissatisfactionStat);

        }
        else if (StudentType.Art == _type)
        {
            int _sliderValue = FindStudent(StudentType.Art);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.Art, _sliderValue, m_NeedArtStat);

            int _needValue = 0;

            if (m_NeedArtStat.Count > 1)
            {
                _needValue = m_NeedArtStat.ElementAt(0).Value + m_NeedArtStat.ElementAt(1).Value;
                //m_GameJamResultPanel.FillArtRequirementSlider(_sliderValue, _needValue);
            }
            else
            {
                _needValue = m_NeedArtStat.ElementAt(0).Value;
                //m_GameJamResultPanel.FillArtRequirementSlider(_sliderValue, _needValue);


            }
            m_GameJamResultPanel.ChangeColorFillImage(StudentType.Art, _sliderValue, _needValue, m_SatisfyStat, m_DissatisfactionStat);
        }
        else if (StudentType.Programming == _type)
        {
            int _sliderValue = FindStudent(StudentType.Programming);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.Programming, _sliderValue, m_NeedProgrammingStat);

            int _needValue = 0;

            if (m_NeedProgrammingStat.Count > 1)
            {
                _needValue = m_NeedProgrammingStat.ElementAt(0).Value + m_NeedProgrammingStat.ElementAt(1).Value;
                //m_GameJamResultPanel.FillProagrammingRequirementSlider(_sliderValue, _needValue);
            }
            else
            {
                _needValue = m_NeedProgrammingStat.ElementAt(0).Value;
                //m_GameJamResultPanel.FillProagrammingRequirementSlider(_sliderValue, _needValue);
            }
            m_GameJamResultPanel.ChangeColorFillImage(StudentType.Programming, _sliderValue, _needValue, m_SatisfyStat, m_DissatisfactionStat);
        }
    }

    // �� �ջ��� ������ ���� ����� �����ش�.
    private string CalculateRank(double _totalScore)
    {
        string _successPercent = m_GameJamPanel.m_ExpectedSuccesPercentgetter.text;

        if (_successPercent == "100%")
        {
            if (500 <= _totalScore)
            {
                _rank = "AAA";
            }
            else if (300 <= _totalScore)
            {
                _rank = "AA";

            }
            else if (100 <= _totalScore)
            {
                _rank = "A";

            }
            else if (0 <= _totalScore)
            {
                _rank = "B";
            }
        }
        else
        {
            int _randomNum = UnityEngine.Random.Range(1, 101);

            if (_randomNum > _average)
            {
                _rank = "�̿ϼ�";
            }
            else
            {
                if (500 <= _totalScore)
                {
                    _rank = "AAA";
                }
                else if (300 <= _totalScore)
                {
                    _rank = "AA";

                }
                else if (100 <= _totalScore)
                {
                    _rank = "A";

                }
                else if (0 <= _totalScore)
                {
                    _rank = "B";
                }
            }
        }

        return _rank;
    }

    // �ʼ� ������ 1.8�� �����ְ� �ƴ� ������ 0.4�� �����ش�.
    private void CalculateStat(List<Student> _list, Dictionary<string, int> _partData, List<SaveGameJamData> _saveData, StudentType _type)
    {
        for (int i = 0; i < _list.Count;)
        {
            if (_list[i].m_StudentStat.m_StudentName == _saveData[0].m_StudentData.m_StudentStat.m_StudentName)
            {
                FindPartDataStat(_partData, _list, i, _type);
                _list[i].m_StudentStat.m_NumberOfEntries = 1;
                return;
            }
            i++;
        }
    }

    // ���� ������ �л��� ������ ������ش�.
    private void FindPartDataStat(Dictionary<string, int> _partData, List<Student> _list, int _listIndex, StudentType _type)
    {
        for (int j = 0; j < _partData.Count; j++)
        {
            if (_partData.ElementAt(j).Value != 0)
            {
                string _statName = _partData.ElementAt(j).Key;
                int _tempIndex = 0;

                for (int i = 0; i < AbilityNameList.Length; i++)
                {
                    if (_statName == AbilityNameList[i])
                    {
                        _tempIndex = i;
                        break;
                    }
                }

                if (_type == StudentType.GameDesigner)
                {
                    Funny += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 1.8; // ���ٷ� ����� ������ funny�� ��Ÿ �� �迭�� �ٲٱ�
                }
                else if (_type == StudentType.Art)
                {
                    Graphic += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 1.8;
                }
                else
                {
                    Perfection += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 1.8;
                }
            }
            else
            {
                string _statName = _partData.ElementAt(j).Key;
                int _tempIndex = 0;

                for (int i = 0; i < AbilityNameList.Length; i++)
                {
                    if (_statName == AbilityNameList[i])
                    {
                        _tempIndex = i;
                        break;
                    }
                }

                if (_type == StudentType.GameDesigner)
                {
                    Funny += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 0.4; // ���ٷ� ����� ������ funny�� ��Ÿ �� �迭�� �ٲٱ�
                }
                else if (_type == StudentType.Art)
                {
                    Graphic += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 0.4;
                }
                else
                {
                    Perfection += _list[_listIndex].m_StudentStat.m_AbilityAmountList[_tempIndex] * 0.4;
                }
            }
        }
        Funny = Math.Truncate(Funny);
        Graphic = Math.Truncate(Graphic);
        Perfection = Math.Truncate(Perfection);
    }

    // ���õ� �л����� ������ ������ �帣�� ���ϴ� �Լ�
    private void DecideGenre()
    {
        List<GenreValuePair> _GMGenreList = SortGenreDic(m_GMStudentData);
        List<GenreValuePair> _ArtGenreList = SortGenreDic(m_ArtStudentData);
        List<GenreValuePair> _ProgrammingGenreList = SortGenreDic(m_ProgrammingStudentData);

        _genreBonusScore = 15;
        m_GenreScore = 0;

        List<GenreValuePair> _list = new List<GenreValuePair>();

        for (int i = 0; i < 2; i++)
        {
            _list.Add(_GMGenreList[i]);
        }

        for (int i = 2; i < 4; i++)
        {
            _list.Add(_ArtGenreList[i]);
        }

        for (int i = 4; i < 6; i++)
        {
            _list.Add(_ProgrammingGenreList[i]);
        }

        _list.Sort((x, y) =>
        {
            if (x.value > y.value) return -1;
            else if (x.value < y.value) return 1;
            else if (x.genre < y.genre) return -1;
            else if (x.genre > y.genre) return 1;
            else return 0;
        });

        FindStudentGenreStat(_GMGenreList, _list[0].genre);
        FindStudentGenreStat(_ArtGenreList, _list[0].genre);
        FindStudentGenreStat(_ProgrammingGenreList, _list[0].genre);

        m_GameJamPanel.ChangeExpectedGenreText(_list[0].genre);

        _genreName = ChangeGenreListName(_list[0].genre);

        if (_genreName == m_SaveGameJamData.m_GameJamInfoData.m_GjamMainGenre)
        {
            _genreBonusScore *= 1.4f;
        }

        if (_genreName == m_SaveGameJamData.m_GameJamInfoData.m_GjamSubGenre)
        {
            _genreBonusScore *= 1.1f;
        }
    }

    // ���� ���� �帣�� �л� �帣 ������ �����ֱ� ���� �Լ�
    private void FindStudentGenreStat(List<GenreValuePair> _studentGenreList, GenreStat _genreName)
    {
        for (int i = 0; i < _studentGenreList.Count; i++)
        {
            if (_genreName == _studentGenreList[i].genre)
            {
                m_GenreScore += _studentGenreList[i].value;
                break;
            }
        }
    }

    // enumŬ������ �Ǿ��ִ� �帣�� ������ panel�� ������ string���� �������ִ� �Լ�
    private string ChangeGenreListName(GenreStat _genreName)
    {
        string _name = "???";

        switch (_genreName)
        {
            case GenreStat.Action:
            {
                _name = "�׼�";
            }
            return _name;

            case GenreStat.Simulation:
            {
                _name = "�ùķ��̼�";
            }
            return _name;

            case GenreStat.Adventure:
            {
                _name = "��庥��";
            }
            return _name;

            case GenreStat.Shooting:
            {
                _name = "����";
            }
            return _name;

            case GenreStat.RPG:
            {
                _name = "RPG";
            }
            return _name;

            case GenreStat.Puzzle:
            {
                _name = "����";
            }
            return _name;

            case GenreStat.Rhythm:
            {
                _name = "����";
            }
            return _name;

            case GenreStat.Sports:
            {
                _name = "������";
            }
            return _name;
        }

        return _name;
    }

    // �л����� �帣�� ���� ���� ������ �����ؼ� ����Ʈ�� �־��ش�.
    private List<GenreValuePair> SortGenreDic(List<SaveGameJamData> _saveData)
    {
        //Dictionary<string, int> _returnGenreValue = new Dictionary<string, int>();  // ������ �帣�� ��ȯ���� ��ųʸ�

        //Dictionary<string, int> _tempGenreValue = new Dictionary<string, int>();    // �帣�� �����ϱ� ���� ��� ��Ƶ� ��ųʸ�

        List<GenreValuePair> _list = new List<GenreValuePair>();

        for (int i = 0; i < 8; ++i)
        {
            GenreValuePair _temp = new GenreValuePair();
            _temp.genre = (GenreStat)i;
            _temp.value = _saveData[0].m_StudentData.m_StudentStat.m_GenreAmountList[i];
            _list.Add(_temp);
        }

        _list.Sort((x, y) =>
        {
            if (x.value > y.value) return -1;
            else if (x.value < y.value) return 1;
            else if (x.genre < y.genre) return -1;
            else if (x.genre > y.genre) return 1;
            else return 0;
        });
        return _list;
    }

    // �л����� ���Ȱ� �ʼ������� ����Ͽ� �ۼ�Ʈ�� ���� �ϼ� Ȯ���� ������ش�.
    private void DeterminesRank()
    {
        double _GMStat = (m_NeedGMStat.Count > 1) ? m_NeedGMStat.ElementAt(0).Value + m_NeedGMStat.ElementAt(1).Value : m_NeedGMStat.ElementAt(0).Value;

        double _ArtStat = (m_NeedArtStat.Count > 1) ? m_NeedArtStat.ElementAt(0).Value + m_NeedArtStat.ElementAt(1).Value : m_NeedArtStat.ElementAt(0).Value;

        double _ProgrammingStat = (m_NeedProgrammingStat.Count > 1) ? m_NeedProgrammingStat.ElementAt(0).Value + m_NeedProgrammingStat.ElementAt(1).Value : m_NeedProgrammingStat.ElementAt(0).Value;

        double _GMStudentStat = m_Requirement1List[(int)StudentType.GameDesigner] + m_Requirement2List[(int)StudentType.GameDesigner];
        double _ArtStudentStat = m_Requirement1List[(int)StudentType.Art] + m_Requirement2List[(int)StudentType.Art];
        double _ProgrammingStudentStat = m_Requirement1List[(int)StudentType.Programming] + m_Requirement2List[(int)StudentType.Programming];

        //  80%�� ������ 100%�� ó��
        double _GMPercent = (_GMStudentStat / _GMStat * 100 >= 80) ? 100 : _GMStudentStat / _GMStat * 100;
        double _ArtPercent = (_ArtStudentStat / _ArtStat * 100 >= 80) ? 100 : _ArtStudentStat / _ArtStat * 100;
        double _ProgrammingPercent = (_ProgrammingStudentStat / _ProgrammingStat * 100 >= 80) ? 100 : _ProgrammingStudentStat / _ProgrammingStat * 100;

        double _totalPercent = _GMPercent + _ArtPercent + _ProgrammingPercent;
        _average = _totalPercent / 3;

        // �ϳ��� 80�� ���̸� �ϼ� Ȯ���� ����ְ� �ƴ϶�� 100���� ����ֱ�
        if (_GMPercent < 80 || _ArtPercent < 80 || _ProgrammingPercent < 80)
        {
            _average = Math.Truncate(_average);
            m_GameJamPanel.ChangeExpectedSuccess(_average.ToString() + "%");
        }
        else
        {
            m_GameJamPanel.ChangeExpectedSuccess("100%");
        }
    }

    // ���� �Է��� �̸��� ������ ���� �����߿� �ִٸ� �̹� �ִ� �̸��̶�� �޼����� ����ش�.
    public void CheckName()
    {
        for (int i = 0; i < m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (m_GameJamResultPanel._changeGameName == m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName)
                {
                    m_OnNoticePanel.TurnOnUI();
                    m_OffNoticePanel.DelayTurnOffUI();
                }
                else
                {
                    m_GameJamResultPanel.EndEditName();
                }
            }
        }
    }

    // ��� ��ư�� ������ �� ��Ÿ�� �ֵ�
    public void ClickRankButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        _rankButtonName = _currentObj.name;

        bool _isActive = m_GameJamListPanel.IsInfoPanelActive();

        if (!_isActive)
        {
            switch (_rankButtonName)
            {
                case "All":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                    Debug.Log("��ü��ư");
                }
                break;

                case "AAA":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                    Debug.Log("AA��ư");
                }
                break;

                case "AA":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "A":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "B":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "�̿ϼ�":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                default:
                {

                }
                break;

            }
        }
    }

    // �帣 ��ư�� ������ �� ��Ÿ�� �ֵ�
    public void ClickGenreButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        string _buttonName = _currentObj.name;

        _genreButtonName = _currentObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;// ��ư�� �޸� �ѱ� �̸��� �����´�.

        m_GameJamListPanel._genreButtonName.text = _genreButtonName;

        bool _isActive = m_GameJamListPanel.IsInfoPanelActive();

        if (!_isActive)
        {
            switch (_buttonName)
            {
                case "All":
                {
                    FindRankAndGenre(_rankButtonName, "All");
                    Debug.Log("��ü��ư");
                }
                break;

                case "Action":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                    Debug.Log("AA��ư");
                }
                break;

                case "Simulation":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "Adventure":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "Shooting":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "RPG":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "Puzzle":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "Rythm":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                case "Sport":
                {
                    FindRankAndGenre(_rankButtonName, _genreButtonName);
                }
                break;

                default:
                {

                }
                break;

            }
        }

    }

    // ��ް� �帣���� ���� ������ ���� �������� ����� ������� �� �´� ���,�帣�� ������ ��� ������ֱ� ���� �Լ�
    private void FindRankAndGenre(string _findRank, string _findGenre)
    {
        m_GameJamListPanel.DestroyObj();

        if (_findGenre == null || _findGenre == "��ü")
        {
            _findGenre = "All";
        }

        if (_findRank == null)
        {
            _findRank = "All";
        }

        for (int i = 0; i < m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (_findGenre == "All" && _findRank == "All")
                {
                    MakePreGameJamList();
                }
                else if (_findRank != "All" && _findGenre == "All")
                {
                    if (m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank == _findRank)
                    {
                        GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                        m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                        string _gameName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                        string _genreName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre;
                        string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank;
                        Sprite _genre = FindGenreSprite(_genreName);

                        _gameJamListPrefab.name = _gameName;

                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

                        GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.gameObject;
                        //m_GameJamListPanel.changePrefabContent(_gameName, _genreName, _rank);
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(() => MakePreGameJamInfo(_gameList));
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(m_GameJamListPanel.ClickGameJamPrefab);
                    }
                }
                else if (_findRank == "All" && _findGenre != "All")
                {
                    if (m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre == _findGenre)
                    {
                        GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                        m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                        string _gameName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                        string _genreName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre;
                        string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank;
                        Sprite _genre = FindGenreSprite(_genreName);

                        _gameJamListPrefab.name = _gameName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

                        GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.gameObject;
                        //m_GameJamListPanel.changePrefabContent(_gameName, _genreName, _rank);
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(() => MakePreGameJamInfo(_gameList));
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(m_GameJamListPanel.ClickGameJamPrefab);
                    }
                }
                else if (_findRank != "All" && _findGenre != "All")
                {
                    if (m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre == _findGenre && m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank == _findRank)
                    {
                        GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                        m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                        string _gameName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                        string _genreName = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Genre;
                        string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank;
                        Sprite _genre = FindGenreSprite(_genreName);

                        _gameJamListPrefab.name = _gameName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

                        GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.gameObject;
                        //m_GameJamListPanel.changePrefabContent(_gameName, _genreName, _rank);
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(() => MakePreGameJamInfo(_gameList));
                        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick.AddListener(m_GameJamListPanel.ClickGameJamPrefab);
                    }
                }
            }
        }
    }

    // �����ߴ� �л��� ü���� ����, ������ �ٲ��ش�.
    private void ChangeStudentData(string _name, int[] _ability)
    {
        List<Student> _studentList = ObjectManager.Instance.m_StudentList;

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_name == _studentList[i].m_StudentStat.m_StudentName)
            {
                for (int j = 0; j < 5; j++)
                {
                    _studentList[i].m_StudentStat.m_AbilityAmountList[j] += _ability[j];
                }
                _studentList[i].m_StudentStat.m_Health += m_SaveGameJamData.m_GameJamInfoData.m_StudentHealth;
                _studentList[i].m_StudentStat.m_Passion += m_SaveGameJamData.m_GameJamInfoData.m_StudentPassion;
            }
        }
    }

    private void SetRiseStat(string _rank, int[] _arr, int[] _riseArr)
    {
        switch (_rank)
        {
            case "AAA":
            {
                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _riseArr[i] = 0;
                    }
                    else
                    {
                        _riseArr[i] = 15;
                    }
                }
            }
            break;

            case "AA":
            {
                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _riseArr[i] = 0;
                    }
                    else
                    {
                        _riseArr[i] = 10;
                    }
                }
            }
            break;

            case "A":
            {
                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _riseArr[i] = 0;
                    }
                    else
                    {
                        _riseArr[i] = 5;
                    }
                }
            }
            break;

            case "B":
            {
                for (int i = 0; i < _arr.Length; i++)
                {
                    if (_arr[i] == 0)
                    {
                        _riseArr[i] = 0;
                    }
                    else
                    {
                        _riseArr[i] = 3;
                    }
                }
            }
            break;

            case "�̿ϼ�":
            {
                for (int i = 0; i < _arr.Length; i++)
                {
                    _riseArr[i] = 0;
                }
            }
            break;
        }
    }

    private int FindRewardToDifficulty(int difficulty, string rank = "B")
    {
        foreach (GameJamReward _reward in m_GameJamRewardList)
        {
            if (_reward.Difficulty == difficulty && _reward.GameJamRank == rank)
            {
                return _reward.Reward;
            }
        }
        return 0;
    }
}