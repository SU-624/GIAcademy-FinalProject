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
    Terrible,       // �־�
    Bad,            // ����
    NotBad,
    SoSo,           // ����
    Good,           // ����
    Great,          // ��������
    Excellent,      // �ֻ�
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
    [SerializeField] private GameObject m_GameShowListPrefab;                                                               // ���Ӽ� ������
    [SerializeField] private GameObject m_PrevGameListPrefab;                                                               // �����뿡�� ���� ���ӵ��� ������
    [SerializeField] private GameObject m_RewardPrefab;
    [SerializeField] private GameObject m_EventPrefab;
    [SerializeField] private TextMeshProUGUI m_NoveltyScript;                                                               // �������� 1�̸��� �� ����� �г��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI m_GoTOGameShowResultPanel;                                                     // ���Ӽ��� ����� �������� ���� ����ִ� �г��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI m_GameShowResultPanelText;                                                     // ���Ӽ��� ����� �����ֱ� ���� ����ִ� �г��� �ؽ�Ʈ

    [Space(5f)]
    [Header("���ٰ� ���� �гε��� PopUp, PopOff ������Ʈ��")]
    [SerializeField] private PopUpUI m_PopUpResultPanel;                                                                    // ���Ӽ� ���â�� ������ popUPâ
    [SerializeField] private PopOffUI m_PopOffResultPanel;
    [SerializeField] private PopUpUI m_PopUpResultAnimationPanel;                                                           // ���Ӽ� ����� ���� �ִϸ��̼��� ������ �г�
    [SerializeField] private PopOffUI m_PopOffResultAnimationPanel;
    [SerializeField] private PopUpUI m_PopUpNoveltyWarningPanel;                                                            // �������� 1�̸����� ������ �� ����� �г�
    [SerializeField] private PopOffUI m_PopOffNoveltyWarningPanel;
    [SerializeField] private PopUpUI m_PopUpResultScriptPanel;                                                              // ���â���� ������ �ޱ��� �����ִ� ��ũ��Ʈ �г�
    [SerializeField] private PopOffUI m_PopOffResultScriptPanel;
    [SerializeField] private PopUpUI m_PopUpRewardPanel;                                                                    // ���� �г�
    [SerializeField] private PopOffUI m_PopOffRewardPanel;
    [Space(5f)]

    [Header("Ʃ�丮���")]
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

    private List<GameDifficulty> m_Difficulty = new List<GameDifficulty>();                                                 // �� ��ī���� ��޿� ���� ���̵��� �����ϴ� ����Ʈ
    private List<GameShowRewardByDifficulty> m_Assessnent = new List<GameShowRewardByDifficulty>();                         // ���̵��� �򰡹����� ���� ������ �����ϴ� ����Ʈ
    private List<GameShowScoreByHeart> m_ScoreByHeartList = new List<GameShowScoreByHeart>();
    private List<EvaluationScoreRange> m_ScoreRangeList = new List<EvaluationScoreRange>();                                 // �� ������ ���� ����
    private List<ScoreConversion> m_ScoreConversionList = new List<ScoreConversion>();                                      // �� ������ ���� �������� ���� ȯ��
    private List<FinalScoreRange> m_FinalScoreRangeList = new List<FinalScoreRange>();                                      //
    private List<AssessmentResponse> m_AssessmentResponseList = new List<AssessmentResponse>();                             // �� ��Ʈ���� ������ ���� ������� �ؽ�Ʈ��
    private List<ResultScript> m_ResultScriptList = new List<ResultScript>();                                               // ������ ��� �гο� ����� ��ũ��Ʈ ����
    private List<string> m_NoveltyScriptList = new List<string>();                                                          // ������ ������ 1�� �ƴ� �� ����� ��ũ��Ʈ��
    private List<string> m_GotoGameShowScriptList = new List<string>();                                                     // ���Ӽ����� �̵��� �� ����� ��ũ��Ʈ��(���â �̵��ϴ� �г�)
    private Dictionary<int, List<GameShowData>> GameShowRandomData = new Dictionary<int, List<GameShowData>>();             // 5������ ������ ���� �����͸� ���� �������ֱ� ���� ��ųʸ�(���� ������ �߰��ǵ� ���� �ٲ� �� �ִ�.)

    private List<GameShowData> m_GameShowRandomDataPool = new List<GameShowData>();                                         // �� �� ���ǿ� �´� �޿� ���̵��� ���� �������� �����͸� �־��ִ� ����Ʈ
    private List<GameShowData> m_SpecialGameShowDataPool = new List<GameShowData>();                                        // ���̵� ������� ������ ���� �־��ִ� ����Ʈ
    private List<GameShowData> m_TotalGameShowData = new List<GameShowData>();                                              // �̹��� ���� ����� ���Ӽ���� ��� ��Ƴ��� ����Ʈ
    private List<GameShowData> highleveltotalpool = new List<GameShowData>();                                               // �� ��ī���� ��޺��� ���� �����͵��� �ѹ��� ��Ƶΰ� �� �߿� �������� �� ���� �̾ƾ��Ѵ�.
    private List<GameShowData> lowleveltotalpool = new List<GameShowData>();                                                // �� ��ī���� ��޺��� ���� �����͵��� �ѹ��� ��Ƶΰ� �� �߿� �������� �� ���� �̾ƾ��Ѵ�.
    private List<GameShowData> m_NowMonthRandomGameShowData = new List<GameShowData>();                                     // �� ��ī���� ��޿� �´� �����͵��� ����ִ�  ����Ʈ

    private List<GameShowData> m_HighLevelRandomPool = new List<GameShowData>();                                            // �� ��ī���� ��޺��� ���� ����� �����͵��� ����ִ� ����Ʈ
    private List<GameShowData> m_LowLevelRandomPool = new List<GameShowData>();                                             // �� ��ī���� ��޺��� ���� ����� �����͵��� ����ִ� ����Ʈ

    private SaveGameShowData m_TemporaryGameShowData = new SaveGameShowData();                                              // ���� Ŭ���� ���Ӽ���� ������ �ӽ÷� �������ִ� ����
    private List<SaveGameShowData> m_ToBeRunningGameShow = new List<SaveGameShowData>();                                    // ���� ���� �����ϴ� ���Ӽ���� ��Ƶ� ����Ʈ
    private Queue<Action> eventQueue = new Queue<Action>();                                                                     // �������� ����ǰ� ���� ���Ӽ ������ �غ� ������Ѵ�. timescale�� 0�� �� �̺�Ʈ�� �����ϴ� �ȵż� Update������ timescale�� 0 ���� Ŭ �� ��������ִ°ɷ� ����

    public static Dictionary<int, List<SaveGameShowData>> m_GameShowHistory = new Dictionary<int, List<SaveGameShowData>>();

    //private Student[] m_StudentsParticipated = new Student[3];
    private Queue<string> m_ResultDialogQueue = new Queue<string>();                                                        // ������ ���â�� ����� ��ũ��Ʈ���� ��� ������� �־��� ť
    //private string[] m_LackCondition;

    public Sprite m_ApplySuccessSprite;
    public Sprite m_NotAllowedApplySprite;
    public Sprite m_ApplySprite;
    public Sprite[] m_FinalScoreSprite;
    public Sprite[] m_EmojiSprite;
    public Sprite[] m_StudentFace;
    private GameObject m_PrevClickObj;                                                                                      // ���� ������ Ŭ���� ������ ��ư�� ��Ƶ� ����

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
                // �ų� �߻��ؾ���
                else if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Year == 0 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month == GameTime.Instance.FlowTime.NowMonth + 1 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_SpecialGameShowDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
                // Ȧ���⵵ �߻�
                else if (AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Year == -1 && (10 % GameTime.Instance.FlowTime.NowYear) != 0 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Month == GameTime.Instance.FlowTime.NowMonth + 1 &&
                    AllOriginalJsonData.Instance.OriginalRandomGameShowData[i].GameShow_Special)
                {
                    m_SpecialGameShowDataPool.Add(AllOriginalJsonData.Instance.OriginalRandomGameShowData[i]);
                }
                // ¦���⵵ �߻�
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
                Debug.Log("���⼭ ���Ӽ� ���ֳ�?");
            }
        }
    }

    public void EnqueueDataChangedEvent()
    {
        eventQueue.Enqueue(() => GameShowDataChangeEvent?.Invoke());
    }

    // ���Ӽ� ��ư�� ������ ��������� �Լ�
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

    // ���� �гο��� �ޱ� ��ư�� ������ �� 
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

    #region _delegate�� ����� �Լ���
    // ���Ӽ ���ִ� ��ư�� Ŭ���Ǿ��� �� ������ �Լ���.
    private void HandleGameShowListButtonClicked()
    {
        MakeGameShowList();
        //m_GameShowPanel.ApplyButton
        m_GameShowPanel.SetActiveGameShowPanel(true);
    }

    // ��û ��ư�� Ŭ���Ǿ��� �� ���� �� �Լ�.
    private void HandleGameShowListApplyButton()
    {
        if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_ApplySprite && m_MonthLimit > 0)
        {
            //// �̶� ��û �Ϸ��ϰ� ��û��ư ��û�Ϸ�� ����
            if (_isCanParticipate)
            {
                ApplySuccess();
            }
            // �ƴ϶�� ��û�� �� ���ٰ� ��� �޼��� ����ֱ�
            else
            {
                ApplyFailed();
            }
        }
        // ����Ƚ�� ��� ��� 
        else if (m_GameShowPanel.ApplyButton.GetComponent<Image>().sprite == m_NotAllowedApplySprite && m_MonthLimit <= 0)
        {
            // ��ư Ŭ���� ������ �� �ִ� Ƚ���� �ʰ��ߴٴ� �޼��� ����ֱ�
            m_GameShowPanel.SetConditionFailPanel("������ �� �ִ� Ƚ���� �ʰ��Ͽ����ϴ�!\n���� ���� ���������.");
        }
        // else : ��û�ߴ� ��ư�� ������ ���� �� ����.
    }

    // ���� ��ư�� Ŭ���Ǿ��� �� ���� �� �Լ�.
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

    // ��������ϴ� �Լ�
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

        // ���Ӽ ������ �������� ������ �� �ʿ�
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

    // ���� ������ ���� ����� �ִϸ��̼��� �ٸ��� ���� �Լ�
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

    // ���â���� �̵��ϱ� ���� ����ִ� �гο� ������ ��ũ��Ʈ ��������
    private void GetGameShowResultPanelScript()
    {
        int _index = UnityEngine.Random.Range(0, m_GotoGameShowScriptList.Count);

        m_GoTOGameShowResultPanel.text = m_GotoGameShowScriptList[_index];

        m_PopUpResultPanel.TurnOnUI();
        m_IsCheckGameShow = true;
    }

    // BGM��� �ð���ŭ ��ٷȴٰ� �г� �������ֱ�
    IEnumerator ChangeAnimationPanel()
    {
        yield return new WaitForSecondsRealtime(5f);

        m_GameShowAnimationPanel.SetActiveAnimationPanel();

        StartCoroutine(SetSatisfactionStatusPanel());
    }

    // ���� ������ ��Ȳ�� �����ֱ� ���� �гο� ��ũ��Ʈ�� �̹����� �־��ִ� �Լ�
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

        // ���� ���� ������ �������ִ°� �߰��ϱ�
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
        m_ResultDialogQueue.Enqueue("�ÿ�ȸ �������δ� \"" + _mainText1 + "\"," + "\"" + _mainText2 + "\"��(��) �ֳ׿�.");

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
    // ����� ���� ������ ������ش�.
    private void MakeResultReward()
    {
        if (m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Plusreward != 0)
        {
            // �߰� ������ ���� �� ��� �� �� ����ϱ�
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
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardMoney.ToString();
            }

            if (m_RewardFamous != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardFamous";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "������";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardFamous.ToString();
            }

            if (m_RewardPassion != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardPassion";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "����";
                //_reward.transform.GetChild(1).GetComponent<Imgae>().sprite = ;
                _reward.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_RewardPassion.ToString();
            }

            if (m_RewardHealth != 0)
            {
                GameObject _reward = Instantiate(m_RewardPrefab, m_RewardPanel.m_ContentsTransform);
                _reward.name = "RewardHealth";
                _reward.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ü��";
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

        // �ջ�� D-Day ��
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

    // ���Ӽ� �г��� ���� �� ���� ���� ������ �ʱ�ȭ
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

    // ���Ӽ��� ��ư�� ������ �� ������ ���� ���� ����� ������� �Լ�
    private void GetPrevGameList()
    {
        m_GameShowPanel.DestroyObj(m_GameShowPanel.PrevGameListContentObj);

        if (GameJam.m_GameJamHistory.Count == 0)
        {
            m_GameShowPanel.SetActiveIsNotPrevGameList(true);
        }
        // ���Ӽ ������ ������ ���� ���ϰ� �ϰ� �� ���� ��������Ѵ�.
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

    // �����뿡�� ���� ���ӵ��� ���� �Լ�
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

    // ���� ������ ���� ã�Ƽ� ���� ������ֱ�
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

        m_GameShowPanel.ApplyButton.interactable = true;
        m_PrevClickObj = _currentObj;
    }

    private void ApplySuccess()
    {
        // ���� �̹� ��û�� �������� ����ִµ� �����Ŷ�� �Ѱ��ֱ�
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
            // ����Ƚ�� ��~
            m_GameShowPanel.SetConditionFailPanel("�̹��� ������ ��~");
        }
    }

    private void ApplyFailed()
    {
        string[] _text = new string[3];

        for (int i = 0; i < 3; i++)
        {
            if (m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Passion < m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Pasion)
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "�л��� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
            }
            else if (m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_Health < m_TemporaryGameShowData.m_TotalGameShowData.GameShow_Health)
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "�л��� ü���� �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
            }
            else
            {
                _text[i] = m_TemporaryGameShowData.m_ParticipatingStudents[i].m_StudentStat.m_StudentName + "�л��� ü�°� ������ �����Ͽ� ���Ӽ ���� �� �����ϴ�.";
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
                    _eventPrrerfab.GetComponent<EventPrefab>().ActivityText.text = "���Ӽ� �غ���" + _subString;
                    _subString = "";
                }
                //m_EventPanel.EventActivity.text = "���Ӽ� �غ���" + _subString;

                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private void SetGenreAndConceptHeart(int _gameJamIndex, int _gameJamListIndex)
    {
        if (m_TemporaryGameShowData.m_TotalGameShowData.GameShowConcept.Any(c => c == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamInfoData.m_GjamConcept) &&
            m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre.Any(g => g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamData.m_Genre)
            || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "��� �帣" && m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "��� ����")
        {
            m_GenreAndConceptHeart = 3;
        }
        else if (m_TemporaryGameShowData.m_TotalGameShowData.GameShowConcept.Any(c => c == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamInfoData.m_GjamConcept) ||
                 m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre.Any(g => g == GameJam.m_GameJamHistory.ElementAt(_gameJamIndex).Value[_gameJamListIndex].m_GameJamData.m_Genre)
                 || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "��� �帣" || m_TemporaryGameShowData.m_TotalGameShowData.GameShowGenre[0] == "��� ����")
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
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "�� " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "��";
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
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "�� " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "��";
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
                        string _gameShowProgressDate = (m_TotalGameShowData[k].GameShow_Month.ToString()) + "�� " + (m_TotalGameShowData[k].GameShow_Week.ToString()) + "��";
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
                    string _gameShowProgressDate = (m_TotalGameShowData[i].GameShow_Month.ToString()) + "�� " + (m_TotalGameShowData[i].GameShow_Week.ToString()) + "��";
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

    // ���� ����� ����� ���� ��� ��ģ ���� �켱������ ���� �������ִ� �Լ�
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

    // �������� ������ �̾Ƴ���
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

    // �� ��ī���� ��޿� ���� ���Ӽ�
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

                // ������ ���� ������ �����͵��� �Ѱ��� ���Ƶΰ� �� �� 1~2���� �̾Ƽ� ����.
                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);
            }
            break;

            case 2:
            {
                SortMyLevelList(GameShowRandomData[2]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[2], _myLevel);

                // ������ ���� ������ �����͵��� �Ѱ��� ���Ƶΰ� �� �� 1~2���� �̾Ƽ� ����.
                for (int i = 0; i < GameShowRandomData[3].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[3][i]);
                }

                for (int i = 0; i < GameShowRandomData[4].Count; i++)
                {
                    highleveltotalpool.Add(GameShowRandomData[4][i]);
                }

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);

                // ������ ���� ������ �����͵�
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

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_HighLevelRandomPool, highleveltotalpool, _highLevel);

                // ������ ���� ������ �����͵��� �Ѱ��� ���Ƶΰ� �� �� 1~2���� �̾Ƽ� ����.
                for (int i = 0; i < GameShowRandomData[1].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[1][i]);
                }

                for (int i = 0; i < GameShowRandomData[2].Count; i++)
                {
                    lowleveltotalpool.Add(GameShowRandomData[2][i]);
                }

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;

            case 4:
            {
                SortMyLevelList(GameShowRandomData[4]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[4], _myLevel);

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_HighLevelRandomPool, GameShowRandomData[5], _highLevel);

                // ������ ���� ������ �����͵��� �Ѱ��� ���Ƶΰ� �� �� 1~2���� �̾Ƽ� ����.
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

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;

            case 5:
            {
                SortMyLevelList(GameShowRandomData[5]);
                RandomGameShowDataList(ref m_NowMonthRandomGameShowData, GameShowRandomData[5], _myLevel);

                // ������ ���� ������ �����͵��� �Ѱ��� ���Ƶΰ� �� �� 1~2���� �̾Ƽ� ����.
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

                // ������ ���� ������ �����͵�
                RandomGameShowDataList(ref m_LowLevelRandomPool, lowleveltotalpool, _lowLevel);
            }
            break;
        }
    }

    #region _������ ���̺� �ʱ�ȭ �Լ���
    public void InitLevelList()
    {
        highleveltotalpool.Clear();
        lowleveltotalpool.Clear();
        GameShowRandomData.Clear();
        m_NowMonthRandomGameShowData.Clear();
        m_LowLevelRandomPool.Clear();
        m_HighLevelRandomPool.Clear();
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
        m_NoveltyScriptList.Add("���� �̵鿡�� �̹� �˷��� �����̶�\n�̹� �ÿ�ȸ���� � ����� ������ ������ �� ���ھ��.\n���Ӱ� ������ ���ӵ�� ������ �� �������� ");
        m_NoveltyScriptList.Add("������ �ÿ� �߾��� ������ ��ǰ�ϼ̳׿�!\n�̹� �������� �����̶� ���� ����� ���� ����� �ְھ��.");
        m_NoveltyScriptList.Add("���ο� ���� ��ǰ ��û�� �� ���� �ʾ����� �;��.\n���� ����� �ٷ�����! ");
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

    // ���Ӽ� ���â�� ������ �켱������ ��µ� �� �ְ� ������ ���ش�.
    // �ű�, ���ӳ��̵� ������, ��� ���� ��, ������ ����
    // �켱 ����� �⺻�� good
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
