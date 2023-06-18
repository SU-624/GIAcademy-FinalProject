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
    [SerializeField] private Button FoldMenuButton;
    [SerializeField] private Button QuestButton;
    [SerializeField] private Button AdvertiseButton;
    [SerializeField] private Button ConsultingButton;
    [Space(5f)]
    //[SerializeField] private Button ExternalActivityButton;             // 외부활동 버튼
    [SerializeField] private Button RecommendUI;                        // 인재추천 버튼
    //[SerializeField] private Button PopOffExternalActivityButton;
    //[SerializeField] private GameObject SecondContentUI;                // 외부활동 버튼 클릭시 팝업되는 메뉴창
    [SerializeField] private Button GameShowButton;
    [SerializeField] private Button GameJamButton;
    //[SerializeField] private Button DiningTogetherButton;
    [Space(5f)]
    [SerializeField] private Button ShopButton;                         // 상점버튼
    [Space(5f)]
    [SerializeField] private Button FacilityButton;                     // 시설버튼
    [Space(5f)]
    [SerializeField] private Button PopUpManagementButton;
    [SerializeField] private Button PopOffManagementButton;

    [SerializeField] private GameObject SecondManagementUI;
    [SerializeField] private Button StudentListButton;
    [SerializeField] private Button InstructorListButton;
    [Space(5f)]
    [SerializeField] private Transform PointPos;

    [Space(10f)]
    [SerializeField] private PopUpUI PopUpAllInstructorInfoPanel;       // 학생 / 강사 버튼 클릭 시 나올 팝업창

    [SerializeField] private PopUpUI PopUpAllStudentInfoPanel;
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;

    [SerializeField] private AllInstructorPanel allInstructorPanel;
    [SerializeField] private AllStudentInfoPanel allStutdentPanel;

    Instructor InstructorInfoPrefab;
    [SerializeField] private InstructorInfoPanel InstructorPanel = new InstructorInfoPanel();

    //[SerializeField] private GameObject SecondContentUI;              // 게임잼 버튼을 눌렀을 때 보여줄 선택창
    [SerializeField] private GameObject WarningPanel;                   // 게임잼이 없는 달에는 없다는 팝업창을 띄워줘야한다.

    [SerializeField] private PopUpUI PopUpInJaeWarningPanel;            // 인재 추천 기간이 아니면 경고창을 띄워준다.
    [SerializeField] private PopOffUI PopOffInJaeWarningPanel;          // 경고창이 뜨면 3초뒤에 창이 닫힌다.
    [SerializeField] private PopUpUI PopUpInJaeRecommendPaenl;

    [SerializeField] private CharacterClickEvent CharacterClickEventScript;

    [Space(5f)]
    [SerializeField] private PopUpUI PopUpRoomInfoPanel;
    [SerializeField] private PopOffUI PopOffRoomInfoPanel;
    [SerializeField] private Button PopOffRoomInfoButton;

    public Button GetPopUpManagementButton
    {
        get { return PopUpManagementButton; }
    }

    public Button GetPopOffManagementButton
    {
        get { return PopOffManagementButton; }
    }
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
        InstructorPanel.GetNextButton.onClick.AddListener(ClickBackOrNextButton);
        InstructorPanel.GetBackButton.onClick.AddListener(ClickBackOrNextButton);

        PopOffManagementButton.gameObject.SetActive(false);

        //SecondContentUI.SetActive(false);
        SecondManagementUI.SetActive(false);
        GameShowButton.interactable = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        #region 모바일 클릭 입력 받으면 네임태그 꺼주기 -> 여러개 추가하기
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            GameObject nowClick = EventSystem.current.currentSelectedGameObject;
            if (SecondManagementUI.activeSelf == true /*|| SecondContentUI.activeSelf == true*/)
            {
                if (nowClick == null)
                {
                    //HideSecondContentUI();
                    // HideManagementMenu();
                    ShowManagementMenu();
                }
                else if (nowClick.name == "StudentListButton")
                {
                    InitializeStudentButton();
                }
                else if (nowClick.name == "InstructorListButton")
                {
                    InitializeInstructorButton();
                }
                //else if (nowClick.name == "RoomInfo_Button")
                //{
                //    if (!PopUpRoomInfoPanel.m_UI.activeSelf)
                //    {
                //        PopUpRoomInfoPanel.TurnOnUI();
                //    }
                //}
                else if (nowClick.name == "GameJam_Button" || nowClick.name == "GameShow_Button" || nowClick.name == "DiningTogether_Button" || nowClick.name == "RecommendUI")
                {

                }
                else
                {
                    //HideSecondContentUI();
                    //HideManagementMenu();
                    ShowManagementMenu();
                }
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            GameObject nowClick = EventSystem.current.currentSelectedGameObject;
            if (SecondManagementUI.activeSelf == true /*|| SecondContentUI.activeSelf == true*/)
            {
                if (nowClick == null)
                {
                    //HideSecondContentUI();
                    HideManagementMenu();
                }
                else if (nowClick.name == "StudentListButton")
                {
                    InitializeStudentButton();
                }
                else if (nowClick.name == "InstructorListButton")
                {
                    InitializeInstructorButton();
                }
                else if (nowClick.name == "GameJam_Button" || nowClick.name == "GameShow_Button" || nowClick.name == "DiningTogether_Button" || nowClick.name == "RecommendUI")
                {

                }
                else
                {
                    //HideSecondContentUI();
                    HideManagementMenu();
                }
            }
        }
        
