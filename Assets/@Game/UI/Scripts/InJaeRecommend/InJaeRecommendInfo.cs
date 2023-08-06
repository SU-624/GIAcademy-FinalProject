using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ���� Ability enum�̶� ������ �޶� ������õ���� ����ϴ� enum��
public enum RecommendAbility
{
    Sense,
    Concentration,
    Wit,
    Technique,
    Insight,
    Count
}

public class InJaeRecommendInfo : MonoBehaviour
{
    [Header("�г��� �ؽ�Ʈ�� ���� �̹�����")]
    [SerializeField] private TextMeshProUGUI m_CompanyName;
    [SerializeField] private TextMeshProUGUI m_Title;
    [SerializeField] private TextMeshProUGUI m_RescommendInfoText;
    [SerializeField] private TextMeshProUGUI m_Salary;
    [SerializeField] private TextMeshProUGUI m_Task;
    [SerializeField] private TextMeshProUGUI m_Percent;
    [SerializeField] private TextMeshProUGUI[] m_Notification;
    [SerializeField] private TextMeshProUGUI[] m_StudentStatText;
    [SerializeField] private Image[] m_StudentStatImage;

    [SerializeField] private GameObject m_StudentStat;
    [SerializeField] private GameObject m_RecruitmentPartGameDesigner;
    [SerializeField] private GameObject m_RecruitmentPartArt;
    [SerializeField] private GameObject m_RecruitmentPartProgramming;
    [SerializeField] private GameObject m_ImpossibleRecommendPanel;
    [SerializeField] private GameObject m_RecommendInfoBGPanel;

    [SerializeField] private Transform m_StudentListContent;
    [SerializeField] private Transform m_RequiredSkillPrefabParent;
    [SerializeField] private ScrollRect m_StudentListRectScrollRect;            // �л��鸮��Ʈ�� ���� ScrollRect
    [Space(5f)]
    [Header("��õ�ϱ� ��ư�� ������ �� ��õ�� �л����� �����ִ� Ȯ��â")]
    [SerializeField] private GameObject m_CheckPanel;
    [SerializeField] private Image m_CheckPanelCompanyGrade;
    [SerializeField] private TextMeshProUGUI m_CheckPanelCompanyName;
    [SerializeField] private Transform m_CheckPanelStudentPrefabParent;
    [SerializeField] private Button m_CheckPanelCancleButton;
    [SerializeField] private Button m_CheckPanelOKButton;
    [SerializeField] private ScrollRect m_CheckStudnetContentScrollRect;        // �л��鸮��Ʈ�� ���� ScrollRect

    [Space(5f)]
    [Header("��Ʈ�� ��ư")]
    [SerializeField] private Button m_PartGameDesigner;
    [SerializeField] private Button m_PartArt;
    [SerializeField] private Button m_PartProgramming;

    [Space(5f)]
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_RecommendButton;
    [SerializeField] private PopUpUI m_PopUpRecommendInfoPanel;                 // ��õ ������ ���� ������ ������ �г�
    [SerializeField] private PopUpUI m_PopUpRecommendListPaenl;                 // ��õ ������ ȸ�� ��ư�� ������ �г�
    [SerializeField] private PopOffUI m_PopOffRecommendInfoPanel;
    [SerializeField] private PopUpUI m_PopUpImpossibleRecommendPanel;           // ���� ��õ�Ϸ��� �л� �� ��õ�� �Ұ����� �л��� �ִٸ� ����� �г�
    [SerializeField] private PopOffUI m_PopOffImpossibleRecommendPanel;
    [SerializeField] private TextMeshProUGUI m_PopUpImpossibleRecommendText;           // ���� ��õ�Ϸ��� �л� �� ��õ�� �Ұ����� �л��� �ִٸ� ����� �г�

    [Space(5f)]
    [Header("Ʃ�丮���")]
    [SerializeField] private RectTransform m_RecommendInfoRect;
    [SerializeField] private RectTransform m_PartSkillInfoRect;
    [SerializeField] private RectTransform m_StatInfoRect;
    [SerializeField] private RectTransform m_StudentListRect;
    [SerializeField] private RectTransform m_PercentRect;

    public RectTransform RecommendInfoRect { get { return m_RecommendInfoRect; } set { m_RecommendInfoRect = value; } }
    public RectTransform PartSkillInfoRect { get { return m_PartSkillInfoRect; } set { m_PartSkillInfoRect = value; } }
    public RectTransform StatInfoRect { get { return m_StatInfoRect; } set { m_StatInfoRect = value; } }
    public RectTransform StudentListRect { get { return m_StudentListRect; } set { m_StudentListRect = value; } }
    public RectTransform PercentRect { get { return m_PercentRect; } set { m_PercentRect = value; } }

    public Button PartGameDesignerButton { get { return m_PartGameDesigner; } set { m_PartGameDesigner = value; } }
    public GameObject StudentStatObj { get { return m_StudentStat; } set { m_StudentStat = value; } }
    public Button CheckPanelOKButton { get { return m_CheckPanelOKButton; } set { m_CheckPanelOKButton = value; } }
    public Button CheckPanelCancleButton { get { return m_CheckPanelCancleButton; } set { m_CheckPanelCancleButton = value; } }
    public Button RecommendButton { get { return m_RecommendButton; } set { m_RecommendButton = value; } }
    public Transform CheckPanelStudentParent { get { return m_CheckPanelStudentPrefabParent; } set { m_CheckPanelStudentPrefabParent = value; } }
    public Transform StudentListContent { get { return m_StudentListContent; } set { m_StudentListContent = value; } }

    private void Start()
    {
        m_CloseButton.onClick.AddListener(ClickCloseButton);
        m_BackButton.onClick.AddListener(ClickBackButton);
        m_CheckPanelCancleButton.onClick.AddListener(ClickCheckPanelCancleButton);
    }

