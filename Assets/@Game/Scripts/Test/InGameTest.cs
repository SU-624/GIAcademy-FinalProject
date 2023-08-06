using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;
using StatData.Runtime;
using System;

public enum ClassState
{
    nothing,
    ClassStart,
    ClassEnd,
    StudyStart,
    Studying
}

/// <summary>
/// �ΰ��ӿ��� ���ư� �׽�Ʈ Ŭ����. ���� �Ŵ����� ���� ������ ��ũ��Ʈ�̴�.
/// ����� �̱������� ������� �л��� ������� ��ư�� ������ �׷��� �������
/// �л��� ������ ����Ʈȭ ���� �����ִ� ��ư�� �ִ�.
/// 
/// 2022.11.04
/// </summary>
/// 
public class InGameTest : MonoBehaviour
{
    private static InGameTest _instance = null;

    public delegate void StudentDataChangedEventHandler();

    public static event StudentDataChangedEventHandler StudentDataChangedEvent;

    [SerializeField] private PopUpUI m_ClassResultPopUp;
    [SerializeField] private GameObject m_ClassResult;

    [SerializeField] private SelectClass _classPrefab;
    [SerializeField] private ApplyChangeStat m_ChangeProfessorStat;
    [SerializeField] private PopOffUI _popOffClassPanel;

    public List<string> _interactionScript;

    public ClassState m_ClassState = ClassState.nothing;

    public bool _isSelectClassNotNull = false;

    public List<Vector3> FreeWalkPointList = new List<Vector3>();
    private List<RankTable> m_AIAcademyRankList = new List<RankTable>();
    private List<AIAcademyInfo> m_AIData = new List<AIAcademyInfo>();

    private bool testCheck;
    private bool testCheck2;
    private bool testCheck3;

    private bool m_isStudentArrived;
    private bool m_isProfessorArrived;

    public static InGameTest Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // ���� �����Ͱ� ���ٸ� �ʱ⼳��
        if (!Json.Instance.UseLoadingData)
        {
            m_ClassState = ClassState.nothing;

            PlayerInfo.Instance.MyMoney = 100000; // ��ó�� �ʱ� �����Ӵ�     �ִ�Ӵ� : 999,999,999   9��
            PlayerInfo.Instance.SpecialPoint = 200; //                         �ִ�Ӵ� : 999,999,999   9��

            // ��ī���� ������, ���� �缺 ���
            PlayerInfo.Instance.Famous = 100;
            PlayerInfo.Instance.Management = 100;
            PlayerInfo.Instance.TalentDevelopment = 100;
            PlayerInfo.Instance.Activity = 100;
            PlayerInfo.Instance.Goods = 100;

            PlayerInfo.Instance.CurrentRank = Rank.F;
        }

        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject[] freewalkPoint = GameObject.FindGameObjectsWithTag("Rest");
        for (int i = 0; i < freewalkPoint.Length; i++)
        {
            FreeWalkPointList.Add(freewalkPoint[i].transform.position);
        }

        testCheck = false;
        testCheck2 = false;
        testCheck3 = false;

