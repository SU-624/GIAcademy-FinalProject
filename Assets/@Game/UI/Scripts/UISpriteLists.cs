using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 2023. 05. 11 Mang
/// 
/// 스프라이트들을 담아놓고 사용하기 위한 enum 모음들과 클래스
/// </summary>
public enum EDepartmentImgIndex     // 오름차순 - 학과 관련 이미지 모음
{
    art_icon_info,
    art_nametag_info,
    art_tab_notselect,
    art_tab_selected,

    gamedegisn_tab_notselect,
    gamedesign_icon_info,
    gamedesign_nametag_info,
    gamedesign_tab_selected,

    prof1_icon_info,
    prof2_icon_info,

    program_nametag_info,
    program_tab_notselect,
    program_tab_selected,
    programming_icon2_icon,
}

public enum ESpeakerImgIndex        // 현재 39명   - 이벤트 중 화자 모음
{
    AdFairy,                   // 어드벤처방 NPC
    ArtProf,                   // 아트 전임강사
    ArtRan,                    // 보유중인 학생 중 랜덤한 아트학생 1명
    Auditman,                  // 세무감사장

    Biopop,
    BTS,                       // 방탄복소년단
    Chairman,                  // 게임협회장
    Collector,                 // 골동품 수집가
    Creditor,                  // 채권자
                               
    DesignProf,                // 기획 전임강사
    DesignRan,                 // 보유중인 학생 중 랜덤한 기획학생 1명
    EZPZ,                      // EZPZ아카데미

    Gift,                      // 선물 상자
    Gletter,                   // 협회장 서신
    letter,                    // 편지
    NewsP,                     // 바니통신 신문
    None,                      // 화자없음
    NoneTemp,   
    
    PD,                        // 팀장
    Prof1,                     // 최웅성
    Prof10,                    // 냥비
    Prof11,                    // 믹헬란젤로
    Prof12,                    // 몬날리자
    Prof13,                    // 엠마부인
    Prof14,                    // 스티븐잡스
    Prof15,                    // NULL
    Prof16,                    // 꼬마아가씨
    Prof17,                    // 윗스턴
    Prof18,                    // 베타고
    Prof2,                     // 헤르마요니
    Prof3,                     // 오르페우스
    Prof4,                     // 아귀
    Prof5,                     // 이건히
    Prof6,                     // 에이멘
    Prof7,                     // 아티스트권
    Prof8,                     // 시저핸드
    Prof9,                     // 밥아저씨
                               
    ProProf,                   // 플밍전임강사
    ProRan,                    // 보유중인 학생 중 랜덤한 플밍학생 1명
    Pumpkin,                   // 할로윈 펌킨고스트
                              
    Reporter,                  // 바니통신 기자
    RhythmNPC,                 // 리듬방 npc
    RPGDragon,                 // rpg방 용
                              
    Salvation,                 // 구세군
    Santa,                     // 산타할아버지
    SelectedProf,              // 선택된 강사
    SelectedStu,               // 선택된 학생
    SweetGirl,                 // 스윗트걸

}

public enum EStudentSkillLevelImgIndex  // 현재 5개    - 학생스킬 레벨 이미지
{
    Arank_icon,
    Brank_icon,
    Crank_icon,
    Drank_icon,
    Srank_icon,
}

public enum EStudentImgIndex    // 현재 5명 (임시)   -   학생이미지인덱스
{
    student1,
    student2,
    student3,
    student4,
    student5,
    student6,
}

public enum EEmotionEmoji // 현재 3개   -   친밀도 감정 이미지(더미)
{
    big_smile,    // 베스트프렌드
    smile,        // 친한 사이
    wonder,       // 아는 사이
}

