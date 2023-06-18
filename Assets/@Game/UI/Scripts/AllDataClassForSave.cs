using System.Collections.Generic;
using Firebase.Firestore;
using StatData.Runtime;

// 플레이어, 학생, 교수, 사용 돌발 이벤트 목록, 발생 이벤트 목록, 메일 정보, 보고서 정보
// 게임잼 게임쇼(어떤 게임쇼, 어느 평가를 받았는지) 


// 0612
[FirestoreData]
public class PlayerSaveData
{
    [FirestoreProperty] public string PrincipalName { get; set; } // 원장 이름
    [FirestoreProperty] public string AcademyName { get; set; } // 아카데미 이름
    [FirestoreProperty] public int Money { get; set; } // 소유 머니
    [FirestoreProperty] public int SpecialPoint { get; set; } // 소유 포인트

    [FirestoreProperty] public int Famous { get; set; } // 인지도
    [FirestoreProperty] public int TalentDevelopment { get; set; } // 인재 양성
    [FirestoreProperty] public int Management { get; set; } // 운영
    [FirestoreProperty] public int Activity { get; set; } // 활동점수
    [FirestoreProperty] public int Goods { get; set; } // 재화점수
    [FirestoreProperty] public int AcademyScore { get; set; } // 아카데미 점수

    // 튜토리얼 여부
    [FirestoreProperty] public bool IsFirstAcademySetting { get; set; }
    [FirestoreProperty] public bool IsFirstClassSetting { get; set; }
    [FirestoreProperty] public bool IsFirstClassSettingPDEnd { get; set; }
    [FirestoreProperty] public bool IsFirstGameJam { get; set; }
    [FirestoreProperty] public bool IsFirstClassEnd { get; set; }

    // 퀘스트
    [FirestoreProperty] public bool[] isMissionClear { get; set; }

    // 게임 시간
    [FirestoreProperty] public int Year { get; set; }
    [FirestoreProperty] public int Month { get; set; }
    [FirestoreProperty] public int Week { get; set; }
    [FirestoreProperty] public int Day { get; set; }

}


// 0612
[FirestoreData]
public class StudentSaveData
{
    // 학생이 가지고있는 기본 스탯. (체력, 열정, 친밀도 등)
    [FirestoreProperty] public string StudentID { get; set; } // 학생 고유 ID 모델링 찾는 용
    [FirestoreProperty] public string StudentName { get; set; } // 학생 이름
    [FirestoreProperty] public int Health { get; set; } // 학생 체력 최대 100, 최소 0
    [FirestoreProperty] public int Passion { get; set; } // 학생 열정 최대 100, 최소 0
    [FirestoreProperty] public int Gender { get; set; } // 학생 성별


    [FirestoreProperty] public StudentType StudentType { get; set; } // 각 파트

    // 파트별로 학생들이 가지고 있는 공통 스탯(특정 수치에 도달하면 스킬을 얻음)
    [FirestoreProperty] public int[] AbilityAmountArr { get; set; }
    [FirestoreProperty] public int[] AbilitySkills { get; set; }
    [FirestoreProperty] public List<string> Skills { get; set; }
    [FirestoreProperty] public int[] GenreAmountArr { get; set; } // 장르

    [FirestoreProperty] public string Personality { get; set; } // 성격
    [FirestoreProperty] public int NumberOfEntries { get; set; }
    [FirestoreProperty] public bool IsActiving { get; set; } // 현재 게임잼이나 이벤트에서 선택되어 파견중인지
    [FirestoreProperty] public bool IsRecommend { get; set; } // 추천을 받았는지 아닌지 판별

    [FirestoreProperty] public List<int> Friendship { get; set; } // 한 학생과 연결되어있는 호감도
    [FirestoreProperty] public int FriendshipIndex { get; set; } // 호감도를 적용하기 위한 인덱스
}


