using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

/// <summary>
/// 2023. 03. 15 Mang
///
/// 캐릭터 클릭 시 띄워줄 정보창의 오브젝트들과 그 오브젝트의 값이 변동하는 것을 하기 위한 
/// </summary>
public class CharacterInfoPanel : MonoBehaviour
{
    // [Tooltip("CharacterInfoPanel의 변동이 있는 모든 오브젝트를 담은 변수들")]
    // [Header("EventList Prefab")]
    [Header("학생 정보창의 학과 인덱스 변수들")]
    [Space(5f)]
    [SerializeField] private Image StudentInfoPage1;
    [Space(5f)]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;
    [Space(5f)]
    [SerializeField] private Button LeftArrowButton;
    [SerializeField] private Button RightArrowButton;

    [Space(10f)]
    [Header("학생의 기본 정보 변수들")]
    [SerializeField] private Image CharacterImg1;
    [SerializeField] private TextMeshProUGUI CharacterName1;
    [Space(5f)]
    [SerializeField] private Image DepartmentImg1;
    // [SerializeField] private Image StudentTerm;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI PassionValue1;
    [SerializeField] private TextMeshProUGUI HealthValue1;

    [Space(10f)]
    [Header("Student가 가진 기본 스탯들(감각, 재치, 집중, 기술, 통찰)")]
    [SerializeField] private Image SenseCheck;
    [SerializeField] private TextMeshProUGUI SenseValue;              // 감각
    [SerializeField] private Button SenseValueButton;

    [Space(5f)]
    [SerializeField] private Image WitCheck;
    [SerializeField] private TextMeshProUGUI WitValue;                // 재치
    [SerializeField] private Button WitValueButton;

    [Space(5f)]
    [SerializeField] private Image ConcentrationCheck;
    [SerializeField] private TextMeshProUGUI ConcentrationValue;      // 집중
    [SerializeField] private Button ConcentartionValueButton;

    [Space(5f)]
    [SerializeField] private Image TechnologyCheck;
    [SerializeField] private TextMeshProUGUI TechnologyValue;         // 기술
    [SerializeField] private Button TechnologyValueButton;

    [Space(5f)]
    [SerializeField] private Image InsightCheck;
    [SerializeField] private TextMeshProUGUI InsightValue;            // 통찰
    [SerializeField] private Button InsightValueButton;

    [Space(10f)]
    [Header("Student가 가진 스탯 연계 스킬의 세부 내용?")]                    // 
    [SerializeField] private Image StatGradeImg;
    [SerializeField] private TextMeshProUGUI DetailedStatName;
    [SerializeField] private TextMeshProUGUI DetailedStatValue;
    [Space(5f)]
    [SerializeField] private Image StudentPersonalityImg;                   // 학생 성격 이미지
    [SerializeField] private TextMeshProUGUI StudentPersonalityName;        // 이름
    [SerializeField] private TextMeshProUGUI StudentPersonalityInfo;        // 설명

    [Space(15f)]
    [SerializeField] private Image StudentStatPage2;
    [Space(10f)]
    [Header("Student가 가진 장르 스탯들(rpg, 액션, 시뮬, 슈팅, 리듬, 어드벤처, 퍼즐, 스포츠)")]
    [SerializeField] private Image CharacterImg2;
    [SerializeField] private TextMeshProUGUI CharacterName2;
    [Space(5f)]
    [SerializeField] private Image DepartmentImg2;
    // [SerializeField] private Image StudentTerm;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI PassionValue2;
    [SerializeField] private TextMeshProUGUI HealthValue2;

    [Space(5f)]
    [SerializeField] private TextMeshProUGUI RPGStatValue;
    [SerializeField] private TextMeshProUGUI ActionStatValue;
    [SerializeField] private TextMeshProUGUI SimulationStatValue;
    [SerializeField] private TextMeshProUGUI ShootingStatValue;
    [SerializeField] private TextMeshProUGUI RhythmStatValue;
    [SerializeField] private TextMeshProUGUI AdventureStatValue;
    [SerializeField] private TextMeshProUGUI PuzzleStatValue;
    [SerializeField] private TextMeshProUGUI SportsStatValue;

    [Space(5f)]
    [Header("학생의 친밀도 페이지에 정보를 넣기 위해 필요한 변수")]
    [SerializeField] private RectTransform ParentFriendshipPrefab;
    [SerializeField] private List<StudentFriendshipPrefab> FriendshipInfoList;

