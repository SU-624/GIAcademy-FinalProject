using System.Collections.Generic;
using Firebase.Firestore;
using StatData.Runtime;

// �÷��̾�, �л�, ����, ��� ���� �̺�Ʈ ���, �߻� �̺�Ʈ ���, ���� ����, ���� ����
// ������ ���Ӽ�(� ���Ӽ�, ��� �򰡸� �޾Ҵ���) 


// 0612
[FirestoreData]
public class PlayerSaveData
{
    [FirestoreProperty] public string PrincipalName { get; set; } // ���� �̸�
    [FirestoreProperty] public string AcademyName { get; set; } // ��ī���� �̸�
    [FirestoreProperty] public int Money { get; set; } // ���� �Ӵ�
    [FirestoreProperty] public int SpecialPoint { get; set; } // ���� ����Ʈ

    [FirestoreProperty] public int Famous { get; set; } // ������
    [FirestoreProperty] public int TalentDevelopment { get; set; } // ���� �缺
    [FirestoreProperty] public int Management { get; set; } // �
    [FirestoreProperty] public int Activity { get; set; } // Ȱ������
    [FirestoreProperty] public int Goods { get; set; } // ��ȭ����
    [FirestoreProperty] public int AcademyScore { get; set; } // ��ī���� ����

    // Ʃ�丮�� ����
    [FirestoreProperty] public bool IsFirstAcademySetting { get; set; }
    [FirestoreProperty] public bool IsFirstClassSetting { get; set; }
    [FirestoreProperty] public bool IsFirstClassSettingPDEnd { get; set; }
    [FirestoreProperty] public bool IsFirstGameJam { get; set; }
    [FirestoreProperty] public bool IsFirstClassEnd { get; set; }

    // ����Ʈ
    [FirestoreProperty] public bool[] isMissionClear { get; set; }

    // ���� �ð�
    [FirestoreProperty] public int Year { get; set; }
    [FirestoreProperty] public int Month { get; set; }
    [FirestoreProperty] public int Week { get; set; }
    [FirestoreProperty] public int Day { get; set; }

}


// 0612
[FirestoreData]
public class StudentSaveData
{
    // �л��� �������ִ� �⺻ ����. (ü��, ����, ģ�е� ��)
    [FirestoreProperty] public string StudentID { get; set; } // �л� ���� ID �𵨸� ã�� ��
    [FirestoreProperty] public string StudentName { get; set; } // �л� �̸�
    [FirestoreProperty] public int Health { get; set; } // �л� ü�� �ִ� 100, �ּ� 0
    [FirestoreProperty] public int Passion { get; set; } // �л� ���� �ִ� 100, �ּ� 0
    [FirestoreProperty] public int Gender { get; set; } // �л� ����


    [FirestoreProperty] public StudentType StudentType { get; set; } // �� ��Ʈ

    // ��Ʈ���� �л����� ������ �ִ� ���� ����(Ư�� ��ġ�� �����ϸ� ��ų�� ����)
    [FirestoreProperty] public int[] AbilityAmountArr { get; set; }
    [FirestoreProperty] public int[] AbilitySkills { get; set; }
    [FirestoreProperty] public List<string> Skills { get; set; }
    [FirestoreProperty] public int[] GenreAmountArr { get; set; } // �帣

    [FirestoreProperty] public string Personality { get; set; } // ����
    [FirestoreProperty] public int NumberOfEntries { get; set; }
    [FirestoreProperty] public bool IsActiving { get; set; } // ���� �������̳� �̺�Ʈ���� ���õǾ� �İ�������
    [FirestoreProperty] public bool IsRecommend { get; set; } // ��õ�� �޾Ҵ��� �ƴ��� �Ǻ�

    [FirestoreProperty] public List<int> Friendship { get; set; } // �� �л��� ����Ǿ��ִ� ȣ����
    [FirestoreProperty] public int FriendshipIndex { get; set; } // ȣ������ �����ϱ� ���� �ε���
}


