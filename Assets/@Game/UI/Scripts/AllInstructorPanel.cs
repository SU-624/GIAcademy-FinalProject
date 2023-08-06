using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using StatData.Runtime;
using System;

/// <summary>
/// 2023. 02. 22 Mang
/// 
/// 전체강사정보 패널의 오브젝트들을 모아 둘 스크립트.
/// 여기서 패널의 동작 등을 관리 해 줄 것이다.
/// </summary>
public class AllInstructorPanel : MonoBehaviour
{
    [Header("전체강사목록에 넣을 프리팹을 받을 친구")]
    [SerializeField] private InstructorPrefab instructorPrefabs;

    [Space(5f)]
    [Header("강사 정보창의 학과 인덱스 변수들")]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;

    [Space(5f)]
    [Header("강사 정보 이외에 팝업창의 버튼 변수들")]
    [SerializeField] private Toggle TeacherTypeToggle;
    [SerializeField] private Toggle TeacherLevelToggle;

    [SerializeField] private Button EachInstructorButton;       // 이 친구는 클릭이 되었을 때에 체크를 해야한다

    [SerializeField] private Button AllInstructorQuitButton;     // 종료 버튼 클릭 시 현재강사오브젝트풀 비우기

    [SerializeField] private Transform ParentInstructorPrefab;

    [SerializeField] private PopOffUI PopOffAllInstructorPanel;

    public RectTransform IndexInitPivot;
    GameObject allInstructorPanel;
    [SerializeField] private SlideMenuPanel slideMenuPanel;

    private int NowTeacherTypeIndex = 0;


    // 강사의 타입을 구별하기 위한 int 변수
    public int InsrtuctorTypeIndex = 0;

    public InstructorPrefab GetInstructorPrefab
    {
        get { return instructorPrefabs; }
        set { instructorPrefabs = value; }
    }

    public Button GetAllInstructorQuitButton
    {
        get { return AllInstructorQuitButton; }
    }

    public Transform GetParentInstructorPrefab
    {
        get { return ParentInstructorPrefab; }
        set { ParentInstructorPrefab = value; }
    }

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

    public Toggle GetTeacherTypeToggle
    {
        get { return TeacherTypeToggle; }
        set { TeacherTypeToggle = value; }
    }

    public Toggle GetTeacherLevelToggle
    {
        get { return TeacherLevelToggle; }
        set { TeacherLevelToggle = value; }
    }

