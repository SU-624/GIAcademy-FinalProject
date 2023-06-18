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

// 메일들의 정보를 담은 클래스
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

    private GameObject m_CurrentButton;                                 // 현재 내가 누른 버튼을 담아 둘 변수(신규 메일, 전체 메일 버튼 중)
    private GameObject m_CurrentReadMail;                               // 내가 어떤 메일을 읽으려고 선택했는지 담아둘 변수
    private List<MailBox> m_SentMailThisMonth = new List<MailBox>();    // 이번달 보낼 메일들을 잠시 담아두는 리스트
    private List<MailBox> m_MailList = new List<MailBox>();             // 여태껏 발송된 모든 메일을 담아둘 리스트
    public List<MailSender> m_MailSender = new List<MailSender>();      // 발송한 사람의 ID에 맞는 이름이 들어있다. 

    private int _currentReadMailButtonIndex;                            // 내가 선택한 메일이 m_MailBoxContent의 몇 번째 자식으로 있는지 담아주는 변수
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

    // AllInOneData에 값 저장하기
    public void CollectMailList()
    {
        AllInOneData.Instance.CollectMailData(m_MailList);
    }

    // AllInONeData에서 값 가져오기
    public void DistributeSendMailData()
    {
        // 전체 저장된 메일 정보 순회
        foreach (var nowSendMailData in AllInOneData.Instance.SendMailData)
        {
            Specification specification = new Specification();
            MailBox sendMail = new MailBox();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var setSpecificationFields = typeof(Specification).GetFields(flags);
            var dataFields = typeof(SendMailSaveData).GetProperties(flags);

            // specification 먼저 넣어주지
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

            // MailBox 넣어주기
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
        m_MailSender.Add(new MailSender(1, "정보수집가"));
        m_MailSender.Add(new MailSender(2, "트렌드게임지"));
        m_MailSender.Add(new MailSender(3, "게임메타"));
        m_MailSender.Add(new MailSender(100, "청소년게이머박과장"));
        m_MailSender.Add(new MailSender(101, "소도서관"));
        m_MailSender.Add(new MailSender(102, "옥수수몬스터"));
        m_MailSender.Add(new MailSender(103, "게임홀릭"));
        m_MailSender.Add(new MailSender(104, "게임소식지"));
        m_MailSender.Add(new MailSender(105, "누리넷"));
        m_MailSender.Add(new MailSender(200, "소식통"));
        m_MailSender.Add(new MailSender(201, "우왕굿"));
        m_MailSender.Add(new MailSender(202, "녹두로만든우유"));
        m_MailSender.Add(new MailSender(203, "학원잡"));
        m_MailSender.Add(new MailSender(300, "익명"));
        m_MailSender.Add(new MailSender(301, "블라블라"));
        m_MailSender.Add(new MailSender(302, "회사인"));
        m_MailSender.Add(new MailSender(400, "최고의게임만들기단"));
        m_MailSender.Add(new MailSender(500, "게임교육협회"));
    }

    // 보낸 사람을 ID값으로 찾아주는 함수
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

    // 보상이 있는 메일에서 수령하기 버튼을 눌렀을 때 보상만큼 재화가 늘고 수령 버튼을 꺼줘야한다.
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
        MailComposition.m_MonthReportMailContent = _monthlyReport;      // 월간 보고 만들 때만 값을 넣어준다.
        MailComposition.m_Month = _month;
        m_MailPanel.SetReceiptButton(_rewardButton);

        m_SentMailThisMonth.Add(MailComposition);
    }

    // 월간 보고는 매 달 해줘야 한다.
    private void MonthlyReportMail()
    {
        string _monthlyReportTitle = PlayerInfo.Instance.m_AcademyName + " " + (GameTime.Instance.FlowTime.NowMonth - 1).ToString() + "월 월간보고";

        if (GameTime.Instance.FlowTime.NowMonth == 1)
        {
            _monthlyReportTitle = PlayerInfo.Instance.m_AcademyName + " " + "1월 월간보고";
        }

        string _monthlyReportDate = GameTime.Instance.FlowTime.NowYear.ToString() + "년 " + GameTime.Instance.FlowTime.NowMonth.ToString() + "월 " + "1일";
        string _month = (GameTime.Instance.FlowTime.NowMonth - 1).ToString();
        string _sender = "운영사무국";
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

    // 보낼 메일들의 프리팹 상태를 셋팅해준다.
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
            m_Mail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";
        }

        m_Mail.GetComponent<MailPrefab>().m_MailFrom.text = m_SentMailThisMonth[_index].m_FromMail;
        m_Mail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
        m_BulbIcon.SetActive(true);

        m_MailList.Add(m_SentMailThisMonth[_index]);
        m_SentMailThisMonth.RemoveAt(_index);
    }

    // 메일의 읽기 버튼을 눌렀을 때 메일의 타입별로 셋팅을 나눠준다.
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

    // 보상이 있는 메일일 때
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

    // 월간 보고일 때
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

    // 인게임 화면에서 메일함을 눌렀을 때 새로운 메일이 없다면 메세지를 띄워줘야한다.
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

    // 신규 메일 버튼이나 전체 메일 버튼을 눌렀을 때 누른 버튼에 맞게 메일함을 셋팅해줘야한다.
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
                        m_NewMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";
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
                    m_AllMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";
                }

                m_AllMail.GetComponent<MailPrefab>().m_MailFrom.text = m_MailList[i].m_FromMail;
                m_AllMail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
            }

            m_MailPanel.SetNotNewMailPanel(false);
        }
    }

    // 닫기 버튼을 누르면 방금 읽은 메일을 삭제시켜주고 신규 메일이 없다면 신규메일 없음 띄워주기
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

    // 뒤로가기 버튼을 눌렀을 때
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