// 0612
[FirestoreData]
public class ProfessorSaveData
{
    [FirestoreProperty] public int ProfessorID { get; set; } // ���� ���� ID
    [FirestoreProperty] public string ProfessorName { get; set; } // ���� �̸�
    [FirestoreProperty] public StudentType ProfessorType { get; set; } // ���� �а� ����
    [FirestoreProperty] public string ProfessorSet { get; set; } // �ܷ�, ���Ӱ��� ����
    [FirestoreProperty] public int ProfessorPower { get; set; } // ���Ƿ�

    [FirestoreProperty] public int ProfessorExperience { get; set; } // ����ġ
    [FirestoreProperty] public List<string> ProfessorSkills { get; set; } // ��ų
    [FirestoreProperty] public int ProfessorPay { get; set; } // ����
    [FirestoreProperty] public int ProfessorHealth { get; set; } // ü��
    [FirestoreProperty] public int ProfessorPassion { get; set; } // ����
}

// ����� �̺�Ʈ ������ ������. 0612
// ����� ������ ������ ������ �ݺ��ؼ� ��� ���� ����
[FirestoreData]
public class UsedEventSaveData
{
    [FirestoreProperty] public int SuddenEventID { get; set; } // �̺�Ʈ�� ID
    [FirestoreProperty] public int YearData { get; set; } // ��� ������ �⵵
    [FirestoreProperty] public int MonthData { get; set; } // ��� ������ ��
    [FirestoreProperty] public int WeekData { get; set; } // ��� ������ ��
    [FirestoreProperty] public int DayData { get; set; } // ��� ������ ��
}

// ���� �����ϴ� �̺�Ʈ ��� 0612
[FirestoreData]
public class TodayEventSaveData
{
    [FirestoreProperty] public int SuddenEventID { get; set; } // �̺�Ʈ�� ID
}

// �Ϲݸ���, �Ϲݺ������, ��������, ��ũ���� ����
// MailManagement�� m_MailList�� ����.
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


// ���� ������ + 


public class GameJamInfo
{
    public int m_GjamID;
    public bool m_Fix;
    public string m_GjamName;
    public string m_GjamHost;
    public int m_GjamHostIcon;
    public string m_GjamDetailInfo;
    public int m_EntryFee;
    public int m_StudentHealth; // ������ ������ �Ҹ�Ǵ� �л� �⺻����
    public int m_StudentPassion; // ������ ������ �Ҹ�Ǵ� �л� �⺻����
    public string m_GjamMainGenre; // �������� �䱸�ϴ� ���� �帣
    public string m_GjamSubGenre; // �������� �䱸�ϴ� ���� �帣
    public string m_GjamConcept;
    public int m_GjamYear; // 0�� ��� �� �� �߻��ϴ� �������� �̺�Ʈ, -1�� Ȧ���⵵, -2�� ¦���⵵�� �߻�
    public int m_GjamMonth;
    public int m_GjamWeek;
    public int m_GjamTime; // �������� �� �ʵ��� ����� ���� ���� ����
    public int m_GjamAI_ID;

    public Dictionary<string, int>
        m_GjamNeedStatGM = new Dictionary<string, int>(); // ���� ���� �ϼ��� �ʿ��� ���������� ��(�� �а����� �־�� ��)

    public Dictionary<string, int>
        m_GjamNeedStatArt = new Dictionary<string, int>(); // ���� ���� �ϼ��� �ʿ��� ���������� ��(�� �а����� �־�� ��)

    public Dictionary<string, int>
        m_GjamNeedStatProgramming = new Dictionary<string, int>(); // ���� ���� �ϼ��� �ʿ��� ���������� ��(�� �а����� �־�� ��)

    public int ParticipationReward;
}

// ������õ ������ Ŭ����
public class InJaeRecommendData
{
    public int m_EmploymentID; // ä�� ����ID
    public string m_EmploymentName; // ä�� ���� ����

    public int m_CompanyID; // ȸ�� ID
    public string m_CompanyName; // ȸ�� �̸�
    public int m_CompanyGameNameID; // ȸ�� �����̸��� ���̵�(���� ����ϴ� ���� ����)
    public int m_CompanyIcon; // ȸ�� ������
    public int m_CompanyGrade_ID; // ȸ�� ��� ���̵�
    public string m_CompanyGrade; // ȸ�� ���

