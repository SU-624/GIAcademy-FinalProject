using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 2023. 05. 11 Mang
/// 
/// ��������Ʈ���� ��Ƴ��� ����ϱ� ���� enum ������� Ŭ����
/// </summary>
public enum EDepartmentImgIndex     // �������� - �а� ���� �̹��� ����
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

public enum ESpeakerImgIndex        // ���� 39��   - �̺�Ʈ �� ȭ�� ����
{
    AdFairy,                   // ��庥ó�� NPC
    ArtProf,                   // ��Ʈ ���Ӱ���
    ArtRan,                    // �������� �л� �� ������ ��Ʈ�л� 1��
    Auditman,                  // ����������

    Biopop,
    BTS,                       // ��ź���ҳ��
    Chairman,                  // ������ȸ��
    Collector,                 // ��ǰ ������
    Creditor,                  // ä����
                               
    DesignProf,                // ��ȹ ���Ӱ���
    DesignRan,                 // �������� �л� �� ������ ��ȹ�л� 1��
    EZPZ,                      // EZPZ��ī����

    Gift,                      // ���� ����
    Gletter,                   // ��ȸ�� ����
    letter,                    // ����
    NewsP,                     // �ٴ���� �Ź�
    None,                      // ȭ�ھ���
    NoneTemp,   
    
    PD,                        // ����
    Prof1,                     // �ֿ���
    Prof10,                    // �ɺ�
    Prof11,                    // ���������
    Prof12,                    // �󳯸���
    Prof13,                    // ��������
    Prof14,                    // ��Ƽ���⽺
    Prof15,                    // NULL
    Prof16,                    // �����ư���
    Prof17,                    // ������
    Prof18,                    // ��Ÿ��
    Prof2,                     // �츣�����
    Prof3,                     // ������콺
    Prof4,                     // �Ʊ�
    Prof5,                     // �̰���
    Prof6,                     // ���̸�
    Prof7,                     // ��Ƽ��Ʈ��
    Prof8,                     // �����ڵ�
    Prof9,                     // �������
                               
    ProProf,                   // �ù����Ӱ���
    ProRan,                    // �������� �л� �� ������ �ù��л� 1��
    Pumpkin,                   // �ҷ��� ��Ų��Ʈ
                              
    Reporter,                  // �ٴ���� ����
    RhythmNPC,                 // ����� npc
    RPGDragon,                 // rpg�� ��
                              
    Salvation,                 // ������
    Santa,                     // ��Ÿ�Ҿƹ���
    SelectedProf,              // ���õ� ����
    SelectedStu,               // ���õ� �л�
    SweetGirl,                 // ����Ʈ��

}

public enum EStudentSkillLevelImgIndex  // ���� 5��    - �л���ų ���� �̹���
{
    Arank_icon,
    Brank_icon,
    Crank_icon,
    Drank_icon,
    Srank_icon,
}

public enum EStudentImgIndex    // ���� 5�� (�ӽ�)   -   �л��̹����ε���
{
    student1,
    student2,
    student3,
    student4,
    student5,
    student6,
}

public enum EEmotionEmoji // ���� 3��   -   ģ�е� ���� �̹���(����)
{
    big_smile,    // ����Ʈ������
    smile,        // ģ�� ����
    wonder,       // �ƴ� ����
}

// ����â�� �� �̹���
public enum ETeacherProfile     // ���� 18��   -   ���� ����������
{
    NULL = 0,
    �����ư���,
    �ɺ�,
    �󳯸���,
    ���������,
    �������,
    ��Ÿ��,
    ��Ƽ���⽺,
    �����ڵ�,
    �Ʊ�,
    ��Ƽ��Ʈ��,
    ���̸�,
    ��������,
    ������콺,
    ������,
    �̰���,
    �ֿ���,
    �츣�����,
}

public enum ERewardImg              // 25��
{
    Action,                 // �帣 - �׼�
    Activity,               // Ȱ������
    Adventure,              // �帣 - ��庥��
    Awareness,              // ��������
    Concentration,          // ���� - ����

    Gold,                   // ���
    Health,
    Insight,                // ���� - ������
    Management,             // �����
    Passion,

    ProfessorExperience,    // ����-����ġ
    ProfessorHealth,        // ����-ü��
    ProfessorPassion,       // ����-����
    ProfessorPay,           // ����-���޻��

    Puzzle,                 // �帣 - ����
    Rhythm,                 // �帣 - ����
    RPG,                    // �帣 -  RPG
    Rubby,                  // ���
    Sense,                  // ���� - ����

    Shooting,               // �帣 - ����
    Simulation,             // �帣 -  �ùķ��̼�
    Sports,                 // �帣 -  ������
    TalentDevelopment,      // ����缺
    Technique,              // ���� - ���

    Wit,                    // ���� - ��ġ
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

    private List<Sprite> DepartmentIndexSpriteList = new List<Sprite>();       // �� �а� ���� �̹���
    private List<Sprite> EmotionEmojiSpriteList = new List<Sprite>();       // �� �а� ���� �̹���

    private List<Sprite> speakerSpriteList = new List<Sprite>();            // �̺�Ʈ �ó����� ȭ��
    private List<Sprite> StudentProfileSpriteList = new List<Sprite>();       // �� �л� �а� ���� �̹���

    private List<Sprite> StudentSkillLevelSpriteList = new List<Sprite>();     // �л��� ��ų���� �̹���

    private List<Sprite> TeacherProfileSpriteList = new List<Sprite>();         // ��� ������ ������ �̹��� ����

    private List<Sprite> RewardSpriteList = new List<Sprite>();                 // ���� �̹��� ����

    // ��������Ʈ���� ��� ���� �̸�������
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

        DepartmentIndexSpriteList = DepartmentIndexSpriteList.OrderBy(i => i.name).ToList();        // ��������
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

        RewardSpriteList = RewardSpriteList.OrderBy(i => i.name).ToList();        // ��������
    }
}
