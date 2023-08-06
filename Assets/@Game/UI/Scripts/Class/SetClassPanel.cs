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
    [SerializeField] private TextMeshProUGUI m_WarningText;
    [SerializeField] private PopUpUI m_PopUpSelectClassPanel;
    [SerializeField] private PopOffUI m_PopOffSelectClassPanel;
    [SerializeField] private PopUpUI m_PopUpWarningPanel;
    [SerializeField] private PopOffUI m_PopOffWarningPanel;

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
    [SerializeField] private GameObject m_1WeekGameDesignerClass;
    [SerializeField] private GameObject m_1WeekArtClass;
    [SerializeField] private GameObject m_1WeekProgrammingClass;
    [SerializeField] private TextMeshProUGUI m_1WeekGameDesignerClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekGameDesignerClassMoney;
    [SerializeField] private TextMeshProUGUI m_1WeekArtClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekArtClassMoney;
    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassName;
    [SerializeField] private TextMeshProUGUI m_1WeekProgrammingClassMoney;
    [SerializeField] private Button m_DownButton;

    [Space(5f)]
    [Header("2주차")]
    [SerializeField] private GameObject m_2WeekPanel;
    [SerializeField] private GameObject m_2WeekGameDesignerClass;
    [SerializeField] private GameObject m_2WeekArtClass;
    [SerializeField] private GameObject m_2WeekProgrammingClass;
    [SerializeField] private TextMeshProUGUI m_2WeekGameDesignerClassName;
    [SerializeField] private TextMeshProUGUI m_2WeekGameDesignerClassMoney;
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

    [Space(5f)]
    [Header("튜토리얼용")]
    [SerializeField] private RectTransform m_ProfessorListRect;
    [SerializeField] private RectTransform m_ClassListRect;
    [SerializeField] private RectTransform m_MoneyRect;


    public RectTransform m_ClassPrefab;
    public RectTransform m_ProfessorPrefab;
    public TextMeshProUGUI CurrentMoney { get { return m_CurrentMoney; } set { m_CurrentMoney = value; } }
    public TextMeshProUGUI m_TotalMoney { get { return m_TotalClassMoney; } }
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
    public RectTransform ProfessorListRect { get { return m_ProfessorListRect; } set { m_ProfessorListRect = value; } }
    public RectTransform ClassListRect { get { return m_ClassListRect; } set { m_ClassListRect = value; } }
    public RectTransform MoneyRect { get { return m_MoneyRect; } set { m_MoneyRect = value; } }

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

    //public void SetClassMoney(Color _color, string _money)
    //{
    //    m_TotalClassMoney.color = _color;
    //    m_TotalClassMoney.text = _money;
    //}

    public void TotalMoney(Color _color, string _money)
    {
        m_TotalClassMoney.color = _color;
        m_TotalClassMoney.text =  _money;
    }

    public void SetWarningText(string _text)
    {
        m_WarningText.text = _text;
        m_PopUpWarningPanel.TurnOnUI();
        m_PopOffWarningPanel.DelayTurnOffUI();
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

    public void Set1WeekGameDesignerPanel(bool _1WeekGameDesignerClass, string _1WeekGameDesignerClassName = "", string _1WeekGameDesignerClassMoney = "")
    {
        m_1WeekGameDesignerClass.SetActive(_1WeekGameDesignerClass);
        m_1WeekGameDesignerClassName.text = _1WeekGameDesignerClassName;
        m_1WeekGameDesignerClassMoney.text = _1WeekGameDesignerClassMoney;
    }

    public void Set2WeekGameDesignerPanel(bool _2WeekGameDesignerClass, string _2WeekGameDesignerClassName = "", string _2WeekGameDesignerClassMoney = "")
    {
        m_2WeekGameDesignerClass.SetActive(_2WeekGameDesignerClass);
        m_2WeekGameDesignerClassName.text = _2WeekGameDesignerClassName;
        m_2WeekGameDesignerClassMoney.text = _2WeekGameDesignerClassMoney;
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
        if (m_1WeekPanel.activeSelf)
        {
            m_DownButton.gameObject.SetActive(false);
            m_2WeekPanel.SetActive(true);
            m_1WeekPanel.SetActive(false);
            m_UpButton.gameObject.SetActive(true);
        }
        else if (m_2WeekPanel.activeSelf)
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
