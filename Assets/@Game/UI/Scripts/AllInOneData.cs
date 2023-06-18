using System.Collections.Generic;
using StatData.Runtime;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Class
    public PlayerSaveData PlayerData = new(); // 플레이어의 인게임 정보
    public List<StudentSaveData> StudentData = new(); // 현재 사용하는 학생 정보
    public List<ProfessorSaveData> ProfessorData = new(); // 현재 사용하는 교수 정보
    public List<UsedEventSaveData> SuddenEventData = new(); // 사용한 돌발 이벤트 정보
    public List<TodayEventSaveData> TodaySuddenEventData = new(); // 불러온 뒤 떠야하는 돌발 이벤트 ID

    public List<SendMailSaveData> SendMailData = new(); // 모든 메일 정보
    public List<SaveGameJamData> EndGameJamData = new(); // 완료한 게임잼 데이타
    


    // 로딩 여부를 판별하는 변수
    public bool ServerLoading = false;
    
    
    
    
    // //public InGameSaveData InGameData = new InGameSaveData(); // 인게임에 필요한 데이터들

    // public List<GameJamInfo> GameJamData = new List<GameJamInfo>();
    // public List<InJaeRecommendData> _InJaeRecommendData = new List<InJaeRecommendData>();
    // public List<RankScript> RankScripteData = new List<RankScript>();
    // public List<EmailData> _EmailData = new List<EmailData>();
    public List<GameShowData> _GameShowData = new(); // 테스트용

    // Data
    // 친밀도 데이터는 학생이 직접 들고있도록 변경했습니다.
    //public List<List<int>> Friendship = new List<List<int>>(); // 모든 학생과 강사의 친밀도 (학생18 강사3)

    
    
    

    private void CleanAllGameData()
    {
        PlayerData = new PlayerSaveData();
        StudentData.Clear();
        ProfessorData.Clear();
        SuddenEventData.Clear();
        TodaySuddenEventData.Clear();

        SendMailData.Clear();
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
        // 돌발 이벤트 정보 모음 EventScheduleSystem 애서 호출
        // CollectSuddenEventData();
        // 메일 정보 모음 MailManagement에서 호출
        // CollectMailData();  
        // 게임잼 정보 모음 GameJam 에서 호출
        // CollectGameJam();
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
        PlayerData.Money = PlayerInfo.Instance.m_MyMoney;
        PlayerData.SpecialPoint = PlayerInfo.Instance.m_SpecialPoint; // 2차 재화입니다
        PlayerData.AcademyName = PlayerInfo.Instance.m_AcademyName;
        PlayerData.PrincipalName = PlayerInfo.Instance.m_TeacherName;

        PlayerData.Famous = PlayerInfo.Instance.m_Awareness; // 인지도
        PlayerData.TalentDevelopment = PlayerInfo.Instance.m_TalentDevelopment; // 인재 양성
        PlayerData.Management = PlayerInfo.Instance.m_Management; // 운영
        PlayerData.Activity = PlayerInfo.Instance.m_Activity; // 활동 점수
        PlayerData.Goods = PlayerInfo.Instance.m_Goods; // 재화 점수
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
                ProfessorID = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorID, // 강사 고유 ID
                ProfessorName =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorNameValue, // 강사 이름
                ProfessorType = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorType, // 강사 학과
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSet, // 왜래, 전임강사 구별
                ProfessorPower = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPower, // 강의력
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorExperience, // 경험치
                ProfessorSkills = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSkills, // 스킬
                ProfessorPay = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPay, // 월급
                ProfessorHealth = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorHealth, // 체력
                ProfessorPassion = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPassion // 열정
            };

            ProfessorData.Add(temp);
        }

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
        {
            ProfessorSaveData temp = new ProfessorSaveData
            {
                ProfessorID = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorID, // 강사 고유 ID
                ProfessorName = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorNameValue, // 강사 이름
                ProfessorType = ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorType, // 강사 학과
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSet, // 왜래, 전임강사 구별
                ProfessorPower = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPower, // 강의력
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorExperience, // 경험치
                ProfessorSkills = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorSkills, // 스킬
                ProfessorPay = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPay, // 월급
                ProfessorHealth = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorHealth, // 체력
                ProfessorPassion = ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorPassion // 열정
            };

            ProfessorData.Add(temp);
        }

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
        {
            ProfessorSaveData temp = new ProfessorSaveData
            {
                ProfessorID = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorID, // 강사 고유 ID
                ProfessorName =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorNameValue, // 강사 이름
                ProfessorType = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorType, // 강사 학과
                ProfessorSet =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorSet, // 왜래, 전임강사 구별
                ProfessorPower = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPower, // 강의력
                
                ProfessorExperience =
                    ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorExperience, // 경험치
                ProfessorSkills = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorSkills, // 스킬
                ProfessorPay = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPay, // 월급
                ProfessorHealth = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorHealth, // 체력
                ProfessorPassion = ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorPassion // 열정
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
        
        
            // 수정 필요
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

    // 이전 코드 유지
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void SaveEventData()
    {
        // for (int i = 0; i < EventData.Count; i++)
        // {
        //     EventData.Remove(EventData[i]);
        // }

        // 리스트 이므로 반복문을 돌면서 데이터를 넣어주자
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
            // // tempData.IsPopUp = SwitchEventList.Instance.MyEventList[i].IsPopUp;     // 불러오기 시 팝업이 된 것인지 아닌지 체크? 어차피 날자로 하면 필요가 없나
            // 
            // tempData.EventRewardMoney = SwitchEventList.Instance.MyEventList[i].EventRewardMoney;
            // tempData.EventRewardSpecialPoint = SwitchEventList.Instance.MyEventList[i].EventRewardSpecialPoint;
            // 
            // tempData.EventRewardStat[0] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[0];       // 학생복지
            // tempData.EventRewardStat[1] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[1];       // 홍보점수
            // tempData.EventRewardStat[2] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[2];       // 버프
            // tempData.EventRewardStat[3] = SwitchEventList.Instance.MyEventList[i].EventRewardStat[3];       // 아이템

            //EventData.Add(tempData);
        }
    }

    // 현재 로드는 여기서 쓰지 않음 각각의 데이터가 필요한 곳에서 직접 넣어주고 있음 한군데서 하는게 더 편하다고 생각했는데 이러면 안될것 같다?
    // 왜냐 각각의 스크립트가 만들어지는 부분이 다르기 때문이다
    // AllInOneData.cs -> MonoBehaviors를 상속받지 않기 때문에 Json.cs 의 Awake에서 json의 정보를 다 넣어주도록 하자
    public void LoadAllJsonData()
    {
        // LoadInGameData();
        // LoadNowMonthEventData();
    }

    // 학생 -> objectManager 의  Awake에서 데이터 로드 하고
    // 메일 -> ChangeMailContent 에서 원본 로드, 현재 메일 상태 로드 


    public void LoadInGameData()
    {
        // 인게임 날짜
        GameTime.Instance.FlowTime.NowYear = PlayerData.Year;
        GameTime.Instance.FlowTime.NowMonth = PlayerData.Month;
        GameTime.Instance.FlowTime.NowWeek = PlayerData.Week;
        GameTime.Instance.FlowTime.NowDay = PlayerData.Day;
    }
}

// 클래스들의 데이터 정보는 AllDataClass로 이동했음.