// 강사창에 뜰 이미지
public enum ETeacherProfile     // 현재 18개   -   현재 강사프로필
{
    NULL = 0,
    꼬마아가씨,
    냥비,
    몬날리자,
    믹헬란젤로,
    밥아저씨,
    베타고,
    스티븐잡스,
    시저핸드,
    아귀,
    아티스트권,
    에이멘,
    엠마부인,
    오르페우스,
    윗스턴,
    이건히,
    최웅성,
    헤르마요니,
}

public enum ERewardImg              // 25개
{
    Action,                 // 장르 - 액션
    Activity,               // 활동점수
    Adventure,              // 장르 - 어드벤쳐
    Awareness,              // 유명점수
    Concentration,          // 스탯 - 집중

    Gold,                   // 골드
    Health,
    Insight,                // 스탯 - 통찰력
    Management,             // 운영점수
    Passion,

    ProfessorExperience,    // 교사-경험치
    ProfessorHealth,        // 교사-체력
    ProfessorPassion,       // 교사-열정
    ProfessorPay,           // 교사-월급상승

    Puzzle,                 // 장르 - 퍼즐
    Rhythm,                 // 장르 - 리듬
    RPG,                    // 장르 -  RPG
    Rubby,                  // 루비
    Sense,                  // 스탯 - 감각

    Shooting,               // 장르 - 슈팅
    Simulation,             // 장르 -  시뮬레이션
    Sports,                 // 장르 -  스포츠
    TalentDevelopment,      // 인재양성
    Technique,              // 스탯 - 기술

    Wit,                    // 스탯 - 재치
}


public class UISpriteLists : MonoBehaviour
{
    private static UISpriteLists instance = null;

    public static UISpriteLists Instance
    {
        get
        {
            return instance;
        }
    }

    private List<Sprite> DepartmentIndexSpriteList = new List<Sprite>();       // 각 학과 관련 이미지
    private List<Sprite> EmotionEmojiSpriteList = new List<Sprite>();       // 각 학과 관련 이미지

    private List<Sprite> speakerSpriteList = new List<Sprite>();            // 이벤트 시나리오 화자
    private List<Sprite> StudentProfileSpriteList = new List<Sprite>();       // 각 학생 학과 관련 이미지

    private List<Sprite> StudentSkillLevelSpriteList = new List<Sprite>();     // 학생의 스킬레벨 이미지

    private List<Sprite> TeacherProfileSpriteList = new List<Sprite>();         // 모든 강사의 프로필 이미지 모음

    private List<Sprite> RewardSpriteList = new List<Sprite>();                 // 보상 이미지 모음

    // 스프라이트들이 담긴 폴더 이름변수들
    string DepartmentIndexSpriteFolder;
    string EmotionEmojiSpriteFolder;
    string SpeakerSpriteFolder;
    string StudentProfileListSpriteFolder;
    string StudentSkillGradeSpriteFolder;
    string TeacherProfileSpriteFolder;
    string EventRewardSpriteFolder;
    public List<Sprite> GetDepartmentIndexImgList
    {
        get { return DepartmentIndexSpriteList; }
        set { DepartmentIndexSpriteList = value; }
    }

    public List<Sprite> GetEmotionEmojiSpriteList
    {
        get { return EmotionEmojiSpriteList; }
        set { EmotionEmojiSpriteList = value; }
    }

    public List<Sprite> GetspeakerSpriteList
    {
        get { return speakerSpriteList; }
        set { speakerSpriteList = value; }
    }
    public List<Sprite> GetStudentProfileSpriteList
    {
        get { return StudentProfileSpriteList; }
        set { StudentProfileSpriteList = value; }
    }

    public List<Sprite> GetStudentSkillLevelImgList
    {
        get { return StudentSkillLevelSpriteList; }
        set { StudentSkillLevelSpriteList = value; }
    }

    public List<Sprite> GetTeacherProfileSpriteList
    {
        get { return TeacherProfileSpriteList; }
        set { TeacherProfileSpriteList = value; }
    }

