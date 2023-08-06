using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  2023. 03. 23 Mang
///  
/// 여기서 슬라이드 메뉴의 클릭 -> 상호작용으로 행동하는 친구들을 다 넣어놔주자
/// </summary>
public class SlideMenuClickEvent : MonoBehaviour
{
    [Space(5f)]
    [SerializeField] private CharacterInfoPanel StudentListInfoPanel;
    [SerializeField] private InstructorInfoPanel InstructorListInfoPanel;
    [Space(5f)]
    [SerializeField] private AllStudentInfoPanel AllStudentListInfoPanel;     // 아직 안만듬
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

        Debug.Log("이거 온클릭이 안되나");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 전체강사리스트패널의 종료버튼에 온클릭함수달기
    public void IfIClickAllInstructorQuitButton()
    {
        AllInstructorListInfoPanel.IfClickAllInstructorQuitButton();
    }

    public void IfIClickAllStudentQuitButton()
    {
        Debug.Log("전체 학생 창 종료버튼 눌렀나");
        AllStudentListInfoPanel.IfClickAllStudentPanelQuitButton();
    }
}
