using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specification
{
    /// ����
    public int IncomeEventResult;
    public int IncomeSell;
    public int IncomeActivity;
    public int IncomeAcademyFee;   // �п� ��ŷ�� ���� �п���(�����ݾ� * 18)

    /// ����
    public int ExpensesEventResult;
    public int ExpensesEventCost;
    public int ExpensesActivity;
    public int ExpensesSalary;
    public int ExpensesFacility;
    public int ExpensesTuitionFee;

    public int TotalIncome;
    public int TotalExpenses;
    public int NetProfit;

    /// �÷��� ����
    public int GoodsScore;                  // ��ȭ����
    public int FamousScore;                 // ��������
    public int ActivityScore;               // Ȱ������
    public int ManagementScore;             // �����
    public int TalentDevelopmentScore;      // ����缺
}

/// <summary>
/// ��������� �б⺸�� �ʿ��� ���Ͱ� ����, �÷��������� �����صδ� �̱��� Ŭ����
/// �ش��ϴ� ������ �� ������ �� Ŭ������ �ҷ� �ȿ� �ִ� ����ü �� �ش��ϴ� ���� ������ ������ �����Ѵ�.
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

    [SerializeField] private Alarm m_Alarm;                                 // �л��� �п��� ��� �� �� �޼����� �������Ѵ�.
    public Specification m_NowMonth = new Specification();                  // �̹��� ���԰� ���⿡ ���� 
    public Specification m_PrevMonth = new Specification();                 // ������ ���԰� ���⿡ ���� 
    public Specification m_PrevQuarter = new Specification();
    public Specification m_NowQuarter = new Specification();                // �б⺸��

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
    private List<IncreaseFee> m_IncreaseFeeList = new List<IncreaseFee>();  // ��ī���� ��޿� ���� �����
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

                m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount.ToString() + "���� ������ " + (m_AcademyFee * m_StudentCount).ToString() + "���� ���Խ��ϴ�.");
            }
        }

        // ���� ���� ������ֱ�
        if (m_CheckMonthForProfessorFee != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 4 && GameTime.Instance.FlowTime.NowDay == 5)
        {
            m_CheckMonthForProfessorFee = GameTime.Instance.FlowTime.NowMonth;

            int totalSalary = Professor.Instance.CalculateProfessorSalary();

            PlayerInfo.Instance.MyMoney -= totalSalary;
            m_NowMonth.ExpensesSalary += totalSalary;

            //m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount.ToString() + "���� ���� " + (m_AcademyFee * m_StudentCount).ToString() + "���� �������ϴ�.");
        }

        if (!m_IsChangeMonth && m_MonthCheck != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
        {
            /// �̹��� ������ �����޿� �Ѱ��ֱ� ���� ��ȭ��������ؼ� �ѱ��
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

            /// 3������ Quarter�� ������ٰ� ����ֱ�
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

    // ��޿� ���� �п��� ��� ������ �ʱ�ȭ ���ش�.
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

    // ��޿� ���� �п��� ��� ������ ã�� �Լ�
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


