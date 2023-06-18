using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

public class InJaeRecommend : MonoBehaviour
{
    [SerializeField] private StatsRadar m_CompanyStatsRadar;
    [SerializeField] private StatsRadar m_StudentStatsRadar;
    [SerializeField] private GameObject m_Alram;
    [SerializeField] private GameObject m_RecommendAni;

    [Space(5f)]
    [Header("������õâ�� ����� ��ư ������")]
    [SerializeField] private GameObject m_CompanyPrefab;
    [SerializeField] private GameObject m_RecommendPrefab;
    [SerializeField] private GameObject m_RequiredSkillPrefab;
    [SerializeField] private GameObject m_StudentPrefab;

    [Space(5f)]
    [Header("������õȮ��â�� ����� �л� ������")]
    [SerializeField] private GameObject m_CheckPanelStudentPrefab;

    [Space(5f)]
    [Header("ȸ�� ��ũ�� ���� �ٲ��� �̹�����")]
    [SerializeField] private Sprite m_SRankSprite;
    [SerializeField] private Sprite m_ARankSprite;
    [SerializeField] private Sprite m_BRankSprite;
    [Space(5f)]

    [Space(5f)]
    [Header("�л��� ������ �䱸���ȿ� �����ϴ����� ���� �̹�����")]
    [SerializeField] private Sprite m_SatisfiedSprite;
    [SerializeField] private Sprite m_NotSatisfiedSprite;
    [Space(5f)]

    [Space(5f)]
    [Header("��õ Ȯ��â���� ���õ� �л����� �հݷ��� ���� �̹�����")]
    [SerializeField] private Sprite m_UpToEightyPercentSprite;
    [SerializeField] private Sprite m_UpToSixtyPercentSprite;
    [SerializeField] private Sprite m_UpToFourtyPercentSprite;
    [SerializeField] private Sprite m_UpToZeroPercentSprite;

    [Space(5f)]
    [Header("���â�� ����� �հ�, ���հ� �̹���")]
    [SerializeField] private Sprite m_Pass;
    [SerializeField] private Sprite m_Fail;

    [SerializeField] private Sprite m_DetailPass;
    [SerializeField] private Sprite m_DetailFail;

    [SerializeField] private InJaeRecommendList m_RecommendListPanel;
    [SerializeField] private InJaeRecommendInfo m_RecommendInfoPanel;
    [SerializeField] private InJaeRecommendResult m_RecommendResultPanel;
    [SerializeField] private Sprite[] m_PartImage;

    private bool m_IsRecommendCreation;                                             // ���� 2���� ���� �� �ѹ��� ������ֱ� ���ؼ�
    private bool m_IsRecommendFalseCheck;                                           // ���� 2�� 3���� 3���� ���� �� �л� �Ѹ��̶� �����õ�� ���ߴٸ� �ѹ��� �г��� ���� ����
    private bool m_IsRecommendResult;                                               // ���� 2�� 4���� 5���� ���� �� �л����� �հ� ���� �г��� �ѹ��� ����ַ���
    private bool _isPass;
    private int m_CompanySense;                                                   // ȸ�簡 �䱸�ϴ� ��ġ��
    private int m_CompanyConcentration;                                           //
    private int m_CompanyWit;                                                     //
    private int m_CompanyTechinque;                                               // 
    private int m_CompanyInsight;                                                 //
    private int m_NowEmploymentIndex;                                               // ���� ������ ������ m_SortCompany���ִ� EmploymentList�� �ε���
    private string m_CompanyName;                                                   // ���� ������ ȸ���� �̸��� string������ ������ �ִ´�
    private int m_NowRecommendCount;
    private double Average;                                                         // ���� ����� Ȯ��
    private string[] GMStudentName = new string[6];                                 // ���� ������ �л����� �̸�
    private string[] ArtStudentName = new string[6];                                //
    private string[] ProgrammingStudentName = new string[6];                        //
    private Sprite[] GMStudentImgae = new Sprite[6];                                 // ���� ������ �л����� �̸�
    private Sprite[] ArtStudentImgae = new Sprite[6];                                //
    private Sprite[] ProgrammingStudentImgae = new Sprite[6];

    public List<Company> m_CompanyList = new List<Company>();                       // json���� ���� �����͵��� ȸ�翡 ���� �з����ֱ� ���� ����Ʈ
    private List<Company> m_SortCompany = new List<Company>();                      // ȸ���� ������� �������ִ� ����Ʈ
    private List<Student> m_SelectedStudent = new List<Student>();                  // ���� ������ �л����� ������ �ӽ÷� ������ ����Ʈ
    private List<double> m_SelectedStudentPercent = new List<double>();             // ���� ������ �л��� ���� ���������� �����ߴ� �л��� �����͸� ���������.
    private List<string> m_CompanyRequiredSkillName = new List<string>();            // ���� ������ ȸ���� �䱸 ��ų �̸� ���
    private Sprite[] GMStudentPassFail = new Sprite[6];                             // ���� �����ߴ� �л����� �հ�, ���հ� ���θ� ������ ��������Ʈ�� �־�����Ѵ�.
    private Sprite[] ArtStudentPassFail = new Sprite[6];
    private Sprite[] ProgrammingStudentPassFail = new Sprite[6];

