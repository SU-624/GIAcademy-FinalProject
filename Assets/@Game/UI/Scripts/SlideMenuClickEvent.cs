using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  2023. 03. 23 Mang
///  
/// ���⼭ �����̵� �޴��� Ŭ�� -> ��ȣ�ۿ����� �ൿ�ϴ� ģ������ �� �־������
/// </summary>
public class SlideMenuClickEvent : MonoBehaviour
{
    [Space(5f)]
    [SerializeField] private CharacterInfoPanel StudentListInfoPanel;
    [SerializeField] private InstructorInfoPanel InstructorListInfoPanel;
    [Space(5f)]
    [SerializeField] private AllStudentInfoPanel AllStudentListInfoPanel;     // ���� �ȸ���
    [SerializeField] private AllInstructorPanel AllInstructorListInfoPanel;
    [Space(5f)]
    [SerializeField] private SlideMenuPanel slideMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        StudentListInfoPanel.LoadAllDepartmentIndexImg();
        StudentListInfoPanel.LoadStudentSkillLevelImg();

        //slideMenuPanel.GetPopOffManagementButton.gameObject.SetActive(false);
        //slideMenuPanel.GetSecondContentUI.SetActive(false);
        slideMenuPanel.GetSecondManagementUI.SetActive(false);

        slideMenuPanel.Initialize();
        slideMenuPanel.GetInstructorListButton.onClick.AddListener(slideMenuPanel.InitializeInstructorButton);
        slideMenuPanel.GetStudentListButton.onClick.AddListener(slideMenuPanel.InitializeStudentButton);

        AllInstructorListInfoPanel.GetAllInstructorQuitButton.onClick.AddListener(IfIClickAllInstructorQuitButton);
        AllInstructorListInfoPanel.InitAllInstructorPanel();

        AllStudentListInfoPanel.GetAllStudentQuitButton.onClick.AddListener(IfIClickAllStudentQuitButton);

        InstructorListInfoPanel.GetInstructorInfoQuitButton.onClick.AddListener(InstructorListInfoPanel.IfClickInsftructorPanelQuitButton);
        StudentListInfoPanel.GetStudentInfoQuitButton.onClick.AddListener(StudentListInfoPanel.IfClickStudentPanelQuitButton);

        Debug.Log("�̰� ��Ŭ���� �ȵǳ�");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ��ü���縮��Ʈ�г��� �����ư�� ��Ŭ���Լ��ޱ�
    public void IfIClickAllInstructorQuitButton()
    {
        AllInstructorListInfoPanel.IfClickAllInstructorQuitButton();
    }

    public void IfIClickAllStudentQuitButton()
    {
        Debug.Log("��ü �л� â �����ư ������");
        AllStudentListInfoPanel.IfClickAllStudentPanelQuitButton();
    }
}
