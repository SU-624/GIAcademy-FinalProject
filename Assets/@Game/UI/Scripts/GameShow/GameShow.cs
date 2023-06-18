using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Coffee.UIExtensions;

public enum Assessment
{
    Terrible,       // 최악
    Bad,            // 나쁨
    NotBad,
    SoSo,           // 보통
    Good,           // 좋음
    Great,          // 아주좋음
    Excellent,      // 최상
    None
}

public struct SaveGameShowData
{
    public GameShowData m_TotalGameShowData;
    public int m_GameJamID;
    public string m_GameJamName;
    public Assessment m_GameShowResultAssessment;

    public int m_FunScore;
    public int m_GraphicScore;
    public int m_PerfectionScore;
    public int m_GenreScore;
    public int m_ConceptScore;
    public Assessment m_FunAssessment;
    public Assessment m_GraphicAssessment;
    public Assessment m_PerfectionAssessment;
    public Assessment m_GenreAssessment;
    public Assessment m_ConceptAssessment;

    public Student[] m_ParticipatingStudents;
    public int m_GameShowGenreAndConceptHeart;
    public int m_FunnyHeart;
    public int m_GenreHeart;
    public int m_GraphicHeart;
    public int m_PerceficationHeart;
}

public class GameShow : MonoBehaviour
{
    public delegate void GameShowDataChange();
    public static event GameShowDataChange GameShowDataChangeEvent;

    [SerializeField] private GameShowPanel m_GameShowPanel;
    [SerializeField] private GameShowAnimationPanel m_GameShowAnimationPanel;
    [SerializeField] private RewardPanel m_RewardPanel;
    [SerializeField] private EventPanel m_EventPanel;
    [SerializeField] private GameObject m_GameShowListPrefab;                                                               // 게임쇼 프리팹
    [SerializeField] private GameObject m_PrevGameListPrefab;                                                               // 게임잼에서 만든 게임들의 프리팹
    [SerializeField] private GameObject m_RewardPrefab;
    [SerializeField] private GameObject m_EventPrefab;
    [SerializeField] private TextMeshProUGUI m_NoveltyScript;                                                               // 참신함이 1미만일 때 띄워줄 패널의 텍스트
    [SerializeField] private TextMeshProUGUI m_GoTOGameShowResultPanel;                                                     // 게임쇼의 결과를 보러가기 전에 띄워주는 패널의 텍스트
    [SerializeField] private TextMeshProUGUI m_GameShowResultPanelText;                                                     // 게임쇼의 결과를 보여주기 위해 띄워주는 패널의 텍스트

    [Space(5f)]
    [Header("껐다가 켜줄 패널들의 PopUp, PopOff 오브젝트들")]
    [SerializeField] private PopUpUI m_PopUpResultPanel;                                                                    // 게임쇼 결과창을 보러갈 popUP창
    [SerializeField] private PopOffUI m_PopOffResultPanel;
    [SerializeField] private PopUpUI m_PopUpResultAnimationPanel;                                                           // 게임쇼 결과에 따른 애니메이션을 보여줄 패널
    [SerializeField] private PopOffUI m_PopOffResultAnimationPanel;
    [SerializeField] private PopUpUI m_PopUpNoveltyWarningPanel;                                                            // 참신함이 1미만으로 내려갈 때 띄워줄 패널
    [SerializeField] private PopOffUI m_PopOffNoveltyWarningPanel;
    [SerializeField] private PopUpUI m_PopUpResultScriptPanel;                                                              // 결과창에서 보상을 받기전 보여주는 스크립트 패널
    [SerializeField] private PopOffUI m_PopOffResultScriptPanel;
    [SerializeField] private PopUpUI m_PopUpRewardPanel;                                                                    // 보상 패널
    [SerializeField] private PopOffUI m_PopOffRewardPanel;
    [Space(5f)]

    [Header("튜토리얼용")]
    [SerializeField] private GameObject m_TutorialPanel;
    [SerializeField] private Unmask m_Unmask;
    [SerializeField] private Image m_TutorialTextImage;
    [SerializeField] private Image m_TutorialArrowImage;
    [SerializeField] private TextMeshProUGUI m_TutorialText;
    [SerializeField] private GameObject m_FoldButton;
    [SerializeField] private GameObject m_GameShowButton;
    [SerializeField] private GameObject m_BlackScreen;
    [SerializeField] private GameObject m_PDAlarm;
    [SerializeField] private TextMeshProUGUI m_AlarmText;
    [Space(5f)]

    private List<GameDifficulty> m_Difficulty = new List<GameDifficulty>();                                                 // 내 아카데미 등급에 따른 난이도를 설정하는 리스트
    private List<GameShowRewardByDifficulty> m_Assessnent = new List<GameShowRewardByDifficulty>();                         // 난이도와 평가반응에 따른 보상을 설정하는 리스트
    private List<GameShowScoreByHeart> m_ScoreByHeartList = new List<GameShowScoreByHeart>();
    private List<EvaluationScoreRange> m_ScoreRangeList = new List<EvaluationScoreRange>();                                 // 평가 점수에 따른 반응
    private List<ScoreConversion> m_ScoreConversionList = new List<ScoreConversion>();                                      // 평가 점수로 받은 반응으로 점수 환산
    private List<FinalScoreRange> m_FinalScoreRangeList = new List<FinalScoreRange>();                                      //
    private List<AssessmentResponse> m_AssessmentResponseList = new List<AssessmentResponse>();                             // 각 파트별로 반응에 따라 출력해줄 텍스트들
    private List<ResultScript> m_ResultScriptList = new List<ResultScript>();                                               // 마지막 결과 패널에 띄워줄 스크립트 값들
    private List<string> m_NoveltyScriptList = new List<string>();                                                          // 참신함 점수가 1이 아닐 때 띄워줄 스크립트들
    private List<string> m_GotoGameShowScriptList = new List<string>();                                                     // 게임쇼장을 이동할 때 띄워줄 스크립트들(결과창 이동하는 패널)
    private Dictionary<int, List<GameShowData>> GameShowRandomData = new Dictionary<int, List<GameShowData>>();             // 5가지의 레벨에 따른 데이터를 각각 저장해주기 위한 딕셔너리(추후 레벨이 추가되도 쉽게 바꿀 수 있다.)

    private List<GameShowData> m_GameShowRandomDataPool = new List<GameShowData>();                                         // 매 년 조건에 맞는 달에 난이도에 따라 랜덤으로 데이터를 넣어주는 리스트
    private List<GameShowData> m_SpecialGameShowDataPool = new List<GameShowData>();                                        // 난이도 상관없이 연도만 맞춰 넣어주는 리스트
    private List<GameShowData> m_TotalGameShowData = new List<GameShowData>();                                              // 이번달 공고에 띄워줄 게임쇼들을 모두 모아놓을 리스트
    private List<GameShowData> highleveltotalpool = new List<GameShowData>();                                               // 내 아카데미 등급보다 높은 데이터들을 한번에 모아두고 그 중에 랜덤으로 몇 개만 뽑아야한다.
    private List<GameShowData> lowleveltotalpool = new List<GameShowData>();                                                // 내 아카데미 등급보다 낮은 데이터들을 한번에 모아두고 그 중에 랜덤으로 몇 개만 뽑아야한다.
    private List<GameShowData> m_NowMonthRandomGameShowData = new List<GameShowData>();                                     // 내 아카데미 등급에 맞는 데이터들이 들어있는  리스트

