using System.Collections;
using System.Collections.Generic;
using StatData.Runtime;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public struct SaveSelectClassInfoData
{
    public Class m_SelectClassDataSave;
    public ProfessorStat m_SelectProfessorDataSave;
    public string m_Week;
}

/// <summary>
/// ���� �ٲ� ���� ���� �˾�â�� ���� �������� ���� ���� ��...
/// 
/// 23.02.13 Ocean
/// </summary>
public class SelectClass : MonoBehaviour
{
    [SerializeField] private GameObject m_ClassPrefab;
    [SerializeField] private GameObject m_ProfessorPrefab;
    [SerializeField] private GameObject m_CheckClassPrefab;


    [SerializeField] private GameObject m_CheckClassPanel;
    [SerializeField] private GameObject m_SelectClassPanel;

    [SerializeField] private GameObject m_AllInfoPanel;
    [SerializeField] private GameObject m_TotalMoneyPanel;
    [SerializeField] private GameObject m_ClassPanel;
    [SerializeField] private GameObject m_CheckPanel;
    [SerializeField] private Transform m_ClassPrefabParent;
    [SerializeField] private Transform m_ProfessorPrefabParent;
    [SerializeField] private Transform m_CheckClassPrefabParent;
    [SerializeField] private GameObject m_ClassPrefabParentObj;
    [SerializeField] private GameObject m_ProfessorPrefabParentObj;
    [SerializeField] private GameObject m_CheckClassPrefabParentObj;

    [SerializeField] private Button m_1WeekButton;
    [SerializeField] private Button m_2WeekButton;
    [SerializeField] private Button m_OpenClassButton;

    [SerializeField] private ClassController m_SelectClass;    // ���ϴ� ������ ���γ����� �����ֱ� ����
    [SerializeField] private ProfessorController m_SelectProfessor;

    [SerializeField] private PopUpUI m_PopUpCheckClassPanel;
    [SerializeField] private PopOffUI m_PopOffSelectPanel;

    [SerializeField] private TextMeshProUGUI m_ChangeWeekClassText;
    [SerializeField] private TextMeshProUGUI m_InGameUICurrentMoney;
    [SerializeField] private TextMeshProUGUI m_SelectPanelCurrentMoney;

    //private List<GameObject> m_ClassList = new List<GameObject>();
    //private List<Class> m_SelectClassDataList = new List<Class>();
    public List<string> m_SelectClassButtonName = new List<string>(); // ���� Ŭ���� ��ư�� ���������� ������ ������ �־��ֱ� ���� ���� ����Ʈ

    public GameObject m_1WeekPanel;
    public GameObject m_2WeekPanel;

    public SaveSelectClassInfoData m_SaveData = new SaveSelectClassInfoData();

    public List<SaveSelectClassInfoData> m_GameDesignerData = new List<SaveSelectClassInfoData>();
    public List<SaveSelectClassInfoData> m_ArtData = new List<SaveSelectClassInfoData>();
    public List<SaveSelectClassInfoData> m_ProgrammingData = new List<SaveSelectClassInfoData>();

    public EachClass m_NowClass = new EachClass();              // ���� ���� ������ �ִ� ������
    public Professor m_NowPlayerProfessor = new Professor();    // ���� ���� ����ϰ� �ִ� �����

    private ProfessorStat _ClickProfessorData;
    private Class _ClickClassData;

    private Color _currentButtonColor;
    private Color _preButtonColor;

    public PopOffUI m_HideSelectPanel;
    public PopOffUI m_HideCheckPanel;
    public PopUpUI m_OpenSelectPanel;
    public PopUpUI m_OpenCheckPanel;

    public int m_WeekClassIndex = 0;                    // 2�� �Ǹ� 1,2���� ���� ��� �����ѰŴ� �Ϸ�â���� �Ѱ��ֱ�
    public int m_WeekProfessorIndex = 0;                // 2�� �Ǹ� 1,2���� ���� ��� �����ѰŴ� �Ϸ�â���� �Ѱ��ֱ�
    public int m_SaveDataIndex = 0;                     // ������ �����͵��� �ε����� �ٲ��ֱ� ���� ����. 0�� 1�� ����Ѵ�.
    public int m_TotalMoney = 0;                        // SelectPanel���� ���� ������ �������� �� �����Ḧ ������ֱ� ���� ����

    private int m_ClassStack = 0;                       // 0~2������ 1���� ��ȹ,��Ʈ, �ù� 3~5������ 2���� ��ȹ,��Ʈ,�ù� ����, ���� ����
    private int[] m_StatNum = new int[3];
    private int[] m_AllInfoStatNum = new int[3];
    private string[] m_StatName = new string[3];
    private string m_Week;                              // ���� ������ ������ �� ���� ���� ������ �� ���� �������� ����ִ� ����
    private string m_ModifyProfessorString;             // ������ �� ������ �� ���� ��ư�� ������ �ش� �а��� ������ �����Ϸ��µ� ������ ���� �� �� �ϳ��� ������ �� ������ ������ ������ ���ܵα� ���� ����ó�� ����
    private string m_ModifyClassString;                 // ������ �� ������ �� ���� ��ư�� ������ �ش� �а��� ������ �����Ϸ��µ� ������ ���� �� �� �ϳ��� ������ �� ������ ������ ������ ���ܵα� ���� ����ó�� ����
    private int m_CurrentMoney = 0;                     // SelectPanel���� ���� ���� �����ϰ� �ִ� ��ȭ�� ����ֱ� ���� ����
    private int[] m_ClassMoney = new int[6];            // Ŭ���� �������� �����Ḧ �迭�� �����Ͽ� �迭�� ��ҵ��� ��� �����ִ� ����
    //private Stack<SaveSelectClassInfoData> m_UndoStack = new Stack<SaveSelectClassInfoData>();

    bool m_IsChangeWeekend = false;                     // 1,2 ���� ���������� ��� �ϰ� �ϷḦ ���� �� �ٽ� false�� �ٲ��ֱ�

    #region _����ü ������ ����Ʈ �ε����� �ٲٱ� ���� �Լ�
    public void ChangeListIndex(List<SaveSelectClassInfoData> _tempList, int _index, SaveSelectClassInfoData _saveData)
    {
        SaveSelectClassInfoData _temp = _tempList[_index];
        _temp = _saveData;
        _tempList[_index] = _temp;
    }
    #endregion

    private void Update()
    {
        int _currentMoney;

        if (int.TryParse(m_InGameUICurrentMoney.text, out _currentMoney))
        {
            m_CurrentMoney = _currentMoney;
        }
    }

    private void Start()
    {
        m_GameDesignerData.Capacity = 2;
        m_ArtData.Capacity = 2;
        m_ProgrammingData.Capacity = 2;

        // ���� ���� �� ������ üũ �κп��� 1,2���� ��ư�� ���� �� ������ �ٲ��ֱ� ���� ���� �̸� ����.
        _currentButtonColor = m_1WeekButton.GetComponent<Image>().color;
        _preButtonColor = m_2WeekButton.GetComponent<Image>().color;

        InitClass();
        InitProfessor();
    }

