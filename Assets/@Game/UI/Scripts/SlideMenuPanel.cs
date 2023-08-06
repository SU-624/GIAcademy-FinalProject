using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using StatData.Runtime;
using System.Linq;
using System;

/// <summary>
/// 2023. 03. 21 Mang
/// 
/// 인게임 화면에서 움직이는 메뉴 바를 컨트롤 하기 위한 스크립트
/// </summary>
public class SlideMenuPanel : MonoBehaviour
{
    public delegate void GameShowListButtonClicked();

    public static event GameShowListButtonClicked OnGameShowListButtonClicked;

    [Header("학생 정보창의 학과 인덱스 변수들")]
    [SerializeField]
    private Button FoldMenuButton;

    [SerializeField] private Button QuestButton;

    [Space(5f)]
    //[SerializeField] private Button ExternalActivityButton;             // 외부활동 버튼
    [SerializeField]
    private Button RecommendUI; // 인재추천 버튼

    //[SerializeField] private Button PopOffExternalActivityButton;
    //[SerializeField] private GameObject SecondContentUI;                // 외부활동 버튼 클릭시 팝업되는 메뉴창
    [SerializeField] private Button GameShowButton;

    [SerializeField] private Button GameJamButton;

    [Space(5f)] [SerializeField] private Button PopUpManagementButton;
    //[SerializeField] private Button PopOffManagementButton;

    [SerializeField] private GameObject SecondManagementUI;
    [SerializeField] private Button StudentListButton;
    [SerializeField] private Button InstructorListButton;
    [Space(5f)] [SerializeField] private Transform PointPos;

    [Space(10f)] [SerializeField] private PopUpUI PopUpAllInstructorInfoPanel; // 학생 / 강사 버튼 클릭 시 나올 팝업창

    [SerializeField] private PopUpUI PopUpAllStudentInfoPanel;
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;

    [SerializeField] private AllInstructorPanel allInstructorPanel;
    [SerializeField] private AllStudentInfoPanel allStutdentPanel;

    Instructor InstructorInfoPrefab;
    [SerializeField] private InstructorInfoPanel InstructorPanel = new InstructorInfoPanel();

    //[SerializeField] private GameObject SecondContentUI;              // 게임잼 버튼을 눌렀을 때 보여줄 선택창
    [SerializeField] private GameObject WarningPanel; // 게임잼이 없는 달에는 없다는 팝업창을 띄워줘야한다.

    [SerializeField] private PopUpUI PopUpInJaeWarningPanel; // 인재 추천 기간이 아니면 경고창을 띄워준다.
    [SerializeField] private PopOffUI PopOffInJaeWarningPanel; // 경고창이 뜨면 3초뒤에 창이 닫힌다.
    [SerializeField] private PopUpUI PopUpInJaeRecommendPaenl;

    [SerializeField] private CharacterClickEvent CharacterClickEventScript;

    [Space(5f)]
    [SerializeField] private PopUpUI PopUpRoomInfoPanel;
    [SerializeField] private PopOffUI PopOffRoomInfoPanel;
    [SerializeField] private Button PopOffRoomInfoButton;
    [Space(5f)]
    [SerializeField] private PopUpUI PopUpGameJamPanel;
    [Space(5f)]
    [SerializeField] private GameObject LeafletGamePanel;
    [SerializeField] private GameObject SelectMiniGamesPanel;
    [SerializeField] private GameObject DustCleanGamePanel;
    [SerializeField] private Button MiniGameButton;
    [SerializeField] private Button LeafletGameOpenButton;
    [SerializeField] private Button SecondeGaME;
    [SerializeField] private Button DustCleanGameButton;
    [SerializeField] private TextMeshProUGUI DustCleanTimerText;
    [SerializeField] private float DustCleanGameCoolTime;

    private float DustCleanGameTimer;

    // [SerializeField] private Button PopUpCharacterInfoPanel;

    public Button GetPopUpManagementButton
    {
        get { return PopUpManagementButton; }
    }

    // public Button GetPopOffManagementButton
    // {
    //     get { return PopOffManagementButton; }
    // }

    public GameObject GetSecondManagementUI
    {
        get { return SecondManagementUI; }
    }

