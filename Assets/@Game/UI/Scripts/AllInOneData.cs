using System.Collections.Generic;
using StatData.Runtime;
using System.Reflection;

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

    // Class
    public PlayerSaveData PlayerData = new(); // �÷��̾��� �ΰ��� ����
    public List<StudentSaveData> StudentData = new(); // ���� ����ϴ� �л� ����
    public List<ProfessorSaveData> ProfessorData = new(); // ���� ����ϴ� ���� ����
    public List<UsedEventSaveData> SuddenEventData = new(); // ����� ���� �̺�Ʈ ����
    public List<TodayEventSaveData> TodaySuddenEventData = new(); // �ҷ��� �� �����ϴ� ���� �̺�Ʈ ID

    public List<SendMailSaveData> SendMailData = new(); // ��� ���� ����
    
    public List<GameJamSaveData>  GameJamToBeRunning = new(); // ������ ������ ����Ÿ
    public List<GameShowSaveData>  GameShowToBeRunning = new(); // ������ ������ ����Ÿ
    
    // ������ ���� ���߰� (7.17)
    public Dictionary<int, List<GameJamSaveData>>  GameJamHistory = new(); // �Ϸ��� ������ ����Ÿ
    public Dictionary<int, List<GameShowSaveData>>  GameShowHistory = new(); // �Ϸ��� ������ ����Ÿ

    
    // �ε� ���θ� �Ǻ��ϴ� ����
    public bool ServerLoading = false;


    // //public InGameSaveData InGameData = new InGameSaveData(); // �ΰ��ӿ� �ʿ��� �����͵�

    // public List<GameJamInfo> GameJamData = new List<GameJamInfo>();
    // public List<InJaeRecommendData> _InJaeRecommendData = new List<InJaeRecommendData>();
    // public List<RankScript> RankScripteData = new List<RankScript>();
    // public List<EmailData> _EmailData = new List<EmailData>();
    //public List<GameShowData> _GameShowData = new(); // �׽�Ʈ��

    // Data
    // ģ�е� �����ʹ� �л��� ���� ����ֵ��� �����߽��ϴ�.
    //public List<List<int>> Friendship = new List<List<int>>(); // ��� �л��� ������ ģ�е� (�л�18 ����3)


    public void CleanAllGameData()
    {
        PlayerData = new PlayerSaveData();
        StudentData.Clear();
        ProfessorData.Clear();
        SuddenEventData.Clear();
        TodaySuddenEventData.Clear();

        SendMailData.Clear();
        
        GameJamToBeRunning.Clear();
        GameJamHistory.Clear();
        GameShowToBeRunning.Clear();
        GameShowHistory.Clear();

        //TestData = new DicSaveTest();
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
        // �������� �̱����� �ƴ϶� �� Ŭ�������� ȣ���ؼ� ����
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
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var setPlayerData = typeof(PlayerSaveData).GetProperties(flags);
        var getPlayerData = typeof(PlayerInfo).GetFields(flags);
        var getTimeData = typeof(NowTime).GetFields(flags);
        
        foreach (var set in setPlayerData)
        {
            foreach (var get in getPlayerData)
            {
                if (set.Name == get.Name)
                {
                    var value = get.GetValue(PlayerInfo.Instance);
                    set.SetValue(PlayerData, value);
                    break;
                }
            }

            foreach (var get in getTimeData)
            {
                if ("Now" + set.Name == get.Name)
                {
                    var value = get.GetValue(GameTime.Instance.FlowTime);
                    set.SetValue(PlayerData, value);
                    break;
                }
            }
        }

        // TestData.GameDesignerClassDic = PlayerData.GameDesignerClassDic;
        // TestData.ArtClassDic = PlayerData.ArtClassDic;
        // TestData.ProgrammingClassDic = PlayerData.ProgrammingClassDic;
        //
        // TestData.GameJamEntryCount = PlayerData.GameJamEntryCount;
        // TestData.NeedGameDesignerStat = PlayerData.NeedGameDesignerStat;
        // TestData.NeedArtStat = PlayerData.NeedArtStat;
        // TestData.NeedProgrammingStat = PlayerData.NeedProgrammingStat;


        // PlayerData.MyMoney = PlayerInfo.Instance.MyMoney;
        // PlayerData.SpecialPoint = PlayerInfo.Instance.SpecialPoint; // 2�� ��ȭ�Դϴ�
        // PlayerData.AcademyName = PlayerInfo.Instance.m_AcademyName;
        // PlayerData.PrincipalName = PlayerInfo.Instance.TeacherName;
        //
        // PlayerData.Famous = PlayerInfo.Instance.Famous; // ������
        // PlayerData.TalentDevelopment = PlayerInfo.Instance.TalentDevelopment; // ���� �缺
        // PlayerData.Management = PlayerInfo.Instance.Management; // �
        // PlayerData.Activity = PlayerInfo.Instance.Activity; // Ȱ�� ����
        // PlayerData.Goods = PlayerInfo.Instance.Goods; // ��ȭ ����
        // PlayerData.AcademyScore = PlayerInfo.Instance.AcademyScore;
        //
        // PlayerData.IsFirstAcademySetting = PlayerInfo.Instance.IsFirstAcademySetting;
        // PlayerData.IsFirstClassSetting = PlayerInfo.Instance.IsFirstClassSetting;
        // PlayerData.IsFirstClassSettingPdEnd = PlayerInfo.Instance.IsFirstClassSettingPdEnd;
        // PlayerData.IsFirstGameJam = PlayerInfo.Instance.IsFirstGameJam;
        // PlayerData.IsFirstClassEnd = PlayerInfo.Instance.IsFirstClassEnd;
        //
        // PlayerData.IsMissionClear = PlayerInfo.Instance.IsMissionClear;
        // PlayerData.NowMissionStepCount = PlayerInfo.Instance.NowMissionStepCount;

        // PlayerData.Year = GameTime.Instance.FlowTime.NowYear;
        // PlayerData.Month = GameTime.Instance.FlowTime.NowMonth;
        // PlayerData.Week = GameTime.Instance.FlowTime.NowWeek;
        // PlayerData.Day = GameTime.Instance.FlowTime.NowDay;
    }

    private void DistributePlayerData()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var setPlayerData = typeof(PlayerInfo).GetFields(flags);
        var getPlayerData = typeof(PlayerSaveData).GetProperties(flags);

        foreach (var set in setPlayerData)
        {
            foreach (var get in getPlayerData)
            {
                if (set.Name == get.Name)
                {
                    var value = get.GetValue(PlayerData);
                    set.SetValue(PlayerInfo.Instance, value);
                    break;
                }
            }
        }
        

        //
        // PlayerInfo.Instance.MyMoney = PlayerData.MyMoney;
        // PlayerInfo.Instance.SpecialPoint = PlayerData.SpecialPoint;
        // PlayerInfo.Instance.m_AcademyName = PlayerData.AcademyName;
        // PlayerInfo.Instance.TeacherName = PlayerData.PrincipalName;
        //
        // PlayerInfo.Instance.Famous = PlayerData.Famous;
        // PlayerInfo.Instance.TalentDevelopment = PlayerData.TalentDevelopment;
        // PlayerInfo.Instance.Management = PlayerData.Management;
        // PlayerInfo.Instance.Activity = PlayerData.Activity;
        // PlayerInfo.Instance.Goods = PlayerData.Goods;
        // PlayerInfo.Instance.AcademyScore = PlayerData.AcademyScore;
        //
        // PlayerInfo.Instance.IsFirstAcademySetting = PlayerData.IsFirstAcademySetting;
        // PlayerInfo.Instance.IsFirstClassSetting = PlayerData.IsFirstClassSetting;
        // PlayerInfo.Instance.IsFirstClassSettingPdEnd = PlayerData.IsFirstClassSettingPdEnd;
        // PlayerInfo.Instance.IsFirstGameJam = PlayerData.IsFirstGameJam;
        // PlayerInfo.Instance.IsFirstClassEnd = PlayerData.IsFirstClassEnd;
        //
        // PlayerInfo.Instance.IsMissionClear = PlayerData.IsMissionClear;
        // PlayerInfo.Instance.NowMissionStepCount = PlayerData.NowMissionStepCount;

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
                UserSettingName = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_UserSettingName,
                Health = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Health,
                Passion = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Passion,
                Gender = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Gender,

                StudentType = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType,
                AbilityAmountArr = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilityAmountArr,
                AbilitySkills = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_AbilitySkills,
                Skills = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Skills,
                GenreAmountArr = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_GenreAmountArr,

                Personality = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_Personality,
                NumberOfEntries = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_NumberOfEntries,
                IsActiving = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsActiving,
                IsRecommend = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsRecommend,


                Friendship = ObjectManager.Instance.m_Friendship[i],
                FriendshipIndex = i,
            };

            StudentData.Add(tempStudent);
        }

        // BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //
        // var setData = typeof(StudentSaveData).GetProperties(flags);
        // var getData = typeof(StudentStat).GetFields(flags);
        //
        //
        // foreach (var student in ObjectManager.Instance.m_StudentList)
        // {
        //     StudentSaveData data = new StudentSaveData();
        //
        //     foreach (var set in setData)
        //     {
        //         foreach (var get in getData)
        //         {
        //             if ("m_" +  set.Name == get.Name)
        //             {
        //                 var value = get.GetValue(student.m_StudentStat);
        //                 set.SetValue(data, value);
        //                 break;
        //             }
        //         }
        //     }
        //
        //     StudentData.Add(data);
        // }
    }

    private void CollectProfessorData()
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var setData = typeof(ProfessorSaveData).GetProperties(flags);
        var getData = typeof(ProfessorStat).GetFields(flags);

        foreach (var professor in Professor.Instance.GameManagerProfessor)
        {
            ProfessorSaveData data = new ProfessorSaveData();

            foreach (var set in setData)
            {
                foreach (var get in getData)
                {
                    if ("m_" + set.Name == get.Name)
                    {
                        var value = get.GetValue(professor);
                        set.SetValue(data, value);
                        break;
                    }
                }
            }

            ProfessorData.Add(data);
        }

        foreach (var professor in Professor.Instance.ArtProfessor)
        {
            ProfessorSaveData data = new ProfessorSaveData();

            foreach (var set in setData)
            {
                foreach (var get in getData)
                {
                    if ("m_" + set.Name == get.Name)
                    {
                        var value = get.GetValue(professor);
                        set.SetValue(data, value);
                        break;
                    }
                }
            }

            ProfessorData.Add(data);
        }

        foreach (var professor in Professor.Instance.ProgrammingProfessor)
        {
            ProfessorSaveData data = new ProfessorSaveData();

            foreach (var set in setData)
            {
                foreach (var get in getData)
                {
                    if ("m_" + set.Name == get.Name)
                    {
                        var value = get.GetValue(professor);
                        set.SetValue(data, value);
                        break;
                    }
                }
            }

            ProfessorData.Add(data);
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
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;


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

    // public void CollectGameJamData(Dictionary<int, List<GameJamSaveData>> GameJamHistory,
    //     List<GameJamSaveData> ToBeRunning)
    // {
    // }
}

// Ŭ�������� ������ ������ AllDataClass�� �̵�����.