using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Coffee.UIExtensions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum Assessment
{
    Terrible, // �־�
    Bad, // ����
    NotBad,
    SoSo, // ����
    Good, // ����
    Great, // ��������
    Excellent, // �ֻ�
    None
}

public class GameShow : MonoBehaviour
{
    public delegate void GameShowDataChange();

    public static event GameShowDataChange GameShowDataChangeEvent;

    [SerializeField] private GameJam m_GameJam;
    [SerializeField] private GameShowPanel m_GameShowPanel;
    [SerializeField] private GameShowAnimationPanel m_GameShowAnimationPanel;
    [SerializeField] private RewardPanel m_RewardPanel;
    [SerializeField] private SoundManager m_SoundManager;
    [SerializeField] private ActivityEvent m_ActivityEvent;
    [SerializeField] private GameObject m_SliderBarArlam;
    [SerializeField] private GameObject m_GameShowArlam;
    [SerializeField] private GameObject m_GameShowListPrefab; // ���Ӽ� ������
    [SerializeField] private GameObject m_PrevGameListPrefab; // �����뿡�� ���� ���ӵ��� ������

    [SerializeField] private GameObject m_RewardPrefab;

    [SerializeField] private TextMeshProUGUI m_NoveltyScript; // �������� 1�̸��� �� ����� �г��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI m_GoTOGameShowResultPanel; // ���Ӽ��� ����� �������� ���� ����ִ� �г��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI m_GameShowResultPanelText; // ���Ӽ��� ����� �����ֱ� ���� ����ִ� �г��� �ؽ�Ʈ
    [SerializeField] private Sprite[] m_RewardSprite;
    [Space(5f)]
    [Header("���ٰ� ���� �гε��� PopUp, PopOff ������Ʈ��")]
    [SerializeField] private PopUpUI m_PopUpResultPanel; // ���Ӽ� ���â�� ������ popUPâ
    [SerializeField] private PopOffUI m_PopOffResultPanel;
    [SerializeField] private PopUpUI m_PopUpResultAnimationPanel; // ���Ӽ� ����� ���� �ִϸ��̼��� ������ �г�
    [SerializeField] private PopOffUI m_PopOffResultAnimationPanel;
    [SerializeField] private PopUpUI m_PopUpNoveltyWarningPanel; // �������� 1�̸����� ������ �� ����� �г�
    [SerializeField] private PopOffUI m_PopOffNoveltyWarningPanel;
    [SerializeField] private PopUpUI m_PopUpResultScriptPanel; // ���â���� ������ �ޱ��� �����ִ� ��ũ��Ʈ �г�
    [SerializeField] private PopOffUI m_PopOffResultScriptPanel;
    [SerializeField] private PopUpUI m_PopUpRewardPanel; // ���� �г�
    [SerializeField] private PopOffUI m_PopOffRewardPanel;

    [Space(5f)]
    [Header("Ʃ�丮���")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_GameShowButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_ClassResultPanel;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;

    [Space(5f)]
    /// ���Ӽ� ���ð� �Ǻ��� �ʿ��� �����͵��� ��Ƶ� ����Ʈ��
    private List<GameDifficulty> m_Difficulty = new List<GameDifficulty>(); // �� ��ī���� ��޿� ���� ���̵��� �����ϴ� ����Ʈ

    private List<DifficultyPreset> m_HeartPresetList = new List<DifficultyPreset>();

    private List<GameShowRewardByDifficulty> m_Assessnent = new List<GameShowRewardByDifficulty>(); // ���̵��� �򰡹����� ���� ������ �����ϴ� ����Ʈ

    private List<GameShowScoreByHeart> m_ScoreByHeartList = new List<GameShowScoreByHeart>();
    private List<EvaluationScoreRange> m_ScoreRangeList = new List<EvaluationScoreRange>(); // �� ������ ���� ����
    private List<ScoreConversion> m_ScoreConversionList = new List<ScoreConversion>(); // �� ������ ���� �������� ���� ȯ��
    private List<FinalScoreRange> m_FinalScoreRangeList = new List<FinalScoreRange>(); //

    private List<AssessmentResponse> m_AssessmentResponseList = new List<AssessmentResponse>(); // �� ��Ʈ���� ������ ���� ������� �ؽ�Ʈ��

    private List<ResultScript> m_ResultScriptList = new List<ResultScript>(); // ������ ��� �гο� ����� ��ũ��Ʈ ����
    private List<string> m_NoveltyScriptList = new List<string>(); // ������ ������ 1�� �ƴ� �� ����� ��ũ��Ʈ��
    private List<string> m_GotoGameShowScriptList = new List<string>(); // ���Ӽ����� �̵��� �� ����� ��ũ��Ʈ��(���â �̵��ϴ� �г�)

    ///
    private List<GameShowData> m_TotalGameShowData = new List<GameShowData>(); // �̹��� ���� ����� ���Ӽ���� ��� ��Ƴ��� ����Ʈ

    private static List<GameShowData> m_FixedGameShowData = new List<GameShowData>();
    private static List<GameShowData> m_RandomGameShowData = new List<GameShowData>();

    private GameShowSaveData _mTemporaryData = new GameShowSaveData(); // ���� Ŭ���� ���Ӽ���� ������ �ӽ÷� �������ִ� ����
    private List<GameShowSaveData> m_ToBeRunningGameShow = new List<GameShowSaveData>(); // ���� ���� �����ϴ� ���Ӽ���� ��Ƶ� ����Ʈ

    private Queue<Action> eventQueue = new Queue<Action>(); // �������� ����ǰ� ���� ���Ӽ ������ �غ� ������Ѵ�. timescale�� 0�� �� �̺�Ʈ�� �����ϴ� �ȵż� Update������ timescale�� 0 ���� Ŭ �� ��������ִ°ɷ� ����

    public static Dictionary<int, List<GameShowSaveData>> m_GameShowHistory = new Dictionary<int, List<GameShowSaveData>>();

    private Queue<string> m_ResultDialogQueue = new Queue<string>(); // ������ ���â�� ����� ��ũ��Ʈ���� ��� ������� �־��� ť
    private Queue<string> m_NoveltyDialoge = new Queue<string>();
    public Sprite m_ApplySuccessSprite;
    public Sprite m_NotAllowedApplySprite;
    public Sprite m_ApplySprite;
    public Sprite[] m_FinalScoreSprite;
    public Sprite[] m_EmojiSprite;
    public Sprite[] m_StudentFace;
    private GameObject m_PrevClickObj;                                          // ���� ������ Ŭ���� ������ ��ư�� ��Ƶ� ����
    private GameObject m_PrevClickGameShowObj;                                   // ������ ��ư ��������Ʈ ������ ���� ���� Ŭ���� ������ ������Ʈ�� ��Ƶ� ����
    private Color m_HighLightColor;
    private IEnumerator m_SetSatisfactionStatusPanelCor;
    private IEnumerator m_ChangeAnimationPanel;

    private int m_NowMonth;
    private int m_MonthLimit;
    private int m_GenreAndConceptHeart = 0;
    private int m_RewardMoney;
    private int m_RewardFamous;
    private int m_RewardPassion;
    private int m_RewardHealth;
    private float m_ScrollSpeed = 0.5f;
    private float _novelty = 1f;
    private string m_ResultScriptTexet;
    private bool m_IsChangeMonth;
    private bool _isCanParticipate;
    private bool _isSameDayInList;
    private bool _isPrevGameJamButtonClick;
    private bool _isFirstGameShow;
    private Vector3 m_TargetPosition;
    private Vector3 m_StartPosition;

    private int m_TutorialCount;
    private int m_ScriptCount;

    private void OnEnable()
    {
        SlideMenuPanel.OnGameShowListButtonClicked += HandleGameShowListButtonClicked;
        GameShowPanel.OnGameShowListPanelApplyButtonClicked += HandleGameShowListApplyButton;
        //GameShowPanel.OnGameShowListPanelSelectButtonClicked += HandleGameShowListSelectButton;
        InGameUI.OnGameShowRsultButtonClicked += HandleGameShowResultButton;
        GameJam.DataChangedEvent += CombineGameShowData;
    }

    private void OnDisable()
    {
        SlideMenuPanel.OnGameShowListButtonClicked -= HandleGameShowListButtonClicked;
        GameShowPanel.OnGameShowListPanelApplyButtonClicked -= HandleGameShowListApplyButton;
        //GameShowPanel.OnGameShowListPanelSelectButtonClicked -= HandleGameShowListSelectButton;
        InGameUI.OnGameShowRsultButtonClicked -= HandleGameShowResultButton;
        GameJam.DataChangedEvent -= CombineGameShowData;
    }

    private void Start()
    {
        InitGameShowDifficultry();
        InitGameShowReward();
        InitHeartScoreByLevel();
        InitScoreRangeList();
        InitScoreAssessment();
        InitFinalScoreList();
        InitEvaluationResponseList();
        InitNoveltyScriptList();
        InitGotoGameShowScriptList();
        InitResultPanelScript();
        InitHeartPresetList();
        ClassifyGameShowData();
        m_MonthLimit = 2;
        m_GameShowPanel.InitGameShowPanel();
        m_GameShowHistory.Clear();
        m_TutorialCount = 0;
        m_ScriptCount = 0;
        m_HighLightColor = new Color(1f, 0.8475146f, 0, 1f);
        _isPrevGameJamButtonClick = false;
        _isFirstGameShow = true;
        m_NextButton.onClick.AddListener(TutorialContinue);
        m_NextButton.onClick.AddListener(ClickEventManager.Instance.Sound.PlayIconTouch);
        m_SetSatisfactionStatusPanelCor = SetSatisfactionStatusPanel();
        m_ChangeAnimationPanel = ChangeAnimationPanel();

        if (Json.Instance.UseLoadingData)
        {
            DistributeGameShowData();
        }
    }

    private static GameShowData SearchGameShowInfo(int _gameShowID)
    {
        foreach (GameShowData gameShow in m_FixedGameShowData)
        {
            if (gameShow.GameShow_ID == _gameShowID)
            {
                return gameShow;
            }
        }

        foreach (GameShowData gameShow in m_RandomGameShowData)
        {
            if (gameShow.GameShow_ID == _gameShowID)
            {
                return gameShow;
            }
        }

        return null;
    }

    private GameShowData SearchGameShowDataForWeek(int _gameShowID)
    {
        foreach (GameShowData gameshow in m_TotalGameShowData)
        {
            if (gameshow.GameShow_ID == _gameShowID)
            {
                return gameshow;
            }
        }

        return null;
    }

    private void Update()
    {
        if (eventQueue.Count > 0 && Time.timeScale > 0)
        {
            eventQueue.Dequeue()?.Invoke();
        }

        if (m_NowMonth != GameTime.Instance.FlowTime.NowMonth /*&& GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 3*/)
        {
            m_TotalGameShowData.Clear();
            SetActiveAlram(false);
            m_GameShowPanel.DestroyObj(m_GameShowPanel.GameShowListContentObj);
            m_NowMonth = GameTime.Instance.FlowTime.NowMonth;
            m_MonthLimit = 2;
            m_IsChangeMonth = true;
        }

        if (m_ToBeRunningGameShow.Count != 0)
        {
            for (int i = 0; i < m_ToBeRunningGameShow.Count; i++)
            {
                GameShowData _nowGameShow = SearchGameShowInfo((int)m_ToBeRunningGameShow[i].m_GameShowID);

                if (m_ToBeRunningGameShow[i].m_MakeMonth == GameTime.Instance.FlowTime.NowMonth
                    && m_ToBeRunningGameShow[i].m_MakeWeek == GameTime.Instance.FlowTime.NowWeek
                    && m_ToBeRunningGameShow[i].m_MakeDay == GameTime.Instance.FlowTime.NowDay
                    && m_ActivityEvent.m_IsCheckGameShow == false && m_ClassResultPanel.activeSelf == false)
                {
                    _mTemporaryData = m_ToBeRunningGameShow[i];
                    GetGameShowResultPanelScript();
                }
            }
        }

        if (m_IsChangeMonth && m_NowMonth != 1 && m_NowMonth != 2)
        {
            m_IsChangeMonth = false;

            if (GameJam.m_GameJamHistory.Count != 0 && m_GameShowPanel.GameShowListContent.childCount == 0)
            {
                CombineGameShowData();
            }
        }

        //        if (PlayerInfo.Instance.IsFirstGameShow && m_TutorialCount > 0)
        //        {
        ////#if UNITY_EDITOR|| UNITY_EDITOR_WIN
        ////            if (Input.GetMouseButtonDown(0))
        ////            {
        ////                TutorialContinue();
        ////                //ClickEventManager.Instance.Sound.PlayIconTouch();
        ////            }
        ////#elif UNITY_ANDROID
        ////            if (Input.touchCount > 0)
        ////            {
        ////                Touch touch = Input.GetTouch(0);
        ////                if (touch.phase == TouchPhase.Ended)
        ////                {
        ////                    TutorialContinue();
        ////                    //ClickEventManager.Instance.Sound.PlayIconTouch();
        ////                }
        ////            }
        ////#endif
        //        }

        if (PlayerInfo.Instance.IsFirstGameShow && m_GameShowButton.GetComponent<Button>().interactable &&
            m_TutorialCount == 0 && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            m_TutorialPanel.SetActive(true);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_NextButton.gameObject.SetActive(true);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    public void CollectGameShowData()
    {
        List<GameShowSaveData> runData = new List<GameShowSaveData>(m_ToBeRunningGameShow);
        Dictionary<int, List<GameShowSaveData>>
            history = new Dictionary<int, List<GameShowSaveData>>(m_GameShowHistory);

        AllInOneData.Instance.GameShowToBeRunning = runData;
        AllInOneData.Instance.GameShowHistory = history;
    }

    private void DistributeGameShowData()
    {
        List<GameShowSaveData> runData = new List<GameShowSaveData>(AllInOneData.Instance.GameShowToBeRunning);
        Dictionary<int, List<GameShowSaveData>> history =
            new Dictionary<int, List<GameShowSaveData>>(AllInOneData.Instance.GameShowHistory);

        m_ToBeRunningGameShow = runData;
        m_GameShowHistory = history;

        if (runData.Count != 0)
        {
            for (int i = 0; i < runData.Count; i++)
            {
                GameShowData _gameshow = SearchGameShowInfo((int)runData[i].m_GameShowID);

                GameObject eventPrefab = m_ActivityEvent.MakeDistributeEventPrefab(_gameshow.GameShow_Name, (int)runData[i].m_MakeMonth, (int)runData[i].m_MakeWeek, (int)runData[i].m_MakeDay,
                    true, (int)runData[i].m_MakeMonth);
                eventPrefab.transform.GetComponent<EventPrefab>().m_IsGameJam = false;
                m_ActivityEvent.SetEventPanelActive(true);

                StartCoroutine(m_ActivityEvent.TypingText(eventPrefab, "���Ӽ� �غ���"));
            }
        }
    }

    private void TutorialContinue()
    {
        if (m_TutorialCount == 1)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 2)
        {
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 3)
        {
            if (m_FoldButton.GetComponent<PopUpUI>().isSlideMenuPanelOpend == false)
            {
                m_FoldButton.GetComponent<PopUpUI>().AutoSlideMenuUI();
            }

            m_BlackScreen.SetActive(true);
            m_PDAlarm.SetActive(false);
            m_Unmask.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(false);
            m_Unmask.fitTarget = m_GameShowButton.GetComponent<RectTransform>();
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 200, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 5)
        {
            m_Unmask.fitTarget = m_GameShowPanel.GameShowRect;
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 150, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 7)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 8)
        {
            m_Unmask.fitTarget = m_GameShowPanel.GameShowInfoRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 300, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 9)
        {
            m_Unmask.fitTarget = m_GameShowPanel.NeedElementRect;
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 300, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 10)
        {
            m_Unmask.fitTarget = m_GameShowPanel.SelectPanelRect;
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-580, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 11)
        {
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_ScriptCount++;
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 12)
        {
            m_Unmask.fitTarget = m_GameShowPanel.PrevGameJamListContent.GetChild(0).GetComponent<RectTransform>();
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(false);
            m_TutorialArrowImage.transform.Rotate(0, 0, 90);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-500, 0, 0);
            m_TutorialCount++;
        }
        else if (m_TutorialCount == 15)
        {
            m_TutorialPanel.SetActive(false);
            m_NextButton.gameObject.SetActive(false);
            PlayerInfo.Instance.IsFirstGameShow = false;
        }
    }

