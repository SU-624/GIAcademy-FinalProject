using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using StatData.Runtime;
using System;

/// <summary>
/// 2023. 03. 21 Mang
/// 
/// 캐릭터 클릭 시 띄워줄 강사 정보창의 오브젝트 모음과 그 오브젝트의 값이 변동하는것을 조정하기 위한 스크립트
/// </summary>
public class InstructorInfoPanel : MonoBehaviour
{
    //////////////////////////////////////
    [SerializeField] private GameObject AllInstructorPanel;     //popup 으로 바꾸기

    [Header("강사 정보창의 학과 인덱스 변수들")]
    [Space(5f)]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;

    [Space(10f)]
    [Header("강사의 기본 정보 변수들")]
    [SerializeField] private Image InstructorImage;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button LevelUpgradeButton;
    [Space(5f)]
    [SerializeField] private TextMeshProUGUI TeacherNameText;
    [SerializeField] private Button ChangeNameButton;
    [SerializeField] private Image DepartmentImage;
    [SerializeField] private Image InstructorTagImage;

    [Space(5f)]
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI PassionText;
    [SerializeField] private TextMeshProUGUI PayText;

    [Space(10f)]
    [Header("강사창의 레벨업 관련 창")]
    [SerializeField] private Image CheckLevelUpScreen;

    [SerializeField] private TextMeshProUGUI NowTeacherSkillLevelText;

    [SerializeField] private Image SpecialMoneyImg;
    [SerializeField] private TextMeshProUGUI NowSpecialMoneyAmountText;

    [SerializeField] private TextMeshProUGUI LevelUpAlarmText;
    [SerializeField] private TextMeshProUGUI LevelUpSliderBarPercentText;
    [SerializeField] private Slider LevelUpSliderBar;
    [SerializeField] private Image LevelUpAMountFillBar;                  // 정해진 타임에 따라 움직일 슬라이드바

    [SerializeField] private Button CancleLevelUpButton;
    [SerializeField] private Button LevelUpOkButton;
    [SerializeField] private TextMeshProUGUI NeedSpecialMoneyForLevelUp;
    [Space(10f)]
    [SerializeField] private Image AlarmInfoScreenImg;                 // 레벨업 완료, 소지재화 부족 등을 보여 줄 알림창
    [SerializeField] private TextMeshProUGUI AlarmInfoScreenText;

    [SerializeField] private Image LastAlarmInfoScreenImg;                 // 레벨업 완료, 소지재화 부족 등을 보여 줄 알림창
    [SerializeField] private TextMeshProUGUI LastAlarmInfoScreenText;

    [Space(10f)]
    [Header("강사 스킬정보 변수")]
    [SerializeField] private TextMeshProUGUI TeacherSkillName;
    [SerializeField] private TextMeshProUGUI TeacherSkillDetailInfo;

    [Space(10f)]
    [Header("강사정보창의 부속 버튼들")]
    [SerializeField] private Button BackButton;         // 이전 버튼 누르면 강사목록 창으로 변경
    [SerializeField] private Button NextButton;         // 이전 버튼 누르면 강사목록 창으로 변경

    [SerializeField] private Button QuitButton;

    [SerializeField] private PopOffUI popOffInstructorPanel;

    private Professor m_ProfessorData = new Professor();
    private ProfessorStat m_nowProfessorStat = new ProfessorStat();

    [SerializeField] private PopUpUI PopUpAlarmInfoScreen;
    [SerializeField] private PopOffUI PopOffAlarmInfoScreen;


    GameObject TempNowClickedTeacher;

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

    public Image GetInstructorImage
    {
        get { return InstructorImage; }
        set { InstructorImage = value; }
    }

    public TextMeshProUGUI GetLevelText
    {
        get { return LevelText; }
        set { LevelText = value; }
    }

    public TextMeshProUGUI GetTeacherNameText
    {
        get { return TeacherNameText; }
        set { TeacherNameText = value; }
    }

    public Image GetDepartmentImage
    {
        get { return DepartmentImage; }
        set { DepartmentImage = value; }
    }

    public Image GetInstructorTagImage
    {
        get { return InstructorTagImage; }
        set { InstructorTagImage = value; }
    }

    public TextMeshProUGUI GetPayText
    {
        get { return PayText; }
        set { PayText = value; }
    }

    public TextMeshProUGUI GetHealthText
    {
        get { return HealthText; }
        set { HealthText = value; }
    }

    public TextMeshProUGUI GetPassionText
    {
        get { return PassionText; }
        set { PassionText = value; }
    }

    public Button GetInstructorInfoQuitButton
    {
        get { return QuitButton; }
        set { QuitButton = value; }
    }

    public ProfessorStat GetnowProfessorStat
    {
        get { return m_nowProfessorStat; }
        set { m_nowProfessorStat = value; }
    }

