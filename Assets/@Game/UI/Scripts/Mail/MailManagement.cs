using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Linq;

public enum MailType
{
    CommonMail,
    MonthReport,
    RewardMail,
}

// ���ϵ��� ������ ���� Ŭ����
public class MailBox
{
    public string m_MailTitle;
    public string m_SendMailDate;
    public string m_FromMail;
    public string m_MailContent;
    public string m_Reward1;

    public string m_Reward2;
    public string m_Month;
    public MailType m_Type;
    public Specification m_MonthReportMailContent;
    public bool m_IsNewMail;
}

public class MailManagement : MonoBehaviour
{
    [SerializeField] private Mail_Panel m_MailPanel;
    [SerializeField] private Sprite[] m_MailButtonSeletedImage;
    [SerializeField] private Sprite[] m_MailButtonImage;
    [SerializeField] private GameObject m_BulbIcon;

    private GameObject m_CurrentButton;                                 // ���� ���� ���� ��ư�� ��� �� ����(�ű� ����, ��ü ���� ��ư ��)
    private GameObject m_CurrentReadMail;                               // ���� � ������ �������� �����ߴ��� ��Ƶ� ����
    private List<MailBox> m_SentMailThisMonth = new List<MailBox>();    // �̹��� ���� ���ϵ��� ��� ��Ƶδ� ����Ʈ
    private List<MailBox> m_MailList = new List<MailBox>();             // ���²� �߼۵� ��� ������ ��Ƶ� ����Ʈ
    public List<MailSender> m_MailSender = new List<MailSender>();      // �߼��� ����� ID�� �´� �̸��� ����ִ�. 

    private int _currentReadMailButtonIndex;                            // ���� ������ ������ m_MailBoxContent�� �� ��° �ڽ����� �ִ��� ����ִ� ����
    private int m_NowDay;
    private int m_NowMonth;

    private void Start()
    {
        m_NowMonth = 0;
        m_NowDay = 1;
        InitMailSender();
        m_CurrentButton = m_MailPanel.NewMailButton.gameObject;
        m_MailPanel.m_ReceiptButton.onClick.AddListener(ClickRewardButton);
        //m_MailList = AllInOneData.Instance.SendMailData;
        DistributeSendMailData();

    }

