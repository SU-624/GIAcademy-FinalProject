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
/// 인게임에서 돌아갈 테스트 클래스. 게임 매니저와 같은 느낌의 스크립트이다.
/// 현재는 싱글톤으로 만들었고 학생을 만드려면 버튼을 누르고 그렇게 만들어진
/// 학생의 정보를 리스트화 시켜 보여주는 버튼도 있다.
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
    public int m_ProfessorTotalSalary = 0;

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
        m_ClassState = ClassState.nothing;

        PlayerInfo.Instance.m_MyMoney = 1500000;      // 맨처음 초기 소지머니     최대머니 : 999,999,999   9억
        PlayerInfo.Instance.m_SpecialPoint = 10000;     //                         최대머니 : 999,999,999   9억

        // 아카데미 인지도, 인재 양성 등등
        PlayerInfo.Instance.m_Awareness = 100;
        PlayerInfo.Instance.m_Management = 100;
        PlayerInfo.Instance.m_TalentDevelopment = 100;
        PlayerInfo.Instance.m_Activity = 100;
        PlayerInfo.Instance.m_Goods = 100;

        PlayerInfo.Instance.m_CurrentRank = Rank.F;

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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

        QuarterlyReport.Instance.Init();
        QuarterlyReport.Instance.AddNewAcademy("NC", "D", "S", "A", "S", "S");
        QuarterlyReport.Instance.AddNewAcademy("Netmarble", "A", "A", "S", "A", "B");
        QuarterlyReport.Instance.AddNewAcademy("Blanc", "S", "D", "D", "S", "B");
        QuarterlyReport.Instance.AddNewAcademy("GI", "B", "S", "B", "A", "S");
        QuarterlyReport.Instance.AddNewAcademy("Woodpie", "A", "A", "S", "D", "A");
        QuarterlyReport.Instance.AddNewAcademy("Ocean", "B", "S", "B", "A", "D");
        QuarterlyReport.Instance.AddNewAcademy("manggugi", "F", "E", "SS", "F", "A");
        QuarterlyReport.Instance.AddNewAcademy("DuoL", "S", "A", "SSS", "C", "S");
        QuarterlyReport.Instance.AddNewAcademy("CockTail", "B", "E", "B", "S", "SS");
        QuarterlyReport.Instance.AddNewAcademy("ThanksLight", "SS", "S", "A", "B", "SSS");
        QuarterlyReport.Instance.AddNewAcademy("ProjectUA", "S", "C", "B", "D", "B");
    }

    private void Update()
    {
        if (m_ClassState == ClassState.ClassStart)
        {
            foreach (var student in ObjectManager.Instance.m_StudentList)
            {
                student.NowRoom = (int)InteractionManager.SpotName.Nothing;
                // isArrivedClass가 false이면 아직 학생들이 도착을 안했다는 뜻.
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
            Time.timeScale = InGameUI.Instance.m_NowGameSpeed; ;
        }

        //if (Input.GetKeyDown(KeyCode.F6))
        //{
        //    GameTime.Instance.Year = 2;
        //    GameTime.Instance.Month = 2;
        //}

    }

    // 버튼을 누르면 2주동안 첫째 주에 m_ClassState가 ClassStart가 될 수 있도록 해주기.
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
            }
        }

        if (_isSelectClassNotNull == true)
        {
            m_ClassState = ClassState.ClassStart;

            foreach (var student in ObjectManager.Instance.m_StudentBehaviorList)
            {
                student.GetComponent<Student>().m_IsDesSetting = false;
            }
            _popOffClassPanel.TurnOffUI();

            // 내 소지금에서 선택한 수업만큼 돈을 빼준다.
            PlayerInfo.Instance.m_MyMoney -= _classPrefab.m_TotalMoney;
            MonthlyReporter.Instance.m_NowMonth.ExpensesTuitionFee += _classPrefab.m_TotalMoney;
            // 교사 월급 계산해주기
            m_ProfessorTotalSalary =_classPrefab.CalculateSalary();
            MonthlyReporter.Instance.m_NowMonth.ExpensesSalary += m_ProfessorTotalSalary;
            _classPrefab.InitSelecteClass();
        }

    }

    // 수업 선택이 끝나고 다음달이 되면 실행시켜줄 함수.
    public void NextClassStart()
    {
        m_ClassState = ClassState.ClassStart;

        foreach (var student in ObjectManager.Instance.m_StudentBehaviorList)
        {
            student.GetComponent<Student>().m_IsDesSetting = false;
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

    // 수업이 끝나면 상태를 수업 종료로 바꿔주고 모든 학생들의 상태는 FreeWalk가 가능한 상태로 만들어준다.
    // 여기서 학생들의 스탯을 수업들은 만큼 올려준다.
    void EndClass()
    {
        m_ClassState = ClassState.ClassEnd;

        m_ClassResultPopUp.TurnOnUI();          // 수업이 끝나면 일단 수업 결과창부터 띄워준다.
        m_ClassResult.GetComponent<ClassResult>().enabled = true;

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

        StudentDataChangedEvent?.Invoke();

        Invoke("StateInit", 4f);
    }

    // 방금 들은 수업의 정보를 지워준다.
    void StateInit()
    {
        PlayerInfo.Instance.IsFirstClassEnd = true;
        m_ClassState = ClassState.nothing;
        m_ClassResult.GetComponent<ClassResult>().enabled = false;
    }

}


