using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class AllStudentInfoPanel : MonoBehaviour
{
    [Space(5f)]
    [Header("학생 정보창의 학과 인덱스 변수들")]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;

    [Space(5f)]
    [Header("강사 정보 이외에 팝업창의 버튼 변수들")]
    [SerializeField] private Image StudentImagePrefab_First;

    [SerializeField] private Image StudentProfileImg_First;            
    [SerializeField] private TextMeshProUGUI FirstStudentName;
    [SerializeField] private Button FirstStudentButton;
    [Space(5f)]
    [SerializeField] private Image StudentImagePrefab_Second;

    [SerializeField] private Image StudentProfileImg_Second;
    [SerializeField] private TextMeshProUGUI SecondStudentName;
    [SerializeField] private Button SecondStudentButton;
    [Space(5f)]
    [SerializeField] private Image StudentImagePrefab_Third;

    [SerializeField] private Image StudentProfileImg_Third;
    [SerializeField] private TextMeshProUGUI ThirdStudentName;
    [SerializeField] private Button ThirdStudentButton;
    [Space(5f)]
    [SerializeField] private Image StudentImagePrefab_Fourth;

    [SerializeField] private Image StudentProfileImg_Fourth;
    [SerializeField] private TextMeshProUGUI FourthStudentName;
    [SerializeField] private Button FourthStudentButton;
    [Space(5f)]
    [SerializeField] private Image StudentImagePrefab_Fifth;

    [SerializeField] private Image StudentProfileImg_Fifth;
    [SerializeField] private TextMeshProUGUI FifthStudentName;
    [SerializeField] private Button FifthStudentButton;
    [Space(5f)]
    [SerializeField] private Image StudentImagePrefab_Sixth;

    [SerializeField] private Image StudentProfileImg_Sixth;
    [SerializeField] private TextMeshProUGUI SixthStudentName;
    [SerializeField] private Button SixthStudentButton;
    [Space(5f)]
    [SerializeField] private Button AllStudentQuitButton;     // 종료 버튼 클릭 시 현재강사오브젝트풀 비우기

    [Space(5f)]
    [SerializeField] private PopUpUI PopUpAllStudentInfoPanel;
    [SerializeField] private PopOffUI PopOffAllStudentInfoPanel;
    [SerializeField] private CharacterInfoPanel StudentInfoPage;
    [Space(5f)]
    [SerializeField] private PopUpUI PopUpStudentInfoPanel;

    Transform StudentListParent;
    List<Image> studentImgList = new List<Image>();
    List<TextMeshProUGUI> studentNameTextList = new List<TextMeshProUGUI>();
    //List<Sprite> DepartmentIndexImgList = new List<Sprite>();
    //List<Sprite> StudentSkillLevelImgList = new List<Sprite>();

    List<Button> studentButtonList = new List<Button>();

    public Button GetAllStudentQuitButton
    {
        get { return AllStudentQuitButton; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("");



    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        StudentInfoPage.Initialize();
        // LoadAllDepartmentIndexImg();

        studentImgList.Add(StudentProfileImg_First);
        studentImgList.Add(StudentProfileImg_Second);
        studentImgList.Add(StudentProfileImg_Third);
        studentImgList.Add(StudentProfileImg_Fourth);
        studentImgList.Add(StudentProfileImg_Fifth);
        studentImgList.Add(StudentProfileImg_Sixth);

        studentNameTextList.Add(FirstStudentName);
        studentNameTextList.Add(SecondStudentName);
        studentNameTextList.Add(ThirdStudentName);
        studentNameTextList.Add(FourthStudentName);
        studentNameTextList.Add(FifthStudentName);
        studentNameTextList.Add(SixthStudentName);

        studentButtonList.Add(FirstStudentButton);
        studentButtonList.Add(SecondStudentButton);
        studentButtonList.Add(ThirdStudentButton);
        studentButtonList.Add(FourthStudentButton);
        studentButtonList.Add(FifthStudentButton);
        studentButtonList.Add(SixthStudentButton);

        // StudentImagePrefab_First.gameObject.SetActive(false);
        // StudentImagePrefab_Second.gameObject.SetActive(false);
        // StudentImagePrefab_Third.gameObject.SetActive(false);
        // StudentImagePrefab_Fourth.gameObject.SetActive(false);
        // StudentImagePrefab_Fifth.gameObject.SetActive(false);
        // StudentImagePrefab_Sixth.gameObject.SetActive(false);

        // 학생 개인정보창의 인덱스를 눌렀을 때 전체 학생창의 정보를 띄우기
        StudentInfoPage.GetGameDesignerIndexButton.onClick.AddListener(StudentInfoPage.IfClickStudentPanelQuitButton);
        StudentInfoPage.GetGameDesignerIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);
        StudentInfoPage.GetGameDesignerIndexButton.onClick.AddListener(PopUpAllStudentInfoPanel.TurnOnUI);

        StudentInfoPage.GetArtIndexButton.onClick.AddListener(StudentInfoPage.IfClickStudentPanelQuitButton);
        StudentInfoPage.GetArtIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);
        StudentInfoPage.GetArtIndexButton.onClick.AddListener(PopUpAllStudentInfoPanel.TurnOnUI);

        StudentInfoPage.GetProgrammingIndexButton.onClick.AddListener(StudentInfoPage.IfClickStudentPanelQuitButton);
        StudentInfoPage.GetProgrammingIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);
        StudentInfoPage.GetProgrammingIndexButton.onClick.AddListener(PopUpAllStudentInfoPanel.TurnOnUI);

        // 전체 학생창의 인덱스 버튼을 눌렀을 때 각 학과별로 학생들 나오도록
        GameDesignerIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);
        ArtIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);
        ProgrammingIndexButton.onClick.AddListener(IfIClickStudentDepartmentIndex);

        for (int i = 0; i < studentButtonList.Count; i++)
        {
            studentButtonList[i].onClick.AddListener(IfIClickEachStudentButton);
        }
    }

    // 학생이 몇명이냐에 따라서 앨범에 넣어줄 학생의 데이터들
    public void IfClickAllStudentButton()
    {
        Debug.Log("전체학생창 패널 세팅");

        List<Student> tempData = ObjectManager.Instance.m_StudentList;
        int studentCount = ObjectManager.Instance.m_StudentList.Count;
        int StudentListCount = 0;

        // StudentImagePrefab_First.gameObject.SetActive(false);
        // StudentImagePrefab_Second.gameObject.SetActive(false);
        // StudentImagePrefab_Third.gameObject.SetActive(false);
        // StudentImagePrefab_Fourth.gameObject.SetActive(false);
        // StudentImagePrefab_Fifth.gameObject.SetActive(false);
        // StudentImagePrefab_Sixth.gameObject.SetActive(false);

        // 학생의 카운트를 받아온다
        // 카운트만큼 리스트를 돈다
        // 기본은 기획반 학생들이 보이는 것이므로 기획반 학생들의 데이터를 보여주도록 하자

        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
        StudentProfileImg_First.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        StudentProfileImg_Second.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        StudentProfileImg_Third.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        StudentProfileImg_Fourth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        StudentProfileImg_Fifth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
        StudentProfileImg_Sixth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];

        for (int i = 0; i < studentCount; i++)
        {
            if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
            {
                // studentImgList[i] = tempData[i].m_StudentStat.image;       // 이미지는 나중에 생긴다면
                studentButtonList[StudentListCount].gameObject.name = tempData[i].m_StudentStat.m_StudentName;
                studentNameTextList[StudentListCount].text = tempData[i].m_StudentStat.m_StudentName;

                studentImgList[StudentListCount].sprite = tempData[i].StudentProfileImg;        // 학생 이미지 넣기
                
                // studentImgList[StudentListCount].gameObject.SetActive(true);
                StudentListCount += 1;
            }
        }

        Debug.Log("전체학생수 ? : " + studentCount);

    }

    public void IfClickAllStudentPanelQuitButton()
    {
        Debug.Log("전체강사창 종료");

        PopOffAllStudentInfoPanel.TurnOffUI();
    }

    public void IfIClickStudentDepartmentIndex()
    {
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;     // 기본적으로 셋팅된 기획강사, 타입별로 띄우기
        int nowStudentCount = ObjectManager.Instance.m_StudentList.Count;
        int StudentListCount = 0;

        switch (nowButton.name)
        {
            case "GameDesignerIndexButton":
                {
                    Debug.Log("기획 인덱스 클릭");
                    FindCorrectDepartmentSprite(nowButton);

                    for (int i = 0; i < nowStudentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                        {
                            studentButtonList[StudentListCount].name = tempData[i].m_StudentStat.m_StudentName;
                            studentNameTextList[StudentListCount].text = tempData[i].m_StudentStat.m_StudentName;
                            studentImgList[StudentListCount].sprite = tempData[i].StudentProfileImg;        // 학생 이미지 넣기
                            StudentListCount += 1;
                        }
                    }
                }
                break;
            case "ArtIndexButton":
                {
                    Debug.Log("아트 인덱스 클릭");
                    FindCorrectDepartmentSprite(nowButton);

                    for (int i = 0; i < nowStudentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.Art)
                        {
                            studentButtonList[StudentListCount].name = tempData[i].m_StudentStat.m_StudentName;
                            studentNameTextList[StudentListCount].text = tempData[i].m_StudentStat.m_StudentName;
                            studentImgList[StudentListCount].sprite = tempData[i].StudentProfileImg;        // 학생 이미지 넣기
                            StudentListCount += 1;
                        }
                    }
                }
                break;
            case "ProgrammingIndexButton":
                {
                    Debug.Log("플밍 인덱스 클릭");
                    FindCorrectDepartmentSprite(nowButton);

                    for (int i = 0; i < nowStudentCount; i++)
                    {
                        if (tempData[i].m_StudentStat.m_StudentType == StudentType.Programming)
                        {
                            studentButtonList[StudentListCount].name = tempData[i].m_StudentStat.m_StudentName;
                            studentNameTextList[StudentListCount].text = tempData[i].m_StudentStat.m_StudentName;
                            studentImgList[StudentListCount].sprite = tempData[i].StudentProfileImg;        // 학생 이미지 넣기
                            StudentListCount += 1;
                        }
                    }
                }
                break;
        }
    }

    // 전체 학생 창에서 각 학생 버튼을 클릭시 호출되는 함수
    public void IfIClickEachStudentButton()
    {
        Debug.Log("학생 각 이미지 누름");

        // 내가 무슨 버튼을 눌렀는지 판별
        // 누른 버튼의 부모의 이름을 가져와서 현재 학생 목록을 비교
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<Student> tempData = ObjectManager.Instance.m_StudentList;       // 기본적으로 셋팅된 기획강사, 타입별로 띄우기
        int nowStudentCount = ObjectManager.Instance.m_StudentList.Count;

        int sense;
        int concentration;
        int wit;
        int technique;
        int insight;

        int tempLevel = 0;
        int StudentCount = 0;

        float x = StudentInfoPage.GetParentFriendshipPrefab.anchoredPosition.x;
        StudentInfoPage.GetParentFriendshipPrefab.anchoredPosition = new Vector3(x, 0, 0);

        StudentInfoPage.GetStudentInfoPage1.gameObject.SetActive(true);
        StudentInfoPage.GetLeftArrowButton.gameObject.SetActive(false);
        StudentInfoPage.GetStudentStatPage2.gameObject.SetActive(false);
        StudentInfoPage.GetRightArrowButton.gameObject.SetActive(true);

        // 현재 학생 페이지에서 내가 누른 버튼의 이름이랑 
        for (int i = 0; i < tempData.Count; i++)
        {
            if (nowButton.name == tempData[i].m_StudentStat.m_StudentName)
            {
                StudentCount = 0;

                foreach (EStudentImgIndex temp in Enum.GetValues(typeof(EStudentImgIndex)))
                {
                    if (tempData[i].m_StudentStat.m_StudentID == temp.ToString())
                    {
                        studentImgList[StudentCount].sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];

                        StudentCount += 1;
                    }
                }

                // 학생 이미지 넣기
                StudentInfoPage.GetCharacterImg1.sprite = tempData[i].StudentProfileImg;
                StudentInfoPage.GetCharacterImg2.sprite = tempData[i].StudentProfileImg;

                sense = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense];
                concentration = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration];
                wit = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit];
                technique = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique];
                insight = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight];

                StudentInfoPage.MakeStudentStatPentagon(sense, concentration, wit, technique, insight);

                // StudentInfoPage.ShowStudentBasicInfo();
                Debug.Log("왜안들어올까");
                // 학생 데이터 다 넣기
                StudentInfoPage.GetCharacterName1.text = tempData[i].m_StudentStat.m_StudentName;
                StudentInfoPage.GetCharacterName2.text = tempData[i].m_StudentStat.m_StudentName;

                StudentInfoPage.GetPassionValue1.text = tempData[i].m_StudentStat.m_Passion.ToString();
                StudentInfoPage.GetHealthValue1.text = tempData[i].m_StudentStat.m_Health.ToString();

                StudentInfoPage.GetPassionValue2.text = tempData[i].m_StudentStat.m_Passion.ToString();
                StudentInfoPage.GetHealthValue2.text = tempData[i].m_StudentStat.m_Health.ToString();

                StudentInfoPage.GetSenseCheck.gameObject.SetActive(true);
                StudentInfoPage.GetWitCheck.gameObject.SetActive(false);
                StudentInfoPage.GetConcentrationCheck.gameObject.SetActive(false);
                StudentInfoPage.GetTechnologyCheck.gameObject.SetActive(false);
                StudentInfoPage.GetInsightCheck.gameObject.SetActive(false);

                // 학생의 개인 성격 띄우기
                string peronalityInfoScript = ObjectManager.Instance.GetPersonalityScript(tempData[i].m_StudentStat.m_Personality);
                StudentInfoPage.GetStudentPersonalityName.text = tempData[i].m_StudentStat.m_Personality;
                StudentInfoPage.GetStudentPersonalityInfo.text = peronalityInfoScript;

                switch (tempData[i].m_StudentStat.m_StudentType)
                {
                    case StudentType.GameDesigner:
                        {
                            StudentInfoPage.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                            StudentInfoPage.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                            StudentInfoPage.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                            StudentInfoPage.GetDepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];
                            StudentInfoPage.GetDepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];

                            tempLevel = StudentInfoPage.SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);

                            StudentInfoPage.GetDetailedStatName.text = "창의력";
                            StudentInfoPage.GetDetailedStatValue.text = "Lv. " + tempLevel;
                            StudentInfoPage.GetStatGradeImg.sprite = StudentInfoPage.SetSkillLevelImg(tempLevel);
                        }
                        break;
                    case StudentType.Art:
                        {
                            StudentInfoPage.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                            StudentInfoPage.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                            StudentInfoPage.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                            StudentInfoPage.GetDepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];
                            StudentInfoPage.GetDepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];

                            tempLevel = StudentInfoPage.SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);

                            StudentInfoPage.GetDetailedStatName.text = "표현력";
                            StudentInfoPage.GetDetailedStatValue.text = "Lv. " + tempLevel;
                            StudentInfoPage.GetStatGradeImg.sprite = StudentInfoPage.SetSkillLevelImg(tempLevel);
                        }
                        break;
                    case StudentType.Programming:
                        {
                            StudentInfoPage.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                            StudentInfoPage.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                            StudentInfoPage.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];
                            StudentInfoPage.GetDepartmentImg1.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];
                            StudentInfoPage.GetDepartmentImg2.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];

                            tempLevel = StudentInfoPage.SetSkillLevel(tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense]);

                            StudentInfoPage.GetDetailedStatName.text = "활용력";
                            StudentInfoPage.GetDetailedStatValue.text = "Lv. " + tempLevel;
                            StudentInfoPage.GetStatGradeImg.sprite = StudentInfoPage.SetSkillLevelImg(tempLevel);
                        }
                        break;
                }

                StudentInfoPage.SetStudentBonusSkill(tempData[i]);

                StudentInfoPage.GetSenseValue.text = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense].ToString();
                StudentInfoPage.GetWitValue.text = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit].ToString();
                StudentInfoPage.GetConcentrationValue.text = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration].ToString();
                StudentInfoPage.GetTechnologyValue.text = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique].ToString();
                StudentInfoPage.GetInsightValue.text = tempData[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight].ToString();

                StudentInfoPage.GetRPGStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.RPG].ToString();
                StudentInfoPage.GetActionStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Action].ToString();
                StudentInfoPage.GetSimulationStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Simulation].ToString();
                StudentInfoPage.GetShootingStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Shooting].ToString();

                StudentInfoPage.GetRhythmStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Rhythm].ToString();
                StudentInfoPage.GetAdventureStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Adventure].ToString();
                StudentInfoPage.GetPuzzleStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Puzzle].ToString();
                StudentInfoPage.GetSportsStatValue.text = tempData[i].m_StudentStat.m_GenreAmountList[(int)GenreStat.Sports].ToString();

                StudentInfoPage.ReadyFriendshipData(tempData[i]);
                StudentInfoPage.SetFriendshipInfo();

                PopOffAllStudentInfoPanel.TurnOffUI();
                PopUpStudentInfoPanel.TurnOnUI();
            }
        }
    }

    public Sprite FindCorrectDepartmentSprite(GameObject nowButton)
    {
        switch (nowButton.name)
        {
            case "GameDesignerIndexButton":
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];

                    StudentProfileImg_First.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    StudentProfileImg_Second.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    StudentProfileImg_Third.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    StudentProfileImg_Fourth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    StudentProfileImg_Fifth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                    StudentProfileImg_Sixth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                }
                break;
            case "ArtIndexButton":
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];

                    StudentProfileImg_First.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                    StudentProfileImg_Second.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                    StudentProfileImg_Third.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                    StudentProfileImg_Fourth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                    StudentProfileImg_Fifth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                    StudentProfileImg_Sixth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                }
                break;
            case "ProgrammingIndexButton":
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];

                    StudentProfileImg_First.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    StudentProfileImg_Second.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    StudentProfileImg_Third.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    StudentProfileImg_Fourth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    StudentProfileImg_Fifth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                    StudentProfileImg_Sixth.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                }
                break;
        }

        return null;
    }
}
