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
/// �ΰ��� ȭ�鿡�� �����̴� �޴� �ٸ� ��Ʈ�� �ϱ� ���� ��ũ��Ʈ
/// </summary>
public class SlideMenuPanel : MonoBehaviour
{
    public delegate void GameShowListButtonClicked();

    public static event GameShowListButtonClicked OnGameShowListButtonClicked;

    [Header("�л� ����â�� �а� �ε��� ������")]
    [SerializeField]
    private Button FoldMenuButton;

    [SerializeField] private Button QuestButton;

    [Space(5f)]
    //[SerializeField] private Button ExternalActivityButton;             // �ܺ�Ȱ�� ��ư
    [SerializeField]
    private Button RecommendUI; // ������õ ��ư

    //[SerializeField] private Button PopOffExternalActivityButton;
    //[SerializeField] private GameObject SecondContentUI;                // �ܺ�Ȱ�� ��ư Ŭ���� �˾��Ǵ� �޴�â
    [SerializeField] private Button GameShowButton;

    [SerializeField] private Button GameJamButton;

    [Space(5f)] [SerializeField] private Button PopUpManagementButton;
    //[SerializeField] private Button PopOffManagementButton;

    [SerializeField] private GameObject SecondManagementUI;
    [SerializeField] private Button StudentListButton;
    [SerializeField] private Button InstructorListButton;
    [Space(5f)] [SerializeField] private Transform PointPos;

    [Space(10f)] [SerializeField] private PopUpUI PopUpAllInstructorInfoPanel; // �л� / ���� ��ư Ŭ�� �� ���� �˾�â

    [SerializeField] private PopUpUI PopUpAllStudentInfoPanel;
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;

    [SerializeField] private AllInstructorPanel allInstructorPanel;
    [SerializeField] private AllStudentInfoPanel allStutdentPanel;

    Instructor InstructorInfoPrefab;
    [SerializeField] private InstructorInfoPanel InstructorPanel = new InstructorInfoPanel();

    //[SerializeField] private GameObject SecondContentUI;              // ������ ��ư�� ������ �� ������ ����â
    [SerializeField] private GameObject WarningPanel; // �������� ���� �޿��� ���ٴ� �˾�â�� �������Ѵ�.

    [SerializeField] private PopUpUI PopUpInJaeWarningPanel; // ���� ��õ �Ⱓ�� �ƴϸ� ���â�� ����ش�.
    [SerializeField] private PopOffUI PopOffInJaeWarningPanel; // ���â�� �߸� 3�ʵڿ� â�� ������.
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

        // ���� ������������ �ε��� Ŭ�� �� Ŭ���� �ε����� �´� ���� ��ü����â���� ��ȯ
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
        #region ����� Ŭ�� �Է� ������ �����±� ���ֱ� -> ������ �߰��ϱ�

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
//                     Debug.Log("����Ͽ��� �����̵� �޴� Ŭ�� - ���������� ��������");
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
//                     Debug.Log("����Ͽ��� �����̵� �޴� ��ư �ٽ� Ŭ�� - ��������");
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

    //�ٸ� �гε�� ��ȣ�ۿ� ����? �ش� �г��� ��ȣ�ۿ��� ���� �ʱ�ȭ �Լ�
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