    //public GameObject GetSecondContentUI
    //{
    //    get { return SecondContentUI; }
    //}

    public Button GetInstructorListButton
    {
        get { return InstructorListButton; }
    }

    public Button GetStudentListButton
    {
        get { return StudentListButton; }
    }

    public GameObject SetWarningPanel
    {
        get { return WarningPanel; }
        set { WarningPanel = value; }
    }

    private void OnEnable()
    {
        GameJam.OnActivateButtonHandler += OnActivateGameShowButton;
    }

    private void OnDisable()
    {
        GameJam.OnActivateButtonHandler -= OnActivateGameShowButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        FoldMenuButton.onClick.AddListener(IfClickSlideMenuFoldButton);

        InstructorPanel.GetNextButton.onClick.AddListener(ClickBackOrNextButton);
        InstructorPanel.GetBackButton.onClick.AddListener(ClickBackOrNextButton);

        // PopOffManagementButton.gameObject.SetActive(false);

        //SecondContentUI.SetActive(false);
        SecondManagementUI.SetActive(false);
        SelectMiniGamesPanel.SetActive(false);
        GameShowButton.interactable = false;

        if (Json.Instance.UseLoadingData && AllInOneData.Instance.GameJamHistory.Count != 0)
        {
            GameShowButton.interactable = true;
        }

        PopUpManagementButton.onClick.AddListener(ShowManagementMenu);

        GameJamButton.onClick.AddListener(PopUpGameJamPanel.TurnOnUI);

        GameShowButton.onClick.AddListener(OnGameShowButtonClicked);
        //ExternalActivityButton.onClick.AddListener(ClickExternalActivityButton);
        RecommendUI.onClick.AddListener(ShowInJaeWarningPaenl);

        // 개별 강사정보에서 인덱스 클릭 시 클릭한 인덱스에 맞는 과의 전체강사창으로 전환
        InstructorPanel.GetGameDesignerIndexButton.onClick.AddListener(InstructorPanel.IfClickInsftructorPanelQuitButton);
        InstructorPanel.GetGameDesignerIndexButton.onClick.AddListener(allInstructorPanel.IfClickInstructorIndex);
        InstructorPanel.GetGameDesignerIndexButton.onClick.AddListener(ShowAllInstructorInfoPanel);

        InstructorPanel.GetArtIndexButton.onClick.AddListener(InstructorPanel.IfClickInsftructorPanelQuitButton);
        InstructorPanel.GetArtIndexButton.onClick.AddListener(allInstructorPanel.IfClickInstructorIndex);
        InstructorPanel.GetArtIndexButton.onClick.AddListener(ShowAllInstructorInfoPanel);

        InstructorPanel.GetProgrammingIndexButton.onClick.AddListener(InstructorPanel.IfClickInsftructorPanelQuitButton);
        InstructorPanel.GetProgrammingIndexButton.onClick.AddListener(allInstructorPanel.IfClickInstructorIndex);
        InstructorPanel.GetProgrammingIndexButton.onClick.AddListener(ShowAllInstructorInfoPanel);

        MiniGameButton.onClick.AddListener(MiniGameButtonActing);
        LeafletGameOpenButton.onClick.AddListener(LeafletMiniGamePanelOpen);
        DustCleanGameButton.onClick.AddListener(DustCleanMiniGamePanelOpen);

        DustCleanGameTimer = DustCleanGameCoolTime;
    }

    private void LeafletMiniGamePanelOpen()
    {
        ClickEventManager.Instance.Sound.StopInGameBgm();
        ClickEventManager.Instance.Sound.PlayLeafletBGM();
        LeafletGamePanel.SetActive(true);
    }

