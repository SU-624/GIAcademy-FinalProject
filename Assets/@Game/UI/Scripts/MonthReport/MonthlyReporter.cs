using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specification
{
    /// 수입
    public int IncomeEventResult;
    public int IncomeSell;
    public int IncomeActivity;
    public int IncomeAcademyFee;   // 학원 랭킹에 따른 학원비(지정금액 * 18)

    /// 지출
    public int ExpensesEventResult;
    public int ExpensesEventCost;
    public int ExpensesActivity;
    public int ExpensesSalary;
    public int ExpensesFacility;
    public int ExpensesTuitionFee;

    public int TotalIncome;
    public int TotalExpenses;
    public int NetProfit;

    /// 플레이 점수
    public int GoodsScore;                  // 재화점수
    public int FamousScore;                 // 유명점수
    public int ActivityScore;               // 활동점수
    public int ManagementScore;             // 운영점수
    public int TalentDevelopmentScore;      // 인재양성
}

/// <summary>
/// 월간보고와 분기보고에 필요한 수익과 지출, 플레이점수를 저장해두는 싱글턴 클래스
/// 해당하는 행위를 한 곳에서 이 클래스를 불러 안에 있는 구조체 중 해당하는 곳에 변수에 값들을 저장한다.
/// 
/// </summary>
public class MonthlyReporter : MonoBehaviour
{
    private static MonthlyReporter _instance;