    /// ���� ��ư�� ���� �� �� UI�� �����ִٸ� ���ֱ�
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
            Debug.Log("�ڷ�ƾ ȣ��");
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
            Debug.Log("��ư Ŭ��, showManagementMenu�� �̵�. ������");
            SecondManagementUI.SetActive(true);
        }
        else
        {
            Debug.Log("������ ������ �������� �� ��ư Ŭ��, showManagementMenu�� �̵�. ������");
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

    // ���� - �л� ��ư 
    public void ShowAllStudentInfoPanel()
    {
        PopUpAllStudentInfoPanel.TurnOnUI();
    }

    // ���� - ���� ��ư 
    public void ShowAllInstructorInfoPanel()
    {
        PopUpAllInstructorInfoPanel.TurnOnUI();
    }


    // ���� - ���� ��ư Ŭ�� �� ���簭�� ������ƮǮ�� ���� ����. ������ �ֱ����
    // ��ü����â�� �⺻���� ��ȹ�ڷ� �߰� �ϴ°ɷ�
    // ���� ���ÿ��� �� ���� �׳� ���� 
    // �ε��� Ŭ�� �ÿ��� �ǵ��� ��Ŭ���� ����
    public void IfIClickInstructorManagementButton()
    {
        ShowManagementMenu();

        List<ProfessorStat> tempData = new List<ProfessorStat>(); // �⺻������ ���õ� ��ȹ����, Ÿ�Ժ��� ����
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
                    InstructorObjectPool.GetInstructorPrefab(allInstructorPanel.GetParentInstructorPrefab); //������ �̺�Ʈ ������ ����������
                tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                prefabParent.name = tempData[i].m_ProfessorName;

                tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;
                tempTeacherPrefabs.GetTeacherLevelText.text = "Lv. " + tempData[i].m_ProfessorPower.ToString();

                tempTeacherPrefabs.name = tempData[i].m_ProfessorName;
                tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorName;

                switch (tempData[i].m_ProfessorSet)
                {
                    case "����":
                    {
                        tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetFullTimeTeacherImage;

                        tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorName;
                    }
                    break;
                    case "�ܷ�":
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

    // �����л���ư Ŭ���� ������ �ֱ�
    public void IfIClickAllStudentButton()
    {
        ShowManagementMenu();

        // �⺻ ���õ� ��ȹ�� �л� ��ϸ� ����
        allStutdentPanel.IfClickAllStudentButton();
    }

    // ���⼭ ���� ��ư�� ���� ������ ������ƮǮ���� ���� ������ ������ŭ parent�� �־��ش�
    // �����ϴ� �κ��� x ��ư�� ���� �� ������ -> allinstructorpanel ���� �ؾ��ҵ�
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

    // �̰Ž� �� �����ư�� Ŭ������ �� ���� ���� ����â���� �Ѿ�� �ϱ� ���� ��ư �۾�
    public void IFClickEachInstructorButton()
    {
        Debug.Log("���� ���� �̹��� Ŭ��...");

        if (PlayerInfo.Instance.TeacherProfileClickCount <= 10)
        {
            PlayerInfo.Instance.TeacherProfileClickCount++;
        }

        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<ProfessorStat> tempData = new List<ProfessorStat>(); // = Professor.Instance.AllProfessor;       // �⺻������ ���õ� ��ȹ����, Ÿ�Ժ��� ����

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

                        if (professorSet == "����")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "�ܷ�")
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

                        if (professorSet == "����")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "�ܷ�")
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

                        if (professorSet == "����")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof1_icon_info];
                        }
                        else if (professorSet == "�ܷ�")
                        {
                            InstructorPanel.GetInstructorTagImage.sprite =
                                UISpriteLists.Instance.GetDepartmentIndexImgList[
                                    (int)EDepartmentImgIndex.prof2_icon_info];
                        }
                    }
                    break;
                }

                string result = string.Format("{0:#,0}", tempData[i].m_ProfessorPay); // ���ڿ� �޸� ��� ���� �Լ�
                InstructorPanel.GetPayText.text = result + "G";

                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                InstructorPanel.GetLevelText.text = "Lv." + tempData[i].m_ProfessorPower.ToString(); // ������ �Ŀ���
                InstructorPanel.GetPassionText.text = tempData[i].m_ProfessorPassion.ToString();
                InstructorPanel.GetHealthText.text = tempData[i].m_ProfessorHealth.ToString();

                InstructorPanel.GetTeacherSkillName.text = tempData[i].m_ProfessorSkills[0];
                InstructorPanel.GetTeacherSkillDetailInfo.text = tempData[i].m_ProfessorSkills[1];

                allInstructorPanel.IfClickAllInstructorQuitButton();
                PopUpInstructorInfoPanel.TurnOnUI();

                InstructorPanel.GetnowProfessorStat = tempData[i];
                // ���������� ���⼭ �Ѱ�����
            }
        }
    }

    // ��������â���� ���� ������ ��ư ���� �� ���� ������ ������ ���� ���ִ� �Լ�
    public void ClickBackOrNextButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        // AllInstructorPanel.SetActive(true);
        // ���� ���� ��ư�� üũ
        switch (nowClick.name)
        {
            case "BackButton": // ���� ��ư Ŭ��
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
            case "NextButton": // ���� ��ư Ŭ��
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

        string result = string.Format("{0:#,0}", nowTeacher.m_ProfessorPay); // ���ڿ� �޸� ��� ���� �Լ�
        InstructorPanel.GetPayText.text = result + "G";

        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        InstructorPanel.GetLevelText.text = "Lv." + nowTeacher.m_ProfessorPower.ToString(); // ������ �Ŀ���
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

    // �����̵� �޴��гο� �̴ϰ��� ��ư Ŭ�� - �̴ϰ��� ��� �������� �ڵ� ����
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