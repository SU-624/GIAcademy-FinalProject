using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rank
{
    SSS,
    SS,
    S,
    A,
    B,
    C,
    D,
    E,
    F,
    None,
}

public class RankSystem : MonoBehaviour
{
    [SerializeField] private RankPanel m_RankPanel;
    [SerializeField] private RankPopUpPanel m_RankPopUpPanel;
    [SerializeField] private CheckRankPnael m_CheckRankPnael;
    [SerializeField] private EndingAlramPanel m_EndingAlramPanel;
    [SerializeField] private RankScene1 m_RankScene1;
    [SerializeField] private RankScene2 m_RankScene2;
    [SerializeField] private RankScene3 m_RankScene3;
    [SerializeField] private RankScene4 m_RankScene4;
    [SerializeField] private Sprite[] m_ScoreRank;
    [SerializeField] private Sprite[] m_Throphy;
    [SerializeField] private MailManagement m_RewardMail;
    //[SerializeField] private  m_MakeMail;

    private bool m_IsTouchScreen;
    private bool m_IsScene2Active;

    private int m_ChangeDay;
    private int _rankConditionIndex;
    private int _gradeConditionIndex;
    private int _currentGrade;
    string[] _rankContent;
    string _rankContentText;

    string[] _gradeContent;
    string _gradeContentText;

    private Dictionary<Rank, int> m_PrevRank = new Dictionary<Rank, int>();     // 내가 몇 등을 몇 번 했었는지 저장할 딕셔너리
    private Dictionary<int, int> m_PrevGrade = new Dictionary<int, int>();
    private List<RankTable> m_ScoreByGrade = new List<RankTable>();
    private List<RankTable> m_ScoreByFinalGrade = new List<RankTable>();
    private List<ScriptRankCondition> m_RankByScriptCondition = new List<ScriptRankCondition>();
    private List<ScriptGradeCondition> m_GradeByScriptCondition = new List<ScriptGradeCondition>();

    private List<RankScript> m_RankScriptList = new List<RankScript>();
    private Queue<string> m_TextScript = new Queue<string>();
    private Coroutine m_SetScoreCo;


    private void Start()
    {
        m_RankPanel.InitFadeImage();
        InitGradeTable();
        InitFinalGradeTable();
        InitScriptCondition();
        InitCheckRankPanelScript();
    }