    public List<Sprite> GetRewardSpriteList
    {
        get { return RewardSpriteList; }
        set { RewardSpriteList = value; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DepartmentIndexSpriteFolder = "DepartmentIndexSprite";
        EmotionEmojiSpriteFolder = "EmotionEmoji";
        SpeakerSpriteFolder = "SpeakerImg";
        StudentProfileListSpriteFolder = "StudentProfileListSprite";
        StudentSkillGradeSpriteFolder = "StudentSkillGradeImg";
        TeacherProfileSpriteFolder = "TeacherProfileSprite";
        EventRewardSpriteFolder = "EventRewardSprite";

        // InitializeSpriteLoad();
    }

    public void InitializeSpriteLoad()
    {
        DepartmentIndexSpriteFolder = "DepartmentIndexSprite";
        EmotionEmojiSpriteFolder = "EmotionEmoji";
        SpeakerSpriteFolder = "SpeakerImg";
        StudentProfileListSpriteFolder = "StudentProfileListSprite";
        StudentSkillGradeSpriteFolder = "StudentSkillGradeImg";
        TeacherProfileSpriteFolder = "TeacherProfileSprite";
        EventRewardSpriteFolder = "EventRewardSprite";

        LoadDepartmentIndexSprite();
        LoadEmotionEmojiSprite();
        LoadSpeakerSprite();
        LoadStudentProfileListSprite();
        LoadStudentSkillLevelSprite();
        LoadTeacherProfileSprite();
        LoadEventRewardSprite();
    }
    public void LoadDepartmentIndexSprite()
    {
        var r = Resources.LoadAll<Sprite>(DepartmentIndexSpriteFolder);

        foreach (var temp in r)
        {
            DepartmentIndexSpriteList.Add(temp);
        }

        DepartmentIndexSpriteList = DepartmentIndexSpriteList.OrderBy(i => i.name).ToList();        // 오름차순
    }

    public void LoadEmotionEmojiSprite()
    {
        var r = Resources.LoadAll<Sprite>(EmotionEmojiSpriteFolder);

        foreach (var temp in r)
        {
            EmotionEmojiSpriteList.Add(temp);
        }

        EmotionEmojiSpriteList = EmotionEmojiSpriteList.OrderBy(i => i.name).ToList();
    }

    public void LoadSpeakerSprite()
    {
        var r = Resources.LoadAll<Sprite>(SpeakerSpriteFolder);

        foreach (var temp in r)
        {
            speakerSpriteList.Add(temp);
        }

        speakerSpriteList = speakerSpriteList.OrderBy(i => i.name).ToList();
    }

    public void LoadStudentProfileListSprite()
    {
        var r = Resources.LoadAll<Sprite>(StudentProfileListSpriteFolder);

        foreach (var temp in r)
        {
            StudentProfileSpriteList.Add(temp);
        }

        StudentProfileSpriteList = StudentProfileSpriteList.OrderBy(i => i.name).ToList();
    }

    public void LoadStudentSkillLevelSprite()
    {
        var r = Resources.LoadAll<Sprite>(StudentSkillGradeSpriteFolder);

        foreach (var temp in r)
        {
            StudentSkillLevelSpriteList.Add(temp);
        }

        StudentSkillLevelSpriteList = StudentSkillLevelSpriteList.OrderBy(i => i.name).ToList();
    }

    public void LoadTeacherProfileSprite()
    {
        var r = Resources.LoadAll<Sprite>(TeacherProfileSpriteFolder);

        foreach (var temp in r)
        {
            TeacherProfileSpriteList.Add(temp);
        }

        TeacherProfileSpriteList = TeacherProfileSpriteList.OrderBy(i => i.name).ToList();
    }

    public void LoadEventRewardSprite()
    {
        var r = Resources.LoadAll<Sprite>(EventRewardSpriteFolder);

        foreach (var temp in r)
        {
            RewardSpriteList.Add(temp);
        }

        RewardSpriteList = RewardSpriteList.OrderBy(i => i.name).ToList();        // 오름차순
    }
}
