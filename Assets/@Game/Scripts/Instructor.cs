using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using StatData.Runtime;
using UnityEngine.AI;


/// <summary>
/// 각 강사들이 가지고 있어야 하는 정보들을 담은 클래스
///
/// </summary>
public class Instructor : MonoBehaviour
{
    public enum Doing
    {
        FreeWalk,
        AtRest,
        Studying,
        StartInteracting,
        EndInteracting,
        InFacility,
        ObjectInteraction,
        Vacation,
        Idle
    }

    public GameObject InteractingObj;
    public ProfessorStat m_InstructorData;
    private List<ProfessorStat> m_professor = new List<ProfessorStat>();

    [SerializeField] private Transform m_artEntrance;
    [SerializeField] private Transform m_artProfessorSeat;
    [SerializeField] private Transform m_programmingEntrance;
    [SerializeField] private Transform m_programmingProfessorSeat;
    [SerializeField] private Transform m_productManagerEntrance;
    [SerializeField] private Transform m_productManagerProfessorSeat;
    [SerializeField] private ExternalBehaviorTree m_professorTree;
    // 임시 강사 이미지 넣기
    [SerializeField] public Sprite m_TeacherImg;

    public int UsingObjNum;
    public bool m_IsInteracting;
    public bool m_IsCoolDown;           // 쿹타임 중인가 판별
    public bool m_IsArrivedClass;
    public bool m_IsDesSetting;
    private Doing m_Doing;
    public Transform LookTransform;

    public Doing DoingValue
    {
        get => m_Doing;
        set
        {
            m_Doing = value;

            switch (value)
            {
                case Doing.FreeWalk:
                    {

                    }
                    break;

                case Doing.AtRest:
                    {

                    }
                    break;

                case Doing.Studying:
                    {

                    }
                    break;

                // 상호작용 시작할 부분
                case Doing.StartInteracting:
                    {
                        //m_IsCoolDown = true;   // 쿨타임 시작
                    }
                    break;
            }
        }
    }

    private const float m_minusHealth = 20f;
    private const float m_speedScale = 0.03f;

    public int NowRoom = (int)InteractionManager.SpotName.Nothing;

    void Update()
    {
        if (m_InstructorData == null)
        {
            return;
        }

        switch (m_Doing)
        {
            case Doing.InFacility:
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(LookTransform.forward),
                        Time.deltaTime * 4f);
                }
                break;