    private void InitCheckRankPanelScript()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalRankScripteData.Count; i++)
        {
            m_RankScriptList.Add(AllOriginalJsonData.Instance.OriginalRankScripteData[i]);
        }
    }

    // 점수에 따른 등급을 넣어준다.
    private void InitGradeTable()
    {
        m_ScoreByGrade.Add(new RankTable(Rank.SSS, 320, 400));
        m_ScoreByGrade.Add(new RankTable(Rank.SS, 280, 360));
        m_ScoreByGrade.Add(new RankTable(Rank.S, 240, 320));
        m_ScoreByGrade.Add(new RankTable(Rank.A, 200, 280));
        m_ScoreByGrade.Add(new RankTable(Rank.B, 160, 240));
        m_ScoreByGrade.Add(new RankTable(Rank.C, 120, 200));
        m_ScoreByGrade.Add(new RankTable(Rank.D, 80, 160));
        m_ScoreByGrade.Add(new RankTable(Rank.E, 40, 120));
        m_ScoreByGrade.Add(new RankTable(Rank.F, 0, 80));
    }

    private void InitScriptCondition()
    {
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.F, 1, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.F, 2, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.E, 3, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.E, 4, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.D, 5, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.D, 6, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.C, 7, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.C, 8, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.B, 9, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.B, 10, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.A, 11, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.A, 12, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.S, 13, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.S, 14, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.SS, 15, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.SS, 16, true));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.SSS, 17, false));
        m_RankByScriptCondition.Add(new ScriptRankCondition(Rank.SSS, 18, true));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(0, 19, false));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(3, 20, false));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(3, 21, true));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(2, 22, false));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(2, 23, true));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(1, 24, false));
        m_GradeByScriptCondition.Add(new ScriptGradeCondition(1, 25, true));
    }

    // 최종 등급을 결정할 점수를 넣어준다.
    private void InitFinalGradeTable()
    {
        m_ScoreByFinalGrade.Add(new RankTable(Rank.SSS, 1280, 1600));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.SS, 1120, 1440));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.S, 960, 1280));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.A, 800, 1120));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.B, 640, 960));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.C, 480, 800));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.D, 320, 640));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.E, 160, 480));
        m_ScoreByFinalGrade.Add(new RankTable(Rank.F, 0, 320));
    }

    private void Update()
    {
        if (m_ChangeDay != GameTime.Instance.FlowTime.NowDay)
        {
            InGameUI.Instance.m_IsPopUpRank = false;
            m_IsTouchScreen = false;
            m_IsScene2Active = false;
            m_ChangeDay = GameTime.Instance.FlowTime.NowDay;
        }

        if (InGameUI.Instance.m_IsPopUpRank && !m_IsTouchScreen && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            m_IsTouchScreen = true;
            m_RankPanel.PopUpRankPanel();
            InitRankPanel();
            SetRankScene1Text();
            m_RankPopUpPanel.RankPopUpAndOff(false);

            StartCoroutine(ChangeScene1atScene2(3f));
        }

        if (!m_IsScene2Active && m_RankScene2.gameObject.activeSelf && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            m_IsScene2Active = true;

            StopCoroutine(m_SetScoreCo);

            for (int i = 0; i < 6; i++)
            {
                m_RankScene2.ScoreTextObj[i].SetActive(true);

                if (i != 5)
                {
                    m_RankScene2.ScoreRankImgae[i].SetActive(true);
                }
                else
                {
                    StartCoroutine(SetFinalGrade());
                }
            }
        }
    }

    IEnumerator ChangeScene1atScene2(float _time)
    {
        yield return YieldInstructionCache.WaitForSecondsRealtime(_time);

        m_RankScene1.gameObject.SetActive(false);
        m_RankScene2.gameObject.SetActive(true);
        m_RankPanel.SetSpeechBubbleActive(false);
        SetScoreScene2();

        m_SetScoreCo = StartCoroutine(SetScore());
    }

    // 1초마다 점수가 뜨고 점수가 뜬 후 0.5초마다 점수의 랭크를 띄워줘야한다.
    IEnumerator SetScore()
    {
        yield return new WaitForSecondsRealtime(1f);

        for (int i = 0; i < 6; i++)
        {
            m_RankScene2.ScoreTextObj[i].SetActive(true);

            yield return new WaitForSecondsRealtime(0.5f);

            if (i != 5)
            {
                m_RankScene2.ScoreRankImgae[i].SetActive(true);
            }
            else
            {
                yield return StartCoroutine(SetFinalGrade());
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    IEnumerator SetFinalGrade()
    {
        yield return YieldInstructionCache.WaitForSecondsRealtime(1.5f);

        string _totalScore = (PlayerInfo.Instance.m_Goods + PlayerInfo.Instance.m_Awareness + PlayerInfo.Instance.m_Management + PlayerInfo.Instance.m_TalentDevelopment + PlayerInfo.Instance.m_Activity).ToString();

        SetFinalRank(GetFinalRankByScore(int.Parse(_totalScore)));

        m_RankPanel.SetSpeechBubbleActive(true);
        m_RankPanel.SetSpeechBubbleText("축하합니다!");

        yield return YieldInstructionCache.WaitForSecondsRealtime(1f);

        m_RankScene2.gameObject.SetActive(false);
        m_RankScene3.gameObject.SetActive(true);
        m_RankPanel.SetSpeechBubbleActive(false);

        SetTitleScene3Text();

        yield return StartCoroutine(ChangeScene2atScene3());
    }

    IEnumerator ChangeScene2atScene3()
    {
        yield return YieldInstructionCache.WaitForSecondsRealtime(0.5f);

        m_RankPanel.SetSpeechBubbleActive(true);
        m_RankPanel.SetSpeechBubbleText("최종순위를 발표하겠습니다.");

        int _count = 2;
        int _textIndex = 0;
        int _throphyIndex = 2;
        string[] _text = { "3위는...", "2위는..." };

        while (_count > 0)
        {
            yield return YieldInstructionCache.WaitForSecondsRealtime(1f);

            if (m_RankScene3.Title.activeSelf)
            {
                m_RankScene3.Title.SetActive(false);
            }

            if (m_RankScene3.AcademyGrade.activeSelf)
            {
                m_RankScene3.AcademyGrade.SetActive(false);
            }

            m_RankPanel.SetSpeechBubbleText(_text[_textIndex]);
            m_RankScene3.SetThrophyImage(_throphyIndex, true);

            yield return YieldInstructionCache.WaitForSecondsRealtime(1f);

            m_RankScene3.SetThrophyImage(_throphyIndex, false);
            m_RankScene3.AcademyGrade.SetActive(true);

            if (QuarterlyReport.Instance.PrevYearAcademyRank.Count != 0)
            {
                m_RankScene3.SetAcademyGrade(QuarterlyReport.Instance.PrevYearAcademyRank[_throphyIndex].Key,
                    QuarterlyReport.Instance.PrevYearAcademyRank[_throphyIndex].Value.ToString(), m_Throphy[_throphyIndex]);
                m_RankPanel.SetSpeechBubbleText(QuarterlyReport.Instance.PrevYearAcademyRank[_throphyIndex].Key);
            }
            _count -= 1;
            _textIndex += 1;
            _throphyIndex -= 1;
        }

        yield return YieldInstructionCache.WaitForSecondsRealtime(1f);

        m_RankScene3.AcademyGrade.SetActive(false);
        m_RankPanel.SetSpeechBubbleText("대망의 1위는!!");
        m_RankScene3.SetThrophyImage(0, true);

        yield return YieldInstructionCache.WaitForSecondsRealtime(1.5f);

        m_RankScene3.SetThrophyImage(0, false);
        m_RankScene3.AcademyGrade.SetActive(true);

        if (QuarterlyReport.Instance.PrevYearAcademyRank.Count != 0)
        {

            m_RankScene3.SetAcademyGrade(PlayerInfo.Instance.m_AcademyName, "4500", m_Throphy[0]);
            m_RankPanel.SetSpeechBubbleText(PlayerInfo.Instance.m_AcademyName + "!!");
        }

        //if (QuarterlyReport.Instance.PrevYearAcademyRank.Count != 0)
        //{
        //    m_RankScene3.SetAcademyGrade(QuarterlyReport.Instance.PrevYearAcademyRank[0].Key, QuarterlyReport.Instance.PrevYearAcademyRank[0].Value.ToString(), m_Throphy[0]);
        //    m_RankPanel.SetSpeechBubbleText(QuarterlyReport.Instance.PrevYearAcademyRank[0].Key + "!!");
        //}

        yield return YieldInstructionCache.WaitForSecondsRealtime(1.5f);
        m_RankScene3.gameObject.SetActive(false);
        m_RankScene4.gameObject.SetActive(true);
        m_RankPanel.SetSpeechBubbleActive(false);
        SetScene4();
    }

    // Scene1에서 보여줄 유저지정 이름과 합친 고정멘트
    private void SetRankScene1Text()
    {
        string _academyName = PlayerInfo.Instance.m_AcademyName;

        m_RankPanel.SetSpeechBubbleText("다음은" + _academyName + "입니다.");
        m_RankScene1.ChangeTitle(_academyName + "점수발표");
    }

    // Scene3에서 보여줄 현재 연도를 합친 고정멘트
    private void SetTitleScene3Text()
    {
        string _year = GameTime.Instance.FlowTime.NowYear.ToString();

        m_RankScene3.SetTitleText(_year + "년 최종순위 발표");
    }

    // Scene2에서 보여줄 점수들을 셋팅해주는 함수
    private void SetScoreScene2()
    {
        string _goodsScore = PlayerInfo.Instance.m_Goods.ToString();
        string _famousScore = PlayerInfo.Instance.m_Awareness.ToString();
        string _managementScore = PlayerInfo.Instance.m_Management.ToString();
        string _activityScore = PlayerInfo.Instance.m_Activity.ToString();
        string _talentDevelopmentScore = PlayerInfo.Instance.m_TalentDevelopment.ToString();

        string _totalScore = (PlayerInfo.Instance.m_Goods + PlayerInfo.Instance.m_Awareness + PlayerInfo.Instance.m_Management + PlayerInfo.Instance.m_TalentDevelopment + PlayerInfo.Instance.m_Activity).ToString();

        Sprite _goodsScoreImage = ScoreRank(GetRankByScore(PlayerInfo.Instance.m_Goods));
        Sprite _famousScoreImage = ScoreRank(GetRankByScore(PlayerInfo.Instance.m_Awareness));
        Sprite _managementScoreImage = ScoreRank(GetRankByScore(PlayerInfo.Instance.m_Management));
        Sprite _activityScoreImage = ScoreRank(GetRankByScore(PlayerInfo.Instance.m_Activity));
        Sprite _talentDevelopmentScoreImage = ScoreRank(GetRankByScore(PlayerInfo.Instance.m_TalentDevelopment));

        m_RankScene2.SetScoreText(_goodsScore, _famousScore, _managementScore, _activityScore, _talentDevelopmentScore, _totalScore);

        m_RankScene2.SetScoreImage(_goodsScoreImage, _famousScoreImage, _managementScoreImage, _activityScoreImage, _talentDevelopmentScoreImage);
    }

    private void SetScene4()
    {
        m_RankPanel.SetSpeechBubbleActive(true);
        m_RankPanel.SetSpeechBubbleText("수상한 학원들 축하합니다.\n1,2,3위 학원들에게는\n부상이 주어집니다.");

        string _1stAcademyScore = "4500";
        string _2ndAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[1].Value.ToString();
        string _3rdAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[2].Value.ToString();

        string _1stAcademyName = PlayerInfo.Instance.m_AcademyName;
        string _2ndAcademyName = QuarterlyReport.Instance.TotalAcademyScores[1].Key;
        string _3rdAcademyName = QuarterlyReport.Instance.TotalAcademyScores[2].Key;

        string _myAcademyName = PlayerInfo.Instance.m_AcademyName;

        int _myAcademyIndex = 0;
        int _myAcademyRankIndex = 1;
        int _myAcademyIncreaseIndex = 3;

        string _myAcademyScore = "4500";
        string _myAcademyRank = "1";
        string _myAcademyIncrease = "3";

        string _otherAcademy1Name = QuarterlyReport.Instance.TotalAcademyScores[0].Key;
        string _otherAcademy1Score = QuarterlyReport.Instance.TotalAcademyScores[0].Value.ToString();
        string _otherAcademy1Rank = QuarterlyReport.Instance.PrevYearAcademyRank[0].Value.ToString();
        string _otherAcademy1Increase = QuarterlyReport.Instance.AcademyNameToRank[2].Value.ToString();

        string _otherAcademy2Name = QuarterlyReport.Instance.TotalAcademyScores[1].Key;
        string _otherAcademy2Score = QuarterlyReport.Instance.TotalAcademyScores[1].Value.ToString();
        string _otherAcademy2Rank = QuarterlyReport.Instance.PrevYearAcademyRank[1].Value.ToString();
        string _otherAcademy2Increase = QuarterlyReport.Instance.AcademyNameToRank[4].Value.ToString();

        //string _1stAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[0].Value.ToString();
        //string _2ndAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[1].Value.ToString();
        //string _3rdAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[2].Value.ToString();

        //string _1stAcademyName = QuarterlyReport.Instance.TotalAcademyScores[0].Key;
        //string _2ndAcademyName = QuarterlyReport.Instance.TotalAcademyScores[1].Key;
        //string _3rdAcademyName = QuarterlyReport.Instance.TotalAcademyScores[2].Key;

        //string _myAcademyName = PlayerInfo.Instance.m_AcademyName;

        //int _myAcademyIndex = QuarterlyReport.Instance.TotalAcademyScores.FindIndex(x => x.Key == _myAcademyName);
        //int _myAcademyRankIndex = QuarterlyReport.Instance.PrevYearAcademyRank.FindIndex(x => x.Key == _myAcademyName);
        //int _myAcademyIncreaseIndex = QuarterlyReport.Instance.AcademyNameToRank.FindIndex(x => x.Key == _myAcademyName);

        //string _myAcademyScore = QuarterlyReport.Instance.TotalAcademyScores[_myAcademyIndex].Value.ToString();
        //string _myAcademyRank = QuarterlyReport.Instance.PrevYearAcademyRank[_myAcademyRankIndex].Value.ToString();
        //string _myAcademyIncrease = QuarterlyReport.Instance.AcademyNameToRank[_myAcademyIncreaseIndex].Value.ToString();

        //string _otherAcademy1Name = QuarterlyReport.Instance.TotalAcademyScores[_myAcademyIndex - 1].Key;
        //string _otherAcademy1Score = QuarterlyReport.Instance.TotalAcademyScores[_myAcademyIndex - 1].Value.ToString();
        //string _otherAcademy1Rank = QuarterlyReport.Instance.PrevYearAcademyRank[_myAcademyRankIndex - 1].Value.ToString();
        //string _otherAcademy1Increase = QuarterlyReport.Instance.AcademyNameToRank[_myAcademyIncreaseIndex - 1].Value.ToString();

        //string _otherAcademy2Name = QuarterlyReport.Instance.TotalAcademyScores[_myAcademyIndex + 1].Key;
        //string _otherAcademy2Score = QuarterlyReport.Instance.TotalAcademyScores[_myAcademyIndex + 1].Value.ToString();
        //string _otherAcademy2Rank = QuarterlyReport.Instance.PrevYearAcademyRank[_myAcademyRankIndex + 1].Value.ToString();
        //string _otherAcademy2Increase = QuarterlyReport.Instance.AcademyNameToRank[_myAcademyIncreaseIndex + 1].Value.ToString();


        m_RankScene4.SetRankGraph(_1stAcademyName, _2ndAcademyName, _3rdAcademyName, _1stAcademyScore, _2ndAcademyScore, _3rdAcademyScore);
        m_RankScene4.SetMyAcademy(_myAcademyName, _myAcademyScore, _myAcademyRank);
        m_RankScene4.IncreaseAndDecrease(_otherAcademy1Increase, _myAcademyIncrease, _otherAcademy2Increase);
        m_RankScene4.SetOtherAcademy(_otherAcademy1Name, _otherAcademy1Score, _otherAcademy2Name, _otherAcademy2Score, _otherAcademy1Rank, _otherAcademy2Rank);

        if (_myAcademyRankIndex == 0)
        {
            m_RankScene4.MyAcademy.transform.SetAsFirstSibling();
        }

        if (_1stAcademyName == PlayerInfo.Instance.m_AcademyName)
        {
            SaveGrade(1);
            _currentGrade = 1;
        }
        else if (_2ndAcademyName == PlayerInfo.Instance.m_AcademyName)
        {
            SaveGrade(2);
            _currentGrade = 2;
        }
        else if (_3rdAcademyName == PlayerInfo.Instance.m_AcademyName)
        {
            SaveGrade(3);
            _currentGrade = 3;
        }
        else
        {
            SaveGrade(0);
            _currentGrade = 0;
        }

    }

    // 몇 등을 몇번 했는지 저장해주기 위한 함수
    private void SaveGrade(int _grade)
    {
        if (!m_PrevGrade.ContainsKey(1))
        {
            m_PrevGrade.Add(_grade, 1);
        }
        else
        {
            m_PrevGrade[_grade] += 1;
        }
    }

    public void ClickBackButton()
    {
        StartCoroutine(BackButtonCor());
    }

    private IEnumerator BackButtonCor()
    {
        m_RankPanel.SetSpeechBubbleText("내년에 또 뵙겠습니다!");

        m_RankPanel.CurtainObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        yield return StartCoroutine(m_RankPanel.FadeInPanel());

        m_CheckRankPnael.SetActiveCheckRankPanel(true);

        _rankConditionIndex = GetConditionByRank(PlayerInfo.Instance.m_CurrentRank);
        yield return StartCoroutine(SetCheckRankPanelContentByRank(_rankConditionIndex));

        _gradeConditionIndex = GetConditionByGrade(_currentGrade);
        yield return StartCoroutine(SetCheckRankPanelContentByGrade(_gradeConditionIndex));

        // 등급 나온 뒤 실행되는 코드
        m_CheckRankPnael.SetActiveCheckRankPanel(false);

        if (m_PrevRank[Rank.SSS] == 1)
        {
            m_EndingAlramPanel.SetEndingAlramPanel(true);
        }
    }

    private IEnumerator CheckRankPanelContent()
    {
        yield return new WaitUntil(() =>
        {
            if (Input.touchCount > 1 || Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else
            {
                return false;
            }

        });
    }

    // 랭크가 다 끝난 후 띄워줄 텍스트 패널
    private IEnumerator SetCheckRankPanelContentByRank(int _condition)
    {
        for (int i = 0; i < m_RankScriptList.Count; i++)
        {
            if (m_RankScriptList[i].Condition == _condition)
            {
                if (m_RankScriptList[i].Text.Contains("/"))
                {
                    _rankContent = m_RankScriptList[i].Text.Split("/");

                    for (int j = 0; j < _rankContent.Length; j++)
                    {
                        m_TextScript.Enqueue(_rankContent[j]);
                    }

                    yield return StartCoroutine(Dialoge(_rankContentText));
                }
                else
                {
                    m_CheckRankPnael.SetContents(m_RankScriptList[i].Text);

                    yield return StartCoroutine(CheckRankPanelContent());
                }

            }
        }
    }

    private IEnumerator SetCheckRankPanelContentByGrade(int _condition)
    {
        Debug.Log(m_RankScriptList.Count);

        for (int i = 0; i < m_RankScriptList.Count; i++)
        {
            if (m_RankScriptList[i].Condition == _condition)
            {
                if (m_RankScriptList[i].Text.Contains("/"))
                {
                    _gradeContent = m_RankScriptList[i].Text.Split("/");

                    for (int j = 0; j < _gradeContent.Length; j++)
                    {
                        m_TextScript.Enqueue(_gradeContent[j]);
                    }

                    yield return StartCoroutine(Dialoge(_gradeContentText));
                }
                else
                {
                    m_CheckRankPnael.SetContents(m_RankScriptList[i].Text);

                    yield return StartCoroutine(CheckRankPanelContent());
                }
            }
        }
    }

    private IEnumerator Dialoge(string _script)
    {
        while (m_TextScript.Count > 0)
        {
            _script = m_TextScript.Dequeue();

            m_CheckRankPnael.SetContents(_script);

            yield return new WaitUntil(() =>
            {
                if ((Input.touchCount > 1 || Input.GetMouseButtonDown(0)) && m_TextScript.Count >= 0)
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

        SendReward();
    }

    private void SendReward()
    {
        for (int i = 0; i < m_RankScriptList.Count; i++)
        {
            if (m_RankScriptList[i].Condition == _rankConditionIndex)
            {
                int _email = AllOriginalJsonData.Instance.OriginalRewardEmailData.FindIndex(x => x.Email_script_id == _rankConditionIndex);
                string _sender = m_RewardMail.FindSenderName(AllOriginalJsonData.Instance.OriginalRewardEmailData[_email].Email_sender_id);
                string _title = AllOriginalJsonData.Instance.OriginalRewardEmailData[_email].Email_sender_Name;
                string _content = AllOriginalJsonData.Instance.OriginalRewardEmailData[_email].Email_script_Text;

                string _reward1 = m_RankScriptList[i].Reward1Amount.ToString();
                string _reward2 = m_RankScriptList[i].Reward2Amount.ToString();

                string _data = GameTime.Instance.FlowTime.NowYear.ToString() + "년 " + GameTime.Instance.FlowTime.NowMonth.ToString() + "월 " + GameTime.Instance.FlowTime.NowDay.ToString() + "일";

                m_RewardMail.MakeMail(_title, _data, _sender, _content, _reward1, _reward2, MailType.RewardMail, true, "", new Specification());
                m_RewardMail.SendMail();
                //m_MakeMail.MakeRewardMail(_title, _data, _sender, _content, _reward1, _reward2);
                //m_MakeMail.SendRewardMail();
            }
        }
    }

    // 내 학원의 최종 등급의 스프라이트를 띄워주는 함수 
    private void SetFinalRank(Rank _rank)
    {
        m_RankScene2.RankImage[(int)_rank].SetActive(true);
    }

    // 점수로 등급을 내보내주는 함수
    private Rank GetRankByScore(int value)
    {
        foreach (RankTable table in m_ScoreByGrade)
        {
            if (table.MinValue <= value && value < table.MaxValue || table.MaxValue == 400 && value >= table.MaxValue)
            {
                return table.Grade;
            }
        }

        return Rank.None;
    }

    // 내 현재 등급으로 마지막 대화창에 띄워줄 스크립트의 인덱스값 가져오기
    private int GetConditionByRank(Rank _rank)
    {
        foreach (ScriptRankCondition condition in m_RankByScriptCondition)
        {
            if (m_PrevRank[_rank] > 1)
            {
                if (condition.RepeatRank == true && condition.MyRank == _rank)
                {
                    return condition.Condition;
                }
            }
            else
            {
                if (condition.RepeatRank == false && condition.MyRank == _rank)
                {
                    return condition.Condition;
                }
            }
        }

        return 0;
    }

    public int GetConditionByGrade(int _grade)
    {
        foreach (ScriptGradeCondition condition in m_GradeByScriptCondition)
        {
            if (m_PrevGrade[_grade] > 1)
            {
                if (condition.RepeatGrade == true && condition.MyGrade == _grade)
                {
                    return condition.Condition;
                }
            }
            else
            {
                if (condition.RepeatGrade == false && condition.MyGrade == _grade)
                {
                    return condition.Condition;
                }
            }
        }

        return 0;
    }

    private Rank GetFinalRankByScore(int value)
    {
        foreach (RankTable table in m_ScoreByFinalGrade)
        {
            if (table.MinValue <= value && value < table.MaxValue || table.MaxValue == 1600 && value >= table.MaxValue)
            {
                if (m_PrevRank.ContainsKey(table.Grade))
                {
                    m_PrevRank[table.Grade] += 1;
                }
                else
                {
                    m_PrevRank.Add(table.Grade, 1);
                }

                PlayerInfo.Instance.m_CurrentRank = table.Grade;

                return table.Grade;
            }
        }

        return Rank.None;
    }

    private Sprite ScoreRank(Rank _rank)
    {
        Sprite _rankImage = m_ScoreRank[(int)_rank];

        return _rankImage;
    }

    // 랭크가 끝나면 상태를 다 돌려놔준다.
    private void InitRankPanel()
    {
        m_RankPanel.InitFadeImage();
    }
}

// Seconds값마다 WaitForSeconds인스턴스를 Dictionary에 캐싱하는 방법
// yield return new WaitForSecondsRealtime을 직접 하는것보다 훨씬 적은 가비지를 생성한다.
internal static class YieldInstructionCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSecondsRealtime> _timeInterval = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        WaitForSecondsRealtime wfs;
        if (!_timeInterval.TryGetValue(seconds, out wfs))
            _timeInterval.Add(seconds, wfs = new WaitForSecondsRealtime(seconds));
        return wfs;
    }
}