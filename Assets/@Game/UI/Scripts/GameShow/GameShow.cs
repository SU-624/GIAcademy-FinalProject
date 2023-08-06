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
    Terrible, // 최악
    Bad, // 나쁨
    NotBad,
    SoSo, // 보통
    Good, // 좋음
    Great, // 아주좋음
    Excellent, // 최상
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
    [SerializeField] private GameObject m_GameShowListPrefab; // 게임쇼 프리팹
    [SerializeField] private GameObject m_PrevGameListPrefab; // 게임잼에서 만든 게임들의 프리팹

    [SerializeField] private GameObject m_RewardPrefab;

    [SerializeField] private TextMeshProUGUI m_NoveltyScript; // 참신함이 1미만일 때 띄워줄 패널의 텍스트
    [SerializeField] private TextMeshProUGUI m_GoTOGameShowResultPanel; // 게임쇼의 결과를 보러가기 전에 띄워주는 패널의 텍스트
    [SerializeField] private TextMeshProUGUI m_GameShowResultPanelText; // 게임쇼의 결과를 보여주기 위해 띄워주는 패널의 텍스트
    [SerializeField] private Sprite[] m_RewardSprite;
    [Space(5f)]
    [Header("껐다가 켜줄 패널들의 PopUp, PopOff 오브젝트들")]
    [SerializeField] private PopUpUI m_PopUpResultPanel; // 게임쇼 결과창을 보러갈 popUP창
    [SerializeField] private PopOffUI m_PopOffResultPanel;
    [SerializeField] private PopUpUI m_PopUpResultAnimationPanel; // 게임쇼 결과에 따른 애니메이션을 보여줄 패널
    [SerializeField] private PopOffUI m_PopOffResultAnimationPanel;
    [SerializeField] private PopUpUI m_PopUpNoveltyWarningPanel; // 참신함이 1미만으로 내려갈 때 띄워줄 패널
    [SerializeField] private PopOffUI m_PopOffNoveltyWarningPanel;
    [SerializeField] private PopUpUI m_PopUpResultScriptPanel; // 결과창에서 보상을 받기전 보여주는 스크립트 패널
    [SerializeField] private PopOffUI m_PopOffResultScriptPanel;
    [SerializeField] private PopUpUI m_PopUpRewardPanel; // 보상 패널
    [SerializeField] private PopOffUI m_PopOffRewardPanel;

    [Space(5f)]
    [Header("튜토리얼용")]
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
    /// 게임쇼 셋팅과 판별에 필요한 데이터들을 담아둔 리스트들
    private List<GameDifficulty> m_Difficulty = new List<GameDifficulty>(); // 내 아카데미 등급에 따른 난이도를 설정하는 리스트

    private List<DifficultyPreset> m_HeartPresetList = new List<DifficultyPreset>();

    private List<GameShowRewardByDifficulty> m_Assessnent = new List<GameShowRewardByDifficulty>(); // 난이도와 평가반응에 따른 보상을 설정하는 리스트

    private List<GameShowScoreByHeart> m_ScoreByHeartList = new List<GameShowScoreByHeart>();
    private List<EvaluationScoreRange> m_ScoreRangeList = new List<EvaluationScoreRange>(); // 평가 점수에 따른 반응
    private List<ScoreConversion> m_ScoreConversionList = new List<ScoreConversion>(); // 평가 점수로 받은 반응으로 점수 환산
    private List<FinalScoreRange> m_FinalScoreRangeList = new List<FinalScoreRange>(); //

    private List<AssessmentResponse> m_AssessmentResponseList = new List<AssessmentResponse>(); // 각 파트별로 반응에 따라 출력해줄 텍스트들

    private List<ResultScript> m_ResultScriptList = new List<ResultScript>(); // 마지막 결과 패널에 띄워줄 스크립트 값들
    private List<string> m_NoveltyScriptList = new List<string>(); // 참신함 점수가 1이 아닐 때 띄워줄 스크립트들
    private List<string> m_GotoGameShowScriptList = new List<string>(); // 게임쇼장을 이동할 때 띄워줄 스크립트들(결과창 이동하는 패널)

    ///
    private List<GameShowData> m_TotalGameShowData = new List<GameShowData>(); // 이번달 공고에 띄워줄 게임쇼들을 모두 모아놓을 리스트

    private static List<GameShowData> m_FixedGameShowData = new List<GameShowData>();
    private static List<GameShowData> m_RandomGameShowData = new List<GameShowData>();

    private GameShowSaveData _mTemporaryData = new GameShowSaveData(); // 내가 클릭한 게임쇼들의 정보를 임시로 저장해주는 변수
    private List<GameShowSaveData> m_ToBeRunningGameShow = new List<GameShowSaveData>(); // 현재 내가 참가하는 게임쇼들을 담아둘 리스트

    private Queue<Action> eventQueue = new Queue<Action>(); // 게임잼이 실행되고 나면 게임쇼를 참가할 준비를 해줘야한다. timescale이 0일 때 이벤트를 실행하니 안돼서 Update문에서 timescale이 0 보다 클 때 실행시켜주는걸로 변경

    public static Dictionary<int, List<GameShowSaveData>> m_GameShowHistory = new Dictionary<int, List<GameShowSaveData>>();

    private Queue<string> m_ResultDialogQueue = new Queue<string>(); // 마지막 결과창에 띄워줄 스크립트들을 출력 순서대로 넣어줄 큐
    private Queue<string> m_NoveltyDialoge = new Queue<string>();
    public Sprite m_ApplySuccessSprite;
    public Sprite m_NotAllowedApplySprite;
    public Sprite m_ApplySprite;
    public Sprite[] m_FinalScoreSprite;
    public Sprite[] m_EmojiSprite;
    public Sprite[] m_StudentFace;
    private GameObject m_PrevClickObj;                                          // 내가 이전에 클릭한 게임잼 버튼을 담아둘 변수
    private GameObject m_PrevClickGameShowObj;                                   // 게임잼 버튼 스프라이트 고정을 위해 이전 클릭한 게임잼 오브젝트를 담아둘 변수
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

                StartCoroutine(m_ActivityEvent.TypingText(eventPrefab, "게임쇼 준비중"));
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

    // 게임쇼 버튼을 누르면 실행시켜줄 함수
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

    // 보상 패널에서 받기 버튼을 눌렀을 때 
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

    #region _delegate로 등록할 함수들

    // 게임쇼를 켜주는 버튼이 클릭되었을 때 실행할 함수들.
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

    // 신청 버튼이 클릭되었을 때 실행 할 함수.
    private void HandleGameShowListApplyButton()
    {
        if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_ApplySprite && m_MonthLimit > 0)
        {
            //// 이때 신청 완료하고 신청버튼 신청완료로 변경
            if (_isCanParticipate && _isSameDayInList)
            {
                ApplySuccess();
            }
            else if (!_isSameDayInList)
            {
                m_GameShowPanel.SetConditionFailPanel("이미 해당 날짜에 참가한 게임쇼가 있습니다!");
            }
            else if (!_isPrevGameJamButtonClick)
            {
                m_GameShowPanel.SetConditionFailPanel("출품할 게임을 선택해 주세요!");
            }
            // 아니라면 신청할 수 없다고 경고 메세지 띄워주기
            else
            {
                ApplyFailed();
            }
        }
        // 참여횟수 모두 사용 
        else if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_NotAllowedApplySprite &&
                 m_MonthLimit <= 0)
        {
            // 버튼 클릭시 참가할 수 있는 횟수를 초과했다는 메세지 띄워주기
            m_GameShowPanel.SetConditionFailPanel("참가할 수 있는 횟수를 초과하였습니다!\n다음 달을 노려보세요.");
        }
        // else : 신청했던 버튼은 눌러도 따로 뭐 없음.

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

    // 점수계산하는 함수
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

        // 게임쇼가 끝나고 점수까지 저장할 때 필요
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

    // 받은 점수에 따라 재생할 애니메이션을 다르게 해줄 함수
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

    // 결과창으로 이동하기 위해 띄워주는 패널에 쓰여질 스크립트 가져오기
    private void GetGameShowResultPanelScript()
    {
        int _index = UnityEngine.Random.Range(0, m_GotoGameShowScriptList.Count);

        m_GoTOGameShowResultPanel.text = m_GotoGameShowScriptList[_index];

        m_PopUpResultPanel.TurnOnUI();
        m_ActivityEvent.m_IsCheckGameShow = true;
    }

    // BGameDesigner재생 시간만큼 기다렸다가 패널 변경해주기
    IEnumerator ChangeAnimationPanel()
    {
        yield return new WaitForSecondsRealtime(5f);
        
        m_SetSatisfactionStatusPanelCor = SetSatisfactionStatusPanel();

        StartCoroutine(m_SetSatisfactionStatusPanelCor);
    }

    #region _심사위원 애니메이션 싱크를 맞추기 위한 함수들

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

    // 게임 만족도 현황을 보여주기 위한 패널에 스크립트와 이미지를 넣어주는 함수
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

    // 보상받는 패널이 나올 때 보상이 3개 이상이면 재생할 코루틴
    // 맨 뒤로갔다가 다시 앞으로 돌아온다.
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

    // 결과창 스크립트를 조건에 따라 랜덤으로 뽑아 큐에 넣어주는 함수
    // 재미 점수가 높으면 3, 그래픽 점수가 높으면4, 완성도 점수가 높으면 5
    private void FindResultScript(Assessment _assessment, int _funScore, int _graphicScore, int _perfectionScore)
    {
        // intro멘트
        m_ResultDialogQueue.Enqueue(GetResultScript(_assessment, 1));

        // main멘트
        string _mainText1 = GetResultScript(_assessment, 2);

        int max = _funScore > _graphicScore ? 3 : 4;
        max = max > _perfectionScore ? 5 : max;

        string _mainText2 = GetResultScript(Assessment.None, max);
        char lastText = _mainText2.ElementAt(_mainText2.Length - 1);

        string selectText = (lastText - 0xAC00) % 28 > 0 ? "이" : "가";
        m_ResultDialogQueue.Enqueue("시연회 반응으로는 \"" + _mainText1 + "\"," + "\"" + _mainText2 + "\"" + selectText + " 있네요.");

        // 마무리멘트
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

    // 결과에 따른 보상을 만들어준다.
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
            _reward.GetComponent<RewardPrefab>().m_Title.text = "상금";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[0];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = string.Format("{0:#,0}", m_RewardMoney);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardFamous != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardFamous";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "인지도";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[1];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = m_RewardFamous.ToString();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardPassion != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardPassion";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "열정";
            _reward.GetComponent<RewardPrefab>().m_Icon.sprite = m_RewardSprite[2];
            _reward.GetComponent<RewardPrefab>().m_RewardText.text = m_RewardPassion.ToString();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        if (m_RewardHealth != 0)
        {
            GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
            _reward.name = "RewardHealth";
            _reward.GetComponent<RewardPrefab>().m_Title.text = "체력";
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

    // 게임쇼 패널을 닫을 때 마다 해줄 데이터 초기화
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

    // 게임쇼가 끝날 때 초기화 해줄것들
    private void InitGameShowPanel()
    {
        m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[3], m_StudentFace[4], m_StudentFace[5]);
        m_GameShowAnimationPanel.InitPanel();
        m_GameShowAnimationPanel.ChangeEvalutionResponseScore("0");
        m_GameShowAnimationPanel.StopAnimation();
    }

    // 게임선택 버튼을 눌렀을 때 이전에 만든 게임 목록을 만들어줄 함수
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

                if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "미완성")
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

                    if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "미완성")
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

                    if ((_gameJamRank == "" || _gameJamRank == _rank) && _gameJamRank != "미완성")
                        MakePrevGameList(i, j);
                }
            }
        }
    }

    // 게임잼에서 만든 게임들을 만들 함수
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

    // 내가 선택한 게임 찾아서 먼저 만들어주기
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

    // 일단 기존에 있던 게임 목록들을 지워주고 해당 랭크의 게임 목록들만 만들어준다.
    /// TODO : 게임쇼에 내보낸 게임이 있으면 맨 위로 올리고 나머지를 만들어준다.
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

    // 출품할 게임 버튼을 눌렀을 때 게임 제작자의 체력과 열정을 보고 부족하다면 참가할 수 없다고 띄우고 신청버튼은 활성화 상태로 아무 기능을 안한다.
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
        // 내가 이미 신청한 게임잼이 들어있는데 누른거라면 넘겨주기
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

            StartCoroutine(m_ActivityEvent.TypingText(_eventPrefab, "게임쇼 준비중"));
        }
        else if (m_MonthLimit <= 0)
        {
            // 참여횟수 끝~
            m_GameShowPanel.SetConditionFailPanel("이번달 참여는 끝~");
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
                               "학생의 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "학생의 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
            }
            else if (_compareStudentHealthAndPassion[i].m_StudentStat.m_Health < _gameShowData.GameShow_Health)
            {
                if (_compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName != "")
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName +
                               "학생의 체력이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "학생의 체력이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
            }
            else
            {
                if (_compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName != "")
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_UserSettingName +
                               "학생의 체력과 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
                else
                {
                    _text[i] = _compareStudentHealthAndPassion[i].m_StudentStat.m_StudentName +
                               "학생의 체력과 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
                }
            }
        }

        m_GameShowPanel.SetConditionFailPanel(_text[0] + "\n" + _text[1] + "\n" + _text[2] + "\n");
    }

    // 같은 날짜의 게임쇼를 참가 못하게 하기위해 있는지 확인해보는 함수
    private bool CheckGameShowDay()
    {
        GameShowData _totalGameShowData = new GameShowData();
        GameShowData _temporaryGameShowData = new GameShowData();

        // 실행시킬 데이터가 있으면 찾아서 넣어주고 없다면 그냥 초기화 시켜준다.
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
            || _gameShowData.GameShowGenre[0] == "모든 장르" && _gameShowData.GameShowGenre[0] == "모든 컨셉")
        {
            m_GenreAndConceptHeart = 3;
        }
        else if (_gameShowData.GameShowConcept.Any(c => c == _nowData.m_GjamConcept)
                 || _gameShowData.GameShowGenre.Any(g =>
                     g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_Genre)
                 || _gameShowData.GameShowGenre[0] == "모든 장르" || _gameShowData.GameShowGenre[0] == "모든 컨셉")
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
                    // 한명이라도 부족하면 안됨.
                    _isStudentConditionFine = false;
                    break;
                }
            }
        }

        return _isStudentConditionFine;
    }

    // 만들어진 게임쇼 목록의 버튼을 눌렀을 때 패널의 정보들을 셋팅해주는 함수
    // 신청한 정보가 있으면 신청 완료 버튼 스프라이트로 변경해주고 반응없게 하기
    private void ClickGameShowListButton()
    {
        #region _버튼을 클릭했을 때 하이라이트가 남아있게 하기

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

    // 게임쇼 제일 처음 부분. 
    private void SetFirstGameShowInfo()
    {
        #region _맨 위에있는 버튼에 하이라이트 해주기
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
        string _gameShowProgressDate = (m_TotalGameShowData[_index].GameShow_Month.ToString()) + "월 " +
                                       (m_TotalGameShowData[_index].GameShow_Week.ToString()) + "주";
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

    // 내 아카데미 등급에 따른 게임쇼
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
            // 매년 발생하는 공고
            if (m_FixedGameShowData[i].GameShow_Year == 0 &&
                m_FixedGameShowData[i].GameShow_Month == _nowMonth)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }
            // 홀수년도 발생
            else if (m_FixedGameShowData[i].GameShow_Year == -1 &&
                     (10 % GameTime.Instance.FlowTime.NowYear) != 0 &&
                     m_FixedGameShowData[i].GameShow_Month == _nowMonth)   // 12월에는 1로 바꿔주기
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }
            // 짝수년도 발생
            else if (m_FixedGameShowData[i].GameShow_Year == -2 &&
                     (10 % GameTime.Instance.FlowTime.NowYear) == 0 &&
                     m_FixedGameShowData[i].GameShow_Month == _nowMonth)
            {
                m_MonthGameShowData.Add(m_FixedGameShowData[i]);

            }

        }

        switch (difficulty)
        {
            // 난이도1 2개, 난이도 2,3,4,5 중 1개
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

            // 난이도1 1개, 난이도 2 1개, 난이도 3,4,5 중 1개
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

            // 난이도1,2 중 1개, 난이도 3 1개, 난이도 4,5 중 1개
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
            // 난이도1,2,3 중 1개, 난이도 4 1개, 난이도 5 1개
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

            // 난이도1,2,3,4 중 1개, 난이도 5 2개
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

        // find로 바꾸기
        if (_id == -1)
        {
            m_TotalGameShowData.Add(monthList[listIndex]);

            if (m_GameShowHistory.ContainsKey(monthList[listIndex].GameShow_ID) == false)
                SetActiveAlram(true);
        }
    }

    #region _데이터 테이블 초기화 함수들

    // 하트 프리셋
    private void InitHeartPresetList()
    {
        // 프리셋 1
        m_HeartPresetList.Add(new DifficultyPreset(1, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(1, 3, 3, 1, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(1, 5, 2, 2, 3, 3));

        // 프리셋 2
        m_HeartPresetList.Add(new DifficultyPreset(2, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 3, 3, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(2, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(2, 5, 3, 3, 2, 2));

        // 프리셋 3
        m_HeartPresetList.Add(new DifficultyPreset(3, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 2, 1, 2, 1, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(3, 4, 1, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(3, 5, 2, 3, 3, 2));

        // 프리셋 4
        m_HeartPresetList.Add(new DifficultyPreset(4, 1, 1, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 3, 1, 2, 3, 1));
        m_HeartPresetList.Add(new DifficultyPreset(4, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(4, 5, 3, 2, 3, 2));

        // 프리셋 5
        m_HeartPresetList.Add(new DifficultyPreset(5, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 2, 2, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 3, 2, 3, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(5, 4, 3, 2, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(5, 5, 3, 3, 2, 2));

        // 프리셋 6
        m_HeartPresetList.Add(new DifficultyPreset(6, 1, 2, 1, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 2, 2, 1, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(6, 4, 2, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(6, 5, 3, 2, 3, 2));

        // 프리셋 7
        m_HeartPresetList.Add(new DifficultyPreset(7, 1, 1, 2, 1, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 2, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 3, 1, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(7, 4, 2, 3, 2, 2));
        m_HeartPresetList.Add(new DifficultyPreset(7, 5, 2, 3, 3, 2));

        // 프리셋 8
        m_HeartPresetList.Add(new DifficultyPreset(8, 1, 1, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 2, 2, 2, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 3, 2, 3, 2, 1));
        m_HeartPresetList.Add(new DifficultyPreset(8, 4, 3, 2, 3, 2));
        m_HeartPresetList.Add(new DifficultyPreset(8, 5, 3, 3, 3, 2));
    }

    // 게임쇼의 난이도를 정해주는 함수
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

    // 게임쇼의 레벨과 평가에 따른 보상을 정해주는 함수
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

    // 레벨과 하트 갯수에 따른 점수
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

    // 점수에 따른 평가 반응을 보여주기 위한 범위 초기화
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

    // 평가받은 반응으로 점수를 환산해주기 위한 범위 초기화
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

    // 최종적인 게임쇼의 점수를 결정해주기 위한 범위 초기화
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

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "최악"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "시간낭비"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Fun", "디자인이 문제"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "최악"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "시간낭비"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Graphic", "아트가 구려"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "최악"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "시간낭비"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Perfection", "버그 댕오바"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "최악"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "시간낭비"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Genre", "노잼"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "최악"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "시간낭비"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Terrible, "Concept", "똥이야"));

        #endregion

        #region _Bad

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "너무한걸"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "게임이 이상해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Fun", "의도가 뭐야"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "너무한걸"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "게임이 이상해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Graphic", "안예뻐"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "너무한걸"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "게임이 이상해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Perfection", "렉이 심하네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "너무한걸"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "게임이 이상해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Genre", "이게...맞나?"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "너무한걸"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "게임이 이상해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Bad, "Concept", "컨셉이랑 안맞아"));

        #endregion

        #region _NotBad

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "그냥저냥하네"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "고민이 필요해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Fun", "재미없어"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "그냥저냥하네"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "고민이 필요해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Graphic", "이게 그래픽?"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "그냥저냥하네"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "고민이 필요해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Perfection", "최적화가..."));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "그냥저냥하네"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "고민이 필요해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Genre", "별로야"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "그냥저냥하네"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "고민이 필요해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.NotBad, "Concept", "컨셉이 구려"));

        #endregion

        #region _SoSo

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "무난해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "괜찮을지도?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Fun", "흠...아쉽네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "무난해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "괜찮을지도?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Graphic", "퀄 괜찮네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "무난해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "괜찮을지도?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Perfection", "조작감 괜찮네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "무난해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "괜찮을지도?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Genre", "쏘쏘"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "무난해"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "괜찮을지도?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.SoSo, "Concept", "좋네"));

        #endregion

        #region _Good

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "괜찮은데?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "또 하고싶다"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Fun", "스토리가 좋아"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "괜찮은데?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "또 하고싶다"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Graphic", "그래픽이 예쁘네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "괜찮은데?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "또 하고싶다"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Perfection", "렉이 없어"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "괜찮은데?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "또 하고싶다"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Genre", "구웃"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "괜찮은데?"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "또 하고싶다"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Good, "Concept", "장르랑 어울리네"));

        #endregion

        #region _Great

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "재미있어"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "출시가 언제지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Fun", "레벨이 좋아"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "재미있어"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "출시가 언제지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Graphic", "캐릭터 미쳤다"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "재미있어"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "출시가 언제지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Perfection", "게임퀄이 좋다"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "재미있어"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "출시가 언제지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Genre", "구뤠잇!"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "재미있어"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "출시가 언제지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Great, "Concept", "컨셉 좋네"));

        #endregion

        #region _Excellent

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "최고야"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "나오면 사야지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Fun", "훌륭해"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "최고야"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "나오면 사야지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Graphic", "배경 미쳤다"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "최고야"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "나오면 사야지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Perfection", "조작감 최고네"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "최고야"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "나오면 사야지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Genre", "핫하다"));

        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "최고야"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "나오면 사야지"));
        m_AssessmentResponseList.Add(new AssessmentResponse(Assessment.Excellent, "Concept", "컨셉 미쳤다"));

        #endregion
    }

    private void InitNoveltyScriptList()
    {
        m_NoveltyScriptList.Add("많은 이들에게 이미 알려진 게임이라\n이번 시연회때는 어떤 결과를 얻을지 예상할 수 없겠어요./새롭고 참신한 게임들과 경쟁할 수 있을지… ");
        m_NoveltyScriptList.Add("저번에 시연 했었던 게임을 출품하셨네요!/이미 선보였던 게임이라 좋은 결과를 얻기는 힘들수 있겠어요.");
        m_NoveltyScriptList.Add("새로운 게임 출품 신청이 더 좋지 않았을까 싶어요./좋은 결과를 바래보죠! ");
    }

    private void InitGotoGameShowScriptList()
    {
        m_GotoGameShowScriptList.Add("드디어 게임쇼가 있는 날이네요!\n학생들은 게임 시연을 무사히 마치고 올 수 있을까요?\n게임쇼장에 한번 다녀와보세요!");
        m_GotoGameShowScriptList.Add("오늘은 게임쇼가 있는 날입니다.\n시연장으로 이동해 학생들의 시연회에 참가해봅시다!");
    }

    // 마지막 결과창에 띄워줄 스크립트를 초기화
    private void InitResultPanelScript()
    {
        #region _introScript

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "아쉽지만 다음을 노려봅시다."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "무난한 첫 게임 시연회였네요!\n다음번엔 컨셉과 장르까지 잘 염두하여 참가해보면 어떨까요?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "축하합니다! 첫 시작인데 출발이 좋네요."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 1, "큰 규모의 게임쇼였는데 좋은 성적을 거두었습니다! 축하합니다!"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "시연회의 결과가 최악입니다!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "시연회의 반응이 심상치않습니다\n나쁜의미로요."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 1, "시연회의 반응이 심각합니다!\n이런식으로 지속된다면 우리 아카데미 평판은 바닥을 칠 것입니다."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "시연회가 좋지 않은 성적으로 마무리되었습니다..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "우리 아카데미를 바라보는 사람들의 눈초리가 심상치 않습니다...!\n이것은...경멸?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 1, "시연회가 드디어 마무리 되었습니다.\n조금...부끄럽네요"));

        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 1, "시연회가 마무리 되었습니다."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "시연회가 성공적으로 마무리 되었습니다!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "시연회의 결과가 좋습니다!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 1, "시연회가 훌륭하게 마무리 되었습니다."));

        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "우리 아카데미에 대한 반응이 뜨겁습니다!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "시연회가 드디어 마무리 되었습니다.\n정말 뿌듯하네요."));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 1, "시연회가 성공적으로 마무리 되었습니다!"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "느껴지시나요 이 핫한 열기?\n시연회가 성공적으로 마무리 되었습니다!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "시연회 반응이 심상치 않습니다.\n놀랍군요!"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 1, "시연회가 성공적으로 마무리 되었습니다!"));


        #endregion

        #region _mainScript

        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "홀리 쉣"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "진짜 똥겜이네요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Terrible, 2, "시간 아까워요"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "추가 개발이 절실합니다"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "분발하세요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Bad, 2, "너무 재미가 없어요..."));

        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "추가 개발이 필요해보입니다"));
        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "음..구매는 안할 것 같네요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.SoSo, 2, "무난합니다"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "우리 아이가 좋아해요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "9ood ㅅrㄹbㅎH"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Good, 2, "갓-겜"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "너무 재밌었어요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "손맛이 훌륭합니다"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Great, 2, "홀홀홀...쌍따봉을 내드리겠네"));

        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "내 27년 인생 최고의 게임이야"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "제발 출시해주세요 현기증나요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.Excellent, 2, "계속 생각나는 게임이 될 것 같아요"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "창의성이 매우 뛰어나 재미있었서요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "디자인이 잘 된 게임이군요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "그냥 게임 자체가 재미있어"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "존잼 갓작 고티 가즈아"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "주인공 내꺼해"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 3, "명작하나 본 느낌"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "아트 퀄이 진짜 죽여줘요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "이게…21세기 그래픽? 눈이 부셔…"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "아트 보고 개안했어요 라식 할 필요 없음"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "저 게임 배경 속에서 살고 싶어"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "캐릭터 누나 너무 예뻐요..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 4, "내 남자친구가 게임속에 있었네"));

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "조작감 진짜 훌륭합니다"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "이거 구현을 어떻게 한거지?"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "현실인줄 알았어요"));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "현실과 가상을 구별해 낼 수 없어..."));
        m_ResultScriptList.Add(new ResultScript(Assessment.None, 5, "완성도가 높은 게임이야"));

        #endregion

        #region _endScript

        m_ResultScriptList.Add(new ResultScript(Assessment.None, 6, "고생하셨습니다!"));

        #endregion
    }

    #endregion

    // 내 레벨에 맞는 난이도에서 우선순위를 전 달에 제작한 게임과 일치하는 컨셉/장르로 설정한다.
    // 상금은 일단 good이 기본
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

        // 조건에 들어가면 concept과 genre둘 다 채워지니 하나만 비교한다.
        if (_gameConcept != "")
        {
            dataList.OrderBy(x =>
            {
                if (x.GameShowConcept[0] == "0")
                {
                    // "모든 컨셉" 을 가진 데이터인 경우
                    // 임의의 컨셉으로 대신 사용하게 되는데, 이것이 현재 컨셉과 일치하면 상위로 정렬한다.
                    // 이는 결국 1/40 확률 검사이기 때문에 다음과 같이 적는다.
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

    // 게임쇼 목록창에 정해진 우선순위로 출력될 수 있게 정렬을 해준다.
    // 신규, 게임난이도 높은순, 상금 많은 순, 가나다 순서
    // 우선 상금의 기본은 good
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

    // 내 아카데미 점수에 따른 게임쇼 난이도 뽑아주는 함수
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

    // 내가 게임쇼에서 받은 평가 반응으로 얻게 될 상금
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

    // 하트 갯수와 레벨로 몇 점인지 계산해주는 함수
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

    // 평가 점수에 따른 평가 반응
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

    // 평가받은 반응을 점수로 환산하는 함수
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

    // 평가 점수에 따른 평가 반응
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

    // 평가 반응에 따른 스크립트 랜덤 출력
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

    // 결과창에 띄워줄 스크립트를 조건에 맞게 랜덤하게 출력
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