    private void Start()
    {
        m_IsRecommendCreation = false;
        m_IsRecommendFalseCheck = false;
        m_IsRecommendResult = false;
        m_NowRecommendCount = 0;

        FillCompanyList(); // ������õ �����͸� �ִ� �Լ�

        m_RecommendInfoPanel.RecommendButton.onClick.AddListener(ClickRecommendButtonToPart);
        m_RecommendInfoPanel.CheckPanelOKButton.onClick.AddListener(ClickCheckPanelOKButton);
    }

    private void Update()
    {
        if (GameTime.Instance.FlowTime.NowMonth == 2 && !m_IsRecommendCreation)
        {
            m_Alram.SetActive(true);

            MakeCompanyButton();

            m_IsRecommendCreation = true;
        }
        // �л��� �Ѹ��̶� ������õ�� ���ߴٸ� ������ ��õ ȭ�� ����ֱ�
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3
            && GameTime.Instance.FlowTime.NowDay == 3 && !m_IsRecommendFalseCheck)
        {
            for (int i = 0; i < ObjectManager.Instance.m_StudentList.Count; i++)
            {
                if (ObjectManager.Instance.m_StudentList[i].m_StudentStat.m_IsRecommend == false)
                {
                    m_IsRecommendFalseCheck = true;
                    m_RecommendListPanel.PopUpRecommendListPanel();
                    m_RecommendListPanel.LockCloseButton(false);
                    m_RecommendInfoPanel.LockInfoPanelCloseButton(false);
                    break;
                }
            }
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3
            && GameTime.Instance.FlowTime.NowDay == 5) // �������� ������ �ð��� ���ַ��� �ð�üũ�ؼ� ������Ѵ�.
        {
            m_RecommendAni.SetActive(false);
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4
            && GameTime.Instance.FlowTime.NowDay == 5 && !m_IsRecommendResult)
        {
            m_IsRecommendResult = true;
            m_RecommendResultPanel.PopUpResultPanel();
        }
        else if (GameTime.Instance.FlowTime.NowMonth == 3)
        {
            m_IsRecommendCreation = false;
            m_IsRecommendFalseCheck = false;
            m_IsRecommendResult = false;
            m_NowRecommendCount = 0;
        }
    }

    // ��� ȸ��� ������� ȸ�� �̸��� �������� ������ش�.
    private void FillCompanyList()
    {
        for (int i = 0; i < AllOriginalJsonData.Instance.OriginalInJaeRecommendData.Count; i++)
        {
            if (m_CompanyList.Find(x => x.CompanyName == AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName) == null)
            {
                Company _newCompany = new Company();
                _newCompany.CompanyID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyID;
                _newCompany.CompanyName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName;
                _newCompany.CompanyGrade = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyGrade_ID;
                _newCompany.CompanyGameNameID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyGameNameID;
                _newCompany.IsNewCompany = true;
                _newCompany.IsUnLockCompany = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyUnLock;
                _newCompany.CompanyIcon = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyIcon;

                _newCompany.EmploymentList = new List<Employment>();

                Employment _newEmployment = new Employment();
                _newEmployment.EmploymentID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentID;
                _newEmployment.EmploymentName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentName;
                _newEmployment.EmploymentText = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentText;
                _newEmployment.CompanyJob = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyJob;
                _newEmployment.CompanySalary = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanySalary;
                _newEmployment.CompanyPart = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyPart;
                string[] _tempEmploymentYear = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentYear.Split(",");
                _newEmployment.EmploymentYear = new int[_tempEmploymentYear.Length];

                for (int j = 0; j < _tempEmploymentYear.Length; j++)
                {
                    _newEmployment.EmploymentYear[j] = int.Parse(_tempEmploymentYear[j]);
                }

                if (AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Contains(","))
                {
                    string[] _tempSkill = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Split(",");
                    _newEmployment.CompanyRequirementSkill = new int[_tempSkill.Length];

                    for (int j = 0; j < _tempSkill.Length; j++)
                    {
                        _newEmployment.CompanyRequirementSkill[j] = int.Parse(_tempSkill[j]);
                    }
                }
                else
                {
                    _newEmployment.CompanyRequirementSkill = new int[1];
                    _newEmployment.CompanyRequirementSkill[0] = int.Parse(AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID);
                }
                _newEmployment.FailStudent = new List<Student>();
                _newEmployment.PassStudent = new List<Student>();
                _newEmployment.CompanyRequirementStats = new Dictionary<string, int>();

                _newEmployment.CompanyRequirementStats = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementStats;
                _newCompany.EmploymentList.Add(_newEmployment);
                m_CompanyList.Add(_newCompany);
            }
            else
            {
                Company _nowCompany = m_CompanyList.Find(x => x.CompanyName == AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyName);

                Employment _newEmployment = new Employment();
                _newEmployment.EmploymentID = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentID;
                _newEmployment.EmploymentName = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentName;
                _newEmployment.EmploymentText = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentText;
                _newEmployment.CompanyJob = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyJob;
                _newEmployment.CompanySalary = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanySalary;
                _newEmployment.CompanyPart = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyPart;

                string[] _tempEmploymentYear = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_EmploymentYear.Split(",");
                _newEmployment.EmploymentYear = new int[_tempEmploymentYear.Length];

                for (int j = 0; j < _tempEmploymentYear.Length; j++)
                {
                    _newEmployment.EmploymentYear[j] = int.Parse(_tempEmploymentYear[j]);
                }

                if (AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Contains(","))
                {
                    string[] _tempSkill = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID.Split(",");
                    _newEmployment.CompanyRequirementSkill = new int[_tempSkill.Length];

                    for (int j = 0; j < _tempSkill.Length; j++)
                    {
                        _newEmployment.CompanyRequirementSkill[j] = int.Parse(_tempSkill[j]);
                    }
                }
                else
                {
                    _newEmployment.CompanyRequirementSkill = new int[1];
                    _newEmployment.CompanyRequirementSkill[0] = int.Parse(AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementSkill_ID);
                }
                _newEmployment.FailStudent = new List<Student>();
                _newEmployment.PassStudent = new List<Student>();
                _newEmployment.CompanyRequirementStats = new Dictionary<string, int>();

                _newEmployment.CompanyRequirementStats = AllOriginalJsonData.Instance.OriginalInJaeRecommendData[i].m_CompanyRequirementStats;
                _nowCompany.EmploymentList.Add(_newEmployment);
            }
        }
    }

    // ȸ�� ��ư�� ������ش�.
    private void MakeCompanyButton()
    {
        Transform _companyparent = m_RecommendListPanel.CompanyContent();

        if (_companyparent.childCount != 0)
        {
            m_RecommendListPanel.DestroyCompanyContent();
        }

        CheckConditionToMakeCompany();
    }

    // ȸ�� �̸����� ���� ��ư�� ������ ���� �������� �� �����. ȸ�� ��ũ �̹����� �־�����Ѵ�.
    private void CheckConditionToMakeCompany()
    {
        // ������ �ְ� ���� ���ǿ� ���� �������ֱ�. ������ ����� �ٽ� ������ ���ֱ� ���� ���⿡ ����
        m_SortCompany = SortCompanyToConditions();

        for (int i = 0; i < m_SortCompany.Count; i++)
        {
            // ���� ������ �ش� ��ư�� ������ ������ �� ���� �������ֱ�
            if (m_SortCompany[i].IsUnLockCompany)
            {
                Transform _companyButtonParent = m_RecommendListPanel.CompanyContent();
                GameObject _companyButton = Instantiate(m_CompanyPrefab, _companyButtonParent);

                _companyButton.name = m_SortCompany[i].CompanyName;
                _companyButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SortCompany[i].CompanyName;

                if (m_SortCompany[i].IsNewCompany)
                {
                    _companyButton.transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    _companyButton.transform.GetChild(2).gameObject.SetActive(false);
                }

                switch (m_SortCompany[i].CompanyGrade)
                {
                    case 0: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_SRankSprite; break;
                    case 1: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_ARankSprite; break;
                    case 2: _companyButton.transform.GetChild(0).GetComponent<Image>().sprite = m_BRankSprite; break;
                }
                _companyButton.GetComponent<Button>().onClick.AddListener(MakeRecommend);
            }
            else
            {
                break;
            }
        }
    }

    // �ش� ȸ�簡 ������ �ִ� ������� �����ִ� �Լ�. 
    private void MakeRecommend()
    {
        m_RecommendListPanel.DestroyRecommendList();

        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        m_CompanyName = _currentObj.name;

        List<Employment> _sortListToPart = SortRecommendToConditions();

        for (int i = 0; i < _sortListToPart.Count; i++)
        {
            for (int j = 0; j < _sortListToPart[i].EmploymentYear.Length; j++)
            {
                if (_sortListToPart[i].EmploymentYear[j] == 0 || _sortListToPart[i].EmploymentYear[j] == GameTime.Instance.FlowTime.NowYear)
                {
                    Transform _recommendButtonParent = m_RecommendListPanel.RecommendeContent();
                    GameObject _recommendButton = Instantiate(m_RecommendPrefab, _recommendButtonParent);

                    _recommendButton.name = _sortListToPart[i].EmploymentName;
                    _recommendButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _sortListToPart[i].EmploymentName;
                    // ������ ��Ʈ �̹����� �־��ִ� �κ�
                    if (_sortListToPart[i].CompanyPart == 1)
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[0];
                    }
                    else if (_sortListToPart[i].CompanyPart == 2)
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[1];
                    }
                    else
                    {
                        _recommendButton.transform.GetChild(1).GetComponent<Image>().sprite = m_PartImage[2];
                    }
                    _recommendButton.GetComponent<Button>().onClick.AddListener(MakeRecommendInfo);
                }
            }
        }

        if (m_SelectedStudent.Count != 0)
        {
            m_SelectedStudent.Clear();
            m_SelectedStudentPercent.Clear();
        }
    }

    // ���� ������ ������ ���� ������ �ٲ��ش�. �л����� ó�� ���� �� ��ȹ�л����� ������ �ϱ�
    private void MakeRecommendInfo()
    {
        GameObject _currentObj = EventSystem.current.currentSelectedGameObject;
        string _recommendName = _currentObj.name;

        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        int _companyIndex = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);
        m_NowEmploymentIndex = m_SortCompany[_index].EmploymentList.FindIndex(x => x.EmploymentName == _recommendName);

        string _companyName = m_CompanyName;
        string _employmentName = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].EmploymentName;
        string _employmentText = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].EmploymentText;
        string _salary = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanySalary.ToString();
        string _task = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyJob;
        int _part = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;
        //string _companyGrade = m_SortCompany[_index].CompanyGrade;
        //int _companyID = m_ClassifyData[m_CompanyName][i].m_CompanyID;

        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Sense", out m_CompanySense);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Concentration", out m_CompanyConcentration);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Wit", out m_CompanyWit);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Technique", out m_CompanyTechinque);
        m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementStats.TryGetValue("Insight", out m_CompanyInsight);

        m_RecommendInfoPanel.SetNotification("����  " + m_CompanySense.ToString(), "����  " + m_CompanyConcentration.ToString(), "��ġ  " + m_CompanyWit.ToString(), "���  " + m_CompanyTechinque.ToString(), "����  " + m_CompanyInsight.ToString());
        m_RecommendInfoPanel.SetRecommendInfoListText(_companyName, _employmentName, _employmentText, _salary, _task);
        m_RecommendInfoPanel.SetRecruitmentPart(_part);

        //MakeSkills(m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyRequirementSkill);
        MakeCompanyPentagon(m_CompanySense, m_CompanyConcentration, m_CompanyWit, m_CompanyTechinque, m_CompanyInsight);

        // �ʱ�ȭ
        m_RecommendInfoPanel.SetPercent("?? %");
        m_RecommendInfoPanel.StudentStatObj.SetActive(false);
        MakeStudentPentagon(0, 0, 0, 0, 0);

        MakeStudent(StudentType.GameDesigner);
        m_RecommendInfoPanel.PartGMButton.Select();

        m_RecommendListPanel.PopOffRecommendListPanel();
        m_RecommendInfoPanel.PopUpRecommendInfoPanel();

        if (m_CompanyList[_companyIndex].IsNewCompany == true)
        {
            m_CompanyList[_companyIndex].IsNewCompany = false;
        }

        MakeCompanyButton();
    }

    // ��Ÿ���� �־��� ������ ������ִ� �Լ�
    private void MakeCompanyPentagon(int _sense, int _concentration, int _wit, int _technique, int _insight)
    {
        PentagonStats stats = new PentagonStats(_sense, _concentration, _wit, _technique, _insight);

        m_CompanyStatsRadar.SetStats(stats);
    }

    private void MakeStudentPentagon(int _sense, int _concentration, int _wit, int _technique, int _insight)
    {
        PentagonStats stats = new PentagonStats(_sense, _concentration, _wit, _technique, _insight);
        m_StudentStatsRadar.SetStats(stats);
    }

    /// TODO : �л��� ���ʽ� ��ų�� �����ϴ� ��. ��ų �Ǻ��� ������Ѵ�.
    private void MakeSkills(int[] _skillList)
    {
        string _skillName = "";
        float _skillObjWidth = 0;           // ������ ���� ��ų�� ���̿��� 23�� �� �� ���� �־���� ������ �� ���´�.
        int _x = 89;                        // 89�� �Ǿ��ִ� ������ ��ų�� ��������� �ϴ� ��ġ�� 89���� �����ϱ� ����    
        m_CompanyRequiredSkillName.Clear();

        Transform _skillParent = m_RecommendInfoPanel.RequiredSkillParentTransform();

        for (int i = 0; i < _skillList.Length; i++)
        {
            // 0�̸� ���ٴ� ���̴� �Լ��� �������ش�.
            if (_skillList[i] == 0)
            {
                break;
            }

            _skillName = StudentSkills.FindSkillName(_skillList[i]);
            GameObject _skill = Instantiate(m_RequiredSkillPrefab, _skillParent);
            _skill.name = _skillName;
            m_CompanyRequiredSkillName.Add(_skillName);
            _skill.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _skillName;
            _skill.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(_x, -385);
            _skillObjWidth = _skill.GetComponent<RectTransform>().sizeDelta.x;
            _x = (int)_skillObjWidth + 23;
        }
    }

    // ��õ ���θ� �������� �л����� ���Ľ����ش�.
    private List<Student> SortStudentToRecommend()
    {
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        _studentList.Sort((x, y) =>
        {
            if (x.m_StudentStat.m_IsRecommend && !y.m_StudentStat.m_IsRecommend) return 1;
            else if (!x.m_StudentStat.m_IsRecommend && y.m_StudentStat.m_IsRecommend) return -1;
            else return 0;
        });

        return _studentList;
    }

    // ȸ���ư ��¼����� ���� ����
    private List<Company> SortCompanyToConditions()
    {
        //List<InJaeRecommendData> _injaeDataList = new List<InJaeRecommendData>(m_RecommendList);
        List<Company> _companyList = new List<Company>(m_CompanyList);

        // -1�� ������ ���� �ϴ°�
        _companyList.Sort((x, y) =>
        {
            if (!x.IsUnLockCompany && y.IsUnLockCompany) return -1;
            else if (x.IsUnLockCompany && !y.IsUnLockCompany) return 1;
            else if (x.IsNewCompany && !y.IsNewCompany) return -1;
            else if (!x.IsNewCompany && y.IsNewCompany) return 1;
            else if (x.CompanyGrade < y.CompanyGrade) return -1;
            else if (x.CompanyGrade > y.CompanyGrade) return 1;
            else if (x.CompanyID < y.CompanyID) return -1;
            else if (x.CompanyID > y.CompanyID) return 1;
            else return 0;
        });

        return _companyList;
    }

    // ȸ�� ���� ��¼����� ���� ����
    private List<Employment> SortRecommendToConditions()
    {
        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        List<Employment> _sortList = new List<Employment>(m_SortCompany[_index].EmploymentList);

        _sortList.Sort((x, y) =>
        {
            if (x.CompanyPart < y.CompanyPart) return -1;
            else return 1;
        });

        return _sortList;
    }

    // ������ �ִ� �л����� ������Ʈ�� �����ְ� �ٽ� �л����� ������ش�.
    private void MakeStudent(StudentType _studentType)
    {
        m_RecommendInfoPanel.DestroyStudentList();

        List<Student> _studentList = SortStudentToRecommend();
        Transform _studentPrefabParent = m_RecommendInfoPanel.StudentListContent;

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_studentList[i].m_StudentStat.m_StudentType == _studentType)
            {
                GameObject _student = Instantiate(m_StudentPrefab, _studentPrefabParent);
                _student.name = _studentList[i].m_StudentStat.m_StudentName;

                if (_studentList[i].m_StudentStat.m_IsRecommend)
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.SetActive(true);
                    _student.GetComponent<Button>().interactable = false;
                }
                else
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.SetActive(false);
                    _student.GetComponent<Button>().interactable = true;
                }

                CompareNameAndTurnOnCheckImage(_student);

                _student.GetComponent<RecommendStudentPrefab>().m_StudentName.text = _studentList[i].m_StudentStat.m_StudentName;
                _student.GetComponent<RecommendStudentPrefab>().m_StudentImgae.sprite = _studentList[i].StudentProfileImg;
                _student.GetComponent<Button>().onClick.AddListener(ClickStudent);
                // �л� �̹��� ��ü�� ����� ��
            }
        }
    }

    // ���� ������ �л��� �ִٸ� üũ ǥ�� �ٽ� ���ֱ�
    private void CompareNameAndTurnOnCheckImage(GameObject _student)
    {
        if (m_SelectedStudent.Count != 0)
        {
            for (int i = 0; i < m_SelectedStudent.Count; i++)
            {
                if (m_SelectedStudent[i].m_StudentStat.m_StudentName == _student.name)
                {
                    _student.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(true);
                }
            }
        }
    }

    // Ŭ���� �л��� ������ ��������Ѵ�.
    private void ClickStudent()
    {
        GameObject _clickStudent = EventSystem.current.currentSelectedGameObject;
        List<Student> _studentList = new List<Student>(ObjectManager.Instance.m_StudentList);

        int _sense;
        int _concentration;
        int _wit;
        int _technique;
        int _insight;

        for (int i = 0; i < _studentList.Count; i++)
        {
            if (_studentList[i].m_StudentStat.m_StudentName == _clickStudent.name)
            {
                _sense = _studentList[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense];
                _concentration = _studentList[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration];
                _wit = _studentList[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit];
                _technique = _studentList[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique];
                _insight = _studentList[i].m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight];

                if (!m_SelectedStudent.Contains(_studentList[i]))
                {
                    m_SelectedStudent.Add(_studentList[i]);
                }

                SetPassPercentage(_studentList[i]);
                m_RecommendInfoPanel.SetStudentStatText("����  " + _sense.ToString(), "����  " + _concentration.ToString(), "��ġ  " + _wit.ToString(), "���  " + _technique.ToString(), "����  " + _insight.ToString());
                DecideStudentStatImage(_sense, _concentration, _wit, _technique, _insight);
                MakeStudentPentagon(_sense, _concentration, _wit, _technique, _insight);
            }
        }

        // 1�� ��õ�Ϸ�, 0�� üũ
        // ��õ�Ϸᰡ ���� ��
        if (!_clickStudent.GetComponent<RecommendStudentPrefab>().m_RecommendTrue.activeSelf)
        {
            if (_clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.activeSelf)
            {
                // üũǥ�� ���ֱ�
                _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(false);

                if (m_SelectedStudent.Count >= 1)
                {
                    for (int i = 0; i < _studentList.Count; i++)
                    {
                        if (_clickStudent.name == m_SelectedStudent[i].m_StudentStat.m_StudentName)
                        {
                            m_SelectedStudent.RemoveAt(i);
                            m_SelectedStudentPercent.RemoveAt(i);

                            m_RecommendInfoPanel.StudentStatObj.SetActive(false);
                            m_RecommendInfoPanel.SetPercent("?? %");
                            MakeStudentPentagon(0, 0, 0, 0, 0);
                            break;
                        }
                    }
                }
            }
            else
            {
                m_SelectedStudentPercent.Add(Average);
                _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(true);
                m_RecommendInfoPanel.StudentStatObj.SetActive(true);
            }
        }
        else
        {
            _clickStudent.GetComponent<RecommendStudentPrefab>().m_CheckImage.SetActive(false);
        }
    }

    // Ŭ���� �л��� �հݷ��� ������ִ� �Լ�
    private void SetPassPercentage(Student _student)
    {
        double _companyrequiredSense = m_CompanySense;
        double _companyrequiredConcentration = m_CompanyConcentration;
        double _companyrequiredWit = m_CompanyWit;
        double _companyrequiredTechinque = m_CompanyTechinque;
        double _companyrequiredInsight = m_CompanyInsight;

        double _selectStudentSense = _student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Sense];
        double _selectStudentConcentration = _student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Concentration];
        double _selectStudentWit = _student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Wit];
        double _selectStudentTechinque = _student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Technique];
        double _selectStudentInsight = _student.m_StudentStat.m_AbilityAmountList[(int)AbilityType.Insight];

        double _sensePercent = (_selectStudentSense / _companyrequiredSense * 100 >= 100) ? 100 : _selectStudentSense / _companyrequiredSense * 100;
        double _concentrationPercent = (_selectStudentConcentration / _companyrequiredConcentration * 100 >= 100) ? 100 : _selectStudentConcentration / _companyrequiredConcentration * 100;
        double _witPercent = (_selectStudentWit / _companyrequiredWit * 100 >= 100) ? 100 : _selectStudentWit / _companyrequiredWit * 100;
        double _techinquePercent = (_selectStudentTechinque / _companyrequiredTechinque * 100 >= 100) ? 100 : _selectStudentTechinque / _companyrequiredTechinque * 100;
        double _insightPercent = (_selectStudentInsight / _companyrequiredInsight * 100 >= 100) ? 100 : _selectStudentInsight / _companyrequiredInsight * 100;

        double _totalPercent = _sensePercent + _concentrationPercent + _witPercent + _techinquePercent + _insightPercent;
        Average = _totalPercent / 5;

        Average = Math.Truncate(Average);
        m_RecommendInfoPanel.SetPercent(Average.ToString() + " %");
    }

    // ȸ�簡 �䱸�� ���ȿ� ���������� �Ķ������� �ƴ϶�� ���������� �ٲ��ֱ�
    private void DecideStudentStatImage(int _StudentSense, int _StudentConcentration, int _StudentWit, int _StudentTechnique, int _StudentInsight)
    {
        Sprite _senseSprite = m_CompanySense <= _StudentSense ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _concentrationSprite = m_CompanyConcentration <= _StudentConcentration ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _witSprite = m_CompanyWit <= _StudentWit ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _techniqueSprite = m_CompanyTechinque <= _StudentTechnique ? m_SatisfiedSprite : m_NotSatisfiedSprite;
        Sprite _insightSprite = m_CompanyInsight <= _StudentInsight ? m_SatisfiedSprite : m_NotSatisfiedSprite;

        m_RecommendInfoPanel.SetStudentStatImage(_senseSprite, _concentrationSprite, _witSprite, _techniqueSprite, _insightSprite);
    }

    // ��Ʈ���� �л��� �����ִ� �Լ� (�ֵ� üũǥ�ô� â�� ���� �� ���ֱ�)
    public void ClickPartButton()
    {
        m_RecommendInfoPanel.DestroyStudentList();

        GameObject _currentbutton = EventSystem.current.currentSelectedGameObject;
        string _buttonName = _currentbutton.name;

        switch (_buttonName)
        {
            case "ArtTab": MakeStudent(StudentType.Art); break;
            case "GMTab": MakeStudent(StudentType.GameDesigner); break;
            case "ProgrammingTab": MakeStudent(StudentType.Programming); break;
        }
        m_RecommendInfoPanel.ChangeScrollRectTransform();
    }

    // ��õ�ϱ� ��ư�� ������ ���� ���ǿ� �´� �л����� �����ߴ��� Ȯ�����ִ� �Լ�(���� ��Ʈ)
    public void ClickRecommendButtonToPart()
    {
        int _index = m_SortCompany.FindIndex(x => x.CompanyName == m_CompanyName);
        int _part = m_SortCompany[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;
        _isPass = false;
        StudentType _type;

        // �� ������ ����ֱ�
        m_RecommendInfoPanel.ChangeScrollRectTransform();

        switch (_part)
        {
            case 0: _type = StudentType.None; break;
            case 1: _type = StudentType.GameDesigner; break;
            case 2: _type = StudentType.Art; break;
            case 3: _type = StudentType.Programming; break;
            default: _type = StudentType.None; break;
        }

        for (int i = 0; i < m_SelectedStudent.Count; i++)
        {
            if (_type == StudentType.None)
            {
                // �а��� ��� . ��ų���� Ȯ���ϱ� 
                ClickRecommendButtonToSkill();
            }
            else if (_type != m_SelectedStudent[i].m_StudentStat.m_StudentType)
            {
                // �ٸ��ٸ� ��ų Ȯ�α��� �Ȱ��� �׳� ��õ�� �Ұ��� �л��� �ִٴ� ��� ����ֱ�
                _isPass = false;
                break;
            }
            else
            {
                // �а��� ��� ��ųȮ���ϱ�.
                _isPass = true;
            }
        }

        if (_isPass)
        {
            ClickRecommendButtonToSkill();
        }
        else
        {
            // �ٸ��ٸ� ��ų Ȯ�α��� �Ȱ��� �׳� ��õ�� �Ұ��� �л��� �ִٴ� ��� ����ֱ�
            m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(true);

            StartCoroutine(HideImpossibleRecommendPanel());
        }
    }

    // ��õ�ϱ� ��ư�� ������ ���� ���ǿ� �´� �л����� �����ߴ��� Ȯ�����ִ� �Լ�(�䱸 ��ų)
    public void ClickRecommendButtonToSkill()
    {
        //_isPass = false;

        //for (int i = 0; i < m_SelectedStudent.Count; i++)
        //{
        //    for (int j = 0; j < m_SelectedStudent[i].m_StudentStat.m_Skills.Count; i++)
        //    {
        //        if (m_CompanyRequiredSkillName[i] == m_SelectedStudent[i].m_StudentStat.m_Skills[i])
        //        {
        //            _isPass = true;
        //        }
        //        else
        //        {
        //            _isPass = false;
        //            break;
        //        }
        //    }

        //    if (!_isPass)
        //    {
        //        break;
        //    }
        //}

        //// ��ų�� ������ �ִٸ�... ������ ��õ �Ұ� �л��� �ִٰ� ����ֱ�
        //if (!_isPass)
        //{
        //    m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(true);

        //    StartCoroutine(HideImpossibleRecommendPanel());
        //}
        //else
        {
            m_RecommendInfoPanel.SetCheckRecommendPanel();
            m_RecommendInfoPanel.DestroyCheckPanelStudentList();

            int _index = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);
            int _part = m_CompanyList[_index].EmploymentList[m_NowEmploymentIndex].CompanyPart;

            int _companyGrade = m_CompanyList[_index].CompanyGrade;
            string _employmentName = m_CompanyList[_index].EmploymentList[m_NowEmploymentIndex].EmploymentName;

            Transform _checkStudentParentTransform = m_RecommendInfoPanel.CheckPanelStudentParent;

            switch (_companyGrade)
            {
                case 0: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_SRankSprite); break;
                case 1: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_ARankSprite); break;
                case 2: m_RecommendInfoPanel.SetCheckPanelCompanyGradeIcon(m_BRankSprite); break;
            }

            m_RecommendInfoPanel.SetCheckPanelCompanyName(_employmentName);

            for (int i = 0; i < m_SelectedStudent.Count; i++)
            {
                GameObject _student = Instantiate(m_CheckPanelStudentPrefab, _checkStudentParentTransform);
                _student.name = m_SelectedStudent[i].m_StudentStat.m_StudentName;

                double _percent = m_SelectedStudentPercent[i];

                if (_percent >= 80)
                {
                    _student.transform.GetChild(0).GetComponent<Image>().sprite = m_UpToEightyPercentSprite;
                }
                else if (_percent >= 60)
                {
                    _student.transform.GetChild(0).GetComponent<Image>().sprite = m_UpToSixtyPercentSprite;
                }
                else if (_percent >= 40)
                {
                    _student.transform.GetChild(0).GetComponent<Image>().sprite = m_UpToFourtyPercentSprite;
                }
                else
                {
                    _student.transform.GetChild(0).GetComponent<Image>().sprite = m_UpToZeroPercentSprite;
                }

                _student.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _percent + " %";
                _student.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_SelectedStudent[i].m_StudentStat.m_StudentName;
            }

            // �л� �������� ������ ��Ŀ�� �������༭ 4������� ����� ���ְ� 4���� �Ѿ�� �������� ���ش�.
            if (_checkStudentParentTransform.childCount <= 4)
            {
                // ���
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            }
            else
            {
                // ����
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
                _checkStudentParentTransform.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 1f);
            }

            m_RecommendAni.SetActive(true);

            m_RecommendInfoPanel.ChangeScrollRectTransform();
        }
    }

    // ��õ�� Ȯ���ϴ� �гο��� OK��ư�� ������ �� ���� �����ߴ� �л����� ������ �հݷ��� �������ش�.
    private void ClickCheckPanelOKButton()
    {
        m_RecommendInfoPanel.PopOffRecommendInfoPanel();
        m_RecommendListPanel.PopUpRecommendListPanel();
        m_RecommendInfoPanel.ClickCheckPanelCancleButton();

        int _companyIndex = m_CompanyList.FindIndex(x => x.CompanyName == m_CompanyName);

        for (int i = 0; i < m_SelectedStudent.Count; i++)
        {
            for (int j = 0; j < ObjectManager.Instance.m_StudentList.Count; j++)
            {
                if (m_SelectedStudent[i].m_StudentStat.m_StudentName == ObjectManager.Instance.m_StudentList[j].m_StudentStat.m_StudentName)
                {
                    ObjectManager.Instance.m_StudentList[j].m_StudentStat.m_IsRecommend = true;
                    // ������ �����ϴ°��� �л����� �����Ѵ�.

                    int _randNum = UnityEngine.Random.Range(1, 101);

                    if (_randNum < m_SelectedStudentPercent[i])
                    {
                        m_CompanyList[_companyIndex].EmploymentList[m_NowEmploymentIndex].PassStudent.Add(m_SelectedStudent[i]);
                        m_CompanyList[_companyIndex].PassStudentCount += 1;
                    }
                    else
                    {
                        m_CompanyList[_companyIndex].EmploymentList[m_NowEmploymentIndex].FailStudent.Add(m_SelectedStudent[i]);
                    }

                    m_NowRecommendCount += 1;
                    break;
                }
            }
        }

        // content�� ������ �÷��ֱ�
        m_RecommendInfoPanel.ChangeScrollRectTransform();

        // ��� ������ �Ŀ� ���â�� ��� �л��� �̸� �־��ֱ�
        if (m_NowRecommendCount == 18)
        {
            SetResultPanelStudentPassFail();
            m_RecommendListPanel.LockCloseButton(true);
            m_RecommendInfoPanel.LockInfoPanelCloseButton(true);
        }

        if (m_Alram.activeSelf)
        {
            m_Alram.SetActive(false);
        }

        m_SelectedStudent.Clear();
    }

    // ���â�� ���� �����ߴ� �л����� �̸��� ����ֱ� ���� �л����� �� �а��� �迭�� �̸��� �־��ش�.
    // ���� ������ �л����� �հݷ��� ���� �հ� ���հ� �̹����� �гο� �ִ� �Լ�
    private void SetResultPanelStudentPassFail()
    {
        int _gmIndex = 0;
        int _artIndex = 0;
        int _programmingIndex = 0;

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int j = 0; j < m_CompanyList[i].EmploymentList.Count; j++)
            {
                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].PassStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {
                        GMStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        GMStudentPassFail[_gmIndex] = m_Pass;
                        GMStudentImgae[_gmIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _gmIndex++;
                    }
                    else if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        ArtStudentPassFail[_artIndex] = m_Pass;
                        ArtStudentImgae[_artIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _artIndex++;
                    }
                    else
                    {
                        ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName;
                        ProgrammingStudentPassFail[_programmingIndex] = m_Pass;
                        ProgrammingStudentImgae[_programmingIndex] = m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg;
                        _programmingIndex++;
                    }
                }

                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].FailStudent.Count; k++)
                {

                    if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentType == StudentType.GameDesigner)
                    {
                        GMStudentName[_gmIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        GMStudentPassFail[_gmIndex] = m_Fail;
                        _gmIndex++;
                    }
                    else if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentType == StudentType.Art)
                    {
                        ArtStudentName[_artIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        ArtStudentPassFail[_artIndex] = m_Fail;
                        _artIndex++;
                    }
                    else
                    {
                        ProgrammingStudentName[_programmingIndex] = m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName;
                        ProgrammingStudentPassFail[_programmingIndex] = m_Fail;
                        _programmingIndex++;
                    }
                }
            }
        }

        m_RecommendResultPanel.ResultPanelStudentNameChange(GMStudentName, ArtStudentName, ProgrammingStudentName);
        m_RecommendResultPanel.ResultPanelStudentImageChange(GMStudentImgae, ArtStudentImgae, ProgrammingStudentImgae);
        m_RecommendResultPanel.ResultPanelStudentPassFail(GMStudentPassFail, ArtStudentPassFail, ProgrammingStudentPassFail);
    }

    // ���â���� �л��� Ŭ������ �� ����� ��â
    public void ClickResultPanelStudent()
    {
        GameObject _currentStudent = EventSystem.current.currentSelectedGameObject;
        string _name = _currentStudent.GetComponent<RecommendResultStudentPrefab>().m_StudentName.text;

        m_RecommendResultPanel.PopUpDetailPanel();

        for (int i = 0; i < m_CompanyList.Count; i++)
        {
            for (int j = 0; j < m_CompanyList[i].EmploymentList.Count; j++)
            {
                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].PassStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].PassStudent[k].m_StudentStat.m_StudentName == _name)
                    {
                        m_RecommendResultPanel.SetDetailStudentName(_name);
                        m_RecommendResultPanel.SetDetailPanelPassFailImage(m_DetailPass);
                        m_RecommendResultPanel.SetDetailPanelStudentImage(m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg);
                        m_RecommendResultPanel.SetDetailCompanyName(m_CompanyList[i].EmploymentList[j].EmploymentName);
                        break;
                    }
                }

                for (int k = 0; k < m_CompanyList[i].EmploymentList[j].FailStudent.Count; k++)
                {
                    if (m_CompanyList[i].EmploymentList[j].FailStudent[k].m_StudentStat.m_StudentName == _name)
                    {
                        m_RecommendResultPanel.SetDetailStudentName(_name);   // �л� �̸�
                        m_RecommendResultPanel.SetDetailPanelPassFailImage(m_DetailFail);  // ���հ� �̹���
                        m_RecommendResultPanel.SetDetailPanelStudentImage(m_CompanyList[i].EmploymentList[j].PassStudent[k].StudentProfileImg);
                        m_RecommendResultPanel.SetDetailCompanyName(m_CompanyList[i].EmploymentList[j].EmploymentName);  // ������ �����̸�
                        break;
                    }
                }
            }
        }
    }

    IEnumerator HideImpossibleRecommendPanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        m_RecommendInfoPanel.SetActiveImpossibleRecommendPanel(false);
    }
}

public class Company
{
    public int CompanyID;
    public string CompanyName;
    public int CompanyGrade;
    public int CompanyGameNameID;
    public int PassStudentCount;
    public int CompanyIcon;
    public bool IsNewCompany;
    public bool IsUnLockCompany;
    public List<Employment> EmploymentList;
}

public class Employment
{
    public int EmploymentID;
    public string EmploymentName;
    public string EmploymentText;
    public string CompanyJob;
    public int CompanySalary;
    public int CompanyPart;
    public int[] CompanyRequirementSkill;
    public int[] EmploymentYear;
    public List<Student> PassStudent;
    public List<Student> FailStudent;
    public Dictionary<string, int> CompanyRequirementStats;
}