    public TextMeshProUGUI GetTeacherSkillName
    {
        get { return TeacherSkillName; }
        set { TeacherSkillName = value; }
    }

    public TextMeshProUGUI GetTeacherSkillDetailInfo
    {
        get { return TeacherSkillDetailInfo; }
        set { TeacherSkillDetailInfo = value; }
    }

    public Button GetNextButton
    {
        get { return NextButton; }
        set { NextButton = value; }
    }

    public Button GetBackButton
    {
        get { return BackButton; }
        set { BackButton = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelUpgradeButton.onClick.AddListener(IfClickLevelUpBUtton);
        LevelUpOkButton.onClick.AddListener(LevelUpTeacherLevel);
        CancleLevelUpButton.onClick.AddListener(ClickLevelUpCancleButton);

        // LockedFirstSkillImage.gameObject.SetActive(true);
        // LockedSecondSkillImage.gameObject.SetActive(true);
        // LockedThirdSkillImage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ClickLevelUpCancleButton()
    {
        CheckLevelUpScreen.gameObject.SetActive(false);
    }

    // 업그레이드 버튼을 눌렀을 때 뜰 업그레이드 하겠냐는 창(데이터 넣고 띄우기)
    private void IfClickLevelUpBUtton()
    {
        int nowProfessorLevel = m_nowProfessorStat.m_ProfessorPower;

        if (m_ProfessorData.m_PointPerClick[nowProfessorLevel] <= PlayerInfo.Instance.m_SpecialPoint)     // 재화 있는지 체크
        {
            AlarmInfoScreenImg.gameObject.SetActive(false);
            NeedSpecialMoneyForLevelUp.text = "<color=#0012FF>" + m_ProfessorData.m_PointPerClick[nowProfessorLevel] + "</color>";      // 재화 충분 (파랑)
        }
        else
        {
            AlarmInfoScreenText.text = "소지 재화가 부족합니다.";
            AlarmInfoScreenImg.gameObject.SetActive(true);
            StartCoroutine(AlarmScreenPopOff(1));

            NeedSpecialMoneyForLevelUp.text = "<color=#FF0000>" + m_ProfessorData.m_PointPerClick[nowProfessorLevel] + "</color>";      // 재화 불충분 (빨강)
        }

        LevelUpSliderBarPercentText.text = string.Format("{0:0.##}", ((float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower] * 100)) + "%";
        LevelUpSliderBar.value = (float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower];        // 다음 레벨에 도달하기 위한 퍼센트 수치

        NowTeacherSkillLevelText.text = "Lv." + m_nowProfessorStat.m_ProfessorPower;     // 강사의 스킬 레벨 표시

        NowSpecialMoneyAmountText.text = PlayerInfo.Instance.m_SpecialPoint.ToString();

        string BonusStatPercent = ((float)m_ProfessorData.m_StatMagnification[m_nowProfessorStat.m_ProfessorPower] * 100 - 100).ToString();

        LevelUpAlarmText.text = "수업 시 학생이 " + "<color=#009A14>" + BonusStatPercent + "%" + "</color>" + "의 보너스 스탯을 얻습니다.";
        //LevelUpSliderBar.value = SliderBarAmount;
        CheckLevelUpScreen.gameObject.SetActive(true);
    }

    // 코루틴으로 딜레이를 주고 싶어서.
    IEnumerator AlarmScreenPopOff(int delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        AlarmInfoScreenImg.gameObject.SetActive(false);
    }

    // 알림창의 업그레이드 버튼을 눌렀을 때! 실행 될 함수
    private void LevelUpTeacherLevel()
    {
        int nowProfessorLevel = m_nowProfessorStat.m_ProfessorPower;
        if (nowProfessorLevel >= 15)
        {
            // 이미 최대 레벨
            AlarmInfoScreenText.text = "업그레이드 최고 단계입니다.";
            AlarmInfoScreenImg.gameObject.SetActive(true);

            StartCoroutine(AlarmScreenPopOff(1));

            return;
        }
        if (PlayerInfo.Instance.m_SpecialPoint >= m_ProfessorData.m_PointPerClick[nowProfessorLevel])
        {
            PlayerInfo.Instance.m_SpecialPoint -= m_ProfessorData.m_PointPerClick[nowProfessorLevel];

            m_nowProfessorStat.m_ProfessorExperience += m_ProfessorData.m_ExperiencePerClick[nowProfessorLevel];
            if (m_nowProfessorStat.m_ProfessorExperience >= m_ProfessorData.m_ExperiencePerLevel[nowProfessorLevel])
            {
                m_nowProfessorStat.m_ProfessorPower++;
                m_nowProfessorStat.m_ProfessorExperience -= m_ProfessorData.m_ExperiencePerLevel[nowProfessorLevel];
                LevelText.text = "Lv." + m_nowProfessorStat.m_ProfessorPower.ToString();
            }
            // -------------------
            if (m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] <= PlayerInfo.Instance.m_SpecialPoint)     // 재화 있는지 체크
            {
                NeedSpecialMoneyForLevelUp.text = "<color=#0012FF>" + m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] + "</color>";      // 재화 충분 (파랑)
            }
            else
            {
                AlarmInfoScreenText.text = "소지 재화가 부족합니다.";
                AlarmInfoScreenText.gameObject.SetActive(true);
                StartCoroutine(AlarmScreenPopOff(1));
                NeedSpecialMoneyForLevelUp.text = "<color=#FF0000>" + m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] + "</color>";      // 재화 불충분 (빨강)
            }

            if (nowProfessorLevel < 15)
            {
                LevelUpSliderBarPercentText.text = string.Format("{0:0.##}", ((float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower] * 100)) + "%";
                LevelUpSliderBar.value = (float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower];        // 다음 레벨에 도달하기 위한 퍼센트 수치

                NowTeacherSkillLevelText.text = "Lv." + m_nowProfessorStat.m_ProfessorPower;     // 강사의 스킬 레벨 표시
                float tempPercentAmount = ((float)m_ProfessorData.m_StatMagnification[m_nowProfessorStat.m_ProfessorPower] * 100 - 100);
                int temp = (int)tempPercentAmount;
                string BonusStatPercent = tempPercentAmount.ToString();

                NowSpecialMoneyAmountText.text = PlayerInfo.Instance.m_SpecialPoint.ToString();

                LevelUpAlarmText.text = "수업 시 학생이 " + "<color=#009A14>" + BonusStatPercent + "%" + "</color>" + "의 보너스 스탯을 얻습니다.";
            }
        }
    }

    // 강사의 i 버튼을 클릭시 나올 강사 개인의 정보창 안에 기본 데이터를 넣을 함수
    public void ShowInstructorBasicInfo(GameObject characterObj)
    {
        Debug.Log("선생 창 떴나?");

        TempNowClickedTeacher = characterObj;

        Instructor professorStat = TempNowClickedTeacher.GetComponent<Instructor>();
        ProfessorStat tempStat = professorStat.m_InstructorData;
        string professorSet = tempStat.m_ProfessorSet;      // 강사 전임 외래 구별
        StudentType tempType = professorStat.m_InstructorData.m_ProfessorType;

        TeacherNameText.text = TempNowClickedTeacher.name;                          // 이름 수정 가능하게 해야함

        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
        //{
        //    if (TempNowClickedTeacher.name == temp.ToString())
        //    {
        //        InstructorImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
        //    }
        //}
        // 강사 이미지 넣기
        InstructorImage.sprite = tempStat.m_TeacherProfileImg;

        switch (tempType)
        {
            case StudentType.GameDesigner:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                    DepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];

                    if (professorSet == "전임")        // 전임 외래 구별은 string 으로 해줘야 한다
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "외래")
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                    }
                }
                break;
            case StudentType.Art:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_selected];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                    DepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_icon_info];

                    if (professorSet == "전임")        // 전임 외래 구별은 string 으로 해줘야 한다
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "외래")
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                    }
                }
                break;
            case StudentType.Programming:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedegisn_tab_notselect];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_selected];
                    DepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.programming_icon2_icon];

                    if (professorSet == "전임")        // 전임 외래 구별은 string 으로 해줘야 한다
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "외래")
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                    }
                }
                break;
        }

        string result = string.Format("{0:#,0}", tempStat.m_ProfessorPay);      // 숫자에 콤마 찍기 위한 함수
        PayText.text = result + "G";

        LevelText.text = "Lv." + tempStat.m_ProfessorPower.ToString();          // 레벨이 파워임
        PassionText.text = tempStat.m_ProfessorPassion.ToString();
        HealthText.text = tempStat.m_ProfessorHealth.ToString();
    }

    // 강사의 i 버튼을 클릭시 나올 강사 개인의 정보창 안에 스킬 데이터를 넣을 함수
    /// <summary>
    /// 여기는 변경이 필요. 강사의 스킬에 대한 설정이 바뀜. 스킬 하나, 설명 하나
    /// </summary>
    /// <param name="characterObj"></param>
    public void ShowInsrtuctorSkillInfo(GameObject characterObj)
    {
        Instructor professorStat = characterObj.GetComponent<Instructor>();
        ProfessorStat tempStat = professorStat.m_InstructorData;
        m_nowProfessorStat = professorStat.m_InstructorData;

        // 
        int power = tempStat.m_ProfessorPower;

        // 스킬관련
        TeacherSkillName.text = tempStat.m_ProfessorSkills[0];
        TeacherSkillDetailInfo.text = tempStat.m_ProfessorSkills[1];
    }

    public void IfClickInsftructorPanelQuitButton()
    {
        CheckLevelUpScreen.gameObject.SetActive(false);

        popOffInstructorPanel.TurnOffUI();
    }
}
