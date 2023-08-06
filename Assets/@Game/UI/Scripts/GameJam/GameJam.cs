using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


struct GenreValuePair
{
    public GenreStat genre;
    public int value;
}

/// <summary>
/// �������� ������� �� ���� �������� �����߳� � �л����� �����뿡 ������ �� �ϴ� 
/// �������� ������ִ� Ŭ����
/// 
/// 2023.03.06 Ocean
/// </summary>
public class GameJam : MonoBehaviour
{
    enum ResultConcept
    {
        ���������,
        �볭��,
        �̼ҳ�,
        �̷�����,
        �������,
        ����,
        �ܰ���,
        ����������,
        �巡��,
        ������ũ,
        �ð�����,
        ����ǽ�,
        �뷡��,
        ���̵�,
        �౸,
        �±ǵ�,
        ��,
        ��,
        ��ġ�Ƿ�,
        ��Ÿ��,
        ����������
    }

    public delegate void ActivateGameShowButtonEventHandler(bool activate);

    public static event ActivateGameShowButtonEventHandler OnActivateButtonHandler;

    public delegate void DataChangedEventHandler();

    public static event DataChangedEventHandler DataChangedEvent;

    public delegate void SkillConditionDataChangedEventHandler();

    public static event SkillConditionDataChangedEventHandler SkillConditionDataChangedEvent;

    public static string[] GenreNameList = new string[] { "�׼�", "�ùķ��̼�", "��庥��", "����", "RPG", "����", "����", "������" };
    public static string[] AbilityNameList = new string[] { "����", "����", "����", "���", "��ġ" };

    public int[] RiseGameDesignerStatList = new int[5];                                                                     // ��޿� ���� ��Ʈ�� ��� ����
    public int[] RiseArtStatList = new int[5];                                                                              // 
    public int[] RiseProgrammingStatList = new int[5];                                                                      //

    public int[] GameDesignerRequirementStatList = new int[5];
    public int[] ArtRequirementStatList = new int[5];
    public int[] ProgrammingRequirementStatList = new int[5];

    private List<GameJamInfo> m_DummyGameJamList = new List<GameJamInfo>();                                                 // ���������͸� ����ִ� ����Ʈ �߿��� ���ǿ� �ش��ϴ� ������ ��� ����ִ� ����Ʈ

    private static List<GameJamInfo> m_FixedGameJam = new List<GameJamInfo>();
    private static List<GameJamInfo> m_RandomGameJam = new List<GameJamInfo>();

    private Dictionary<string, int> m_GameJamEntryCount = new Dictionary<string, int>();                                    // �� �����뿡 �� ���� �����ߴ��� 

    private Dictionary<string, int> m_NeedGameDesignerStat = new Dictionary<string, int>();                                 // �ʼ� ������ �̸��� ���� �����ص־��Ѵ�.
    private Dictionary<string, int> m_NeedArtStat = new Dictionary<string, int>();
    private Dictionary<string, int> m_NeedProgrammingStat = new Dictionary<string, int>();

    private GameJamSaveData m_RunningGameJameData = new GameJamSaveData();                                                  // ������ �޿� �������� ��������� �� �ʿ��� ����

    private GameJamInfo m_NowGameJamInfo = new GameJamInfo();                                                               // �̹��� ������ ������ ���� �����͵�

    private List<GameJamSaveData> m_ToBeRunning = new List<GameJamSaveData>();                                              // �̹��޿� �����ؼ� �����޿� ����� ������ ����

    private List<GameDifficulty> m_GameJamDifficultiesList = new List<GameDifficulty>();                                    // ������ ���̵��� �Ǻ����ֱ� ���� ������ �־� �� ����Ʈ

    private List<GameJamReward> m_GameJamRewardList = new List<GameJamReward>();

    private Student[] m_ClickStudent = new Student[3];

    public static Dictionary<int, List<GameJamSaveData>> m_GameJamHistory = new Dictionary<int, List<GameJamSaveData>>();   // �������� �ѹ� ������ �Ǹ� ���⿡ ����.  ���ݱ����� ��� ������ ��� (�ϼ�ǰ��)                  

    private Queue<Action> eventQueue = new Queue<Action>();                                                                 // �������� ����ǰ� ���� ���Ӽ ������ �غ� ������Ѵ�. timescale�� 0�� �� �̺�Ʈ�� �����ϴ� �ȵż� Update������ timescale�� 0 ���� Ŭ �� ��������ִ°ɷ� ����

    [SerializeField] private ActivityEvent m_ActivityEvent;
    [SerializeField] private SoundManager m_SoundManager;
    [SerializeField] private Sprite m_NotEntry;
    [SerializeField] private Sprite m_EntryComplete;
    [SerializeField] private Sprite m_Entry;
    [SerializeField] private Sprite m_SatisfyStat;
    [SerializeField] private Sprite m_DissatisfactionStat;
    [SerializeField] private Sprite m_UpArrow;
    [SerializeField] private Sprite m_DownArrow;
    [SerializeField] private Sprite[] m_RequirementStats;
    [SerializeField] private Sprite[] m_GenreSprite;
    [SerializeField] private Sprite[] m_ResultConceptSprite;
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_GameJamButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;

    private Sprite m_NoneArrow;
    private bool m_IsGameJamEventCreation;                                  // ������ �����ϸ� �������� �ѹ��� ���� �� �ְ� ���ִ� bool��
    private bool m_IsGameJamStart;
    private bool m_IsFirstGameJam;
    private int m_Year;
    private int m_Month;
    private int m_Day;
    private string m_PartName;                                              // �����뿡�� �л����� ������ �� �����Ʈ �л����� Ȯ�����ִ� ����, ��� �������� ������ �ٽ� ��ȹ���� �ٲ��ֱ�
    private string _warningMessage;
    private int m_Requirement1 = 0;
    private int m_Requirement2 = 0;
    private int[] m_Requirement1List = new int[3];                          // ������ �л����� ������ �ִ� �ʼ� ������ ����
    private int[] m_Requirement2List = new int[3];

    private int m_MonthLimit;                                               // �Ѵ޿� 2���ۿ� ������ �� ������
    private int GameJamEntryCoolTimeCount;                                  // �Ѵ޿� 2���ۿ� ������ �� ������
    private bool m_GameJamEntryCoolTime;                                    // �ѹ� �����뿡 �����ϸ� 2�� ���� �д�.
    private int m_EnteryCount;                                              // �Ȱ��� �����뿡 �� �� ���� �ߴ��� 
    private int m_EntryGameJamDifficulty;
    private float m_HighEntryFee;
    private float m_MiddleEntryFee;
    private double Funny;
    private double Graphic;
    private double Perfection;
    private double _genreBonusScore;
    private double m_GenreScore;
    private double m_TotalGenreScore;
    private int m_MiniGameScore;
    private string _rank;                                                   // �̹� �����뿡�� ���� ��ũ
    private double _average;                                                // �̹� ������ ��� ����
    private string _genreName;                                              // ���� ������ �帣�� �������ֱ� ���� ��� �־�δ� ����
    private string _gameJameName;                                           // ���̸���Ʈ�� �ִ� ��� �� ������ �����ߴ��� �����صδ� ����
    private string _rankButtonName;                                         // ������ ���� ������ ��ũ ��ư�� ������ ��ư�� �̸��� �־��ִ� ����
    private string _genreButtonName;                                        // ������ ���� ������ �帣 ��ư�� ������ ��ư�� �̸��� �־��ִ� ����

    private int m_TutorialCount;
    private int m_ScriptCount;
    private int m_DifficultyMiddle;
    private int m_DifficultyHigh;

    private GameObject m_PrevClickGameJamObj;                                   // ������ ��ư ��������Ʈ ������ ���� ���� Ŭ���� ������ ������Ʈ�� ��Ƶ� ����
    private GameObject m_PrevRankButtonObj;
    private Color m_HighLightColor;
    private Color m_RankButtonHighLightColor;
    public GameObject m_SliderBarArlam;
    public GameObject m_GameJamArlam;
    public GameObject m_GameJamPrefab;
    public GameObject m_StudentPrefab;
    public GameObject m_GameJamListPrefab;
    public GameJamSelect_Panel m_GameJamPanel;
    public GameJamResult m_GameJamResultPanel;
    public FinalGameJamResult m_FinalGameJamResultPanel;
    public GameJamList m_GameJamListPanel;
    public GameJamTimer m_Timer; // �������� ���� �� �� ������ Ÿ�̸�
    public SlideMenuPanel m_Slider;
    public PopUpUI m_PopUpResultPanel;
    public PopOffUI m_PopOffResultPanel;

    public PopUpUI m_OnNoticePanel;
    public PopOffUI m_OffNoticePanel;
    public int MiniGameScore { get { return m_MiniGameScore; } set { m_MiniGameScore = value; } }
    public Sprite[] RestultSprite { get { return m_ResultConceptSprite; } }

    public static GameJamInfo SearchAllGameJamInfo(int ID)
    {
        foreach (var _random in m_RandomGameJam)
        {
            if (ID == _random.m_GjamID)
            {
                return _random;
            }
        }

        foreach (var _fixed in m_FixedGameJam)
        {
            if (ID == _fixed.m_GjamID)
            {
                return _fixed;
            }
        }

        Debug.Log("������ ���� gamejam ID");

        return null;
    }

    private void Start()
    {
        m_IsGameJamEventCreation = false;
        m_GameJamEntryCoolTime = false;
        m_NoneArrow = null;
        m_PartName = "��ȹ";
        m_MonthLimit = 2;
        GameJamEntryCoolTimeCount = 0;
        m_GameJamHistory.Clear();
        m_FixedGameJam.Clear();
        m_RandomGameJam.Clear();

        InitDifficulyList();
        InitRewardList();
        ClassifyGameJamData();
        m_TutorialCount = 0;
        m_ScriptCount = 0;
        m_DifficultyMiddle = 10;
        m_DifficultyHigh = 20;
        m_HighEntryFee = 2.4f;
        m_MiddleEntryFee = 1.2f;
        m_HighLightColor = new Color(1f, 0.8475146f, 0, 1f);
        m_RankButtonHighLightColor = new Color(1f, 0.7679189f, 0f, 1f);

        m_NextButton.onClick.AddListener(TutorialContinue);
        m_NextButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);