            case Doing.ObjectInteraction:
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(LookTransform.forward),
                        Time.deltaTime * 4f);
                }
                break;

            // 쿨타임 적용할 부분
            case Doing.EndInteracting:
                {
                    //InteractingObj = null;
                    m_Doing = Doing.FreeWalk;
                    StartCoroutine(CoolTimeStart());
                }
                break;
        }

        // 체력 0 밑으로 안떨어지도록 조정
        if (m_InstructorData.m_ProfessorHealth <= 0)
        {
            m_InstructorData.m_ProfessorHealth = 0;
        }

        if (m_InstructorData.m_ProfessorHealth >= 100)
        {
            m_InstructorData.m_ProfessorHealth = 100;
        }

        if (m_InstructorData.m_ProfessorPassion <= 0)
        {
            m_InstructorData.m_ProfessorPassion = 0;
        }

        if (m_InstructorData.m_ProfessorPassion >= 100)
        {
            m_InstructorData.m_ProfessorPassion = 100;
        }

        // 체력에따른 속도 조정
        gameObject.GetComponent<NavMeshAgent>().speed = 2.5f + (m_InstructorData.m_ProfessorHealth - m_minusHealth) * m_speedScale;

        if (InGameTest.Instance.m_ClassState == ClassState.ClassEnd)
        {
            m_IsArrivedClass = false;
        }

        if (m_IsArrivedClass)
        {
            Transform lookTransform = (Transform)gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("SeatTransform").GetValue();
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookTransform.forward),
                Time.deltaTime * 10);
        }

        if (GameTime.Instance.Month == 2 && GameTime.Instance.Week == 4 && GameTime.Instance.Day == 5 &&
            DoingValue == Doing.Vacation)
        {
            DoingValue = Doing.FreeWalk;
        }
    }

    public void Initialize(ProfessorStat stat)
    {
        this.name = stat.m_ProfessorName;
        //gameObject = professorTag.m_ProfessorName;

        m_InstructorData = stat;
        // m_InstructorData.m_ProfessorName = stat.m_ProfessorName;
        // m_InstructorData.m_ProfessorType = stat.m_ProfessorType;
        // m_InstructorData.m_ProfessorPassion = stat.m_ProfessorPassion;
        // m_InstructorData.m_ProfessorHealth = stat.m_ProfessorHealth;
        // m_InstructorData.m_ProfessorPower = stat.m_ProfessorPower;

        this.gameObject.GetComponent<BehaviorTree>().StartWhenEnabled = true;
        this.gameObject.GetComponent<BehaviorTree>().PauseWhenDisabled = true;
        this.gameObject.GetComponent<BehaviorTree>().RestartWhenComplete = true;

        this.gameObject.GetComponent<BehaviorTree>().DisableBehavior();

        ExternalBehavior professorTreeInstance = Instantiate(m_professorTree);
        professorTreeInstance.Init();

        if (m_InstructorData.m_ProfessorType == StudentType.Art)
        {
            professorTreeInstance.SetVariableValue("MyClassEntrance", m_artEntrance.position);
            professorTreeInstance.SetVariableValue("MyClassSeat", m_artProfessorSeat.position);
            professorTreeInstance.SetVariableValue("SeatTransform", m_artProfessorSeat);
        }
        else if (m_InstructorData.m_ProfessorType == StudentType.Programming)
        {
            professorTreeInstance.SetVariableValue("MyClassEntrance", m_programmingEntrance.position);
            professorTreeInstance.SetVariableValue("MyClassSeat", m_programmingProfessorSeat.position);
            professorTreeInstance.SetVariableValue("SeatTransform", m_programmingProfessorSeat);
        }
        else if (m_InstructorData.m_ProfessorType == StudentType.GameDesigner)
        {
            professorTreeInstance.SetVariableValue("MyClassEntrance", m_productManagerEntrance.position);
            professorTreeInstance.SetVariableValue("MyClassSeat", m_productManagerProfessorSeat.position);
            professorTreeInstance.SetVariableValue("SeatTransform", m_productManagerProfessorSeat);
        }

        this.gameObject.GetComponent<BehaviorTree>().ExternalBehavior = professorTreeInstance;
        this.gameObject.GetComponent<BehaviorTree>().EnableBehavior();

        Debug.Log(m_InstructorData);

        m_professor.Add(m_InstructorData);
    }


    void OnTriggerEnter(Collider other)
    {
        if (InGameTest.Instance == null || InGameTest.Instance.m_ClassState != ClassState.nothing || other.tag != "Interacting")
            return;

        GameObject nowObject = other.transform.parent.gameObject;
        bool otherCoolDown = false;
        GameObject otherInterObj;
        bool otherInteracting = false;

        if (nowObject.tag == "Student")
        {
            otherCoolDown = nowObject.GetComponent<Student>().m_IsCoolDown;
            otherInterObj = nowObject.GetComponent<Student>().InteractingObj;
            otherInteracting = nowObject.GetComponent<Student>().m_IsInteracting;
        }
        else
        {
            otherCoolDown = nowObject.GetComponent<Instructor>().m_IsCoolDown;
            otherInterObj = nowObject.GetComponent<Instructor>().InteractingObj;
            otherInteracting = nowObject.GetComponent<Instructor>().m_IsInteracting;
        }

        if (m_IsCoolDown || otherCoolDown)
        {
            return;
        }

        if (m_Doing != Doing.FreeWalk ||
            (nowObject.tag == "Student"
                ? (int)nowObject.GetComponent<Student>().DoingValue
                : (int)nowObject.GetComponent<Instructor>().DoingValue) != (int)Doing.FreeWalk)
        {
            return;
        }

        if (NowRoom != (int)InteractionManager.SpotName.Nothing || (nowObject.tag == "Student"
                ? (int)nowObject.GetComponent<Student>().NowRoom
                : (int)nowObject.GetComponent<Instructor>().NowRoom) != (int)InteractionManager.SpotName.Nothing)
        {
            return;
        }

        if (m_IsInteracting == false && otherInteracting == false && InteractingObj == null && otherInterObj == null)
        {
            if (nowObject.tag == "Student")
            {
                if (RandomSeed(CheckFriendship(nowObject.GetComponent<Student>())))
                {
                    InteractingObj = nowObject;
                    nowObject.GetComponent<Student>().InteractingObj = this.gameObject;
                }
            }
            else
            {
                int randomValue = Random.Range(1, 101);
                if (randomValue <= 20)
                {
                    InteractingObj = nowObject;
                    nowObject.GetComponent<Instructor>().InteractingObj = this.gameObject;
                }
                else
                {
                    m_IsCoolDown = true;
                    DoingValue = Doing.EndInteracting;
                }
            }
        }
    }

    private bool RandomSeed(int friendShip)
    {
        int randomValue = Random.Range(1, 101);
        int successProbability = 0;

        if (friendShip < 150)
        {
            successProbability = 15;
        }
        else if (friendShip < 300)
        {
            successProbability = 20;
        }
        else
        {
            successProbability = 25;
        }

        if (randomValue <= successProbability)
        {
            return true;
        }

        //m_IsCoolDown = true;
        //DoingValue = Doing.EndInteracting;
        return false;
    }

    IEnumerator CoolTimeStart()
    {
        yield return new WaitForSeconds(5f);

        //Debug.Log(gameObject.name + " 쿨타임 초기화");
        m_IsCoolDown = false;    // 쿨타임 끝남.
        InteractingObj = null;
    }

    private int CheckFriendship(Student otherStudent)
    {
        int myIndex = ObjectManager.Instance.m_InstructorList.FindIndex(x => x.Equals(this.gameObject.GetComponent<Instructor>()));
        int otherIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(otherStudent));
        int friendShip = ObjectManager.Instance.m_Friendship[otherIndex][18 + myIndex];

        return friendShip;
    }
}