    private List<GameShowData> m_HighLevelRandomPool = new List<GameShowData>();                                            // 내 아카데미 등급보다 높은 등급의 데이터들이 들어있는 리스트
    private List<GameShowData> m_LowLevelRandomPool = new List<GameShowData>();                                             // 내 아카데미 등급보다 낮은 등급의 데이터들이 들어있는 리스트

    private SaveGameShowData m_TemporaryGameShowData = new SaveGameShowData();                                              // 내가 클릭한 게임쇼들의 정보를 임시로 저장해주는 변수
    private List<SaveGameShowData> m_ToBeRunningGameShow = new List<SaveGameShowData>();                                    // 현재 내가 참가하는 게임쇼들을 담아둘 리스트
    private Queue<Action> eventQueue = new Queue<Action>();                                                                     // 게임잼이 실행되고 나면 게임쇼를 참가할 준비를 해줘야한다. timescale이 0일 때 이벤트를 실행하니 안돼서 Update문에서 timescale이 0 보다 클 때 실행시켜주는걸로 변경

    public static Dictionary<int, List<SaveGameShowData>> m_GameShowHistory = new Dictionary<int, List<SaveGameShowData>>();

    //private Student[] m_StudentsParticipated = new Student[3];
    private Queue<string> m_ResultDialogQueue = new Queue<string>();                                                        // 마지막 결과창에 띄워줄 스크립트들을 출력 순서대로 넣어줄 큐
    //private string[] m_LackCondition;

    public Sprite m_ApplySuccessSprite;
    public Sprite m_NotAllowedApplySprite;
    public Sprite m_ApplySprite;
    public Sprite[] m_FinalScoreSprite;
    public Sprite[] m_EmojiSprite;
    public Sprite[] m_StudentFace;
    private GameObject m_PrevClickObj;                                                                                      // 내가 이전에 클릭한 게임잼 버튼을 담아둘 변수

    private int m_NowDay;
    private int m_NowMonth;
    private int m_MonthLimit;
    private int m_GenreAndConceptHeart = 0;
    private int m_RewardMoney;
    private int m_RewardFamous;
    private int m_RewardPassion;
    private int m_RewardHealth;
    private float m_ScrollSpeed = 0.5f;
    private string m_ResultScriptTexet;
    private bool m_IsChangeMonth;
    private bool _isCanParticipate;
    private bool m_IsCheckGameShow;
    private bool m_IsTypingAnmation;
    private Vector3 m_TargetPosition;
    private Vector3 m_StartPosition;

    private void OnEnable()
    {
        SlideMenuPanel.OnGameShowListButtonClicked += HandleGameShowListButtonClicked;
        GameShowPanel.OnGameShowListPanelApplyButtonClicked += HandleGameShowListApplyButton;
        GameShowPanel.OnGameShowListPanelSelectButtonClicked += HandleGameShowListSelectButton;
        InGameUI.OnGameShowRsultButtonClicked += HandleGameShowResultButton;
        GameJam.DataChangedEvent += TakeOutGameShowData;
    }

    private void OnDisable()
    {
        SlideMenuPanel.OnGameShowListButtonClicked -= HandleGameShowListButtonClicked;
        GameShowPanel.OnGameShowListPanelApplyButtonClicked -= HandleGameShowListApplyButton;
        GameShowPanel.OnGameShowListPanelSelectButtonClicked -= HandleGameShowListSelectButton;
        InGameUI.OnGameShowRsultButtonClicked -= HandleGameShowResultButton;
        GameJam.DataChangedEvent -= TakeOutGameShowData;
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
        m_MonthLimit = 2;
        m_GameShowPanel.InitGameShowPanel();
        m_GameShowHistory.Clear();
    }