    Dictionary<int, int> FriendshipDic = new Dictionary<int, int>();
    Dictionary<Instructor, int> TeacherFriendshipDic = new Dictionary<Instructor, int>();

    List<FriendshipOption> FriendshipList = new List<FriendshipOption>();

    [Space(5f)]
    [SerializeField] private Button QuitButton;

    [SerializeField] private PopOffUI popOffStudentPanel;
    [Space(5f)]
    [SerializeField] private Transform StudentBonusSkillParent;


    private List<Sprite> DepartmentIndexImgList = new List<Sprite>();
    private List<Sprite> StudentSkillLevelImgList = new List<Sprite>();

    [SerializeField] private StatsRadar StudentStatsRader;

    private List<SkillSpriteByStatAmount> skillSpriteByStatAmounts = new List<SkillSpriteByStatAmount>();
    #region 프로퍼티들
    public Button GetGameDesignerIndexButton
    {
        get { return GameDesignerIndexButton; }
        set { GameDesignerIndexButton = value; }
    }
    public Button GetArtIndexButton
    {
        get { return ArtIndexButton; }
        set { ArtIndexButton = value; }
    }
    public Button GetProgrammingIndexButton
    {
        get { return ProgrammingIndexButton; }
        set { ProgrammingIndexButton = value; }
    }

    // 학과 이미지
    public Image GetDepartmentImg1
    {
        get { return DepartmentImg1; }
        set { DepartmentImg1 = value; }
    }

    public Image GetDepartmentImg2
    {
        get { return DepartmentImg2; }
        set { DepartmentImg2 = value; }
    }

    public TextMeshProUGUI GetCharacterName1
    {
        get { return CharacterName1; }
        set { CharacterName1 = value; }
    }

    // 캐릭터 프로필 이미지
    public Image GetCharacterImg1
    {
        get { return CharacterImg1; }
        set { CharacterImg1 = value; }
    }

    public Image GetCharacterImg2
    {
        get { return CharacterImg2; }
        set { CharacterImg2 = value; }
    }

    public TextMeshProUGUI GetCharacterName2
    {
        get { return CharacterName2; }
        set { CharacterName2 = value; }
    }

    //public Image GetStudentTerm
    //{
    //get { return StudentTerm; }
    //set { StudentTerm = value; }
    //}

    // ------------------------------
    public TextMeshProUGUI GetPassionValue1
    {
        get { return PassionValue1; }
        set { PassionValue1 = value; }
    }
    public TextMeshProUGUI GetHealthValue1
    {
        get { return HealthValue1; }
        set { HealthValue1 = value; }
    }

    public TextMeshProUGUI GetPassionValue2
    {
        get { return PassionValue2; }
        set { PassionValue2 = value; }
    }
    public TextMeshProUGUI GetHealthValue2
    {
        get { return HealthValue2; }
        set { HealthValue2 = value; }
    }

    // ------------------------------
    // 학생의 다섯가지 스탯
    public Image GetSenseCheck
    {
        get { return SenseCheck; }
        set { SenseCheck = value; }
    }

    public Image GetWitCheck
    {
        get { return WitCheck; }
        set { WitCheck = value; }
    }
    public Image GetConcentrationCheck
    {
        get { return ConcentrationCheck; }
        set { ConcentrationCheck = value; }
    }
    public Image GetTechnologyCheck
    {
        get { return TechnologyCheck; }
        set { TechnologyCheck = value; }
    }
    public Image GetInsightCheck
    {
        get { return InsightCheck; }
        set { InsightCheck = value; }
    }

    public TextMeshProUGUI GetSenseValue
    {
        get { return SenseValue; }
        set { SenseValue = value; }
    }

    public TextMeshProUGUI GetWitValue
    {
        get { return WitValue; }
        set { WitValue = value; }
    }

    public TextMeshProUGUI GetConcentrationValue
    {
        get { return ConcentrationValue; }
        set { ConcentrationValue = value; }
    }

    public TextMeshProUGUI GetTechnologyValue
    {
        get { return TechnologyValue; }
        set { TechnologyValue = value; }
    }

    public TextMeshProUGUI GetInsightValue
    {
        get { return InsightValue; }
        set { InsightValue = value; }
    }
    // ------------------------------
    public Image GetStudentPersonalityImg
    {
        get { return StudentPersonalityImg; }
        set { StudentPersonalityImg = value; }
    }

