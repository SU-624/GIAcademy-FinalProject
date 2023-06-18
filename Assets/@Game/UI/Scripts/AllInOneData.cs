using System.Collections.Generic;
using StatData.Runtime;

/// <summary>
///  Mang 23. 02. 08
/// 
/// Json ȭ �� ������ �����
/// ���⿡ �����͵��� �� �־�θ� �������� ����������Ұ� ��
///
/// woodpie 23. 05. 11
/// �����͸� json ȭ �ؼ� ���� �����ϰ�, ���̾���� �����ϱ� ���� �籸����
/// NewtoneJson�� ����� ���� monobehabear�� ��ӹ��� ����.
///
/// �����͸� ������ �ڵ�� Collect
/// �����͸� ������ �����ϴ� �ڵ忡�� Save�� ���δ�.
///
/// AllInOneData �� ��� ����
/// 1. ������ ������ �� �̹� ����� �����Ͱ� �ִٸ� �� ������ �о��
/// 1.1 �о�� �����͸� �ΰ��ӿ� �����Ŵ.
/// 1.2 �ΰ��ӿ� ������ �ð�, ��Ȳ, ������ ����
/// 2. �ڵ� Ȥ�� ���� ������ �� ���� ���� ������ִ� �����͵��� ����
/// 2.1 json ���Ͽ� ����
/// 2.2 ���̾�� ����
/// 3. ���� �α��� ���� �����͸� �ҷ��� �� ���� �������� ������ �о��.
/// 1,1 / 1.2 �ݺ�
///
/// �ΰ��ӿ��� ����ϴ� Ŭ������ �ƴ϶� ������ Ŭ������ ���� ���� �۾��ϴ� ����!
/// 1. NewtonJson�� MonoBehaviour ���� Ŭ������ �Ľ����� ���� ������ Ŭ������ �ʿ�
/// 2. �����ϰ� �ҷ��� �ʿ��� �����͸� ��Ƽ� �ѹ��� ���� �;���.
/// 3. ���̾�̽��� ������ ���Ŀ� ������ ����� ���� (�� ������ �׽�Ʈ�غ��� ��)
/// </summary>
///
public class AllInOneData
{
    private static AllInOneData instance = null;