    private void Update()
    {
        if (eventQueue.Count > 0 && Time.timeScale > 0)
        {
            eventQueue.Dequeue()?.Invoke();
        }

        if (m_NowDay != GameTime.Instance.FlowTime.NowDay)
        {
            if (m_EventPanel.gameObject.activeSelf)
            {
                for (int i = 0; i < m_EventPanel.EventTransform.childCount; i++)
                {
                    string DdayText = m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().DDAyText.text;

                    int Dday = int.Parse(DdayText[2..]) - 1;

                    m_EventPanel.EventTransform.GetChild(i).GetComponent<EventPrefab>().DDAyText.text = "D-" + Dday.ToString();

                    if (Dday <= 0)
                    {
                        Dday = 0;
                        m_IsTypingAnmation = false;
                        Destroy(m_EventPanel.EventTransform.GetChild(i).gameObject, 0.5f);

                    }
                }

                if (m_EventPanel.EventTransform.childCount == 0)
                {
                    m_EventPanel.gameObject.SetActive(false);
                }
            }

            m_NowDay = GameTime.Instance.FlowTime.NowDay;
        }

        if (m_NowMonth != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 3)
        {
            m_GameShowRandomDataPool.Clear();
            m_SpecialGameShowDataPool.Clear();
            m_GameShowPanel.DestroyObj(m_GameShowPanel.GameShowListContentObj);
            m_NowMonth = GameTime.Instance.FlowTime.NowMonth;
            m_MonthLimit = 2;
            m_IsChangeMonth = true;
        }

        if (m_ToBeRunningGameShow.Count != 0)
        {
            for (int i = 0; i < m_ToBeRunningGameShow.Count; i++)
            {
                if (m_ToBeRunningGameShow[i].m_TotalGameShowData.GameShow_Month == GameTime.Instance.FlowTime.NowMonth
                    && m_ToBeRunningGameShow[i].m_TotalGameShowData.GameShow_Week == GameTime.Instance.FlowTime.NowWeek
                    && m_ToBeRunningGameShow[i].m_TotalGameShowData.GameShow_Day == GameTime.Instance.FlowTime.NowDay
                    && !m_IsCheckGameShow)
                {
                    m_TemporaryGameShowData = m_ToBeRunningGameShow[i];
                    GetGameShowResultPanelScript();
                }
            }
        }

        if (m_IsChangeMonth)
        {
            for (int i = 0; i < AllOriginalJsonData.Instance.OriginalRandomGameShowData.Count; i++)
            {
                if (GameTime.Instance.FlowTime.NowMonth + 1 == AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month &&
                    !AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_GameShowRandomDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
                // 매년 발생해야함
                else if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Year == 0 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month == GameTime.Instance.FlowTime.NowMonth + 1 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_SpecialGameShowDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
                // 홀수년도 발생
                else if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Year == -1 && (10 % GameTime.Instance.FlowTime.NowYear) != 0 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month == GameTime.Instance.FlowTime.NowMonth + 1 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_SpecialGameShowDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
                // 짝수년도 발생
                else if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Year == -2 && (10 % GameTime.Instance.FlowTime.NowYear) == 0 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month == GameTime.Instance.FlowTime.NowMonth + 1 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_SpecialGameShowDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
            }

            m_IsChangeMonth = false;

            if (GameJam.m_GameJamHistory.Count != 0 && m_GameShowPanel.GameShowListContent.childCount == 0)
            {
                TakeOutGameShowData();
                Debug.Log("여기서 게임쇼 켜주나?");
            }
        }
    }

    public void EnqueueDataChangedEvent()
    {
        eventQueue.Enqueue(() => GameShowDataChangeEvent?.Invoke());
    }

    // 게임쇼 버튼을 누르면 실행시켜줄 함수
    public void MakeGameShowList()
    {
        CombineGameShowDataList();

        if (m_TotalGameShowData.Count == 0)
        {
            m_GameShowPanel.SetNoGameShow(true);
        }
        else if (m_MonthLimit > 0)
        {
            for (int i = 0; i < m_TotalGameShowData.Count; i++)
            {
                GameObject _gameShowList = Instantiate(m_GameShowListPrefab, m_GameShowPanel.GameShowListContent);

                _gameShowList.name = m_TotalGameShowData[i].GameShow_Name;
                _gameShowList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_TotalGameShowData[i].GameShow_Name;

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
        }
        else
        {
            for (int i = 0; i < m_TotalGameShowData.Count; i++)
            {
                GameObject _gameShowList = Instantiate(m_GameShowListPrefab, m_GameShowPanel.GameShowListContent);

                _gameShowList.name = m_TotalGameShowData[i].GameShow_Name;
                _gameShowList.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_TotalGameShowData[i].GameShow_Name;

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
        }
    }

    // 보상 패널에서 받기 버튼을 눌렀을 때 
    public void ClickCloseRewardPanel()
    {
        m_PopOffRewardPanel.TurnOffUI();

        PlayerInfo.Instance.m_MyMoney += m_RewardMoney;
        PlayerInfo.Instance.m_Awareness += m_RewardFamous;


        for (int i = 0; i < m_TemporaryGameShowData.m_ParticipatingStudents.Length; i++)
        {
            m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Health += m_RewardHealth;
            m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Passion += m_RewardPassion;
        }
    }

    #region _delegate로 등록할 함수들
    // 게임쇼를 켜주는 버튼이 클릭되었을 때 실행할 함수들.
    private void HandleGameShowListButtonClicked()
    {
        MakeGameShowList();
        //m_GameShowPanel.ApplyButton
        m_GameShowPanel.SetActiveGameShowPanel(true);
    }

    // 신청 버튼이 클릭되었을 때 실행 할 함수.
    private void HandleGameShowListApplyButton()
    {
        if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_ApplySprite && m_MonthLimit > 0)
        {
            //// 이때 신청 완료하고 신청버튼 신청완료로 변경
            if (_isCanParticipate)
            {
                ApplySuccess();
            }
            // 아니라면 신청할 수 없다고 경고 메세지 띄워주기
            else
            {
                ApplyFailed();
            }
        }
        // 참여횟수 모두 사용 
        else if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_NotAllowedApplySprite && m_MonthLimit <= 0)
        {
            // 버튼 클릭시 참가할 수 있는 횟수를 초과했다는 메세지 띄워주기
            m_GameShowPanel.SetConditionFailPanel("참가할 수 있는 횟수를 초과하였습니다!\n다음 달을 노려보세요.");
        }
        // else : 신청했던 버튼은 눌러도 따로 뭐 없음.
    }

    // 선택 버튼이 클릭되었을 때 실행 할 함수.
    private void HandleGameShowListSelectButton()
    {
        GetPrevGameList();
        m_GameShowPanel.ClickSelectButton();
    }

    private void HandleGameShowResultButton()
    {
        m_PopOffResultPanel.TurnOffUI();
        CalculateScore();
        TurnAnimationByScore(m_TemporaryGameShowData.m_GameShowResultAssessment);
    }
    #endregion

    // 점수계산하는 함수
    private void CalculateScore()
    {
        int _standardGenreAndConcept = GetScoreByHeart(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_GameShowGenreAndConceptHeart);
        int _standardGenre = GetScoreByHeart(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_GenreHeart);
        int _standardFun = GetScoreByHeart(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_FunnyHeart);
        int _standardGraphic = GetScoreByHeart(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_GraphicHeart);
        int _standardPercefication = GetScoreByHeart(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_PerceficationHeart);

        int _selectGameJamIndex = GameJam.m_GameJamHistory[m_TemporaryGameShowData.m_GameJamID].FindIndex(x => x.m_GameJamData.m_GameName == m_TemporaryGameShowData.m_GameJamName);
        int _myGenre = (int)GameJam.m_GameJamHistory[m_TemporaryGameShowData.m_GameJamID][_selectGameJamIndex].m_GameJamData.m_TotalGenreScore;
        int _myFunny = (int)GameJam.m_GameJamHistory[m_TemporaryGameShowData.m_GameJamID][_selectGameJamIndex].m_GameJamData.m_Funny;
        int _myGraphic = (int)GameJam.m_GameJamHistory[m_TemporaryGameShowData.m_GameJamID][_selectGameJamIndex].m_GameJamData.m_Graphic;
        int _myPerfecation = (int)GameJam.m_GameJamHistory[m_TemporaryGameShowData.m_GameJamID][_selectGameJamIndex].m_GameJamData.m_Perfection;
        float _novelty = 1f;

        if (m_GameShowHistory.Count != 0 && m_GameShowHistory.ContainsKey(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID))
        {
            List<SaveGameShowData> _temporary = new List<SaveGameShowData>();

            m_GameShowHistory.TryGetValue(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID, out _temporary);

            for (int i = 0; i < _temporary.Count; i++)
            {
                if ((int)_temporary[i].m_GameShowResultAssessment >= 4)
                {
                    _novelty *= 0.2f;
                }
            }

            if (_novelty < 0.1f)
            {
                _novelty = 0.1f;
            }

            m_PopUpNoveltyWarningPanel.TurnOnUI();
            int _index = UnityEngine.Random.Range(0, m_NoveltyScriptList.Count);
            m_NoveltyScript.text = m_NoveltyScriptList[_index];
        }

        Assessment _genre = GetGameShow((int)Math.Floor(_myGenre * _novelty) - _standardGenre);
        int _genreScore = GetScoreConversion(_genre);

        Assessment _funny = GetGameShow((int)Math.Floor(_myFunny * _novelty) - _standardFun);
        int _funnyScore = GetScoreConversion(_funny);

        Assessment _graphic = GetGameShow((int)Math.Floor(_myGraphic * _novelty) - _standardGraphic);
        int _graphicScore = GetScoreConversion(_graphic);

        Assessment _perfecation = GetGameShow((int)Math.Floor(_myPerfecation * _novelty) - _standardPercefication);
        int _perfecationScore = GetScoreConversion(_perfecation);

        Assessment _genreAndConcept = GetGameShow((int)Math.Floor(_standardGenreAndConcept * _novelty) - _standardGenreAndConcept);
        int _genreAndConceptScore = GetScoreConversion(_genreAndConcept);

        int _totalSocre = _genreScore + _funnyScore + _graphicScore + _perfecationScore + _genreAndConceptScore;

        m_TemporaryGameShowData.m_FunScore = _funnyScore;
        m_TemporaryGameShowData.m_FunAssessment = _funny;

        m_TemporaryGameShowData.m_GenreScore = _genreScore;
        m_TemporaryGameShowData.m_GenreAssessment = _genre;

        m_TemporaryGameShowData.m_PerfectionScore = _perfecationScore;
        m_TemporaryGameShowData.m_PerfectionAssessment = _perfecation;

        m_TemporaryGameShowData.m_GraphicScore = _graphicScore;
        m_TemporaryGameShowData.m_GraphicAssessment = _graphic;

        m_TemporaryGameShowData.m_ConceptScore = _genreAndConceptScore;
        m_TemporaryGameShowData.m_ConceptAssessment = _genreAndConcept;

        m_TemporaryGameShowData.m_GameShowResultAssessment = FinalGameShowScore(_totalSocre);

        // 게임쇼가 끝나고 점수까지 저장할 때 필요
        if (m_GameShowHistory.ContainsKey(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID))
        {
            m_GameShowHistory[m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID].Add(m_TemporaryGameShowData);
        }
        else
        {
            List<SaveGameShowData> m_GameshowDataList = new List<SaveGameShowData>();
            m_GameshowDataList.Add(m_TemporaryGameShowData);
            m_GameShowHistory.Add(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID, m_GameshowDataList);
        }
    }

    // 받은 점수에 따라 재생할 애니메이션을 다르게 해줄 함수
    private void TurnAnimationByScore(Assessment _gameShowFinalAssessment)
    {
        m_PopUpResultAnimationPanel.TurnOnUI();

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
            m_GameShowAnimationPanel.ChangeSpriteAnimation(m_FinalScoreSprite[2]);
        }

        StartCoroutine(ChangeAnimationPanel());
    }

