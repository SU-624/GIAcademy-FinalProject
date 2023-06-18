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
            m_Alarm.AlarmMessageQ.Enqueue(m_StudentCount .ToString() + "���� ������ " + (m_AcademyFee * m_StudentCount).ToString() + "���� ���Խ��ϴ�.");
        }

        if (!m_IsChangeMonth && m_MonthCheck != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
        {
            /// �̹��� ������ �����޿� �Ѱ��ֱ� ���� ��ȭ��������ؼ� �ѱ��
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