    private void DustCleanMiniGamePanelOpen()
    {
        //ClickEventManager.Instance.Sound.PlayLeafletBGM();
        DustCleanGamePanel.SetActive(true);
        DustCleanGamePanel.GetComponent<CleanMiniGame>().Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        #region 모바일 클릭 입력 받으면 네임태그 꺼주기 -> 여러개 추가하기

        if (ObjectManager.Instance.m_StudentList.Count != 18)
        {
            // StudentListButton.enabled = false;
            StudentListButton.interactable = false;
        }
        else if (ObjectManager.Instance.m_StudentList.Count == 18)
        {
            //StudentListButton.enabled = true;
            StudentListButton.interactable = true;
        }

// #if UNITY_EDITOR
//         if (Input.GetMouseButtonDown(0))
//         {
//             GameObject nowClick = EventSystem.current.currentSelectedGameObject;
//             if (SecondManagementUI.activeSelf == true)
//             {
//                 if (nowClick == null)
//                 {
//                     ShowManagementMenu();
//                 }
//                 else if (nowClick.name == "StudentListButton" && StudentListButton.interactable == true)
//                 {
//                     InitializeStudentButton();
//                 }
//                 else if (nowClick.name == "InstructorListButton")
//                 {
//                     InitializeInstructorButton();
//                 }
//                 else if (nowClick.name == "GameJam_Button" || nowClick.name == "GameShow_Button" ||
//                          nowClick.name == "DiningTogether_Button" || nowClick.name == "RecommendUI")
//                 {
//                 }
//                 else
//                 {
//                     ShowManagementMenu();
//                 }
//             }
//         }
// #elif UNITY_ANDROID
//         if (Input.touchCount > 0)
//         {
//             GameObject nowClick = EventSystem.current.currentSelectedGameObject;
//             if (SecondManagementUI.activeSelf == true)
//             {
//                 if (nowClick == null)
//                 {
//                     Debug.Log("모바일에서 슬라이드 메뉴 클릭 - 세컨유아이 켜져야함");
// 
//                     ShowManagementMenu();
//                 }
//                 else if (nowClick.name == "StudentListButton")
//                 {
//                     InitializeStudentButton();
//                 }
//                 else if (nowClick.name == "InstructorListButton")
//                 {
//                     InitializeInstructorButton();
//                 }
//                 else if (nowClick.name == "GameJam_Button" || nowClick.name == "GameShow_Button" || nowClick.name == "DiningTogether_Button" || nowClick.name == "RecommendUI")
//                 {
// 
//                 }
//                 else
//                 {
//                     Debug.Log("모바일에서 슬라이드 메뉴 버튼 다시 클릭 - 꺼질거임");
// 
//                     ShowManagementMenu();
//                 }
//             }
//         }
// 
// #endif

        #endregion

        if (DustCleanGamePanel.GetComponent<CleanMiniGame>().m_IsGameEnd)
        {
            DustCleanGameButton.interactable = false;
            DustCleanGamePanel.GetComponent<CleanMiniGame>().m_IsGameEnd = false;
            DustCleanTimerText.gameObject.SetActive(true);
            StartCoroutine(DustCleanTimerStart());
        }
    }

    IEnumerator DustCleanTimerStart()
    {
        while (DustCleanGameTimer > 0)
        {
            DustCleanTimerText.text = ((int)DustCleanGameTimer).ToString();
            DustCleanGameTimer -= 1f;

            yield return new WaitForSeconds(1f);
        }

        DustCleanGameTimer = DustCleanGameCoolTime;
        DustCleanGameButton.interactable = true;
        DustCleanTimerText.gameObject.SetActive(false);
    }

    //다른 패널들과 상호작용 없이? 해당 패널의 상호작용을 위한 초기화 함수
    public void Initialize()
    {
        //PopOffExternalActivityButton.onClick.AddListener(HideExternalActivityButton);

        //PopUpManagementButton.onClick.AddListener(ShowManagementMenu);
        // PopOffManagementButton.onClick.AddListener(HideManagementMenu);
        //StudentListButton.onClick.AddListener(ShowAllStudentInfoPanel);
        //InstructorListButton.onClick.AddListener(InitializeInstructorButton);
        // StudentListButton.onClick.AddListener(IfIClickAllStudentButton);
        //PopOffRoomInfoButton.onClick.AddListener(HideRoomInfoPanel);

        allStutdentPanel.Initialize();
    }

    public void IfClickSlideMenuFoldButton()
    {
        SecondManagementUI.gameObject.SetActive(false);
        SelectMiniGamesPanel.gameObject.SetActive(false);
    }