    // 결과창으로 이동하기 위해 띄워주는 패널에 쓰여질 스크립트 가져오기
    private void GetGameShowResultPanelScript()
    {
        int _index = UnityEngine.Random.Range(0, m_GotoGameShowScriptList.Count);

        m_GoTOGameShowResultPanel.text = m_GotoGameShowScriptList[_index];

        m_PopUpResultPanel.TurnOnUI();
        m_IsCheckGameShow = true;
    }

    // BGM재생 시간만큼 기다렸다가 패널 변경해주기
    IEnumerator ChangeAnimationPanel()
    {
        yield return new WaitForSecondsRealtime(5f);

        m_GameShowAnimationPanel.SetActiveAnimationPanel();

        StartCoroutine(SetSatisfactionStatusPanel());
    }

    // 게임 만족도 현황을 보여주기 위한 패널에 스크립트와 이미지를 넣어주는 함수
    private IEnumerator SetSatisfactionStatusPanel()
    {
        m_GameShowAnimationPanel.SetAcademyName(PlayerInfo.Instance.m_AcademyName);

        m_GameShowAnimationPanel.ChangeEvalutionResponseScore(m_TemporaryGameShowData.m_FunScore.ToString());
        FindEmojiAndScript(m_TemporaryGameShowData.m_FunAssessment, "Fun");

        yield return new WaitForSecondsRealtime(3f);

        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((m_TemporaryGameShowData.m_FunScore + m_TemporaryGameShowData.m_GraphicScore).ToString());
        FindEmojiAndScript(m_TemporaryGameShowData.m_GraphicAssessment, "Graphic");

        yield return new WaitForSecondsRealtime(3f);

        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((m_TemporaryGameShowData.m_FunScore + m_TemporaryGameShowData.m_GraphicScore + m_TemporaryGameShowData.m_PerfectionScore).ToString());
        FindEmojiAndScript(m_TemporaryGameShowData.m_PerfectionAssessment, "Perfection");

        yield return new WaitForSecondsRealtime(3f);

        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((m_TemporaryGameShowData.m_FunScore + m_TemporaryGameShowData.m_GraphicScore + m_TemporaryGameShowData.m_PerfectionScore + m_TemporaryGameShowData.m_GenreScore).ToString());
        FindEmojiAndScript(m_TemporaryGameShowData.m_GenreAssessment, "Genre");

        yield return new WaitForSecondsRealtime(3f);

        m_GameShowAnimationPanel.ChangeEvalutionResponseScore((m_TemporaryGameShowData.m_FunScore + m_TemporaryGameShowData.m_GraphicScore + m_TemporaryGameShowData.m_PerfectionScore + m_TemporaryGameShowData.m_GenreScore + m_TemporaryGameShowData.m_ConceptScore).ToString());
        FindEmojiAndScript(m_TemporaryGameShowData.m_ConceptAssessment, "Concept");

        yield return new WaitForSecondsRealtime(2f);

        SetStudentFaceSprite(m_TemporaryGameShowData.m_GameShowResultAssessment);

        yield return new WaitForSecondsRealtime(3f);

        m_PopOffResultAnimationPanel.TurnOffUI();

        m_PopUpResultScriptPanel.TurnOnUI();

        FindResultScript(m_TemporaryGameShowData.m_GameShowResultAssessment, m_TemporaryGameShowData.m_FunScore, m_TemporaryGameShowData.m_GraphicScore, m_TemporaryGameShowData.m_PerfectionScore);

        yield return StartCoroutine(ResultPanelScript());

        m_PopOffResultScriptPanel.TurnOffUI();

        m_PopUpRewardPanel.TurnOnUI();

        // 보상에 따라 프리팹 생성해주는거 추가하기
        m_StartPosition = m_RewardPanel.m_ContentsAnchor.position;
        m_TargetPosition = m_StartPosition + Vector3.right * m_RewardPanel.m_ContentsAnchor.rect.width;

        MakeResultReward();

        if (m_RewardPanel.m_ContentsTransform.childCount > 3)
        {
            m_RewardPanel.SetContentAnchor(false);

            yield return StartCoroutine(ResultPanelScrollView());
        }
        else
        {
            m_RewardPanel.SetContentAnchor(true);
        }
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
            m_RewardPanel.m_ContentsAnchor.Translate(Vector3.right * m_ScrollSpeed * direction * Time.unscaledDeltaTime);

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
        m_ResultDialogQueue.Enqueue("시연회 반응으로는 \"" + _mainText1 + "\"," + "\"" + _mainText2 + "\"가(이) 있네요.");

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
                m_GameShowAnimationPanel.ChangeStudentFaceImage(m_StudentFace[0], m_StudentFace[1], m_StudentFace[2]);
            }
            break;

            case Assessment.NotBad:
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
    private void MakeResultReward()
    {
        if (m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Plusreward != 0)
        {
            // 추가 보상이 있을 때 어떻게 할 지 고민하기
        }
        else
        {
            var (reward, famous, passion, health) = GetRewardByAssessment(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Level, m_TemporaryGameShowData.m_GameShowResultAssessment);

            m_RewardMoney = reward;
            m_RewardFamous = famous;
            m_RewardPassion = passion;
            m_RewardHealth = health;

            if (m_RewardMoney != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardMoney";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "상금";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardMoney.ToString();
            }

            if (m_RewardFamous != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardFamous";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "인지도";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardFamous.ToString();
            }

            if (m_RewardPassion != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardPassion";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "열정";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardPassion.ToString();
            }

            if (m_RewardHealth != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardHealth";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "체력";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardHealth.ToString();
            }
        }

        int _index = m_ToBeRunningGameShow.FindIndex(x => x.m_TotalGameShowData.GameShow_ID == m_TemporaryGameShowData.m_TotalGameShowData.GameShow_ID);
        m_ToBeRunningGameShow.RemoveAt(_index);
    }