    public Dictionary<string, int> m_CompanyRequirementStats = new Dictionary<string, int>(); // ȸ��䱸����(����)
    public string m_CompanyRequirementSkill_ID; // ȸ�� �䱸����(��ų), ������ �⺻ ��ų�� 

    public bool m_CompanyUnLock; // ȭ�� �ر� ����

    //public int m_CompanyUnLockCondition;                                                      // ȸ�� �ر� ����(���)
    public string m_EmploymentYear; // ä�� ���� �߻��� �� ����, �ѹ� ������ 0���� �ٲ��ֱ�
    public string m_EmploymentText; // ä�� ���� ����
    public int m_CompanySalary; // ����
    public string m_CompanyJob; // ���� ����
    public int m_CompanyJob_ID; // ���� ����
    public int m_CompanyPart; // ���� ��Ʈ
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

// ���� �˶��� ���� �� ���� ��ũ��Ʈ
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

// ��ũ�� ��������� ������ ������ ���ϵ��� ����(�Ϲݸ���, �Ϲݺ������, ��������)
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

// ��ũ ���� ������ ������ �⺻ ���ϰ� ���� �޶� ���� ����
// 1�⿡ �ѹ�
public class RewardEmailData
{
    public int Email_ID;
    public int Email_sender_id;
    public int Email_script_id;
    public string Email_sender_Name;
    public string Email_script_Text;
    public string Email_image_id;
}

// �л��� ���ʽ� ��ų�� ���� ���ǵ��� Ŭ����
public class BonusSkillConditions
{
    public int Ability_ID;
    public string Ability_Name;
    public int Ability_Script_ID;
    public int StudentPart_ID;
    public int[] State;

    public int[] Genre;

    // ������ ���� ��ų ���ǵ�
    public int GamedevelopmentNumber; // �����뿡 Nȸ �������� ��.
    public string GameGenre_ID; // Ư�� �帣 ���̵�
    public int GameGenre_DPNumber; // Ư�� �帣 ���̵��� ������ Nȸ �������� ���.
    public string[] GameJameLevel; // Ư�� ����� ������ �������� ���.(��������)
    public string GameConcept_ID; // Ư�� ���� ���̵�

    public int GameConcept_DPNumber; // Ư�� ������ ������ �������� ���

    // ���Ӽ� ���� ��ų ����
    public int[] GameShow_Level; // Ư�� ���̵��� �����뿡�� ���� �̻��� ����� �޾��� ��(���̵� ��������)

    // ���� ���� ��ų ���ǵ�
    public int ClassID; // Ư�� ���� ���̵�
    public int ClassType; // ������ Ÿ��(���� : 0, ��ȹ : 1, ��Ʈ : 2, �ù� : 3, ������ -1)
    public int Numberoflessons; // Ư�� ���̵��� ������ Nȸ ����� ���.

    // ģ�е� ���� ��ų ���ǵ�
    public string Intimacy; // �䱸�ϴ� ģ�е�
    public int Intimacy_Number; // �� ���� ĳ���Ϳ� �䱸�ϴ� ģ�е��� ������ �ִ���.
    public int Health;
    public int Passion;
}

// ���ʽ� ��ų���� ��ũ��Ʈ Ŭ����
public class BonusSkillScript
{
    public int Ability_ID;
    public string Ability_Name;
    public int Ability_Script_ID;
    public string Ability_Script;
}

// ����Ʈ ���������̺�
public class MissionData
{
    public int MissionID;               // ����Ʈ ���̵�
    public int MissionStep;             // ����Ʈ �ܰ�

    public bool IsMissionClear;         // �̼� Ŭ���� ����

    public string MissionName;          // ����Ʈ �̸�
    public int MissionCondition;        // ����Ʈ ����
    public int ConditionAmountN;      // ����Ʈ ������ ��

    public int Reward1Index; // ���� 1 �ε���
    public int Reward1Amount; // ���� 1 ��
    public int Reward2Index; // ���� 2 �ε���
    public int Reward2Amount; // ���� 2 ��
}

// Test��
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