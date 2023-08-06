using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mang 11. 08
/// 
/// 플레이어 자체의 정보
/// 여기에 플레이어가 들고있을 데이터들을 다 넣어두면 최종본의 데이터저장소가 됨
/// </summary>
public class PlayerInfo
{
    // 5/15 woodpie9 
    // public List<StudentSaveData> studentData = new List<StudentSaveData>();

    private static PlayerInfo instance = null; // Manager 변수는 싱글톤으로 사용

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

    // 로그인 데이터
    // public string m_PlayerID;  // 미사용...
    public string AcademyName;
    public string PrincipalName;

    public int MyMoney;
    public int SpecialPoint; // 2차 재화입니다
    public int AcademyScore;
    public string PlayerServer; // 서버 어디에다 저장했는지?

    public int Famous; // 인지도 - 유명
    public int TalentDevelopment; // 인재 양성
    public int Management; // 운영
    public int Activity; // 활동점수
    public int Goods; // 재화점수
    public Rank CurrentRank;

    // 튜토리얼 여부
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

    // 퀘스트
    public bool[] IsMissionClear; // 미션 클리어 유무만 저장
    public bool[] IsGetReward; // 미션 보상을 받았는지 저장
    public int NowMissionStepCount; // 현재 미션의 스텝 정보 저장
    public bool IsAllMissionClear; // 모든 미션을 끝냈는지 체크하는 불변수

    // 캘린더
    public int StudentProfileClickCount; // 학생 상세 프로필 확인 카운트
    public int TeacherProfileClickCount; // 강사 상세 프로필 확인 카운트
    public int GenreRoomClickCount; // 모든 장르방 터치 카운트
    public int UnLockTeacherCount;

    // 게임잼 , 게임쇼
    public int ParticipatedGameJamCount; // 게임잼 참여 카운트
    public int ParticipatedGameShowCount; // 게임쇼 참여 카운트

    // 각 학과마다 학생들이 들은 수업의 ID를 키값으로 갖는 딕셔너리. Value값은 해당 ID의 수업을 몇 번들었는지 알려주는 값.
    public Dictionary<int, int> GameDesignerClassDic;
    public Dictionary<int, int> ArtClassDic;
    public Dictionary<int, int> ProgrammingClassDic;

    // 게임잼 참여 수업 정보
    public Dictionary<string, int> GameJamEntryCount;

    /// 새로 추가된 참여시킨 게임잼의 필요스탯 
    public Dictionary<string, int> NeedGameDesignerStat;

    public Dictionary<string, int> NeedArtStat;
    public Dictionary<string, int> NeedProgrammingStat;

    // Push Alarm On, Off 를 위한 트리거
    public bool isPushAlarmOn;

    // 업적 적용을 위한 데이터 서버 저장 O
    public int ActionCenterCount;
    public int SimulationCenterCount;
    public int ShootingCenterCount;
    public int RhythmCenterCount;
    public int RPGCenterCount;
    public int SportsCenterCount;
    public int AdventureCenterCount;
    public int PuzzleCenterCount;

    // 업적용 임시 데이터. 서버저장 X
    public int LuckyBox;
    public int ProfessorUpgrade;
    public int GameJamRankAUP;
    
    // 게임잼 장르정보 버그 방지용. 서버 저장 O
    public double _genreBonusScore;
    public string _genreName; 
    public double m_GenreScore;
    public int m_MiniGameScore;

    public bool isRankButtonOn;
}