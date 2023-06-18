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
/// ĳ���� Ŭ�� �� ����� ���� ����â�� ������Ʈ ������ �� ������Ʈ�� ���� �����ϴ°��� �����ϱ� ���� ��ũ��Ʈ
/// </summary>
public class InstructorInfoPanel : MonoBehaviour
{
    //////////////////////////////////////
    [SerializeField] private GameObject AllInstructorPanel;     //popup ���� �ٲٱ�

    [Header("���� ����â�� �а� �ε��� ������")]
    [Space(5f)]
    [SerializeField] private Button GameDesignerIndexButton;
    [SerializeField] private Button ArtIndexButton;
    [SerializeField] private Button ProgrammingIndexButton;

    [Space(10f)]
    [Header("������ �⺻ ���� ������")]
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
    [Header("����â�� ������ ���� â")]
    [SerializeField] private Image CheckLevelUpScreen;

    [SerializeField] private TextMeshProUGUI NowTeacherSkillLevelText;

    [SerializeField] private Image SpecialMoneyImg;
    [SerializeField] private TextMeshProUGUI NowSpecialMoneyAmountText;

    [SerializeField] private TextMeshProUGUI LevelUpAlarmText;
    [SerializeField] private TextMeshProUGUI LevelUpSliderBarPercentText;
    [SerializeField] private Slider LevelUpSliderBar;
    [SerializeField] private Image LevelUpAMountFillBar;                  // ������ Ÿ�ӿ� ���� ������ �����̵��

    [SerializeField] private Button CancleLevelUpButton;
    [SerializeField] private Button LevelUpOkButton;
    [SerializeField] private TextMeshProUGUI NeedSpecialMoneyForLevelUp;
    [Space(10f)]
    [SerializeField] private Image AlarmInfoScreenImg;                 // ������ �Ϸ�, ������ȭ ���� ���� ���� �� �˸�â
    [SerializeField] private TextMeshProUGUI AlarmInfoScreenText;

    [SerializeField] private Image LastAlarmInfoScreenImg;                 // ������ �Ϸ�, ������ȭ ���� ���� ���� �� �˸�â
    [SerializeField] private TextMeshProUGUI LastAlarmInfoScreenText;

    [Space(10f)]
    [Header("���� ��ų���� ����")]
    [SerializeField] private TextMeshProUGUI TeacherSkillName;
    [SerializeField] private TextMeshProUGUI TeacherSkillDetailInfo;

    [Space(10f)]
    [Header("��������â�� �μ� ��ư��")]
    [SerializeField] private Button BackButton;         // ���� ��ư ������ ������ â���� ����
    [SerializeField] private Button NextButton;         // ���� ��ư ������ ������ â���� ����

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

    // ���׷��̵� ��ư�� ������ �� �� ���׷��̵� �ϰڳĴ� â(������ �ְ� ����)
    private void IfClickLevelUpBUtton()
    {
        int nowProfessorLevel = m_nowProfessorStat.m_ProfessorPower;

        if (m_ProfessorData.m_PointPerClick[nowProfessorLevel] <= PlayerInfo.Instance.m_SpecialPoint)     // ��ȭ �ִ��� üũ
        {
            AlarmInfoScreenImg.gameObject.SetActive(false);
            NeedSpecialMoneyForLevelUp.text = "<color=#0012FF>" + m_ProfessorData.m_PointPerClick[nowProfessorLevel] + "</color>";      // ��ȭ ��� (�Ķ�)
        }
        else
        {
            AlarmInfoScreenText.text = "���� ��ȭ�� �����մϴ�.";
            AlarmInfoScreenImg.gameObject.SetActive(true);
            StartCoroutine(AlarmScreenPopOff(1));

            NeedSpecialMoneyForLevelUp.text = "<color=#FF0000>" + m_ProfessorData.m_PointPerClick[nowProfessorLevel] + "</color>";      // ��ȭ ����� (����)
        }

        LevelUpSliderBarPercentText.text = string.Format("{0:0.##}", ((float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower] * 100)) + "%";
        LevelUpSliderBar.value = (float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower];        // ���� ������ �����ϱ� ���� �ۼ�Ʈ ��ġ

        NowTeacherSkillLevelText.text = "Lv." + m_nowProfessorStat.m_ProfessorPower;     // ������ ��ų ���� ǥ��

        NowSpecialMoneyAmountText.text = PlayerInfo.Instance.m_SpecialPoint.ToString();

        string BonusStatPercent = ((float)m_ProfessorData.m_StatMagnification[m_nowProfessorStat.m_ProfessorPower] * 100 - 100).ToString();

        LevelUpAlarmText.text = "���� �� �л��� " + "<color=#009A14>" + BonusStatPercent + "%" + "</color>" + "�� ���ʽ� ������ ����ϴ�.";
        //LevelUpSliderBar.value = SliderBarAmount;
        CheckLevelUpScreen.gameObject.SetActive(true);
    }

    // �ڷ�ƾ���� �����̸� �ְ� �;.
    IEnumerator AlarmScreenPopOff(int delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        AlarmInfoScreenImg.gameObject.SetActive(false);
    }