// 0612
[FirestoreData]
public class ProfessorSaveData
{
    [FirestoreProperty] public int ProfessorID { get; set; } // 강사 고유 ID
    [FirestoreProperty] public string ProfessorName { get; set; } // 강사 이름
    [FirestoreProperty] public StudentType ProfessorType { get; set; } // 강사 학과 구별
    [FirestoreProperty] public string ProfessorSet { get; set; } // 외래, 전임강사 구별
    [FirestoreProperty] public int ProfessorPower { get; set; } // 강의력

    [FirestoreProperty] public int ProfessorExperience { get; set; } // 경험치
    [FirestoreProperty] public List<string> ProfessorSkills { get; set; } // 스킬
    [FirestoreProperty] public int ProfessorPay { get; set; } // 월급
    [FirestoreProperty] public int ProfessorHealth { get; set; } // 체력
    [FirestoreProperty] public int ProfessorPassion { get; set; } // 열정
}

// 사용한 이벤트 정보를 모은다. 0612
// 사용한 정보만 가지고 있으면 반복해서 사용 방지 가능
[FirestoreData]
public class UsedEventSaveData
{
    [FirestoreProperty] public int SuddenEventID { get; set; } // 이벤트의 ID
    [FirestoreProperty] public int YearData { get; set; } // 사용 가능한 년도
    [FirestoreProperty] public int MonthData { get; set; } // 사용 가능한 월
    [FirestoreProperty] public int WeekData { get; set; } // 사용 가능한 주
    [FirestoreProperty] public int DayData { get; set; } // 사용 가능한 날
}

// 오늘 떠야하는 이벤트 목록 0612
[FirestoreData]
public class TodayEventSaveData
{
    [FirestoreProperty] public int SuddenEventID { get; set; } // 이벤트의 ID
}

// 일반메일, 일반보상메일, 월간보고, 랭크보상 까지
// MailManagement의 m_MailList을 담자.
[FirestoreData]
public class SendMailSaveData
{
    [FirestoreProperty] public string MailTitle { get; set; }
    [FirestoreProperty] public string SendMailDate { get; set; }
    [FirestoreProperty] public string FromMail { get; set; }

    [FirestoreProperty] public string MailContent { get; set; }

    [FirestoreProperty] public string Reward1 { get; set; }
    [FirestoreProperty] public string Reward2 { get; set; }
    [FirestoreProperty] public string Month { get; set; }
    [FirestoreProperty] public MailType Type { get; set; }
    [FirestoreProperty] public bool IsNewMail { get; set; }

    //[FirestoreProperty] public Specification MonthReportMailContent { get; set; }

    // Specification
    [FirestoreProperty] public int IncomeEventResult { get; set; }
    [FirestoreProperty] public int IncomeSell { get; set; }
    [FirestoreProperty] public int IncomeActivity { get; set; }
    [FirestoreProperty] public int IncomeAcademyFee { get; set; }
    [FirestoreProperty] public int ExpensesEventResult { get; set; }

    [FirestoreProperty] public int ExpensesEventCost { get; set; }
    [FirestoreProperty] public int ExpensesActivity { get; set; }
    [FirestoreProperty] public int ExpensesSalary { get; set; }
    [FirestoreProperty] public int ExpensesFacility { get; set; }
    [FirestoreProperty] public int ExpensesTuitionFee { get; set; }

    [FirestoreProperty] public int TotalIncome { get; set; }
    [FirestoreProperty] public int TotalExpenses { get; set; }
    [FirestoreProperty] public int NetProfit { get; set; }
    [FirestoreProperty] public int GoodsScore { get; set; }
    [FirestoreProperty] public int FamousScore { get; set; }

    [FirestoreProperty] public int ActivityScore { get; set; }
    [FirestoreProperty] public int ManagementScore { get; set; }
    [FirestoreProperty] public int TalentDevelopmentScore { get; set; }
}


// 이전 데이터 + 


