using System.Collections.Generic;
using StatData.Runtime;
using System.Reflection;

/// <summary>
///  Mang 23. 02. 08
/// 
/// Json 화 할 데이터 덩어리들
/// 여기에 데이터들을 다 넣어두면 최종본의 데이터저장소가 됨
///
/// woodpie 23. 05. 11
/// 데이터를 json 화 해서 로컬 저장하고, 파이어스토어에도 저장하기 위해 재구성함
/// NewtoneJson의 사용을 위해 monobehabear를 상속받지 않음.
///
/// 데이터를 모으는 코드는 Collect
/// 데이터를 실제로 저장하는 코드에만 Save를 붙인다.
///
/// AllInOneData 의 사용 목적
/// 1. 게임을 시작할 때 이미 저장된 데이터가 있다면 그 정보를 읽어옴
/// 1.1 읽어온 데이터를 인게임에 적용시킴.
/// 1.2 인게임에 적절한 시간, 상황, 데이터 적용
/// 2. 자동 혹은 수동 저장할 때 게임 내에 흩어져있는 데이터들을 수집
/// 2.1 json 파일에 저장
/// 2.2 파이어스토어에 저장
/// 3. 구글 로그인 이후 데이터를 불러올 때 구글 서버에서 정보를 읽어옴.
/// 1,1 / 1.2 반복
///
/// 인게임에서 사용하는 클래스가 아니라 별도의 클래스를 따로 만들어서 작업하는 이유!
/// 1. NewtonJson은 MonoBehaviour 붙은 클래스는 파싱하지 못함 별도의 클래스가 필요
/// 2. 저장하고 불러올 필요한 데이터만 모아서 한번에 보고 싶었음.
/// 3. 파이어베이스의 데이터 형식에 적절히 맞춰야 했음 (이 문제는 테스트해봐야 함)
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
    public PlayerSaveData PlayerData = new(); // 플레이어의 인게임 정보
    public List<StudentSaveData> StudentData = new(); // 현재 사용하는 학생 정보
    public List<ProfessorSaveData> ProfessorData = new(); // 현재 사용하는 교수 정보
    public List<UsedEventSaveData> SuddenEventData = new(); // 사용한 돌발 이벤트 정보
    public List<TodayEventSaveData> TodaySuddenEventData = new(); // 불러온 뒤 떠야하는 돌발 이벤트 ID

    public List<SendMailSaveData> SendMailData = new(); // 모든 메일 정보
    
    public List<GameJamSaveData>  GameJamToBeRunning = new(); // 진행할 게임잼 데이타
    public List<GameShowSaveData>  GameShowToBeRunning = new(); // 진행할 게임잼 데이타
    
    // 서버에 아직 미추가 (7.17)
    public Dictionary<int, List<GameJamSaveData>>  GameJamHistory = new(); // 완료한 게임잼 데이타
    public Dictionary<int, List<GameShowSaveData>>  GameShowHistory = new(); // 완료한 게임잼 데이타

    
    // 로딩 여부를 판별하는 변수
    public bool ServerLoading = false;


    // //public InGameSaveData InGameData = new InGameSaveData(); // 인게임에 필요한 데이터들

    // public List<GameJamInfo> GameJamData = new List<GameJamInfo>();
    // public List<InJaeRecommendData> _InJaeRecommendData = new List<InJaeRecommendData>();
    // public List<RankScript> RankScripteData = new List<RankScript>();
    // public List<EmailData> _EmailData = new List<EmailData>();
    //public List<GameShowData> _GameShowData = new(); // 테스트용

    // Data
    // 친밀도 데이터는 학생이 직접 들고있도록 변경했습니다.
    //public List<List<int>> Friendship = new List<List<int>>(); // 모든 학생과 강사의 친밀도 (학생18 강사3)


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

    // 인게임 에서 저장하기 버튼 눌렀을 때 동작하는 함수.
    // 게임의 데이터를 모으는 기능을 한다.
    public void CollectAllGameData()
    {
        // 새로 데이터를 모으기 전 이전 데이터를 모두 지운다.
        CleanAllGameData();

        // 플레이어 정보 + 시간 정보 모음
        CollectPlayerData();
        // 학생 정보 모음
        CollectStudentData();
        // 강사 정보 모음
        CollectProfessorData();
        // 나머지는 싱글톤이 아니라서 각 클래스에서 호출해서 저장
    }


    // 이런 식이 아니라 각 데이터를 직접 사용하는 곳의 Start에서 데이터를 가져와야 할꺼 같다.
    public void DistributeLoadGameData()
    {
        DistributePlayerData();
        //DistributeStudentData();
        //DistributeProfessorData();

        // 이벤트 정보 적용하기
        // DistributeSuddenEventData();
        // 메일 정보 적용하기. MailManager에서 호출하자
        // DistributeMailData();
    }

    // 각 Json으로 저장 할 데이터들의 변화되는 값을 여기 함수에서 넣어주기
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
        // PlayerData.SpecialPoint = PlayerInfo.Instance.SpecialPoint; // 2차 재화입니다
        // PlayerData.AcademyName = PlayerInfo.Instance.m_AcademyName;
        // PlayerData.PrincipalName = PlayerInfo.Instance.TeacherName;
        //
        // PlayerData.Famous = PlayerInfo.Instance.Famous; // 인지도
        // PlayerData.TalentDevelopment = PlayerInfo.Instance.TalentDevelopment; // 인재 양성
        // PlayerData.Management = PlayerInfo.Instance.Management; // 운영
        // PlayerData.Activity = PlayerInfo.Instance.Activity; // 활동 점수
        // PlayerData.Goods = PlayerInfo.Instance.Goods; // 재화 점수
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

        // 시간 정보는 GameTime에서 직접 넣는다.
    }

    private void CollectStudentData()
    {
        for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
        {
            StudentSaveData tempStudent = new StudentSaveData
            {
                // 학생 ID
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

// 클래스들의 데이터 정보는 AllDataClass로 이동했음.