    private void OnGameShowButtonClicked()
    {
        if (OnGameShowListButtonClicked != null)
        {
            OnGameShowListButtonClicked();
        }
    }

    public void OnActivateGameShowButton(bool activate)
    {
        StartCoroutine(DelayOnActivateGameShowButton(activate));
        //GameShowButton.interactable = activate;
    }

    private IEnumerator DelayOnActivateGameShowButton(bool activate)
    {
        yield return new WaitForSeconds(GameTime.Instance.ThirdFourthWeekTime);

        GameShowButton.interactable = activate;
    }

    /// 폴드 버튼을 닫을 때 이 UI가 켜져있다면 꺼주기
    //public void HideSecondContentUI()
    //{
    //    if (SecondContentUI.activeSelf == true)
    //    {
    //        SecondContentUI.SetActive(false);
    //    }
    //}
    private void ShowInJaeWarningPaenl()
    {
        if (GameTime.Instance.FlowTime.NowMonth != 2)
        {
            PopUpInJaeWarningPanel.TurnOnUI();
            StartCoroutine(HideInJaeWarningPanel());
            Debug.Log("코루틴 호출");
        }
        else
        {
            PopUpInJaeRecommendPaenl.TurnOnUI();
        }
    }

    IEnumerator HideInJaeWarningPanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        PopOffInJaeWarningPanel.TurnOffUI();
    }

    //public void ClickExternalActivityButton()
    //{
    //    if (SecondContentUI.gameObject.activeSelf == true)
    //    {
    //        SecondContentUI.SetActive(false);
    //    }
    //    else
    //    {
    //        SecondContentUI.SetActive(true);
    //    }
    //}

    public void HideExternalActivityButton()
    {
        // PopUpExternalActivityButton.gameObject.SetActive(true);
        // PopOffExternalActivityButton.gameObject.SetActive(false);
        // 
        // SecondContentUI.gameObject.SetActive(false);
    }

    public void ShowManagementMenu()
    {
        ClickEventManager.Instance.Sound.PlayIconTouch();
        if (SecondManagementUI.activeSelf == false)
        {
            Debug.Log("버튼 클릭, showManagementMenu로 이동. 켜져라");
            SecondManagementUI.SetActive(true);
        }
        else
        {
            Debug.Log("세컨드 유아이 켜져있을 때 버튼 클릭, showManagementMenu로 이동. 꺼져라");
            SecondManagementUI.SetActive(false);
        }
    }

    public void HideManagementMenu()
    {
        ClickEventManager.Instance.Sound.PlayIconTouch();
        PopUpManagementButton.gameObject.SetActive(true);
        //PopOffManagementButton.gameObject.SetActive(false);

        SecondManagementUI.SetActive(false);
    }

    public void HideRoomInfoPanel()
    {
        PopOffRoomInfoPanel.TurnOffUI();
    }

    // 관리 - 학생 버튼 
    public void ShowAllStudentInfoPanel()
    {
        PopUpAllStudentInfoPanel.TurnOnUI();
    }

    // 관리 - 강사 버튼 
    public void ShowAllInstructorInfoPanel()
    {
        PopUpAllInstructorInfoPanel.TurnOnUI();
    }


    // 관리 - 강사 버튼 클릭 시 현재강사 오브젝트풀에 강사 생성. 데이터 넣기까지
    // 전체강사창의 기본값은 기획자로 뜨게 하는걸로
    // 현재 세팅에서 켤 때는 그냥 켜짐 
    // 인덱스 클릭 시에도 되도록 온클릭을 달자
    public void IfIClickInstructorManagementButton()
    {
        ShowManagementMenu();

        List<ProfessorStat> tempData = new List<ProfessorStat>(); // 기본적으로 셋팅된 기획강사, 타입별로 띄우기
        int nowInstructorCount;
        tempData.Clear();

        for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
        {
            tempData.Add(Professor.Instance.GameManagerProfessor[i]);
        }

        nowInstructorCount = tempData.Count;

        if (allInstructorPanel.GetTeacherLevelToggle.isOn == true)
        {
            tempData = tempData.OrderByDescending(x => x.m_ProfessorPower).ToList();
        }
        else
        {
            tempData = tempData.OrderByDescending(x => x.m_ProfessorSet).ToList();
        }

        int count = allInstructorPanel.GetParentInstructorPrefab.childCount;

        for (int j = count - 1; j >= 0; j--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(allInstructorPanel.GetParentInstructorPrefab.GetChild(0).gameObject);
        }

        for (int i = 0; i < nowInstructorCount; i++)
        {
            GameObject prefabParent;
            InstructorPrefab tempTeacherPrefabs;

            if (tempData[i].m_ProfessorType == StudentType.GameDesigner)
            {
                prefabParent =
                    InstructorObjectPool.GetInstructorPrefab(allInstructorPanel.GetParentInstructorPrefab); //선택한 이벤트 프리팹 생성했으니
                tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                prefabParent.name = tempData[i].m_ProfessorName;

                tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;
                tempTeacherPrefabs.GetTeacherLevelText.text = "Lv. " + tempData[i].m_ProfessorPower.ToString();

                tempTeacherPrefabs.name = tempData[i].m_ProfessorName;
                tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorName;

                switch (tempData[i].m_ProfessorSet)
                {
                    case "전임":
                    {
                        tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetFullTimeTeacherImage;

                        tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorName;
                    }
                    break;
                    case "외래":
                    {
                        tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetoutsideTeacherImage;

                        tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorName;
                    }
                    break;
                }

                allInstructorPanel.GetGameDesignerIndexButton.image.sprite =
                    UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                allInstructorPanel.GetArtIndexButton.image.sprite =
                    UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                allInstructorPanel.GetProgrammingIndexButton.image.sprite =
                    UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                tempTeacherPrefabs.GetTeacherNameImage.sprite =
                    UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];

                tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(IFClickEachInstructorButton);
            }
        }
    }

    // 현재학생버튼 클릭시 데이터 넣기
    public void IfIClickAllStudentButton()
    {
        ShowManagementMenu();

        // 기본 세팅된 기획반 학생 목록만 띄우기
        allStutdentPanel.IfClickAllStudentButton();
    }

    // 여기서 강사 버튼을 누를 때마다 오브젝트풀에서 현재 강사의 갯수만큼 parent에 넣어준다
    // 리턴하는 부분은 x 버튼을 누를 때 해주자 -> allinstructorpanel 에서 해야할듯
    public void InitializeInstructorButton()
    {
        IfIClickInstructorManagementButton();
        ShowAllInstructorInfoPanel();
        //HideManagementMenu();
        ShowManagementMenu();
    }

    public void InitializeStudentButton()
    {
        IfIClickAllStudentButton();
        ShowAllStudentInfoPanel();
        //  HideManagementMenu();
        ShowManagementMenu();
    }

    // 이거시 각 강사버튼을 클릭했을 때 개인 강사 정보창으로 넘어가게 하기 위한 버튼 작업
    public void IFClickEachInstructorButton()
    {
        Debug.Log("강사 개인 이미지 클릭...");

        if (PlayerInfo.Instance.TeacherProfileClickCount <= 10)
        {
            PlayerInfo.Instance.TeacherProfileClickCount++;
        }

        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<ProfessorStat> tempData = new List<ProfessorStat>(); // = Professor.Instance.AllProfessor;       // 기본적으로 셋팅된 기획강사, 타입별로 띄우기

        for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
        {
            tempData.Add(Professor.Instance.GameManagerProfessor[i]);
        }

        for (int i = 0; i < Professor.Instance.ArtProfessor.Count; i++)
        {
            tempData.Add(Professor.Instance.ArtProfessor[i]);
        }

        for (int i = 0; i < Professor.Instance.ProgrammingProfessor.Count; i++)
        {
            tempData.Add(Professor.Instance.ProgrammingProfessor[i]);
        }


        int nowInstructorCount = tempData.Count;

        for (int i = 0; i < nowInstructorCount; i++)
        {
            StudentType tempType = tempData[i].m_ProfessorType;
            string professorSet = tempData[i].m_ProfessorSet;

            if (nowButton.name == tempData[i].m_ProfessorName)
            {
                //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                //{
                //    if (tempData[i].m_ProfessorNameValue == temp.ToString())
                //    {
                //        InstructorPanel.GetInstructorImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                //        // InstructorPanel.GetInstructorImage.sprite = tempData[i].

                //    }
                //}
                InstructorPanel.GetInstructorImage.sprite = tempData[i].m_TeacherProfileImg;

                InstructorPanel.GetTeacherNameText.text = tempData[i].m_ProfessorName.ToString();
                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                switch (tempType)
                {
                    case StudentType.GameDesigner:
                    {
                        InstructorPanel.GetGameDesignerIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.gamedesign_tab_selected];
                        InstructorPanel.GetArtIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList
                                [(int)EDepartmentImgIndex.art_tab_notselect];
                        InstructorPanel.GetProgrammingIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.program_tab_notselect];
                        InstructorPanel.GetDepartmentImage.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.gamedesign_icon_info];

                        if (professorSet == "전임")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "외래")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof2_icon_info];
                        }
                    }
                    break;
                    case StudentType.Art:
                    {
                        InstructorPanel.GetGameDesignerIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        InstructorPanel.GetArtIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                        InstructorPanel.GetProgrammingIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.program_tab_notselect];
                        InstructorPanel.GetDepartmentImage.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];

                        if (professorSet == "전임")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "외래")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof2_icon_info];
                        }
                    }
                    break;
                    case StudentType.Programming:
                    {
                        InstructorPanel.GetGameDesignerIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        InstructorPanel.GetArtIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList
                                [(int)EDepartmentImgIndex.art_tab_notselect];
                        InstructorPanel.GetProgrammingIndexButton.image.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.program_tab_selected];
                        InstructorPanel.GetDepartmentImage.sprite =
                            UISpriteLists.Instance.GetDepartmentIndexImgList[
                                (int)EDepartmentImgIndex.programming_icon2_icon];

                        if (professorSet == "전임")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "외래")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof2_icon_info];
                        }
                    }
                    break;
                }

                string result = string.Format("{0:#,0}", tempData[i].m_ProfessorPay); // 숫자에 콤마 찍기 위한 함수
                InstructorPanel.GetPayText.text = result + "G";

                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                InstructorPanel.GetLevelText.text = "Lv." + tempData[i].m_ProfessorPower.ToString(); // 레벨이 파워임
                InstructorPanel.GetPassionText.text = tempData[i].m_ProfessorPassion.ToString();
                InstructorPanel.GetHealthText.text = tempData[i].m_ProfessorHealth.ToString();

                InstructorPanel.GetTeacherSkillName.text = tempData[i].m_ProfessorSkills[0];
                InstructorPanel.GetTeacherSkillDetailInfo.text = tempData[i].m_ProfessorSkills[1];

                allInstructorPanel.IfClickAllInstructorQuitButton();
                PopUpInstructorInfoPanel.TurnOnUI();

                InstructorPanel.GetnowProfessorStat = tempData[i];
                // 강사정보를 여기서 넘겨주자
            }
        }
    }

    // 강사정보창에서 왼쪽 오른쪽 버튼 누를 시 다음 강사의 정보로 변경 해주는 함수
    public void ClickBackOrNextButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        // AllInstructorPanel.SetActive(true);
        // 내가 누른 버튼을 체크
        switch (nowClick.name)
        {
            case "BackButton": // 이전 버튼 클릭
            {
                string nowTeacherName = InstructorPanel.GetTeacherNameText.text;
                int BackIndex;
                PlayerInfo.Instance.TeacherProfileClickCount++;

                if (InstructorPanel.GetDepartmentImage.sprite.name == "gamedesign_icon_info")
                {
                    for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.GameManagerProfessor[i].m_ProfessorName)
                        {
                            BackIndex = i - 1;
                            if (0 <= BackIndex)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.GameManagerProfessor[BackIndex]);
                            }
                        }
                    }
                }
                else if (InstructorPanel.GetDepartmentImage.sprite.name == "art_icon_info")
                {
                    for (int i = 0; i < Professor.Instance.ArtProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.ArtProfessor[i].m_ProfessorName)
                        {
                            BackIndex = i - 1;
                            if (0 <= BackIndex)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.ArtProfessor[BackIndex]);
                            }
                        }
                    }
                }
                else if (InstructorPanel.GetDepartmentImage.sprite.name == "programming_icon2_icon")
                {
                    for (int i = 0; i < Professor.Instance.ProgrammingProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.ProgrammingProfessor[i].m_ProfessorName)
                        {
                            BackIndex = i - 1;
                            if (0 <= BackIndex)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.ProgrammingProfessor[BackIndex]);
                            }
                        }
                    }
                }
            }
            break;
            case "NextButton": // 다음 버튼 클릭
            {
                string nowTeacherName = InstructorPanel.GetTeacherNameText.text;
                int NextIndex;

                PlayerInfo.Instance.TeacherProfileClickCount++;

                if (InstructorPanel.GetDepartmentImage.sprite.name == "gamedesign_icon_info")
                {
                    for (int i = 0; i < Professor.Instance.GameManagerProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.GameManagerProfessor[i].m_ProfessorName)
                        {
                            NextIndex = i + 1;
                            if (NextIndex < Professor.Instance.GameManagerProfessor.Count)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.GameManagerProfessor[NextIndex]);
                            }
                        }
                    }
                }
                else if (InstructorPanel.GetDepartmentImage.sprite.name == "art_icon_info")
                {
                    for (int i = 0; i < Professor.Instance.ArtProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.ArtProfessor[i].m_ProfessorName)
                        {
                            NextIndex = i + 1;
                            if (NextIndex < Professor.Instance.ArtProfessor.Count)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.ArtProfessor[NextIndex]);
                            }
                        }
                    }
                }
                else if (InstructorPanel.GetDepartmentImage.sprite.name == "programming_icon2_icon")
                {
                    for (int i = 0; i < Professor.Instance.ProgrammingProfessor.Count; i++)
                    {
                        if (nowTeacherName == Professor.Instance.ProgrammingProfessor[i].m_ProfessorName)
                        {
                            NextIndex = i + 1;
                            if (NextIndex < Professor.Instance.ProgrammingProfessor.Count)
                            {
                                NextBackButtonClickReadyEachInfo(Professor.Instance.ProgrammingProfessor[NextIndex]);
                            }
                        }
                    }
                }
            }
            break;
        }
    }

    public void NextBackButtonClickReadyEachInfo(ProfessorStat nowTeacher)
    {
        InstructorPanel.GetInstructorImage.sprite = nowTeacher.m_TeacherProfileImg;

        InstructorPanel.GetTeacherNameText.text = nowTeacher.m_ProfessorName.ToString();
        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        string result = string.Format("{0:#,0}", nowTeacher.m_ProfessorPay); // 숫자에 콤마 찍기 위한 함수
        InstructorPanel.GetPayText.text = result + "G";

        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        InstructorPanel.GetLevelText.text = "Lv." + nowTeacher.m_ProfessorPower.ToString(); // 레벨이 파워임
        InstructorPanel.GetPassionText.text = nowTeacher.m_ProfessorPassion.ToString();
        InstructorPanel.GetHealthText.text = nowTeacher.m_ProfessorHealth.ToString();

        InstructorPanel.GetTeacherSkillName.text = nowTeacher.m_ProfessorSkills[0];
        InstructorPanel.GetTeacherSkillDetailInfo.text = nowTeacher.m_ProfessorSkills[1];

        allInstructorPanel.IfClickAllInstructorQuitButton();
        PopUpInstructorInfoPanel.TurnOnUI();

        InstructorPanel.GetnowProfessorStat = nowTeacher;

        switch (nowTeacher.m_ProfessorType)
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

    // 슬라이드 메뉴패널에 미니게임 버튼 클릭 - 미니게임 목록 나오도록 코드 수정
    public void MiniGameButtonActing()
    {
        if (SelectMiniGamesPanel.gameObject.activeSelf == false)
        {
            SelectMiniGamesPanel.gameObject.SetActive(true);
        }
        else
        {
            SelectMiniGamesPanel.gameObject.SetActive(false);
        }
    }
}