    public static MonthlyReporter Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }

    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    [SerializeField] private Alarm m_Alarm;                                 // 학생들 학원비가 들어 올 때 메세지를 띄워줘야한다.
    public Specification m_NowMonth = new Specification();                  // 이번달 수입과 지출에 관한 
    public Specification m_PrevMonth = new Specification();                 // 저번달 수입과 지출에 관한 
    public Specification m_PrevQuarter = new Specification();
    public Specification m_NowQuarter = new Specification();                // 분기보고

    private int m_QuarterCount;
    private int m_MonthCheck;
    private int m_DayCheck;
    private int m_sNetProfit;
    private int m_ANetProfit;
    private bool m_IsChangeMonth;
    private float m_AcademyFee;
    private float m_StudentCount;
    private float m_IncreaseAcademyFee;
    private int m_CheckMonthForAcademyFee;
    private int m_CheckMonthForProfessorFee;
    private List<IncreaseFee> m_IncreaseFeeList = new List<IncreaseFee>();  // 아카데미 등급에 따른 비용상승
    private int m_FirstQuarterScore;
    private int m_SecondQuarterScore;
    private int m_ThirdQuarterScore;
    private int m_FourthQuarterScore;

    public int FirstQuarterScore { get { return m_FirstQuarterScore; } set { m_FirstQuarterScore = value; } }
    public int SecondQuarterScore { get { return m_SecondQuarterScore; } set { m_SecondQuarterScore = value; } }
    public int ThirdQuarterScore { get { return m_ThirdQuarterScore; } set { m_ThirdQuarterScore = value; } }
    public int FourthQuarterScore { get { return m_FourthQuarterScore; } set { m_FourthQuarterScore = value; } }

    private void Start()
    {
        m_QuarterCount = 0;
        m_MonthCheck = 3;
        m_DayCheck = 1;
        m_IsChangeMonth = false;
        m_AcademyFee = 10000f;
        m_StudentCount = 18f;
        m_CheckMonthForAcademyFee = 0;
        m_sNetProfit = 100000;
        m_ANetProfit = 30000;
        InitIncreaseFeeList();
    }

    private void Update()
    {
        if (m_CheckMonthForAcademyFee != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowMonth != 2 && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
        {
            if (!PlayerInfo.Instance.IsFirstClassSetting)
            {
                m_CheckMonthForAcademyFee = GameTime.Instance.FlowTime.NowMonth;

                m_IncreaseAcademyFee = CheckIncreaseFee(PlayerInfo.Instance.CurrentRank);

                PlayerInfo.Instance.MyMoney += (int)(m_AcademyFee * m_StudentCount * m_IncreaseAcademyFee);

                m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount.ToString() + "명의 수업료 " + (m_AcademyFee * m_StudentCount).ToString() + "원이 들어왔습니다.");
            }
        }

        // 교사 월급 계산해주기
        if (m_CheckMonthForProfessorFee != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 4 && GameTime.Instance.FlowTime.NowDay == 5)
        {
            m_CheckMonthForProfessorFee = GameTime.Instance.FlowTime.NowMonth;

            int totalSalary = Professor.Instance.CalculateProfessorSalary();

            PlayerInfo.Instance.MyMoney -= totalSalary;
            m_NowMonth.ExpensesSalary += totalSalary;

            //m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount.ToString() + "교사 월급 " + (m_AcademyFee * m_StudentCount).ToString() + "원이 나갔습니다.");
        }

        if (!m_IsChangeMonth && m_MonthCheck != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
        {
            /// 이번달 정보를 다음달에 넘겨주기 전에 재화점수계산해서 넘기기
            m_NowMonth.IncomeAcademyFee = (int)(m_AcademyFee * m_StudentCount * m_IncreaseAcademyFee);
            m_NowMonth.TotalIncome = m_NowMonth.IncomeEventResult + m_NowMonth.IncomeActivity + m_NowMonth.IncomeSell + m_NowMonth.IncomeAcademyFee;
            m_NowMonth.TotalExpenses = m_NowMonth.ExpensesActivity + m_NowMonth.ExpensesEventCost + m_NowMonth.ExpensesEventResult + m_NowMonth.ExpensesFacility + m_NowMonth.ExpensesSalary + m_NowMonth.ExpensesTuitionFee;
            m_NowMonth.NetProfit = m_NowMonth.TotalIncome - m_NowMonth.TotalExpenses;

            if (m_NowMonth.NetProfit > m_sNetProfit)
            {
                m_NowMonth.GoodsScore = 40;

            }
            else if (m_NowMonth.NetProfit > m_ANetProfit)
            {
                m_NowMonth.GoodsScore = 20;
            }
            else if (m_NowMonth.NetProfit > 0)
            {
                m_NowMonth.GoodsScore = 5;
            }
            else
            {
                m_NowMonth.GoodsScore = 0;
            }

            PlayerInfo.Instance.Goods += m_NowMonth.GoodsScore;
            m_PrevMonth = m_NowMonth;

            /// 3개월씩 Quarter에 더해줬다가 비워주기
            CalculateQuarter(m_NowMonth);

            m_NowMonth = new Specification();

            m_MonthCheck = GameTime.Instance.FlowTime.NowMonth;
            m_IsChangeMonth = true;
        }

        if (m_DayCheck != GameTime.Instance.FlowTime.NowDay)
        {
            m_IsChangeMonth = false;
            m_DayCheck = GameTime.Instance.FlowTime.NowDay;
        }
    }

    // 등급에 따른 학원비 상승 비율을 초기화 해준다.
    private void InitIncreaseFeeList()
    {
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.SSS, 4.8f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.SS, 4.8f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.S, 3.6f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.A, 3.6f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.B, 2.4f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.C, 2.4f, 0));
        m_IncreaseFeeList.Add(new IncreaseFee(Rank.D, 1.2f, 0));
    }

    // 등급에 따른 학원비 상승 비율을 찾는 함수
    private float CheckIncreaseFee(Rank _myAcademyRank)
    {
        foreach (IncreaseFee increase in m_IncreaseFeeList)
        {
            if (increase.MyAcademyRank == _myAcademyRank)
            {
                return increase.RisingFee;
            }
        }
        return 1;
    }

    private void CalculateQuarter(Specification _specification)
    {
        m_NowQuarter.IncomeEventResult += _specification.IncomeEventResult;
        m_NowQuarter.IncomeSell += _specification.IncomeSell;
        m_NowQuarter.IncomeActivity += _specification.IncomeActivity;
        m_NowQuarter.IncomeAcademyFee += _specification.IncomeAcademyFee;

        m_NowQuarter.ExpensesActivity += _specification.ExpensesActivity;
        m_NowQuarter.ExpensesEventResult += _specification.ExpensesEventResult;
        m_NowQuarter.ExpensesEventCost += _specification.ExpensesEventCost;
        m_NowQuarter.ExpensesSalary += _specification.ExpensesSalary;
        m_NowQuarter.ExpensesFacility += _specification.ExpensesFacility;
        m_NowQuarter.ExpensesTuitionFee += _specification.ExpensesTuitionFee;

        m_NowQuarter.TotalIncome += _specification.TotalIncome;
        m_NowQuarter.TotalExpenses += _specification.TotalExpenses;
        m_NowQuarter.NetProfit += _specification.NetProfit;

        m_NowQuarter.ActivityScore += _specification.ActivityScore;
        m_NowQuarter.GoodsScore += _specification.GoodsScore;
        m_NowQuarter.FamousScore += _specification.FamousScore;
        m_NowQuarter.ManagementScore += _specification.ManagementScore;
        m_NowQuarter.TalentDevelopmentScore += _specification.TalentDevelopmentScore;

        m_QuarterCount += 1;

        if (m_QuarterCount >= 3)
        {
            if (GameTime.Instance.FlowTime.NowMonth == 6)
            {
                m_FirstQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 9)
            {
                m_SecondQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 12)
            {
                m_ThirdQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 3)
            {
                m_FourthQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }

            m_PrevQuarter = m_NowQuarter;
            m_NowQuarter = new Specification();
            m_QuarterCount = 0;
        }
    }
}