    private void ClassifyGameShowData()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalRandomGameShowData.Count; i++)
        {
            if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Fix)
            {
                m_FixedGameShowData.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
            }
            else
            {
                m_RandomGameShowData.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
            }
        }
    }

    public void SetActiveAlram(bool _isFirstAlram = false)
    {
        m_SliderBarArlam.SetActive(_isFirstAlram);
        m_GameShowArlam.SetActive(_isFirstAlram);
    }

    public void EnqueueDataChangedEvent()
    {
        eventQueue.Enqueue(() => GameShowDataChangeEvent?.Invoke());
    }

    // ���Ӽ� ��ư�� ������ ��������� �Լ�
    public void MakeGameShowList()
    {
        SortRecommendList();
        //CombineGameShowData();
        if (m_TotalGameShowData.Count == 0)
        {
            m_GameShowPanel.SetNoGameShow(true);
        }
        else
        {
            for (int i = 0; i < m_TotalGameShowData.Count; i++)
            {
                GameObject _gameShowList = Instantiate(m_GameShowListPrefab, m_GameShowPanel.GameShowListContent);

                _gameShowList.name = m_TotalGameShowData[i].GameShow_Name;
                _gameShowList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    m_TotalGameShowData[i].GameShow_Name;

                if (!m_GameShowHistory.ContainsKey(m_TotalGameShowData[i].GameShow_ID))
                {
                    _gameShowList.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    _gameShowList.transform.GetChild(1).gameObject.SetActive(false);
                }

                _gameShowList.GetComponent<Button>().onClick.AddListener(ClickGameShowListButton);
            }

            SetFirstGameShowInfo();
        }

    }

    public void CombineGameShowData()
    {
        BringFixedGameShowData();
        BringRandomGameShowData();
    }

    // ���� �гο��� �ޱ� ��ư�� ������ �� 
    public void ClickCloseRewardPanel()
    {
        m_PopOffRewardPanel.TurnOffUI();

        PlayerInfo.Instance.MyMoney += m_RewardMoney;
        PlayerInfo.Instance.Famous += m_RewardFamous;

        Student _gameDesignerStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);
        Student _artStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ArtStudentName);
        Student _programmingStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ProgrammingStudentName);

        _gameDesignerStudent.m_StudentStat.m_Health += m_RewardHealth;
        _gameDesignerStudent.m_StudentStat.m_Passion += m_RewardPassion;

        _artStudent.m_StudentStat.m_Health += m_RewardHealth;
        _artStudent.m_StudentStat.m_Passion += m_RewardPassion;

        _programmingStudent.m_StudentStat.m_Health += m_RewardHealth;
        _programmingStudent.m_StudentStat.m_Passion += m_RewardPassion;
    }

    #region _delegate�� ����� �Լ���

    // ���Ӽ ���ִ� ��ư�� Ŭ���Ǿ��� �� ������ �Լ���.
    private void HandleGameShowListButtonClicked()
    {
        MakeGameShowList();
        MakePrevGameJamForToBeRunning();
        m_GameShowPanel.SetActiveGameShowPanel(true);
        m_GameShowPanel.ClickSelectButton();
        _isPrevGameJamButtonClick = false;

        if (m_TutorialCount == 4)
        {
            m_GameShowPanel.GameShowScrollView.GetComponent<ScrollRect>().vertical = false;
            m_Unmask.fitTarget = m_GameShowPanel.GameShowScrollView.GetComponent<RectTransform>();
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_TutorialTextImage.gameObject.SetActive(true);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameJamTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(500, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // ��û ��ư�� Ŭ���Ǿ��� �� ���� �� �Լ�.
    private void HandleGameShowListApplyButton()
    {
        if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_ApplySprite && m_MonthLimit > 0)
        {
            //// �̶� ��û �Ϸ��ϰ� ��û��ư ��û�Ϸ�� ����
            if (_isCanParticipate && _isSameDayInList)
            {
                ApplySuccess();
            }
            else if (!_isSameDayInList)
            {
                m_GameShowPanel.SetConditionFailPanel("�̹� �ش� ��¥�� ������ ���Ӽ �ֽ��ϴ�!");
            }
            else if (!_isPrevGameJamButtonClick)
            {
                m_GameShowPanel.SetConditionFailPanel("��ǰ�� ������ ������ �ּ���!");
            }
            // �ƴ϶�� ��û�� �� ���ٰ� ��� �޼��� ����ֱ�
            else
            {
                ApplyFailed();
            }
        }
        // ����Ƚ�� ��� ��� 
        else if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_NotAllowedApplySprite &&
                 m_MonthLimit <= 0)
        {
            // ��ư Ŭ���� ������ �� �ִ� Ƚ���� �ʰ��ߴٴ� �޼��� ����ֱ�
            m_GameShowPanel.SetConditionFailPanel("������ �� �ִ� Ƚ���� �ʰ��Ͽ����ϴ�!\n���� ���� ���������.");
        }
        // else : ��û�ߴ� ��ư�� ������ ���� �� ����.

        if (m_TutorialCount == 14)
        {
            //m_TutorialPanel.SetActive(false);
            //PlayerInfo.Instance.IsFirstGameShow = false;

            m_TutorialArrowImage.gameObject.SetActive(false);
            m_Unmask.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(true);
            m_PDAlarm.SetActive(true);
            m_AlarmText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];

            m_TutorialCount++;
            m_ScriptCount++;
        }
    }

    private void HandleGameShowResultButton()
    {
        m_PopOffResultPanel.TurnOffUI();
        CalculateScore();
        TurnAnimationByScore((Assessment)_mTemporaryData.m_GameShowResultAssessment);
    }

    #endregion

    // ��������ϴ� �Լ�
    private void CalculateScore()
    {
        GameShowData gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        int _standardGenreAndConcept =
            GetScoreByHeart(gameShowData.GameShow_Level, (int)_mTemporaryData.m_GameShowGenreAndConceptHeart);
        int _standardGenre = GetScoreByHeart(gameShowData.GameShow_Level, (int)_mTemporaryData.m_GenreHeart);
        int _standardFun = GetScoreByHeart(gameShowData.GameShow_Level, (int)_mTemporaryData.m_FunnyHeart);
        int _standardGraphic = GetScoreByHeart(gameShowData.GameShow_Level, (int)_mTemporaryData.m_GraphicHeart);
        int _standardPercefication = GetScoreByHeart(gameShowData.GameShow_Level, (int)_mTemporaryData.m_PerceficationHeart);

        int _selectGameJamIndex = GameJam.m_GameJamHistory[(int)_mTemporaryData.m_GameJamID]
            .FindIndex(x => x.m_GameName == (string)_mTemporaryData.m_GameJamName);
        int _myGenre = (int)GameJam.m_GameJamHistory[(int)_mTemporaryData.m_GameJamID][_selectGameJamIndex]
            .m_TotalGenreScore;
        int _myFunny = (int)GameJam.m_GameJamHistory[(int)_mTemporaryData.m_GameJamID][_selectGameJamIndex].m_Funny;
        int _myGraphic = (int)GameJam.m_GameJamHistory[(int)_mTemporaryData.m_GameJamID][_selectGameJamIndex].m_Graphic;
        int _myPerfecation =
            (int)GameJam.m_GameJamHistory[(int)_mTemporaryData.m_GameJamID][_selectGameJamIndex].m_Perfection;

        Assessment _genre = GetGameShow((int)Math.Floor(_myGenre * _novelty) - _standardGenre);
        int _genreScore = GetScoreConversion(_genre);

        Assessment _funny = GetGameShow((int)Math.Floor(_myFunny * _novelty) - _standardFun);
        int _funnyScore = GetScoreConversion(_funny);

        Assessment _graphic = GetGameShow((int)Math.Floor(_myGraphic * _novelty) - _standardGraphic);
        int _graphicScore = GetScoreConversion(_graphic);

        Assessment _perfecation = GetGameShow((int)Math.Floor(_myPerfecation * _novelty) - _standardPercefication);
        int _perfecationScore = GetScoreConversion(_perfecation);

        Assessment _genreAndConcept =
            GetGameShow((int)Math.Floor(_standardGenreAndConcept * _novelty) - _standardGenreAndConcept);
        int _genreAndConceptScore = GetScoreConversion(_genreAndConcept);

        int _totalSocre = _genreScore + _funnyScore + _graphicScore + _perfecationScore + _genreAndConceptScore;

        _mTemporaryData.m_FunScore = _funnyScore;
        _mTemporaryData.m_FunAssessment = (int)_funny;

        _mTemporaryData.m_GenreScore = _genreScore;
        _mTemporaryData.m_GenreAssessment = (int)_genre;

        _mTemporaryData.m_PerfectionScore = _perfecationScore;
        _mTemporaryData.m_PerfectionAssessment = (int)_perfecation;

        _mTemporaryData.m_GraphicScore = _graphicScore;
        _mTemporaryData.m_GraphicAssessment = (int)_graphic;

        _mTemporaryData.m_ConceptScore = _genreAndConceptScore;
        _mTemporaryData.m_ConceptAssessment = (int)_genreAndConcept;

        _mTemporaryData.m_GameShowResultAssessment = (int)FinalGameShowScore(_totalSocre);

        // ���Ӽ ������ �������� ������ �� �ʿ�
        if (m_GameShowHistory.ContainsKey(gameShowData.GameShow_ID))
        {
            m_GameShowHistory[gameShowData.GameShow_ID].Add(_mTemporaryData);
        }
        else
        {
            List<GameShowSaveData> m_GameshowDataList = new List<GameShowSaveData>();
            m_GameshowDataList.Add(_mTemporaryData);
            m_GameShowHistory.Add(gameShowData.GameShow_ID, m_GameshowDataList);
        }
    }

    // ���� ������ ���� ����� �ִϸ��̼��� �ٸ��� ���� �Լ�
    private void TurnAnimationByScore(Assessment _gameShowFinalAssessment)
    {
        m_PopUpResultAnimationPanel.TurnOnUI();

        m_SoundManager.StopInGameBgm();
        m_SoundManager.PlayGameShowBgm();

        if (_gameShowFinalAssessment <= Assessment.Bad)
        {
            m_GameShowAnimationPanel.ChangeSpriteAnimation(m_FinalScoreSprite[0]);
        }
        else if (_gameShowFinalAssessment == Assessment.SoSo)
        {
            m_GameShowAnimationPanel.ChangeSpriteAnimation(m_FinalScoreSprite[1]);
        }
        else if (_gameShowFinalAssessment >= Assessment.Good)
        {
            m_GameShowAnimationPanel.ChangeSpriteAnimation(m_FinalScoreSprite[2], true);
        }

        m_ChangeAnimationPanel = ChangeAnimationPanel();

        StartCoroutine(m_ChangeAnimationPanel);
    }

    // ���â���� �̵��ϱ� ���� ����ִ� �гο� ������ ��ũ��Ʈ ��������
    private void GetGameShowResultPanelScript()
    {
        int _index = UnityEngine.Random.Range(0, m_GotoGameShowScriptList.Count);

        m_GoTOGameShowResultPanel.text = m_GotoGameShowScriptList[_index];

        m_PopUpResultPanel.TurnOnUI();
        m_ActivityEvent.m_IsCheckGameShow = true;
    }

    // BGameDesigner��� �ð���ŭ ��ٷȴٰ� �г� �������ֱ�
    IEnumerator ChangeAnimationPanel()
    {
        yield return new WaitForSecondsRealtime(5f);
        
        m_SetSatisfactionStatusPanelCor = SetSatisfactionStatusPanel();

        StartCoroutine(m_SetSatisfactionStatusPanelCor);
    }

    #region _�ɻ����� �ִϸ��̼� ��ũ�� ���߱� ���� �Լ���

    public void ShowFunAssessment()
    {
        FindEmojiAndScript((Assessment)_mTemporaryData.m_FunAssessment, "Fun");
    }

    public void ShowFunScore()
    {
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore(_mTemporaryData.m_FunScore.ToString());
    }

    public void ShowGraphicAssessment()
    {
        FindEmojiAndScript((Assessment)_mTemporaryData.m_GraphicAssessment, "Graphic");
    }

    public void ShowGraphicScore()
    {
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore(
            (_mTemporaryData.m_FunScore + _mTemporaryData.m_GraphicScore).ToString());
    }

    public void ShowPerfectionAssessment()
    {
        FindEmojiAndScript((Assessment)_mTemporaryData.m_PerfectionAssessment, "Perfection");
    }

    public void ShowPerfectionScore()
    {
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore(
            (_mTemporaryData.m_FunScore + _mTemporaryData.m_GraphicScore + _mTemporaryData.m_PerfectionScore)
            .ToString());
    }

    public void ShowGenreAssessment()
    {
        FindEmojiAndScript((Assessment)_mTemporaryData.m_GenreAssessment, "Genre");
    }

    public void ShowGenreScore()
    {
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((_mTemporaryData.m_FunScore +
                                                               _mTemporaryData.m_GraphicScore +
                                                               _mTemporaryData.m_PerfectionScore +
                                                               _mTemporaryData.m_GenreScore).ToString());
    }

    public void ShowConceptAssessment()
    {
        FindEmojiAndScript((Assessment)_mTemporaryData.m_ConceptAssessment, "Concept");
    }

    public void ShowConceptScore()
    {
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((_mTemporaryData.m_FunScore +
                                                               _mTemporaryData.m_GraphicScore +
                                                               _mTemporaryData.m_PerfectionScore +
                                                               _mTemporaryData.m_GenreScore +
                                                               _mTemporaryData.m_ConceptScore).ToString());
    }

    #endregion

    // ���� ������ ��Ȳ�� �����ֱ� ���� �гο� ��ũ��Ʈ�� �̹����� �־��ִ� �Լ�
    private IEnumerator SetSatisfactionStatusPanel()
    {
        m_GameShowAnimationPanel.SetActiveAnimationPanel();

        m_GameShowAnimationPanel.SetAcademyName(PlayerInfo.Instance.AcademyName);

        yield return new WaitForSecondsRealtime(25f);

        SetStudentFaceSprite((Assessment)_mTemporaryData.m_GameShowResultAssessment);

        yield return new WaitForSecondsRealtime(3f);

        m_PopOffResultAnimationPanel.TurnOffUI();

        m_PopUpResultScriptPanel.TurnOnUI();

        FindResultScript((Assessment)_mTemporaryData.m_GameShowResultAssessment, (int)_mTemporaryData.m_FunScore,
            (int)_mTemporaryData.m_GraphicScore, (int)_mTemporaryData.m_PerfectionScore);

        yield return StartCoroutine(ResultPanelScript());

        m_PopOffResultScriptPanel.TurnOffUI();
        m_SoundManager.ReplayInGameBgm();
        m_SoundManager.StopMain2Audio();

        yield return StartCoroutine(MakeResultReward());

        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        int _index = m_ToBeRunningGameShow.FindIndex(x => x.m_GameShowName == _gameShowData.GameShow_Name);
        m_ToBeRunningGameShow.RemoveAt(_index);
        
        InitGameShowPanel();
    }

    private IEnumerator ResultPanelScript()
    {
        while (m_ResultDialogQueue.Count > 0)
        {
            m_ResultScriptTexet = m_ResultDialogQueue.Dequeue();

            m_GameShowResultPanelText.text = m_ResultScriptTexet;

            yield return new WaitUntil(() =>
            {
                if ((Input.touchCount > 1 || Input.GetMouseButtonDown(0)) && m_ResultDialogQueue.Count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });

            yield return null;
        }
    }

    // ����޴� �г��� ���� �� ������ 3�� �̻��̸� ����� �ڷ�ƾ
    // �� �ڷΰ��ٰ� �ٽ� ������ ���ƿ´�.
    private IEnumerator ResultPanelScrollView()
    {
        bool scrollForward = true;
        bool completedCycle = false;

        yield return new WaitForSecondsRealtime(1f);

        while (!completedCycle)
        {
            float direction = scrollForward ? 1f : -1f;
            m_RewardPanel.m_ContentsAnchor.Translate(Vector3.right * m_ScrollSpeed * direction *
                                                     Time.unscaledDeltaTime);

            if (scrollForward && m_RewardPanel.m_ContentsAnchor.position.x >= m_TargetPosition.x)
            {
                m_RewardPanel.m_ContentsAnchor.position = m_StartPosition;
                scrollForward = false;
            }
            else if (!scrollForward && m_RewardPanel.m_ContentsAnchor.position.x <= m_StartPosition.x)
            {
                //m_RewardPanel.m_ContentsAnchor.position = m_TargetPosition;
                completedCycle = true;
            }

            yield return null;
        }
    }

    // ���â ��ũ��Ʈ�� ���ǿ� ���� �������� �̾� ť�� �־��ִ� �Լ�
    // ��� ������ ������ 3, �׷��� ������ ������4, �ϼ��� ������ ������ 5
    private void FindResultScript(Assessment _assessment, int _funScore, int _graphicScore, int _perfectionScore)
    {
        // intro��Ʈ
        m_ResultDialogQueue.Enqueue(GetResultScript(_assessment, 1));

        // main��Ʈ
        string _mainText1 = GetResultScript(_assessment, 2);

        int max = _funScore > _graphicScore ? 3 : 4;
        max = max > _perfectionScore ? 5 : max;

        string _mainText2 = GetResultScript(Assessment.None, max);
        char lastText = _mainText2.ElementAt(_mainText2.Length - 1);

        string selectText = (lastText - 0xAC00) % 28 > 0 ? "��" : "��";
        m_ResultDialogQueue.Enqueue("�ÿ�ȸ �������δ� \"" + _mainText1 + "\"," + "\"" + _mainText2 + "\"" + selectText + " �ֳ׿�.");

        // ��������Ʈ
        m_ResultDialogQueue.Enqueue(GetResultScript(Assessment.None, 6));
    }

    private void FindEmojiAndScript(Assessment _assessment, string _part)
    {
        switch (_assessment)
        {
            case Assessment.Terrible:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[0]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.Terrible, _part));
            }
            break;

            case Assessment.Bad:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[1]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.Bad, _part));
            }
            break;

            case Assessment.NotBad:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[2]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.NotBad, _part));
            }
            break;

            case Assessment.SoSo:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[3]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.SoSo, _part));
            }
            break;

            case Assessment.Good:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[4]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.Good, _part));
            }
            break;

            case Assessment.Great:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[5]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.Great, _part));
            }
            break;

            case Assessment.Excellent:
            {
                m_GameShowAnimationPanel.ChangeSpriteEmoji(m_EmojiSprite[6]);
                m_GameShowAnimationPanel.ChangeJudgesScript(GetJudgesScript(Assessment.Excellent, _part));
            }
            break;
        }
    }

    public void SkipAnim()
    {
        StopCoroutine(m_SetSatisfactionStatusPanelCor);
        StopCoroutine(m_ChangeAnimationPanel);

        StartCoroutine(ClickSkipButton());
    }

    private IEnumerator ClickSkipButton()
    {
        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        m_PopOffResultAnimationPanel.TurnOffUI();

        m_PopUpResultScriptPanel.TurnOnUI();

        FindResultScript((Assessment)_mTemporaryData.m_GameShowResultAssessment, (int)_mTemporaryData.m_FunScore,
            (int)_mTemporaryData.m_GraphicScore, (int)_mTemporaryData.m_PerfectionScore);

        yield return StartCoroutine(ResultPanelScript());

        m_PopOffResultScriptPanel.TurnOffUI();
        m_SoundManager.ReplayInGameBgm();
        m_SoundManager.StopMain2Audio();

        yield return StartCoroutine(MakeResultReward());

        int _index = m_ToBeRunningGameShow.FindIndex(x => x.m_GameShowName == _gameShowData.GameShow_Name);
        m_ToBeRunningGameShow.RemoveAt(_index);

        InitGameShowPanel();
    }

    private void SetStudentFaceSprite(Assessment _assessment)
    {
        switch (_assessment)
        {
            case Assessment.Terrible:
            {
                m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[0], m_StudentFace[1], m_StudentFace[2]);
            }
            break;

            case Assessment.Bad:
            {
                m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[3], m_StudentFace[4], m_StudentFace[5]);
            }
            break;

            case Assessment.SoSo:
            {
                m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[6], m_StudentFace[7], m_StudentFace[8]);
            }
            break;

            case Assessment.Good:
            {
                m_GameShowAnimationPanel.ChangeStudnetFaceImageGood();
            }
            break;

            case Assessment.Great:
            {
                m_GameShowAnimationPanel.ChangeStudnetFaceImageGood();
            }
            break;

            case Assessment.Excellent:
            {
                m_GameShowAnimationPanel.ChangeStudnetFaceImageGood();
            }
            break;
        }
    }

    // ����� ���� ������ ������ش�.
    private IEnumerator MakeResultReward()
    {
        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        if (m_RewardPanel.m_ContentsTransform.childCount != 0)
        {
            for (int i = m_RewardPanel.m_ContentsTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(m_RewardPanel.m_ContentsTransform.GetChild(i).gameObject);
            }
        }

        var (reward, famous, passion, health) = GetRewardByAssessment(_gameShowData.GameShow_Level,
            (Assessment)_mTemporaryData.m_GameShowResultAssessment);

        m_RewardMoney = reward;
        m_RewardFamous = famous;
        m_RewardPassion = passion;
        m_RewardHealth = health;

        if (m_RewardFamous != 0 || m_RewardHealth != 0 || m_RewardMoney != 0 || m_RewardPassion != 0)
        {
            m_PopUpRewardPanel.TurnOnUI();
            m_SoundManager.PlayMoneyJackpotSound();
        }
        else
        {
            yield break;
        }

        if (m_RewardMoney != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardMoney";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "���";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[0];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = string.Format("{0:#,0}", m_RewardMoney);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardFamous != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardFamous";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "������";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[1];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = m_RewardFamous.ToString();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardPassion != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardPassion";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "����";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[2];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = m_RewardPassion.ToString();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardHealth != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardHealth";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "ü��";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[3];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = m_RewardHealth.ToString();
        }

        m_StartPosition = m_RewardPanel.m_ContentsAnchor.position;
        m_TargetPosition = m_StartPosition + Vector3.right * m_RewardPanel.m_ContentsAnchor.rect.width;

        if (m_RewardPanel.m_ContentsTransform.childCount > 3)
        {
            //m_RewardPanel.SetContentAnchor(false);

            yield return StartCoroutine(ResultPanelScrollView());
        }
        //else
        //{
        //    //m_RewardPanel.SetContentAnchor(true);
        //    //Math.Cos(90.0f);
        //}
    }

    // ���Ӽ� �г��� ���� �� ���� ���� ������ �ʱ�ȭ
    public void InitData()
    {
        _mTemporaryData.m_GameJamID = 0;
        _mTemporaryData.m_FunnyHeart = 0;
        _mTemporaryData.m_GenreHeart = 0;
        _mTemporaryData.m_GraphicHeart = 0;
        _mTemporaryData.m_PerceficationHeart = 0;
        _mTemporaryData.m_GameShowGenreAndConceptHeart = 0;
        _mTemporaryData.m_GameShowResultAssessment = (int)Assessment.None;
    }

    // ���Ӽ ���� �� �ʱ�ȭ ���ٰ͵�
    private void InitGameShowPanel()
    {
        m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[3], m_StudentFace[4], m_StudentFace[5]);
        m_GameShowAnimationPanel.InitPanel();
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore("0");
        m_GameShowAnimationPanel.StopAnimation();
    }

    // ���Ӽ��� ��ư�� ������ �� ������ ���� ���� ����� ������� �Լ�
    private void MakePrevGameJamForToBeRunning(string _rank = "")
    {
        m_GameShowPanel.DestroyObj(m_GameShowPanel.PrevGameListContentObj);

        if (GameJam.m_GameJamHistory.Count == 0)
        {
            m_GameShowPanel.SetActiveIsNotPrevGameList(true);
            return;
        }

        Dictionary<int, List<GameJamSaveData>> _gameJam = StaticDeepCopy.DeepClone(GameJam.m_GameJamHistory);

        if (m_ToBeRunningGameShow.Count != 0)
        {
            for (int i = 0; i < m_ToBeRunningGameShow.Count; i++)
            {
                int _index = (int)m_ToBeRunningGameShow[i].m_GameJamID;
                int _gamejamIndex = _gameJam[_index].FindIndex(x => x.m_GameJamID == _index);
                string _gameJamRank = "";

                if (_rank != "")
                    _gameJamRank = _gameJam[_index][_gamejamIndex].m_Rank;
                else
                {
                    _rank = _gameJam[_index][_gamejamIndex].m_Rank;
                    _gameJamRank = _gameJam[_index][_gamejamIndex].m_Rank;
                }

                if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "�̿ϼ�")
                {
                    List<GameJamSaveData> gamejamdata1 = new List<GameJamSaveData>(_gameJam[_index]);
                    MakeSelectPrevGameShowList(gamejamdata1);
                    _gameJam[_index].RemoveAt(_gamejamIndex);
                }
            }

            for (int j = 0; j < _gameJam.Count; j++)
            {
                for (int k = 0; k < _gameJam.ElementAt(j).Value.Count; k++)
                {
                    string _gameJamRank = "";

                    if (_rank != "")
                        _gameJamRank = _gameJam.ElementAt(j).Value[k].m_Rank;
                    else
                    {
                        _rank = _gameJam.ElementAt(j).Value[k].m_Rank;
                        _gameJamRank = _gameJam.ElementAt(j).Value[k].m_Rank;
                    }

                    if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "�̿ϼ�")
                        MakePrevGameList(j, k);
                }
            }
        }
        else
        {
            for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
            {
                for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
                {
                    string _gameJamRank = "";

                    if (_rank != "")
                        _gameJamRank = _gameJam.ElementAt(i).Value[j].m_Rank;
                    else
                    {
                        _rank = _gameJam.ElementAt(i).Value[j].m_Rank;
                        _gameJamRank = _gameJam.ElementAt(i).Value[j].m_Rank;
                    }

                    if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "�̿ϼ�")
                        MakePrevGameList(i, j);
                }
            }
        }
    }

    // �����뿡�� ���� ���ӵ��� ���� �Լ�
    private void MakePrevGameList(int _gameJamHistoryIndex, int _gameJamHistoryValueIndex)
    {
        GameJamInfo _nowData = GameJam.SearchAllGameJamInfo(GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Key);

        GameObject _prevGameList = Instantiate(m_PrevGameListPrefab, m_GameShowPanel.PrevGameJamListContent);

        int _gameConceptImageIndex = (int)GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_ConceptIndex;
        string _gameConcept = _nowData.m_GjamConcept;
        string _gameName = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameName;
        string _gameRank = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_Rank;
        string _gameGenre = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_Genre;
        string _gameFunny = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_Funny.ToString();
        string _gameGraphic = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_Graphic.ToString();
        string _gamePerfection = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_Perfection.ToString();

        _prevGameList.name = _gameName;
        _prevGameList.GetComponent<PrevGamePrefab>().m_ConceptImage.sprite = m_GameJam.RestultSprite[_gameConceptImageIndex];
        _prevGameList.GetComponent<PrevGamePrefab>().m_GameName.text = _gameName;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Grade.text = _gameRank;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Concept.text = _gameConcept;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Genre.text = _gameGenre;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Funny.text = _gameFunny;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Graphic.text = _gameGraphic;
        _prevGameList.GetComponent<PrevGamePrefab>().m_Perfection.text = _gamePerfection;
        _prevGameList.GetComponent<PrevGamePrefab>().m_PrefabButton.onClick.AddListener(ClickPrevGameButton);
    }

    // ���� ������ ���� ã�Ƽ� ���� ������ֱ�
    private void MakeSelectPrevGameShowList(List<GameJamSaveData> _data)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            GameObject _prevGameList = Instantiate(m_PrevGameListPrefab, m_GameShowPanel.PrevGameJamListContent);

            GameJamInfo _nowData = GameJam.SearchAllGameJamInfo((int)_data[i].m_GameJamID);

            string _gameName = _data[i].m_GameName;
            string _gameRank = _data[i].m_Rank;
            string _gameConcept = _nowData.m_GjamConcept;
            string _gameGenre = _data[i].m_Genre;
            string _gameFunny = _data[i].m_Funny.ToString();
            string _gameGraphic = _data[i].m_Funny.ToString();
            string _gamePerfection = _data[i].m_Funny.ToString();

            _prevGameList.name = _gameName;
            _prevGameList.GetComponent<PrevGamePrefab>().m_ConceptImage.sprite = m_GameJam.RestultSprite[(int)_data[i].m_ConceptIndex];
            _prevGameList.GetComponent<PrevGamePrefab>().m_GameName.text = _gameName;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Grade.text = _gameRank;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Concept.text = _gameConcept;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Genre.text = _gameGenre;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Funny.text = _gameFunny;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Graphic.text = _gameGraphic;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Perfection.text = _gamePerfection;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Glow.SetActive(true);
            _prevGameList.GetComponent<PrevGamePrefab>().m_PrefabButton.interactable = false;
        }
    }

    // �ϴ� ������ �ִ� ���� ��ϵ��� �����ְ� �ش� ��ũ�� ���� ��ϵ鸸 ������ش�.
    /// TODO : ���Ӽ ������ ������ ������ �� ���� �ø��� �������� ������ش�.
    public void ClickPrevGameRankButton()
    {
        m_GameShowPanel.DestroyObj(m_GameShowPanel.PrevGameListContentObj);

        GameObject _rankbutton = EventSystem.current.currentSelectedGameObject;
        string _rankName = _rankbutton.name;

        if (GameJam.m_GameJamHistory.Count == 0)
        {
            m_GameShowPanel.SetActiveIsNotPrevGameList(true);
        }
        else if (_rankName == "All")
        {
            MakePrevGameJamForToBeRunning();
        }
        else
        {
            MakePrevGameJamForToBeRunning(_rankName);
        }
    }

    // ��ǰ�� ���� ��ư�� ������ �� ���� �������� ü�°� ������ ���� �����ϴٸ� ������ �� ���ٰ� ���� ��û��ư�� Ȱ��ȭ ���·� �ƹ� ����� ���Ѵ�.
    private void ClickPrevGameButton()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (m_PrevClickObj != null && m_PrevClickObj != _currentObj)
        {
            m_PrevClickObj.GetComponent<PrevGamePrefab>().m_Check.SetActive(false);
        }

        _currentObj.GetComponent<PrevGamePrefab>().m_Check.SetActive(true);

        _isCanParticipate = CheckStudentHealthAndPassion();
        _isSameDayInList = CheckGameShowDay();
        _isPrevGameJamButtonClick = true;
        m_GameShowPanel.ApplyButton.interactable = true;
        m_PrevClickObj = _currentObj;

        if (m_TutorialCount == 13)
        {
            m_Unmask.fitTarget = m_GameShowPanel.ApplyButton.GetComponent<RectTransform>();
            m_TutorialTextImage.gameObject.SetActive(false);
            m_TutorialArrowImage.gameObject.SetActive(true);
            m_TutorialArrowImage.transform.Rotate(0, 0, -90);
            m_TutorialArrowImage.transform.position = m_Unmask.fitTarget.position + new Vector3(0, 190, 0);
            m_TutorialCount++;
        }
    }

    private void ApplySuccess()
    {
        // ���� �̹� ��û�� �������� ����ִµ� �����Ŷ�� �Ѱ��ֱ�
        int index = m_ToBeRunningGameShow.FindIndex(x => x.m_GameShowID == _mTemporaryData.m_GameShowID);

        if (m_MonthLimit > 0 && index == -1)
        {
            GameShowData _gameData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);
            Student _gameDesignerStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);
            Student _artStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ArtStudentName);
            Student _programmingStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ProgrammingStudentName);

            m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySuccessSprite;

            _mTemporaryData.m_GameShowGenreAndConceptHeart = m_GenreAndConceptHeart;
            _mTemporaryData.m_FunnyHeart = _gameData.GameShow_State["Fun"];
            _mTemporaryData.m_PerceficationHeart = _gameData.GameShow_State["Perfection"];
            _mTemporaryData.m_GraphicHeart = _gameData.GameShow_State["Graphic"];
            _mTemporaryData.m_GenreHeart = _gameData.GameShow_State["Genre"];
            _mTemporaryData.m_GameShowName = _gameData.GameShow_Name;
            _gameDesignerStudent.m_StudentStat.m_Health -= _gameData.GameShow_Health;
            _gameDesignerStudent.m_StudentStat.m_Passion -= _gameData.GameShow_Pasion;

            _artStudent.m_StudentStat.m_Health -= _gameData.GameShow_Health;
            _artStudent.m_StudentStat.m_Passion -= _gameData.GameShow_Pasion;

            _programmingStudent.m_StudentStat.m_Health -= _gameData.GameShow_Health;
            _programmingStudent.m_StudentStat.m_Passion -= _gameData.GameShow_Pasion;
            _mTemporaryData.m_MakeMonth = _gameData.GameShow_Month;
            _mTemporaryData.m_MakeWeek = _gameData.GameShow_Week;
            _mTemporaryData.m_MakeDay = _gameData.GameShow_Day;

            GameShowSaveData _newdata = StaticDeepCopy.DeepClone<GameShowSaveData>(_mTemporaryData);

            m_ToBeRunningGameShow.Add(_newdata);

            m_MonthLimit -= 1;

            GameObject _eventPrefab = m_ActivityEvent.MakeEventPrefab(_gameData.GameShow_Name, _gameData.GameShow_Month,
                _gameData.GameShow_Week, _gameData.GameShow_Day, false, _gameData.GameShow_Month);

            m_ActivityEvent.SetEventPanelActive(true);
            //m_EventPanel.gameObject.SetActive(true);
            //m_IsTypingAnmation = true;
            m_PrevClickObj.GetComponent<PrevGamePrefab>().m_Glow.SetActive(true);
            m_PrevClickObj.GetComponent<PrevGamePrefab>().m_Check.SetActive(false);

            m_GameShowPanel.SetPractivePanel();

            CheckNovelty(_mTemporaryData);

            if (_novelty < 1f)
                StartCoroutine(NoveltyScript());

            StartCoroutine(m_ActivityEvent.TypingText(_eventPrefab, "���Ӽ� �غ���"));
        }
        else if (m_MonthLimit <= 0)
        {
            // ����Ƚ�� ��~
            m_GameShowPanel.SetConditionFailPanel("�̹��� ������ ��~");
        }
    }

    private IEnumerator NoveltyScript()
    {
        yield return new WaitForSecondsRealtime(1f);

        int _index = UnityEngine.Random.Range(0, m_NoveltyScriptList.Count);
        string[] _dialog = m_NoveltyScriptList[_index].Split("/");

        for (int i = 0; i < _dialog.Length; i++)
        {
            m_NoveltyDialoge.Enqueue(_dialog[i]);
        }

        m_PopUpNoveltyWarningPanel.TurnOnUI();

        yield return StartCoroutine(Dialoge());

        m_PopOffNoveltyWarningPanel.TurnOffUI();
    }

    private IEnumerator Dialoge()
    {
        while (m_NoveltyDialoge.Count > 0)
        {
            m_NoveltyScript.text = m_NoveltyDialoge.Dequeue();

            yield return new WaitUntil(() =>
            {
                if ((Input.touchCount > 1 || Input.GetMouseButtonDown(0)) && m_NoveltyDialoge.Count >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            });

            yield return null;
        }
    }

    private void CheckNovelty(GameShowSaveData _data)
    {
        _novelty = 1f;

        if (m_GameShowHistory.Count != 0)
        {
            for (int i = 0; i < m_GameShowHistory.Count; i++)
            {
                for (int j = 0; j < m_GameShowHistory.ElementAt(i).Value.Count; j++)
                {
                    if (_data.m_GameJamName == m_GameShowHistory.ElementAt(i).Value[j].m_GameJamName)
                    {
                        if ((int)_data.m_GameShowResultAssessment >= 4)
                        {
                            _novelty *= 0.2f;
                        }

                        if (_novelty < 0.1f)
                        {
                            _novelty = 0.1f;
                        }
                    }
                }
            }
        }
    }

    private void ApplyFailed()
    {
        string[] _text = new string[3];

        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);
        List<Student> _compareStudentHealthAndPassion = new List<Student>();

        Student _gameDesignerStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);
        Student _artStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ArtStudentName);
        Student _programmingStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_ProgrammingStudentName);

        _compareStudentHealthAndPassion.Add(_gameDesignerStudent);
        _compareStudentHealthAndPassion.Add(_artStudent);
        _compareStudentHealthAndPassion.Add(_programmingStudent);

        for (int i = 0; i < _compareStudentHealthAndPassion.Count; i++)
        {
            if (_compareStudentHealthAndPassion[i].m_StudentStat.m_Passion < _gameShowData.GameShow_Pasion)
            {
                if (_compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName != "")
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName +
                               "�л��� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "�л��� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
            }
            else if (_compareStudentHealthAndPassion[i].m_StudentStat.m_Health < _gameShowData.GameShow_Health)
            {
                if (_compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName != "")
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName +
                               "�л��� ü���� �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "�л��� ü���� �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
            }
            else
            {
                if (_compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName != "")
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName +
                               "�л��� ü�°� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "�л��� ü�°� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
                }
            }
        }

        m_GameShowPanel.SetConditionFailPanel(_text[0] + "\n" + _text[1] + "\n" + _text[2] + "\n");
    }

    // ���� ��¥�� ���Ӽ ���� ���ϰ� �ϱ����� �ִ��� Ȯ���غ��� �Լ�
    private bool CheckGameShowDay()
    {
        GameShowData _totalGameShowData = new GameShowData();
        GameShowData _temporaryGameShowData = new GameShowData();

        // �����ų �����Ͱ� ������ ã�Ƽ� �־��ְ� ���ٸ� �׳� �ʱ�ȭ �����ش�.
        if (m_ToBeRunningGameShow.Count != 0)
        {
            _totalGameShowData = SearchGameShowInfo((int)m_ToBeRunningGameShow[0].m_GameShowID);
            _temporaryGameShowData = SearchGameShowDataForWeek((int)_mTemporaryData.m_GameShowID);
        }

        if (m_ToBeRunningGameShow.Count == 0)
        {
            return true;
        }
        else if ((_totalGameShowData.GameShow_Month != _temporaryGameShowData.GameShow_Month &&
                _totalGameShowData.GameShow_Week != _temporaryGameShowData.GameShow_Week) ||
                (_totalGameShowData.GameShow_Month == _temporaryGameShowData.GameShow_Month &&
                _totalGameShowData.GameShow_Week != _temporaryGameShowData.GameShow_Week))
        {
            return true;
        }

        return false;
    }

    private void SetGenreAndConceptHeart(int _gameJamIndex, int _gameJamListIndex)
    {
        GameJamInfo _nowData = GameJam.SearchAllGameJamInfo(GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Key);
        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        if (_gameShowData.GameShowConcept.Any(c => c == _nowData.m_GjamConcept)
            && _gameShowData.GameShowGenre.Any(g =>
                g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_Genre)
            || _gameShowData.GameShowGenre[0] == "��� �帣" && _gameShowData.GameShowGenre[0] == "��� ����")
        {
            m_GenreAndConceptHeart = 3;
        }
        else if (_gameShowData.GameShowConcept.Any(c => c == _nowData.m_GjamConcept)
                 || _gameShowData.GameShowGenre.Any(g =>
                     g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_Genre)
                 || _gameShowData.GameShowGenre[0] == "��� �帣" || _gameShowData.GameShowGenre[0] == "��� ����")
        {
            m_GenreAndConceptHeart = 2;
        }
        else
        {
            m_GenreAndConceptHeart = 1;
        }
    }

    private bool CheckStudentHealthAndPassion()
    {
        bool _isStudentConditionFine = false;

        GameObject _prevGameButton = EventSystem.current.currentSelectedGameObject;
        string _gameName = _prevGameButton.name;

        List<Student> _student = new List<Student>(ObjectManager.Instance.m_StudentList);

        for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (_gameName == GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameName)
                {
                    GameJamInfo _nowData = GameJam.SearchAllGameJamInfo(GameJam.m_GameJamHistory.ElementAt(i).Key);

                    SetGenreAndConceptHeart(i, j);

                    _mTemporaryData.m_GameDesignerStudentName =
                        GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameDesignerStudentName;
                    _mTemporaryData.m_ArtStudentName = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ArtStudentName;
                    _mTemporaryData.m_ProgrammingStudentName =
                        GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_ProgrammingStudentName;

                    _mTemporaryData.m_GameJamID = _nowData.m_GjamID;
                    _mTemporaryData.m_GameJamName = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameName;
                    break;
                }
            }
        }

        Student _gameDesignerStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);
        Student _artStudent = ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);
        Student _programmingStudent =
            ObjectManager.Instance.SearchStudentInfo(_mTemporaryData.m_GameDesignerStudentName);

        GameShowData _gameShowData = SearchGameShowInfo((int)_mTemporaryData.m_GameShowID);

        for (int i = 0; i < _student.Count; i++)
        {
            if (_student[i].m_StudentStat.m_StudentName == _gameDesignerStudent.m_StudentStat.m_StudentName ||
                _student[i].m_StudentStat.m_StudentName == _artStudent.m_StudentStat.m_StudentName ||
                _student[i].m_StudentStat.m_StudentName == _programmingStudent.m_StudentStat.m_StudentName)
            {
                if (_student[i].m_StudentStat.m_Health >= _gameShowData.GameShow_Health &&
                    _student[i].m_StudentStat.m_Passion >= _gameShowData.GameShow_Pasion)
                {
                    _isStudentConditionFine = true;
                }
                else
                {
                    // �Ѹ��̶� �����ϸ� �ȵ�.
                    _isStudentConditionFine = false;
                    break;
                }
            }
        }

        return _isStudentConditionFine;
    }

    // ������� ���Ӽ� ����� ��ư�� ������ �� �г��� �������� �������ִ� �Լ�
    // ��û�� ������ ������ ��û �Ϸ� ��ư ��������Ʈ�� �������ְ� �������� �ϱ�
    private void ClickGameShowListButton()
    {
        #region _��ư�� Ŭ������ �� ���̶���Ʈ�� �����ְ� �ϱ�

        if (m_PrevClickGameShowObj != null)
        {
            Button _prevButton = m_PrevClickGameShowObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        if (_currentObj == null)
        {
            return;
        }

        m_PrevClickGameShowObj = _currentObj;

        Button _currentButton = _currentObj.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        //GameObject _clickButton = EventSystem.current.currentSelectedGameObject;

        string _buttonName = _currentObj.name;

        m_GameShowPanel.InitGameShowPanel();

        if (m_ToBeRunningGameShow.Count != 0)
        {
            int _index = m_ToBeRunningGameShow.FindIndex(x => x.m_GameShowName == _buttonName);

            if (-1 != _index)
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySuccessSprite;

                for (int k = 0; k < m_TotalGameShowData.Count; k++)
                {
                    if (m_TotalGameShowData[k].GameShow_Name == _buttonName)
                    {
                        SettingGameShowInfoPanel(k);
                        break;
                    }
                }
            }
            else if (m_MonthLimit > 0)
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySprite;

                for (int k = 0; k < m_TotalGameShowData.Count; k++)
                {
                    if (m_TotalGameShowData[k].GameShow_Name == _buttonName)
                    {
                        SettingGameShowInfoPanel(k);
                        break;
                    }
                }
            }
            else
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_NotAllowedApplySprite;

                for (int k = 0; k < m_TotalGameShowData.Count; k++)
                {
                    if (m_TotalGameShowData[k].GameShow_Name == _buttonName)
                    {
                        SettingGameShowInfoPanel(k);
                        break;
                    }
                }
            }

            _isCanParticipate = CheckStudentHealthAndPassion();
            _isSameDayInList = CheckGameShowDay();
        }
        else
        {
            m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySprite;

            for (int i = 0; i < m_TotalGameShowData.Count; i++)
            {
                if (m_TotalGameShowData[i].GameShow_Name == _buttonName)
                {
                    SettingGameShowInfoPanel(i);
                    break;
                }
            }
        }

        if (m_TutorialCount == 6)
        {
            m_Unmask.fitTarget = m_GameShowPanel.NameLevelRect;
            m_TutorialTextImage.gameObject.SetActive(true);
            m_TutorialArrowImage.gameObject.SetActive(false);
            m_NextButton.gameObject.SetActive(true);
            m_TutorialText.text = m_TutorialPanel.GetComponent<Tutorial>().GameShowTutorial[m_ScriptCount];
            m_TutorialTextImage.transform.position = m_Unmask.fitTarget.position + new Vector3(-600, 0, 0);
            m_ScriptCount++;
            m_TutorialCount++;
        }
    }

    // ���Ӽ� ���� ó�� �κ�. 
    private void SetFirstGameShowInfo()
    {
        #region _�� �����ִ� ��ư�� ���̶���Ʈ ���ֱ�
        if (m_PrevClickGameShowObj != null)
        {
            Button _prevButton = m_PrevClickGameShowObj.GetComponent<Button>();

            ColorBlock _prevButtonColor = _prevButton.colors;

            _prevButtonColor = ColorBlock.defaultColorBlock;

            _prevButton.colors = _prevButtonColor;
        }

        m_PrevClickGameShowObj = m_GameShowPanel.GameShowListContent.GetChild(0).gameObject;

        Button _currentButton = m_GameShowPanel.GameShowListContent.GetChild(0).gameObject.GetComponent<Button>();

        ColorBlock _currentButtonColor = _currentButton.colors;

        _currentButtonColor.normalColor = m_HighLightColor;
        _currentButtonColor.highlightedColor = m_HighLightColor;
        _currentButtonColor.pressedColor = m_HighLightColor;
        _currentButtonColor.selectedColor = m_HighLightColor;

        _currentButton.colors = _currentButtonColor;

        #endregion

        m_GameShowPanel.InitGameShowPanel();

        if (m_ToBeRunningGameShow.Count != 0)
        {
            GameShowData _gameShowData = SearchGameShowInfo((int)m_ToBeRunningGameShow[0].m_GameShowID);

            if (_gameShowData.GameShow_Name == m_TotalGameShowData[0].GameShow_Name)
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySuccessSprite;
            }
            else
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySprite;
            }
        }
        else
        {
            m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySprite;
        }

        SettingGameShowInfoPanel(0);
    }

    private void SettingGameShowInfoPanel(int _index)
    {
        string _gameShowHostCompany = m_TotalGameShowData[_index].GameShow_Host_Name;
        string _gameShowName = m_TotalGameShowData[_index].GameShow_Name;
        string _gameShowLevel = m_TotalGameShowData[_index].GameShow_Level.ToString();
        string _gameShowProgressDate = (m_TotalGameShowData[_index].GameShow_Month.ToString()) + "�� " +
                                       (m_TotalGameShowData[_index].GameShow_Week.ToString()) + "��";
        string _gameShowJudegeMent = m_TotalGameShowData[_index].GameShow_Judges_Name;
        string _gameShowReward = string.Format("{0:#,0}", m_TotalGameShowData[_index].GameShow_Reward);
        string _gameShowHostHelath = m_TotalGameShowData[_index].GameShow_Health.ToString();
        string _gameShowHostPassion = m_TotalGameShowData[_index].GameShow_Pasion.ToString();
        int _gameShowFunny = m_TotalGameShowData[_index].GameShow_State["Fun"];
        int _gameShowGraphic = m_TotalGameShowData[_index].GameShow_State["Graphic"];
        int _gameShowPerfection = m_TotalGameShowData[_index].GameShow_State["Perfection"];
        int _gameShowGenre = m_TotalGameShowData[_index].GameShow_State["Genre"];

        m_GameShowPanel.SetGameShowListText(_gameShowHostCompany, _gameShowName, _gameShowLevel, _gameShowProgressDate,
            _gameShowJudegeMent, _gameShowReward, _gameShowHostHelath, _gameShowHostPassion);
        m_GameShowPanel.SetFunny(_gameShowFunny, true);
        m_GameShowPanel.SetGraphic(_gameShowGraphic, true);
        m_GameShowPanel.SetPerfection(_gameShowPerfection, true);
        m_GameShowPanel.SetGenre(_gameShowGenre, true);

        _mTemporaryData.m_GameShowID = m_TotalGameShowData[_index].GameShow_ID;
        //_mTemporaryData.m_TotalGameShowData = m_TotalGameShowData[_index];
    }

    public static void Swap<T>(List<T> list, int from, int to)
    {
        T tmp = list[from];
        list[from] = list[to];
        list[to] = tmp;
    }

    // �� ��ī���� ��޿� ���� ���Ӽ�
    private void BringFixedGameShowData()
    {
        List<GameShowData> m_MonthGameShowData = new List<GameShowData>();

        int difficulty = GetRankByMyAcademyRank(PlayerInfo.Instance.CurrentRank);
        int _nowMonth = m_NowMonth + 1;
        int _fun = 0;
        int _graphic = 0;
        int _perfection = 0;
        int _genre = 0;

        if (m_NowMonth == 12)
        {
            _nowMonth = 1;
        }

        for (int i = 0; i < m_FixedGameShowData.Count; i++)
        {
            // �ų� �߻��ϴ� ����
            if (m_FixedGameShowData[i].GameShow_Year == 0 &&
                m_FixedGameShowData[i].GameShow_Month == _nowMonth)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }
            // Ȧ���⵵ �߻�
            else if (m_FixedGameShowData[i].GameShow_Year == -1 &&
                     (10 % GameTime.Instance.FlowTime.NowYear) != 0 &&
                     m_FixedGameShowData[i].GameShow_Month == _nowMonth)   // 12������ 1�� �ٲ��ֱ�
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }
            // ¦���⵵ �߻�
            else if (m_FixedGameShowData[i].GameShow_Year == -2 &&
                     (10 % GameTime.Instance.FlowTime.NowYear) == 0 &&
                     m_FixedGameShowData[i].GameShow_Month == _nowMonth)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }

        }

        switch (difficulty)
        {
            // ���̵�1 2��, ���̵� 2,3,4,5 �� 1��
            case 1:
            {
                SortMyLevelList(m_MonthGameShowData);

                int _listCount = m_MonthGameShowData.Count;

                for (int i = 0; i < m_MonthGameShowData.Count; i++)
                {
                    int _randomDifficulty = UnityEngine.Random.Range(2, 6);
                    int _randomIndex = UnityEngine.Random.Range(0, _listCount);

                    if (i != _randomIndex)
                    {
                        SetGameShowData(m_MonthGameShowData, i, 1);
                    }
                    else
                    {
                        SetGameShowData(m_MonthGameShowData, _randomIndex, _randomDifficulty);
                    }

                    Swap(m_MonthGameShowData, _randomIndex, _listCount - 1);
                    _listCount -= 1;
                }
            }
            break;

            // ���̵�1 1��, ���̵� 2 1��, ���̵� 3,4,5 �� 1��
            case 2:
            {
                SortMyLevelList(m_MonthGameShowData);

                int _listCount1 = 0;
                int _listCount2 = 0;
                int _randomDifficulty = UnityEngine.Random.Range(3, 6);
                int _randomIndex = UnityEngine.Random.Range(0, m_MonthGameShowData.Count);

                if (_randomIndex == 0)
                {
                    _listCount1 = 1;
                    _listCount2 = 2;
                }
                else if (_randomIndex == 1)
                {
                    _listCount1 = 0;
                    _listCount2 = 1;
                }
                else
                {
                    _listCount1 = 0;
                    _listCount2 = 1;
                }

                SetGameShowData(m_MonthGameShowData, _randomIndex, _randomDifficulty);
                SetGameShowData(m_MonthGameShowData, _listCount1, 1);
                SetGameShowData(m_MonthGameShowData, _listCount2, 2);
            }
            break;

            // ���̵�1,2 �� 1��, ���̵� 3 1��, ���̵� 4,5 �� 1��
            case 3:
            {
                SortMyLevelList(m_MonthGameShowData);

                int _randomDifficulty1 = UnityEngine.Random.Range(1, 3);
                int _randomDifficulty2 = UnityEngine.Random.Range(4, 6);

                SetGameShowData(m_MonthGameShowData, 0, _randomDifficulty1);
                SetGameShowData(m_MonthGameShowData, 2, _randomDifficulty2);
                SetGameShowData(m_MonthGameShowData, 1, 3);
            }
            break;
            // ���̵�1,2,3 �� 1��, ���̵� 4 1��, ���̵� 5 1��
            case 4:
            {
                SortMyLevelList(m_MonthGameShowData);

                int _listCount1 = 0;
                int _listCount2 = 0;
                int _randomDifficulty = UnityEngine.Random.Range(1, 4);
                int _randomIndex = UnityEngine.Random.Range(0, m_MonthGameShowData.Count);

                if (_randomIndex == 0)
                {
                    _listCount1 = 1;
                    _listCount2 = 2;
                }
                else if (_randomIndex == 1)
                {
                    _listCount1 = 0;
                    _listCount2 = 1;
                }
                else
                {
                    _listCount1 = 0;
                    _listCount2 = 1;
                }

                SetGameShowData(m_MonthGameShowData, _randomIndex, _randomDifficulty);
                SetGameShowData(m_MonthGameShowData, _listCount1, 4);
                SetGameShowData(m_MonthGameShowData, _listCount2, 5);
            }
            break;

            // ���̵�1,2,3,4 �� 1��, ���̵� 5 2��
            case 5:
            {
                SortMyLevelList(m_MonthGameShowData);

                int _listCount = m_MonthGameShowData.Count;

                for (int i = 0; i < m_MonthGameShowData.Count; i++)
                {
                    int _randomDifficulty = UnityEngine.Random.Range(1, 5);
                    int _randomIndex = UnityEngine.Random.Range(0, _listCount);

                    if (i != _randomIndex)
                    {
                        SetGameShowData(m_MonthGameShowData, i, 5);
                    }
                    else
                    {
                        SetGameShowData(m_MonthGameShowData, _randomIndex, _randomDifficulty);
                    }

                    Swap(m_MonthGameShowData, _randomIndex, _listCount - 1);
                    _listCount -= 1;
                }
            }
            break;
        }
    }

    private void BringRandomGameShowData()
    {
        List<GameShowData> m_MonthRandomList = new List<GameShowData>();

        for (int i = 0; i < m_RandomGameShowData.Count; i++)
        {
            if (m_RandomGameShowData[i].GameShow_Month - 1 == m_NowMonth)
            {
                m_MonthRandomList.Add(m_RandomGameShowData[i]);
            }
        }

        SortMyLevelList(m_MonthRandomList);

        int _listCount = m_MonthRandomList.Count;

        if (_listCount == 0)
        {
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            int _randomIndex = UnityEngine.Random.Range(0, _listCount);
            int _randomDifficulty = UnityEngine.Random.Range(1, 6);

            SetGameShowData(m_MonthRandomList, _randomIndex, _randomDifficulty);
            Swap(m_MonthRandomList, _randomIndex, _listCount - 1);
            _listCount -= 1;
        }
    }

    private (int fun, int graphic, int perfection, int genre) GetPresetForDifficulty(int presetID, int difficulty)
    {
        foreach (DifficultyPreset data in m_HeartPresetList)
        {
            if (presetID == data.PresetID && difficulty == data.Difficulty)
            {
                return (data.FunnyHeart, data.GraphicHeart, data.PerfectionHeart, data.GenreHeart);
            }
        }

        return (0, 0, 0, 0);
    }

    private void SetGameShowData(List<GameShowData> monthList, int listIndex, int difficulty)
    {
        int _fun = 0;
        int _graphic = 0;
        int _perfection = 0;
        int _genre = 0;

        if (listIndex == 0)
        {
            return;
        }

        var (randomFun, randomGraphic, randomPerfection, randomGenre) =
            GetPresetForDifficulty(monthList[listIndex].Preset_ID, difficulty);

        _fun = randomFun;
        _graphic = randomGraphic;
        _perfection = randomPerfection;
        _genre = randomGenre;

        monthList[listIndex].GameShow_State["Fun"] = _fun;
        monthList[listIndex].GameShow_State["Perfection"] = _perfection;
        monthList[listIndex].GameShow_State["Graphic"] = _graphic;
        monthList[listIndex].GameShow_State["Genre"] = _genre;
        monthList[listIndex].GameShow_Level = difficulty;

        if (monthList[listIndex].GameShow_Day == 0 || monthList[listIndex].GameShow_Week == 0)
        {
            int randomweek = UnityEngine.Random.Range(1, 5);
            int randomday = UnityEngine.Random.Range(1, 6);
            monthList[listIndex].GameShow_Day = randomday;
            monthList[listIndex].GameShow_Week = randomweek;
        }

        int _id = m_TotalGameShowData.FindIndex(x => x.GameShow_ID == monthList[listIndex].GameShow_ID);

        // find�� �ٲٱ�
        if (_id == -1)
        {
            m_TotalGameShowData.Add(monthList[listIndex]);

            if (m_GameShowHistory.ContainsKey(monthList[listIndex].GameShow_ID) == false)
                SetActiveAlram(true);
        }
    }

    #region _������ ���̺� �ʱ�ȭ �Լ���

    // ��Ʈ ������
    private void InitHeartPresetList()
    {
        // ������ 1
        m_HeartPresetList.Add(new DifficultyPreset(1, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 3, 3, 1, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 5, 2, 2, 3, 3));

        // ������ 2
        m_HeartPresetList.Add(new DifficultyPreset(2, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 3, 3, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(2, 5, 3, 3, 2, 2));

        // ������ 3
        m_HeartPresetList.Add(new DifficultyPreset(3, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 2, 1, 2, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 4, 1, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 5, 2, 3, 3, 2));

        // ������ 4
        m_HeartPresetList.Add(new DifficultyPreset(4, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 3, 1, 2, 3, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(4, 5, 3, 2, 3, 2));

        // ������ 5
        m_HeartPresetList.Add(new DifficultyPreset(5, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 3, 2, 3, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(5, 5, 3, 3, 2, 2));

        // ������ 6
        m_HeartPresetList.Add(new DifficultyPreset(6, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(6, 5, 3, 2, 3, 2));

        // ������ 7
        m_HeartPresetList.Add(new DifficultyPreset(7, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 4, 2, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(7, 5, 2, 3, 3, 2));

        // ������ 8
        m_HeartPresetList.Add(new DifficultyPreset(8, 1, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 2, 2, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 3, 2, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 4, 3, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(8, 5, 3, 3, 3, 2));
    }

    // ���Ӽ��� ���̵��� �����ִ� �Լ�
    private void InitGameShowDifficultry()
    {
        m_Difficulty.Add(new GameDifficulty(Rank.SSS, 5));
        m_Difficulty.Add(new GameDifficulty(Rank.SS, 5));
        m_Difficulty.Add(new GameDifficulty(Rank.S, 4));
        m_Difficulty.Add(new GameDifficulty(Rank.A, 4));
        m_Difficulty.Add(new GameDifficulty(Rank.B, 3));
        m_Difficulty.Add(new GameDifficulty(Rank.C, 3));
        m_Difficulty.Add(new GameDifficulty(Rank.D, 2));
        m_Difficulty.Add(new GameDifficulty(Rank.E, 2));
        m_Difficulty.Add(new GameDifficulty(Rank.F, 1));
    }

    // ���Ӽ��� ������ �򰡿� ���� ������ �����ִ� �Լ�
    private void InitGameShowReward()
    {
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Excellent, 1, 65000, 10, 7, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Excellent, 2, 120000, 12, 10, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Excellent, 3, 180000, 15, 12, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Excellent, 4, 250000, 19, 12, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Excellent, 5, 300000, 25, 17, 0));

        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Great, 1, 60000, 6, 6, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Great, 2, 100000, 9, 9, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Great, 3, 150000, 12, 11, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Great, 4, 200000, 16, 11, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Great, 5, 250000, 23, 16, 0));

        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Good, 1, 55000, 3, 5, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Good, 2, 80000, 6, 8, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Good, 3, 100000, 9, 10, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Good, 4, 150000, 13, 10, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Good, 5, 200000, 19, 12, 0));

        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.SoSo, 1, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.SoSo, 2, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.SoSo, 3, 80000, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.SoSo, 4, 100000, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.SoSo, 5, 150000, 0, 0, 0));

        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Bad, 1, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Bad, 2, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Bad, 3, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Bad, 4, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Bad, 5, 0, 0, 0, 0));

        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Terrible, 1, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Terrible, 2, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Terrible, 3, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Terrible, 4, 0, 0, 0, 0));
        m_Assessnent.Add(new GameShowRewardByDifficulty(Assessment.Terrible, 5, 0, 0, 0, 0));
    }

    // ������ ��Ʈ ������ ���� ����
    private void InitHeartScoreByLevel()
    {
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(1, 10, 1));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(2, 20, 1));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(3, 30, 1));

        m_ScoreByHeartList.Add(new GameShowScoreByHeart(1, 15, 2));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(2, 25, 2));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(3, 35, 2));

        m_ScoreByHeartList.Add(new GameShowScoreByHeart(1, 20, 3));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(2, 30, 3));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(3, 40, 3));

        m_ScoreByHeartList.Add(new GameShowScoreByHeart(1, 25, 4));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(2, 35, 4));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(3, 45, 4));

        m_ScoreByHeartList.Add(new GameShowScoreByHeart(1, 30, 5));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(2, 40, 5));
        m_ScoreByHeartList.Add(new GameShowScoreByHeart(3, 50, 5));
    }

    // ������ ���� �� ������ �����ֱ� ���� ���� �ʱ�ȭ
    private void InitScoreRangeList()
    {
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.Terrible, -10, -9999));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.Bad, -7, -9));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.NotBad, -4, -6));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.SoSo, -2, -3));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.Good, -1, 2));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.Great, 3, 6));
        m_ScoreRangeList.Add(new EvaluationScoreRange(Assessment.Excellent, 7, 9999));
    }

    // �򰡹��� �������� ������ ȯ�����ֱ� ���� ���� �ʱ�ȭ
    private void InitScoreAssessment()
    {
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.Terrible, -4));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.Bad, -1));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.NotBad, 0));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.SoSo, 1));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.Good, 2));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.Great, 3));
        m_ScoreConversionList.Add(new ScoreConversion(Assessment.Excellent, 4));
    }

    // �������� ���Ӽ��� ������ �������ֱ� ���� ���� �ʱ�ȭ
    private void InitFinalScoreList()
    {
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.Terrible, -20, -4));
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.Bad, -3, -1));
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.SoSo, 0, 3));
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.Good, 4, 10));
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.Great, 11, 17));
        m_FinalScoreRangeList.Add(new FinalScoreRange(Assessment.Excellent, 18, 20));
    }

    private void InitEvaluationResponseList()
    {
        #region _Terrible

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "�־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "�ð�����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "�������� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "�־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "�ð�����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "��Ʈ�� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "�־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "�ð�����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "���� �����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "�־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "�ð�����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "�־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "�ð�����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "���̾�"));

        #endregion

        #region _Bad

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "�ʹ��Ѱ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "������ �̻���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "�ǵ��� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "�ʹ��Ѱ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "������ �̻���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "�ȿ���"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "�ʹ��Ѱ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "������ �̻���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "���� ���ϳ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "�ʹ��Ѱ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "������ �̻���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "�̰�...�³�?"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "�ʹ��Ѱ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "������ �̻���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "�����̶� �ȸ¾�"));

        #endregion

        #region _NotBad

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "�׳������ϳ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "����� �ʿ���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "��̾���"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "�׳������ϳ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "����� �ʿ���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "�̰� �׷���?"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "�׳������ϳ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "����� �ʿ���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "����ȭ��..."));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "�׳������ϳ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "����� �ʿ���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "���ξ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "�׳������ϳ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "����� �ʿ���"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "������ ����"));

        #endregion

        #region _SoSo

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "����������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "��...�ƽ���"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "����������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "�� ������"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "����������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "���۰� ������"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "����������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "���"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "����������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "����"));

        #endregion

        #region _Good

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "��������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "�� �ϰ�ʹ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "���丮�� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "��������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "�� �ϰ�ʹ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "�׷����� ���ڳ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "��������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "�� �ϰ�ʹ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "���� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "��������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "�� �ϰ�ʹ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "��������?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "�� �ϰ�ʹ�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "�帣�� ��︮��"));

        #endregion

        #region _Great

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "����־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "��ð� ������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "������ ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "����־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "��ð� ������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "ĳ���� ���ƴ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "����־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "��ð� ������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "�������� ����"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "����־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "��ð� ������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "������!"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "����־�"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "��ð� ������"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "���� ����"));

        #endregion

        #region _Excellent

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "�ְ��"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "������ �����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "�Ǹ���"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "�ְ��"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "������ �����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "��� ���ƴ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "�ְ��"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "������ �����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "���۰� �ְ��"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "�ְ��"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "������ �����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "���ϴ�"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "�ְ��"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "������ �����"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "���� ���ƴ�"));

        #endregion
    }

    private void InitNoveltyScriptList()
    {
        m_NoveltyScriptList.Add("���� �̵鿡�� �̹� �˷��� �����̶�\n�̹� �ÿ�ȸ���� � ����� ������ ������ �� ���ھ��./���Ӱ� ������ ���ӵ�� ������ �� �������� ");
        m_NoveltyScriptList.Add("������ �ÿ� �߾��� ������ ��ǰ�ϼ̳׿�!/�̹� �������� �����̶� ���� ����� ���� ����� �ְھ��.");
        m_NoveltyScriptList.Add("���ο� ���� ��ǰ ��û�� �� ���� �ʾ����� �;��./���� ����� �ٷ�����! ");
    }

    private void InitGotoGameShowScriptList()
    {
        m_GotoGameShowScriptList.Add("���� ���Ӽ �ִ� ���̳׿�!\n�л����� ���� �ÿ��� ������ ��ġ�� �� �� �������?\n���Ӽ��忡 �ѹ� �ٳ�ͺ�����!");
        m_GotoGameShowScriptList.Add("������ ���Ӽ �ִ� ���Դϴ�.\n�ÿ������� �̵��� �л����� �ÿ�ȸ�� �����غ��ô�!");
    }

    // ������ ���â�� ����� ��ũ��Ʈ�� �ʱ�ȭ
    private void InitResultPanelScript()
    {
        #region _introScript

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "�ƽ����� ������ ������ô�."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "������ ù ���� �ÿ�ȸ���׿�!\n�������� ������ �帣���� �� �����Ͽ� �����غ��� ����?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "�����մϴ�! ù �����ε� ����� ���׿�."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "ū �Ը��� ���Ӽ�µ� ���� ������ �ŵξ����ϴ�! �����մϴ�!"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "�ÿ�ȸ�� ����� �־��Դϴ�!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "�ÿ�ȸ�� ������ �ɻ�ġ�ʽ��ϴ�\n�����ǹ̷ο�."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "�ÿ�ȸ�� ������ �ɰ��մϴ�!\n�̷������� ���ӵȴٸ� �츮 ��ī���� ������ �ٴ��� ĥ ���Դϴ�."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "�ÿ�ȸ�� ���� ���� �������� �������Ǿ����ϴ�..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "�츮 ��ī���̸� �ٶ󺸴� ������� ���ʸ��� �ɻ�ġ �ʽ��ϴ�...!\n�̰���...���?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "�ÿ�ȸ�� ���� ������ �Ǿ����ϴ�.\n����...�β����׿�"));

        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 1, "�ÿ�ȸ�� ������ �Ǿ����ϴ�."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "�ÿ�ȸ�� ���������� ������ �Ǿ����ϴ�!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "�ÿ�ȸ�� ����� �����ϴ�!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "�ÿ�ȸ�� �Ǹ��ϰ� ������ �Ǿ����ϴ�."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "�츮 ��ī���̿� ���� ������ �߰̽��ϴ�!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "�ÿ�ȸ�� ���� ������ �Ǿ����ϴ�.\n���� �ѵ��ϳ׿�."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "�ÿ�ȸ�� ���������� ������ �Ǿ����ϴ�!"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "�������ó��� �� ���� ����?\n�ÿ�ȸ�� ���������� ������ �Ǿ����ϴ�!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "�ÿ�ȸ ������ �ɻ�ġ �ʽ��ϴ�.\n�������!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "�ÿ�ȸ�� ���������� ������ �Ǿ����ϴ�!"));


        #endregion

        #region _mainScript

        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "Ȧ�� �x"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "��¥ �˰��̳׿�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "�ð� �Ʊ����"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "�߰� ������ �����մϴ�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "�й��ϼ���"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "�ʹ� ��̰� �����..."));

        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "�߰� ������ �ʿ��غ��Դϴ�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "��..���Ŵ� ���� �� ���׿�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "�����մϴ�"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "�츮 ���̰� �����ؿ�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "9ood ��r��b��H"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "��-��"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "�ʹ� ��վ����"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "�ո��� �Ǹ��մϴ�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "ȦȦȦ...�ֵ����� ���帮�ڳ�"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "�� 27�� �λ� �ְ��� �����̾�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "���� ������ּ��� ����������"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "��� �������� ������ �� �� ���ƿ�"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "â�Ǽ��� �ſ� �پ ����־�����"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "�������� �� �� �����̱���"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "�׳� ���� ��ü�� ����־�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "���� ���� ��Ƽ �����"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "���ΰ� ������"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "�����ϳ� �� ����"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "��Ʈ ���� ��¥ �׿����"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "�̰ԡ�21���� �׷���? ���� �μš�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "��Ʈ ���� �����߾�� ��� �� �ʿ� ����"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "�� ���� ��� �ӿ��� ��� �;�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "ĳ���� ���� �ʹ� ������..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "�� ����ģ���� ���Ӽӿ� �־���"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "���۰� ��¥ �Ǹ��մϴ�"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "�̰� ������ ��� �Ѱ���?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "�������� �˾Ҿ��"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "���ǰ� ������ ������ �� �� ����..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "�ϼ����� ���� �����̾�"));

        #endregion

        #region _endScript

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 6, "����ϼ̽��ϴ�!"));

        #endregion
    }

    #endregion

    // �� ������ �´� ���̵����� �켱������ �� �޿� ������ ���Ӱ� ��ġ�ϴ� ����/�帣�� �����Ѵ�.
    // ����� �ϴ� good�� �⺻
    private void SortMyLevelList(List<GameShowData> dataList)
    {
        string _gameGenre = "";
        string _gameConcept = "";

        for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (GameTime.Instance.FlowTime.NowMonth - 1 ==
                    GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_MakeMonth)
                {
                    GameJamInfo _nowData = GameJam.SearchAllGameJamInfo(GameJam.m_GameJamHistory.ElementAt(i).Key);

                    _gameConcept = _nowData.m_GjamConcept;
                    _gameGenre = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_Genre;
                    break;
                }
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            var (reward, famous, passion, health) = GetRewardByAssessment(dataList[i].GameShow_Level, Assessment.Good);

            dataList[i].GameShow_Reward = reward;
        }

        // ���ǿ� ���� concept�� genre�� �� ä������ �ϳ��� ���Ѵ�.
        if (_gameConcept != "")
        {
            dataList.OrderBy(x =>
            {
                if (x.GameShowConcept[0] == "0")
                {
                    // "��� ����" �� ���� �������� ���
                    // ������ �������� ��� ����ϰ� �Ǵµ�, �̰��� ���� ������ ��ġ�ϸ� ������ �����Ѵ�.
                    // �̴� �ᱹ 1/40 Ȯ�� �˻��̱� ������ ������ ���� ���´�.
                    return UnityEngine.Random.Range(0, 40) == 0 ? -1 : 1;
                }

                bool _compareConcept = _gameConcept == x.GameShowConcept[0] ||
                                       (x.GameShowConcept.Length == 2 && _gameConcept == x.GameShowConcept[1]);
                if (_compareConcept)
                    return -1;

                bool _compareGenre = _gameGenre == x.GameShowGenre[0] ||
                                     (x.GameShowGenre.Length == 2 && _gameGenre == x.GameShowGenre[1]);
                if (_compareGenre)
                    return -1;

                return 1;
            });
        }
    }

    // ���Ӽ� ���â�� ������ �켱������ ��µ� �� �ְ� ������ ���ش�.
    // �ű�, ���ӳ��̵� ������, ��� ���� ��, ������ ����
    // �켱 ����� �⺻�� good
    private void SortRecommendList()
    {
        for (int i = 0; i < m_TotalGameShowData.Count; i++)
        {
            var (_reward, famous, passion, health) =
                GetRewardByAssessment(m_TotalGameShowData[i].GameShow_Level, Assessment.Good);
            m_TotalGameShowData[i].GameShow_Reward = _reward;
        }

        m_TotalGameShowData = m_TotalGameShowData.OrderByDescending(x =>
            {
                if (!m_GameShowHistory.ContainsKey(x.GameShow_ID))
                    return 1;
                else
                    return 0;
            }).ThenBy(x => x.GameShow_Level)
            .ThenByDescending(x => x.GameShow_Reward)
            .ThenBy(x => x.GameShow_Name, StringComparer.CurrentCulture).ToList();
    }

    // �� ��ī���� ������ ���� ���Ӽ� ���̵� �̾��ִ� �Լ�
    private int GetRankByMyAcademyRank(Rank myRank)
    {
        foreach (GameDifficulty table in m_Difficulty)
        {
            if (table.MyRank == myRank)
            {
                return table.Difficulty;
            }
        }

        return 0;
    }

    // ���� ���Ӽ�� ���� �� �������� ��� �� ���
    private (int _rewardMoney, int _famous, int _passion, int _health) GetRewardByAssessment(int level,
        Assessment assessment)
    {
        foreach (GameShowRewardByDifficulty table in m_Assessnent)
        {
            if (table.Level == level && table.GameShowAssessment == assessment)
            {
                return (table.RewardByAssessment, table.Famous, table.StudentPassion, table.StudentHealth);
            }
        }

        return (0, 0, 0, 0);
    }

    // ��Ʈ ������ ������ �� ������ ������ִ� �Լ�
    private int GetScoreByHeart(int level, int heartCount)
    {
        foreach (GameShowScoreByHeart table in m_ScoreByHeartList)
        {
            if (table.Level == level && table.HeartCount == heartCount)
            {
                return table.Score;
            }
        }

        return 0;
    }

    // �� ������ ���� �� ����
    private Assessment GetGameShow(int value)
    {
        foreach (EvaluationScoreRange table in m_ScoreRangeList)
        {
            if (table.MinValue <= value && value <= table.MaxValue)
            {
                return table.GameShowAssessment;
            }
        }

        return 0;
    }

    // �򰡹��� ������ ������ ȯ���ϴ� �Լ�
    private int GetScoreConversion(Assessment _scoreAssessment)
    {
        foreach (ScoreConversion table in m_ScoreConversionList)
        {
            if (_scoreAssessment == table.ScoreAssessment)
            {
                return table.Score;
            }
        }

        return 0;
    }

    // �� ������ ���� �� ����
    private Assessment FinalGameShowScore(int value)
    {
        foreach (FinalScoreRange table in m_FinalScoreRangeList)
        {
            if (table.MinValue <= value && value <= table.MaxValue)
            {
                return table.FinalAssessment;
            }
        }

        return 0;
    }

    // �� ������ ���� ��ũ��Ʈ ���� ���
    private string GetJudgesScript(Assessment _assessment, string _part)
    {
        List<AssessmentResponse> filteredList = m_AssessmentResponseList
            .FindAll(response => response.PartAssessment == _assessment && response.Part == _part);

        int count = filteredList.Count;

        if (count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, count);
            return filteredList[randomIndex].Script;
        }

        return "";
    }

    // ���â�� ����� ��ũ��Ʈ�� ���ǿ� �°� �����ϰ� ���
    private string GetResultScript(Assessment _assessment, int _scriptOrder)
    {
        List<ResultScript> filteredList =
            m_ResultScriptList.FindAll(x => x.ResultAssessment == _assessment && x.ScriptOrder == _scriptOrder);

        int count = filteredList.Count;

        if (count > 0 && !_isFirstGameShow)
        {
            int randomIndex = UnityEngine.Random.Range(0, count);
            return filteredList[randomIndex].Script;
        }
        else
        {
            int _index = _assessment == Assessment.Terrible ? 0 : _assessment == Assessment.SoSo ? 1 : 2;
            _isFirstGameShow = false;
            return filteredList[_index].Script;
        }
    }
}

public class StaticDeepCopy
{
    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }
}