    public TextMeshProUGUI GetStudentPersonalityName
    {
        get { return StudentPersonalityName; }
        set { StudentPersonalityName = value; }
    }

    public TextMeshProUGUI GetStudentPersonalityInfo
    {
        get { return StudentPersonalityInfo; }
        set { StudentPersonalityInfo = value; }
    }
    // ------------------------------
    public TextMeshProUGUI GetRPGStatValue
    {
        get { return RPGStatValue; }
        set { RPGStatValue = value; }
    }

    public TextMeshProUGUI GetActionStatValue
    {
        get { return ActionStatValue; }
        set { ActionStatValue = value; }
    }

    public TextMeshProUGUI GetSimulationStatValue
    {
        get { return SimulationStatValue; }
        set { SimulationStatValue = value; }
    }

    public TextMeshProUGUI GetShootingStatValue
    {
        get { return ShootingStatValue; }
        set { ShootingStatValue = value; }
    }

    public TextMeshProUGUI GetRhythmStatValue
    {
        get { return RhythmStatValue; }
        set { RhythmStatValue = value; }
    }

    public TextMeshProUGUI GetAdventureStatValue
    {
        get { return AdventureStatValue; }
        set { AdventureStatValue = value; }
    }

    public TextMeshProUGUI GetPuzzleStatValue
    {
        get { return PuzzleStatValue; }
        set { PuzzleStatValue = value; }
    }
    public TextMeshProUGUI GetSportsStatValue
    {
        get { return SportsStatValue; }
        set { SportsStatValue = value; }
    }

    // ------------------------------
    public Button GetStudentInfoQuitButton
    {
        get { return QuitButton; }
        set { QuitButton = value; }
    }

    public List<Sprite> GetDepartmentIndexImgList
    {
        get { return DepartmentIndexImgList; }
        set { DepartmentIndexImgList = value; }
    }

    public List<Sprite> GetStudentSkillLevelImgList
    {
        get { return StudentSkillLevelImgList; }
        set { StudentSkillLevelImgList = value; }
    }

    public TextMeshProUGUI GetDetailedStatName
    {
        get { return DetailedStatName; }
        set { DetailedStatName = value; }
    }

    public TextMeshProUGUI GetDetailedStatValue
    {
        get { return DetailedStatValue; }
        set { DetailedStatValue = value; }
    }

    public Image GetStatGradeImg
    {
        get { return StatGradeImg; }
        set { StatGradeImg = value; }
    }

    public Image GetStudentInfoPage1
    {
        get { return StudentInfoPage1; }
        set { StudentInfoPage1 = value; }
    }

    public Image GetStudentStatPage2
    {
        get { return StudentStatPage2; }
        set { StudentStatPage2 = value; }
    }

    public Button GetLeftArrowButton
    {
        get { return LeftArrowButton; }
        set { LeftArrowButton = value; }
    }

    public Button GetRightArrowButton
    {
        get { return RightArrowButton; }
        set { RightArrowButton = value; }
    }

