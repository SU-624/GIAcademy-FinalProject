using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetClassPanel : MonoBehaviour
{
    [SerializeField] private Image m_PartImage;
    [SerializeField] private Image m_ProfessorImage;
    [SerializeField] private TextMeshProUGUI m_Week;
    [SerializeField] private TextMeshProUGUI m_CurrentMoney;
    [SerializeField] private TextMeshProUGUI m_TotalClassMoney;
    [SerializeField] private TextMeshProUGUI m_TotalHealth;
    [SerializeField] private PopUpUI m_PopUpSelectClassPanel;
    [SerializeField] private PopOffUI m_PopOffSelectClassPanel;

    [Space(5f)]
    [Header("내가 클릭한 수업과 강사의 정보를 띄워주는 패널")]
    [SerializeField] private TextMeshProUGUI m_ClickClassMoney;
    [SerializeField] private TextMeshProUGUI m_ClickClassHealth;
    [SerializeField] private TextMeshProUGUI m_AllInfoPanelProfessorName;
    [SerializeField] private TextMeshProUGUI m_AllInfoPanelClassName;
    [SerializeField] private TextMeshProUGUI[] m_AllInfoPanelClassStatText;
    [SerializeField] private TextMeshProUGUI m_AllInfoPanelClassAllStatText;
    [SerializeField] private GameObject[] m_AllInfoPanelClassStat;
    [SerializeField] private GameObject m_AllInfoPanelClassAllStat;
    [SerializeField] private Image[] m_AllInfoPanelClassStatImage;
    [SerializeField] private Image m_AllInfoPanelClassAllStatImage;

    [Space(5f)]
    [Header("클릭한 수업의 수업료만큼 돈이 없거나 강사의 체력이 부족할 때 띄워줄 텍스트")]
    [SerializeField] private GameObject m_MoneyWarningMessage;
    [SerializeField] private GameObject m_HealthWarningMessage;
    [SerializeField] private GameObject m_CurrentMoneyWarningMessage;

    [Space(5f)]
    [Header("내가 지정한 수업이 몇 주차 어느 파트의 수업인지 띄워주는 패널")]
    [Header("1주차")]
    [SerializeField] private GameObject m_1WeekPanel;
    [SerializeField] private GameObject m_1WeekGMClass;
    [SerializeField] private GameObject m_1WeekArtClass;
    [SerializeField] private GameObject m_1WeekProgrammingClass;
    [SerializeField] private TextMeshProUGUI m_1WeekGMClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekGMClassMoney;
    [SerializeField] private TextMeshProUGUI m_1WeekArtClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekArtClassMoney;
    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassMoney;
    [SerializeField] private Button m_DownButton;

    [Space(5f)]
    [Header("2주차")]
    [SerializeField] private GameObject m_2WeekPanel;
    [SerializeField] private GameObject m_2WeekGMClass;
    [SerializeField] private GameObject m_2WeekArtClass;
    [SerializeField] private GameObject m_2WeekProgrammingClass;
    [SerializeField] private TextMeshProUGUI m_2WeekGMClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekGMClassMoney;
    [SerializeField] private TextMeshProUGUI m_2WeekArtClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekArtClassMoney;
    [SerializeField] private TextMeshProUGUI m_2WeekProgrammingClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekProgrammingClassMoney;
    [SerializeField] private Button m_UpButton;

    [Space(5f)]
    [SerializeField] private Transform m_ClassPrefabParent;
    [SerializeField] private Transform m_ProfessorPrefabParent;
    [SerializeField] private GameObject ClassPrefabParentObj;
    [SerializeField] private GameObject ProfessorPrefabParentObj;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_CompleteButton;
    [SerializeField] private Button m_HideButton;
    [SerializeField] private Button m_ModifiyButton;
    [SerializeField] private ScrollRect m_ClassScrollView;
    [SerializeField] private ScrollRect m_ProfessorScrollView;
    [SerializeField] private GameObject m_PartWeek;

    public RectTransform m_ClassPrefab;
    public RectTransform m_ProfessorPrefab;
    public GameObject MoneyWarningMessage { get { return m_MoneyWarningMessage; } set { m_MoneyWarningMessage = value; } }
    public GameObject HealthWarningMessage { get { return m_HealthWarningMessage; } set { m_HealthWarningMessage = value; } }
    public GameObject CurrentMoneyWarningMessage { get { return m_CurrentMoneyWarningMessage; } set { m_CurrentMoneyWarningMessage = value; } }
    public TextMeshProUGUI CurrentMoney { get { return m_CurrentMoney; } set { m_CurrentMoney = value; } }
    public Transform ClassPrefabParent { get { return m_ClassPrefabParent; } set { m_ClassPrefabParent = value; } }
    public Transform ProfessorPrefabParent { get { return m_ProfessorPrefabParent; } set { m_ProfessorPrefabParent = value; } }
    public GameObject[] AllInfoPanelClassStat { get { return m_AllInfoPanelClassStat; } set { m_AllInfoPanelClassStat = value; } }
    public GameObject AllInfoPanelClassAllStat { get { return m_AllInfoPanelClassAllStat; } set { m_AllInfoPanelClassAllStat = value; } }
    public TextMeshProUGUI[] AllInfoPanelClassStatText { get { return m_AllInfoPanelClassStatText; } set { m_AllInfoPanelClassStatText = value; } }
    public TextMeshProUGUI AllInfoPanelClassAllStatText { get { return m_AllInfoPanelClassAllStatText; } set { m_AllInfoPanelClassAllStatText = value; } }
    public TextMeshProUGUI WeekText { get { return m_Week; } set { m_Week = value; } }
    public Image[] AllInfoPanelClassStatImage { get { return m_AllInfoPanelClassStatImage; } set { m_AllInfoPanelClassStatImage = value; } }
    public Image PartImage { get { return m_PartImage; } set { m_PartImage = value; } }
    public GameObject _1WeekPanel { get { return m_1WeekPanel; } set { m_1WeekPanel = value; } }
    public GameObject _2WeekPanel { get { return m_2WeekPanel; } set { m_2WeekPanel = value; } }
    public Button ModifiyButton { get { return m_ModifiyButton; } set { m_ModifiyButton = value; } }
    public Button CompleteButton { get { return m_CompleteButton; } set { m_CompleteButton = value; } }
    public ScrollRect ClassScrollView { get { return m_ClassScrollView; } set { m_ClassScrollView = value; } }
    public ScrollRect ProfessorScrollView { get { return m_ProfessorScrollView; } set { m_ProfessorScrollView = value; } }
    public GameObject PartWeek { get { return m_PartWeek; } set { m_PartWeek = value; } }

    private void Start()
    {
        m_DownButton.onClick.AddListener(ClickUpAndDownButton);
        m_UpButton.onClick.AddListener(ClickUpAndDownButton);
    }

    public void SetPartImage(Sprite _partImage)
    {
        m_PartImage.sprite = _partImage;
    }

    public void SetWeekText(string _week)
    {
        m_Week.text = _week;
    }

    public void SetClassName(string _class)
    {
        m_AllInfoPanelClassName.text = _class;
    }

    public void SetClassUseHealth(string _health)
    {
        m_ClickClassHealth.text = _health;
    }

    public void SetProfessorInfo(string _name, Sprite _profile)
    {
        m_AllInfoPanelProfessorName.text = _name;
        m_ProfessorImage.sprite = _profile;
    }

    public void SetClassMoney(Color _color, string _money)
    {
        m_ClickClassMoney.color = _color;
        m_ClickClassMoney.text = _money;
    }

    public void TotalMoney(Color _color, string _money)
    {
        m_TotalClassMoney.color = _color;
        m_TotalClassMoney.text = _money;
    }

    public void TotalHealth(string _health)
    {
        m_TotalHealth.text = _health;
    }

    public void SetProfessorHealth(Color _color)
    {
        m_ClickClassHealth.color = _color;
    }

    public void SetModifiyButton(bool _flag)
    {
        m_ModifiyButton.gameObject.SetActive(_flag);
    }

    public void SetBackButtonActive(bool _flag)
    {
        m_BackButton.gameObject.SetActive(_flag);
    }

    public void PopUpSelectClassPanel()
    {
        m_PopUpSelectClassPanel.TurnOnUI();
    }

    public void PopOffSelectClassPanel()
    {
        m_PopOffSelectClassPanel.TurnOffUI();
    }

    public void SetAllStatText(string _stat = "")
    {
        m_AllInfoPanelClassAllStatText.text = _stat;
    }

    public void SetStatsActive(bool _allStatFlag = false)
    {
        m_AllInfoPanelClassAllStat.SetActive(_allStatFlag);
    }

    public void Set1WeekGMPanel(bool _1WeekGMClass, string _1WeekGMClassName = "", string _1WeekGMClassMoney = "")
    {
        m_1WeekGMClass.SetActive(_1WeekGMClass);
        m_1WeekGMClassName.text = _1WeekGMClassName;
        m_1WeekGMClassMoney.text = _1WeekGMClassMoney;
    }

    public void Set2WeekGMPanel(bool _2WeekGMClass, string _2WeekGMClassName = "", string _2WeekGMClassMoney = "")
    {
        m_2WeekGMClass.SetActive(_2WeekGMClass);
        m_2WeekGMClassName.text = _2WeekGMClassName;
        m_2WeekGMClassMoney.text = _2WeekGMClassMoney;
    }

    public void Set1WeekArtPanel(bool _1WeekArtClass, string _1WeekArtClassName = "", string _1WeekArtClassMoney = "")
    {
        m_1WeekArtClass.SetActive(_1WeekArtClass);
        m_1WeekArtClassName.text = _1WeekArtClassName;
        m_1WeekArtClassMoney.text = _1WeekArtClassMoney;
    }

    public void Set2WeekArtPanel(bool _2WeekArtClass, string _2WeekArtClassName = "", string _2WeekArtClassMoney = "")
    {
        m_2WeekArtClass.SetActive(_2WeekArtClass);
        m_2WeekArtClassName.text = _2WeekArtClassName;
        m_2WeekArtClassMoney.text = _2WeekArtClassMoney;
    }

    public void Set1WeekProgrammingPanel(bool _1WeekProgrammingClass, string _1WeekProgrammingClassName = "", string _1WeekProgrammingClassMoney = "")
    {
        m_1WeekProgrammingClass.SetActive(_1WeekProgrammingClass);
        m_1WeekProgrammingClassName.text = _1WeekProgrammingClassName;
        m_1WeekProgrammingClassMoney.text = _1WeekProgrammingClassMoney;
    }

    public void Set2WeekProgrammingPanel(bool _2WeekProgrammingClass, string _2WeekProgrammingClassName = "", string _2WeekProgrammingClassMoney = "")
    {
        m_2WeekProgrammingClass.SetActive(_2WeekProgrammingClass);
        m_2WeekProgrammingClassName.text = _2WeekProgrammingClassName;
        m_2WeekProgrammingClassMoney.text = _2WeekProgrammingClassMoney;
    }

    public void SetDownButton(bool _flag)
    {
        m_DownButton.gameObject.SetActive(_flag);
    }

    public void SetUpButton(bool _flag)
    {
        m_UpButton.gameObject.SetActive(_flag);
    }

    public void ClickUpAndDownButton()
    {
        if(m_1WeekPanel.activeSelf)
        {
            m_DownButton.gameObject.SetActive(false);
            m_2WeekPanel.SetActive(true);
            m_1WeekPanel.SetActive(false);
            m_UpButton.gameObject.SetActive(true);
        }
        else if(m_2WeekPanel.activeSelf)
        {
            m_UpButton.gameObject.SetActive(false);
            m_1WeekPanel.SetActive(true);
            m_2WeekPanel.SetActive(false);
            m_DownButton.gameObject.SetActive(true);
        }
    }

    public void DestroySelectClassObject()
    {
        Transform[] _childCount = ClassPrefabParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != ClassPrefabParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    public void DestroySelectPanelProfessorObject()
    {
        Transform[] _childCount = ProfessorPrefabParentObj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != ProfessorPrefabParentObj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

}