    private void Update()
    {
        if (m_NowMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            m_SentMailThisMonth.Clear();

            if (m_NowMonth != 0 && m_NowMonth != GameTime.Instance.FlowTime.NowMonth && GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
            {
                MonthlyReportMail();
                SendMail();
            }

            for (int i = 0; i < AllOriginalJsonData.Instance.OriginalEmailData.Count; i++)
            {
                string[] _date = AllOriginalJsonData.Instance.OriginalEmailData[i].Email_date.Split("/");

                if (int.Parse(_date[0]) == GameTime.Instance.FlowTime.NowYear && int.Parse(_date[1]) == GameTime.Instance.FlowTime.NowMonth)
                {
                    string _senderName = FindSenderName(AllOriginalJsonData.Instance.OriginalEmailData[i].Email_sender_id);

                    MakeMail(AllOriginalJsonData.Instance.OriginalEmailData[i].Email_Name, AllOriginalJsonData.Instance.OriginalEmailData[i].Email_date, _senderName,
                        AllOriginalJsonData.Instance.OriginalEmailData[i].Email_script_Text, "", "", MailType.CommonMail, false, "", new Specification());
                }
            }

            m_NowMonth = GameTime.Instance.FlowTime.NowMonth;
        }

        if (m_NowDay != GameTime.Instance.FlowTime.NowDay && m_SentMailThisMonth.Count != 0)
        {
            m_NowDay = GameTime.Instance.FlowTime.NowDay;
            SendMail();
        }
    }

    // AllInOneData�� �� �����ϱ�
    public void CollectMailList()
    {
        AllInOneData.Instance.CollectMailData(m_MailList);
    }

    // AllInONeData���� �� ��������
    public void DistributeSendMailData()
    {
        // ��ü ����� ���� ���� ��ȸ
        foreach (var nowSendMailData in AllInOneData.Instance.SendMailData)
        {
            Specification specification = new Specification();
            MailBox sendMail = new MailBox();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var setSpecificationFields = typeof(Specification).GetFields(flags);
            var dataFields = typeof(SendMailSaveData).GetProperties(flags);

            // specification ���� �־�����
            foreach (var newData in setSpecificationFields)
            {
                foreach (var saveData in dataFields)
                {
                    if (newData.Name == saveData.Name)
                    {
                        var value = saveData.GetValue(nowSendMailData);
                        newData.SetValue(specification, value);
                        break;
                    }
                }
            }

            var setFields = typeof(MailBox).GetFields(flags);

            // MailBox �־��ֱ�
            foreach (var newData in setFields)
            {
                foreach (var saveData in dataFields)
                {
                    if (newData.Name == "m_" + saveData.Name)
                    {
                        var value = saveData.GetValue(nowSendMailData);
                        newData.SetValue(sendMail, value);
                        break;
                    }
                }
            }

            sendMail.m_MonthReportMailContent = specification;

            m_MailList.Add(sendMail);
        }
    }

    private void InitMailSender()
    {
        m_MailSender.Add(new MailSender(1, "����������"));
        m_MailSender.Add(new MailSender(2, "Ʈ���������"));
        m_MailSender.Add(new MailSender(3, "���Ӹ�Ÿ"));
        m_MailSender.Add(new MailSender(100, "û�ҳ���̸ӹڰ���"));
        m_MailSender.Add(new MailSender(101, "�ҵ�����"));
        m_MailSender.Add(new MailSender(102, "����������"));
        m_MailSender.Add(new MailSender(103, "����Ȧ��"));
        m_MailSender.Add(new MailSender(104, "���Ӽҽ���"));
        m_MailSender.Add(new MailSender(105, "������"));
        m_MailSender.Add(new MailSender(200, "�ҽ���"));
        m_MailSender.Add(new MailSender(201, "��ձ�"));
        m_MailSender.Add(new MailSender(202, "��ηθ������"));
        m_MailSender.Add(new MailSender(203, "�п���"));
        m_MailSender.Add(new MailSender(300, "�͸�"));
        m_MailSender.Add(new MailSender(301, "�����"));
        m_MailSender.Add(new MailSender(302, "ȸ����"));
        m_MailSender.Add(new MailSender(400, "�ְ��ǰ��Ӹ�����"));
        m_MailSender.Add(new MailSender(500, "���ӱ�����ȸ"));
    }

    // ���� ����� ID������ ã���ִ� �Լ�
    public string FindSenderName(int _senderID)
    {
        foreach (MailSender data in m_MailSender)
        {
            if (data.SenderID == _senderID)
            {
                return data.SenderName;
            }
        }

        return "";
    }

    // ������ �ִ� ���Ͽ��� �����ϱ� ��ư�� ������ �� ����ŭ ��ȭ�� �ð� ���� ��ư�� ������Ѵ�.
    private void ClickRewardButton()
    {
        m_CurrentReadMail = EventSystem.current.currentSelectedGameObject;
        _currentReadMailButtonIndex = m_CurrentReadMail.transform.parent.GetSiblingIndex();

        string m_Parent = m_CurrentReadMail.transform.parent.name;

        int _index = m_MailList.FindIndex(x => x.m_MailTitle == m_Parent);

        PlayerInfo.Instance.m_MyMoney += int.Parse(m_MailList[_index].m_Reward1);
        PlayerInfo.Instance.m_SpecialPoint += int.Parse(m_MailList[_index].m_Reward2);

        m_MailPanel.SetReceiptButton(false);
    }

    public void MakeMail(string _title, string _date, string _from, string _content, string _reward1, string _reward2, MailType _type, bool _rewardButton, string _month, Specification _monthlyReport)
    {
        MailBox MailComposition = new MailBox();
        MailComposition.m_MailTitle = _title;
        MailComposition.m_SendMailDate = _date;
        MailComposition.m_FromMail = _from;
        MailComposition.m_MailContent = _content;
        MailComposition.m_Type = _type;
        MailComposition.m_Reward1 = _reward1;
        MailComposition.m_Reward2 = _reward2;
        MailComposition.m_IsNewMail = true;
        MailComposition.m_MonthReportMailContent = _monthlyReport;      // ���� ���� ���� ���� ���� �־��ش�.
        MailComposition.m_Month = _month;
        m_MailPanel.SetReceiptButton(_rewardButton);

        m_SentMailThisMonth.Add(MailComposition);
    }

    // ���� ����� �� �� ����� �Ѵ�.
    private void MonthlyReportMail()
    {
        string _monthlyReportTitle = PlayerInfo.Instance.m_AcademyName + " " + (GameTime.Instance.FlowTime.NowMonth - 1).ToString() + "�� ��������";

        if (GameTime.Instance.FlowTime.NowMonth == 1)
        {
            _monthlyReportTitle = PlayerInfo.Instance.m_AcademyName + " " + "1�� ��������";
        }

        string _monthlyReportDate = GameTime.Instance.FlowTime.NowYear.ToString() + "�� " + GameTime.Instance.FlowTime.NowMonth.ToString() + "�� " + "1��";
        string _month = (GameTime.Instance.FlowTime.NowMonth - 1).ToString();
        string _sender = "��繫��";
        Specification _mailContent = MonthlyReporter.Instance.m_PrevMonth;

        MakeMail(_monthlyReportTitle, _monthlyReportDate, _sender, "", "", "", MailType.MonthReport, false, _month, _mailContent);
    }

    public void SendMail()
    {
        for (int i = 0; i < m_SentMailThisMonth.Count; i++)
        {
            string[] _date = m_SentMailThisMonth[i].m_SendMailDate.Split("/");

            if (m_SentMailThisMonth[i].m_Type == MailType.CommonMail)
            {
                if (int.Parse(_date[0]) == GameTime.Instance.FlowTime.NowYear &&
                    int.Parse(_date[1]) == GameTime.Instance.FlowTime.NowMonth &&
                    int.Parse(_date[2]) == GameTime.Instance.FlowTime.NowDay)
                {
                    SetMailPrefab(i, _date);
                }
            }
            else
            {
                SetMailPrefab(i, _date);
            }
        }
    }

    // ���� ���ϵ��� ������ ���¸� �������ش�.
    private void SetMailPrefab(int _index, string[] _date)
    {
        GameObject m_Mail;

        m_Mail = MailObjectPool.GetObject(m_MailPanel.MailBoxContent);

        m_Mail.transform.localPosition = new Vector3(1f, 1f, 1f);
        m_Mail.name = m_SentMailThisMonth[_index].m_MailTitle;
        m_Mail.GetComponent<MailPrefab>().m_MailTitle.text = m_SentMailThisMonth[_index].m_MailTitle;

        if (m_SentMailThisMonth[_index].m_Type == MailType.MonthReport || m_SentMailThisMonth[_index].m_Type == MailType.RewardMail)
        {
            m_Mail.GetComponent<MailPrefab>().m_MailDate.text = _date[0];
        }
        else
        {
            m_Mail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "�� " + _date[1] + "�� " + _date[2] + "��";
        }

        m_Mail.GetComponent<MailPrefab>().m_MailFrom.text = m_SentMailThisMonth[_index].m_FromMail;
        m_Mail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
        m_BulbIcon.SetActive(true);

        m_MailList.Add(m_SentMailThisMonth[_index]);
        m_SentMailThisMonth.RemoveAt(_index);
    }

    // ������ �б� ��ư�� ������ �� ������ Ÿ�Ժ��� ������ �����ش�.
    private void ReadMail()
    {
        if (m_CurrentButton.name == "New_Mail_Button")
        {
            m_CurrentReadMail = EventSystem.current.currentSelectedGameObject;
            _currentReadMailButtonIndex = m_CurrentReadMail.transform.parent.GetSiblingIndex();

            string m_Parent = m_CurrentReadMail.transform.parent.name;

            int _index = m_MailList.FindIndex(x => x.m_MailTitle == m_Parent);

            m_MailList[_index].m_IsNewMail = false;

            if (m_MailList[_index].m_Type == MailType.CommonMail)
            {
                m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail, m_MailList[_index].m_MailContent);
                m_MailPanel.SetActiveReadMail(true);
                m_MailPanel.SetReward(false, false, "", "");
            }
            else if (m_MailList[_index].m_Type == MailType.RewardMail)
            {
                m_MailPanel.SetReceiptButton(true);
                SetRewardMailContent(_index);
            }
            else
            {
                SetMonthReportMailContent(_index);
            }
        }
        else if (m_CurrentButton.name == "All_Mail_Button")
        {
            m_CurrentReadMail = EventSystem.current.currentSelectedGameObject;
            _currentReadMailButtonIndex = m_CurrentReadMail.transform.parent.GetSiblingIndex();

            string m_Parent = m_CurrentReadMail.transform.parent.name;

            int _index = m_MailList.FindIndex(x => x.m_MailTitle == m_Parent);

            if (m_MailList[_index].m_IsNewMail)
            {
                m_MailList[_index].m_IsNewMail = false;
            }

            if (m_MailList[_index].m_Type == MailType.CommonMail)
            {
                m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail, m_MailList[_index].m_MailContent);
                m_MailPanel.SetActiveReadMail(true);
                m_MailPanel.SetReward(false, false, "", "");
            }
            else if (m_MailList[_index].m_Type == MailType.RewardMail)
            {
                SetRewardMailContent(_index);
            }
            else
            {
                SetMonthReportMailContent(_index);
            }
        }
    }

    // ������ �ִ� ������ ��
    private void SetRewardMailContent(int _index)
    {
        if (m_MailList[_index].m_Reward1 != "0" && m_MailList[_index].m_Reward2 != "0")
        {
            m_MailPanel.SetReward(true, true, m_MailList[_index].m_Reward1, m_MailList[_index].m_Reward2);
        }
        else if (m_MailList[_index].m_Reward1 != "0" && m_MailList[_index].m_Reward2 == "0")
        {
            m_MailPanel.SetReward(true, false, m_MailList[_index].m_Reward1, "");
        }
        else if (m_MailList[_index].m_Reward1 == "0" && m_MailList[_index].m_Reward2 != "0")
        {
            m_MailPanel.SetReward(false, true, "", m_MailList[_index].m_Reward2);
        }
        else
        {
            m_MailPanel.SetReward(false, false, "", "");
        }

        m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail, m_MailList[_index].m_MailContent);
        m_MailPanel.SetActiveReadMail(true);
    }

    // ���� ������ ��
    private void SetMonthReportMailContent(int _index)
    {
        string incomeEventResult = "+" + m_MailList[_index].m_MonthReportMailContent.IncomeEventResult.ToString() + "G";
        string incomeSell = "+" + m_MailList[_index].m_MonthReportMailContent.IncomeSell.ToString() + "G";
        string incomeActivity = "+" + m_MailList[_index].m_MonthReportMailContent.IncomeActivity.ToString() + "G";
        string incomeAcademyFee = "+" + m_MailList[_index].m_MonthReportMailContent.IncomeAcademyFee.ToString() + "G";

        string expenseEventResult = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesEventResult.ToString() + "G";
        string expensesEventCost = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesEventCost.ToString() + "G";
        string expenseActivity = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesActivity.ToString() + "G";
        string expensesSalary = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesSalary.ToString() + "G";
        string expensesFacility = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesFacility.ToString() + "G";
        string expensesTuitionFee = "-" + m_MailList[_index].m_MonthReportMailContent.ExpensesTuitionFee.ToString() + "G";

        int totalIncome = m_MailList[_index].m_MonthReportMailContent.TotalIncome;

        int totalExpenses = m_MailList[_index].m_MonthReportMailContent.TotalExpenses;

        int netProfit = m_MailList[_index].m_MonthReportMailContent.NetProfit;

        string totalIncomeText = "+" + totalIncome.ToString() + "G";
        string totalExpensesText = "-" + totalExpenses.ToString() + "G";
        string netProfitText = totalIncome < totalExpenses ? netProfit.ToString() + "G" : "+" + netProfit.ToString() + "G";

        string goodsScore = m_MailList[_index].m_MonthReportMailContent.GoodsScore.ToString();
        string talentDevelopScore = m_MailList[_index].m_MonthReportMailContent.TalentDevelopmentScore.ToString();
        string famousScore = m_MailList[_index].m_MonthReportMailContent.FamousScore.ToString();
        string activityScore = m_MailList[_index].m_MonthReportMailContent.ActivityScore.ToString();
        string managementScore = m_MailList[_index].m_MonthReportMailContent.ManagementScore.ToString();

        string contentDate = m_MailList[_index].m_Month;

        m_MailPanel.SetMonthlyReportMailContnet(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail, m_MailList[_index].m_SendMailDate,
           incomeEventResult, incomeSell, incomeActivity, incomeAcademyFee, expenseEventResult, expensesEventCost, expenseActivity, expensesSalary,
            expensesFacility, expensesTuitionFee, goodsScore, famousScore, activityScore, managementScore, talentDevelopScore, contentDate);

        m_MailPanel.SetMonthlyReportTotalCostMailContent(totalIncomeText, totalExpensesText, netProfitText);

        m_MailPanel.SetActiveReadMonthlyReportMail(true);
    }

    // �ΰ��� ȭ�鿡�� �������� ������ �� ���ο� ������ ���ٸ� �޼����� �������Ѵ�.
    public void ClickMailPanelButton()
    {
        if (m_CurrentButton.name == "All_Mail_Button")
        {
            m_CurrentButton = m_MailPanel.NewMailButton.gameObject;
        }
        m_MailPanel.NewMailButtonImgae.sprite = m_MailButtonSeletedImage[0];
        m_MailPanel.AllMailButtonImgae.sprite = m_MailButtonImage[1];

        bool _isNewMail = false;

        foreach (MailBox mail in m_MailList)
        {
            if (mail.m_IsNewMail)
            {
                _isNewMail = true;
                break;
            }
        }

        if (!_isNewMail)
        {
            m_MailPanel.SetNotNewMailPanel(true);
        }
        else
        {
            m_MailPanel.SetNotNewMailPanel(false);
        }
    }

    // �ű� ���� ��ư�̳� ��ü ���� ��ư�� ������ �� ���� ��ư�� �°� �������� ����������Ѵ�.
    public void ClickAllMailButtonOrNewMailButton()
    {
        m_CurrentButton = EventSystem.current.currentSelectedGameObject;

        int _destroyMailCount = m_MailPanel.MailBoxContent.childCount;

        if (m_CurrentButton.name == "New_Mail_Button")
        {
            if (m_MailPanel.ReadMailContent.activeSelf || m_MailPanel.ReadMonthReport.activeSelf)
            {
                CloseMail();
                _destroyMailCount -= 1;
            }

            m_MailPanel.NewMailButtonImgae.sprite = m_MailButtonSeletedImage[0];
            m_MailPanel.AllMailButtonImgae.sprite = m_MailButtonImage[1];

            GameObject m_NewMail;

            if (_destroyMailCount != 0)
            {
                for (int i = _destroyMailCount - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailPanel.MailBoxContent.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < m_MailList.Count; i++)
            {
                if (m_MailList[i].m_IsNewMail)
                {
                    m_NewMail = MailObjectPool.GetObject(m_MailPanel.MailBoxContent);

                    m_NewMail.transform.localScale = new Vector3(1f, 1f, 1f);
                    m_NewMail.name = m_MailList[i].m_MailTitle;

                    m_NewMail.GetComponent<MailPrefab>().m_MailTitle.text = m_MailList[i].m_MailTitle;

                    if (m_MailList[i].m_Type == MailType.MonthReport || m_MailList[i].m_Type == MailType.RewardMail)
                    {
                        m_NewMail.GetComponent<MailPrefab>().m_MailDate.text = m_MailList[i].m_SendMailDate;
                    }
                    else
                    {
                        string[] _date = m_MailList[i].m_SendMailDate.Split("/");
                        m_NewMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "�� " + _date[1] + "�� " + _date[2] + "��";
                    }

                    m_NewMail.GetComponent<MailPrefab>().m_MailFrom.text = m_MailList[i].m_FromMail;
                    m_NewMail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
                }
            }

            if (m_MailPanel.MailBoxContent.childCount == 0)
            {
                m_MailPanel.SetNotNewMailPanel(true);
            }
        }
        else if (m_CurrentButton.name == "All_Mail_Button")
        {
            if (m_MailPanel.ReadMailContent.activeSelf || m_MailPanel.ReadMonthReport.activeSelf)
            {
                CloseMail();
            }

            m_MailPanel.NewMailButtonImgae.sprite = m_MailButtonImage[0];
            m_MailPanel.AllMailButtonImgae.sprite = m_MailButtonSeletedImage[1];

            GameObject m_AllMail;

            if (_destroyMailCount != 0)
            {
                for (int i = _destroyMailCount - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailPanel.MailBoxContent.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < m_MailList.Count; i++)
            {
                m_AllMail = MailObjectPool.GetObject(m_MailPanel.MailBoxContent);

                m_AllMail.transform.localScale = new Vector3(1f, 1f, 1f);
                m_AllMail.name = m_MailList[i].m_MailTitle;

                m_AllMail.GetComponent<MailPrefab>().m_MailTitle.text = m_MailList[i].m_MailTitle;

                if (m_MailList[i].m_Type == MailType.MonthReport || m_MailList[i].m_Type == MailType.RewardMail)
                {
                    m_AllMail.GetComponent<MailPrefab>().m_MailDate.text = m_MailList[i].m_SendMailDate;
                }
                else
                {
                    string[] _date = m_MailList[i].m_SendMailDate.Split("/");
                    m_AllMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "�� " + _date[1] + "�� " + _date[2] + "��";
                }

                m_AllMail.GetComponent<MailPrefab>().m_MailFrom.text = m_MailList[i].m_FromMail;
                m_AllMail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
            }

            m_MailPanel.SetNotNewMailPanel(false);
        }
    }

    // �ݱ� ��ư�� ������ ��� ���� ������ ���������ְ� �ű� ������ ���ٸ� �űԸ��� ���� ����ֱ�
    public void CloseMail()
    {
        if (m_CurrentButton.name == "New_Mail_Button")
        {
            m_MailPanel.SetActiveReadMail(false);
            m_MailPanel.SetActiveReadMonthlyReportMail(false);

            MailObjectPool.ReturnObject(m_MailPanel.MailBoxContent.GetChild(_currentReadMailButtonIndex).gameObject);

            if (m_MailPanel.MailBoxContent.childCount == 0)
            {
                m_MailPanel.SetNotNewMailPanel(true);
            }
        }
        else if (m_CurrentButton.name == "All_Mail_Button")
        {
            m_MailPanel.SetActiveReadMail(false);
            m_MailPanel.SetActiveReadMonthlyReportMail(false);
        }
    }

    // �ڷΰ��� ��ư�� ������ ��
    public void ClickBackButton()
    {
        if (m_MailPanel.ReadMailContent.activeSelf || m_MailPanel.ReadMonthReport.activeSelf)
        {
            CloseMail();
        }
        else
        {
            if (m_CurrentButton.name == "New_Mail_Button")
            {
                int _destoryMailObj = m_MailPanel.MailBoxContent.childCount;

                if (_destoryMailObj != 0)
                {
                    for (int i = 0; i < _destoryMailObj; i++)
                    {
                        MailObjectPool.ReturnObject(m_MailPanel.MailBoxContent.GetChild(i).gameObject);
                    }
                }

                m_MailPanel.SetNotNewMailPanel(true);

                foreach (MailBox mail in m_MailList)
                {
                    if (mail.m_IsNewMail)
                    {
                        mail.m_IsNewMail = false;
                    }
                }

                m_BulbIcon.SetActive(false);
            }
            else if (m_CurrentButton.name == "All_Mail_Button")
            {
                for (int i = m_MailList.Count - 1; i >= 0; i--)
                {
                    MailObjectPool.ReturnObject(m_MailPanel.MailBoxContent.GetChild(i).gameObject);
                }

                m_BulbIcon.SetActive(false);
            }

            m_MailPanel.ClickMailPanelBackButton();
        }
    }
}