        InitAIAcademyRankList();
        MakeAIAcademy();
    }

    private void Update()
    {
        if (m_ClassState == ClassState.ClassStart)
        {
            foreach (var student in ObjectManager.Instance.m_StudentList)
            {
                student.NowRoom = (int)InteractionManager.SpotName.Nothing;
                // isArrivedClass�� false�̸� ���� �л����� ������ ���ߴٴ� ��.
                if (student.m_IsArrivedClass == false)
                {
                    m_isStudentArrived = false;
                    break;
                }
                //student.gameObject.GetComponent<Animator>().SetBool("isStudying", true);

                m_isStudentArrived = true;
            }

            foreach (var professor in ObjectManager.Instance.m_InstructorList)
            {
                if (professor.m_IsArrivedClass == false)
                {
                    m_isProfessorArrived = false;
                    break;
                }

                //professor.gameObject.GetComponent<Animator>().SetBool("isStudying", true);
                m_isProfessorArrived = true;
            }
        }

        if (m_isStudentArrived && m_isProfessorArrived)
        {
            m_ClassState = ClassState.StudyStart;
        }

        if (m_ClassState == ClassState.StudyStart)
        {
            m_isStudentArrived = false;
            m_isProfessorArrived = false;

            StartStudy();
        }

        if (m_ClassState == ClassState.Studying)
        {
            if (GameTime.Instance.FlowTime.NowWeek == 3)
            {
                EndClass();
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = InGameUI.Instance.m_NowGameSpeed;
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            GameTime.Instance.Year = 2;
            GameTime.Instance.Month = 2;
            GameTime.Instance.Week = 1;
            PlayerInfo.Instance.IsFirstGameJam = false;
            PlayerInfo.Instance.IsFirstGameShow = false;
            PlayerInfo.Instance.IsFirstClassSetting = false;
        }
    }

    private void InitAIAcademyRankList()
    {
        m_AIAcademyRankList.Add(new RankTable(Rank.SSS, 320, 400));
        m_AIAcademyRankList.Add(new RankTable(Rank.SS, 280, 360));
        m_AIAcademyRankList.Add(new RankTable(Rank.S, 240, 320));
        m_AIAcademyRankList.Add(new RankTable(Rank.A, 200, 280));
        m_AIAcademyRankList.Add(new RankTable(Rank.B, 160, 240));
        m_AIAcademyRankList.Add(new RankTable(Rank.C, 120, 200));
        m_AIAcademyRankList.Add(new RankTable(Rank.D, 80, 160));
        m_AIAcademyRankList.Add(new RankTable(Rank.E, 40, 120));
        m_AIAcademyRankList.Add(new RankTable(Rank.F, 0, 80));
    }

    private void MakeAIAcademy()
    {
        QuarterlyReport.Instance.Init();

        foreach (AIAcademyInfo info in AllOriginalJsonData.Instance.OriginalAIAcademyInfoData)
        {
            string _money = SearchRankForData(info.MoneyScore).ToString();
            string _famous = SearchRankForData(info.FamousScore).ToString();
            string _activity = SearchRankForData(info.ActivityScore).ToString();
            string _managing = SearchRankForData(info.ManagingScore).ToString();
            string _education = SearchRankForData(info.EducationScore).ToString();

            QuarterlyReport.Instance.AddNewAcademy(info.Name, _money, _famous, _activity, _managing, _education);
        }
    }

    private Rank SearchRankForData(int _value)
    {
        foreach (RankTable info in m_AIAcademyRankList)
        {
            if (info.MaxValue < _value && _value > info.MinValue)
            {
                return info.Grade;
            }
        }

        return Rank.None;
    }

    // ��ư�� ������ 2�ֵ��� ù° �ֿ� m_ClassState�� ClassStart�� �� �� �ֵ��� ���ֱ�.
    public void StarClass()
    {
        _isSelectClassNotNull = false;

        for (int i = 0; i < 2; i++)
        {
            if (SelectClass.m_ArtData[i].m_SelectClassDataSave == null ||
                SelectClass.m_GameDesignerData[i].m_SelectClassDataSave == null ||
                SelectClass.m_ProgrammingData[i].m_SelectClassDataSave == null)
            {
                _isSelectClassNotNull = false;
                break;
            }
            else
            {
                _isSelectClassNotNull = true;
                AssignClassDataToDepartmentDictionary(SelectClass.m_GameDesignerData[i].m_SelectClassDataSave.m_ClassStatType, SelectClass.m_GameDesignerData[i].m_SelectClassDataSave);
                AssignClassDataToDepartmentDictionary(SelectClass.m_ArtData[i].m_SelectClassDataSave.m_ClassStatType, SelectClass.m_ArtData[i].m_SelectClassDataSave);
                AssignClassDataToDepartmentDictionary(SelectClass.m_ProgrammingData[i].m_SelectClassDataSave.m_ClassStatType, SelectClass.m_ProgrammingData[i].m_SelectClassDataSave);
            }
        }

        if (_isSelectClassNotNull == true)
        {
            m_ClassState = ClassState.ClassStart;

            foreach (var student in ObjectManager.Instance.m_StudentList)
            {
                student.m_IsDesSetting = false;
            }

            _popOffClassPanel.TurnOffUI();

            // �� �����ݿ��� ������ ������ŭ ���� ���ش�.
            PlayerInfo.Instance.MyMoney -= _classPrefab.m_TotalMoney;
            MonthlyReporter.Instance.m_NowMonth.ExpensesTuitionFee += _classPrefab.m_TotalMoney;
            // ���� ���� ������ֱ�
            //m_ProfessorTotalSalary =_classPrefab.CalculateSalary();
            //MonthlyReporter.Instance.m_NowMonth.ExpensesSalary += m_ProfessorTotalSalary;
            _classPrefab.InitSelecteClass();

            // �̹� �� ������ ���� �ܷ������ ü���� ȸ���ȴ�.
            foreach (var professor in Professor.Instance.GameManagerProfessor)
            {
                bool isTeaching = false;
                if (professor.m_ProfessorSet == "�ܷ�")
                {
                    for (var i = 0; i < 2; i++)
                    {
                        if (professor == SelectClass.m_GameDesignerData[i].m_SelectProfessorDataSave)
                        {
                            isTeaching = true;
                            break;
                        }
                    }

                    if (!isTeaching)
                    {
                        professor.m_ProfessorHealth += 5;
                        if (professor.m_ProfessorHealth > 100)
                        {
                            professor.m_ProfessorHealth = 100;
                        }
                    }
                }
            }

            foreach (var professor in Professor.Instance.ArtProfessor)
            {
                bool isTeaching = false;
                if (professor.m_ProfessorSet == "�ܷ�")
                {
                    for (var i = 0; i < 2; i++)
                    {
                        if (professor == SelectClass.m_ArtData[i].m_SelectProfessorDataSave)
                        {
                            isTeaching = true;
                            break;
                        }
                    }

                    if (!isTeaching)
                    {
                        professor.m_ProfessorHealth += 5;
                        if (professor.m_ProfessorHealth > 100)
                        {
                            professor.m_ProfessorHealth = 100;
                        }
                    }
                }
            }

            foreach (var professor in Professor.Instance.ProgrammingProfessor)
            {
                bool isTeaching = false;
                if (professor.m_ProfessorSet == "�ܷ�")
                {
                    for (var i = 0; i < 2; i++)
                    {
                        if (professor == SelectClass.m_ProgrammingData[i].m_SelectProfessorDataSave)
                        {
                            isTeaching = true;
                            break;
                        }
                    }

                    if (!isTeaching)
                    {
                        professor.m_ProfessorHealth += 5;
                        if (professor.m_ProfessorHealth > 100)
                        {
                            professor.m_ProfessorHealth = 100;
                        }
                    }
                }
            }
        }
    }

    // ���� ������ ������ �������� �Ǹ� ��������� �Լ�.
    public void NextClassStart()
    {
        m_ClassState = ClassState.ClassStart;

        foreach (var student in ObjectManager.Instance.m_StudentList)
        {
            student.m_IsDesSetting = false;
        }
    }

    void StartStudy()
    {
        m_ClassState = ClassState.Studying;
        foreach (var student in ObjectManager.Instance.m_StudentList)
        {
            student.DoingValue = Student.Doing.Studying;
        }
    }

    // ������ ������ ���¸� ���� ����� �ٲ��ְ� ��� �л����� ���´� FreeWalk�� ������ ���·� ������ش�.
    // ���⼭ �л����� ������ �������� ��ŭ �÷��ش�.
    void EndClass()
    {
        m_ClassState = ClassState.ClassEnd;

        m_ClassResultPopUp.TurnOnUI(); // ������ ������ �ϴ� ���� ���â���� ����ش�.
        //m_ClassResult.GetComponent<ClassResult>().enabled = true;

        foreach (var student in ObjectManager.Instance.m_StudentList)
        {
            student.m_IsArrivedClass = false;
            student.m_IsDesSetting = false;
            student.m_IsInteracting = false;
            student.GetComponent<NavMeshAgent>().isStopped = false;
            //student.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }

        foreach (var professor in ObjectManager.Instance.m_InstructorList)
        {
            professor.m_IsArrivedClass = false;
            professor.m_IsDesSetting = false;
            professor.m_IsInteracting = false;
            professor.GetComponent<NavMeshAgent>().isStopped = false;
            //professor.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }

        m_ChangeProfessorStat.ApplyProfessorStat();
        m_ChangeProfessorStat.ApplyStudentStat();


        Invoke("StateInit", 6f);
    }

    // ��� ���� ������ ������ �����ش�.
    void StateInit()
    {
        PlayerInfo.Instance.IsFirstClassEnd = true;
        m_ClassState = ClassState.nothing;
        //m_ClassResult.GetComponent<ClassResult>().enabled = false;
        StudentDataChangedEvent?.Invoke();
    }

    private void AssignClassDataToDepartmentDictionary(ClassType _classType, Class _classData)
    {
        switch (_classType)
        {
            case ClassType.Class:
            {
                if (_classData.m_ClassType == StudentType.GameDesigner)
                {
                    if (!PlayerInfo.Instance.GameDesignerClassDic.ContainsKey(_classData.m_ClassID))
                    {
                        PlayerInfo.Instance.GameDesignerClassDic.Add(_classData.m_ClassID, 1);
                    }
                    else
                    {
                        PlayerInfo.Instance.GameDesignerClassDic[_classData.m_ClassID] += 1;
                    }
                }
                else if (_classData.m_ClassType == StudentType.Art)
                {
                    if (!PlayerInfo.Instance.ArtClassDic.ContainsKey(_classData.m_ClassID))
                    {
                        PlayerInfo.Instance.ArtClassDic.Add(_classData.m_ClassID, 1);
                    }
                    else
                    {
                        PlayerInfo.Instance.ArtClassDic[_classData.m_ClassID] += 1;
                    }
                }
                else if (_classData.m_ClassType == StudentType.Programming)
                {
                    if (!PlayerInfo.Instance.ProgrammingClassDic.ContainsKey(_classData.m_ClassID))
                    {
                        PlayerInfo.Instance.ProgrammingClassDic.Add(_classData.m_ClassID, 1);
                    }
                    else
                    {
                        PlayerInfo.Instance.ProgrammingClassDic[_classData.m_ClassID] += 1;
                    }
                }
            }
            break;

            case ClassType.Commonm:
            {
                if (!PlayerInfo.Instance.GameDesignerClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.GameDesignerClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.GameDesignerClassDic[_classData.m_ClassID] += 1;
                }

                if (!PlayerInfo.Instance.ArtClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.ArtClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.ArtClassDic[_classData.m_ClassID] += 1;
                }

                if (!PlayerInfo.Instance.ProgrammingClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.ProgrammingClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.ProgrammingClassDic[_classData.m_ClassID] += 1;
                }
            }
            break;

            case ClassType.Special:
            {
                if (!PlayerInfo.Instance.GameDesignerClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.GameDesignerClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.GameDesignerClassDic[_classData.m_ClassID] += 1;
                }

                if (!PlayerInfo.Instance.ArtClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.ArtClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.ArtClassDic[_classData.m_ClassID] += 1;
                }

                if (!PlayerInfo.Instance.ProgrammingClassDic.ContainsKey(_classData.m_ClassID))
                {
                    PlayerInfo.Instance.ProgrammingClassDic.Add(_classData.m_ClassID, 1);
                }
                else
                {
                    PlayerInfo.Instance.ProgrammingClassDic[_classData.m_ClassID] += 1;
                }
            }
            break;
        }
    }
}