    // ���� ���� �г��� ������ ���� ����� �������� �� ����Ʈ�� �־��ش�.
    private void InitClass()
    {
        for (int i = 0; i < m_SelectClass.classData.Count; i++)
        {
            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Art)
            {
                m_NowClass.ArtClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.GameDesigner)
            {
                m_NowClass.GameDesignerClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.Programming)
            {
                m_NowClass.ProgrammingClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }

            if (m_SelectClass.classData.ElementAt(i).Value.m_ClassType == StudentType.None)
            {
                m_NowClass.NoneClass.Add(m_SelectClass.classData.ElementAt(i).Value);
            }
        }
    }

    private void InitProfessor()
    {
        for (int i = 0; i < m_SelectProfessor.professorData.Count; i++)
        {
            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Art)
            {
                m_NowPlayerProfessor.ArtProcessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.GameDesigner)
            {
                m_NowPlayerProfessor.GameManagerProcessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
            }

            if (m_SelectProfessor.professorData.ElementAt(i).Value.m_ProfessorType == StudentType.Programming)
            {
                m_NowPlayerProfessor.ProgrammingProfessor.Add(m_SelectProfessor.professorData.ElementAt(i).Value);
            }

        }
    }

    public void InitData()
    {
        m_GameDesignerData.Clear();
        m_ArtData.Clear();
        m_ProgrammingData.Clear();
        m_InGameUICurrentMoney.text = (int.Parse(m_InGameUICurrentMoney.text) - m_TotalMoney).ToString();
        for (int i = 0; i < 2; i++)
        {
            m_GameDesignerData.Add(new SaveSelectClassInfoData());
            m_ArtData.Add(new SaveSelectClassInfoData());
            m_ProgrammingData.Add(new SaveSelectClassInfoData());
        }
    }

    private void MakeCommonClass()
    {
        for (int i = 0; i < m_NowClass.NoneClass.Count; i++)
        {
            GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

            _classPrefab.name = m_NowClass.NoneClass[i].m_ClassName;
            _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

            _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "����";

            _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.NoneClass[i].m_ClassName;

            if (m_NowClass.NoneClass[i].m_Insight != 0 && m_NowClass.NoneClass[i].m_Concentration != 0 &&
                m_NowClass.NoneClass[i].m_Sense != 0 && m_NowClass.NoneClass[i].m_Technique != 0 &&
                m_NowClass.NoneClass[i].m_Wit != 0)
            {
                int _temp = m_NowClass.NoneClass[i].m_Insight + m_NowClass.NoneClass[i].m_Concentration + m_NowClass.NoneClass[i].m_Sense
                        + m_NowClass.NoneClass[i].m_Technique + m_NowClass.NoneClass[i].m_Wit;

                _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
            }
            else
            {
                FindNoneStat(i);

                for (int j = 1; j < m_StatNum.Length + 1; j++)
                {
                    int _tempNum = j - 1;

                    if (m_StatNum[_tempNum] < 0)
                    {
                        string _splitString = m_StatNum[_tempNum].ToString();

                        _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                        _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                    }
                    else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                    {
                        continue;
                    }
                    else
                    {
                        _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                        _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                    }
                }
            }
            _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.NoneClass[i].m_Money.ToString();
            _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.NoneClass[i].m_Health.ToString();
        }
    }

    // ���� ���� �г��� ������� 1���� ��ȹ ��Ʈ �ù�, 2���� ��ȹ ��Ʈ �ù����� ���´�. �� �Լ��� ó�� ȭ���� ���� �Լ��̴�
    public void MakeClass()
    {
        m_SelectPanelCurrentMoney.text = m_CurrentMoney.ToString();
        MakeCommonClass();
        for (int i = 0; i < m_NowClass.GameDesignerClass.Count; i++)
        {
            GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

            _classPrefab.name = m_NowClass.GameDesignerClass[i].m_ClassName;
            _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

            // �� �κ��� ����� �а� Ư���� �����ؼ� �ٲ��ֱ�

            if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Class)
            {
                _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "��ȹ";
            }
            else if (m_NowClass.GameDesignerClass[i].m_ClassStatType == ClassType.Special &&
                m_NowClass.GameDesignerClass[i].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth)
            {
                _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Ư��";
            }
            else
            {
                Destroy(_classPrefab);
                continue;
            }

            _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_ClassName;

            if (m_NowClass.GameDesignerClass[i].m_Insight != 0 && m_NowClass.GameDesignerClass[i].m_Concentration != 0 &&
                m_NowClass.GameDesignerClass[i].m_Sense != 0 && m_NowClass.GameDesignerClass[i].m_Technique != 0 &&
                m_NowClass.GameDesignerClass[i].m_Wit != 0)
            {
                int _temp = m_NowClass.GameDesignerClass[i].m_Insight + m_NowClass.GameDesignerClass[i].m_Concentration + m_NowClass.GameDesignerClass[i].m_Sense
                        + m_NowClass.GameDesignerClass[i].m_Technique + m_NowClass.GameDesignerClass[i].m_Wit;

                _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
            }
            else
            {
                FindGMStat(i);

                for (int j = 1; j < m_StatNum.Length + 1; j++)
                {
                    int _tempNum = j - 1;

                    if (m_StatNum[_tempNum] < 0)
                    {
                        string _splitString = m_StatNum[_tempNum].ToString();

                        _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                        _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                    }
                    else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                    {
                        continue;
                    }
                    else
                    {
                        _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                        _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                        _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                    }
                }
            }
            _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_Money.ToString();
            _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_Health.ToString();
        }
    }

    public void MakeProfessor()
    {
        for (int i = 0; i < m_NowPlayerProfessor.GameManagerProcessor.Count; i++)
        {
            GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

            _professorPrefab.name = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorNameValue;
            _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

            _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorNameValue;
            _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSet;
            _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorHealth.ToString();
            _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorPassion.ToString();

            if (m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills != null)
            {
                for (int j = 0; j < 3;)
                {
                    if (m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills[j] != "")
                    {
                        _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                        _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills[j];
                        j++;
                    }
                    else
                    {
                        j = 3;
                    }
                }
            }
        }
    }

    public void MakeOtherClass()
    {

        // ��ȹ���� ���õư� ��Ʈ���� ������ �ȵ��� �� 
        if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            MakeCommonClass();
            m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��Ʈ";

            for (int i = 0; i < m_NowClass.ArtClass.Count; i++)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

                _classPrefab.name = m_NowClass.ArtClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Class)
                {
                    _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "��Ʈ";
                }

                else if (m_NowClass.ArtClass[i].m_ClassStatType == ClassType.Special &&
                        m_NowClass.ArtClass[i].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth)
                {
                    _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Ư��";
                }
                else
                {
                    Destroy(_classPrefab);
                    continue;
                }

                _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_ClassName;

                if (m_NowClass.ArtClass[i].m_Insight != 0 && m_NowClass.ArtClass[i].m_Concentration != 0 &&
                    m_NowClass.ArtClass[i].m_Sense != 0 && m_NowClass.ArtClass[i].m_Technique != 0 &&
                    m_NowClass.ArtClass[i].m_Wit != 0)
                {
                    int _temp = m_NowClass.ArtClass[i].m_Insight + m_NowClass.ArtClass[i].m_Concentration + m_NowClass.ArtClass[i].m_Sense
                            + m_NowClass.ArtClass[i].m_Technique + m_NowClass.ArtClass[i].m_Wit;

                    _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                    _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
                }
                else
                {
                    FindArtStat(i);

                    for (int j = 1; j < m_StatNum.Length + 1; j++)
                    {
                        int _tempNum = j - 1;

                        if (m_StatNum[_tempNum] < 0)
                        {
                            string _splitString = m_StatNum[_tempNum].ToString();

                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                        }
                        else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                        {
                            continue;
                        }
                        else
                        {
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                        }
                    }
                }
                _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_Money.ToString();
                _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_Health.ToString();
            }
        }
        // ��ȹ�ݰ� ��Ʈ�� ��� ���õƴµ� �ù��� ������ �ȵ��� ��
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null && m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave == null)
        {
            MakeCommonClass();

            m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "�ù�";

            for (int i = 0; i < m_NowClass.ProgrammingClass.Count; i++)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

                _classPrefab.name = m_NowClass.ProgrammingClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Class)
                {
                    _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "�ù�";
                }
                else if (m_NowClass.ProgrammingClass[i].m_ClassStatType == ClassType.Special &&
                        m_NowClass.ProgrammingClass[i].m_OpenMonth == GameTime.Instance.FlowTime.NowMonth)
                {
                    _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Ư��";

                }
                else
                {
                    Destroy(_classPrefab);
                    continue;
                }

                _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_ClassName;

                if (m_NowClass.ProgrammingClass[i].m_Insight != 0 && m_NowClass.ProgrammingClass[i].m_Concentration != 0 &&
                    m_NowClass.ProgrammingClass[i].m_Sense != 0 && m_NowClass.ProgrammingClass[i].m_Technique != 0 &&
                    m_NowClass.ProgrammingClass[i].m_Wit != 0)
                {
                    int _temp = m_NowClass.ProgrammingClass[i].m_Insight + m_NowClass.ProgrammingClass[i].m_Concentration + m_NowClass.ProgrammingClass[i].m_Sense
                            + m_NowClass.ProgrammingClass[i].m_Technique + m_NowClass.ProgrammingClass[i].m_Wit;

                    _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                    _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
                }
                else
                {
                    FindProgrammingStat(i);

                    for (int j = 1; j < m_StatNum.Length + 1; j++)
                    {
                        int _tempNum = j - 1;

                        if (m_StatNum[_tempNum] < 0)
                        {
                            string _splitString = m_StatNum[_tempNum].ToString();
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                        }
                        else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                        {
                            continue;
                        }
                        else
                        {
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                        }
                    }
                }
                _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_Money.ToString();
                _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_Health.ToString();
            }
        }
        // ��ȹ ��Ʈ �ù� ��� �������� ��
        else if (m_GameDesignerData[m_WeekClassIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekClassIndex].m_SelectClassDataSave != null && m_ProgrammingData[m_WeekClassIndex].m_SelectClassDataSave != null)
        {
            m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��ȹ";

            MakeClass();
            m_WeekClassIndex++;
        }
    }

    public void MakeOtherProfessor()
    {
        if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave == null)
        {
            for (int i = 0; i < m_NowPlayerProfessor.ArtProcessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorNameValue;
                _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSet;
                _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorPassion.ToString();

                if (m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills != null)
                {
                    for (int j = 0; j < 3;)
                    {
                        if (m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills[j] != "")
                        {
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills[j];
                            j++;
                        }
                        else
                        {
                            j = 3;
                        }
                    }
                }
            }
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null && m_ProgrammingData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            MakeProfessor();
            m_WeekProfessorIndex++;
        }
        else if (m_GameDesignerData[m_WeekProfessorIndex].m_SelectClassDataSave != null && m_ArtData[m_WeekProfessorIndex].m_SelectClassDataSave != null)
        {
            for (int i = 0; i < m_NowPlayerProfessor.ProgrammingProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSet;
                _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorPassion.ToString();

                if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills != null)
                {
                    for (int j = 0; j < 3;)
                    {
                        if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[j] != "")
                        {
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[j];
                            j++;
                        }
                        else
                        {
                            j = 3;
                        }
                    }
                }
            }
        }
    }

    // �����ϱ� ��ư�� ������ �� �ش��ϴ� �г��� ������ ��������ֱ� ���� �Լ�
    private void SetStatInfo(GameObject _obj, List<SaveSelectClassInfoData> _tempList, int _index)
    {
        // CompleteClassPrefab�� ���� �̸��� ���� �̸� ���� �κ�.
        _obj.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectClassDataSave.m_ClassName;
        _obj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectProfessorDataSave.m_ProfessorNameValue;

        if (_tempList[_index].m_SelectClassDataSave.m_Insight != 0 && _tempList[_index].m_SelectClassDataSave.m_Concentration != 0 &&
            _tempList[_index].m_SelectClassDataSave.m_Sense != 0 && _tempList[_index].m_SelectClassDataSave.m_Technique != 0 &&
            _tempList[_index].m_SelectClassDataSave.m_Wit != 0)
        {
            int _temp = _tempList[_index].m_SelectClassDataSave.m_Insight + _tempList[_index].m_SelectClassDataSave.m_Concentration + _tempList[_index].m_SelectClassDataSave.m_Sense
                    + _tempList[_index].m_SelectClassDataSave.m_Technique + _tempList[_index].m_SelectClassDataSave.m_Wit;

            _obj.transform.GetChild(3).gameObject.SetActive(true);
            _obj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
            _obj.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
            _obj.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (_temp + _tempList[_index].m_SelectProfessorDataSave.professorPower).ToString();
        }
        else
        {
            int _tempindex = SelectedClassDataStatIndex(_tempList, _index);

            for (int j = 0; j < _tempindex;)
            {
                if (_tempList[_index].m_SelectClassDataSave.m_Insight > 0 || _tempList[_index].m_SelectClassDataSave.m_Insight < 0)
                {
                    m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Insight;
                    m_StatName[j] = "����";
                    j++;
                }

                if (_tempList[_index].m_SelectClassDataSave.m_Concentration > 0 || _tempList[_index].m_SelectClassDataSave.m_Concentration < 0)
                {
                    m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Concentration;
                    m_StatName[j] = "����";
                    j++;
                }

                if (_tempList[_index].m_SelectClassDataSave.m_Sense > 0 || _tempList[_index].m_SelectClassDataSave.m_Sense < 0)
                {
                    m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Sense;
                    m_StatName[j] = "����";
                    j++;
                }

                if (_tempList[_index].m_SelectClassDataSave.m_Technique > 0 || _tempList[_index].m_SelectClassDataSave.m_Technique < 0)
                {
                    m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Technique;
                    m_StatName[j] = "���";
                    j++;
                }

                if (_tempList[_index].m_SelectClassDataSave.m_Wit > 0 || _tempList[_index].m_SelectClassDataSave.m_Wit < 0)
                {
                    m_StatNum[j] = _tempList[_index].m_SelectClassDataSave.m_Wit;
                    m_StatName[j] = "��ġ";
                    j++;
                }
            }

            for (int j = 0; j < m_StatNum.Length; j++)
            {
                int _tempIndex = j + 3;

                // �����ϴ� ������ ������ ���� ������ �ʿ䰡 ����.
                if (m_StatNum[j] < 0)
                {
                    string _splitString = m_StatNum[j].ToString();

                    _obj.transform.GetChild(_tempIndex).gameObject.SetActive(true);
                    _obj.transform.GetChild(_tempIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[j];

                    // Ȥ�� �� ���� �ѵ� �÷����� �������� �� ������ ������Ѵ�.
                    if (_obj.transform.GetChild(_tempIndex).GetChild(2).gameObject.activeSelf == true)
                    {
                        _obj.transform.GetChild(_tempIndex).GetChild(2).gameObject.SetActive(false);
                    }

                    _obj.transform.GetChild(_tempIndex).GetChild(1).gameObject.SetActive(true);
                    _obj.transform.GetChild(_tempIndex).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                }
                else if (m_StatNum[j] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                {
                    continue;
                }
                else
                {
                    _obj.transform.GetChild(_tempIndex).gameObject.SetActive(true);
                    _obj.transform.GetChild(_tempIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[j];

                    if (_obj.transform.GetChild(_tempIndex).GetChild(1).gameObject.activeSelf == true)
                    {
                        _obj.transform.GetChild(_tempIndex).GetChild(1).gameObject.SetActive(false);
                    }
                    _obj.transform.GetChild(_tempIndex).GetChild(2).gameObject.SetActive(true);
                    _obj.transform.GetChild(_tempIndex).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (m_StatNum[j] + _tempList[_index].m_SelectProfessorDataSave.professorPower).ToString();
                }
            }
        }

        _obj.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectClassDataSave.m_Money.ToString();
        _obj.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = _tempList[_index].m_SelectClassDataSave.m_Health.ToString();
    }

    private int SelectedClassDataStatIndex(List<SaveSelectClassInfoData> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_SelectClassDataSave.m_Insight != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_SelectClassDataSave.m_Concentration != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_SelectClassDataSave.m_Sense != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_SelectClassDataSave.m_Technique != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_SelectClassDataSave.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int AllInfoDataStatIndex(SaveSelectClassInfoData _saveData)
    {
        int _index = 0;

        if (_saveData.m_SelectClassDataSave.m_Insight != 0)
        {
            _index++;
        }
        if (_saveData.m_SelectClassDataSave.m_Concentration != 0)
        {
            _index++;
        }
        if (_saveData.m_SelectClassDataSave.m_Sense != 0)
        {
            _index++;
        }
        if (_saveData.m_SelectClassDataSave.m_Technique != 0)
        {
            _index++;
        }
        if (_saveData.m_SelectClassDataSave.m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    private int UnSelectedClassDataStatIndex(List<Class> _tempList, int _listIndex)
    {
        int _index = 0;

        if (_tempList[_listIndex].m_Insight != 0)
        {
            _index++;
        }

        if (_tempList[_listIndex].m_Concentration != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_Sense != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_Technique != 0)
        {
            _index++;
        }
        if (_tempList[_listIndex].m_Wit != 0)
        {
            _index++;
        }

        return _index;
    }

    // 1�ִ� 0, 2�ִ� 1 �̰� ��ȹ���� 1, ��Ʈ���� 2, �ùֹ��� 3�̴�.
    private void MakeTotalInfo(List<SaveSelectClassInfoData> _tempList, int _weekIndex, int _partIndex)
    {
        m_CheckClassPanel.transform.GetChild(5).GetChild(0).GetChild(_weekIndex).GetChild(_partIndex).GetChild(1).GetComponent<TextMeshProUGUI>().text = _tempList[_weekIndex].m_SelectClassDataSave.m_ClassName;
        m_CheckClassPanel.transform.GetChild(5).GetChild(0).GetChild(_weekIndex).GetChild(_partIndex).GetChild(2).GetComponent<TextMeshProUGUI>().text = _tempList[_weekIndex].m_SelectClassDataSave.m_Money.ToString();
    }

    private int CalculateTotalMoneyCheckPanel()
    {
        int _totalMoney;

        _totalMoney = m_ArtData[0].m_SelectClassDataSave.m_Money + m_ArtData[1].m_SelectClassDataSave.m_Money + m_ProgrammingData[0].m_SelectClassDataSave.m_Money + m_ProgrammingData[1].m_SelectClassDataSave.m_Money
            + m_GameDesignerData[0].m_SelectClassDataSave.m_Money + m_GameDesignerData[1].m_SelectClassDataSave.m_Money;

        return _totalMoney;
    }

    private void ModifyButton()
    {
        m_OpenSelectPanel.TurnOnUI();
        //m_SelectClassPanel.SetActive(true);
        // �����Ϸ��ư Ȱ��ȭ
        m_SelectClassPanel.transform.GetChild(5).gameObject.SetActive(true);
        //m_CheckClassPanel.SetActive(false);
        m_HideCheckPanel.BackButton();
        if (m_ClassPrefabParent.childCount > 0)
        {
            DestroyObject(m_ClassPrefabParentObj);
        }

        if (m_ProfessorPrefabParent.childCount > 0)
        {
            DestroyObject(m_ProfessorPrefabParentObj);
        }

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        GameObject _parentObj = _currentObj.transform.parent.gameObject;

        // ���� ��ư�� ������ �� m_SaveData�� ������� �� ������ ���� Ŭ���� ������Ʈ�� ������ �ٽ� ä���ֱ�
        Class _tempData;

        m_ModifyClassString = _parentObj.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;

        m_SelectClass.classData.TryGetValue(m_ModifyClassString, out _tempData);

        ProfessorStat _tempProfessor;

        m_ModifyProfessorString = _parentObj.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;

        m_SelectProfessor.professorData.TryGetValue(m_ModifyProfessorString, out _tempProfessor);

        if (m_SaveData.m_SelectClassDataSave == null || m_SaveData.m_SelectClassDataSave != _tempData)
        {
            m_SaveData.m_SelectClassDataSave = _tempData;
        }

        if (m_SaveData.m_SelectProfessorDataSave == null || m_SaveData.m_SelectProfessorDataSave != _tempProfessor)
        {

            m_SaveData.m_SelectProfessorDataSave = _tempProfessor;
        }

        if (m_Week == "1����")
        {
            m_ChangeWeekClassText.text = "1����";
            m_SaveDataIndex = 0;
            ResetSelectPanel(_tempData, _tempProfessor);
        }
        else
        {
            m_ChangeWeekClassText.text = "2����";
            m_SaveDataIndex = 1;
            ResetSelectPanel(_tempData, _tempProfessor);
        }

    }

    private void ResetSelectPanel(Class _tempClass, ProfessorStat _tempProfessor)
    {
        if (_tempClass.m_ClassType == StudentType.Art)
        {
            m_SelectClassPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��Ʈ";
            for (int i = 0; i < m_NowClass.ArtClass.Count; i++)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

                _classPrefab.name = m_NowClass.ArtClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "��Ʈ";
                _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_ClassName;

                if (m_NowClass.ArtClass[i].m_Insight != 0 && m_NowClass.ArtClass[i].m_Concentration != 0 &&
                    m_NowClass.ArtClass[i].m_Sense != 0 && m_NowClass.ArtClass[i].m_Technique != 0 &&
                    m_NowClass.ArtClass[i].m_Wit != 0)
                {
                    int _temp = m_NowClass.ArtClass[i].m_Insight + m_NowClass.ArtClass[i].m_Concentration + m_NowClass.ArtClass[i].m_Sense
                            + m_NowClass.ArtClass[i].m_Technique + m_NowClass.ArtClass[i].m_Wit;

                    _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                    _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
                }
                else
                {
                    FindArtStat(i);

                    for (int j = 1; j < m_StatNum.Length + 1; j++)
                    {
                        int _tempNum = j - 1;

                        if (m_StatNum[_tempNum] < 0)
                        {
                            string _splitString = m_StatNum[_tempNum].ToString();

                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                        }
                        else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                        {
                            continue;
                        }
                        else
                        {
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                        }
                    }
                }
                _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_Money.ToString();
                _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ArtClass[i].m_Health.ToString();
            }

            for (int i = 0; i < m_NowPlayerProfessor.ArtProcessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorNameValue;
                _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSet;
                _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorPassion.ToString();

                if (m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills != null)
                {
                    for (int j = 0; j < 3;)
                    {
                        if (m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills[j] != "")
                        {
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ArtProcessor[i].m_ProfessorSkills[j];
                            j++;
                        }
                        else
                        {
                            j = 3;
                        }
                    }
                }
            }

            if (m_SaveDataIndex == 0)
            {
                SetStatInfo(m_AllInfoPanel, m_ArtData, 0);
            }
            else
            {
                SetStatInfo(m_AllInfoPanel, m_ArtData, 1);
            }

        }
        else if (_tempClass.m_ClassType == StudentType.GameDesigner)
        {
            m_SelectClassPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��ȹ";

            for (int i = 0; i < m_NowClass.GameDesignerClass.Count; i++)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

                _classPrefab.name = m_NowClass.GameDesignerClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "��ȹ";
                _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_ClassName;

                if (m_NowClass.GameDesignerClass[i].m_Insight != 0 && m_NowClass.GameDesignerClass[i].m_Concentration != 0 &&
                    m_NowClass.GameDesignerClass[i].m_Sense != 0 && m_NowClass.GameDesignerClass[i].m_Technique != 0 &&
                    m_NowClass.GameDesignerClass[i].m_Wit != 0)
                {
                    int _temp = m_NowClass.GameDesignerClass[i].m_Insight + m_NowClass.GameDesignerClass[i].m_Concentration + m_NowClass.GameDesignerClass[i].m_Sense
                            + m_NowClass.GameDesignerClass[i].m_Technique + m_NowClass.GameDesignerClass[i].m_Wit;

                    _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                    _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
                }
                else
                {
                    FindGMStat(i);

                    for (int j = 1; j < m_StatNum.Length + 1; j++)
                    {
                        int _tempNum = j - 1;

                        if (m_StatNum[_tempNum] < 0)
                        {
                            string _splitString = m_StatNum[_tempNum].ToString();

                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                        }
                        else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                        {
                            continue;
                        }
                        else
                        {
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                        }
                    }
                }
                _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_Money.ToString();
                _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.GameDesignerClass[i].m_Health.ToString();
            }

            for (int i = 0; i < m_NowPlayerProfessor.GameManagerProcessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorNameValue;
                _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSet;
                _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorPassion.ToString();

                if (m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills != null)
                {
                    for (int j = 0; j < 3;)
                    {
                        if (m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills[j] != "")
                        {
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.GameManagerProcessor[i].m_ProfessorSkills[j];
                            j++;
                        }
                        else
                        {
                            j = 3;
                        }
                    }
                }
            }

            if (m_SaveDataIndex == 0)
            {
                SetStatInfo(m_AllInfoPanel, m_GameDesignerData, 0);
            }
            else
            {
                SetStatInfo(m_AllInfoPanel, m_GameDesignerData, 1);
            }
        }
        else if (_tempClass.m_ClassType == StudentType.Programming)
        {
            m_SelectClassPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "�ù�";

            for (int i = 0; i < m_NowClass.ProgrammingClass.Count; i++)
            {
                GameObject _classPrefab = GameObject.Instantiate(m_ClassPrefab, m_ClassPrefabParent);

                _classPrefab.name = m_NowClass.ProgrammingClass[i].m_ClassName;
                _classPrefab.GetComponent<Button>().onClick.AddListener(ClickClass);

                _classPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "��ȹ";
                _classPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_ClassName;

                if (m_NowClass.ProgrammingClass[i].m_Insight != 0 && m_NowClass.ProgrammingClass[i].m_Concentration != 0 &&
                    m_NowClass.ProgrammingClass[i].m_Sense != 0 && m_NowClass.ProgrammingClass[i].m_Technique != 0 &&
                    m_NowClass.ProgrammingClass[i].m_Wit != 0)
                {
                    int _temp = m_NowClass.ProgrammingClass[i].m_Insight + m_NowClass.ProgrammingClass[i].m_Concentration + m_NowClass.ProgrammingClass[i].m_Sense
                            + m_NowClass.ProgrammingClass[i].m_Technique + m_NowClass.ProgrammingClass[i].m_Wit;

                    _classPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "��罺��";
                    _classPrefab.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                    _classPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();
                }
                else
                {
                    FindProgrammingStat(i);

                    for (int j = 1; j < m_StatNum.Length + 1; j++)
                    {
                        int _tempNum = j - 1;

                        if (m_StatNum[_tempNum] < 0)
                        {
                            string _splitString = m_StatNum[_tempNum].ToString();

                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(1).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _splitString.Substring(1);
                        }
                        else if (m_StatNum[_tempNum] == 0)   // 0�̸� �׳� �Ѱ��ֱ�
                        {
                            continue;
                        }
                        else
                        {
                            _classPrefab.transform.GetChild(j).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatName[_tempNum];

                            _classPrefab.transform.GetChild(j).GetChild(2).gameObject.SetActive(true);
                            _classPrefab.transform.GetChild(j).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_StatNum[_tempNum].ToString();
                        }
                    }
                }
                _classPrefab.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_Money.ToString();
                _classPrefab.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowClass.ProgrammingClass[i].m_Health.ToString();
            }

            for (int i = 0; i < m_NowPlayerProfessor.ProgrammingProfessor.Count; i++)
            {
                GameObject _professorPrefab = Instantiate(m_ProfessorPrefab, m_ProfessorPrefabParent);

                _professorPrefab.name = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                _professorPrefab.GetComponent<Button>().onClick.AddListener(ClickProfessor);

                _professorPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorNameValue;
                _professorPrefab.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSet;
                _professorPrefab.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorHealth.ToString();
                _professorPrefab.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorPassion.ToString();

                if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills != null)
                {
                    for (int j = 0; j < 3;)
                    {
                        if (m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[j] != "")
                        {
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(1).gameObject.SetActive(false);
                            _professorPrefab.transform.GetChild(2).GetChild(j + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_NowPlayerProfessor.ProgrammingProfessor[i].m_ProfessorSkills[j];
                            j++;
                        }
                        else
                        {
                            j = 3;
                        }
                    }
                }
            }

            if (m_SaveDataIndex == 0)
            {
                SetStatInfo(m_AllInfoPanel, m_ProgrammingData, 0);
            }
            else
            {
                SetStatInfo(m_AllInfoPanel, m_ProgrammingData, 1);
            }
        }

    }

    public void ModifyCompleteButton()
    {
        m_SelectClassPanel.transform.GetChild(5).gameObject.SetActive(false);

        if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
        {
            m_ArtData.Add(m_SaveData);
            ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
            m_ArtData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
        {
            m_GameDesignerData.Add(m_SaveData);
            ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
            m_GameDesignerData.RemoveAt(2);
        }
        else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
        {
            m_ProgrammingData.Add(m_SaveData);
            ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
            m_ProgrammingData.RemoveAt(2);
        }

        // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
        if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
            && m_GameDesignerData[1].m_SelectClassDataSave != null)
        {
            m_HideSelectPanel.BackButton();
            //m_SelectClassPanel.SetActive(false);
            m_OpenCheckPanel.TurnOnUI();
            //m_CheckClassPanel.SetActive(true);
            //m_PopOffSelectPanel.TurnOffUI();
            //m_PopUpCheckClassPanel.PopUpMyUI();

            m_CheckClassPanel.transform.GetChild(5).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = CalculateTotalMoneyCheckPanel().ToString();

            m_CheckClassPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(MakeCheckClass);
            m_CheckClassPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(MakeCheckClass);

            MakeCheckClass();
        }
    }

    public void MakeCheckClass()
    {
        GameObject _currentButton = EventSystem.current.currentSelectedGameObject;
        m_CheckPanel.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = m_CurrentMoney.ToString();


        if (_currentButton.name != "2WeekButton")
        {
            _currentButton = m_1WeekButton.gameObject;
            m_Week = "1����";

            m_1WeekButton.GetComponent<Image>().color = _currentButtonColor;
            m_2WeekButton.GetComponent<Image>().color = _preButtonColor;
        }
        else
        {
            m_Week = "2����";
            m_2WeekButton.GetComponent<Image>().color = _currentButtonColor;
            m_1WeekButton.GetComponent<Image>().color = _preButtonColor;
        }

        if (m_CheckClassPrefabParent.childCount > 0)
        {
            DestroyObject(m_CheckClassPrefabParentObj);
        }

        for (int i = 0; i < 3; i++)
        {
            m_StatName[i] = "";
            m_StatNum[i] = 0;
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject _checkClassPrefab = Instantiate(m_CheckClassPrefab, m_CheckClassPrefabParent);

            _checkClassPrefab.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(ModifyButton);

            switch (i)
            {
                case 0:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatInfo(_checkClassPrefab, m_GameDesignerData, 0);
                        MakeTotalInfo(m_GameDesignerData, 0, 1);
                        MakeTotalInfo(m_GameDesignerData, 1, 1);
                    }
                    else
                    {
                        SetStatInfo(_checkClassPrefab, m_GameDesignerData, 1);
                        MakeTotalInfo(m_GameDesignerData, 0, 1);
                        MakeTotalInfo(m_GameDesignerData, 1, 1);
                    }
                }
                break;

                case 1:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatInfo(_checkClassPrefab, m_ArtData, 0);
                        MakeTotalInfo(m_ArtData, 0, 2);
                        MakeTotalInfo(m_ArtData, 1, 2);
                    }
                    else
                    {
                        SetStatInfo(_checkClassPrefab, m_ArtData, 1);
                        MakeTotalInfo(m_ArtData, 0, 2);
                        MakeTotalInfo(m_ArtData, 1, 2);
                    }
                }
                break;

                case 2:
                {
                    if (_currentButton.name == "1WeekButton")
                    {
                        SetStatInfo(_checkClassPrefab, m_ProgrammingData, 0);
                        MakeTotalInfo(m_ProgrammingData, 0, 3);
                        MakeTotalInfo(m_ProgrammingData, 1, 3);
                    }
                    else
                    {
                        SetStatInfo(_checkClassPrefab, m_ProgrammingData, 1);
                        MakeTotalInfo(m_ProgrammingData, 0, 3);
                        MakeTotalInfo(m_ProgrammingData, 1, 3);
                    }
                }
                break;
            }
        }
    }

    public void ClickCompleteButton()
    {
        // ������� ������ȭ������ ������ ü�� ����, ���� ������ �� �����ᰡ ���� ��ȭ�� �Ѿ�� �� �ߴ� ����̴�.
        if (m_AllInfoPanel.transform.GetChild(8).gameObject.activeSelf == false &&
            m_AllInfoPanel.transform.GetChild(9).gameObject.activeSelf == false &&
            m_AllInfoPanel.transform.GetChild(10).gameObject.activeSelf == false)
        {
            if (m_SaveData.m_SelectClassDataSave != null && m_SaveData.m_SelectProfessorDataSave != null)
            {
                if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Art)
                {
                    m_ArtData.Add(m_SaveData);
                    ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                    m_ArtData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.GameDesigner)
                {
                    m_GameDesignerData.Add(m_SaveData);
                    ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                    m_GameDesignerData.RemoveAt(2);
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.Programming)
                {
                    m_ProgrammingData.Add(m_SaveData);
                    ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                    m_ProgrammingData.RemoveAt(2);

                    // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
                    if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
                        && m_GameDesignerData[1].m_SelectClassDataSave != null)
                    {
                        m_HideSelectPanel.BackButton();
                        m_OpenCheckPanel.TurnOnUI();
                        //m_SelectClassPanel.SetActive(false);
                        //m_CheckClassPanel.SetActive(true);

                        m_CheckClassPanel.transform.GetChild(5).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = CalculateTotalMoneyCheckPanel().ToString();

                        m_CheckClassPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(MakeCheckClass);
                        m_CheckClassPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(MakeCheckClass);

                        MakeCheckClass();
                    }
                }
                else if (m_SaveData.m_SelectClassDataSave.m_ClassType == StudentType.None)
                {
                    if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��ȹ")
                    {
                        m_GameDesignerData.Add(m_SaveData);
                        ChangeListIndex(m_GameDesignerData, m_SaveDataIndex, m_SaveData);
                        m_GameDesignerData.RemoveAt(2);
                    }
                    else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��Ʈ")
                    {
                        m_ArtData.Add(m_SaveData);
                        ChangeListIndex(m_ArtData, m_SaveDataIndex, m_SaveData);
                        m_ArtData.RemoveAt(2);
                    }
                    else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "�ù�")
                    {
                        m_ProgrammingData.Add(m_SaveData);
                        ChangeListIndex(m_ProgrammingData, m_SaveDataIndex, m_SaveData);
                        m_ProgrammingData.RemoveAt(2);

                        // ��� �а��� ������ ���������� ���� ������ �������� �ѹ��� ��� �� UI�� ������Ѵ�.
                        if (m_ArtData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null
                            && m_GameDesignerData[1].m_SelectClassDataSave != null)
                        {
                            m_HideSelectPanel.BackButton();
                            m_OpenCheckPanel.TurnOnUI();
                            //m_SelectClassPanel.SetActive(false);
                            //m_CheckClassPanel.SetActive(true);

                            m_CheckClassPanel.transform.GetChild(5).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = CalculateTotalMoneyCheckPanel().ToString();

                            m_CheckClassPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(MakeCheckClass);
                            m_CheckClassPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(MakeCheckClass);

                            MakeCheckClass();
                        }
                    }
                }

                // ���� �а��� �Ѿ�ϱ� ������ �����͸� ��Ƶ� ���� ����ش�.
                for (int i = 0; i < 3; i++)
                {
                    m_StatName[i] = "";
                    m_StatNum[i] = 0;
                    m_AllInfoStatNum[i] = 0;
                }

                DestroyObject(m_ClassPrefabParentObj);
                DestroyObject(m_ProfessorPrefabParentObj);

                InitAllInfoStat();

                MakeOtherClass();
                MakeOtherProfessor();

                if (m_WeekClassIndex == 1 && m_WeekProfessorIndex == 1 && m_IsChangeWeekend == false)
                {
                    m_IsChangeWeekend = true;
                    m_ChangeWeekClassText.text = "2����";
                    m_2WeekPanel.SetActive(true);
                    m_SaveDataIndex++;
                }

                m_SaveData.m_SelectClassDataSave = null;
                m_SaveData.m_SelectProfessorDataSave = null;
                _ClickProfessorData = null;
                _ClickClassData = null;
            }
            // ���� ������ �Ϸ� ������ ������Ű��
            m_ClassStack++;
        }
    }

    // ������ ���� �� ���� ����â�� ����ֱ� ���� �Լ�
    public void InitSelecteClass()
    {
        // ���� �а��� �Ѿ�ϱ� ������ �����͸� ��Ƶ� ���� ����ش�.
        for (int i = 0; i < 3; i++)
        {
            m_StatName[i] = "";
            m_StatNum[i] = 0;
            m_AllInfoStatNum[i] = 0;
        }

        DestroyObject(m_ClassPrefabParentObj);
        DestroyObject(m_ProfessorPrefabParentObj);

        InitAllInfoStat();

        InitTotalInfo(m_1WeekPanel);
        InitTotalInfo(m_2WeekPanel);

        //MakeOtherClass();
        //MakeOtherProfessor();
        m_ChangeWeekClassText.text = "1����";

        m_2WeekPanel.SetActive(false);
        m_IsChangeWeekend = false;

        for (int i = 0; i < 6; i++)
        {
            m_ClassMoney[i] = 0;
        }

        m_SelectClassPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";

        m_SaveData.m_SelectClassDataSave = null;
        m_SaveData.m_SelectProfessorDataSave = null;
        _ClickProfessorData = null;
        _ClickClassData = null;

        m_WeekClassIndex = 0;
        m_WeekProfessorIndex = 0;
        m_SaveDataIndex = 0;

    }

    private void DestroyObject(GameObject _obj)
    {
        Transform[] _childCount = _obj.GetComponentsInChildren<Transform>();

        if (_childCount != null)
        {
            for (int i = 0; i < _childCount.Length; i++)
            {
                if (_childCount[i] != _obj.transform)
                {
                    Destroy(_childCount[i].gameObject);
                }
            }
        }
    }

    private void FindNoneStat(int _index)
    {
        for (int j = 0; j < 3;)
        {
            if (m_NowClass.NoneClass[_index].m_Insight > 0 || m_NowClass.NoneClass[_index].m_Insight < 0)
            {
                m_StatNum[j] = m_NowClass.NoneClass[_index].m_Insight;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.NoneClass[_index].m_Concentration > 0 || m_NowClass.NoneClass[_index].m_Concentration < 0)
            {
                m_StatNum[j] = m_NowClass.NoneClass[_index].m_Concentration;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.NoneClass[_index].m_Sense > 0 || m_NowClass.NoneClass[_index].m_Sense < 0)
            {
                m_StatNum[j] = m_NowClass.NoneClass[_index].m_Sense;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.NoneClass[_index].m_Technique > 0 || m_NowClass.NoneClass[_index].m_Technique < 0)
            {
                m_StatNum[j] = m_NowClass.NoneClass[_index].m_Technique;
                m_StatName[j] = "���";
                j++;
            }

            if (m_NowClass.NoneClass[_index].m_Wit > 0 || m_NowClass.NoneClass[_index].m_Wit < 0)
            {
                m_StatNum[j] = m_NowClass.NoneClass[_index].m_Wit;
                m_StatName[j] = "��ġ";
                j++;
            }
        }
    }

    // ���� ������ ������ �� ���ִ°� �ƴ϶�� 0�� �ƴ� 3���� ���ȸ� �������Ѵ�.
    private void FindGMStat(int _index) // , List<Class> _class�̰ɷ� �Ű����� �޾ƿ���
    {
        int _tempindex = UnSelectedClassDataStatIndex(m_NowClass.GameDesignerClass, _index);

        for (int j = 0; j < _tempindex;)
        {
            if (m_NowClass.GameDesignerClass[_index].m_Insight > 0 || m_NowClass.GameDesignerClass[_index].m_Insight < 0)
            {
                m_StatNum[j] = m_NowClass.GameDesignerClass[_index].m_Insight;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.GameDesignerClass[_index].m_Concentration > 0 || m_NowClass.GameDesignerClass[_index].m_Concentration < 0)
            {
                m_StatNum[j] = m_NowClass.GameDesignerClass[_index].m_Concentration;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.GameDesignerClass[_index].m_Sense > 0 || m_NowClass.GameDesignerClass[_index].m_Sense < 0)
            {
                m_StatNum[j] = m_NowClass.GameDesignerClass[_index].m_Sense;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.GameDesignerClass[_index].m_Technique > 0 || m_NowClass.GameDesignerClass[_index].m_Technique < 0)
            {
                m_StatNum[j] = m_NowClass.GameDesignerClass[_index].m_Technique;
                m_StatName[j] = "���";
                j++;
            }

            if (m_NowClass.GameDesignerClass[_index].m_Wit > 0 || m_NowClass.GameDesignerClass[_index].m_Wit < 0)
            {
                m_StatNum[j] = m_NowClass.GameDesignerClass[_index].m_Wit;
                m_StatName[j] = "��ġ";
                j++;
            }
        }
    }

    private void FindArtStat(int _index)
    {
        int _tempindex = UnSelectedClassDataStatIndex(m_NowClass.ArtClass, _index);

        for (int j = 0; j < _tempindex;)
        {
            if (m_NowClass.ArtClass[_index].m_Insight > 0 || m_NowClass.ArtClass[_index].m_Insight < 0)
            {
                m_StatNum[j] = m_NowClass.ArtClass[_index].m_Insight;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ArtClass[_index].m_Concentration > 0 || m_NowClass.ArtClass[_index].m_Concentration < 0)
            {
                m_StatNum[j] = m_NowClass.ArtClass[_index].m_Concentration;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ArtClass[_index].m_Sense > 0 || m_NowClass.ArtClass[_index].m_Sense < 0)
            {
                m_StatNum[j] = m_NowClass.ArtClass[_index].m_Sense;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ArtClass[_index].m_Technique > 0 || m_NowClass.ArtClass[_index].m_Technique < 0)
            {
                m_StatNum[j] = m_NowClass.ArtClass[_index].m_Technique;
                m_StatName[j] = "���";
                j++;
            }

            if (m_NowClass.ArtClass[_index].m_Wit > 0 || m_NowClass.ArtClass[_index].m_Wit < 0)
            {
                m_StatNum[j] = m_NowClass.ArtClass[_index].m_Wit;
                m_StatName[j] = "��ġ";
                j++;
            }
        }
    }

    private void FindProgrammingStat(int _index)
    {
        int _tempindex = UnSelectedClassDataStatIndex(m_NowClass.ProgrammingClass, _index);

        for (int j = 0; j < _tempindex;)
        {
            if (m_NowClass.ProgrammingClass[_index].m_Insight > 0 || m_NowClass.ProgrammingClass[_index].m_Insight < 0)
            {
                m_StatNum[j] = m_NowClass.ProgrammingClass[_index].m_Insight;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ProgrammingClass[_index].m_Concentration > 0 || m_NowClass.ProgrammingClass[_index].m_Concentration < 0)
            {
                m_StatNum[j] = m_NowClass.ProgrammingClass[_index].m_Concentration;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ProgrammingClass[_index].m_Sense > 0 || m_NowClass.ProgrammingClass[_index].m_Sense < 0)
            {
                m_StatNum[j] = m_NowClass.ProgrammingClass[_index].m_Sense;
                m_StatName[j] = "����";
                j++;
            }

            if (m_NowClass.ProgrammingClass[_index].m_Technique > 0 || m_NowClass.ProgrammingClass[_index].m_Technique < 0)
            {
                m_StatNum[j] = m_NowClass.ProgrammingClass[_index].m_Technique;
                m_StatName[j] = "���";
                j++;
            }

            if (m_NowClass.ProgrammingClass[_index].m_Wit > 0 || m_NowClass.ProgrammingClass[_index].m_Wit < 0)
            {
                m_StatNum[j] = m_NowClass.ProgrammingClass[_index].m_Wit;
                m_StatName[j] = "��ġ";
                j++;
            }
        }
    }

    // �̹� ������ ������ ���¿��� ������ ������ �� AllInfo�� ������ �ɷ�ġ�� ���� ������ ����ֱ� ���� �Լ�.
    private void FindStatAllInfo()
    {
        int _tempindex = AllInfoDataStatIndex(m_SaveData);

        for (int j = 0; j < _tempindex;)
        {
            if (m_SaveData.m_SelectClassDataSave.m_Insight > 0 || m_SaveData.m_SelectClassDataSave.m_Insight < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Insight;
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Concentration > 0 || m_SaveData.m_SelectClassDataSave.m_Concentration < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Concentration;
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Sense > 0 || m_SaveData.m_SelectClassDataSave.m_Sense < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Sense;
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Technique > 0 || m_SaveData.m_SelectClassDataSave.m_Technique < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Technique;
                j++;
            }

            if (m_SaveData.m_SelectClassDataSave.m_Wit > 0 || m_SaveData.m_SelectClassDataSave.m_Wit < 0)
            {
                m_AllInfoStatNum[j] = m_SaveData.m_SelectClassDataSave.m_Wit;
                j++;
            }
        }
    }

    // ������ ������ �� AllInfo�� ������ ������ ������ �Ȱ��� ����ֱ� ���� �Լ�
    private void SetStat(GameObject _gameObject)
    {
        if (_gameObject.transform.GetChild(1).gameObject.activeSelf == true)
        {
            // ù���� ���� �κ�
            m_AllInfoPanel.transform.GetChild(3).gameObject.SetActive(true);
            m_AllInfoPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;

            // ���� Ŭ���� ������ ������ ���� �����̸�
            if (_gameObject.transform.GetChild(1).GetChild(1).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                m_AllInfoPanel.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            }

            if (_gameObject.transform.GetChild(1).GetChild(2).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);

                // ������ ������ ��Ȳ���� ������ �����ϸ� ���� ������ ������ �����̶� ������ ������ �ɷ�ġ�� ���ؼ� ����� AllInfo�� �����ش�.
                if (m_SaveData.m_SelectProfessorDataSave != null)
                {
                    string _tempStat = _gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                    int _parshingStat = int.Parse(_tempStat) + m_SaveData.m_SelectProfessorDataSave.professorPower;

                    m_AllInfoPanel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _parshingStat.ToString();
                }
                else
                {
                    m_AllInfoPanel.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                }
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            m_AllInfoPanel.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (_gameObject.transform.GetChild(2).gameObject.activeSelf == true)
        {
            // �ι��� ���� �κ�
            m_AllInfoPanel.transform.GetChild(4).gameObject.SetActive(true);
            m_AllInfoPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;

            // ���� Ŭ���� ������ ������ ���� �����̸�
            if (_gameObject.transform.GetChild(2).GetChild(1).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
                m_AllInfoPanel.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
            }

            if (_gameObject.transform.GetChild(2).GetChild(2).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);

                if (m_SaveData.m_SelectProfessorDataSave != null)
                {
                    string _tempStat = _gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                    int _parshingStat = int.Parse(_tempStat) + m_SaveData.m_SelectProfessorDataSave.m_ProfessorPower;

                    m_AllInfoPanel.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _parshingStat.ToString();
                }
                else
                {
                    m_AllInfoPanel.transform.GetChild(4).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                }
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            m_AllInfoPanel.transform.GetChild(4).gameObject.SetActive(false);
        }

        if (_gameObject.transform.GetChild(3).gameObject.activeSelf == true)
        {
            // ������ ���� �κ�
            m_AllInfoPanel.transform.GetChild(5).gameObject.SetActive(true);
            m_AllInfoPanel.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text;

            // ���� Ŭ���� ������ ������ ���� �����̸�
            if (_gameObject.transform.GetChild(3).GetChild(1).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                m_AllInfoPanel.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
            }

            if (_gameObject.transform.GetChild(3).GetChild(2).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(true);

                if (m_SaveData.m_SelectProfessorDataSave != null)
                {
                    string _tempStat = _gameObject.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                    int _parshingStat = int.Parse(_tempStat) + m_SaveData.m_SelectProfessorDataSave.m_ProfessorPower;

                    m_AllInfoPanel.transform.GetChild(5).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _parshingStat.ToString();
                }
                else
                {
                    m_AllInfoPanel.transform.GetChild(5).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameObject.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
                }
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(5).GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            m_AllInfoPanel.transform.GetChild(5).gameObject.SetActive(false);
        }
    }

    private void SetPanel(GameObject _panel, Class _classData, int _index)
    {
        _panel.transform.GetChild(_index).gameObject.SetActive(true);
        _panel.transform.GetChild(_index).GetChild(1).GetComponent<TextMeshProUGUI>().text = _classData.m_ClassName;
        _panel.transform.GetChild(_index).GetChild(2).GetComponent<TextMeshProUGUI>().text = _classData.m_Money.ToString();
    }

    // ������ �������� �� �ٷιٷ� �� �ݾ��� �ٲ� �� �ְ� ���ִ� �Լ�
    private void SaveMoneySelectPaenl(GameObject _Weekpanel, Class _className, int _2WeekPanelIndex)
    {
        m_TotalMoney = 0;

        for (int i = 0; i < _Weekpanel.transform.childCount - 1; i++)
        {
            if (_Weekpanel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                string _tempGMMoney = _Weekpanel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text;

                m_ClassMoney[i + _2WeekPanelIndex] = int.Parse(_tempGMMoney);
            }
        }
    }

    private void InitAllInfoStat()
    {
        // ü�°� ��
        m_AllInfoPanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        // �����̸�
        m_AllInfoPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        // �����̸�
        m_AllInfoPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

        // ���ȵ�
        for (int i = 3; i < 6;)
        {
            if (m_AllInfoPanel.transform.GetChild(i).gameObject.activeSelf == true)
            {
                m_AllInfoPanel.transform.GetChild(i).gameObject.SetActive(false);
                m_AllInfoPanel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";

                if (m_AllInfoPanel.transform.GetChild(i).GetChild(1).gameObject.activeSelf == true)
                {
                    m_AllInfoPanel.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
                else if (m_AllInfoPanel.transform.GetChild(i).GetChild(2).gameObject.activeSelf == true)
                {
                    m_AllInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                }
                i++;
            }
            else
            {
                i++;
            }
        }

    }

    // ���� Ŭ������ �� Ŭ���� ������ ������ AllInfoPanel�� ����ֱ� ���� �Լ�
    private void ClickClass()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        //Class _ClickClassData;

        m_SelectClass.classData.TryGetValue(_currentObj.name, out _ClickClassData);

        //m_CompareHealth = _ClickClassData;

        int _compareMoney = int.Parse(m_InGameUICurrentMoney.text);
        int _compareHealth = _ClickClassData.m_Health;

        m_AllInfoPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _ClickClassData.m_ClassName;
        m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = _ClickClassData.m_Health.ToString();

        // ���� ���� �����ϰ� �ִ� ������ �����ᰡ �� ��θ� ��� ����ֱ�
        if (_compareMoney >= _ClickClassData.m_Money)
        {
            m_AllInfoPanel.transform.GetChild(8).gameObject.SetActive(false);
            m_AllInfoPanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            m_AllInfoPanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _ClickClassData.m_Money.ToString();

        }
        else
        {
            m_AllInfoPanel.transform.GetChild(8).gameObject.SetActive(true);
            m_AllInfoPanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            m_AllInfoPanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = _ClickClassData.m_Money.ToString();
        }

        if (_ClickProfessorData != null)
        {
            if (_compareHealth <= _ClickProfessorData.m_ProfessorHealth)
            {
                m_AllInfoPanel.transform.GetChild(9).gameObject.SetActive(false);
                m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(9).gameObject.SetActive(true);
                m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            }
        }

        SetStat(_currentObj);

        if (m_ChangeWeekClassText.text == "1����")
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                SetPanel(m_1WeekPanel, _ClickClassData, 1);
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                SetPanel(m_1WeekPanel, _ClickClassData, 2);
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                SetPanel(m_1WeekPanel, _ClickClassData, 3);
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��ȹ")
                {
                    SetPanel(m_1WeekPanel, _ClickClassData, 1);
                }
                else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��Ʈ")
                {
                    SetPanel(m_1WeekPanel, _ClickClassData, 2);
                }
                else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "�ù�")
                {
                    SetPanel(m_1WeekPanel, _ClickClassData, 3);
                }
            }
            m_SaveData.m_Week = "1";
            SaveMoneySelectPaenl(m_1WeekPanel, _ClickClassData, 0);
        }
        else
        {
            if (_ClickClassData.m_ClassType == StudentType.GameDesigner)
            {
                SetPanel(m_2WeekPanel, _ClickClassData, 1);
            }
            else if (_ClickClassData.m_ClassType == StudentType.Art)
            {
                SetPanel(m_2WeekPanel, _ClickClassData, 2);
            }
            else if (_ClickClassData.m_ClassType == StudentType.Programming)
            {
                SetPanel(m_2WeekPanel, _ClickClassData, 3);
            }
            else if (_ClickClassData.m_ClassType == StudentType.None)
            {
                if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��ȹ")
                {
                    SetPanel(m_2WeekPanel, _ClickClassData, 1);
                }
                else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "��Ʈ")
                {
                    SetPanel(m_2WeekPanel, _ClickClassData, 2);
                }
                else if (m_ClassPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text == "�ù�")
                {
                    SetPanel(m_2WeekPanel, _ClickClassData, 3);
                }
            }
            m_SaveData.m_Week = "2";
            SaveMoneySelectPaenl(m_2WeekPanel, _ClickClassData, 3);
        }
        //m_CurrentMoney.text = m_CurrentTotalClassMoney.ToString();

        for (int i = 0; i < m_ClassMoney.Length; i++)
        {
            m_TotalMoney += m_ClassMoney[i];
        }

        if (m_TotalMoney > m_CurrentMoney)
        {
            m_AllInfoPanel.transform.GetChild(10).gameObject.SetActive(true);
            m_TotalMoneyPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_TotalMoney.ToString();
            m_TotalMoneyPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            // totalmoney 0, 0�� Text ������ ������ֱ�
        }
        else
        {
            m_AllInfoPanel.transform.GetChild(10).gameObject.SetActive(false);
            m_TotalMoneyPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = m_TotalMoney.ToString();
            m_TotalMoneyPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        }


        m_SaveData.m_SelectClassDataSave = _ClickClassData;
    }

    private void ClickProfessor()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;

        //ProfessorStat _ClickProfessorData;

        m_SelectProfessor.professorData.TryGetValue(_currentObj.name, out _ClickProfessorData);

        m_AllInfoPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _ClickProfessorData.m_ProfessorNameValue;

        if (_ClickClassData != null)
        {
            int _compareHealth = _ClickClassData.m_Health;

            if (_compareHealth <= _ClickProfessorData.m_ProfessorHealth)
            {
                m_AllInfoPanel.transform.GetChild(9).gameObject.SetActive(false);
                m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            }
            else
            {
                m_AllInfoPanel.transform.GetChild(9).gameObject.SetActive(true);
                m_AllInfoPanel.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            int _tempindex = AllInfoDataStatIndex(m_SaveData);

            for (int i = 3; i < _tempindex + 3;)
            {
                if (m_AllInfoPanel.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    if (m_AllInfoPanel.transform.GetChild(i).GetChild(1).gameObject.activeSelf == true)
                    {
                        i++;
                    }
                    else if (m_AllInfoPanel.transform.GetChild(i).GetChild(2).gameObject.activeSelf == true)
                    {
                        if (m_SaveData.m_SelectClassDataSave.m_Insight != 0 && m_SaveData.m_SelectClassDataSave.m_Concentration != 0 &&
                            m_SaveData.m_SelectClassDataSave.m_Sense != 0 && m_SaveData.m_SelectClassDataSave.m_Technique != 0 &&
                            m_SaveData.m_SelectClassDataSave.m_Wit != 0)
                        {
                            int _temp = m_SaveData.m_SelectClassDataSave.m_Insight + m_SaveData.m_SelectClassDataSave.m_Concentration + m_SaveData.m_SelectClassDataSave.m_Sense
                                        + m_SaveData.m_SelectClassDataSave.m_Technique + m_SaveData.m_SelectClassDataSave.m_Wit + _ClickProfessorData.m_ProfessorPower;

                            m_AllInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = _temp.ToString();

                        }
                        else
                        {
                            FindStatAllInfo();

                            if (m_AllInfoStatNum[i - 3] > 0)
                            {
                                m_AllInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (m_AllInfoStatNum[i - 3] + _ClickProfessorData.m_ProfessorPower).ToString();
                            }
                        }
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
        }
        else
        {

        }
        m_SaveData.m_SelectProfessorDataSave = _ClickProfessorData;
    }

    private void CheckClassToBackButton()
    {
        switch (m_ClassStack)
        {
            case 0:
            {

            }
            break;

            case 1:
            {

            }
            break;

            case 2:
            {

            }
            break;

            case 3:
            {

            }
            break;

            case 4:
            {

            }
            break;

            case 5:
            {

            }
            break;
        }

    }

    public void InitTotalInfo(GameObject _panel)
    {
        for (int i = 0; i < _panel.transform.childCount - 1; i++)
        {
            if (_panel.transform.GetChild(i + 1).gameObject.activeSelf == true)
            {
                _panel.transform.GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                _panel.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
        }
    }

    // �ڷΰ��� ��ư�� ������ �� ������ �Լ�
    public void ClickBackButton()
    {
        m_ClassStack--;

        CheckClassToBackButton();

    }

    public void ClickHideButton()
    {
        if(m_CheckClassPanel.activeSelf == true)
        {
            m_HideCheckPanel.BackButton();
            //m_CheckClassPanel.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
        else if(m_SelectClassPanel.activeSelf == true)
        {
            m_HideSelectPanel.BackButton();
            //m_SelectClassPanel.SetActive(false);
            m_OpenClassButton.gameObject.SetActive(true);
        }
    }

    public void OpenClassButton()
    {
        if(m_ArtData[1].m_SelectClassDataSave != null && m_GameDesignerData[1].m_SelectClassDataSave != null && m_ProgrammingData[1].m_SelectClassDataSave != null)
        {
            m_OpenCheckPanel.TurnOnUI();
            //m_CheckClassPanel.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
        else
        {
            m_OpenSelectPanel.TurnOnUI();
            //m_SelectClassPanel.SetActive(true);
            m_OpenClassButton.gameObject.SetActive(false);
        }
    }
}