    private GameObject MakeEventPrefab(string _gameShowName, int _gameShowMonth, int _gameShowWeek, int _gameShowDay)
    {
        GameObject _eventPrefab = Instantiate(m_EventPrefab, m_EventPanel.EventTransform);

        _eventPrefab.name = _gameShowName;

        // 합산된 D-Day 값
        int totalDDay = ((GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5) + ((_gameShowMonth - 1) * 4 * 5) + ((_gameShowWeek - 1) * 5) + _gameShowDay;
        int today = (GameTime.Instance.FlowTime.NowYear - 1) * 12 * 4 * 5 + ((GameTime.Instance.FlowTime.NowMonth - 1) * 4 * 5) +
            ((GameTime.Instance.FlowTime.NowWeek - 1) * 5) + GameTime.Instance.FlowTime.NowDay;

        totalDDay -= today;

        _eventPrefab.GetComponent<EventPrefab>().DDAyText.text = "D-" + totalDDay.ToString();

        return _eventPrefab;
        //StartCoroutine(DDayTimer());
    }

    public void ClickEventButton()
    {
        if (m_EventPanel.ScorllViewObj.activeSelf)
        {
            m_EventPanel.ScorllViewObj.SetActive(false);
        }
        else
        {
            m_EventPanel.ScorllViewObj.SetActive(true);
        }
    }

    // 게임쇼 패널을 닫을 때 마다 해줄 데이터 초기화
    public void InitData()
    {
        m_TemporaryGameShowData.m_TotalGameShowData = null;
        m_TemporaryGameShowData.m_ParticipatingStudents = null;
        m_TemporaryGameShowData.m_GameJamID = 0;
        m_TemporaryGameShowData.m_FunnyHeart = 0;
        m_TemporaryGameShowData.m_GenreHeart = 0;
        m_TemporaryGameShowData.m_GraphicHeart = 0;
        m_TemporaryGameShowData.m_PerceficationHeart = 0;
        m_TemporaryGameShowData.m_GameShowGenreAndConceptHeart = 0;
        m_TemporaryGameShowData.m_GameShowResultAssessment = Assessment.None;
    }

    // 게임선택 버튼을 눌렀을 때 이전에 만든 게임 목록을 만들어줄 함수
    private void GetPrevGameList()
    {
        m_GameShowPanel.DestroyObj(m_GameShowPanel.PrevGameListContentObj);

        if (GameJam.m_GameJamHistory.Count == 0)
        {
            m_GameShowPanel.SetActiveIsNotPrevGameList(true);
        }
        // 게임쇼에 내보낸 게임은 선택 못하게 하고 맨 위로 보내줘야한다.
        else if (m_ToBeRunningGameShow.Count != 0)
        {
            Dictionary<int, List<SaveGameJamData>> _gameJam = new Dictionary<int, List<SaveGameJamData>>(GameJam.m_GameJamHistory);

            for (int i = 0; i < m_ToBeRunningGameShow.Count; i++)
            {
                List<SaveGameJamData> gamejamdata1 = new List<SaveGameJamData>(_gameJam[m_ToBeRunningGameShow[i].m_GameJamID]);
                MakeSelectPrevGameShowList(gamejamdata1);

                for (int j = 0; j < GameJam.m_GameJamHistory.Count; j++)
                {
                    for (int k = 0; k < GameJam.m_GameJamHistory.ElementAt(j).Value.Count; k++)
                    {
                        if (m_ToBeRunningGameShow[i].m_GameJamID != GameJam.m_GameJamHistory.ElementAt(j).Key)
                        {
                            MakePrevGameList(j, k);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
            {
                for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
                {
                    MakePrevGameList(i, j);
                }
            }
        }
    }

    // 게임잼에서 만든 게임들을 만들 함수
    private void MakePrevGameList(int _gameJamHistoryIndex, int _gameJamHistoryValueIndex)
    {
        GameObject _prevGameList = Instantiate(m_PrevGameListPrefab, m_GameShowPanel.PrevGameJamListContent);

        string _gameName = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_GameName;
        string _gameRank = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_Rank;
        string _gameConcept = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamInfoData.m_GjamConcept;
        string _gameGenre = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_Genre;
        string _gameFunny = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_Funny.ToString();
        string _gameGraphic = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_Funny.ToString();
        string _gamePerfection = GameJam.m_GameJamHistory.ElementAt(_gameJamHistoryIndex).Value[_gameJamHistoryValueIndex].m_GameJamData.m_Funny.ToString();

        _prevGameList.name = _gameName;
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
    private void MakeSelectPrevGameShowList(List<SaveGameJamData> _data)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            GameObject _prevGameList = Instantiate(m_PrevGameListPrefab, m_GameShowPanel.PrevGameJamListContent);

            string _gameName = _data[i].m_GameJamData.m_GameName;
            string _gameRank = _data[i].m_GameJamData.m_Rank;
            string _gameConcept = _data[i].m_GameJamInfoData.m_GjamConcept;
            string _gameGenre = _data[i].m_GameJamData.m_Genre;
            string _gameFunny = _data[i].m_GameJamData.m_Funny.ToString();
            string _gameGraphic = _data[i].m_GameJamData.m_Funny.ToString();
            string _gamePerfection = _data[i].m_GameJamData.m_Funny.ToString();

            _prevGameList.name = _gameName;
            _prevGameList.GetComponent<PrevGamePrefab>().m_GameName.text = _gameName;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Grade.text = _gameRank;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Concept.text = _gameConcept;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Genre.text = _gameGenre;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Funny.text = _gameFunny;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Graphic.text = _gameGraphic;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Perfection.text = _gamePerfection;
            _prevGameList.GetComponent<PrevGamePrefab>().m_Glow.SetActive(true);
            _prevGameList.GetComponent<PrevGamePrefab>().m_PrefabButton.interactable = false;
            //_prevGameList.GetComponent<PrevGamePrefab>().m_PrefabButton.onClick.AddListener(ClickPrevGameButton);
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
            GetPrevGameList();
        }
        else
        {
            if (m_ToBeRunningGameShow.Count != 0)
            {
                Dictionary<int, List<SaveGameJamData>> _gameJam = new Dictionary<int, List<SaveGameJamData>>(GameJam.m_GameJamHistory);

                for (int i = 0; i < m_ToBeRunningGameShow.Count; i++)
                {
                    for (int j = 0; j < GameJam.m_GameJamHistory.Count; j++)
                    {
                        for (int k = 0; k < GameJam.m_GameJamHistory.ElementAt(j).Value.Count; k++)
                        {
                            if (GameJam.m_GameJamHistory.ElementAt(j).Value[k].m_GameJamData.m_Rank == _rankName)
                            {
                                if (m_ToBeRunningGameShow[i].m_GameJamID != GameJam.m_GameJamHistory.ElementAt(j).Key)
                                {
                                    MakePrevGameList(j, k);
                                }
                                else
                                {
                                    List<SaveGameJamData> gamejamdata1 = new List<SaveGameJamData>(_gameJam[m_ToBeRunningGameShow[i].m_GameJamID]);
                                    MakeSelectPrevGameShowList(gamejamdata1);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
                {
                    for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
                    {
                        if (GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Rank == _rankName)
                        {
                            MakePrevGameList(i, j);
                        }
                    }
                }
            }
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

        m_GameShowPanel.ApplyButton.interactable = true;
        m_PrevClickObj = _currentObj;
    }

    private void ApplySuccess()
    {
        // 내가 이미 신청한 게임잼이 들어있는데 누른거라면 넘겨주기
        int index = m_ToBeRunningGameShow.FindIndex(x => x.m_TotalGameShowData.GameShow_Name == m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Name);

        if (m_MonthLimit > 0 && index == -1)
        {
            m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySuccessSprite;

            m_TemporaryGameShowData.m_GameShowGenreAndConceptHeart = m_GenreAndConceptHeart;
            m_TemporaryGameShowData.m_FunnyHeart = m_TemporaryGameShowData.m_TotalGameShowData.GameShow_State["Fun"];
            m_TemporaryGameShowData.m_PerceficationHeart = m_TemporaryGameShowData.m_TotalGameShowData.GameShow_State["Perfection"];
            m_TemporaryGameShowData.m_GraphicHeart = m_TemporaryGameShowData.m_TotalGameShowData.GameShow_State["Graphic"];
            m_TemporaryGameShowData.m_GenreHeart = m_TemporaryGameShowData.m_TotalGameShowData.GameShow_State["Genre"];

            m_ToBeRunningGameShow.Add(m_TemporaryGameShowData);

            m_MonthLimit -= 1;

            GameObject _eventPrefab = MakeEventPrefab(m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Name, m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Month,
                 m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Week, m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Day);

            m_EventPanel.gameObject.SetActive(true);
            m_IsTypingAnmation = true;
            m_PrevClickObj.GetComponent<PrevGamePrefab>().m_Glow.SetActive(true);
            StartCoroutine(TypingText(_eventPrefab));
        }
        else if (m_MonthLimit <= 0)
        {
            // 참여횟수 끝~
            m_GameShowPanel.SetConditionFailPanel("이번달 참여는 끝~");
        }
    }

    private void ApplyFailed()
    {
        string[] _text = new string[3];

        for (int i = 0; i < 3; i++)
        {
            if (m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Passion < m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Pasion)
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "학생의 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
            }
            else if (m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Health < m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Health)
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "학생의 체력이 부족하여 게임쇼에 나갈 수 없습니다.";
            }
            else
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "학생의 체력과 열정이 부족하여 게임쇼에 나갈 수 없습니다.";
            }

        }

        m_GameShowPanel.SetConditionFailPanel(_text[0] + "\n" + _text[1] + "\n" + _text[2] + "\n");
    }

    private IEnumerator TypingText(GameObject _eventPrrerfab)
    {
        string _originalText;
        string _subString = "";

        _originalText = "...";

        while (m_IsTypingAnmation)
        {
            for (int i = 0; i < _originalText.Length; i++)
            {
                _subString += _originalText.Substring(0, i);

                for (int j = 0; j < m_EventPanel.EventTransform.childCount; j++)
                {
                    _eventPrrerfab.GetComponent<EventPrefab>().ActivityText.text = "게임쇼 준비중" + _subString;
                    _subString = "";
                }
                //m_EventPanel.EventActivity.text = "게임쇼 준비중" + _subString;

                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private void SetGenreAndConceptHeart(int _gameJamIndex, int _gameJamListIndex)
    {
        if (m_TemporaryGameShowData.m_TotalGameShowData.GameShowConcept.Any(c => c == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamInfoData.m_GjamConcept) &&
            m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre.Any(g => g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamData.m_Genre)
            || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "모든 장르" && m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "모든 컨셉")
        {
            m_GenreAndConceptHeart = 3;
        }
        else if (m_TemporaryGameShowData.m_TotalGameShowData.GameShowConcept.Any(c => c == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamInfoData.m_GjamConcept) ||
                 m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre.Any(g => g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamData.m_Genre)
                 || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "모든 장르" || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "모든 컨셉")
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
        int _index = 0;

        m_TemporaryGameShowData.m_ParticipatingStudents = new Student[3];

        List<Student> _student = new List<Student>(ObjectManager.Instance.m_StudentList);

        for (int i = 0; i < GameJam.m_GameJamHistory.Count; i++)
        {
            for (int j = 0; j < GameJam.m_GameJamHistory.ElementAt(i).Value.Count; j++)
            {
                if (_gameName == GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName)
                {
                    SetGenreAndConceptHeart(i, j);
                    //m_StudentsParticipated[0] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GM[0];
                    //m_StudentsParticipated[1] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Art[0];
                    //m_StudentsParticipated[2] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Programming[0];
                    m_TemporaryGameShowData.m_ParticipatingStudents[0] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GM[0];
                    m_TemporaryGameShowData.m_ParticipatingStudents[1] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Art[0];
                    m_TemporaryGameShowData.m_ParticipatingStudents[2] = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_Programming[0];
                    m_TemporaryGameShowData.m_GameJamID = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamInfoData.m_GjamID;
                    m_TemporaryGameShowData.m_GameJamName = GameJam.m_GameJamHistory.ElementAt(i).Value[j].m_GameJamData.m_GameName;
                    break;
                }
            }
        }

        for (int i = 0; i < _student.Count; i++)
        {
            if (_student[i].m_StudentStat.m_StudentName == m_TemporaryGameShowData.m_ParticipatingStudents[_index].m_StudentStat.m_StudentName)
            {
                if (_student[i].m_StudentStat.m_Health >= m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Health &&
                    _student[i].m_StudentStat.m_Passion >= m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Pasion)
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
        GameObject _clickButton = EventSystem.current.currentSelectedGameObject;
        string _buttonName = _clickButton.name;
        m_GameShowPanel.InitGameShowPanel();

        if (m_ToBeRunningGameShow.Count != 0)
        {
            int _index = m_ToBeRunningGameShow.FindIndex(x => x.m_TotalGameShowData.GameShow_Name == _buttonName);

            if (-1 != _index)
            {
                m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySuccessSprite;

                for (int k = 0; k < m_TotalGameShowData.Count; k++)
                {
                    if (m_TotalGameShowData[k].GameShow_Name == _buttonName)
                    {
                        string _gameShowHostCompany = m_TotalGameShowData[k].GameShow_Host_Name;
                        string _gameShowName = m_TotalGameShowData[k].GameShow_Name;
                        string _gameShowLevel = m_TotalGameShowData[k].GameShow_Level.ToString();
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "월 " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "주";
                        string _gameShowJudegeMent = m_TotalGameShowData[k].GameShow_Judges_Name;
                        string _gameShowReward = m_TotalGameShowData[k].GameShow_Reward.ToString();
                        string _gameShowHostHelath = m_TotalGameShowData[k].GameShow_Health.ToString();
                        string _gameShowHostPassion = m_TotalGameShowData[k].GameShow_Pasion.ToString();
                        int _gameShowFunny = m_TotalGameShowData[k].GameShow_State["Fun"];
                        int _gameShowGraphic = m_TotalGameShowData[k].GameShow_State["Graphic"];
                        int _gameShowPerfection = m_TotalGameShowData[k].GameShow_State["Perfection"];
                        int _gameShowGenre = m_TotalGameShowData[k].GameShow_State["Genre"];

                        m_GameShowPanel.SetGameShowListText(_gameShowHostCompany, _gameShowName, _gameShowLevel, _gameShowProgressDate, _gameShowJudegeMent, _gameShowReward, _gameShowHostHelath, _gameShowHostPassion);
                        m_GameShowPanel.SetFunny(_gameShowFunny, true);
                        m_GameShowPanel.SetGraphic(_gameShowGraphic, true);
                        m_GameShowPanel.SetPerfection(_gameShowPerfection, true);
                        m_GameShowPanel.SetGenre(_gameShowGenre, true);

                        m_TemporaryGameShowData.m_TotalGameShowData = m_TotalGameShowData[k];
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
                        string _gameShowHostCompany = m_TotalGameShowData[k].GameShow_Host_Name;
                        string _gameShowName = m_TotalGameShowData[k].GameShow_Name;
                        string _gameShowLevel = m_TotalGameShowData[k].GameShow_Level.ToString();
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "월 " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "주";
                        string _gameShowJudegeMent = m_TotalGameShowData[k].GameShow_Judges_Name;
                        string _gameShowReward = m_TotalGameShowData[k].GameShow_Reward.ToString();
                        string _gameShowHostHelath = m_TotalGameShowData[k].GameShow_Health.ToString();
                        string _gameShowHostPassion = m_TotalGameShowData[k].GameShow_Pasion.ToString();
                        int _gameShowFunny = m_TotalGameShowData[k].GameShow_State["Fun"];
                        int _gameShowGraphic = m_TotalGameShowData[k].GameShow_State["Graphic"];
                        int _gameShowPerfection = m_TotalGameShowData[k].GameShow_State["Perfection"];
                        int _gameShowGenre = m_TotalGameShowData[k].GameShow_State["Genre"];

                        m_GameShowPanel.SetGameShowListText(_gameShowHostCompany, _gameShowName, _gameShowLevel, _gameShowProgressDate, _gameShowJudegeMent, _gameShowReward, _gameShowHostHelath, _gameShowHostPassion);
                        m_GameShowPanel.SetFunny(_gameShowFunny, true);
                        m_GameShowPanel.SetGraphic(_gameShowGraphic, true);
                        m_GameShowPanel.SetPerfection(_gameShowPerfection, true);
                        m_GameShowPanel.SetGenre(_gameShowGenre, true);

                        m_TemporaryGameShowData.m_TotalGameShowData = m_TotalGameShowData[k];
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
                        string _gameShowHostCompany = m_TotalGameShowData[k].GameShow_Host_Name;
                        string _gameShowName = m_TotalGameShowData[k].GameShow_Name;
                        string _gameShowLevel = m_TotalGameShowData[k].GameShow_Level.ToString();
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "월 " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "주";
                        string _gameShowJudegeMent = m_TotalGameShowData[k].GameShow_Judges_Name;
                        string _gameShowReward = m_TotalGameShowData[k].GameShow_Reward.ToString();
                        string _gameShowHostHelath = m_TotalGameShowData[k].GameShow_Health.ToString();
                        string _gameShowHostPassion = m_TotalGameShowData[k].GameShow_Pasion.ToString();
                        int _gameShowFunny = m_TotalGameShowData[k].GameShow_State["Fun"];
                        int _gameShowGraphic = m_TotalGameShowData[k].GameShow_State["Graphic"];
                        int _gameShowPerfection = m_TotalGameShowData[k].GameShow_State["Perfection"];
                        int _gameShowGenre = m_TotalGameShowData[k].GameShow_State["Genre"];

                        m_GameShowPanel.SetGameShowListText(_gameShowHostCompany, _gameShowName, _gameShowLevel, _gameShowProgressDate, _gameShowJudegeMent, _gameShowReward, _gameShowHostHelath, _gameShowHostPassion);
                        m_GameShowPanel.SetFunny(_gameShowFunny, true);
                        m_GameShowPanel.SetGraphic(_gameShowGraphic, true);
                        m_GameShowPanel.SetPerfection(_gameShowPerfection, true);
                        m_GameShowPanel.SetGenre(_gameShowGenre, true);

                        m_TemporaryGameShowData.m_TotalGameShowData = m_TotalGameShowData[k];
                        break;
                    }
                }
            }
        }
        else
        {
            m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite = m_ApplySprite;

            for (int i = 0; i < m_TotalGameShowData.Count; i++)
            {
                if (m_TotalGameShowData[i].GameShow_Name == _buttonName)
                {
                    string _gameShowHostCompany = m_TotalGameShowData[i].GameShow_Host_Name;
                    string _gameShowName = m_TotalGameShowData[i].GameShow_Name;
                    string _gameShowLevel = m_TotalGameShowData[i].GameShow_Level.ToString();
                    string _gameShowProgressDate = (m_TotalGameShowData[i].GameShow_Month.ToString()) + "월 " + (m_TotalGameShowData[i].GameShow_Week.ToString()) + "주";
                    string _gameShowJudegeMent = m_TotalGameShowData[i].GameShow_Judges_Name;
                    string _gameShowReward = m_TotalGameShowData[i].GameShow_Reward.ToString();
                    string _gameShowHostHelath = m_TotalGameShowData[i].GameShow_Health.ToString();
                    string _gameShowHostPassion = m_TotalGameShowData[i].GameShow_Pasion.ToString();
                    int _gameShowFunny = m_TotalGameShowData[i].GameShow_State["Fun"];
                    int _gameShowGraphic = m_TotalGameShowData[i].GameShow_State["Graphic"];
                    int _gameShowPerfection = m_TotalGameShowData[i].GameShow_State["Perfection"];
                    int _gameShowGenre = m_TotalGameShowData[i].GameShow_State["Genre"];

                    m_GameShowPanel.SetGameShowListText(_gameShowHostCompany, _gameShowName, _gameShowLevel, _gameShowProgressDate, _gameShowJudegeMent, _gameShowReward, _gameShowHostHelath, _gameShowHostPassion);
                    m_GameShowPanel.SetFunny(_gameShowFunny, true);
                    m_GameShowPanel.SetGraphic(_gameShowGraphic, true);
                    m_GameShowPanel.SetPerfection(_gameShowPerfection, true);
                    m_GameShowPanel.SetGenre(_gameShowGenre, true);

                    m_TemporaryGameShowData.m_TotalGameShowData = m_TotalGameShowData[i];
                    break;
                }
            }
        }
    }

    // 랜덤 공고와 스페셜 공고를 모두 합친 다음 우선순위에 따라 정렬해주는 함수
    private void CombineGameShowDataList()
    {
        foreach (var list in m_NowMonthRandomGameShowData)
        {
            if (!m_TotalGameShowData.Contains(list))
            {
                m_TotalGameShowData.Add(list);
            }
        }

        foreach (var list in m_SpecialGameShowDataPool)
        {
            if (!m_TotalGameShowData.Contains(list))
            {
                m_TotalGameShowData.Add(list);
            }
        }

        foreach (var list in m_HighLevelRandomPool)
        {
            if (!m_TotalGameShowData.Contains(list))
            {
                m_TotalGameShowData.Add(list);
            }
        }

        foreach (var list in m_LowLevelRandomPool)
        {
            if (!m_TotalGameShowData.Contains(list))
            {
                m_TotalGameShowData.Add(list);
            }
        }

        SortRecommendList();
    }

    public static void Swap<T>(List<T> list, int from, int to)
    {
        T tmp = list[from];
        list[from] = list[to];
        list[to] = tmp;
    }

    // 랜덤으로 데이터 뽑아내기
    private void RandomGameShowDataList(ref List<GameShowData> _list, List<GameShowData> dataList, int count)
    {
        List<GameShowData> _tempGameShowList = new List<GameShowData>(dataList);
        int _listCount = _tempGameShowList.Count;

        for (int i = 0; i < count; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, _listCount);
            _list.Add(_tempGameShowList[randomNum]);

            Swap(_tempGameShowList, randomNum, _tempGameShowList.Count - 1);
            _listCount -= 1;
        }
    }

    // 내 아카데미 등급에 따른 게임쇼
    public void TakeOutGameShowData()
    {
        InitLevelList();

        int _difficul = GetRankByMyAcademyRank(PlayerInfo.Instance.m_CurrentRank);

        int _myLevel = UnityEngine.Random.Range(2, 4);
        int _highLevel = UnityEngine.Random.Range(1, 3);
        int _lowLevel = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < m_GameShowRandomDataPool.Count; i++)
        {
            int level = m_GameShowRandomDataPool[i].GameShow_Level;

            if (!GameShowRandomData.ContainsKey(level))
            {
                GameShowRandomData[level] = new List<GameShowData>();
            }

            GameShowRandomData[level].Add(m_GameShowRandomDataPool[i]);
        }

        if (!GameShowRandomData.ContainsKey(_difficul))
        {
            return;
        }

        switch (_difficul)
        {
            case 1:
            {
                SortMyLevelList(GameShowRandomData[1]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[1], _myLevel);

                // 나보다 높은 레벨의 데이터들을 한곳에 몰아두고 그 중 1~2개만 뽑아서 쓴다.
                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                // 나보다 높은 레벨의 데이터들
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);
            }
            break;

            case 2:
            {
                SortMyLevelList(GameShowRandomData[2]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[2], _myLevel);

                // 나보다 높은 레벨의 데이터들을 한곳에 몰아두고 그 중 1~2개만 뽑아서 쓴다.
                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                for (int i = 0; i < GameShowRandomData[4].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[4][i]);
                }

                // 나보다 높은 레벨의 데이터들
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);

                // 나보다 낮은 레벨의 데이터들
                RandomGameShowDataList(ref m_LowLevelRandomPool, GameShowRandomData[1], _lowLevel);
            }
            break;

            case 3:
            {
                SortMyLevelList(GameShowRandomData[3]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[3], _myLevel);

                for (int i = 0; i < GameShowRandomData[4].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[4][i]);
                }

                for (int i = 0; i < GameShowRandomData[5].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[5][i]);
                }

                // 나보다 높은 레벨의 데이터들
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);

                // 나보다 낮은 레벨의 데이터들을 한곳에 몰아두고 그 중 1~2개만 뽑아서 쓴다.
                for (int i = 0; i < GameShowRandomData[1].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[1][i]);
                }

                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                // 나보다 낮은 레벨의 데이터들
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;

            case 4:
            {
                SortMyLevelList(GameShowRandomData[4]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[4], _myLevel);

                // 나보다 높은 레벨의 데이터들
                RandomGameShowDataList(ref m_HighLevelRandomPool, GameShowRandomData[5], _highLevel);

                // 나보다 낮은 레벨의 데이터들을 한곳에 몰아두고 그 중 1~2개만 뽑아서 쓴다.
                for (int i = 0; i < GameShowRandomData[1].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[1][i]);
                }

                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                // 나보다 낮은 레벨의 데이터들
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;

            case 5:
            {
                SortMyLevelList(GameShowRandomData[5]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[5], _myLevel);

                // 나보다 낮은 레벨의 데이터들을 한곳에 몰아두고 그 중 1~2개만 뽑아서 쓴다.
                for (int i = 0; i < GameShowRandomData[1].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[1][i]);
                }

                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                for (int i = 0; i < GameShowRandomData[4].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[4][i]);
                }

                // 나보다 낮은 레벨의 데이터들
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;
        }
    }

    #region _데이터 테이블 초기화 함수들
    public void InitLevelList()
    {
        highleveltotalpool.Clear();
        lowleveltotalpool.Clear();
        GameShowRandomData.Clear();
        m_NowMonthRandomGameShowData.Clear();
        m_LowLevelRandomPool.Clear();
        m_HighLevelRandomPool.Clear();
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
        m_NoveltyScriptList.Add("많은 이들에게 이미 알려진 게임이라\n이번 시연회때는 어떤 결과를 얻을지 예상할 수 없겠어요.\n새롭고 참신한 게임들과 경쟁할 수 있을지… ");
        m_NoveltyScriptList.Add("저번에 시연 했었던 게임을 출품하셨네요!\n이미 선보였던 게임이라 좋은 결과를 얻기는 힘들수 있겠어요.");
        m_NoveltyScriptList.Add("새로운 게임 출품 신청이 더 좋지 않았을까 싶어요.\n좋은 결과를 바래보죠! ");
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
            if (GameTime.Instance.FlowTime.NowMonth - 1 == GameJam.m_GameJamHistory.ElementAt(i).Value[i].m_GameJamData.m_MakeMonth)
            {
                _gameConcept = GameJam.m_GameJamHistory.ElementAt(i).Value[i].m_GameJamInfoData.m_GjamConcept;
                _gameGenre = GameJam.m_GameJamHistory.ElementAt(i).Value[i].m_GameJamData.m_Genre;
                break;
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

                bool _compareConcept = _gameConcept == x.GameShowConcept[0] || (x.GameShowConcept.Length == 2 && _gameConcept == x.GameShowConcept[1]);
                if (_compareConcept)
                    return -1;

                bool _compareGenre = _gameGenre == x.GameShowGenre[0] || (x.GameShowGenre.Length == 2 && _gameGenre == x.GameShowGenre[1]);
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
            var (_reward, famous, passion, health) = GetRewardByAssessment(m_TotalGameShowData[i].GameShow_Level, Assessment.Good);
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
    private (int _rewardMoney, int _famous, int _passion, int _health) GetRewardByAssessment(int level, Assessment assessment)
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
        List<ResultScript> filteredList = m_ResultScriptList.FindAll(x => x.ResultAssessment == _assessment && x.ScriptOrder == _scriptOrder);

        int count = filteredList.Count;

        if (count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, count);
            return filteredList[randomIndex].Script;
        }

        return "";
    }
}