        if (Json.Instance.UseLoadingData)
        {
            DistributeGameJamData();
        }
    }

    private void Update()
    {
        if (m_Month != GameTime.Instance.FlowTime.NowMonth)
        {
            m_IsGameJamEventCreation = false;
            m_MonthLimit = 2;
            m_DummyGameJamList.Clear();
            m_GameJamPanel.DestroyGameJamListObj();
            SetActiveAlram(false);
        }

        if (eventQueue.Count > 0 && Time.timeScale >= 1f)
        {
            eventQueue.Dequeue()?.Invoke();
            m_IsGameJamStart = false;
        }

        if (m_ToBeRunning.Count != 0 && !m_IsGameJamStart)
        {
            for (int i = 0; i < m_ToBeRunning.Count; i++)
            {
                if (!m_ActivityEvent.m_IsCheckGameJam)
                {
                    m_RunningGameJameData = m_ToBeRunning[i];
                    m_ActivityEvent.m_IsCheckGameJam = true;
                    m_IsGameJamStart = true;
                    m_PopUpResultPanel.TurnOnUI();
                }
            }
        }

        // �� �ϸ��� ��¥���� �������ش�.
        if (m_Day != GameTime.Instance.FlowTime.NowDay)
        {
            if (m_GameJamEntryCoolTime == true)
            {
                if (GameJamEntryCoolTimeCount == 2)
                {
                    m_GameJamEntryCoolTime = false;
                    GameJamEntryCoolTimeCount = 0;
                }
                else
                {
                    GameJamEntryCoolTimeCount++;
                }
            }

            m_Year = GameTime.Instance.FlowTime.NowYear;
            m_Month = GameTime.Instance.FlowTime.NowMonth;
            m_Day = GameTime.Instance.FlowTime.NowDay;
        }

        // 1���� ������ 3���� �Ǹ� ���ο� �л����� ������ ������ ���� ���ӵ��� ��� �����ش�.
        if (m_Year != GameTime.Instance.FlowTime.NowYear && m_Month == 3)
        {
            m_GameJamHistory.Clear();
        }

        // ó���� ������ ��ư�� Ȱ��ȭ���� �ʵ��� �ؾ��ϴµ�...
        if (!m_IsGameJamEventCreation && PlayerInfo.Instance.IsFirstClassEnd && m_Month != 2)
        {
            if (m_GameJamButton.GetComponent<Button>().interactable == false)
            {
                m_GameJamButton.GetComponent<Button>().interactable = true;
            }

            m_IsGameJamEventCreation = true;

            BringFixedGameJamData();
            BringRandomGameJamData();

            if (m_DummyGameJamList.Count != 0)
            {
                MakeGameJamList();

                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 0 &&
                    PlayerInfo.Instance.IsFirstAcademySetting)
                {
                    Time.timeScale = 0;

                    m_TutorialPanel.SetActive(true);
                    m_PDAlarm.SetActive(true);
                    m_Unmask.gameObject.SetActive(false);
                    m_TutorialTextImage.gameObject.SetActive(false);
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_NextButton.gameObject.SetActive(true);
                    m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
        }
        //#if UNITY_EDITOR || UNITY_EDITOR_WIN
        //        if (PlayerInfo.Instance.IsFirstGameJam && Input.GetMouseButtonDown(0) && m_TutorialCount > 0)
        //        {
        //            TutorialContinue();
        //            ClickEventManager.Instance.Sound.PlayIconTouch();
        //        }
        //#elif UNITY_ANDROID
        //        if (PlayerInfo.Instance.IsFirstGameJam  && Input.touchCount == 1 && m_TutorialCount > 0)
        //        {
        //            Touch touch = Input.GetTouch(0); 
        //            if (PlayerInfo.Instance.IsFirstGameJam && touch.phase == TouchPhase.Ended)
        //            {
        //                TutorialContinue();
        //                ClickEventManager.Instance.Sound.PlayIconTouch();
        //            }
        //        }

        //#endif
    }

    private void TutorialContinue()
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
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 6)
        {
            m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = false;
            m_GameJamPanel.GameJamListParentObj.transform.GetChild(0).GetComponent<Button>().interactable = true;
            m_GameJamPanel.GameJamListParentObj.transform.GetChild(1).GetComponent<Button>().interactable = true;
            m_GameJamPanel.GameJamListParentObj.transform.GetChild(2).GetComponent<Button>().interactable = true;
            m_GameJamPanel.GameJamListParentObj.transform.GetChild(3).GetComponent<Button>().interactable = true;
            m_GameJamPanel.GameJamListParentObj.transform.GetChild(4).GetComponent<Button>().interactable = true;
            m_Unmask.fitTarget = m_GameJamPanel.GameJamButtonRect;
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 130, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 8)
        {
            m_Unmask.fitTarget = m_GameJamPanel.m_SetStartButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
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
            m_Unmask.fitTarget = m_GameJamPanel.FirstRect.GetComponent<RectTransform>();
            m_GameJamPanel.GameDesignerButton.gameObject.GetComponent<Button>().interactable = true;
            m_GameJamPanel.ArtButton.gameObject.GetComponent<Button>().interactable = true;
            m_GameJamPanel.ProgrammingButton.gameObject.GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(0).GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(1).GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(2).GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(3).GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(4).GetComponent<Button>().interactable = true;
            m_GameJamPanel.SelectStudentParentObj.transform.GetChild(5).GetComponent<Button>().interactable = true;
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 12)
        {
            m_Unmask.fitTarget = m_GameJamPanel.SecondRect.GetComponent<RectTransform>();
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-700, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 13)
        {
            m_Unmask.fitTarget = m_GameJamPanel.ThirdRect.GetComponent<RectTransform>();
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-350, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 14)
        {
            m_Unmask.fitTarget = m_GameJamPanel.PartInfoRect.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-800, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 15)
        {
            m_Unmask.fitTarget = m_GameJamPanel.GameDesignerButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 280, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 24)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 25)
        {
            Time.timeScale = 1;
            m_NextButton.gameObject.SetActive(false);
            m_TutorialPanel.SetActive(false);
            PlayerInfo.Instance.IsFirstGameJam = false;
        }
    }

    public void CollectGameJamData()
    {
        List<GameJamSaveData> runData = new List<GameJamSaveData>(m_ToBeRunning);
        Dictionary<int, List<GameJamSaveData>> history = new Dictionary<int, List<GameJamSaveData>>(m_GameJamHistory);

        AllInOneData.Instance.GameJamToBeRunning = runData;
        AllInOneData.Instance.GameJamHistory = history;

        Dictionary<string, int> count = new Dictionary<string, int>(m_GameJamEntryCount);
        Dictionary<string, int> needGameDesignerStat = new Dictionary<string, int>(m_NeedGameDesignerStat);
        Dictionary<string, int> needArtStat = new Dictionary<string, int>(m_NeedArtStat);
        Dictionary<string, int> needProgrammingStat = new Dictionary<string, int>(m_NeedProgrammingStat);

        PlayerInfo.Instance.GameJamEntryCount = count;
        PlayerInfo.Instance.NeedGameDesignerStat = needGameDesignerStat;
        PlayerInfo.Instance.NeedArtStat = needArtStat;
        PlayerInfo.Instance.NeedProgrammingStat = needProgrammingStat;
        
        // �����뿡�� ���� �����͵�
        PlayerInfo.Instance._genreBonusScore = _genreBonusScore;
        PlayerInfo.Instance._genreName = _genreName;
        PlayerInfo.Instance.m_GenreScore = m_GenreScore;
        PlayerInfo.Instance.m_MiniGameScore = m_MiniGameScore;
    }

    private void DistributeGameJamData()
    {
        List<GameJamSaveData> runData = new List<GameJamSaveData>(AllInOneData.Instance.GameJamToBeRunning);
        Dictionary<int, List<GameJamSaveData>> history =
            new Dictionary<int, List<GameJamSaveData>>(AllInOneData.Instance.GameJamHistory);

        m_ToBeRunning = runData;
        m_GameJamHistory = history;

        Dictionary<string, int> count = new Dictionary<string, int>(AllInOneData.Instance.PlayerData.GameJamEntryCount);
        Dictionary<string, int> needGameDesignerStat = new Dictionary<string, int>(AllInOneData.Instance.PlayerData.NeedGameDesignerStat);
        Dictionary<string, int> needArtStat = new Dictionary<string, int>(AllInOneData.Instance.PlayerData.NeedArtStat);
        Dictionary<string, int> needProgrammingStat = new Dictionary<string, int>(AllInOneData.Instance.PlayerData.NeedProgrammingStat);

        m_GameJamEntryCount = count;
        m_NeedGameDesignerStat = needGameDesignerStat;
        m_NeedArtStat = needArtStat;
        m_NeedProgrammingStat = needProgrammingStat;

        InitRequirementStatArry(needGameDesignerStat, ref GameDesignerRequirementStatList);
        InitRequirementStatArry(needArtStat, ref ArtRequirementStatList);
        InitRequirementStatArry(needProgrammingStat, ref ProgrammingRequirementStatList);

        if (runData.Count != 0)
        {
            for (int i = 0; i < runData.Count; i++)
            {
                GameJamInfo _gamejam = SearchAllGameJamInfo((int)runData[i].m_GameJamID);
                int _month = (int)runData[i].m_MakeMonth;

                if (runData[i].m_MakeWeek == 4 && runData[i].m_MakeDay >= 3)
                {
                    _month += 1;
                }

                GameObject eventPrefab = m_ActivityEvent.MakeDistributeEventPrefab(_gamejam.m_GjamName, (int)runData[i].m_MakeMonth, (int)runData[i].m_MakeWeek, (int)runData[i].m_MakeDay + 3,
                    true, _month);

                string day = eventPrefab.GetComponent<EventPrefab>().DDAyText.text;
                eventPrefab.transform.GetComponent<EventPrefab>().m_IsGameJam = true;

                if (!day.Contains("Day"))
                {
                    day = day.Replace("-", "");
                    if (day == "3")
                        m_GameJamEntryCoolTime = true;
                }

                m_ActivityEvent.SetEventPanelActive(true);

                StartCoroutine(m_ActivityEvent.TypingText(eventPrefab, "������ ������"));
            }
        }
        
        // �����뿡�� ���� �����͵�
        _genreBonusScore = PlayerInfo.Instance._genreBonusScore;
        _genreName = PlayerInfo.Instance._genreName;
        m_GenreScore = PlayerInfo.Instance.m_GenreScore;
        m_MiniGameScore = PlayerInfo.Instance.m_MiniGameScore;
    }

    private void InitRequirementStatArry(Dictionary<string, int> _needStat, ref int[] _requirementStatArry)
    {
        if (_needStat.Count > 1)
        {
            for (int i = 0; i < AbilityNameList.Length; i++)
            {
                if (AbilityNameList[i] == _needStat.ElementAt(0).Key)
                {
                    _requirementStatArry[i] = _needStat.ElementAt(0).Value;
                }

                if (AbilityNameList[i] == _needStat.ElementAt(1).Key)
                {
                    _requirementStatArry[i] = _needStat.ElementAt(1).Value;
                }
            }
        }
        else if (_needStat.Count == 1)
        {
            for (int i = 0; i < AbilityNameList.Length; i++)
            {
                if (AbilityNameList[i] == _needStat.ElementAt(0).Key)
                {
                    _requirementStatArry[i] = _needStat.ElementAt(0).Value;
                }
            }
        }
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
        if (!m_IsFirstGameJam)
        {
            eventQueue.Enqueue(() => DataChangedEvent?.Invoke());
            m_IsFirstGameJam = true;
        }
        eventQueue.Enqueue(() => SkillConditionDataChangedEvent?.Invoke());
    }

    // ���� ������ ������ Ǯ���� ��¥�� �´� �������� ������ �� ��ī���� ���̵��� ���� ������ �ٲ��ش�.
    private void BringFixedGameJamData()
    {
        int difficulty = CheckDifficulties(PlayerInfo.Instance.CurrentRank);

        for (int i = 0; i < m_FixedGameJam.Count; i++)
        {
            if (m_FixedGameJam[i].m_GjamYear == -2 &&
            m_Year % 2 == 0 &&
            m_FixedGameJam[i].m_GjamMonth == m_Month)
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
                     m_FixedGameJam[i].m_GjamMonth == m_Month)
            {
                m_DummyGameJamList.Add(m_FixedGameJam[i]);

                if (!m_GameJamHistory.ContainsKey(m_FixedGameJam[i].m_GjamID))
                {
                    SetActiveAlram(true);
                }
            }
            // 0�� �ų� �����ϴ°Ŵϱ� �ų� �ش��ϴ� �޿� �˶��ֱ�
            else if (m_FixedGameJam[i].m_GjamYear == 0 &&
                     m_FixedGameJam[i].m_GjamMonth == m_Month)
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

        if (m_Year == 1 && m_Month == 3)
        {
            int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamName == "�׼��谨");

            m_DummyGameJamList[_index].m_GjamMonth = 3;
        }

        switch (difficulty)
        {
            // ��
            case 100:
            {
                // �Ѱ��� ���̵��� ��, �� �ϳ��� �����ϰ�
                int _randomNum = UnityEngine.Random.Range(0, m_DummyGameJamList.Count);
                int _difficurly = _randomNum == 0 || _randomNum == 1 ? m_DifficultyMiddle : 0;
                float m_EntryFeeIncrease = difficulty == 0 ? m_HighEntryFee : difficulty == 1 ? m_MiddleEntryFee : 1;

                Dictionary<string, int> m_GameDesignerNeedStat = m_DummyGameJamList[_randomNum]
                    .m_GjamNeedStatGameDesigner
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                m_DummyGameJamList[_randomNum].m_GjamNeedStatGameDesigner = m_GameDesignerNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 || _randomNum == 1 ? 200 : 300;
                m_DummyGameJamList[_randomNum].m_EntryFee *= m_EntryFeeIncrease;

                // ������ �� ���� ������ �����ش�.
                for (int i = 0; i < 2; i++)
                {
                    if (i != _randomNum)
                    {
                        Dictionary<string, int> m_MyDifficurlyGameDesignerNeedStat = m_DummyGameJamList[i]
                            .m_GjamNeedStatGameDesigner.ToDictionary((x => x.Key),
                                (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));
                        Dictionary<string, int> m_MyDifficurlyArtNeedStat = m_DummyGameJamList[i].m_GjamNeedStatArt
                            .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));
                        Dictionary<string, int> m_MyDifficurlyProgrammingNeedStat = m_DummyGameJamList[i]
                            .m_GjamNeedStatProgramming.ToDictionary((x => x.Key),
                                (x => x.Value > 0 ? x.Value + m_DifficultyHigh : x.Value));

                        m_DummyGameJamList[i].m_GjamNeedStatGameDesigner = m_MyDifficurlyGameDesignerNeedStat;
                        m_DummyGameJamList[i].m_GjamNeedStatArt = m_MyDifficurlyArtNeedStat;
                        m_DummyGameJamList[i].m_GjamNeedStatProgramming = m_MyDifficurlyProgrammingNeedStat;
                        m_DummyGameJamList[i].m_GjamAI_ID = 100;
                        m_DummyGameJamList[i].m_EntryFee *= m_EntryFeeIncrease;
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
                    float m_EntryFeeIncrease =
                        difficulty == 0 ? m_HighEntryFee : difficulty == 1 ? m_MiddleEntryFee : 1;

                    Dictionary<string, int> m_GameDesignerNeedStat = m_DummyGameJamList[_randomNum]
                        .m_GjamNeedStatGameDesigner.ToDictionary((x => x.Key),
                            (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                    Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt
                        .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                    Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum]
                        .m_GjamNeedStatProgramming.ToDictionary((x => x.Key),
                            (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                    m_DummyGameJamList[_randomNum].m_GjamNeedStatGameDesigner = m_GameDesignerNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                    m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 ? 100 : _randomNum == 1 ? 200 : 300;
                    m_DummyGameJamList[_randomNum].m_EntryFee *= m_EntryFeeIncrease;
                    GameShow.Swap(m_DummyGameJamList, _randomNum, _listCount - 1);
                    _listCount -= 1;
                }
            }
            break;

            // ��
            case 300:
            {
                int _randomNum = UnityEngine.Random.Range(0, m_DummyGameJamList.Count);
                int _difficurly = _randomNum == 0 || _randomNum == 1 ? m_DifficultyMiddle : m_DifficultyHigh;
                float m_EntryFeeIncrease = difficulty == 0 ? m_HighEntryFee : difficulty == 1 ? m_MiddleEntryFee : 1;

                if (m_DummyGameJamList[_randomNum].m_GjamName == "�׼��谨" && m_Year == 1 && m_Month == 3)
                {
                    _randomNum = UnityEngine.Random.Range(1, m_DummyGameJamList.Count);
                    _difficurly = _randomNum == 1 || _randomNum == 2 ? m_DifficultyMiddle : m_DifficultyHigh;
                    if (_difficurly == m_DifficultyMiddle || _difficurly == m_DifficultyHigh)
                    {
                        _difficurly = 1;
                    }
                }

                Dictionary<string, int> m_GameDesignerNeedStat = m_DummyGameJamList[_randomNum]
                    .m_GjamNeedStatGameDesigner
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ArtNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatArt
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));
                Dictionary<string, int> m_ProgrammingNeedStat = m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming
                    .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + _difficurly : x.Value));

                m_DummyGameJamList[_randomNum].m_GjamNeedStatGameDesigner = m_GameDesignerNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatArt = m_ArtNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
                m_DummyGameJamList[_randomNum].m_GjamAI_ID = _randomNum == 0 || _randomNum == 1 ? 200 : 300;
                m_DummyGameJamList[_randomNum].m_EntryFee *= m_EntryFeeIncrease;
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
            if (m_RandomGameJam[i].m_GjamMonth == m_Month)
            {
                m_MonthRandomList.Add(m_RandomGameJam[i]);
            }
        }

        if (m_MonthRandomList.Count == 0)
            return;

        int _listCount = m_MonthRandomList.Count;
        int _weekend = 5;

        for (int i = 0; i < 2; i++)
        {
            int randomInex = UnityEngine.Random.Range(0, _listCount);
            int randomDifficuly = UnityEngine.Random.Range(0, 3);
            int randomWeekwnd = UnityEngine.Random.Range(1, _weekend);
            int difficuly = randomDifficuly == 0 ? m_DifficultyHigh : randomDifficuly == 1 ? m_DifficultyMiddle : 0;
            float m_EntryFeeIncrease = difficuly == 0 ? m_HighEntryFee : difficuly == 1 ? m_MiddleEntryFee : 1;


            Dictionary<string, int> m_GameDesignerNeedStat = m_MonthRandomList[randomInex].m_GjamNeedStatGameDesigner
                .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));
            Dictionary<string, int> m_ArtNeedStat = m_MonthRandomList[randomInex].m_GjamNeedStatArt
                .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));
            Dictionary<string, int> m_ProgrammingNeedStat = m_MonthRandomList[randomInex].m_GjamNeedStatProgramming
                .ToDictionary((x => x.Key), (x => x.Value > 0 ? x.Value + difficuly : x.Value));

            m_MonthRandomList[randomInex].m_GjamNeedStatGameDesigner = m_GameDesignerNeedStat;
            m_MonthRandomList[randomInex].m_GjamNeedStatArt = m_ArtNeedStat;
            m_MonthRandomList[randomInex].m_GjamNeedStatProgramming = m_ProgrammingNeedStat;
            m_MonthRandomList[randomInex].m_GjamYear = m_Year;
            m_MonthRandomList[randomInex].m_GjamAI_ID = randomDifficuly == 0 ? 100 : randomDifficuly == 1 ? 200 : 3000;
            m_MonthRandomList[randomInex].m_EntryFee *= m_EntryFeeIncrease;
            m_DummyGameJamList.Add(m_MonthRandomList[randomInex]);

            if (!m_GameJamHistory.ContainsKey(m_MonthRandomList[randomInex].m_GjamID))
            {
                SetActiveAlram(true);
            }

            GameShow.Swap(m_MonthRandomList, randomInex, _listCount - 1);
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

        m_GameJamPanel.m_GameDesignerName.text = "";
        m_GameJamPanel.m_ArtName.text = "";
        m_GameJamPanel.m_ProgrammingName.text = "";
    }

    // Ȩ��ư�� ������ �� �˶�ǥ�ø� ���ֱ� ���� �Լ�
    public void SetActiveAlram(bool _isFirstAlram = false)
    {
        m_SliderBarArlam.SetActive(_isFirstAlram);
        m_GameJamArlam.SetActive(_isFirstAlram);
    }

    // Ȩ��ư�� ������ ���� ȭ������ ���ٸ� ���� �����ϰ� �ִ� �����鵵 �ʱ�ȭ ��������Ѵ�.
    // �������� �� �ϰ� ���� ������ ���� �� �Լ� ����ϱ�
    public void ClickHomeButtonYes()
    {
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

                //if (_list[i].m_StudentStat.m_UserSettingName != "")
                //{
                //    _studentInfo.name = _list[i].m_StudentStat.m_UserSettingName;
                //}
                //else
                //{
                //}
                _studentInfo.GetComponent<Image>().sprite = _list[i].StudentProfileImg;
                _studentInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    _list[i].m_StudentStat.m_NumberOfEntries.ToString();

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
            else if (x.m_StudentStat.m_AbilityAmountArr[_tempIndex] >
                     y.m_StudentStat.m_AbilityAmountArr[_tempIndex]) return -1;
            else if (x.m_StudentStat.m_AbilityAmountArr[_tempIndex] <
                     y.m_StudentStat.m_AbilityAmountArr[_tempIndex]) return 1;
            else return 0;
        });

        MakeStudent(_list, _type);
    }

    // �л��� �ʼ� ������ ���� ū ������� �����ϴ� �Լ�. ���� �ʼ� ������ �� ����� �� ū ģ���� �������� �ϱ� ���� �񱳸� �Ѵ�.
    private void SortList(StudentType _type)
    {
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        Dictionary<string, int> _dict = null;
        if (_type == StudentType.GameDesigner) _dict = m_NeedGameDesignerStat;
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
            if (m_GameJamEntryCoolTime == true)
            {
                _warningMessage = "���� �ٸ� �������� �������Դϴ�!";
                m_GameJamPanel.SetActiveEntryCountWarningPanel(true, _warningMessage);
                StartCoroutine(SetEntryWarningPanel());

                return;
            }

            if (m_ToBeRunning.Count != 0)
            {
                GameJamInfo _info = SearchAllGameJamInfo((int)m_ToBeRunning[0].m_GameJamID);

                SetActiveAlram(false);

                for (int i = 0; i < m_DummyGameJamList.Count; i++)
                {
                    if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                    {
                        m_NowGameJamInfo = m_DummyGameJamList[i];
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
                    m_GameJamPanel.GameDesignerButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.ArtButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.ProgrammingButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(1).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(2).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(5).GetComponent<Button>().interactable = false;
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_NextButton.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(700, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
            else
            {
                SetActiveAlram(false);

                for (int i = 0; i < m_DummyGameJamList.Count; i++)
                {
                    if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                    {
                        m_NowGameJamInfo = m_DummyGameJamList[i];
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
                    m_GameJamPanel.GameDesignerButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.ArtButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.ProgrammingButton.gameObject.GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(1).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(2).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.SelectStudentParentObj.transform.GetChild(5).GetComponent<Button>().interactable = false;
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_NextButton.gameObject.SetActive(true);
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
            case "GameDesignerButton":
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

        if (PlayerInfo.Instance.IsFirstGameJam &&
            (m_TutorialCount == 16 || m_TutorialCount == 18 || m_TutorialCount == 20))
        {
            m_Unmask.fitTarget = m_GameJamPanel.m_StudentInfoParent.GetChild(0).GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 170, 0);
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
                if (_list[_index].m_StudentStat.m_StudentName == m_GameJamPanel.m_GameDesignerName.text)
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

    /// TODO : ���� �κ� �����ϱ�. �л��� ���°Ŷ� ���â ���°Ŷ� ������ �������
    private int FindStudent(StudentType _type, bool _isResult, int _stat1 = 0, int _stat2 = 0)
    {
        int _sliderValue = 0;

        if (_isResult == false)
        {
            for (int i = 0; i < (int)StudentType.Count; i++)
            {
                if ((int)_type == i)
                {
                    _sliderValue = m_Requirement1List[i] + m_Requirement2List[i];
                    break;
                }
            }
        }
        else
        {
            _sliderValue = _stat1 + _stat2;
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
                        m_ClickStudent[0] = _studentList[i];

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_GameDesignerNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.GameDesignerStudentPanel, true);

                        string _name;
                        if (_studentList[i].m_StudentStat.m_UserSettingName != "")
                        {
                            _name = _studentList[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            _name = _studentList[i].m_StudentStat.m_StudentName;
                        }

                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedGameDesignerStat);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.GameDesigner, _name, _health, _passion,
                            _studentList[i].m_StudentStat.m_AbilityAmountArr, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.GameDesigner, false);

                        m_GameJamPanel.ChangeSlider(StudentType.GameDesigner, _sliderValue);

                        int _needValue = 0;

                        if (m_NeedGameDesignerStat.Count > 1)
                        {
                            _needValue = m_NeedGameDesignerStat.ElementAt(0).Value +
                                         m_NeedGameDesignerStat.ElementAt(1).Value;
                        }
                        else
                        {
                            _needValue = m_NeedGameDesignerStat.ElementAt(0).Value;
                        }

                        m_GameJamPanel.ChangeSliderFillSprite(StudentType.GameDesigner, _sliderValue, _needValue);
                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_ArtNone, m_GameJamPanel.m_ProgrammingNone);
                    }
                    else if (_studentList[i].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        m_ClickStudent[1] = _studentList[i];

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_ArtNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.ArtStudentPanel, true);

                        string _name;

                        if (_studentList[i].m_StudentStat.m_UserSettingName != "")
                        {
                            _name = _studentList[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            _name = _studentList[i].m_StudentStat.m_StudentName;
                        }

                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedArtStat);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.Art, _name, _health, _passion,
                            _studentList[i].m_StudentStat.m_AbilityAmountArr, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.Art, false);

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
                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_GameDesignerNone,
                            m_GameJamPanel.m_ProgrammingNone);
                    }
                    else if (_studentList[i].m_StudentStat.m_StudentType == StudentType.Programming)
                    {
                        m_ClickStudent[2] = _studentList[i];

                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.m_ProgrammingNone, false);
                        m_GameJamPanel.SetActiveObj(m_GameJamPanel.ProgrammingStudentPanel, true);

                        string _name;
                        if (_studentList[i].m_StudentStat.m_UserSettingName != "")
                        {
                            _name = _studentList[i].m_StudentStat.m_UserSettingName;
                        }
                        else
                        {
                            _name = _studentList[i].m_StudentStat.m_StudentName;
                        }

                        string _health = _studentList[i].m_StudentStat.m_Health.ToString();
                        string _passion = _studentList[i].m_StudentStat.m_Passion.ToString();

                        StudentNeedStat(_studentList, i, m_NeedProgrammingStat);

                        m_GameJamPanel.ChangeStudentInfo(StudentType.Programming, _name, _health, _passion,
                            _studentList[i].m_StudentStat.m_AbilityAmountArr, _studentList[i].StudentProfileImg);

                        int _sliderValue = FindStudent(StudentType.Programming, false);

                        m_GameJamPanel.ChangeSlider(StudentType.Programming, _sliderValue);

                        int _needValue = 0;

                        if (m_NeedProgrammingStat.Count > 1)
                        {
                            _needValue = m_NeedProgrammingStat.ElementAt(0).Value +
                                         m_NeedProgrammingStat.ElementAt(1).Value;
                        }
                        else
                        {
                            _needValue = m_NeedProgrammingStat.ElementAt(0).Value;
                        }

                        m_GameJamPanel.ChangeSliderFillSprite(StudentType.Programming, _sliderValue, _needValue);
                        m_GameJamPanel.CheckSelectStudent(m_GameJamPanel.m_GameDesignerNone,
                            m_GameJamPanel.m_ArtNone);
                    }
                    else
                    {
                        // �̹� ������ �л��̴� �ȵȴٴ� ��� ����ֱ�
                        _currentObj.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }

        if (m_GameJamPanel.m_GameDesignerNone.activeSelf == false && m_GameJamPanel.m_ArtNone.activeSelf == false &&
            m_GameJamPanel.m_ProgrammingNone.activeSelf == false)
        {
            DecideGenre();
            DeterminesRank();
        }

        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 17)
        {
            m_Unmask.fitTarget = m_GameJamPanel.ArtButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 280, 0);
            m_TutorialCount++;
        }

        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 19)
        {
            m_Unmask.fitTarget = m_GameJamPanel.ProgrammingButton.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 280, 0);
            m_TutorialCount++;
        }

        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 21)
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
        if (m_ClickStudent[0] != null && m_ClickStudent[1] != null && m_ClickStudent[2] != null)
        {
            m_GameJamPanel.ClickSelectCompleteButton();

            if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 22)
            {
                m_Unmask.fitTarget = m_GameJamPanel.ParticipationYesButton.GetComponent<RectTransform>();
                m_TutorialArrowImage.gameObject.SetActive(true);
                m_TutorialTextImage.gameObject.SetActive(false);
                m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 300, 0);
                m_TutorialCount++;
            }
        }
    }

    // ������ ��ư���� ������ ���� ȭ���� �ڷ�ƾ���� ���ش�.
    public void GameJamEntry()
    {
        SetActiveAlram(false);
        SelectComplete();

        for (int i = 0; i < m_ClickStudent.Length; i++)
        {
            m_ClickStudent[i] = null;
        }

        StartCoroutine(m_GameJamPanel.CloseParticipationPanel());

        PlayerInfo.Instance.ParticipatedGameJamCount++;

        if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 23)
        {
            m_NextButton.gameObject.SetActive(true);
            m_TutorialPanel.SetActive(false);
            StartCoroutine(GameJamTutorialEnd());
        }
    }

    IEnumerator GameJamTutorialEnd()
    {
        yield return new WaitForSecondsRealtime(3.7f);

        Time.timeScale = 0;
        m_TutorialPanel.SetActive(true);
        m_TutorialArrowImage.gameObject.SetActive(false);
        m_Unmask.gameObject.SetActive(false);
        m_TutorialTextImage.gameObject.SetActive(false);
        m_PDAlarm.SetActive(true);
        m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
        m_ScriptCount++;
        m_TutorialCount++;
    }

    private int CheckResultConceptSprite(string _conceptName)
    {
        int _randomIndex = 0;

        switch (_conceptName)
        {
            case "���������":
            {
                _randomIndex = (int)ResultConcept.��������� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);

                return _index;
            }

            case "�볭��":
            {
                _randomIndex = (int)ResultConcept.�볭�� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "�̼ҳ�":
            {
                _randomIndex = (int)ResultConcept.�̼ҳ� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "�̷�����":
            {
                _randomIndex = (int)ResultConcept.�̷����� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "�������":
            {
                _randomIndex = (int)ResultConcept.������� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "����":
            {
                _randomIndex = (int)ResultConcept.���� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "�ܰ���":
            {
                _randomIndex = (int)ResultConcept.�ܰ��� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "����������":
            {
                _randomIndex = (int)ResultConcept.���������� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "�巡��":
            {
                _randomIndex = (int)ResultConcept.�巡�� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "������ũ":
            {
                _randomIndex = (int)ResultConcept.������ũ * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            // 3��
            case "�ð�����":
            {
                _randomIndex = (int)ResultConcept.�ð����� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 3);


                return _index;
            }

            // 1��
            case "����� ��":
            {
                _randomIndex = (int)ResultConcept.����ǽ� * 2 + 1;
                //int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);

                return _randomIndex;
            }

            // 3��
            case "�뷡��":
            {
                _randomIndex = (int)ResultConcept.�뷡�� * 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 3);


                return _index;
            }

            // 3��
            case "���̵�":
            {
                _randomIndex = (int)ResultConcept.���̵� * 2 + 1;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 3);


                return _index;
            }

            // 3��
            case "�౸":
            {
                _randomIndex = (int)ResultConcept.�౸ * 2 + 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 3);


                return _index;
            }

            case "�±ǵ�":
            {
                _randomIndex = (int)ResultConcept.�±ǵ� * 2 + 3;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "��":
            {
                _randomIndex = (int)ResultConcept.�� * 2 + 3;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "��":
            {
                _randomIndex = (int)ResultConcept.�� * 2 + 3;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "��ġ�Ƿ�":
            {
                _randomIndex = (int)ResultConcept.��ġ�Ƿ� * 2 + 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);


                return _index;
            }

            case "��Ÿ��":
            {
                _randomIndex = (int)ResultConcept.��Ÿ�� * 2 + 2;
                int _index = UnityEngine.Random.Range(_randomIndex, _randomIndex + 2);

                return _randomIndex;
            }

            case "����������":
            {
                return 42;
            }
        }
        return 0;
    }

    // �������� �����ϴ°� Ȯ���� �Ǹ� �л����� ����Ƚ���� ���ְ� ��¥�� �������ش�.
    private void SelectComplete()
    {
        m_ClickStudent[0].m_StudentStat.m_NumberOfEntries -= 1;
        m_ClickStudent[1].m_StudentStat.m_NumberOfEntries -= 1;
        m_ClickStudent[2].m_StudentStat.m_NumberOfEntries -= 1;

        GameJamSaveData _newData = new GameJamSaveData();

        _newData.m_MakeYear = GameTime.Instance.FlowTime.NowYear;
        _newData.m_MakeMonth = GameTime.Instance.FlowTime.NowMonth;
        _newData.m_MakeWeek = GameTime.Instance.FlowTime.NowWeek;
        _newData.m_MakeDay = GameTime.Instance.FlowTime.NowDay;

        _newData.m_GameDesignerStudentName = m_ClickStudent[0].m_StudentStat.m_StudentName;
        _newData.m_ArtStudentName = m_ClickStudent[1].m_StudentStat.m_StudentName;
        _newData.m_ProgrammingStudentName = m_ClickStudent[2].m_StudentStat.m_StudentName;

        _newData.m_GameJamID = m_NowGameJamInfo.m_GjamID;
        _newData.m_Difficulty = m_EntryGameJamDifficulty;

        var (gameDesignerStatName1, gameDesignerStat1, gameDesignerStatName2, gameDesignerStat2) = FillNeedStat(m_NeedGameDesignerStat);
        var (ArtStatName1, ArtStat1, ArtStatName2, ArtStat2) = FillNeedStat(m_NeedArtStat);
        var (programmingStatName1, programmingStat1, programmingStatName2, programmingStat2) = FillNeedStat(m_NeedProgrammingStat);

        _newData.m_GameDesignerFirstNeedStatName = gameDesignerStatName1;
        _newData.m_GameDesignerFirstNeedStat = gameDesignerStat1;
        _newData.m_GameDesignerSecondNeedStatName = gameDesignerStatName2;
        _newData.m_GameDesignerSecondNeedStat = gameDesignerStat2;

        _newData.m_ArtFirstNeedStatName = ArtStatName1;
        _newData.m_ArtFirstNeedStat = ArtStat1;
        _newData.m_ArtSecondNeedStatName = ArtStatName2;
        _newData.m_ArtSecondNeedStat = ArtStat2;

        _newData.m_ProgrammingFirstNeedStatName = programmingStatName1;
        _newData.m_ProgrammingFirstNeedStat = programmingStat1;
        _newData.m_ProgrammingSecondNeedStatName = programmingStatName2;
        _newData.m_ProgrammingSecondNeedStat = programmingStat2;

        _newData.m_ConceptIndex = CheckResultConceptSprite(m_NowGameJamInfo.m_GjamConcept);

        _newData.m_GameDesignerEntryStudentStat1 = m_Requirement1List[0];
        _newData.m_GameDesignerEntryStudentStat2 = m_Requirement2List[0];

        _newData.m_ArtEntryStudentStat1 = m_Requirement1List[1];
        _newData.m_ArtEntryStudentStat2 = m_Requirement2List[1];

        _newData.m_ProgrammingEntryStudentStat1 = m_Requirement1List[2];
        _newData.m_ProgrammingEntryStudentStat2 = m_Requirement2List[2];

        _newData.m_SuccessPercent = m_GameJamPanel.m_ExpectedSuccesPercentgetter.text;

        string _name = m_NowGameJamInfo.m_GjamName;
        int _day = GameTime.Instance.FlowTime.NowDay + 3;
        int _month = GameTime.Instance.FlowTime.NowMonth;
        int _week = GameTime.Instance.FlowTime.NowWeek;

        if (m_GameJamEntryCount.ContainsKey(_name))
        {
            m_EnteryCount++;
            m_GameJamEntryCount[_name] = m_EnteryCount;
        }
        else
        {
            m_GameJamEntryCount.Add(_name, 1);
        }

        m_MonthLimit -= 1;

        PlayerInfo.Instance.MyMoney -= (int)m_NowGameJamInfo.m_EntryFee;
        MonthlyReporter.Instance.m_NowMonth.ExpensesActivity += (int)m_NowGameJamInfo.m_EntryFee;

        if (_week == 4 && GameTime.Instance.FlowTime.NowDay >= 3)
        {
            _month += 1;
        }

        GameObject _eventPrefab = m_ActivityEvent.MakeEventPrefab(m_NowGameJamInfo.m_GjamName,
            GameTime.Instance.FlowTime.NowMonth, _week, _day, true, _month);

        m_ActivityEvent.SetEventPanelActive(true);

        m_ToBeRunning.Add(_newData);
        m_GameJamEntryCoolTime = true;
        StartCoroutine(m_ActivityEvent.TypingText(_eventPrefab, "������ ������"));
    }

    private (string _statName1, int _stat1, string _statName2, int _stat2) FillNeedStat(Dictionary<string, int> _needDic)
    {
        if (_needDic.Count > 1)
            return (_needDic.ElementAt(0).Key, _needDic.ElementAt(0).Value, _needDic.ElementAt(1).Key, _needDic.ElementAt(1).Value);
        else
            return (_needDic.ElementAt(0).Key, _needDic.ElementAt(0).Value, "", 0);
    }

    // Ÿ�̸Ӱ� �� ������ ���� ���â�� �������Ѵ�.
    public void ClickResultPanelButtn()
    {
        m_GameJamResultPanel.SetResultPanelMoneyAndSpecialPoint();

        string _gmName = m_RunningGameJameData.m_GameDesignerStudentName;
        string _artName = m_RunningGameJameData.m_ArtStudentName;
        string _programmingName = m_RunningGameJameData.m_ProgrammingStudentName;

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

        CalculateFinalGameJamResult();
        int _awareness = (int)m_RunningGameJameData.m_Awareness;
        int _TalentDevelopment = (int)m_RunningGameJameData.m_PracticalTalent;
        int _management = (int)m_RunningGameJameData.m_Management;

        int _prevAwereness = PlayerInfo.Instance.Famous;
        int _prevTalentDevelopment = PlayerInfo.Instance.TalentDevelopment;
        int _prevManagement = PlayerInfo.Instance.Management;

        float _finalAwereness = _prevAwereness + _awareness;
        float _finalTalentDevelopment = _prevTalentDevelopment + _TalentDevelopment;
        float _finalManagement = _prevManagement + _management;

        double _finalAwerenessPercent = Math.Round(_finalAwereness / 999 * 100);
        double _finalTalentDevelopmentPercent = Math.Round(_finalTalentDevelopment / 999 * 100);
        double _finalManagementPercent = Math.Round(_finalManagement / 999 * 100);

        Sprite _awarenessArrow = _awareness > 0 ? m_UpArrow : _awareness == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _TalentDevelopmentArrow =
            _TalentDevelopment > 0 ? m_UpArrow : _TalentDevelopment == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _managementArrow = _management > 0 ? m_UpArrow : _management == 0 ? m_NoneArrow : m_DownArrow;

        Sprite _FinalawarenessArrow = _finalAwereness - _prevAwereness > 0 ? m_UpArrow :
            _finalAwereness - _prevAwereness == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _FinalTalentDevelopmentArrow = _finalTalentDevelopment - _prevTalentDevelopment > 0 ? m_UpArrow :
            _finalTalentDevelopment - _prevTalentDevelopment == 0 ? m_NoneArrow : m_DownArrow;
        Sprite _FinalmanagementArrow = _finalManagement - _prevManagement > 0 ? m_UpArrow :
            _finalManagement - _prevManagement == 0 ? m_NoneArrow : m_DownArrow;

        string _gameJamName = m_GameJamResultPanel._changeGameName;

        GameJamInfo _nowMonthGameJamData = SearchAllGameJamInfo((int)m_RunningGameJameData.m_GameJamID);

        if (_gameJamName == "")
        {
            string GjamName = _nowMonthGameJamData.m_GjamName;
            _gameJamName = GjamName + m_GameJamEntryCount[GjamName].ToString();
        }

        string _date = GameTime.Instance.FlowTime.NowYear + "�� " + GameTime.Instance.FlowTime.NowMonth + "�� " +
                       GameTime.Instance.FlowTime.NowWeek + "���� ���";
        m_FinalGameJamResultPanel.SetPreInfo(_prevAwereness.ToString(), _prevTalentDevelopment.ToString(), _prevManagement.ToString());
        m_FinalGameJamResultPanel.SetChangeInfo(_awareness.ToString(), _TalentDevelopment.ToString(), _management.ToString());
        m_FinalGameJamResultPanel.SetChangeInfoArrowImage(_awarenessArrow, _TalentDevelopmentArrow, _managementArrow);
        m_FinalGameJamResultPanel.SetSlider((int)_finalAwerenessPercent, (int)_finalTalentDevelopmentPercent, (int)_finalManagementPercent);
        m_FinalGameJamResultPanel.SetFinalInfo(_finalAwereness.ToString(), _finalTalentDevelopment.ToString(), _finalManagement.ToString());
        m_FinalGameJamResultPanel.SetFinalInfoArrowImage(_FinalawarenessArrow, _FinalTalentDevelopmentArrow, _FinalmanagementArrow);

        m_RunningGameJameData.m_GameName = _gameJamName;
        m_RunningGameJameData.m_Genre = _genreName;
        m_RunningGameJameData.m_Rank = _rank;
        m_RunningGameJameData.m_Funny = Funny;
        m_RunningGameJameData.m_Graphic = Graphic;
        m_RunningGameJameData.m_Perfection = Perfection;
        m_RunningGameJameData.m_TotalGenreScore = m_TotalGenreScore;

        // �����͸� ������ �� �̹� �� �� �����غô� �̺�Ʈ�� �ش� ���̵� ���� ����Ʈ�� ���� ����. �ƴ϶�� ���ο� ����Ʈ ���� ���̵�� �����ϱ�
        if (m_GameJamHistory.ContainsKey(_nowMonthGameJamData.m_GjamID))
        {
            m_GameJamHistory[_nowMonthGameJamData.m_GjamID].Add(m_RunningGameJameData);
        }
        else
        {
            List<GameJamSaveData> m_GameJamDataList = new List<GameJamSaveData>();
            m_GameJamDataList.Add(m_RunningGameJameData);
            m_GameJamHistory.Add(_nowMonthGameJamData.m_GjamID, m_GameJamDataList);
            OnDataChanged();
        }

        EnqueueDataChangedEvent();

        SetRiseStat(_rank, GameDesignerRequirementStatList, ref RiseGameDesignerStatList);
        SetRiseStat(_rank, ArtRequirementStatList, ref RiseArtStatList);
        SetRiseStat(_rank, ProgrammingRequirementStatList, ref RiseProgrammingStatList);

        ChangeStudentData(m_RunningGameJameData.m_GameDesignerStudentName, RiseGameDesignerStatList);
        ChangeStudentData(m_RunningGameJameData.m_ArtStudentName, RiseArtStatList);
        ChangeStudentData(m_RunningGameJameData.m_ProgrammingStudentName, RiseProgrammingStatList);

        PlayerInfo.Instance.Famous = (int)_finalAwereness;
        PlayerInfo.Instance.Management = (int)_finalManagement;
        PlayerInfo.Instance.TalentDevelopment = (int)_finalTalentDevelopment;

        /// �������� ���� ����, �, ����缺 ���� ����
        MonthlyReporter.Instance.m_NowMonth.ManagementScore = (int)_finalManagement;
        MonthlyReporter.Instance.m_NowMonth.FamousScore = (int)_finalAwereness;
        MonthlyReporter.Instance.m_NowMonth.TalentDevelopmentScore = (int)_finalTalentDevelopment;

        int reward = FindRewardToDifficulty(_nowMonthGameJamData.m_GjamAI_ID, m_RunningGameJameData.m_Rank);

        m_FinalGameJamResultPanel.SetResultPanel(InGameUI.Instance.m_nowAcademyName.text, _gameJamName, _date, reward.ToString());

        if (_rank != "�̿ϼ�")
        {
            PlayerInfo.Instance.MyMoney += reward;
            MonthlyReporter.Instance.m_NowMonth.IncomeActivity += reward;
        }
        else
        {
            m_SoundManager.PlayFailSound();
        }

        int index = m_ToBeRunning.FindIndex(x => x.m_GameJamID == _nowMonthGameJamData.m_GjamID);
        m_ToBeRunning.RemoveAt(index);
        //m_GameJamResultPanel.InitChangeName();
    }

    // ������ ���� �濵���� ����ϴ� �Լ�
    private void CalculateFinalGameJamResult()
    {
        if (_rank == "�̿ϼ�")
        {
            m_RunningGameJameData.m_Awareness = -10;
            m_RunningGameJameData.m_PracticalTalent = 0;
            m_RunningGameJameData.m_Management = 0;
        }
        else if (m_RunningGameJameData.m_Score <= 50)
        {
            m_RunningGameJameData.m_Awareness = 2;
            m_RunningGameJameData.m_PracticalTalent = 0;
            m_RunningGameJameData.m_Management = 0;
        }
        else if (m_RunningGameJameData.m_Score <= 80)
        {
            m_RunningGameJameData.m_Awareness = 4;
            m_RunningGameJameData.m_PracticalTalent = 0;
            m_RunningGameJameData.m_Management = 0;
        }
        else if (m_RunningGameJameData.m_Score <= 110)
        {
            m_RunningGameJameData.m_Awareness = 6;
            m_RunningGameJameData.m_PracticalTalent = 1;
            m_RunningGameJameData.m_Management = 0;
        }
        else if (m_RunningGameJameData.m_Score <= 140)
        {
            m_RunningGameJameData.m_Awareness = 8;
            m_RunningGameJameData.m_PracticalTalent = 2;
            m_RunningGameJameData.m_Management = 1;
        }
        else if (m_RunningGameJameData.m_Score <= 170)
        {
            m_RunningGameJameData.m_Awareness = 10;
            m_RunningGameJameData.m_PracticalTalent = 3;
            m_RunningGameJameData.m_Management = 2;
        }
        else if (m_RunningGameJameData.m_Score <= 200)
        {
            m_RunningGameJameData.m_Awareness = 12;
            m_RunningGameJameData.m_PracticalTalent = 4;
            m_RunningGameJameData.m_Management = 3;
        }
        else if (m_RunningGameJameData.m_Score <= 230)
        {
            m_RunningGameJameData.m_Awareness = 14;
            m_RunningGameJameData.m_PracticalTalent = 5;
            m_RunningGameJameData.m_Management = 4;
        }
        else if (m_RunningGameJameData.m_Score <= 260)
        {
            m_RunningGameJameData.m_Awareness = 16;
            m_RunningGameJameData.m_PracticalTalent = 6;
            m_RunningGameJameData.m_Management = 5;
        }
        else if (m_RunningGameJameData.m_Score <= 290)
        {
            m_RunningGameJameData.m_Awareness = 18;
            m_RunningGameJameData.m_PracticalTalent = 7;
            m_RunningGameJameData.m_Management = 6;
        }
        else if (m_RunningGameJameData.m_Score <= 320)
        {
            m_RunningGameJameData.m_Awareness = 20;
            m_RunningGameJameData.m_PracticalTalent = 8;
            m_RunningGameJameData.m_Management = 7;
        }
        else if (m_RunningGameJameData.m_Score <= 350)
        {
            m_RunningGameJameData.m_Awareness = 22;
            m_RunningGameJameData.m_PracticalTalent = 9;
            m_RunningGameJameData.m_Management = 8;
        }
        else if (m_RunningGameJameData.m_Score <= 380)
        {
            m_RunningGameJameData.m_Awareness = 24;
            m_RunningGameJameData.m_PracticalTalent = 10;
            m_RunningGameJameData.m_Management = 9;
        }
        else if (m_RunningGameJameData.m_Score <= 410)
        {
            m_RunningGameJameData.m_Awareness = 26;
            m_RunningGameJameData.m_PracticalTalent = 11;
            m_RunningGameJameData.m_Management = 10;
        }
        else if (m_RunningGameJameData.m_Score <= 440)
        {
            m_RunningGameJameData.m_Awareness = 28;
            m_RunningGameJameData.m_PracticalTalent = 12;
            m_RunningGameJameData.m_Management = 11;
        }
        else if (m_RunningGameJameData.m_Score <= 470)
        {
            m_RunningGameJameData.m_Awareness = 30;
            m_RunningGameJameData.m_PracticalTalent = 13;
            m_RunningGameJameData.m_Management = 12;
        }
        else if (m_RunningGameJameData.m_Score <= 500)
        {
            m_RunningGameJameData.m_Awareness = 32;
            m_RunningGameJameData.m_PracticalTalent = 14;
            m_RunningGameJameData.m_Management = 13;
        }
        else if (m_RunningGameJameData.m_Score <= 530)
        {
            m_RunningGameJameData.m_Awareness = 34;
            m_RunningGameJameData.m_PracticalTalent = 15;
            m_RunningGameJameData.m_Management = 14;
        }
        else if (m_RunningGameJameData.m_Score <= 560)
        {
            m_RunningGameJameData.m_Awareness = 36;
            m_RunningGameJameData.m_PracticalTalent = 16;
            m_RunningGameJameData.m_Management = 15;
        }
        else if (m_RunningGameJameData.m_Score <= 590)
        {
            m_RunningGameJameData.m_Awareness = 38;
            m_RunningGameJameData.m_PracticalTalent = 17;
            m_RunningGameJameData.m_Management = 16;
        }
        else if (m_RunningGameJameData.m_Score <= 620)
        {
            m_RunningGameJameData.m_Awareness = 40;
            m_RunningGameJameData.m_PracticalTalent = 18;
            m_RunningGameJameData.m_Management = 17;
        }
        else if (m_RunningGameJameData.m_Score <= 650)
        {
            m_RunningGameJameData.m_Awareness = 42;
            m_RunningGameJameData.m_PracticalTalent = 19;
            m_RunningGameJameData.m_Management = 18;
        }
        else if (m_RunningGameJameData.m_Score <= 680)
        {
            m_RunningGameJameData.m_Awareness = 44;
            m_RunningGameJameData.m_PracticalTalent = 20;
            m_RunningGameJameData.m_Management = 19;
        }
        else if (m_RunningGameJameData.m_Score <= 710)
        {
            m_RunningGameJameData.m_Awareness = 46;
            m_RunningGameJameData.m_PracticalTalent = 21;
            m_RunningGameJameData.m_Management = 20;
        }
        else if (m_RunningGameJameData.m_Score <= 740)
        {
            m_RunningGameJameData.m_Awareness = 48;
            m_RunningGameJameData.m_PracticalTalent = 22;
            m_RunningGameJameData.m_Management = 21;
        }
        else if (m_RunningGameJameData.m_Score <= 999)
        {
            m_RunningGameJameData.m_Awareness = 50;
            m_RunningGameJameData.m_PracticalTalent = 23;
            m_RunningGameJameData.m_Management = 22;
        }
    }

    // �䱸�ϴ� ������ ù��°. �ϳ��ۿ� ������ �� �Լ��� �����ϸ� �ȴ�
    private void FindPartFirstRequirementStat(string _statName, List<Student> _list, int _index)
    {
        for (int i = 0; i < AbilityNameList.Length; i++)
        {
            if (_statName == AbilityNameList[i])
            {
                m_Requirement1List[(int)_list[_index].m_StudentStat.m_StudentType] =
                    _list[_index].m_StudentStat.m_AbilityAmountArr[i];
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
                m_Requirement2List[(int)_list[_index].m_StudentStat.m_StudentType] =
                    _list[_index].m_StudentStat.m_AbilityAmountArr[i];
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
        m_EntryGameJamDifficulty = m_DummyGameJamList[_index].m_GjamAI_ID;

        string _name = m_DummyGameJamList[_index].m_GjamName + " ������";
        string _subName = m_DummyGameJamList[_index].m_GjamDetailInfo;
        string _reward = string.Format("{0:#,0}", FindRewardToDifficulty(m_DummyGameJamList[_index].m_GjamAI_ID));
        string _awareness = "2 ~ 50";
        string _date = m_DummyGameJamList[_index].m_GjamMonth.ToString() + "�� ";
        string _health = m_DummyGameJamList[_index].m_StudentHealth.ToString();
        string _passion = m_DummyGameJamList[_index].m_StudentPassion.ToString();
        Sprite _maingenre = FindGenreSprite(m_DummyGameJamList[_index].m_GjamMainGenre);
        Sprite _subGenre = FindGenreSprite(m_DummyGameJamList[_index].m_GjamSubGenre);

        ClassifyPartNeedStat("��ȹ", m_DummyGameJamList[_index].m_GjamNeedStatGameDesigner);
        ClassifyPartNeedStat("��Ʈ", m_DummyGameJamList[_index].m_GjamNeedStatArt);
        ClassifyPartNeedStat("�ù�", m_DummyGameJamList[_index].m_GjamNeedStatProgramming);

        m_GameJamPanel.ChangeRecruitNoticeInfo(_name, _subName, _reward, _awareness, _date, _health, _passion, _entryFee);
        m_GameJamPanel.SetAwarenessPlusMinus(_awareness, m_UpArrow, m_DownArrow);
        m_GameJamPanel.SetHealthPlusMinus(_health, m_UpArrow, m_DownArrow);
        m_GameJamPanel.SetPassionPlusMinus(_passion, m_UpArrow, m_DownArrow);
        m_GameJamPanel.ChangeGenreSprite(_maingenre, _subGenre);

        SetReauirementIcon(m_GameJamPanel.GameDesignerRequirementStatIcon1,
            m_GameJamPanel.GameDesignerRequirementStatIcon2, m_NeedGameDesignerStat);
        SetReauirementIcon(m_GameJamPanel.ArtRequirementStatIcon1, m_GameJamPanel.ArtRequirementStatIcon2,
            m_NeedArtStat);
        SetReauirementIcon(m_GameJamPanel.ProgrammingRequirementStatIcon1,
            m_GameJamPanel.ProgrammingRequirementStatIcon2, m_NeedProgrammingStat);

        for (int j = 0; j < 5; j++)
        {
            GameDesignerRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatGameDesigner[AbilityNameList[j]];
            ArtRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatArt[AbilityNameList[j]];
            ProgrammingRequirementStatList[j] = m_DummyGameJamList[_index].m_GjamNeedStatProgramming[AbilityNameList[j]];
        }

        m_NowGameJamInfo = SearchAllGameJamInfo(m_DummyGameJamList[_index].m_GjamID);
    }

    // ������ �г��� ���� �� ���� ó�� �����ִ� ������ ����Ʈ �� �����ִ� �������� �Ѵ�.
    public void FirstGameJamIfoToRecruitNoticeInfo()
    {
        if (m_DummyGameJamList.Count == 0)
        {
            if (m_GameJamPanel.GameJamCanvas.activeSelf)
            {
                m_GameJamPanel.SetActiveGameJamCanvas(false);
            }

            m_Slider.SetWarningPanel.SetActive(true);

            StartCoroutine(SetWarningPanel());

            return;
        }

        #region _�� �����ִ� ��ư�� ���̶���Ʈ ���ֱ�
        if (m_PrevClickGameJamObj != null)
        {
            Button _prevButton = m_PrevClickGameJamObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        m_PrevClickGameJamObj = m_GameJamPanel.m_GameJamParent.GetChild(0).gameObject;

        Button _currentButton = m_GameJamPanel.m_GameJamParent.GetChild(0).gameObject.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        if (!m_GameJamPanel.GameJamCanvas.activeSelf)
        {
            m_GameJamPanel.SetActiveGameJamCanvas(true);
            m_Slider.SetWarningPanel.SetActive(false);
        }

        switch (m_MonthLimit)
        {
            case 2:
            {
                /// �غ�� �����Ͱ� ���� �� ����ó���ϱ�

                string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[0].m_EntryFee);
                string _money = m_GameJamPanel.SetCurrentMoney();

                MakeRecruitContent(_entryFee, _money, 0, true, m_Entry, "����");

                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 5)
                {
                    m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = false;
                    m_GameJamPanel.GameJamListParentObj.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.GameJamListParentObj.transform.GetChild(1).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.GameJamListParentObj.transform.GetChild(2).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.GameJamListParentObj.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    m_GameJamPanel.GameJamListParentObj.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    m_Unmask.fitTarget = m_GameJamPanel.RecruitNoticePanel.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_NextButton.gameObject.SetActive(true);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(700, -200, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
            break;

            case 1:
            {
                if (m_ToBeRunning.Count != 0)
                {

                    bool _isEntryGameJam = false;

                    GameJamInfo _nowData = new GameJamInfo();
                    _nowData = SearchAllGameJamInfo((int)m_ToBeRunning[0].m_GameJamID);

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");

                            if (_name == m_DummyGameJamList[0].m_GjamName || gameJamInfo.m_GjamName == m_DummyGameJamList[0].m_GjamName)
                            {
                                string _entryFee = "-";
                                MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");
                                _isEntryGameJam = true;
                            }
                        }
                    }

                    if (!_isEntryGameJam)
                    {
                        if (m_DummyGameJamList[0].m_GjamName == _nowData.m_GjamName)
                        {
                            string _entryFee = "-";
                            MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");
                        }
                        else
                        {
                            string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[0].m_EntryFee);
                            string _money = m_GameJamPanel.SetCurrentMoney();

                            MakeRecruitContent(_entryFee, _money, 0, true, m_Entry, "����");
                        }
                    }
                }
                else
                {
                    bool _isEntryGameJam = false;

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);

                            if (_name == m_DummyGameJamList[0].m_GjamName || gameJamInfo.m_GjamName == m_DummyGameJamList[0].m_GjamName)
                            {
                                string _entryFee = "-";
                                MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");

                                _isEntryGameJam = true;
                            }
                        }
                    }

                    // ���� �������� �ִµ� ���� ��� Ŭ���� �������� �ƴ϶��
                    if (_isEntryGameJam == false)
                    {
                        string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[0].m_EntryFee);
                        string _money = m_GameJamPanel.SetCurrentMoney();
                        MakeRecruitContent(_entryFee, _money, 0, true, m_Entry, "����");
                    }
                }
            }
            break;

            case 0:
            {
                if (m_ToBeRunning.Count != 0)
                {
                    bool _isEntryGameJam = false;

                    GameJamInfo _nowData = new GameJamInfo();
                    _nowData = SearchAllGameJamInfo((int)m_ToBeRunning[0].m_GameJamID);

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");

                            if (_name == m_DummyGameJamList[0].m_GjamName || gameJamInfo.m_GjamName == m_DummyGameJamList[0].m_GjamName)
                            {
                                string _entryFee = "-";
                                MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");
                                _isEntryGameJam = true;
                            }
                        }
                    }

                    if (!_isEntryGameJam)
                    {
                        if (m_DummyGameJamList[0].m_GjamName == _nowData.m_GjamName)
                        {
                            string _entryFee = "-";
                            MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");
                        }
                        else
                        {
                            string _entryFee = "-";
                            MakeRecruitContent(_entryFee, "0", 0, true, m_NotEntry, "��û �Ұ�");
                        }
                    }
                }
                else
                {
                    bool _isEntryGameJam = false;

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");

                            if (_name == m_DummyGameJamList[0].m_GjamName || gameJamInfo.m_GjamName == m_DummyGameJamList[0].m_GjamName)
                            {
                                string _entryFee = "-";
                                MakeRecruitContent(_entryFee, "0", 0, false, m_EntryComplete, "���� ��û �Ϸ�");
                                _isEntryGameJam = true;
                            }
                        }
                    }

                    if (_isEntryGameJam == false)
                    {
                        string _entryFee = "-";
                        MakeRecruitContent(_entryFee, "0", 0, true, m_NotEntry, "��û �Ұ�");
                    }
                }
            }
            break;
        }
    }

    IEnumerator SetWarningPanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        m_Slider.SetWarningPanel.SetActive(false);
    }

    IEnumerator SetEntryWarningPanel()
    {
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

            _gameJamPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                m_DummyGameJamList[i].m_GjamName;
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

                    string _gameName = m_GameJamHistory.ElementAt(i).Value[j].m_GameName;
                    string _genreName = m_GameJamHistory.ElementAt(i).Value[j].m_Genre;
                    string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_Rank;
                    Sprite _genre = FindGenreSprite(_genreName);
                    Sprite _concept = m_ResultConceptSprite[m_GameJamHistory.ElementAt(i).Value[j].m_ConceptIndex];

                    _gameJamListPrefab.name = _gameName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameConceptImage.sprite = _concept;

                    GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton
                        .gameObject;
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
                        .AddListener(() => MakePreGameJamInfo(_gameList));
                    _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
                        .AddListener(m_GameJamListPanel.ClickGameJamPrefab);
                }
            }
            else
            {
                GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
                m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab, m_GameJamListPanel._gamejameContentParent);

                string _gameName = m_GameJamHistory.ElementAt(i).Value[0].m_GameName;
                string _genreName = m_GameJamHistory.ElementAt(i).Value[0].m_Genre;
                string _rank = m_GameJamHistory.ElementAt(i).Value[0].m_Rank;
                Sprite _genre = FindGenreSprite(_genreName);
                Sprite _concept = m_ResultConceptSprite[m_GameJamHistory.ElementAt(i).Value[0].m_ConceptIndex];

                _gameJamListPrefab.name = _gameName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameConceptImage.sprite = _concept;

                GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton
                    .gameObject;
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
                    .AddListener(() => MakePreGameJamInfo(_gameList));
                _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
                    .AddListener(m_GameJamListPanel.ClickGameJamPrefab);
            }
        }

        #region _�� �����ִ� ��ư�� ���̶���Ʈ ���ֱ�
        if (m_PrevRankButtonObj != null)
        {
            Button _prevButton = m_PrevRankButtonObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        //GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        m_PrevRankButtonObj = m_GameJamListPanel.AllButton.gameObject;

        Button _currentButton = m_GameJamListPanel.AllButton;

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_RankButtonHighLightColor;
        _currentButtonColor.highlightedColor = m_RankButtonHighLightColor;
        _currentButtonColor.pressedColor = m_RankButtonHighLightColor;
        _currentButtonColor.selectedColor = m_RankButtonHighLightColor;

        _currentButton.colors = _currentButtonColor;
        #endregion
    }

    // ���� ���� ��ư���� ������ �������ִµ� �ʿ��� �۾����� ���ĳ��� �Լ�
    private void MakeRecruitContent(string _entryFee, string _money, int _index, bool _entrybuttonActive, Sprite _entrybuttonSprite, string _entrybuttonName)
    {
        SetRecruitContent(_index, _entryFee);
        m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, _entrybuttonActive, _entrybuttonSprite);
        m_GameJamPanel.ChangeStartButtonName(_entrybuttonName);

        string _replaceEntryFee = _entryFee.Replace(",", "");

        if (_replaceEntryFee != "-")
        {
            // �����ϰ� �ִ� �ݾ��� �����⺸�� ������ ���������� ���� �ٲ��ֱ�
            if (int.Parse(_replaceEntryFee) > int.Parse(_money))
            {
                m_GameJamPanel.ChangeColorMoneyText(new Color(1, 0, 0, 1));
                m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, false, m_NotEntry);
                m_GameJamPanel.ChangeStartButtonName("��û �Ұ�");
            }
            else
            {
                m_GameJamPanel.ChangeColorMoneyText(new Color(0, 0, 0, 1));
                m_GameJamPanel.SetButton(m_GameJamPanel.m_SetStartButton, true, m_Entry);
                m_GameJamPanel.ChangeStartButtonName("����");
            }
        }
    }

    // ���� ��ư�� �� ������ �������ִ� �Լ�
    /// TODO : �ٲ� �ý��ۿ� ���� ���� ����. ����ó�� ����� �� �κ��� �� �����.
    private void MakeRecruitNoticeInfo()
    {
        #region _��ư�� Ŭ������ �� ���̶���Ʈ�� �����ְ� �ϱ�

        if (m_PrevClickGameJamObj != null)
        {
            Button _prevButton = m_PrevClickGameJamObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        m_PrevClickGameJamObj = _currentObj;

        Button _currentButton = _currentObj.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        _gameJameName = _currentObj.name;

        switch (m_MonthLimit)
        {
            case 2:
            {
                for (int i = 0; i < m_DummyGameJamList.Count; i++)
                {
                    if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                    {
                        string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[i].m_EntryFee);
                        string _money = m_GameJamPanel.SetCurrentMoney();

                        MakeRecruitContent(_entryFee, _money, i, true, m_Entry, "����");
                    }
                }

                if (PlayerInfo.Instance.IsFirstGameJam && m_TutorialCount == 7)
                {
                    m_GameJamPanel.RecruitNoticePanel.transform.GetChild(0).GetComponent<ScrollRect>().vertical = true;
                    m_Unmask.fitTarget = m_GameJamPanel.RecruitNoticeInfoPanel.GetComponent<RectTransform>();
                    m_TutorialArrowImage.gameObject.SetActive(false);
                    m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
                    m_TutorialTextImage.gameObject.SetActive(true);
                    m_NextButton.gameObject.SetActive(true);
                    m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-1600, 0, 0);
                    m_ScriptCount++;
                    m_TutorialCount++;
                }
            }
            break;

            case 1:
            {
                if (m_ToBeRunning.Count != 0)
                {
                    for (int i = 0; i < m_ToBeRunning.Count; i++)
                    {
                        GameJamInfo _nowData = SearchAllGameJamInfo((int)m_ToBeRunning[i].m_GameJamID);

                        if (_nowData.m_GjamName != _gameJameName)
                        {
                            for (int j = 0; j < m_DummyGameJamList.Count; j++)
                            {
                                if (_gameJameName == m_DummyGameJamList[j].m_GjamName)
                                {
                                    string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[j].m_EntryFee);
                                    string _money = m_GameJamPanel.SetCurrentMoney();

                                    MakeRecruitContent(_entryFee, _money, j, true, m_Entry, "����");
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < m_DummyGameJamList.Count; j++)
                            {
                                if (_gameJameName == m_DummyGameJamList[j].m_GjamName)
                                {
                                    string _entryFee = "-";
                                    MakeRecruitContent(_entryFee, "0", j, false, m_EntryComplete, "���� ��û �Ϸ�");
                                }
                            }
                        }
                    }
                }
                else
                {
                    bool _isEntryGameJam = false;

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");

                            if (_name == _gameJameName || gameJamInfo.m_GjamName == _gameJameName)
                            {
                                int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamID == m_GameJamHistory.ElementAt(i).Value[j].m_GameJamID);
                                string _entryFee = "-";

                                MakeRecruitContent(_entryFee, "0", _index, false, m_EntryComplete, "���� ��û �Ϸ�");

                                _isEntryGameJam = true;
                            }
                        }
                    }

                    // ���� �������� �ִµ� ���� ��� Ŭ���� �������� �ƴ϶��
                    if (_isEntryGameJam == false)
                    {
                        for (int i = 0; i < m_DummyGameJamList.Count; i++)
                        {
                            if (_gameJameName == m_DummyGameJamList[i].m_GjamName)
                            {
                                string _entryFee = string.Format("{0:#,0}", m_DummyGameJamList[i].m_EntryFee);
                                string _money = m_GameJamPanel.SetCurrentMoney();

                                MakeRecruitContent(_entryFee, _money, i, true, m_Entry, "����");
                            }
                        }
                    }
                }

            }
            break;

            case 0:
            {
                if (m_ToBeRunning.Count != 0)
                {
                    bool _isEntryGameJam = false;

                    for (int j = 0; j < m_GameJamHistory.Count; j++)
                    {
                        for (int k = 0; k < m_GameJamHistory.ElementAt(j).Value.Count; k++)
                        {
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(j).Value[k].m_GameName, @"\d", "");
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(j).Key);

                            if (_name == _gameJameName || gameJamInfo.m_GjamName == _gameJameName)
                            {
                                int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamID == m_GameJamHistory.ElementAt(j).Value[k].m_GameJamID);

                                string _entryFee = "-";

                                MakeRecruitContent(_entryFee, "0", _index, false, m_EntryComplete, "���� ��û �Ϸ�");
                                _isEntryGameJam = true;
                            }
                        }
                    }

                    for (int j = 0; j < m_ToBeRunning.Count; j++)
                    {
                        GameJamInfo gameJamInfo = SearchAllGameJamInfo((int)m_ToBeRunning[j].m_GameJamID);

                        if (_gameJameName == gameJamInfo.m_GjamName)
                        {
                            string _entryFee = "-";
                            MakeRecruitContent(_entryFee, "0", j, false, m_EntryComplete, "���� ��û �Ϸ�");
                            _isEntryGameJam = true;
                        }
                    }

                    if (_isEntryGameJam == false)
                    {
                        string _entryFee = "-";
                        int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamName == _gameJameName);

                        MakeRecruitContent(_entryFee, "0", _index, true, m_NotEntry, "��û �Ұ�");
                    }

                }
                else
                {
                    bool _isEntryGameJam = false;

                    // ���� ��û�� �������� �̹� �������� �� �޵����� ���� ��û �Ϸ��� �������Ѵ�.
                    for (int i = 0; i < m_GameJamHistory.Count; i++)
                    {
                        for (int j = 0; j < m_GameJamHistory.ElementAt(i).Value.Count; j++)
                        {
                            string _name = Regex.Replace(m_GameJamHistory.ElementAt(i).Value[j].m_GameName, @"\d", "");
                            GameJamInfo gameJamInfo = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);

                            if (_name == _gameJameName || gameJamInfo.m_GjamName == _gameJameName)
                            {
                                int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamID == m_GameJamHistory.ElementAt(i).Value[j].m_GameJamID);

                                string _entryFee = "-";

                                MakeRecruitContent(_entryFee, "0", _index, false, m_EntryComplete, "���� ��û �Ϸ�");
                                _isEntryGameJam = true;
                            }
                        }
                    }

                    if (_isEntryGameJam == false)
                    {
                        string _entryFee = "-";
                        int _index = m_DummyGameJamList.FindIndex(x => x.m_GjamName == _gameJameName);

                        MakeRecruitContent(_entryFee, "0", _index, true, m_NotEntry, "��û �Ұ�");
                    }
                }
            }
            break;
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
                if (_gameName == m_GameJamHistory.ElementAt(i).Value[j].m_GameName)
                {
                    GameJamInfo _nowData = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(i).Key);

                    string _year = _nowData.m_GjamYear.ToString();
                    string _month = _nowData.m_GjamMonth.ToString();
                    string _concept = _nowData.m_GjamConcept;

                    string _name = m_GameJamHistory.ElementAt(i).Value[j].m_GameName;

                    Student m_GameDesignerStudent = ObjectManager.Instance.SearchStudentInfo(m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerStudentName);
                    Student m_ArtStudent = ObjectManager.Instance.SearchStudentInfo(m_GameJamHistory.ElementAt(i).Value[j].m_ArtStudentName);
                    Student m_ProgrammingStudent = ObjectManager.Instance.SearchStudentInfo(m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingStudentName);

                    string _gmName;
                    string _artName;
                    string _programmingName;

                    if (m_GameDesignerStudent.m_StudentStat.m_UserSettingName != "")
                    {
                        _gmName = m_GameDesignerStudent.m_StudentStat.m_UserSettingName;
                    }
                    else
                    {
                        _gmName = m_GameDesignerStudent.m_StudentStat.m_StudentName;
                    }

                    if (m_ArtStudent.m_StudentStat.m_UserSettingName != "")
                    {
                        _artName = m_ArtStudent.m_StudentStat.m_UserSettingName;
                    }
                    else
                    {
                        _artName = m_ArtStudent.m_StudentStat.m_StudentName;
                    }

                    if (m_ProgrammingStudent.m_StudentStat.m_UserSettingName != "")
                    {
                        _programmingName = m_ProgrammingStudent.m_StudentStat.m_UserSettingName;
                    }
                    else
                    {
                        _programmingName = m_ProgrammingStudent.m_StudentStat.m_StudentName;
                    }

                    Sprite _gmProfile = m_GameDesignerStudent.StudentProfileImg;
                    Sprite _artProfile = m_ArtStudent.StudentProfileImg;
                    Sprite _programmingProfile = m_ProgrammingStudent.StudentProfileImg;
                    Sprite _genreSprite = FindGenreSprite(m_GameJamHistory.ElementAt(i).Value[j].m_Genre);
                    Sprite _conceptSprite = m_ResultConceptSprite[m_GameJamHistory.ElementAt(i).Value[j].m_ConceptIndex];

                    string _funny = m_GameJamHistory.ElementAt(i).Value[j].m_Funny.ToString();
                    string _graphic = m_GameJamHistory.ElementAt(i).Value[j].m_Graphic.ToString();
                    string _perfection = m_GameJamHistory.ElementAt(i).Value[j].m_Perfection.ToString();
                    string _totalGenreScore = m_GameJamHistory.ElementAt(i).Value[j].m_TotalGenreScore.ToString();
                    string _score = m_GameJamHistory.ElementAt(i).Value[j].m_Score.ToString();
                    string _rank = m_GameJamHistory.ElementAt(i).Value[j].m_Rank;

                    if (_year == "0")
                    {
                        _year = GameTime.Instance.FlowTime.NowYear.ToString();
                    }

                    m_GameJamListPanel.ChangeGameInfo(_year + "���� " + _month + "��", _name, _name);
                    m_GameJamListPanel.ChangeStudentInfo(_gmName, _artName, _programmingName, _gmProfile, _artProfile,
                        _programmingProfile);
                    m_GameJamListPanel.ChangeGameJamScorePanel(_concept, _funny, _graphic, _perfection,
                        _totalGenreScore, _score, _rank); /// �л� �帣������ �߰�����
                    m_GameJamListPanel.ChangeGenreSprite(_genreSprite);
                    m_GameJamListPanel.ChangeMVPImageGameListPanel(Funny, Graphic, Perfection);
                    m_GameJamListPanel.ChangeConceptSprite(_conceptSprite);

                    int _gmValue = FindStudent(StudentType.GameDesigner, true,
                        (int)m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerEntryStudentStat1, (int)m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerEntryStudentStat2);
                    int _artValue = FindStudent(StudentType.Art, true,
                        (int)m_GameJamHistory.ElementAt(i).Value[j].m_ArtEntryStudentStat1, (int)m_GameJamHistory.ElementAt(i).Value[j].m_ArtEntryStudentStat2);
                    int _programmingValue = FindStudent(StudentType.Programming, true,
                        (int)m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingEntryStudentStat1, (int)m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingEntryStudentStat2);

                    int _gmNeedValue = SetDifficultyForPrevGame(i, j, StudentType.GameDesigner);
                    int _artNeedValue = SetDifficultyForPrevGame(i, j, StudentType.Art);
                    int _programmingNeedValue = SetDifficultyForPrevGame(i, j, StudentType.Programming);

                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.GameDesigner, _gmValue, _gmNeedValue);
                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.Art, _artValue, _artNeedValue);
                    m_GameJamListPanel.SetPreGameStatSliderValue(StudentType.Programming, _programmingValue, _programmingNeedValue);

                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerFirstNeedStatName, m_GameJamListPanel.GameDesignerRequiredStatImage1);
                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerSecondNeedStatName, m_GameJamListPanel.GameDesignerRequiredStatImage2);

                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_ArtFirstNeedStatName, m_GameJamListPanel.ArtRequiredStatImage1);
                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_ArtFirstNeedStatName, m_GameJamListPanel.ArtRequiredStatImage1);

                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingFirstNeedStatName, m_GameJamListPanel.ProgrammingRequiredStatImage1);
                    ChangeRequirementStatImage(m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingSecondNeedStatName, m_GameJamListPanel.ProgrammingRequiredStatImage1);

                    if (_gmValue >= _gmNeedValue)
                    {
                        m_GameJamListPanel.SetGameDesignerPreGameStatSliderColor(m_SatisfyStat);
                    }
                    else
                    {
                        m_GameJamListPanel.SetGameDesignerPreGameStatSliderColor(m_DissatisfactionStat);
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

    // ������ ������� �����뿡�� �䱸�ϴ� ������ ������ �����ߴ� ����� ���̵��� ���� ����������Ѵ�.
    private int SetDifficultyForPrevGame(int _gamejamHistoryKeyIndex, int _gamejamHistoryValueIndex, StudentType _type)
    {
        int _needStat = 0;

        GameJamInfo _findData = SearchAllGameJamInfo(m_GameJamHistory.ElementAt(_gamejamHistoryKeyIndex).Key);

        if (m_GameJamHistory.ElementAt(_gamejamHistoryKeyIndex).Value[_gamejamHistoryValueIndex].m_Difficulty == 100)
        {
            CalculateNeedStat(_type, _findData, ref _needStat, 20);
            return _needStat;
        }
        else if (m_GameJamHistory.ElementAt(_gamejamHistoryKeyIndex).Value[_gamejamHistoryValueIndex].m_Difficulty ==
                 200)
        {
            CalculateNeedStat(_type, _findData, ref _needStat, 10);
            return _needStat;
        }
        else
        {
            CalculateNeedStat(_type, _findData, ref _needStat, 0);
            return _needStat;
        }
    }

    // �а����� �ʼ� ������ �������ش�.
    private void CalculateNeedStat(StudentType _type, GameJamInfo _data, ref int _needStat, int _difficulty)
    {
        switch (_type)
        {
            case StudentType.GameDesigner:
            {
                for (int i = 0; i < _data.m_GjamNeedStatGameDesigner.Count; i++)
                {
                    if (_data.m_GjamNeedStatGameDesigner.ElementAt(i).Value != 0)
                    {
                        _needStat += (_data.m_GjamNeedStatGameDesigner.ElementAt(i).Value + _difficulty);
                    }
                }
            }
            break;
            case StudentType.Art:
            {
                for (int i = 0; i < _data.m_GjamNeedStatArt.Count; i++)
                {
                    if (_data.m_GjamNeedStatArt.ElementAt(i).Value != 0)
                    {
                        _needStat += (_data.m_GjamNeedStatArt.ElementAt(i).Value + _difficulty);
                    }
                }
            }
            break;
            case StudentType.Programming:
            {
                for (int i = 0; i < _data.m_GjamNeedStatProgramming.Count; i++)
                {
                    if (_data.m_GjamNeedStatProgramming.ElementAt(i).Value != 0)
                    {
                        _needStat += (_data.m_GjamNeedStatProgramming.ElementAt(i).Value + _difficulty);
                    }
                }
            }
            break;
        }
    }

    // �а����� �ʿ��� �ʼ� ���ȵ��� ��ųʸ��� �־��ִ� �Լ��̴�.
    private void ClassifyPartNeedStat(string _name, Dictionary<string, int> _info)
    {
        int _tempIndex = 0;

        if (_name == "��ȹ")
        {
            m_NeedGameDesignerStat.Clear();
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
                    m_NeedGameDesignerStat.Add(_statName, _value);
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
            FindOutRequiredStatCount(_name, _tempIndex, m_NeedGameDesignerStat);
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
        GameJamInfo _nowData = SearchAllGameJamInfo((int)m_RunningGameJameData.m_GameJamID);

        string _academyName = PlayerInfo.Instance.AcademyName;
        string _gameName = _nowData.m_GjamName;
        string _placeHolder = _gameName + m_GameJamEntryCount[_gameName].ToString();
        string _concept = _nowData.m_GjamConcept;
        Sprite _genreImage = FindGenreSprite(_genreName);

        m_GameJamResultPanel.ChangeConceptSprite(m_ResultConceptSprite[m_RunningGameJameData.m_ConceptIndex]);

        Funny = 0;
        Graphic = 0;
        Perfection = 0;
        m_TotalGenreScore = 0;

        CalculateStat(_studentList, m_RunningGameJameData.m_GameDesignerFirstNeedStatName, m_RunningGameJameData.m_GameDesignerSecondNeedStatName,
            (int)m_RunningGameJameData.m_GameDesignerFirstNeedStat, (int)m_RunningGameJameData.m_GameDesignerSecondNeedStat,
            m_RunningGameJameData.m_GameDesignerStudentName, StudentType.GameDesigner);

        CalculateStat(_studentList, m_RunningGameJameData.m_ArtFirstNeedStatName, m_RunningGameJameData.m_ArtSecondNeedStatName,
            (int)m_RunningGameJameData.m_ArtFirstNeedStat, (int)m_RunningGameJameData.m_ArtSecondNeedStat,
            m_RunningGameJameData.m_ArtStudentName, StudentType.Art);

        CalculateStat(_studentList, m_RunningGameJameData.m_ProgrammingFirstNeedStatName, m_RunningGameJameData.m_ProgrammingSecondNeedStatName,
            (int)m_RunningGameJameData.m_ProgrammingFirstNeedStat, (int)m_RunningGameJameData.m_ProgrammingSecondNeedStat,
            m_RunningGameJameData.m_ProgrammingStudentName, StudentType.Programming);

        double _score = m_GenreScore + _genreBonusScore + Funny + Graphic + Perfection;
        m_TotalGenreScore = m_GenreScore + _genreBonusScore + MiniGameScore;

        _score = Math.Truncate(_score);
        m_TotalGenreScore = Math.Truncate(m_TotalGenreScore);

        m_RunningGameJameData.m_Score = _score;

        CalculateRank(_score);

        m_GameJamResultPanel.SetPlusMinus(StudentType.GameDesigner, _rank,
            m_RunningGameJameData.m_GameDesignerStudentName, GameDesignerRequirementStatList, m_RunningGameJameData);
        m_GameJamResultPanel.SetPlusMinus(StudentType.Art, _rank, m_RunningGameJameData.m_ArtStudentName,
            ArtRequirementStatList, m_RunningGameJameData);
        m_GameJamResultPanel.SetPlusMinus(StudentType.Programming, _rank,
            m_RunningGameJameData.m_ProgrammingStudentName, ProgrammingRequirementStatList, m_RunningGameJameData);

        m_GameJamResultPanel.ChangeGenreSprite(_genreImage);

        if (_rank == "�̿ϼ�")
        {
            m_GameJamResultPanel.SetInfo(_academyName, _placeHolder, _concept, "-", "-", "-", "-", _rank, "-");
        }
        else
        {
            m_GameJamResultPanel.SetInfo(_academyName, _placeHolder, _concept, Funny.ToString(), Graphic.ToString(),
                Perfection.ToString(), _score.ToString(), _rank, m_TotalGenreScore.ToString());
        }

        m_GameJamResultPanel.ChangeMVPImageResultPanel(Funny, Graphic, Perfection);
    }

    // ������ ���â �� �ϳ��� ���� �����ߴ� �л����� ������ �����ִ� �г��� �������ִ� �Լ��̴�.
    private void SetGameJamNeedStatPanel(StudentType _type)
    {
        if (StudentType.GameDesigner == _type)
        {
            int _sliderValue = FindStudent(StudentType.GameDesigner, true, (int)m_RunningGameJameData.m_GameDesignerEntryStudentStat1, (int)m_RunningGameJameData.m_GameDesignerEntryStudentStat2);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.GameDesigner, _sliderValue, (int)m_RunningGameJameData.m_GameDesignerEntryStudentStat1, (int)m_RunningGameJameData.m_GameDesignerEntryStudentStat2);

            int _needValue = (int)(m_RunningGameJameData.m_GameDesignerFirstNeedStat + m_RunningGameJameData.m_GameDesignerSecondNeedStat);

            m_GameJamResultPanel.ChangeColorFillImage(StudentType.GameDesigner, _sliderValue, _needValue, m_SatisfyStat,
                m_DissatisfactionStat);
        }
        else if (StudentType.Art == _type)
        {
            int _sliderValue = FindStudent(StudentType.Art, true, (int)m_RunningGameJameData.m_ArtEntryStudentStat1, (int)m_RunningGameJameData.m_ArtEntryStudentStat2);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.Art, _sliderValue, (int)m_RunningGameJameData.m_ArtEntryStudentStat1, (int)m_RunningGameJameData.m_ArtEntryStudentStat2);

            int _needValue = (int)(m_RunningGameJameData.m_ArtFirstNeedStat + m_RunningGameJameData.m_ArtSecondNeedStat);

            m_GameJamResultPanel.ChangeColorFillImage(StudentType.Art, _sliderValue, _needValue, m_SatisfyStat,
                m_DissatisfactionStat);
        }
        else if (StudentType.Programming == _type)
        {
            int _sliderValue = FindStudent(StudentType.Programming, true, (int)m_RunningGameJameData.m_ProgrammingEntryStudentStat1, (int)m_RunningGameJameData.m_ProgrammingEntryStudentStat2);

            m_GameJamResultPanel.FillRequirementSlider(StudentType.Programming, _sliderValue, (int)m_RunningGameJameData.m_ProgrammingEntryStudentStat1, (int)m_RunningGameJameData.m_ProgrammingEntryStudentStat2);

            int _needValue = (int)(m_RunningGameJameData.m_ProgrammingFirstNeedStat + m_RunningGameJameData.m_ProgrammingSecondNeedStat);

            m_GameJamResultPanel.ChangeColorFillImage(StudentType.Programming, _sliderValue, _needValue, m_SatisfyStat,
                m_DissatisfactionStat);
        }
    }

    // �� �ջ��� ������ ���� ����� �����ش�.
    private string CalculateRank(double _totalScore)
    {
        string _successPercent = m_RunningGameJameData.m_SuccessPercent;

        if (_successPercent == "100%")
        {
            if (500 <= _totalScore)
            {
                _rank = "AAA";
                PlayerInfo.Instance.GameJamRankAUP++;
            }
            else if (300 <= _totalScore)
            {
                _rank = "AA";
                PlayerInfo.Instance.GameJamRankAUP++;
            }
            else if (100 <= _totalScore)
            {
                _rank = "A";
                PlayerInfo.Instance.GameJamRankAUP++;
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
    private void CalculateStat(List<Student> _list, string _statName1, string _statName2, int _stat1, int _stat2, string _studnetName, StudentType _type)
    {
        for (int i = 0; i < _list.Count;)
        {
            if (_list[i].m_StudentStat.m_StudentName == _studnetName)
            {
                if (_stat1 != 0)
                {
                    FindPartDataStat(_statName1, 1.8f, _list, i, _type);
                }
                else
                {
                    FindPartDataStat(_statName1, 0.4f, _list, i, _type);
                }

                if (_stat2 != 0)
                {
                    FindPartDataStat(_statName2, 1.8f, _list, i, _type);
                }
                else
                {
                    FindPartDataStat(_statName2, 0.4f, _list, i, _type);
                }

                _list[i].m_StudentStat.m_NumberOfEntries = 1;

                Funny = Math.Truncate(Funny);
                Graphic = Math.Truncate(Graphic);
                Perfection = Math.Truncate(Perfection);
                return;
            }

            i++;
        }
    }

    // ���� ������ �л��� ������ ������ش�.
    private void FindPartDataStat(string _statName, float _value, List<Student> _list, int _listIndex, StudentType _type)
    {
        int _tempIndex = 0;

        for (int j = 0; j < AbilityNameList.Length; j++)
        {
            if (_statName == AbilityNameList[j])
            {
                _tempIndex = j;
                break;
            }
        }

        if (_list[_listIndex].m_StudentStat.m_StudentType == StudentType.GameDesigner)
        {
            Funny += _list[_listIndex].m_StudentStat.m_AbilityAmountArr[_tempIndex] * _value; // ���ٷ� ����� ������ funny�� ��Ÿ �� �迭�� �ٲٱ�
        }
        else if (_list[_listIndex].m_StudentStat.m_StudentType == StudentType.Art)
        {
            Graphic += _list[_listIndex].m_StudentStat.m_AbilityAmountArr[_tempIndex] * _value;
        }
        else
        {
            Perfection += _list[_listIndex].m_StudentStat.m_AbilityAmountArr[_tempIndex] * _value;
        }
    }

    // ���õ� �л����� ������ ������ �帣�� ���ϴ� �Լ�
    private void DecideGenre()
    {
        List<GenreValuePair> _GameDesignerGenreList = SortGenreDic(m_ClickStudent[0]);
        List<GenreValuePair> _ArtGenreList = SortGenreDic(m_ClickStudent[1]);
        List<GenreValuePair> _ProgrammingGenreList = SortGenreDic(m_ClickStudent[2]);

        _genreBonusScore = 15;
        m_GenreScore = 0;

        List<GenreValuePair> _list = new List<GenreValuePair>();

        for (int i = 0; i < 2; i++)
        {
            _list.Add(_GameDesignerGenreList[i]);
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

        FindStudentGenreStat(_GameDesignerGenreList, _list[0].genre);
        FindStudentGenreStat(_ArtGenreList, _list[0].genre);
        FindStudentGenreStat(_ProgrammingGenreList, _list[0].genre);

        m_GameJamPanel.ChangeExpectedGenreText(_list[0].genre);

        _genreName = ChangeGenreListName(_list[0].genre);

        if (_genreName == m_NowGameJamInfo.m_GjamMainGenre)
        {
            _genreBonusScore *= 1.4f;
        }

        if (_genreName == m_NowGameJamInfo.m_GjamSubGenre)
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
    private List<GenreValuePair> SortGenreDic(Student _student)
    {
        //Dictionary<string, int> _returnGenreValue = new Dictionary<string, int>();  // ������ �帣�� ��ȯ���� ��ųʸ�

        //Dictionary<string, int> _tempGenreValue = new Dictionary<string, int>();    // �帣�� �����ϱ� ���� ��� ��Ƶ� ��ųʸ�

        List<GenreValuePair> _list = new List<GenreValuePair>();

        for (int i = 0; i < 8; ++i)
        {
            GenreValuePair _temp = new GenreValuePair();
            _temp.genre = (GenreStat)i;
            _temp.value = _student.m_StudentStat.m_GenreAmountArr[i];
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
        double _GameDesignerStat = (m_NeedGameDesignerStat.Count > 1)
            ? m_NeedGameDesignerStat.ElementAt(0).Value + m_NeedGameDesignerStat.ElementAt(1).Value
            : m_NeedGameDesignerStat.ElementAt(0).Value;

        double _ArtStat = (m_NeedArtStat.Count > 1)
            ? m_NeedArtStat.ElementAt(0).Value + m_NeedArtStat.ElementAt(1).Value
            : m_NeedArtStat.ElementAt(0).Value;

        double _ProgrammingStat = (m_NeedProgrammingStat.Count > 1)
            ? m_NeedProgrammingStat.ElementAt(0).Value + m_NeedProgrammingStat.ElementAt(1).Value
            : m_NeedProgrammingStat.ElementAt(0).Value;

        double _GameDesignerStudentStat = m_Requirement1List[(int)StudentType.GameDesigner] +
                                          m_Requirement2List[(int)StudentType.GameDesigner];
        double _ArtStudentStat = m_Requirement1List[(int)StudentType.Art] + m_Requirement2List[(int)StudentType.Art];
        double _ProgrammingStudentStat = m_Requirement1List[(int)StudentType.Programming] +
                                         m_Requirement2List[(int)StudentType.Programming];

        //  80%�� ������ 100%�� ó��
        double _GameDesignerPercent = (_GameDesignerStudentStat / _GameDesignerStat * 100 >= 80)
            ? 100
            : _GameDesignerStudentStat / _GameDesignerStat * 100;
        double _ArtPercent = (_ArtStudentStat / _ArtStat * 100 >= 80) ? 100 : _ArtStudentStat / _ArtStat * 100;
        double _ProgrammingPercent = (_ProgrammingStudentStat / _ProgrammingStat * 100 >= 80)
            ? 100
            : _ProgrammingStudentStat / _ProgrammingStat * 100;

        double _totalPercent = _GameDesignerPercent + _ArtPercent + _ProgrammingPercent;
        _average = _totalPercent / 3;

        // �ϳ��� 80�� ���̸� �ϼ� Ȯ���� ����ְ� �ƴ϶�� 100���� ����ֱ�
        if (_GameDesignerPercent < 80 || _ArtPercent < 80 || _ProgrammingPercent < 80)
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
                if (m_GameJamResultPanel._changeGameName == m_GameJamHistory.ElementAt(i).Value[j].m_GameName)
                {
                    m_OnNoticePanel.TurnOnUI();
                    m_OffNoticePanel.DelayTurnOffUI();
                    m_GameJamResultPanel.SetChangeGameName();
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
        #region _�� �����ִ� ��ư�� ���̶���Ʈ ���ֱ�
        if (m_PrevRankButtonObj != null)
        {
            Button _prevButton = m_PrevRankButtonObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        m_PrevRankButtonObj = _currentObj;

        Button _currentButton = _currentObj.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_RankButtonHighLightColor;
        _currentButtonColor.highlightedColor = m_RankButtonHighLightColor;
        _currentButtonColor.pressedColor = m_RankButtonHighLightColor;
        _currentButtonColor.selectedColor = m_RankButtonHighLightColor;

        _currentButton.colors = _currentButtonColor;
        #endregion

        m_GameJamListPanel.ClickRankButton();

        _rankButtonName = _currentObj.name;

        bool _isActive = m_GameJamListPanel.IsInfoPanelActive();

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

    // �帣 ��ư�� ������ �� ��Ÿ�� �ֵ�
    public void ClickGenreButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        string _buttonName = _currentObj.name;

        _genreButtonName =
            _currentObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text; // ��ư�� �޸� �ѱ� �̸��� �����´�.

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
                    if (m_GameJamHistory.ElementAt(i).Value[j].m_Rank == _findRank)
                    {
                        SetPrevGameInfo(i, j);
                    }
                }
                else if (_findRank == "All" && _findGenre != "All")
                {
                    SetPrevGameInfo(i, j);
                }
                else if (_findRank != "All" && _findGenre != "All")
                {
                    if (m_GameJamHistory.ElementAt(i).Value[j].m_Genre == _findGenre &&
                        m_GameJamHistory.ElementAt(i).Value[j].m_Rank == _findRank)
                    {
                        SetPrevGameInfo(i, j);
                    }
                }
            }
        }
    }

    private void SetPrevGameInfo(int _keyIndex, int _valueIndex)
    {
        GameObject _gameJamListPrefab = Instantiate(m_GameJamListPrefab);
        m_GameJamListPanel.MoveGameJamList(_gameJamListPrefab,
            m_GameJamListPanel._gamejameContentParent);

        string _gameName = m_GameJamHistory.ElementAt(_keyIndex).Value[_valueIndex].m_GameName;
        string _genreName = m_GameJamHistory.ElementAt(_keyIndex).Value[_valueIndex].m_Genre;
        string _rank = m_GameJamHistory.ElementAt(_keyIndex).Value[_valueIndex].m_Rank;
        Sprite _genre = FindGenreSprite(_genreName);
        Sprite _concept = m_ResultConceptSprite[m_GameJamHistory.ElementAt(_keyIndex).Value[_valueIndex].m_ConceptIndex];

        _gameJamListPrefab.name = _gameName;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameConceptImage.sprite = _concept;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameName.text = _gameName;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreName.text = _genreName;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_Rank.text = _rank;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GenreImage.sprite = _genre;

        GameObject _gameList = _gameJamListPrefab.GetComponent<GameJamListPrefab>()
            .m_GameListPrefabButton.gameObject;
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
            .AddListener(() => MakePreGameJamInfo(_gameList));
        _gameJamListPrefab.GetComponent<GameJamListPrefab>().m_GameListPrefabButton.onClick
            .AddListener(m_GameJamListPanel.ClickGameJamPrefab);
    }

    // �����ߴ� �л��� ü���� ����, ������ �ٲ��ش�.
    private void ChangeStudentData(string _name, int[] _ability)
    {
        List<Student> _studentList = ObjectManager.Instance.m_StudentList;

        GameJamInfo _nowData = SearchAllGameJamInfo((int)m_RunningGameJameData.m_GameJamID);

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_name == _studentList[i].m_StudentStat.m_StudentName)
            {
                for (int j = 0; j < 5; j++)
                {
                    _studentList[i].m_StudentStat.m_AbilityAmountArr[j] += _ability[j];
                }

                _studentList[i].m_StudentStat.m_Health -= _nowData.m_StudentHealth;
                _studentList[i].m_StudentStat.m_Passion -= _nowData.m_StudentPassion;
            }
        }
    }

    private void SetRiseStat(string _rank, int[] _arr, ref int[] _riseArr)
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

#if UNITY_EDITOR || UNITY_EDITOR_WIN
    [ContextMenu("ResultConceptSprite")]
    private void FillResultConceptSprite()
    {
        Sprite[] _allConceptSprite = Resources.LoadAll<Sprite>("GameJamConcept");
        m_ResultConceptSprite = new Sprite[_allConceptSprite.Length];

        for (int i = 0; i < m_ResultConceptSprite.Length; i++)
        {
            m_ResultConceptSprite[i] = _allConceptSprite[i];
        }
    }
#endif
}