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
/// ��ü�������� �г��� ������Ʈ���� ��� �� ��ũ��Ʈ.
/// ���⼭ �г��� ���� ���� ���� �� �� ���̴�.
/// </summary>
public class AllInstructorPanel : MonoBehaviour
{
    [Header("��ü�����Ͽ� ���� �������� ���� ģ��")]
    [SerializeField] private InstructorPrefab instructorPrefabs;

    [Space(5f)]
    [Header("���� ����â�� �а� �ε��� ������")]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;

    [Space(5f)]
    [Header("���� ���� �̿ܿ� �˾�â�� ��ư ������")]
    [SerializeField] private Toggle TeacherTypeToggle;
    [SerializeField] private Toggle TeacherLevelToggle;

    [SerializeField] private Button EachInstructorButton;       // �� ģ���� Ŭ���� �Ǿ��� ���� üũ�� �ؾ��Ѵ�

    [SerializeField] private Button AllInstructorQuitButton;     // ���� ��ư Ŭ�� �� ���簭�������ƮǮ ����

    [SerializeField] private Transform ParentInstructorPrefab;

    [SerializeField] private PopOffUI PopOffAllInstructorPanel;

    public RectTransform IndexInitPivot;
    GameObject allInstructorPanel;
    [SerializeField] private SlideMenuPanel slideMenuPanel;

    private int NowTeacherTypeIndex = 0;


    // ������ Ÿ���� �����ϱ� ���� int ����
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

        // ��ü ����â���� �� �а����� ������ ���̵���
        GameDesignerIndexButton.onClick.AddListener(IfClickInstructorIndex);
        ArtIndexButton.onClick.AddListener(IfClickInstructorIndex);
        ProgrammingIndexButton.onClick.AddListener(IfClickInstructorIndex);

        //slideMenuPanel = GameObject.Find("SlideMenu_Panel").GetComponent<SlideMenuPanel>();
    }

    // ������ �а� ��ư�� Ŭ�� ���� �� ���� ������ ������ �а�Ÿ�Ժ��� ������
    void IfClickDepartmentIndex(StudentType departmentType)
    {
        switch (departmentType)
        {
            case StudentType.GameDesigner:
            {
                // �л� �߸���
                // �⺻ Ÿ�Ժ��� ����
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

    // ��� ������ �Լ� �ߵ�, ���� ��� ���¸� bool������ ��ȯ�ؼ� ���
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
            Debug.Log("����Ÿ�����Ŭ��");
            tempData = tempData.OrderByDescending(x => x.m_ProfessorSet).ToList();

            thistoggle = TeacherTypeToggle.gameObject;
            ArrangeInstructorType(tempData);
        }
        else
        {
            Debug.Log("���緹�����Ŭ��");
            tempData = tempData.OrderByDescending(x => x.m_ProfessorPower).ToList();


            thistoggle = TeacherLevelToggle.gameObject;
            ArrangeInstructorType(tempData);
        }

    }

    // ��� Ŭ�� �� ��۴�� 
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
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // ���⼭ ������ �����յ鿡 �����͸� �־���� �Ѵ�
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
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // ���⼭ ������ �����յ鿡 �����͸� �־���� �Ѵ�
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
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
                        tempTeacherPrefabs = prefabParent.GetComponent<InstructorPrefab>();
                        prefabParent.transform.localScale = new Vector3(1f, 1f, 1f);

                        // ���⼭ ������ �����յ鿡 �����͸� �־���� �Ѵ�
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

    // �����ư�� �����ϵ��� �ϴ� ����Լ�
    public void IfClickAllInstructorQuitButton()
    {
        Debug.Log("��ü����â ����");

        // ������Ʈ Ǯ �����ֱ�
        // ������ �ִ� ������Ʈ�� �ٽ� ������Ʈ Ǯ�� ��������
        // ���⼭ �ȵ��ư��� ������ �˰�, �����Ͱ� ����� �ȵ��� ������ üũ����
        int AllInstructorObj = ParentInstructorPrefab.childCount;        // 

        for (int i = AllInstructorObj - 1; i >= 0; i--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(ParentInstructorPrefab.GetChild(0).gameObject);
        }

        PopOffAllInstructorPanel.TurnOffUI();
    }

    // ��ü����â�� �ε����� Ŭ�� ���� ��
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

        //  �� �а��� �ε��� ��ġ �̵�
        //InstructorDepartmentIndexSetting();
        // ������ �ִ� ������Ʈ�� �ٽ� ������Ʈ Ǯ�� ��������
        // ���⼭ �ȵ��ư��� ������ �˰�, �����Ͱ� ����� �ȵ��� ������ üũ����
        int AllInstructorObj = ParentInstructorPrefab.childCount;        // 

        for (int i = AllInstructorObj - 1; i >= 0; i--)
        {
            InstructorObjectPool.ReturnInstructorPrefab(ParentInstructorPrefab.GetChild(0).gameObject);
        }

        // ���� ����� � �������� Ȯ��

        switch (nowButton.name)
        {
            case "GameDesignerIndexButton":
            {
                Debug.Log("��ȹ�ε�����ưŬ��");
                NowTeacherTypeIndex = (int)StudentType.GameDesigner;

                for (int i = 0; i < nowInstructorCount; i++)
                {
                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.GameDesigner)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
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
                    Debug.Log("��Ʈ�ε�����ưŬ��");
                    NowTeacherTypeIndex = (int)StudentType.Art;

                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;

                    if (tempData[i].m_ProfessorType == StudentType.Art)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
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
                    Debug.Log("�ù��ε�����ưŬ��");
                    NowTeacherTypeIndex = (int)StudentType.Programming;

                    GameObject prefabParent;
                    InstructorPrefab tempTeacherPrefabs;
                    if (tempData[i].m_ProfessorType == StudentType.Programming)
                    {
                        prefabParent = InstructorObjectPool.GetInstructorPrefab(ParentInstructorPrefab);       //������ �̺�Ʈ ������ ����������
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