    // content���� �� ���� �����ش�.
    public void ChangeScrollRectTransform()
    {
        m_StudentListContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        m_CheckStudnetContentScrollRect.horizontalNormalizedPosition = 0f;
    }

    public void LockInfoPanelCloseButton(bool _isTrue = true)
    {
        m_CloseButton.enabled = _isTrue;
    }

    // ���� �г��� ���ְ� ���� �������� �����ش�.
    private void ClickBackButton()
    {
        m_PopUpRecommendListPaenl.TurnOnUI();
        m_PopOffRecommendInfoPanel.TurnOffUI();
        ChangeScrollRectTransform();
    }

    private void ClickCloseButton()
    {
        PopOffRecommendInfoPanel();
        SetActiveImpossibleRecommendPanel(false);
        SetPercent("?? %");
        ChangeScrollRectTransform();
    }

    public void ClickCheckPanelCancleButton()
    {
        m_RecommendInfoBGPanel.SetActive(true);
        m_CheckPanel.SetActive(false);
        ChangeScrollRectTransform();
    }

    public void PopUpRecommendInfoPanel()
    {
        m_PopUpRecommendInfoPanel.TurnOnUI();
    }

    public void PopOffRecommendInfoPanel()
    {
        m_PopOffRecommendInfoPanel.TurnOffUI();
    }

    public Transform RequiredSkillParentTransform()
    {
        return m_RequiredSkillPrefabParent;
    }

    public void SetRecommendInfoListText(string _companyname, string _title, string _recommendinfotext, string _salary, string _task)
    {
        m_CompanyName.text = _companyname;
        m_Title.text = _title;
        m_RescommendInfoText.text = _recommendinfotext;
        m_Salary.text = _salary;
        m_Task.text = _task;
    }

    public void SetNotification(string _sense, string _concentration, string _wit, string _technique, string _insight)
    {
        m_Notification[(int)RecommendAbility.Sense].text = _sense;
        m_Notification[(int)RecommendAbility.Concentration].text = _concentration;
        m_Notification[(int)RecommendAbility.Wit].text = _wit;
        m_Notification[(int)RecommendAbility.Technique].text = _technique;
        m_Notification[(int)RecommendAbility.Insight].text = _insight;
    }

    public void SetStudentStatText(string _sense, string _concentration, string _wit, string _technique, string _insight)
    {
        m_StudentStatText[(int)RecommendAbility.Sense].text = _sense;
        m_StudentStatText[(int)RecommendAbility.Concentration].text = _concentration;
        m_StudentStatText[(int)RecommendAbility.Wit].text = _wit;
        m_StudentStatText[(int)RecommendAbility.Technique].text = _technique;
        m_StudentStatText[(int)RecommendAbility.Insight].text = _insight;
    }

    public void SetStudentStatImage(Sprite _sense, Sprite _concentration, Sprite _wit, Sprite _technique, Sprite _insight)
    {
        m_StudentStatImage[(int)RecommendAbility.Sense].sprite = _sense;
        m_StudentStatImage[(int)RecommendAbility.Concentration].sprite = _concentration;
        m_StudentStatImage[(int)RecommendAbility.Wit].sprite = _wit;
        m_StudentStatImage[(int)RecommendAbility.Technique].sprite = _technique;
        m_StudentStatImage[(int)RecommendAbility.Insight].sprite = _insight;
    }

    // �������� ���ϴ� ��Ʈ�� �����ֱ� ���� �Լ�
    public void SetRecruitmentPart(int _part)
    {
        // �ϴ� �� ���ְ� �ٽ� �������ֱ�
        m_RecruitmentPartGameDesigner.SetActive(false);
        m_RecruitmentPartArt.SetActive(false);
        m_RecruitmentPartProgramming.SetActive(false);

        switch (_part)
        {
            case 0:
            {
                m_RecruitmentPartGameDesigner.SetActive(true);
                m_RecruitmentPartArt.SetActive(true);
                m_RecruitmentPartProgramming.SetActive(true);
            }
            break;

            case 1: m_RecruitmentPartGameDesigner.SetActive(true); break;
            case 2: m_RecruitmentPartArt.SetActive(true); break;
            case 3: m_RecruitmentPartProgramming.SetActive(true); break;
        }
    }
    // �л��� ������ �� �ٲ��ַ��� ���� ���� �Լ��� �������.
    public void SetPercent(string _percent)
    {
        m_Percent.text = _percent;
    }

    public void SetActiveImpossibleRecommendPanel(bool _isTure, string _warnningMessage = "��õ�� �Ұ��� �л��� �����մϴ�!")
    {
        if (_isTure)
        {
            m_PopUpImpossibleRecommendPanel.TurnOnUI();
            m_PopUpImpossibleRecommendText.text = _warnningMessage;
        }
        else
        {
            m_PopOffImpossibleRecommendPanel.TurnOffUI();
        }

        //m_ImpossibleRecommendPanel.SetActive(_isTure);
    }

    public void DestroyStudentList()
    {
        Transform[] _childCount = m_StudentListContent.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_StudentListContent.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void DestroyCheckPanelStudentList()
    {
        Transform[] _childCount = m_CheckPanelStudentPrefabParent.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != m_CheckPanelStudentPrefabParent.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void SetCheckRecommendPanel()
    {
        m_RecommendInfoBGPanel.SetActive(false);
        m_CheckPanel.SetActive(true);
    }

    public void SetCheckPanelCompanyGradeIcon(Sprite _grade)
    {
        m_CheckPanelCompanyGrade.sprite = _grade;
    }

    public void SetCheckPanelCompanyName(string _name)
    {
        m_CheckPanelCompanyName.text = _name;
    }
}
