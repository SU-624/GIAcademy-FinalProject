using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// G.I ������ ����ϴ� UI
/// 
/// �� ���� UI �������� ��Ʈ�� �ϴ� Ŭ����
/// �� ���� �����տ��� �����ϴ� �� ���� ��ũ��Ʈ�� �־�� �Ѵ�.
/// </summary>
public class Mail_Panel : MonoBehaviour
{
    [SerializeField] private Button m_NewMailButton;            // �űԸ��� ��ư
    [SerializeField] private Image m_AllMailButtonImage;        // ��ü���� ��ư �̹���
    [SerializeField] private Image m_NewMailButtonImage;        // �űԸ��� ��ư �̹���
    [SerializeField] private GameObject m_Button;               // ó�� �������� ������ ������ �űԸ��Ϸ� ������ֱ� ���� ������Ʈ
    [SerializeField] private GameObject m_ReadMailContent;      // ���� ������ �ִ� ������Ʈ. ���� ������� �־�����Ѵ�.
    [SerializeField] private GameObject m_ReadMonthReport;      // �������� ������ �ִ� ������Ʈ.
    [SerializeField] private ScrollRect m_MonthlyReportRect;
    [SerializeField] private Transform m_MailBox;               // �߼۵� ���ϵ��� ��� ������ �� �������ִ� Transform
    [SerializeField] private GameObject m_NotNewMail;           // ���ο� ������ ������ "�űԸ����� �����ϴ�"�� ����ֱ� ���� ������Ʈ
    [SerializeField] private GameObject m_Filter;               // Ÿ�Ժ��� ������ �� ���ְ� �ϱ����� ������Ʈ
    [SerializeField] private PopOffUI m_CloseMailPanel;
    [SerializeField] private Button m_BackButton;