    // �˸�â�� ���׷��̵� ��ư�� ������ ��! ���� �� �Լ�
    private void LevelUpTeacherLevel()
    {
        int nowProfessorLevel = m_nowProfessorStat.m_ProfessorPower;
        if (nowProfessorLevel >= 15)
        {
            // �̹� �ִ� ����
            AlarmInfoScreenText.text = "���׷��̵� �ְ� �ܰ��Դϴ�.";
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
            if (m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] <= PlayerInfo.Instance.m_SpecialPoint)     // ��ȭ �ִ��� üũ
            {
                NeedSpecialMoneyForLevelUp.text = "<color=#0012FF>" + m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] + "</color>";      // ��ȭ ��� (�Ķ�)
            }
            else
            {
                AlarmInfoScreenText.text = "���� ��ȭ�� �����մϴ�.";
                AlarmInfoScreenText.gameObject.SetActive(true);
                StartCoroutine(AlarmScreenPopOff(1));
                NeedSpecialMoneyForLevelUp.text = "<color=#FF0000>" + m_ProfessorData.m_PointPerClick[m_nowProfessorStat.m_ProfessorPower] + "</color>";      // ��ȭ ����� (����)
            }

            if (nowProfessorLevel < 15)
            {
                LevelUpSliderBarPercentText.text = string.Format("{0:0.##}", ((float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower] * 100)) + "%";
                LevelUpSliderBar.value = (float)m_nowProfessorStat.m_ProfessorExperience / (float)m_ProfessorData.m_ExperiencePerLevel[m_nowProfessorStat.m_ProfessorPower];        // ���� ������ �����ϱ� ���� �ۼ�Ʈ ��ġ

                NowTeacherSkillLevelText.text = "Lv." + m_nowProfessorStat.m_ProfessorPower;     // ������ ��ų ���� ǥ��
                float tempPercentAmount = ((float)m_ProfessorData.m_StatMagnification[m_nowProfessorStat.m_ProfessorPower] * 100 - 100);
                int temp = (int)tempPercentAmount;
                string BonusStatPercent = tempPercentAmount.ToString();

                NowSpecialMoneyAmountText.text = PlayerInfo.Instance.m_SpecialPoint.ToString();

                LevelUpAlarmText.text = "���� �� �л��� " + "<color=#009A14>" + BonusStatPercent + "%" + "</color>" + "�� ���ʽ� ������ ����ϴ�.";
            }
        }
    }

    // ������ i ��ư�� Ŭ���� ���� ���� ������ ����â �ȿ� �⺻ �����͸� ���� �Լ�
    public void ShowInstructorBasicInfo(GameObject characterObj)
    {
        Debug.Log("���� â ����?");

        TempNowClickedTeacher = characterObj;

        Instructor professorStat = TempNowClickedTeacher.GetComponent<Instructor>();
        ProfessorStat tempStat = professorStat.m_InstructorData;
        string professorSet = tempStat.m_ProfessorSet;      // ���� ���� �ܷ� ����
        StudentType tempType = professorStat.m_InstructorData.m_ProfessorType;

        TeacherNameText.text = TempNowClickedTeacher.name;                          // �̸� ���� �����ϰ� �ؾ���

        //foreach (ETeacherProfile temp in Enum.GetValues(typeof(ETeacherProfile)))
        //{
        //    if (TempNowClickedTeacher.name == temp.ToString())
        //    {
        //        InstructorImage.sprite = UISpriteLists.Instance.GetTeacherProfileSpriteList[(int)temp];
        //    }
        //}
        // ���� �̹��� �ֱ�
        InstructorImage.sprite = tempStat.m_TeacherProfileImg;

        switch (tempType)
        {
            case StudentType.GameDesigner:
                {
                    GameDesignerIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_tab_selected];
                    ArtIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.art_tab_notselect];
                    ProgrammingIndexButton.image.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.program_tab_notselect];
                    DepartmentImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.gamedesign_icon_info];

                    if (professorSet == "����")        // ���� �ܷ� ������ string ���� ����� �Ѵ�
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "�ܷ�")
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

                    if (professorSet == "����")        // ���� �ܷ� ������ string ���� ����� �Ѵ�
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "�ܷ�")
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

                    if (professorSet == "����")        // ���� �ܷ� ������ string ���� ����� �Ѵ�
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof1_icon_info];
                    }
                    else if (professorSet == "�ܷ�")
                    {
                        InstructorTagImage.sprite = UISpriteLists.Instance.GetDepartmentIndexImgList[(int)EDepartmentImgIndex.prof2_icon_info];
                    }
                }
                break;
        }

        string result = string.Format("{0:#,0}", tempStat.m_ProfessorPay);      // ���ڿ� �޸� ��� ���� �Լ�
        PayText.text = result + "G";

        LevelText.text = "Lv." + tempStat.m_ProfessorPower.ToString();          // ������ �Ŀ���
        PassionText.text = tempStat.m_ProfessorPassion.ToString();
        HealthText.text = tempStat.m_ProfessorHealth.ToString();
    }

    // ������ i ��ư�� Ŭ���� ���� ���� ������ ����â �ȿ� ��ų �����͸� ���� �Լ�
    /// <summary>
    /// ����� ������ �ʿ�. ������ ��ų�� ���� ������ �ٲ�. ��ų �ϳ�, ���� �ϳ�
    /// </summary>
    /// <param name="characterObj"></param>
    public void ShowInsrtuctorSkillInfo(GameObject characterObj)
    {
        Instructor professorStat = characterObj.GetComponent<Instructor>();
        ProfessorStat tempStat = professorStat.m_InstructorData;
        m_nowProfessorStat = professorStat.m_InstructorData;

        // 
        int power = tempStat.m_ProfessorPower;

        // ��ų����
        TeacherSkillName.text = tempStat.m_ProfessorSkills[0];
        TeacherSkillDetailInfo.text = tempStat.m_ProfessorSkills[1];
    }

    public void IfClickInsftructorPanelQuitButton()
    {
        CheckLevelUpScreen.gameObject.SetActive(false);

        popOffInstructorPanel.TurnOffUI();
    }
}