    public static AllInOneData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AllInOneData();
            }

            return instance;
        }

        set { instance = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Class
    public PlayerSaveData PlayerData = new(); // �÷��̾��� �ΰ��� ����
    public List<StudentSaveData> StudentData = new(); // ���� ����ϴ� �л� ����
    public List<ProfessorSaveData> ProfessorData = new(); // ���� ����ϴ� ���� ����
    public List<UsedEventSaveData> SuddenEventData = new(); // ����� ���� �̺�Ʈ ����
    public List<TodayEventSaveData> TodaySuddenEventData = new(); // �ҷ��� �� �����ϴ� ���� �̺�Ʈ ID

    public List<SendMailSaveData> SendMailData = new(); // ��� ���� ����
    public List<SaveGameJamData> EndGameJamData = new(); // �Ϸ��� ������ ����Ÿ
    


    // �ε� ���θ� �Ǻ��ϴ� ����
    public bool ServerLoading = false;
    
    
    
    
    // //public InGameSaveData InGameData = new InGameSaveData(); // �ΰ��ӿ� �ʿ��� �����͵�

    // public List<GameJamInfo> GameJamData = new List<GameJamInfo>();
    // public List<InJaeRecommendData> _InJaeRecommendData = new List<InJaeRecommendData>();
    // public List<RankScript> RankScripteData = new List<RankScript>();
    // public List<EmailData> _EmailData = new List<EmailData>();
    public List<GameShowData> _GameShowData = new(); // �׽�Ʈ��

    // Data
    // ģ�е� �����ʹ� �л��� ���� ����ֵ��� �����߽��ϴ�.
    //public List<List<int>> Friendship = new List<List<int>>(); // ��� �л��� ������ ģ�е� (�л�18 ����3)

    
    
    

    private void CleanAllGameData()
    {
        PlayerData = new PlayerSaveData();
        StudentData.Clear();
        ProfessorData.Clear();
        SuddenEventData.Clear();
        TodaySuddenEventData.Clear();

        SendMailData.Clear();
    }

    // �ΰ��� ���� �����ϱ� ��ư ������ �� �����ϴ� �Լ�.
    // ������ �����͸� ������ ����� �Ѵ�.
    public void CollectAllGameData()
    {
        // ���� �����͸� ������ �� ���� �����͸� ��� �����.
        CleanAllGameData();

        // �÷��̾� ���� + �ð� ���� ����
        CollectPlayerData();
        // �л� ���� ����
        CollectStudentData();
        // ���� ���� ����
        CollectProfessorData();
        // ���� �̺�Ʈ ���� ���� EventScheduleSystem �ּ� ȣ��
        // CollectSuddenEventData();
        // ���� ���� ���� MailManagement���� ȣ��
        // CollectMailData();  
        // ������ ���� ���� GameJam ���� ȣ��
        // CollectGameJam();
    }

    
    // �̷� ���� �ƴ϶� �� �����͸� ���� ����ϴ� ���� Start���� �����͸� �����;� �Ҳ� ����.
    public void DistributeLoadGameData()
    {
        DistributePlayerData();
        //DistributeStudentData();
        //DistributeProfessorData();

        // �̺�Ʈ ���� �����ϱ�
        // DistributeSuddenEventData();
        // ���� ���� �����ϱ�. MailManager���� ȣ������
        // DistributeMailData();
    }

    // �� Json���� ���� �� �����͵��� ��ȭ�Ǵ� ���� ���� �Լ����� �־��ֱ�
    private void CollectPlayerData()
    {
        PlayerData.Money = PlayerInfo.Instance.m_MyMoney;
        PlayerData.SpecialPoint = PlayerInfo.Instance.m_SpecialPoint; // 2�� ��ȭ�Դϴ�
        PlayerData.AcademyName = PlayerInfo.Instance.m_AcademyName;
        PlayerData.PrincipalName = PlayerInfo.Instance.m_TeacherName;

        PlayerData.Famous = PlayerInfo.Instance.m_Awareness; // ������
        PlayerData.TalentDevelopment = PlayerInfo.Instance.m_TalentDevelopment; // ���� �缺
        PlayerData.Management = PlayerInfo.Instance.m_Management; // �
        PlayerData.Activity = PlayerInfo.Instance.m_Activity; // Ȱ�� ����
        PlayerData.Goods = PlayerInfo.Instance.m_Goods; // ��ȭ ����
        PlayerData.AcademyScore = PlayerInfo.Instance.m_AcademyScore;

        PlayerData.IsFirstAcademySetting = PlayerInfo.Instance.IsFirstAcademySetting;
        PlayerData.IsFirstClassSetting = PlayerInfo.Instance.IsFirstClassSetting;
        PlayerData.IsFirstClassSettingPDEnd = PlayerInfo.Instance.IsFirstClassSettingPDEnd;
        PlayerData.IsFirstGameJam = PlayerInfo.Instance.IsFirstGameJam;
        PlayerData.IsFirstClassEnd = PlayerInfo.Instance.IsFirstClassEnd;

        PlayerData.isMissionClear = PlayerInfo.Instance.isMissionClear;
        
        PlayerData.Year = GameTime.Instance.FlowTime.NowYear;
        PlayerData.Month = GameTime.Instance.FlowTime.NowMonth;
        PlayerData.Week = GameTime.Instance.FlowTime.NowWeek;
        PlayerData.Day = GameTime.Instance.FlowTime.NowDay;
    }

    private void DistributePlayerData()
    {
        PlayerInfo.Instance.m_MyMoney = PlayerData.Money;
        PlayerInfo.Instance.m_SpecialPoint = PlayerData.SpecialPoint;
        PlayerInfo.Instance.m_AcademyName = PlayerData.AcademyName;
        PlayerInfo.Instance.m_TeacherName = PlayerData.PrincipalName;

        PlayerInfo.Instance.m_Awareness = PlayerData.Famous;
        PlayerInfo.Instance.m_TalentDevelopment = PlayerData.TalentDevelopment;
        PlayerInfo.Instance.m_Management = PlayerData.Management;
        PlayerInfo.Instance.m_Activity = PlayerData.Activity;
        PlayerInfo.Instance.m_Goods = PlayerData.Goods;
        PlayerInfo.Instance.m_AcademyScore = PlayerData.AcademyScore;
        
        PlayerInfo.Instance.IsFirstAcademySetting = PlayerData.IsFirstAcademySetting;
        PlayerInfo.Instance.IsFirstClassSetting = PlayerData.IsFirstClassSetting;
        PlayerInfo.Instance.IsFirstClassSettingPDEnd = PlayerData.IsFirstClassSettingPDEnd;
        PlayerInfo.Instance.IsFirstGameJam = PlayerData.IsFirstGameJam;
        PlayerInfo.Instance.IsFirstClassEnd = PlayerData.IsFirstClassEnd;

        PlayerInfo.Instance.isMissionClear = PlayerData.isMissionClear;

        // �ð� ������ GameTime���� ���� �ִ´�.
    }

    private void CollectStudentData()
    {
        for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
        {
            StudentSaveData tempStudent = new StudentSaveData
            {
                // �л� ID
                StudentID = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentID,
                StudentName = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName,
                Health = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Health,
                Passion = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Passion,
                Gender = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Gender,
                
                StudentType = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType,
                AbilityAmountArr = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountList,
                AbilitySkills = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilitySkills,
                Skills = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Skills,
                GenreAmountArr = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_GenreAmountList,
                
                Personality = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality,
                NumberOfEntries = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_NumberOfEntries,
                IsActiving = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsActiving,
                IsRecommend = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsRecommend,

                
                Friendship = ObjectManager.Instance.m_Friendship[i],
                FriendshipIndex = i,
            };

            StudentData.Add(tempStudent);
        }
    }

    private void CollectProfessorData()
    {
        for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
        {
            ProfessorSaveData temp = new ProfessorSaveData
            {
                ProfessorID = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorID, // ���� ���� ID
                ProfessorName =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorNameValue, // ���� �̸�
                ProfessorType = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorType, // ���� �а�
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSet, // �ַ�, ���Ӱ��� ����
                ProfessorPower = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPower, // ���Ƿ�
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorExperience, // ����ġ
                ProfessorSkills = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSkills, // ��ų
                ProfessorPay = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPay, // ����
                ProfessorHealth = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorHealth, // ü��
                ProfessorPassion = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPassion // ����
            };

            ProfessorData.Add(temp);
        }

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
        {
            ProfessorSaveData temp = new ProfessorSaveData
            {
                ProfessorID = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorID, // ���� ���� ID
                ProfessorName = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorNameValue, // ���� �̸�
                ProfessorType = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorType, // ���� �а�
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSet, // �ַ�, ���Ӱ��� ����
                ProfessorPower = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPower, // ���Ƿ�
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorExperience, // ����ġ
                ProfessorSkills = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSkills, // ��ų
                ProfessorPay = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPay, // ����
                ProfessorHealth = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorHealth, // ü��
                ProfessorPassion = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPassion // ����
            };

            ProfessorData.Add(temp);
        }

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
        {
            ProfessorSaveData temp = new ProfessorSaveData
            {
                ProfessorID = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorID, // ���� ���� ID
                ProfessorName =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorNameValue, // ���� �̸�
                ProfessorType = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorType, // ���� �а�
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorSet, // �ַ�, ���Ӱ��� ����
                ProfessorPower = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPower, // ���Ƿ�
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorExperience, // ����ġ
                ProfessorSkills = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorSkills, // ��ų
                ProfessorPay = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPay, // ����
                ProfessorHealth = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorHealth, // ü��
                ProfessorPassion = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPassion // ����
            };

            ProfessorData.Add(temp);
        }
    }

    private void DistributeProfessorData()
    {
        ObjectManager.Instance.nowProfessor.GameManagerProfessor.Clear();
        ObjectManager.Instance.nowProfessor.ArtProfessor.Clear();
        ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Clear();

        foreach (ProfessorSaveData professorData in ProfessorData)
        {
            ProfessorStat professor = new ProfessorStat();
            professor.m_ProfessorID = professorData.ProfessorID;
            professor.m_ProfessorNameValue = professorData.ProfessorName;
            professor.m_ProfessorType = professorData.ProfessorType;
            professor.m_ProfessorSet = professorData.ProfessorSet;
            professor.m_ProfessorPower = professorData.ProfessorPower;
            professor.m_ProfessorExperience = professorData.ProfessorExperience;
            professor.m_ProfessorSkills = professorData.ProfessorSkills;
            professor.m_ProfessorPay = professorData.ProfessorPay;
            professor.m_ProfessorHealth = professorData.ProfessorHealth;
            professor.m_ProfessorPassion = professorData.ProfessorPassion;
        
        
            // ���� �ʿ�
            switch (professorData.ProfessorType)
            {
                case StudentType.GameDesigner:
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor.Add(professor);
                    break;
                case StudentType.Art:
                    ObjectManager.Instance.nowProfessor.ArtProfessor.Add(professor);
                    break;
                case StudentType.Programming:
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Add(professor);
                    break;
            }
        }
    }

    public void CollectSuddenEventData(List<UsedEventRepeatData> usedEvent, List<SuddenEventTableData> todayEvent)
    {
        for (int i = 0; i < usedEvent.Count; i++)
        {
            SuddenEventData.Add(new UsedEventSaveData()
            {
                SuddenEventID = usedEvent[i].SuddenEventID,
                YearData = usedEvent[i].YearData,
                MonthData = usedEvent[i].MonthData,
                WeekData = usedEvent[i].WeekData,
                DayData = usedEvent[i].DayData,
            });
        }

        for (int i = 0; i < todayEvent.Count; i++)
        {
            TodaySuddenEventData.Add(new TodayEventSaveData()
            {
                SuddenEventID = todayEvent[i].SuddenEventID,
            });
        }
    }

    public void CollectMailData(List<MailBox> mailList)
    {
        for (int i = 0; i < mailList.Count; i++)
        {
            SendMailData.Add(new SendMailSaveData()
                {
                    MailTitle = mailList[i].m_MailTitle,
                    SendMailDate = mailList[i].m_SendMailDate,
                    FromMail = mailList[i].m_FromMail,

                    MailContent = mailList[i].m_MailContent,

                    Reward1 = mailList[i].m_Reward1,
                    Reward2 = mailList[i].m_Reward2,
                    Month = mailList[i].m_Month,
                    Type = mailList[i].m_Type,
                    IsNewMail = mailList[i].m_IsNewMail,
                    
                    //MonthReportMailContent = mailList[i].m_MonthReportMailContent,
                
                    
                    // Specification
                    IncomeEventResult = mailList[i].m_MonthReportMailContent.IncomeEventResult,
                    IncomeSell = mailList[i].m_MonthReportMailContent.IncomeSell,
                    IncomeActivity = mailList[i].m_MonthReportMailContent.IncomeActivity,
                    IncomeAcademyFee = mailList[i].m_MonthReportMailContent.IncomeAcademyFee,
                    ExpensesEventResult = mailList[i].m_MonthReportMailContent.ExpensesEventResult,
                    
                    ExpensesEventCost = mailList[i].m_MonthReportMailContent.ExpensesEventCost,
                    ExpensesActivity = mailList[i].m_MonthReportMailContent.ExpensesActivity,
                    ExpensesSalary = mailList[i].m_MonthReportMailContent.ExpensesSalary,
                    ExpensesFacility = mailList[i].m_MonthReportMailContent.ExpensesFacility,
                    ExpensesTuitionFee = mailList[i].m_MonthReportMailContent.ExpensesTuitionFee,
                    
                    TotalIncome = mailList[i].m_MonthReportMailContent.TotalIncome,
                    TotalExpenses = mailList[i].m_MonthReportMailContent.TotalExpenses,
                    NetProfit = mailList[i].m_MonthReportMailContent.NetProfit,
                    GoodsScore = mailList[i].m_MonthReportMailContent.GoodsScore,
                    FamousScore = mailList[i].m_MonthReportMailContent.FamousScore,
                    
                    ActivityScore = mailList[i].m_MonthReportMailContent.ActivityScore,
                    ManagementScore = mailList[i].m_MonthReportMailContent.ManagementScore,
                    TalentDevelopmentScore = mailList[i].m_MonthReportMailContent.TalentDevelopmentScore,
                }
            
            );
        }
    }

    public void CollectGameJamData(Dictionary<int, List<SaveGameJamData>> GameJamHistory, List<SaveGameJamData> ToBeRunning)
    {
            
    }

    // ���� �ڵ� ����
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void SaveEventData()
    {
        // for (int i = 0; i < EventData.Count; i++)
        // {
        //     EventData.Remove(EventData[i]);
        // }

        // ����Ʈ �̹Ƿ� �ݺ����� ���鼭 �����͸� �־�����
        for (int i = 0; i < SwitchEventList.Instance.ThisMonthMySelectEventList.Count; i++)
        {
            //EventSaveData tempData = new EventSaveData();

            // tempData.EventNumber = SwitchEventList.Instance.MyEventList[i].EventNumber;
            // 
            // tempData.EventDate[0] = SwitchEventList.Instance.MyEventList[i].EventDate[0];
            // tempData.EventDate[1] = SwitchEventList.Instance.MyEventList[i].EventDate[1];
            // tempData.EventDate[2] = SwitchEventList.Instance.MyEventList[i].EventDate[2];
            // tempData.EventDate[3] = SwitchEventList.Instance.MyEventList[i].EventDate[3];
            // 
            // tempData.IsPossibleUseEvent = SwitchEventList.Instance.MyEventList[i].IsPossibleUseEvent;
            // tempData.EventName = SwitchEventList.Instance.MyEventList[i].EventClassName;
            // tempData.EventInformation = SwitchEventList.Instance.MyEventList[i].EventInformation;
            // // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // �ҷ����� �� �˾��� �� ������ �ƴ��� üũ? ������ ���ڷ� �ϸ� �ʿ䰡 ����
            // 
            // tempData.EventRewardMoney = SwitchEventList.Instance.MyEventList[i].EventRewardMoney;
            // tempData.EventRewardSpecialPoint = SwitchEventList.Instance.MyEventList[i].EventRewardSpecialPoint;
            // 
            // tempData.EventRewardStat[0] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[0];       // �л�����
            // tempData.EventRewardStat[1] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[1];       // ȫ������
            // tempData.EventRewardStat[2] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[2];       // ����
            // tempData.EventRewardStat[3] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[3];       // ������

            //EventData.Add(tempData);
        }
    }

    // ���� �ε�� ���⼭ ���� ���� ������ �����Ͱ� �ʿ��� ������ ���� �־��ְ� ���� �ѱ����� �ϴ°� �� ���ϴٰ� �����ߴµ� �̷��� �ȵɰ� ����?
    // �ֳ� ������ ��ũ��Ʈ�� ��������� �κ��� �ٸ��� �����̴�
    // AllInOneData.cs -> MonoBehaviors�� ��ӹ��� �ʱ� ������ Json.cs �� Awake���� json�� ������ �� �־��ֵ��� ����
    public void LoadAllJsonData()
    {
        // LoadInGameData();
        // LoadNowMonthEventData();
    }

    // �л� -> objectManager ��  Awake���� ������ �ε� �ϰ�
    // ���� -> ChangeMailContent ���� ���� �ε�, ���� ���� ���� �ε� 


    public void LoadInGameData()
    {
        // �ΰ��� ��¥
        GameTime.Instance.FlowTime.NowYear = PlayerData.Year;
        GameTime.Instance.FlowTime.NowMonth = PlayerData.Month;
        GameTime.Instance.FlowTime.NowWeek = PlayerData.Week;
        GameTime.Instance.FlowTime.NowDay = PlayerData.Day;
    }
}

// Ŭ�������� ������ ������ AllDataClass�� �̵�����.