public class GameJamInfo
{
    public int m_GjamID;
    public bool m_Fix;
    public string m_GjamName;
    public string m_GjamHost;
    public int m_GjamHostIcon;
    public string m_GjamDetailInfo;
    public int m_EntryFee;
    public int m_StudentHealth; // 게임잼 참여시 소모되는 학생 기본스탯
    public int m_StudentPassion; // 게임잼 참여시 소모되는 학생 기본스탯
    public string m_GjamMainGenre; // 게임잼이 요구하는 메인 장르
    public string m_GjamSubGenre; // 게임잼이 요구하는 서브 장르
    public string m_GjamConcept;
    public int m_GjamYear; // 0일 경우 매 년 발생하는 정기적인 이벤트, -1은 홀수년도, -2는 짝수년도에 발생
    public int m_GjamMonth;
    public int m_GjamWeek;
    public int m_GjamTime; // 게임잼이 몇 초동안 실행될 지에 대한 정보
    public int m_GjamAI_ID;

    public Dictionary<string, int>
        m_GjamNeedStatGM = new Dictionary<string, int>(); // 게임 제작 완성에 필요한 스탯종류와 값(각 학과별로 있어야 함)

    public Dictionary<string, int>
        m_GjamNeedStatArt = new Dictionary<string, int>(); // 게임 제작 완성에 필요한 스탯종류와 값(각 학과별로 있어야 함)

    public Dictionary<string, int>
        m_GjamNeedStatProgramming = new Dictionary<string, int>(); // 게임 제작 완성에 필요한 스탯종류와 값(각 학과별로 있어야 함)

    public int ParticipationReward;
}

// 인재추천 데이터 클래스
public class InJaeRecommendData
{
    public int m_EmploymentID; // 채용 공고ID
    public string m_EmploymentName; // 채용 공고 제목

    public int m_CompanyID; // 회사 ID
    public string m_CompanyName; // 회사 이름
    public int m_CompanyGameNameID; // 회사 게임이름의 아이디(아직 사용하는 곳은 없음)
    public int m_CompanyIcon; // 회사 아이콘
    public int m_CompanyGrade_ID; // 회사 등급 아이디
    public string m_CompanyGrade; // 회사 등급

    public Dictionary<string, int> m_CompanyRequirementStats = new Dictionary<string, int>(); // 회사요구조건(스탯)
    public string m_CompanyRequirementSkill_ID; // 회사 요구조건(스킬), 지금은 기본 스킬로 

    public bool m_CompanyUnLock; // 화사 해금 여부

    //public int m_CompanyUnLockCondition;                                                      // 회사 해금 조건(등급)
    public string m_EmploymentYear; // 채용 공고가 발생할 년 정보, 한번 나오면 0으로 바꿔주기
    public string m_EmploymentText; // 채용 공고 내용
    public int m_CompanySalary; // 연봉
    public string m_CompanyJob; // 모집 직무
    public int m_CompanyJob_ID; // 모집 직무
    public int m_CompanyPart; // 모집 파트
}

public class RankScript
{
    public int ID;
    public int Condition;
    public string Text;
    public int Img;
    public int Reward1;
    public int Reward1Amount;
    public int Reward2;
    public int Reward2Amount;
}

// 수업 알람이 떴을 때 나올 스크립트
public class ClassAlramScript
{
    public int No;
    public int Month;
    public string Text;
}

public class ClassInfoData
{
    public int ClassID;
    public string ClassName;
    public int ClassType_ID;
    public string ClassStatType;
    public string OpenYear;
    public string OpenMonth;
    public int Sense;
    public int Concentration;
    public int Wit;
    public int Technique;
    public int Insight;
    public int Money;
    public int Health;
    public int Passion;
    public bool Class_Unlock;
}


public class GameShowData
{
    public int GameShow_ID;
    public bool GameShow_Fix;
    public string GameShow_Name;

    public int GameShow_Level;
    public int GameShow_Health;
    public int GameShow_Pasion;
    public Dictionary<string, int> GameShow_State = new Dictionary<string, int>();
    public string[] GameShowConcept;
    public string[] GameShowGenre;
    public int GameShow_Host_ID;
    public string GameShow_Host_Name;
    public int GameShow_Judges_ID;
    public string GameShow_Judges_Name;
    public int Preset_ID;
    public int GameShow_Year;
    public int GameShow_Month;
    public int GameShow_Week;
    public int GameShow_Day;

