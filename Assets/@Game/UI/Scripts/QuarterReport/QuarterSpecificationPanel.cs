using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuarterSpecificationPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_QuarterTitleText;
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private Button m_NextButton;
    [SerializeField] private GameObject m_QuarterSpecificationObj;
    [SerializeField] private PopOffUI m_QuarterSpecificationPanel;

    [Space(5f)]
    [Header("수입관련 Text")]
    [SerializeField] private TextMeshProUGUI m_TotalEventResult;
    [SerializeField] private TextMeshProUGUI m_TotalSell;
    [SerializeField] private TextMeshProUGUI m_TotalActivityIncome;
    [SerializeField] private TextMeshProUGUI m_TotalAcademyFee;
    [SerializeField] private TextMeshProUGUI m_TotalIncome;

    [Space(5f)]
    [Header("지출관련 Text")]
    [SerializeField] private TextMeshProUGUI m_TotalExpensesEventResult;
    [SerializeField] private TextMeshProUGUI m_TotalExpensesEventCost;
    [SerializeField] private TextMeshProUGUI m_TotalExpensesActivity;
    [SerializeField] private TextMeshProUGUI m_TotalExpensesSalary;
    [SerializeField] private TextMeshProUGUI m_TotalExpensesFacitiy;
    [SerializeField] private TextMeshProUGUI m_TotalExpensesTuitionFee;
    [SerializeField] private TextMeshProUGUI m_TotalExpenses;

    [Space(5f)]
    [Header("순수익")]
    [SerializeField] private TextMeshProUGUI m_TotalNetProfit;

    [Space(5f)]
    [Header("분기별 점수관련 오브젝트들")]
    [SerializeField] private GameObject m_QuarterScoreObj;
    [SerializeField] private Slider m_1stQuarter;
    [SerializeField] private Slider m_2ndQuarter;
    [SerializeField] private Slider m_3rdQuarter;
    [SerializeField] private Slider m_4thQuarter;
    [SerializeField] private Image m_1stQuarterImage;
    [SerializeField] private Image m_2ndQuarterImage;
    [SerializeField] private Image m_3rdQuarterImage;
    [SerializeField] private Image m_4thQuarterImage;
    [SerializeField] private TextMeshProUGUI m_1stQuarterScore;
    [SerializeField] private TextMeshProUGUI m_2ndQuarterScore;
    [SerializeField] private TextMeshProUGUI m_3rdQuarterScore;
    [SerializeField] private TextMeshProUGUI m_4thQuarterScore;
    [SerializeField] private TextMeshProUGUI m_GoodsScore;
    [SerializeField] private TextMeshProUGUI m_FamousScore;
    [SerializeField] private TextMeshProUGUI m_ActivityScore;
    [SerializeField] private TextMeshProUGUI m_ManagementScore;
    [SerializeField] private TextMeshProUGUI m_TalentDevelopmentScore;

    [Space(5f)]
    [Header("분기별 랭크관련 오브젝트들")]
    [SerializeField] private GameObject m_QuarterRankObj;
    [SerializeField] private TextMeshProUGUI m_QuarterTitle;
    [SerializeField] private TextMeshProUGUI m_1stAcademyName;
    [SerializeField] private TextMeshProUGUI m_1stAcademyScore;
    [Header("분기순위에서 1위 아카데이 정보")]
    [SerializeField] private TextMeshProUGUI m_1stAcademyNameToAcademys;
    [SerializeField] private TextMeshProUGUI m_1stAcademyScoreToAcademys;
    [SerializeField] private TextMeshProUGUI m_1stAcademyScoreIncrease;
    [SerializeField] private Image m_1stAcademyScoreIncreaseIcon;
    [Header("분기순위에서 2위 아카데이 정보")]
    [SerializeField] private TextMeshProUGUI m_2ndAcademyNameToAcademys;
    [SerializeField] private TextMeshProUGUI m_2ndAcademyScoreToAcademys;
    [SerializeField] private TextMeshProUGUI m_2ndAcademyScoreIncrease;
    [SerializeField] private Image m_2ndAcademyScoreIncreaseIcon;
    [Header("분기순위에서 3위 아카데이 정보")]
    [SerializeField] private TextMeshProUGUI m_3rdAcademyNameToAcademys;
    [SerializeField] private TextMeshProUGUI m_3rdAcademyScoreToAcademys;
    [SerializeField] private TextMeshProUGUI m_3rdAcademyScoreIncrease;
    [SerializeField] private Image m_3rdAcademyScoreIncreaseIcon;
    [Space(5f)]
    [Header("분기순위 나와 AI아카데이 정보")]
    [SerializeField] private GameObject m_MyAcademyRankPalceParent;              // 내 학원이 1등을 하면 자식의 인덱스를 바꿔준다.
    [Header("AI아카데이 정보1")]
    [SerializeField] private TextMeshProUGUI m_AcademyRankToMyAcademyRankPalce1;
    [SerializeField] private TextMeshProUGUI m_AcademyNameToMyAcademyRankPalce1;
    [SerializeField] private TextMeshProUGUI m_AcademyScoreToMyAcademyRankPalce1;
    [SerializeField] private TextMeshProUGUI m_AcademyScoreIncrease1;
    [SerializeField] private Image m_AcademyScoreIncreaseIcon1;
    [Header("내 아카데이 정보")]
    [SerializeField] private TextMeshProUGUI m_MyAcademyRankToMyAcademyRankPalce;
    [SerializeField] private TextMeshProUGUI m_MyAcademyNameToMyAcademyRankPalce;
    [SerializeField] private TextMeshProUGUI m_MyAcademyScoreToMyAcademyRankPalce;
    [SerializeField] private TextMeshProUGUI m_MyAcademyScoreIncrease;
    [SerializeField] private Image m_MyAcademyScoreIncreaseIcon1;
    [Header("AI아카데이 정보2")]
    [SerializeField] private TextMeshProUGUI m_AcademyRankToMyAcademyRankPalce2;
    [SerializeField] private TextMeshProUGUI m_AcademyNameToMyAcademyRankPalce2;
    [SerializeField] private TextMeshProUGUI m_AcademyScoreToMyAcademyRankPalce2;
    [SerializeField] private TextMeshProUGUI m_AcademyScoreIncrease2;
    [SerializeField] private Image m_AcademyScoreIncreaseIcon2;
    [SerializeField] private Sprite[] m_IncreaseImage;

    public GameObject MyAcademyRankPlace { get { return m_MyAcademyRankPalceParent; } set { m_MyAcademyRankPalceParent = value; } }

    public void ChangeQuarterTitle(string _title)
    {
        m_QuarterTitleText.text = _title;
    }

    public void SetTotalIncomeText(string _totalIncome, string _totalEventResult, string _totalSell, string _totalActivityIncome, string _totalAcademyFee)
    {
        m_TotalIncome.text = _totalIncome;
        m_TotalEventResult.text = _totalEventResult;
        m_TotalSell.text = _totalSell;
        m_TotalActivityIncome.text = _totalActivityIncome;
        m_TotalAcademyFee.text = _totalAcademyFee;
    }

    public void SetTotalExpensesText(string _totalExpenses, string _totalExpensesEventResult, string _totalExpensesEventCost, string _totalExpensesActivity,
        string _totalExpensesSalary, string _totalExpensesFacitiy, string _totalExpensesTuituinFee)
    {
        m_TotalExpenses.text = _totalExpenses;
        m_TotalExpensesEventResult.text = _totalExpensesEventResult;
        m_TotalExpensesEventCost.text = _totalExpensesEventCost;
        m_TotalExpensesActivity.text = _totalExpensesActivity;
        m_TotalExpensesSalary.text = _totalExpensesSalary;
        m_TotalExpensesFacitiy.text = _totalExpensesFacitiy;
        m_TotalExpensesTuitionFee.text = _totalExpensesTuituinFee;
    }

    public void SetNetProfitText(string _totalNetProfit)
    {
        m_TotalNetProfit.text = _totalNetProfit;
    }

    public void Set1stQuarterScore(string _score, int _barScore)
    {
        m_1stQuarterScore.text = _score;
        m_1stQuarter.value = _barScore;
        m_1stQuarterImage.color = Color.red;
        m_4thQuarterImage.color = Color.blue;
    }

    public void Set2ndQuarterScore(string _score, int _barScore)
    {
        m_2ndQuarterScore.text = _score;
        m_2ndQuarter.value = _barScore;
        m_2ndQuarterImage.color = Color.red;
        m_1stQuarterImage.color = Color.blue;
    }

    public void Set3rdQuarterScore(string _score, int _barScore)
    {
        m_3rdQuarterScore.text = _score;
        m_3rdQuarter.value = _barScore;
        m_3rdQuarterImage.color = Color.red;
        m_2ndQuarterImage.color = Color.blue;
    }

    public void Set4thQuarterScore(string _score, int _barScore)
    {
        m_4thQuarterScore.text = _score;
        m_4thQuarter.value = _barScore;
        m_4thQuarterImage.color = Color.red;
        m_3rdQuarterImage.color = Color.blue;
    }

    public void SetPlayScore(string _goodsScore, string _managementScore, string _famousScore, string _activityScore, string _talentDevelopmentScore)
    {
        m_GoodsScore.text = _goodsScore;
        m_ManagementScore.text = _managementScore;
        m_FamousScore.text = _famousScore;
        m_ActivityScore.text = _activityScore;
        m_TalentDevelopmentScore.text = _talentDevelopmentScore;
    }

    // 분기순위 제목(분기마다 숫자를 바꿔서 띄워줘야한다.)
    public void SetQuarterRankTitle(string _rankScore)
    {
        m_QuarterTitle.text = _rankScore;
    }

    // 이번 분기에 1등한 학원의 정보를 넣어주는 함수
    public void SetQuarter1stAcademyInfo(string _academyName, string _score)
    {
        m_1stAcademyName.text = _academyName;
        m_1stAcademyScore.text = _score;
    }

    public void SetIncreaseImage(int _increase1, int _increase2, int _increase3, int _myacademy, int _other1, int _other2)
    {
        if(_increase1 > 0 || _increase2 > 0 || _increase3 > 0 || _myacademy > 0 || _other1 > 0 || _other2 > 0)
        {
            m_1stAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[0];
            m_2ndAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[0];
            m_3rdAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[0];
            m_MyAcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[0];
            m_AcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[0];
            m_AcademyScoreIncreaseIcon2.sprite = m_IncreaseImage[0];
        }
        else if(_increase1 < 0 || _increase2 < 0 || _increase3 < 0 || _myacademy < 0 || _other1 < 0 || _other2 < 0)
        {
            m_1stAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[1];
            m_2ndAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[1];
            m_3rdAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[1];
            m_MyAcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[1];
            m_AcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[1];
            m_AcademyScoreIncreaseIcon2.sprite = m_IncreaseImage[1];
        }
        else
        {
            m_1stAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[2];
            m_2ndAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[2];
            m_3rdAcademyScoreIncreaseIcon.sprite = m_IncreaseImage[2];
            m_MyAcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[2];
            m_AcademyScoreIncreaseIcon1.sprite = m_IncreaseImage[2];
            m_AcademyScoreIncreaseIcon2.sprite = m_IncreaseImage[2];
        }
    }

    // 이번 분기에 1~3등한 학원들의 정보를 넣어주는 함수
    public void SetAcademysPanel(string _1stAcademyName, string _1stAcademyScore, string _1stAcademyScoreIncrease, string _2ndAcademyName, string _2ndAcademyScore, string _2ndAcademyScoreIncrease,
        string _3rdAcademyName, string _3rdAcademyScore, string _3rdAcademyScoreIncrease)
    {
        m_1stAcademyNameToAcademys.text = _1stAcademyName;
        m_1stAcademyScoreToAcademys.text = _1stAcademyScore;
        m_1stAcademyScoreIncrease.text = _1stAcademyScoreIncrease;

        m_2ndAcademyNameToAcademys.text = _2ndAcademyName;
        m_2ndAcademyScoreToAcademys.text = _2ndAcademyScore;
        m_2ndAcademyScoreIncrease.text = _2ndAcademyScoreIncrease;

        m_3rdAcademyNameToAcademys.text = _3rdAcademyName;
        m_3rdAcademyScoreToAcademys.text = _3rdAcademyScore;
        m_3rdAcademyScoreIncrease.text = _3rdAcademyScoreIncrease;
    }

    // 내 학원의 점수와 AI들의 점수를 비교해서 띄워주기
    public void SetMyAcademyRankPalce(string _academy1Name, string _academy1Score, string _academy1Rank, string _academy1IncreaseScore,
        string _myAcademyName, string _myAcademyScore, string _myAcademyRank, string _myAcademyIncreaseScore,
        string _academy2Name, string _academy2Score, string _academy2Rank, string _academy2IncreaseScore)
    {
        m_AcademyRankToMyAcademyRankPalce1.text = _academy1Rank;
        m_AcademyNameToMyAcademyRankPalce1.text = _academy1Name;
        m_AcademyScoreToMyAcademyRankPalce1.text = _academy1Score;
        m_AcademyScoreIncrease1.text = _academy1IncreaseScore;

        m_MyAcademyRankToMyAcademyRankPalce.text = _myAcademyRank;
        m_MyAcademyNameToMyAcademyRankPalce.text = _myAcademyName;
        m_MyAcademyScoreToMyAcademyRankPalce.text = _myAcademyScore;
        m_MyAcademyScoreIncrease.text = _myAcademyIncreaseScore;

        m_AcademyRankToMyAcademyRankPalce2.text = _academy2Rank;
        m_AcademyNameToMyAcademyRankPalce2.text = _academy2Name;
        m_AcademyScoreToMyAcademyRankPalce2.text = _academy2Score;
        m_AcademyScoreIncrease2.text = _academy2IncreaseScore;


    }

    public void ClickNextButton()
    {
        if (m_QuarterSpecificationObj.activeSelf)
        {
            m_QuarterSpecificationObj.SetActive(false);
            m_QuarterScoreObj.SetActive(true);
        }
        else if (m_QuarterScoreObj.activeSelf)
        {
            m_QuarterScoreObj.SetActive(false);
            m_QuarterRankObj.SetActive(true);
        }
    }

    public void ClickPrevButton()
    {
        if (m_QuarterRankObj.activeSelf)
        {
            m_QuarterRankObj.SetActive(false);
            m_QuarterScoreObj.SetActive(true);
        }
        else if (m_QuarterScoreObj.activeSelf)
        {
            m_QuarterScoreObj.SetActive(false);
            m_QuarterSpecificationObj.SetActive(true);
        }
    }

    public void ClickCloseButton()
    {
        m_QuarterSpecificationPanel.TurnOffUI();

        m_QuarterSpecificationObj.SetActive(true);
        m_QuarterScoreObj.SetActive(false);
        m_QuarterRankObj.SetActive(false);
    }
}
