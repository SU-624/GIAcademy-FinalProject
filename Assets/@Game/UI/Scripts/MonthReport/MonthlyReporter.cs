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
    private int m_AcademyFee;
    private int m_StudentCount;
    private int m_CheckMonthForAcademyFee;

    public int m_1stQuarterScore;
    public int m_2ndQuarterScore;
    public int m_3rdQuarterScore;
    public int m_4thQuarterScore;

    private void Start()
    {
        m_QuarterCount = 0;
        m_MonthCheck = 3;
        m_DayCheck = 1;
        m_IsChangeMonth = false;
        m_AcademyFee = 30000;
        m_StudentCount = 18;
        m_CheckMonthForAcademyFee = 0;
        m_sNetProfit = 100000;
        m_ANetProfit = 30000;
    }

    private void Update()
    {
        if (m_CheckMonthForAcademyFee != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowMonth != 2)
        {
            m_CheckMonthForAcademyFee = GameTime.Instance.FlowTime.NowMonth;

            PlayerInfo.Instance.m_MyMoney += (m_AcademyFee * m_StudentCount);
            PlayerInfo.Instance.m_MyMoney -= InGameTest.Instance.m_ProfessorTotalSalary;
            m_StudentCount = ObjectManager.Instance.m_StudentList.Count;
            m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount .ToString() + "명의 수업료 " + (m_AcademyFee * m_StudentCount).ToString() + "원이 들어왔습니다.");
        }

        if (!m_IsChangeMonth && m_MonthCheck != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
        {
            /// 이번달 정보를 다음달에 넘겨주기 전에 재화점수계산해서 넘기기
            m_NowMonth.IncomeAcademyFee = m_AcademyFee * m_StudentCount;
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

            PlayerInfo.Instance.m_Goods += m_NowMonth.GoodsScore;
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
                m_1stQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 9)
            {
                m_2ndQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 12)
            {
                m_3rdQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }
            else if (GameTime.Instance.FlowTime.NowMonth == 3)
            {
                m_4thQuarterScore = m_NowQuarter.ActivityScore + m_NowQuarter.FamousScore + m_NowQuarter.GoodsScore + m_NowQuarter.ManagementScore + m_NowQuarter.TalentDevelopmentScore;
            }

            m_PrevQuarter = m_NowQuarter;
            m_NowQuarter = new Specification();
            m_QuarterCount = 0;
        }
    }
}