    public int GetNowTeacherTypeIndex
    {
        get { return NowTeacherTypeIndex; }
        set { NowTeacherTypeIndex = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitAllInstructorPanel()
    {
        TeacherTypeToggle.onValueChanged.AddListener(Function_Toggle);
        TeacherLevelToggle.onValueChanged.AddListener(Function_Toggle);

        // 전체 강사창에서 각 학과별로 데이터 보이도록
        GameDesignerIndexButton.onClick.AddListener(IfClickInstructorIndex);
        ArtIndexButton.onClick.AddListener(IfClickInstructorIndex);
        ProgrammingIndexButton.onClick.AddListener(IfClickInstructorIndex);

        //slideMenuPanel = GameObject.Find("SlideMenu_Panel").GetComponent<SlideMenuPanel>();
    }

    // 각각의 학과 버튼을 클릭 했을 때 현재 강사중 각각의 학과타입별로 나누기
    void IfClickDepartmentIndex(StudentType departmentType)
    {
        switch (departmentType)
        {
            case StudentType.GameDesigner:
            {
                // 학생 추리기
                // 기본 타입별로 정렬
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

    // 토글 누르면 함수 발동, 현재 토글 상태를 bool값으로 반환해서 사용
    private void Function_Toggle(bool _bool)
    {
        List<ProfessorStat> tempData = new List<ProfessorStat>();

        tempData.Clear();

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

        GameObject thistoggle;

        // tempData.Clear();

        if (TeacherTypeToggle.isOn == true)
        {
            Debug.Log("강사타입토글클릭");
            tempData = tempData.OrderByDescending(x => x.m_ProfessorSet).ToList();

            thistoggle = TeacherTypeToggle.gameObject;
            ArrangeInstructorType(tempData);
        }
        else
        {
            Debug.Log("강사레벨토글클릭");
            tempData = tempData.OrderByDescending(x => x.m_ProfessorPower).ToList();


            thistoggle = TeacherLevelToggle.gameObject;
            ArrangeInstructorType(tempData);
        }

    }

    // 토글 클릭 시 토글대로 
    public void ArrangeInstructorType(List<ProfessorStat> tempData)
    {
        int allProfessorCount = Professor.Instance.GameManagerProfessor.Count +
            Professor.Instance.ArtProfessor.Count + Professor.Instance.ProgrammingProfessor.Count;
        int AllInstructorObj = ParentInstructorPrefab.childCount;        // 

        for (int i = AllInstructorObj - 1; i >= 0; i--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(ParentInstructorPrefab.GetChild(0).gameObject);
        }

        switch (NowTeacherTypeIndex)
        {
            case (int)StudentType.GameDesigner:
            {
                for (int i = 0; i < allProfessorCount; i++)
                {
                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.GameDesigner)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // 여기서 생성된 프리팹들에 데이터를 넣어줘야 한다
                        prefabParent.name = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}
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

                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];
                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
            case (int)StudentType.Art:
            {
                for (int i = 0; i < allProfessorCount; i++)
                {
                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.Art)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // 여기서 생성된 프리팹들에 데이터를 넣어줘야 한다
                        prefabParent.name = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}
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
                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];
                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
            case (int)StudentType.Programming:
            {
                for (int i = 0; i < allProfessorCount; i++)
                {
                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.Programming)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // 여기서 생성된 프리팹들에 데이터를 넣어줘야 한다
                        prefabParent.name = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}
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
                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];
                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
        }
    }

    // 종료버튼이 동작하도록 하는 기능함수
    public void IfClickAllInstructorQuitButton()
    {
        Debug.Log("전체강사창 종료");

        // 오브젝트 풀 돌려주기
        // 기존에 있던 오브젝트를 다시 오브젝트 풀에 돌려주자
        // 여기서 안돌아가는 이유를 알고, 데이터가 제대로 안들어가는 이유도 체크하자
        int AllInstructorObj = ParentInstructorPrefab.childCount;        // 

        for (int i = AllInstructorObj - 1; i >= 0; i--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(ParentInstructorPrefab.GetChild(0).gameObject);
        }

        PopOffAllInstructorPanel.TurnOffUI();
    }

    // 전체강사창의 인덱스를 클릭 했을 때
    public void IfClickInstructorIndex()
    {
        GameObject nowButton = EventSystem.current.currentSelectedGameObject;
        int nowInstructorCount;
        List<ProfessorStat> tempData = new List<ProfessorStat>();
        tempData.Clear();

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

        nowInstructorCount = tempData.Count;

        if (TeacherLevelToggle.isOn == true)
        {
            tempData = tempData.OrderByDescending(x => x.m_ProfessorPower).ToList();
        }
        else
        {
            tempData = tempData.OrderByDescending(x => x.m_ProfessorSet).ToList();
        }

        //  각 학과별 인덱스 위치 이동
        //InstructorDepartmentIndexSetting();
        // 기존에 있던 오브젝트를 다시 오브젝트 풀에 돌려주자
        // 여기서 안돌아가는 이유를 알고, 데이터가 제대로 안들어가는 이유도 체크하자
        int AllInstructorObj = ParentInstructorPrefab.childCount;        // 

        for (int i = AllInstructorObj - 1; i >= 0; i--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(ParentInstructorPrefab.GetChild(0).gameObject);
        }

        // 현재 토글이 어떤 상태인지 확인

        switch (nowButton.name)
        {
            case "GameDesignerIndexButton":
            {
                Debug.Log("기획인덱스버튼클릭");
                NowTeacherTypeIndex = (int)StudentType.GameDesigner;

                for (int i = 0; i < nowInstructorCount; i++)
                {
                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.GameDesigner)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        prefabParent.name = tempData[i].m_ProfessorName;

                        tempTeacherPrefabs.name = tempData[i].m_ProfessorName;
                        tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}

                        tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;
                        tempTeacherPrefabs.GetTeacherLevelText.text = "Lv. " + tempData[i].m_ProfessorPower.ToString();

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
                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_nametag_info];

                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
            case "ArtIndexButton":
            {

                for (int i = 0; i < nowInstructorCount; i++)
                {
                    Debug.Log("아트인덱스버튼클릭");
                    NowTeacherTypeIndex = (int)StudentType.Art;

                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.Art)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        prefabParent.name = tempData[i].m_ProfessorName;

                        tempTeacherPrefabs.name = tempData[i].m_ProfessorName;
                        tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}

                        tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;
                        tempTeacherPrefabs.GetTeacherLevelText.text = "Lv. " + tempData[i].m_ProfessorPower.ToString();

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

                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_nametag_info];

                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
            case "ProgrammingIndexButton":
            {

                for (int i = 0; i < nowInstructorCount; i++)
                {
                    Debug.Log("플밍인덱스버튼클릭");
                    NowTeacherTypeIndex = (int)StudentType.Programming;

                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;
                    if (tempData[i].m_ProfessorType == StudentType.Programming)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //선택한 이벤트 프리팹 생성했으니
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        prefabParent.name = tempData[i].m_ProfessorName;

                        tempTeacherPrefabs.name = tempData[i].m_ProfessorName;
                        tempTeacherPrefabs.GetInstructorName.text = tempData[i].m_ProfessorName;

                        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
                        //{
                        //    if (tempData[i].m_ProfessorName == temp.ToString())
                        //    {
                        //        tempTeacherPrefabs.GetTeacherProfileImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
                        //    }
                        //}

                        tempTeacherPrefabs.GetTeacherProfileImage.sprite = tempData[i].m_TeacherProfileImg;
                        tempTeacherPrefabs.GetTeacherLevelText.text = "Lv. " + tempData[i].m_ProfessorPower.ToString();

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

                        GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                        ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                        ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];
                        tempTeacherPrefabs.GetTeacherNameImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_nametag_info];

                        tempTeacherPrefabs.GetInstructorImageButton.onClick.AddListener(slideMenuPanel.IFClickEachInstructorButton);
                    }
                }
            }
            break;
        }
    }
}

