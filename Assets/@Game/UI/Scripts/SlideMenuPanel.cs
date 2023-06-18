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
    [SerializeField] private Button FoldMenuButton;
    [SerializeField] private Button QuestButton;
    [SerializeField] private Button AdvertiseButton;
    [SerializeField] private Button ConsultingButton;
    [Space(5f)]
    //[SerializeField] private Button ExternalActivityButton;             // �ܺ�Ȱ�� ��ư
    [SerializeField] private Button RecommendUI;                        // ������õ ��ư
    //[SerializeField] private Button PopOffExternalActivityButton;
    //[SerializeField] private GameObject SecondContentUI;                // �ܺ�Ȱ�� ��ư Ŭ���� �˾��Ǵ� �޴�â
    [SerializeField] private Button GameShowButton;
    [SerializeField] private Button GameJamButton;
    //[SerializeField] private Button DiningTogetherButton;
    [Space(5f)]
    [SerializeField] private Button ShopButton;                         // ������ư
    [Space(5f)]
    [SerializeField] private Button FacilityButton;                     // �ü���ư
    [Space(5f)]
    [SerializeField] private Button PopUpManagementButton;
    [SerializeField] private Button PopOffManagementButton;

    [SerializeField] private GameObject SecondManagementUI;
    [SerializeField] private Button StudentListButton;
    [SerializeField] private Button InstructorListButton;
    [Space(5f)]
    [SerializeField] private Transform PointPos;

    [Space(10f)]
    [SerializeField] private PopUpUI PopUpAllInstructorInfoPanel;       // �л� / ���� ��ư Ŭ�� �� ���� �˾�â

    [SerializeField] private PopUpUI PopUpAllStudentInfoPanel;
    [SerializeField] private PopUpUI PopUpInstructorInfoPanel;

    [SerializeField] private AllInstructorPanel allInstructorPanel;
    [SerializeField] private AllStudentInfoPanel allStutdentPanel;

    Instructor InstructorInfoPrefab;
    [SerializeField] private InstructorInfoPanel InstructorPanel = new InstructorInfoPanel();

    //[SerializeField] private GameObject SecondContentUI;              // ������ ��ư�� ������ �� ������ ����â
    [SerializeField] private GameObject WarningPanel;                   // �������� ���� �޿��� ���ٴ� �˾�â�� �������Ѵ�.

    [SerializeField] private PopUpUI PopUpInJaeWarningPanel;            // ���� ��õ �Ⱓ�� �ƴϸ� ���â�� ����ش�.
    [SerializeField] private PopOffUI PopOffInJaeWarningPanel;          // ���â�� �߸� 3�ʵڿ� â�� ������.
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
    }

    // Update is called once per frame
    void Update()
    {
        #region ����� Ŭ�� �Է� ������ �����±� ���ֱ� -> ������ �߰��ϱ�
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

    //�ٸ� �гε�� ��ȣ�ۿ� ����? �ش� �г��� ��ȣ�ۿ��� ���� �ʱ�ȭ �Լ�
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
        //HideManagementMenu();
        ShowManagementMenu();

        List<ProfessorStat> tempData = new List<ProfessorStat>();       // �⺻������ ���õ� ��ȹ����, Ÿ�Ժ��� ����
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
                prefabParent = InstructorObjectPool.GetInstructorPrefab(allInstructorPanel.GetParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
                tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                prefabParent.name = tempData[i].m_ProfessorNameValue;
                // ���⼭ ������ �����յ鿡 �����͸� �־���� �Ѵ�
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
                    case "����":
                        {
                            tempTeacherPrefabs.GetInstructorTypeImage.sprite = tempTeacherPrefabs.GetFullTimeTeacherImage;

                            tempTeacherPrefabs.GetInstructorImageButton.name = tempData[i].m_ProfessorNameValue;
                        }
                        break;
                    case "�ܷ�":
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
            // ��Ʈ -> ����
        }
    }

    // �����л���ư Ŭ���� ������ �ֱ�
    public void IfIClickAllStudentButton()
    {
        //HideManagementMenu();
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
        Debug.Log("�̰� �ɱ��...");
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        List<ProfessorStat> tempData = new List<ProfessorStat>();   // = ObjectManager.Instance.nowProfessor.AllProfessor;       // �⺻������ ���õ� ��ȹ����, Ÿ�Ժ��� ����

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

                            if (professorSet == "����")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "�ܷ�")
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

                            if (professorSet == "����")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "�ܷ�")
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

                            if (professorSet == "����")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                            }
                            else if (professorSet == "�ܷ�")
                            {
                                InstructorPanel.GetInstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                            }

                        }
                        break;
                }

                string result = string.Format("{0:#,0}", tempData[i].m_ProfessorPay);      // ���ڿ� �޸� ��� ���� �Լ�
                InstructorPanel.GetPayText.text = result + "G";

                InstructorPanel.GetLevelText.text = tempData[i].m_ProfessorPower.ToString();

                InstructorPanel.GetLevelText.text = "Lv." + tempData[i].m_ProfessorPower.ToString();          // ������ �Ŀ���
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

    public void ClickBackOrNextButton()
    {
        GameObject nowClick = EventSystem.current.currentSelectedGameObject;
        // AllInstructorPanel.SetActive(true);
        // ���� ���� ��ư�� üũ
        switch (nowClick.name)
        {
            case "BackButton":       // ���� ��ư Ŭ��
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
            case "NextButton":       // ���� ��ư Ŭ��
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

        string result = string.Format("{0:#,0}", nowTeacher.m_ProfessorPay);      // ���ڿ� �޸� ��� ���� �Լ�
        InstructorPanel.GetPayText.text = result + "G";

        InstructorPanel.GetLevelText.text = nowTeacher.m_ProfessorPower.ToString();

        InstructorPanel.GetLevelText.text = "Lv." + nowTeacher.m_ProfessorPower.ToString();          // ������ �Ŀ���
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
