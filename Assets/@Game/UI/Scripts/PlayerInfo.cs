using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mang 11. 08
/// 
/// �÷��̾� ��ü�� ����
/// ���⿡ �÷��̾ ������� �����͵��� �� �־�θ� �������� ����������Ұ� ��
/// </summary>
public class PlayerInfo
{
    // 5/15 woodpie9 
    // public List<StudentSaveData> studentData = new List<StudentSaveData>();

    private static PlayerInfo instance = null; // Manager ������ �̱������� ���

    public static PlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfo();
            }

            return instance;
        }
    }

    private PlayerInfo()
    {
        AcademyName = null;
        PrincipalName = null;

        MyMoney = 0;
        SpecialPoint = 0;
        AcademyScore = 0;

        Famous = 0;
        TalentDevelopment = 0;
        Management = 0;
        Activity = 0;
        Goods = 0;
        CurrentRank = Rank.None;

        IsFirstAcademySetting = true;
        IsFirstClassSetting = false;
        IsFirstClassSettingPdEnd = false;
        IsFirstGameJam = true;
        IsFirstGameShow = true;
        IsFirstClassEnd = false;
        IsFirstInJaeRecommend = true;
        IsGameJamMiniGameFirst = true;

        IsMissionClear = new bool[30];
        IsGetReward = new bool[30];
        NowMissionStepCount = 0;
        IsAllMissionClear = false;

        StudentProfileClickCount = 0;
        TeacherProfileClickCount = 0;
        GenreRoomClickCount = 0;
        UnLockTeacherCount = 0;

        ParticipatedGameJamCount = 0;
        ParticipatedGameShowCount = 0;

        GameDesignerClassDic = new Dictionary<int, int>();
        ArtClassDic = new Dictionary<int, int>();
        ProgrammingClassDic = new Dictionary<int, int>();

        GameJamEntryCount = new Dictionary<string, int>();

        NeedGameDesignerStat = new Dictionary<string, int>();
        NeedArtStat = new Dictionary<string, int>();
        NeedProgrammingStat = new Dictionary<string, int>();

        ActionCenterCount = 0;
        SimulationCenterCount = 0;
        ShootingCenterCount = 0;
        RhythmCenterCount = 0;
        RPGCenterCount = 0;
        SportsCenterCount = 0;
        AdventureCenterCount = 0;
        PuzzleCenterCount = 0;
        
        LuckyBox = 0;
        ProfessorUpgrade = 0;

        isRankButtonOn = false;
    }

    // �α��� ������
    // public string m_PlayerID;  // �̻��...
    public string AcademyName;
    public string PrincipalName;

    public int MyMoney;
    public int SpecialPoint; // 2�� ��ȭ�Դϴ�
    public int AcademyScore;
    public string PlayerServer; // ���� ��𿡴� �����ߴ���?

    public int Famous; // ������ - ����
    public int TalentDevelopment; // ���� �缺
    public int Management; // �
    public int Activity; // Ȱ������
    public int Goods; // ��ȭ����
    public Rank CurrentRank;

    // Ʃ�丮�� ����
    public bool IsFirstAcademySetting;
    public bool IsFirstClassSetting;
    public bool IsFirstClassSettingPdEnd;
    public bool IsFirstGameJam;
    public bool IsFirstGameShow;
    public bool IsFirstClassEnd;
    public bool IsFirstInJaeRecommend;
    public bool IsUpgradePossible;
    public bool IsUpgradeTutorialEnd;
    public bool IsFirstVacation;
    public bool IsGameJamMiniGameFirst;

    // ����Ʈ
    public bool[] IsMissionClear; // �̼� Ŭ���� ������ ����
    public bool[] IsGetReward; // �̼� ������ �޾Ҵ��� ����
    public int NowMissionStepCount; // ���� �̼��� ���� ���� ����
    public bool IsAllMissionClear; // ��� �̼��� ���´��� üũ�ϴ� �Һ���

    // Ķ����
    public int StudentProfileClickCount; // �л� �� ������ Ȯ�� ī��Ʈ
    public int TeacherProfileClickCount; // ���� �� ������ Ȯ�� ī��Ʈ
    public int GenreRoomClickCount; // ��� �帣�� ��ġ ī��Ʈ
    public int UnLockTeacherCount;

    // ������ , ���Ӽ�
    public int ParticipatedGameJamCount; // ������ ���� ī��Ʈ
    public int ParticipatedGameShowCount; // ���Ӽ� ���� ī��Ʈ

    // �� �а����� �л����� ���� ������ ID�� Ű������ ���� ��ųʸ�. Value���� �ش� ID�� ������ �� ��������� �˷��ִ� ��.
    public Dictionary<int, int> GameDesignerClassDic;
    public Dictionary<int, int> ArtClassDic;
    public Dictionary<int, int> ProgrammingClassDic;

    // ������ ���� ���� ����
    public Dictionary<string, int> GameJamEntryCount;

    /// ���� �߰��� ������Ų �������� �ʿ佺�� 
    public Dictionary<string, int> NeedGameDesignerStat;

    public Dictionary<string, int> NeedArtStat;
    public Dictionary<string, int> NeedProgrammingStat;

    // Push Alarm On, Off �� ���� Ʈ����
    public bool isPushAlarmOn;

    // ���� ������ ���� ������ ���� ���� O
    public int ActionCenterCount;
    public int SimulationCenterCount;
    public int ShootingCenterCount;
    public int RhythmCenterCount;
    public int RPGCenterCount;
    public int SportsCenterCount;
    public int AdventureCenterCount;
    public int PuzzleCenterCount;

    // ������ �ӽ� ������. �������� X
    public int LuckyBox;
    public int ProfessorUpgrade;
    public int GameJamRankAUP;
    
    // ������ �帣���� ���� ������. ���� ���� O
    public double _genreBonusScore;
    public string _genreName; 
    public double m_GenreScore;
    public int m_MiniGameScore;

    public bool isRankButtonOn;
}