    public RectTransform GetParentFriendshipPrefab
    {
        get { return ParentFriendshipPrefab; }
        set { ParentFriendshipPrefab = value; }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Initialize()
    {
        // GameDesignerIndexButton.onClick.AddListener(IfClickEachIndexButton);
        // ArtIndexButton.onClick.AddListener(IfClickEachIndexButton);
        // ProgrammingIndexButton.onClick.AddListener(IfClickEachIndexButton);

        SenseValueButton.onClick.AddListener(IfClickEachStatButton);
        WitValueButton.onClick.AddListener(IfClickEachStatButton);
        ConcentartionValueButton.onClick.AddListener(IfClickEachStatButton);
        TechnologyValueButton.onClick.AddListener(IfClickEachStatButton);
        InsightValueButton.onClick.AddListener(IfClickEachStatButton);

        LeftArrowButton.onClick.AddListener(IfClickArrowButton);
        RightArrowButton.onClick.AddListener(IfClickArrowButton);
    }

    // 캐릭터의 기본 정보를 담아서 보여줄 함수
    public void ShowStudentBasicInfo(GameObject CharacterObj)
    {
        Student _temp = CharacterObj.GetComponent<Student>();
        StudentStat tempStat = _temp.m_StudentStat;
        StudentType tempType = _temp.m_StudentStat.m_StudentType;

        int sense = tempStat.m_AbilityAmountList[(int)AbilityType.Sense];
        int concentration = tempStat.m_AbilityAmountList[(int)AbilityType.Concentration];
        int wit = tempStat.m_AbilityAmountList[(int)AbilityType.Wit];
        int technique = tempStat.m_AbilityAmountList[(int)AbilityType.Technique];
        int insight = tempStat.m_AbilityAmountList[(int)AbilityType.Insight];

        // 학생의 개인 성격 띄우기
        string peronalityInfoScript = ObjectManager.Instance.GetPersonalityScript(tempStat.m_Personality);
        StudentPersonalityName.text = tempStat.m_Personality;
        StudentPersonalityInfo.text = peronalityInfoScript;

        CharacterName1.text = CharacterObj.name;                 // 캐릭터 이름
        CharacterName2.text = CharacterObj.name;                 // 캐릭터 이름

        StudentInfoPage1.gameObject.SetActive(true);
        LeftArrowButton.gameObject.SetActive(false);
        StudentStatPage2.gameObject.SetActive(false);
        RightArrowButton.gameObject.SetActive(true);

        MakeStudentStatPentagon(sense, concentration, wit, technique, insight);

        float x = ParentFriendshipPrefab.anchoredPosition.x;
        ParentFriendshipPrefab.anchoredPosition = new Vector3(x, 0, 0);

        switch (tempType)                                       // 캐릭터 학과
        {
            case StudentType.GameDesigner:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];

                    DepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];
                    DepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];
                }
                break;
            case StudentType.Art:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];

                    DepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];
                    DepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];
                }
                break;
            case StudentType.Programming:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];

                    DepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];
                    DepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];
                }
                break;
        }

        // 캐릭터 연차

        PassionValue1.text = tempStat.m_Passion.ToString();      // 캐릭터 열정
        PassionValue2.text = tempStat.m_Passion.ToString();      // 캐릭터 열정
        HealthValue1.text = tempStat.m_Health.ToString();         // 캐릭터 체력
        HealthValue2.text = tempStat.m_Health.ToString();         // 캐릭터 체력

        // StudentPersonalityName.text = tempStat.m_Personality.ToString();

        ReadyFriendshipData(_temp);      // 여기서 각 학생이 가진 친밀도 부분 정보를 찾음
        SetFriendshipInfo();
    }

    // 캐릭터의 스탯을 담아서 보여 줄 함수
    public void ShowCharacterDetailedStat(GameObject CharacterObj)
    {
        Student _temp = CharacterObj.GetComponent<Student>();
        StudentStat tempStat = _temp.m_StudentStat;
        int tempLevel = 0;

        // 학생이 가진 다섯가지 기본 스탯
        SenseValue.text = tempStat.m_AbilityAmountList[(int)AbilityType.Sense].ToString();
        ConcentrationValue.text = tempStat.m_AbilityAmountList[(int)AbilityType.Concentration].ToString();
        WitValue.text = tempStat.m_AbilityAmountList[(int)AbilityType.Wit].ToString();
        TechnologyValue.text = tempStat.m_AbilityAmountList[(int)AbilityType.Technique].ToString();
        InsightValue.text = tempStat.m_AbilityAmountList[(int)AbilityType.Insight].ToString();

        RPGStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.RPG].ToString();
        ActionStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Action].ToString();
        SimulationStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Simulation].ToString();
        ShootingStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Shooting].ToString();

        RhythmStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Rhythm].ToString();
        AdventureStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Adventure].ToString();
        PuzzleStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Puzzle].ToString();
        SportsStatValue.text = tempStat.m_GenreAmountList[(int)GenreStat.Sports].ToString();

        SenseCheck.gameObject.SetActive(true);
        ConcentrationCheck.gameObject.SetActive(false);
        WitCheck.gameObject.SetActive(false);
        TechnologyCheck.gameObject.SetActive(false);
        InsightCheck.gameObject.SetActive(false);

        switch (tempStat.m_StudentType)
        {
            case StudentType.GameDesigner:
                {
                    tempLevel = SetSkillLevel(tempStat.m_AbilityAmountList[(int)AbilityType.Sense]);

                    DetailedStatName.text = "창의력";
                    DetailedStatValue.text = "Lv. " + tempLevel;
                    StatGradeImg.sprite = SetSkillLevelImg(tempLevel);
                }
                break;
            case StudentType.Art:
                {
                    tempLevel = SetSkillLevel(tempStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                    DetailedStatName.text = "표현력";
                    DetailedStatValue.text = "Lv. " + tempLevel;
                    StatGradeImg.sprite = SetSkillLevelImg(tempLevel);
                }
                break;
            case StudentType.Programming:
                {
                    tempLevel = SetSkillLevel(tempStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                    StatGradeImg.sprite = SetSkillLevelImg(tempLevel);
                    DetailedStatName.text = "활용력";
                    DetailedStatValue.text = "Lv. " + tempLevel;
                }
                break;
        }
    }

    public void IfClickStudentPanelQuitButton()
    {
        popOffStudentPanel.TurnOffUI();

        if (StudentBonusSkillParent.childCount != 0)
        {
            for (int i = StudentBonusSkillParent.childCount; i > 0; i--)
            {
                StudentBonusSkillObjectPool.ReturnStudentBonusSkillPrefab(StudentBonusSkillParent.GetChild(0).gameObject);
            }
        }
    }

    // 학생의 각 세부 스탯 버튼을 누르면 해당 학생 찾기, 학생의 학과 구별, 학과별 스킬 찾기, 텍스트 띄우기
    public void IfClickEachStatButton()
    {
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        int nowStudentCount = ObjectManager.Instance.m_StudentList.Count;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;

        for (int i = 0; i < nowStudentCount; i++)
        {
            if (tempData[i].m_StudentStat.m_StudentName == CharacterName1.text)
            {
                int tempLevel = 0;
                StudentType tempType = tempData[i].m_StudentStat.m_StudentType;

                SenseCheck.gameObject.SetActive(false);
                WitCheck.gameObject.SetActive(false);
                ConcentrationCheck.gameObject.SetActive(false);
                TechnologyCheck.gameObject.SetActive(false);
                InsightCheck.gameObject.SetActive(false);

                switch (tempType)                // 이 학생의 학과 구별 (학과별로 상세 스킬이 달라짐)
                {
                    case StudentType.GameDesigner:
                        {
                            // 이 학생을 눌렀을 때 -> 해당 학생이라는 것을 알고, 해당 학생정보창에 띄울 때 기본 설정 -> 첫번째 버튼 선택 된 상태로. 이건 학생 버튼 클릭 될때 설정
                            // 해당 학생의 스탯 (ex ) 감각) 클릭 시 감각에 맞는 스킬(창의력) 글씨 뜨기

                            if (nowButton.name == "SenseValueButton")       // 감각
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                SenseCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "창의력";
                            }
                            else if (nowButton.name == "ConcentrationValueButton")  // 집중
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                ConcentrationCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "이해력";
                            }
                            else if (nowButton.name == "WitValueButton")        //재치
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                WitCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "분석력";
                            }
                            else if (nowButton.name == "TechnologyValueButton")     // 기술
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                TechnologyCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "소통력";
                            }
                            else if (nowButton.name == "InsightValueButton")        // 연구
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                InsightCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "사업력";
                            }
                        }
                        break;
                    case StudentType.Art:
                        {
                            if (nowButton.name == "SenseValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                SenseCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "표현력";
                            }
                            else if (nowButton.name == "ConcentrationValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                ConcentrationCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "상상력";
                            }
                            else if (nowButton.name == "WitValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                WitCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "색감각";
                            }
                            else if (nowButton.name == "TechnologyValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                TechnologyCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "테크닉";
                            }
                            else if (nowButton.name == "InsightValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                InsightCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "공간감";
                            }
                        }
                        break;
                    case StudentType.Programming:
                        {
                            if (nowButton.name == "SenseValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                SenseCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "활용력";
                            }
                            else if (nowButton.name == "ConcentrationValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                ConcentrationCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "구현력";
                            }
                            else if (nowButton.name == "WitValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                WitCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "논리력";
                            }
                            else if (nowButton.name == "TechnologyValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                TechnologyCheck.gameObject.SetActive(true);
                                DetailedStatName.text = "탐구력";

                            }
                            else if (nowButton.name == "InsightValueButton")
                            {
                                tempLevel = SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);
                                InsightCheck.gameObject.SetActive(true);

                                DetailedStatName.text = "광기";
                            }
                        }
                        break;
                }

                StatGradeImg.sprite = SetSkillLevelImg(tempLevel);
                DetailedStatValue.text = "Lv. " + tempLevel;
            }
        }
    }

    public void CheckDepartmentStudentStat(StudentType tempType)
    {
        switch (tempType)                // 이 학생의 학과 구별 (학과별로 상세 스킬이 달라짐)
        {
            case StudentType.GameDesigner:
                {

                }
                break;
            case StudentType.Art:
                {

                }
                break;
            case StudentType.Programming:
                {

                }
                break;
        }
    }


    public void LoadAllDepartmentIndexImg()
    {
        var r = Resources.LoadAll<Sprite>("DepartmentIndexSprite");

        foreach (var temp in r)
        {
            DepartmentIndexImgList.Add(temp);
        }
    }

    public void LoadStudentSkillLevelImg()
    {
        var r = Resources.LoadAll<Sprite>("StudentSkillGradeImg");

        foreach (var temp in r)
        {
            StudentSkillLevelImgList.Add(temp);
        }
    }

    // 좌 우 화살표 버튼을 눌렀을 때 실행 될 함수. 데이터 세팅은 없이 작동만 되도록.
    // 데이터는 인포 창을 띄울 때 다 집어넣기
    public void IfClickArrowButton()
    {
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;

        switch (nowButton.name)
        {
            case "LeftArrowButton":
                {
                    LeftArrowButton.gameObject.SetActive(false);
                    StudentStatPage2.gameObject.SetActive(false);

                    RightArrowButton.gameObject.SetActive(true);
                    StudentInfoPage1.gameObject.SetActive(true);
                }
                break;
            case "RigthArrowButton":
                {
                    LeftArrowButton.gameObject.SetActive(true);
                    StudentStatPage2.gameObject.SetActive(true);

                    RightArrowButton.gameObject.SetActive(false);
                    StudentInfoPage1.gameObject.SetActive(false);
                }
                break;
        }
    }

    // 
    public int SetSkillLevel(int statAmount)
    {
        float a = statAmount;
        float b = 50;       // 레벨을 나누기 위한 수

        double quotient = System.Math.Truncate((double)a / b);
        quotient += 1;

        return (int)quotient;
    }

    Sprite tempSprie;

    public Sprite SetSkillLevelImg(int StatAmount)
    {
        if (StatAmount == 1 || StatAmount == 2)
        {
            tempSprie = UISpriteLists.Instance.GetStudentSkillLevelImgList[(int)EStudentSkillLevelImgIndex.Drank_icon];
            return tempSprie;
        }
        else if (StatAmount == 3 || StatAmount == 4)
        {
            tempSprie = UISpriteLists.Instance.GetStudentSkillLevelImgList[(int)EStudentSkillLevelImgIndex.Crank_icon];
            return tempSprie;
        }
        else if (StatAmount == 5 || StatAmount == 6)
        {
            tempSprie = UISpriteLists.Instance.GetStudentSkillLevelImgList[(int)EStudentSkillLevelImgIndex.Brank_icon];
            return tempSprie;
        }
        else if (StatAmount == 7 || StatAmount == 8)
        {
            tempSprie = UISpriteLists.Instance.GetStudentSkillLevelImgList[(int)EStudentSkillLevelImgIndex.Arank_icon];
            return tempSprie;
        }
        else if (8 < StatAmount)
        {
            tempSprie = UISpriteLists.Instance.GetStudentSkillLevelImgList[(int)EStudentSkillLevelImgIndex.Srank_icon];
            return tempSprie;
        }

        return null;
    }

    public Sprite Roop(string name)
    {
        foreach (var temp in UISpriteLists.Instance.GetStudentSkillLevelImgList)
        {
            if (temp.name == name)
            {
                return temp;
            }
        }
        return null;
    }

    // 캐릭터 정보창의 펜타곤 그래프를 그려줄 함수
    public void MakeStudentStatPentagon(int sense, int concentration, int wit, int technique, int insight)
    {
        PentagonStats stats = new PentagonStats(sense, concentration, wit, technique, insight);

        StudentStatsRader.SetStats(stats);
    }

    // 각 학생의 호감도 데이터를 가져와서 정렬해주기 위한 함수
    public void ReadyFriendshipData(Student nowStudent)
    {
        FriendshipDic.Clear();
        FriendshipList.Clear();

        int studentcount = ObjectManager.Instance.m_StudentList.Count;
        int myIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(nowStudent));

        for (int i = 0; i < ObjectManager.Instance.m_Friendship[myIndex].Count; i++)
        {
            if (i != myIndex)
            {
                FriendshipOption tempFriendshipData = new FriendshipOption();

                if (i < studentcount)
                {
                    tempFriendshipData.Department = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentType;
                    tempFriendshipData.Name = ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_StudentName;
                }
                else
                {
                    tempFriendshipData.Department = ObjectManager.Instance.m_InstructorList[i - studentcount].m_InstructorData.m_ProfessorType;
                    tempFriendshipData.Name = ObjectManager.Instance.m_InstructorList[i - studentcount].m_InstructorData.m_ProfessorNameValue;
                }
                tempFriendshipData.FriendshipAmount = ObjectManager.Instance.m_Friendship[myIndex][i];

                FriendshipList.Add(tempFriendshipData);
            }
        }
        FriendshipList = FriendshipList.OrderByDescending(i => i.FriendshipAmount).ToList();
    }

    public void SetFriendshipInfo()
    {
        int SpritePrefabCount = 0;

        for (int i = 0; i < FriendshipList.Count; i++)
        {
            switch (FriendshipList[i].Department)      // for문을 돌면서 딕셔너리의 인덱스로 접근해 값을 가져오는 것...
            {
                case StudentType.GameDesigner:
                    {
                        FriendshipInfoList[SpritePrefabCount].SetDepartmentImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];
                        FriendshipInfoList[SpritePrefabCount].SetStudentNameImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    }
                    break;
                case StudentType.Art:
                    {
                        FriendshipInfoList[SpritePrefabCount].SetDepartmentImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];
                        FriendshipInfoList[SpritePrefabCount].SetStudentNameImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];

                    }
                    break;
                case StudentType.Programming:
                    {
                        FriendshipInfoList[SpritePrefabCount].SetDepartmentImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];
                        FriendshipInfoList[SpritePrefabCount].SetStudentNameImg.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    }
                    break;
            }
            FriendshipInfoList[SpritePrefabCount].SetStudentName.text = FriendshipList[i].Name;
            FriendshipInfoList[SpritePrefabCount].SetFriendshipAmount.text = FriendshipList[i].FriendshipAmount.ToString();

            if (FriendshipList[i].FriendshipAmount < 150)
            {
                FriendshipInfoList[SpritePrefabCount].SetFriendshipInfoText.text = "아는사이";
                FriendshipInfoList[SpritePrefabCount].SetFriendshipImg.sprite = UISpriteLists.Instance.GetEmotionEmojiSpriteList[(int)EEmotionEmoji.wonder];
            }
            else if (FriendshipList[i].FriendshipAmount < 300)
            {
                FriendshipInfoList[SpritePrefabCount].SetFriendshipInfoText.text = "친한사이";
                FriendshipInfoList[SpritePrefabCount].SetFriendshipImg.sprite = UISpriteLists.Instance.GetEmotionEmojiSpriteList[(int)EEmotionEmoji.smile];
            }
            else
            {
                FriendshipInfoList[SpritePrefabCount].SetFriendshipInfoText.text = "베스트프렌드";
                FriendshipInfoList[SpritePrefabCount].SetFriendshipImg.sprite = UISpriteLists.Instance.GetEmotionEmojiSpriteList[(int)EEmotionEmoji.big_smile];
            }

            SpritePrefabCount += 1;
        }
    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

    // 학생 정보창의 1page의 스킬부분에 데이터를 넣기 위한 준비
    public void SetStudentBonusSkill(Student nowStudent)
    {
        if (StudentBonusSkillParent.childCount != 0)
        {
            for (int i = StudentBonusSkillParent.childCount; i > 0; i--)
            {
                StudentBonusSkillObjectPool.ReturnStudentBonusSkillPrefab(StudentBonusSkillParent.GetChild(0).gameObject);
            }
        }

        if (nowStudent.m_StudentStat.m_Skills.Count != 0)
        {
            for (int i = 0; i < nowStudent.m_StudentStat.m_Skills.Count; i++)
            {
                GameObject prefabParent;
                BonusSkillPrefab TempBonusSkillPrefab;
                prefabParent = StudentBonusSkillObjectPool.GetStudentBonusSkillPrefab(StudentBonusSkillParent);       //선택한 이벤트 프리팹 생성했으니
                TempBonusSkillPrefab = prefabParent.GetComponent<BonusSkillPrefab>();

                prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                TempBonusSkillPrefab.GetStudentBonusSkillNameText.text = nowStudent.m_StudentStat.m_Skills[i];                                                           
            }
        }
    }
}