    /// ReadMailContent
    [Space(5f)]
    [Header("�Ϲ� ���� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI m_MailTitle;
    public TextMeshProUGUI m_FromMail;
    public TextMeshProUGUI m_MainContent;
    public GameObject m_Reward1Name;
    public GameObject m_Reward2Name;
    public TextMeshProUGUI m_Reward1;
    public TextMeshProUGUI m_Reward2;

    public Image m_MainSprite;
    public Button m_ReceiptButton;

    /// ReadMonthReport
    [Space(5f)]
    [Header("�������� ���� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI m_ReportMailTitle;
    public TextMeshProUGUI m_FromReportMail;
    public TextMeshProUGUI m_DateReportMail;
    public TextMeshProUGUI m_ContentDateReportMail;

    [Space(5f)]
    [Header("����")]
    public TextMeshProUGUI m_IncomeEventResult;
    public TextMeshProUGUI m_IncomeSell;
    public TextMeshProUGUI m_IncomeActivityIncome;
    public TextMeshProUGUI m_IncomeAcademyFee;

    [Space(5f)]
    [Header("����")]
    public TextMeshProUGUI m_ExpensesEventResult;
    public TextMeshProUGUI m_ExpensesEventCost;
    public TextMeshProUGUI m_ExpensesActivityExpenditure;
    public TextMeshProUGUI m_ExpensesSalary;
    public TextMeshProUGUI m_ExpensesFacility;
    public TextMeshProUGUI m_ExpensesTuitionFee;

    [Space(5f)]
    [Header("�� ����� ����")]
    public TextMeshProUGUI m_TotalIncome;
    public TextMeshProUGUI m_TotalExpenses;
    public TextMeshProUGUI m_MonthlyRevenue;

    [Space(5f)]
    [Header("�÷�������")]
    public TextMeshProUGUI m_GoodsScore;
    public TextMeshProUGUI m_FamousScore;
    public TextMeshProUGUI m_ActivityScore;
    public TextMeshProUGUI m_ManagementScore;
    public TextMeshProUGUI m_TalentDevelopmentScore;

    public GameObject ReadMailContent { get { return m_ReadMailContent; } set { m_ReadMailContent = value; } }
    public GameObject ReadMonthReport { get { return m_ReadMonthReport; } set { m_ReadMonthReport = value; } }
    public Button NewMailButton { get { return m_NewMailButton; } set { m_NewMailButton = value; } }
    public Image AllMailButtonImgae { get { return m_AllMailButtonImage; } set { m_AllMailButtonImage = value; } }
    public Image NewMailButtonImgae { get { return m_NewMailButtonImage; } set { m_NewMailButtonImage = value; } }

    public Transform MailBoxContent { get { return m_MailBox; } set { m_MailBox = value; } }

    public void SetReadMailContent(string mailTitle, string fromMail, string mailContent)
    {
        m_MailTitle.text = mailTitle;
        m_FromMail.text = fromMail;
        m_MainContent.text = mailContent;
    }

    public void SetMonthlyReportMailContnet(string monthReportMailTitle, string monthReportMailFrom, string dateReportMail,
        string incomeEventResult, string incomeSell, string incomeActivityIncome, string incomeAcademyFee,
        string expensesEventResult, string expensesEventCost, string expensesActivityExpenditure, string expensesSalary, string expensesFacility, string expensesTuitionFee,
        string goodsScore, string famousScore, string activityScore, string managementScore, string talentDevelopmentScore, string contentDateReportMail)
    {
        m_ReportMailTitle.text = monthReportMailTitle;
        m_FromReportMail.text = monthReportMailFrom;
        m_DateReportMail.text = dateReportMail;

        m_IncomeEventResult.text = incomeEventResult;
        m_IncomeSell.text = incomeSell;
        m_IncomeActivityIncome.text = incomeActivityIncome;
        m_IncomeAcademyFee.text = incomeAcademyFee;

        m_ExpensesEventResult.text = expensesEventResult;
        m_ExpensesEventCost.text = expensesEventCost;
        m_ExpensesActivityExpenditure.text = expensesActivityExpenditure;
        m_ExpensesSalary.text = expensesSalary;
        m_ExpensesFacility.text = expensesFacility;
        m_ExpensesTuitionFee.text = expensesTuitionFee;

        m_GoodsScore.text = goodsScore;
        m_FamousScore.text = famousScore;
        m_ActivityScore.text = activityScore;
        m_ManagementScore.text = managementScore;
        m_TalentDevelopmentScore.text = talentDevelopmentScore;

        m_ContentDateReportMail.text = contentDateReportMail;
    }

    public void SetMonthlyReportTotalCostMailContent(string totalIncome, string totalExpenses, string totalMonthlyRevenue)
    {
        m_TotalIncome.text = totalIncome;
        m_TotalExpenses.text = totalExpenses;
        m_MonthlyRevenue.text = totalMonthlyRevenue;
    }

    public void SetReward(bool _flag1, bool _flag2, string _reward1, string _reward2)
    {
        m_Reward1Name.SetActive(_flag1);
        m_Reward2Name.SetActive(_flag2);
        //m_Reward.SetActive(_flag);
        m_Reward1.text = _reward1;
        m_Reward2.text = _reward2;
    }

    public string GetTitleName()
    {
        return m_MailTitle.text;
    }

    public void SetActiveReadMail(bool flag)
    {
        m_ReadMailContent.SetActive(flag);
    }

    public void SetActiveReadMonthlyReportMail(bool flag)
    {
        m_ReadMonthReport.SetActive(flag);
        m_MonthlyReportRect.verticalNormalizedPosition = 1f;
    }

    public void ClickMailPanelBackButton()
    {
        m_CloseMailPanel.TurnOffUI();
    }

    public void SetNotNewMailPanel(bool _flag)
    {
        m_NotNewMail.SetActive(_flag);
    }

    public void SetReceiptButton(bool _flag)
    {
        m_ReceiptButton.gameObject.SetActive(_flag);
    }
}