    public int GameShow_Plusreward;
    public int GameShow_Plusreward_value;
    public bool GameShow_Special;
    public int GameShow_Reward;
}

// 랭크의 보상메일을 제외한 나머지 메일들의 형식(일반메일, 일반보상메일, 월간보고)
public class EmailData
{
    public int Email_ID;
    public int Email_sender_id;
    public string Email_date;
    public string Email_Name;
    public string Email_script_Text;
    public int Email_reward1_id;
    public int Email_reward1_Amount;
    public int Email_reward2_id;
    public int Email_reward2_Amount;
}

// 랭크 보상 메일의 형식은 기본 메일과 조금 달라 따로 관리
// 1년에 한번
public class RewardEmailData
{
    public int Email_ID;
    public int Email_sender_id;
    public int Email_script_id;
    public string Email_sender_Name;
    public string Email_script_Text;
    public string Email_image_id;
}

// 학생들 보너스 스킬에 대한 조건들의 클래스
public class BonusSkillConditions
{
    public int Ability_ID;
    public string Ability_Name;
    public int Ability_Script_ID;
    public int StudentPart_ID;
    public int[] State;

    public int[] Genre;

    // 게임잼 관련 스킬 조건들
    public int GamedevelopmentNumber; // 게임잼에 N회 참여했을 때.
    public string GameGenre_ID; // 특정 장르 아이디
    public int GameGenre_DPNumber; // 특정 장르 아이디의 게임을 N회 제작했을 경우.
    public string[] GameJameLevel; // 특정 등급의 게임을 제작했을 경우.(복수가능)
    public string GameConcept_ID; // 특정 컨셉 아이디

    public int GameConcept_DPNumber; // 특정 컨셉의 게임을 제작했을 경우

    // 게임쇼 관련 스킬 조건
    public int[] GameShow_Level; // 특정 난이도의 게임잼에서 좋음 이상의 결과를 받았을 때(난이도 복수가능)

    // 수업 관련 스킬 조건들
    public int ClassID; // 특정 수업 아이디
    public int ClassType; // 수업의 타입(공통 : 0, 기획 : 1, 아트 : 2, 플밍 : 3, 없으면 -1)
    public int Numberoflessons; // 특정 아이디의 수업을 N회 들었을 경우.

    // 친밀도 관련 스킬 조건들
    public string Intimacy; // 요구하는 친밀도
    public int Intimacy_Number; // 몇 명의 캐릭터와 요구하는 친밀도를 가지고 있는지.
    public int Health;
    public int Passion;
}

// 보너스 스킬들의 스크립트 클래스
public class BonusSkillScript
{
    public int Ability_ID;
    public string Ability_Name;
    public int Ability_Script_ID;
    public string Ability_Script;
}

// 퀘스트 데이터테이블
public class MissionData
{
    public int MissionID;               // 퀘스트 아이디
    public int MissionStep;             // 퀘스트 단계

    public bool IsMissionClear;         // 미션 클리어 유무

    public string MissionName;          // 퀘스트 이름
    public int MissionCondition;        // 퀘스트 조건
    public int ConditionAmountN;      // 퀘스트 조건의 양

    public int Reward1Index; // 보상 1 인덱스
    public int Reward1Amount; // 보상 1 양
    public int Reward2Index; // 보상 2 인덱스
    public int Reward2Amount; // 보상 2 양
}

// Test용
[FirestoreData]
public class City
{
    [FirestoreProperty] public string Name { get; set; }

    [FirestoreProperty] public string State { get; set; }

    [FirestoreProperty] public string Country { get; set; }

    [FirestoreProperty] public bool Capital { get; set; }

    [FirestoreProperty] public long Population { get; set; }

    [FirestoreProperty] public int[] TestArr { get; set; }

    [FirestoreProperty] public Dictionary<string, int> TestDic { get; set; }
}