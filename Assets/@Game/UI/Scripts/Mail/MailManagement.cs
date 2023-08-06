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
    public int[] m_SendMailDate;
    public int m_SenderID;
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
    [SerializeField] private Sprite[] m_SenderImage;
    [SerializeField] private GameObject m_BulbIcon;

    private GameObject m_CurrentButton;                                                 // 현재 내가 누른 버튼을 담아 둘 변수(신규 메일, 전체 메일 버튼 중)
    private GameObject m_CurrentReadMail;                                               // 내가 어떤 메일을 읽으려고 선택했는지 담아둘 변수
    private List<MailBox> m_SentMailThisMonth = new List<MailBox>();                    // 이번달 보낼 메일들을 잠시 담아두는 리스트
    private List<MailBox> m_MailList = new List<MailBox>();                             // 여태껏 발송된 모든 메일을 담아둘 리스트
    public List<MailSender> m_MailSender = new List<MailSender>();                      // 발송한 사람의 ID에 맞는 이름이 들어있다. 
    private List<MailSenderImage> m_MailSenderImages = new List<MailSenderImage>();     // 발송한 사람의 ID에 맞는 이미지 인덱스가 들어있다.
    private int _currentReadMailButtonIndex;                                            // 내가 선택한 메일이 m_MailBoxContent의 몇 번째 자식으로 있는지 담아주는 변수
    private int m_NowDay;
    private int m_NowMonth;

    private void Start()
    {
        m_NowMonth = 0;
        m_NowDay = 1;
        InitMailSender();
        InitMailSenderImageIndex();
        m_CurrentButton = m_MailPanel.NewMailButton.gameObject;
        m_MailPanel.m_ReceiptButton.onClick.AddListener(ClickRewardButton);

        if (Json.Instance.UseLoadingData)
        {
            // 로딩한 정보를 메일함에 넣는다.
            DistributeSendMailData();
        }
    }

    private void Update()
    {
        if (m_NowMonth != GameTime.Instance.FlowTime.NowMonth)
        {
            m_SentMailThisMonth.Clear();

            if (m_NowMonth != 0 && m_NowMonth != GameTime.Instance.FlowTime.NowMonth &&
                GameTime.Instance.FlowTime.NowWeek == 1 && GameTime.Instance.FlowTime.NowDay == 1)
            {
                MonthlyReportMail();
                SendMail();
            }

            for (int i = 0; i < AllOriginalJsonData.Instance.OriginalEmailData.Count; i++)
            {
                int[] _date = AllOriginalJsonData.Instance.OriginalEmailData[i].Email_date;

                if (_date[0] == GameTime.Instance.FlowTime.NowYear &&
                    _date[1] == GameTime.Instance.FlowTime.NowMonth)
                {
                    string _senderName =
                        FindSenderName(AllOriginalJsonData.Instance.OriginalEmailData[i].Email_sender_id);

                    MakeMail(AllOriginalJsonData.Instance.OriginalEmailData[i].Email_Name,
                        AllOriginalJsonData.Instance.OriginalEmailData[i].Email_date, AllOriginalJsonData.Instance.OriginalEmailData[i].Email_sender_id, _senderName,
                        AllOriginalJsonData.Instance.OriginalEmailData[i].Email_script_Text, "", "",
                        MailType.CommonMail, false, "", new Specification());
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
        m_MailList.Clear();

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
        m_MailSender.Add(new MailSender(1000, "정보수집가"));
        m_MailSender.Add(new MailSender(1001, "트렌드게임지"));
        m_MailSender.Add(new MailSender(1002, "게임메타"));
        m_MailSender.Add(new MailSender(2000, "청소년게이머박과장"));
        m_MailSender.Add(new MailSender(2001, "소도서관"));
        m_MailSender.Add(new MailSender(2002, "옥수수몬스터"));
        m_MailSender.Add(new MailSender(2003, "게임홀릭"));
        m_MailSender.Add(new MailSender(2004, "게임소식지"));
        m_MailSender.Add(new MailSender(2005, "누리넷"));
        m_MailSender.Add(new MailSender(3000, "소식통"));
        m_MailSender.Add(new MailSender(3001, "우왕굿"));
        m_MailSender.Add(new MailSender(3002, "녹두로만든우유"));
        m_MailSender.Add(new MailSender(3003, "학원잡"));
        m_MailSender.Add(new MailSender(4000, "익명"));
        m_MailSender.Add(new MailSender(4001, "블라블라"));
        m_MailSender.Add(new MailSender(4002, "회사인"));
        m_MailSender.Add(new MailSender(5000, "최고의게임만들기단"));
        m_MailSender.Add(new MailSender(6000, "게임교육협회"));
    }

    private void InitMailSenderImageIndex()
    {
        m_MailSenderImages.Add(new MailSenderImage(1000, 0));
        m_MailSenderImages.Add(new MailSenderImage(1001, 1));
        m_MailSenderImages.Add(new MailSenderImage(1002, 2));
        m_MailSenderImages.Add(new MailSenderImage(2000, 3));
        //m_MailSenderImages.Add(new MailSenderImage(2001, 1));
        m_MailSenderImages.Add(new MailSenderImage(2002, 4));
        //m_MailSenderImages.Add(new MailSenderImage(2003, 1));
        //m_MailSenderImages.Add(new MailSenderImage(2004, 1));
        //m_MailSenderImages.Add(new MailSenderImage(2005, 1));
        m_MailSenderImages.Add(new MailSenderImage(3000, 5));
        //m_MailSenderImages.Add(new MailSenderImage(3001, 10));
        m_MailSenderImages.Add(new MailSenderImage(3002, 6));
        //m_MailSenderImages.Add(new MailSenderImage(3003, 12));
        m_MailSenderImages.Add(new MailSenderImage(4000, 7));
        //m_MailSenderImages.Add(new MailSenderImage(4001, 14));
        //m_MailSenderImages.Add(new MailSenderImage(4002, 15));
        m_MailSenderImages.Add(new MailSenderImage(5000, 8));
        m_MailSenderImages.Add(new MailSenderImage(6000, 9));
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

        PlayerInfo.Instance.MyMoney += int.Parse(m_MailList[_index].m_Reward1);
        PlayerInfo.Instance.SpecialPoint += int.Parse(m_MailList[_index].m_Reward2);

        m_MailPanel.SetReceiptButton(false);
    }

    public void MakeMail(string _title, int[] _date, int _senderID, string _from, string _content, string _reward1, string _reward2,
        MailType _type, bool _rewardButton, string _month, Specification _monthlyReport)
    {
        MailBox MailComposition = new MailBox();
        MailComposition.m_MailTitle = _title;
        MailComposition.m_SendMailDate = _date;
        MailComposition.m_SenderID = _senderID;
        MailComposition.m_FromMail = _from;
        MailComposition.m_MailContent = _content;
        MailComposition.m_Type = _type;
        MailComposition.m_Reward1 = _reward1;
        MailComposition.m_Reward2 = _reward2;
        MailComposition.m_IsNewMail = true;
        MailComposition.m_MonthReportMailContent = _monthlyReport; // 월간 보고 만들 때만 값을 넣어준다.
        MailComposition.m_Month = _month;
        m_MailPanel.SetReceiptButton(_rewardButton);

        m_SentMailThisMonth.Add(MailComposition);
    }

    // 월간 보고는 매 달 해줘야 한다.
    private void MonthlyReportMail()
    {
        string _monthlyReportTitle = PlayerInfo.Instance.AcademyName + " " +
                                     (GameTime.Instance.FlowTime.NowMonth - 1).ToString() + "월 월간보고";

        if (GameTime.Instance.FlowTime.NowMonth == 1)
        {
            _monthlyReportTitle = PlayerInfo.Instance.AcademyName + " " + "1월 월간보고";
        }
        int[] _date = new int[4];

        _date[0] = GameTime.Instance.FlowTime.NowYear;
        _date[1] = GameTime.Instance.FlowTime.NowMonth;
        _date[3] = 1;

        string _month = (GameTime.Instance.FlowTime.NowMonth - 1).ToString();
        string _sender = "운영사무국";
        Specification _mailContent = MonthlyReporter.Instance.m_PrevMonth;

        MakeMail(_monthlyReportTitle, _date, 6000, _sender, "", "", "", MailType.MonthReport, false, _month,
            _mailContent);
    }

    public void SendMail()
    {
        for (int i = 0; i < m_SentMailThisMonth.Count; i++)
        {
            int[] _date = m_SentMailThisMonth[i].m_SendMailDate;

            if (m_SentMailThisMonth[i].m_Type == MailType.CommonMail)
            {
                if (_date[0] == GameTime.Instance.FlowTime.NowYear &&
                    _date[1] == GameTime.Instance.FlowTime.NowMonth &&
                    _date[2] == GameTime.Instance.FlowTime.NowWeek &&
                    _date[3] == GameTime.Instance.FlowTime.NowDay)
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
    private void SetMailPrefab(int _index, int[] _date)
    {
        GameObject m_Mail;

        m_Mail = MailObjectPool.GetObject(m_MailPanel.MailBoxContent);

        m_Mail.transform.localPosition = new Vector3(1f, 1f, 1f);
        m_Mail.name = m_SentMailThisMonth[_index].m_MailTitle;
        m_Mail.GetComponent<MailPrefab>().m_MailTitle.text = m_SentMailThisMonth[_index].m_MailTitle;

        m_Mail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";

        m_Mail.GetComponent<MailPrefab>().m_MailFrom.text = m_SentMailThisMonth[_index].m_FromMail;
        Sprite _senderImage = SenderImage(m_SentMailThisMonth[_index].m_SenderID);
        m_Mail.GetComponent<MailPrefab>().m_MailIcon.sprite = _senderImage;
        m_Mail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
        m_BulbIcon.SetActive(true);

        m_MailList.Add(m_SentMailThisMonth[_index]);
        m_SentMailThisMonth.RemoveAt(_index);
    }

    private Sprite SenderImage(int _senderID)
    {
        foreach (MailSenderImage image in m_MailSenderImages)
        {
            if (image.SenderID == _senderID)
            {
                return m_SenderImage[image.Spriteindex];
            }
        }

        return null;
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
                m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail,
                    m_MailList[_index].m_MailContent);
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
                m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail,
                    m_MailList[_index].m_MailContent);
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

        m_MailPanel.SetBackButtonActive(true);
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

        m_MailPanel.SetReadMailContent(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail,
            m_MailList[_index].m_MailContent);
        m_MailPanel.SetActiveReadMail(true);
    }

    // 월간 보고일 때
    private void SetMonthReportMailContent(int _index)
    {
        string incomeEventResult = "+" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.IncomeEventResult) + "G";
        string incomeSell = "+" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.IncomeSell) + "G";
        string incomeActivity = "+" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.IncomeActivity) + "G";
        string incomeAcademyFee = "+" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.IncomeAcademyFee) + "G";

        string expenseEventResult = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesEventResult) + "G";
        string expensesEventCost = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesEventCost) + "G";
        string expenseActivity = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesActivity) + "G";
        string expensesSalary = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesSalary) + "G";
        string expensesFacility = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesFacility) + "G";
        string expensesTuitionFee = "-" + string.Format("{0:#,0}", m_MailList[_index].m_MonthReportMailContent.ExpensesTuitionFee) + "G";

        int totalIncome = m_MailList[_index].m_MonthReportMailContent.TotalIncome;

        int totalExpenses = m_MailList[_index].m_MonthReportMailContent.TotalExpenses;

        int netProfit = m_MailList[_index].m_MonthReportMailContent.NetProfit;

        string totalIncomeText = "+" + string.Format("{0:#,0}", totalIncome) + "G";
        string totalExpensesText = "-" + string.Format("{0:#,0}", totalExpenses) + "G";
        string netProfitText =
            totalIncome < totalExpenses ? string.Format("{0:#,0}", netProfit) + "G" : "+" + string.Format("{0:#,0}", netProfit) + "G";

        string goodsScore = m_MailList[_index].m_MonthReportMailContent.GoodsScore.ToString();
        string talentDevelopScore = m_MailList[_index].m_MonthReportMailContent.TalentDevelopmentScore.ToString();
        string famousScore = m_MailList[_index].m_MonthReportMailContent.FamousScore.ToString();
        string activityScore = m_MailList[_index].m_MonthReportMailContent.ActivityScore.ToString();
        string managementScore = m_MailList[_index].m_MonthReportMailContent.ManagementScore.ToString();

        string contentDate = m_MailList[_index].m_Month;
        string date = m_MailList[_index].m_SendMailDate[0].ToString() + "년" + m_MailList[_index].m_SendMailDate[1].ToString() + "월" + m_MailList[_index].m_SendMailDate[3].ToString() + "일";

        m_MailPanel.SetMonthlyReportMailContnet(m_MailList[_index].m_MailTitle, m_MailList[_index].m_FromMail,
            date, incomeEventResult, incomeSell, incomeActivity, incomeAcademyFee, expenseEventResult, expensesEventCost,
            expenseActivity, expensesSalary, expensesFacility, expensesTuitionFee, goodsScore, famousScore, activityScore, managementScore,
            talentDevelopScore, contentDate);

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
                ClickBackButton();
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

                    int[] _date = m_MailList[i].m_SendMailDate;
                    m_NewMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";

                    Sprite _senderImage = SenderImage(m_MailList[i].m_SenderID);
                    m_NewMail.GetComponent<MailPrefab>().m_MailIcon.sprite = _senderImage;
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
                ClickBackButton();
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

                int[] _date = m_MailList[i].m_SendMailDate;
                m_AllMail.GetComponent<MailPrefab>().m_MailDate.text = _date[0] + "년 " + _date[1] + "월 " + _date[2] + "일";
                Sprite _senderImage = SenderImage(m_MailList[i].m_SenderID);
                m_AllMail.GetComponent<MailPrefab>().m_MailIcon.sprite = _senderImage;

                m_AllMail.GetComponent<MailPrefab>().m_MailFrom.text = m_MailList[i].m_FromMail;
                m_AllMail.GetComponent<MailPrefab>().m_ReadButton.onClick.AddListener(ReadMail);
            }

            m_MailPanel.SetNotNewMailPanel(false);
        }
    }

    public void CloseMail()
    {
        if (m_MailPanel.ReadMailContent.activeSelf || m_MailPanel.ReadMonthReport.activeSelf)
        {
            m_MailPanel.SetActiveReadMail(false);
            m_MailPanel.SetActiveReadMonthlyReportMail(false);
        }

        if (m_CurrentButton.name == "New_Mail_Button")
        {
            int _destoryMailObj = m_MailPanel.MailBoxContent.childCount;

            if (_destoryMailObj != 0)
            {
                for (int i = _destoryMailObj - 1; i >= 0; i--)
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

        m_MailPanel.ClickMailPanelCloseButton();
    }

    public void ClickBackButton()
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

        m_MailPanel.SetBackButtonActive(false);
    }
}