#endif
        #endregion

    }

    //다른 패널들과 상호작용 없이? 해당 패널의 상호작용을 위한 초기화 함수
    public void Initialize()
    {
        //PopOffExternalActivityButton.onClick.AddListener(HideExternalActivityButton);

        PopUpManagementButton.onClick.AddListener(ShowManagementMenu);
        PopOffManagementButton.onClick.AddListener(HideManagementMenu);
        //StudentListButton.onClick.AddListener(ShowAllStudentInfoPanel);
        //InstructorListButton.onClick.AddListener(InitializeInstructorButton);
        // StudentListButton.onClick.AddListener(IfIClickAllStudentButton);
        PopOffRoomInfoButton.onClick.AddListener(HideRoomInfoPanel);

        allStutdentPanel.Initialize();
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
        if (SecondManagementUI.activeSelf == false)
        {
            PopUpManagementButton.gameObject.SetActive(false);
            PopOffManagementButton.gameObject.SetActive(true);

            SecondManagementUI.SetActive(true);
        }
        else
        {
            PopUpManagementButton.gameObject.SetActive(true);
            PopOffManagementButton.gameObject.SetActive(false);

            SecondManagementUI.SetActive(false);
        }
    }

    public void HideManagementMenu()
    {
        PopUpManagementButton.gameObject.SetActive(true);
        PopOffManagementButton.gameObject.SetActive(false);

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
        //HideManagementMenu();
        ShowManagementMenu();

        List<ProfessorStat> tempData = new List<ProfessorStat>();       // 기본적으로 셋팅된 기획강사, 타입별로 띄우기
        int nowInstructorCount;
        tempData.Clear();

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.GameManagerProfessor[i]);
        }
        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.ArtProfessor[i]);
        }
        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i]);
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

        for (int i = 0; i < nowInstructorCount; i++)
        {
            GameObject prefabParent;
            InstructorPrefab tempTeacherPrefabs;

            if (tempData[i].m_ProfessorType == StudentType.GameDesigner)
            {
                prefabParent = InstructorObjectPool.GetInstructorPrefab(allInstructorPanel.GetParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                prefabParent.name = tempData[i].m_ProfessorNameValue;
                // 여기서 생성된 프리팹들에 데이터를 넣어줘야 한다
                //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                //{
                //    if (tempData[i].m_ProfessorNameValue == temp.ToString())
                //    {
                //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                //    }
                //}
                tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;

                tempTeacherPrefabs.name = tempData[i].m_ProfessorNameValue;
                tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorNameValue;

                switch (tempData[i].m_ProfessorSet)
                {
                    case "전임":
                        {
                            tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetFullTimeTeacherImage;

                            tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorNameValue;
                        }
                        break;
                    case "외래":
                        {
                            tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetoutsideTeacherImage;

                            tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorNameValue;
                        }
                        break;
                }
                allInstructorPanel.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                allInstructorPanel.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                allInstructorPanel.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];

                tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(IFClickEachInstructorButton);
            }
            // 소트 -> 정렬
        }
    }

    // 현재학생버튼 클릭시 데이터 넣기
    public void IfIClickAllStudentButton()
    {
        //HideManagementMenu();
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
        Debug.Log("이게 될까요...");
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<ProfessorStat> tempData = new List<ProfessorStat>();   // = ObjectManager.Instance.nowProfessor.AllProfessor;       // 기본적으로 셋팅된 기획강사, 타입별로 띄우기

        for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.GameManagerProfessor[i]);
        }
        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.ArtProfessor[i]);
        }
        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
        {
            tempData.Add(ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i]);
        }


        int nowInstructorCount = tempData.Count;

        for (int i = 0; i < nowInstructorCount; i++)
        {
            StudentType tempType = tempData[i].m_ProfessorType;
            string professorSet = tempData[i].m_ProfessorSet;

            if (nowButton.name == tempData[i].m_ProfessorNameValue)
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

                InstructorPanel.GetTeacherNameText.text = tempData[i].m_ProfessorNameValue.ToString();
                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                switch (tempType)
                {
                    case StudentType.GameDesigner:
                        {
                            InstructorPanel.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                            InstructorPanel.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                            InstructorPanel.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                            InstructorPanel.GetDepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];

                            if (professorSet == "전임")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "외래")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                            }

                        }
                        break;
                    case StudentType.Art:
                        {
                            InstructorPanel.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                            InstructorPanel.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                            InstructorPanel.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                            InstructorPanel.GetDepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];

                            if (professorSet == "전임")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "외래")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                            }

                        }
                        break;
                    case StudentType.Programming:
                        {
                            InstructorPanel.GetGameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                            InstructorPanel.GetArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                            InstructorPanel.GetProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];
                            InstructorPanel.GetDepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];

                            if (professorSet == "전임")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "외래")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                            }

                        }
                        break;
                }

                string result = string.Format("{0:#,0}", tempData[i].m_ProfessorPay);      // 숫자에 콤마 찍기 위한 함수
                InstructorPanel.GetPayText.text = result + "G";

                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                InstructorPanel.GetLevelText.text = "Lv." + tempData[i].m_ProfessorPower.ToString();          // 레벨이 파워임
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

    public void ClickBackOrNextButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        // AllInstructorPanel.SetActive(true);
        // 내가 누른 버튼을 체크
        switch (nowClick.name)
        {
            case "BackButton":       // 이전 버튼 클릭
                {
                    string nowTeacherName = InstructorPanel.GetTeacherNameText.text;
                    int BackIndex;

                    if (InstructorPanel.GetDepartmentImage.sprite.name == "gamedesign_icon_info")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorNameValue)
                            {
                                BackIndex = i - 1;
                                if (0 <= BackIndex)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.GameManagerProfessor[BackIndex]);
                                }
                            }
                        }
                    }
                    else if (InstructorPanel.GetDepartmentImage.sprite.name == "art_icon_info")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorNameValue)
                            {
                                BackIndex = i - 1;
                                if (0 <= BackIndex)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.ArtProfessor[BackIndex]);
                                }
                            }
                        }
                    }
                    else if (InstructorPanel.GetDepartmentImage.sprite.name == "programming_icon2_icon")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorNameValue)
                            {
                                BackIndex = i - 1;
                                if (0 <= BackIndex)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.ProgrammingProfessor[BackIndex]);
                                }
                            }
                        }
                    }

                }
                break;
            case "NextButton":       // 다음 버튼 클릭
                {
                    string nowTeacherName = InstructorPanel.GetTeacherNameText.text;
                    int NextIndex;

                    if (InstructorPanel.GetDepartmentImage.sprite.name == "gamedesign_icon_info")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.GameManagerProfessor[i].m_ProfessorNameValue)
                            {
                                NextIndex = i + 1;
                                if (NextIndex < ObjectManager.Instance.nowProfessor.GameManagerProfessor.Count)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.GameManagerProfessor[NextIndex]);
                                }
                            }
                        }
                    }
                    else if (InstructorPanel.GetDepartmentImage.sprite.name == "art_icon_info")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ArtProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.ArtProfessor[i].m_ProfessorNameValue)
                            {
                                NextIndex = i + 1;
                                if (NextIndex < ObjectManager.Instance.nowProfessor.ArtProfessor.Count)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.ArtProfessor[NextIndex]);
                                }
                            }
                        }
                    }
                    else if (InstructorPanel.GetDepartmentImage.sprite.name == "programming_icon2_icon")
                    {
                        for (int i = 0; i < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count; i++)
                        {
                            if (nowTeacherName == ObjectManager.Instance.nowProfessor.ProgrammingProfessor[i].m_ProfessorNameValue)
                            {
                                NextIndex = i + 1;
                                if (NextIndex < ObjectManager.Instance.nowProfessor.ProgrammingProfessor.Count)
                                {
                                    NextBackButtonClickReadyEachInfo(ObjectManager.Instance.nowProfessor.ProgrammingProfessor[NextIndex]);
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

        InstructorPanel.GetTeacherNameText.text = nowTeacher.m_ProfessorNameValue.ToString();
        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        string result = string.Format("{0:#,0}", nowTeacher.m_ProfessorPay);      // 숫자에 콤마 찍기 위한 함수
        InstructorPanel.GetPayText.text = result + "G";

        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        InstructorPanel.GetLevelText.text = "Lv." + nowTeacher.m_ProfessorPower.ToString();          // 레벨이 파워임
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
}
