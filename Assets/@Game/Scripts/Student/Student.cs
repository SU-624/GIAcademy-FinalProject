using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// �� �л����� ������ �־���ϴ� �������� ���� Ŭ����
/// 
/// 2022.11.04
/// </summary>
public class Student : MonoBehaviour
{
    [SerializeField] private Animator m_StudentAnimator;
    private int m_RandomNumTalkAniIndex;

    public enum Doing
    {
        FreeWalk,
        AtRest,
        Studying,
        StartInteracting,
        EndInteracting,
        InGenreRoom,
        InFacility,
        Vacation,
        ObjectInteraction,
        Idle
    }

    // �л��� ��� ������ ����ִ� ����
    public StudentStat m_StudentStat;
    public GameObject ChatObject;
    public Transform LookTransform;

    private Doing m_Doing;
    public Doing DoingValue
    {
        get => m_Doing;
        set
        {
            m_Doing = value;

            switch (value)
            {

                case Doing.InGenreRoom:
                    {

                    }
                    break;

                case Doing.InFacility:
                    {

                    }
                    break;

                case Doing.ObjectInteraction:
                    {

                    }
                    break;

                case Doing.Idle:
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

                // ��ȣ�ۿ� ������ �κ�
                case Doing.StartInteracting:
                    {
                        m_IsCoolDown = true;   // ��Ÿ�� ����
                    }
                    break;

                case Doing.Vacation:
                    {
                        //Destroy(this.gameObject);
                        // ������Ʈ�� ��ȯ�� �� ���� �̸����� �ٽ� �ٲ��� ���� ��ȯ��Ų��.
                        this.gameObject.name = m_StudentStat.m_StudentID;
                        this.gameObject.GetComponent<BehaviorTree>().ExternalBehavior = null;
                        ObjectManager.Instance.m_StudentList.Remove(this);

                        if (ChatObject != null)
                        {
                            InGameObjectPool.ReturnChatObject(ChatObject);
                        }

                        if (m_StudentStat.m_Gender == 1)
                        {
                            InGameObjectPool.ReturnMaleStudentObject(this.gameObject);
                        }
                        else
                        {
                            InGameObjectPool.ReturnFemaleStudentObject(this.gameObject);
                        }
                    }
                    break;
            }
        }
    }

    // Student Stat �ȿ� �ִ� �̸�����.
    // public string m_NameStudent;

    // about BT
    public int UsingObjNum;
    public bool m_IsArrivedClass;
    public bool m_IsDesSetting;
    public bool m_IsInteracting;
    public bool m_IsCoolDown;           // �lŸ�� ���ΰ� �Ǻ�
    public int m_NowSeatNum;

    public GameObject InteractingObj;

    private const float m_minusHealth = 20f;
    private const float m_speedScale = 0.03f;

    private int m_NowMonth;
    private float m_doingResetTimer;

    public int NowRoom = (int)InteractionManager.SpotName.Nothing;

    // �ӽ� �׽�Ʈ
    public Sprite StudentProfileImg;

    private void Awake()
    {
        m_IsArrivedClass = false;
        m_IsDesSetting = false;
        m_IsInteracting = false;
        m_IsCoolDown = true;
        m_NowSeatNum = 0;
        m_NowMonth = 3;
        NowRoom = (int)InteractionManager.SpotName.Nothing;
    }

    void Update()
    {
        if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 3 &&
            GameTime.Instance.FlowTime.NowDay == 5)
        {
            DoingValue = Doing.FreeWalk;
        }

        switch (m_Doing)
        {
            case Doing.FreeWalk:
                {
                    if (InGameTest.Instance.m_ClassState != ClassState.ClassStart)
                    {
                        //if (m_StudentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idel_1"))
                        //{
                        //    m_StudentAnimator.SetBool("IsWalkToIdel", false);
                        //}
                        //else if (m_StudentAnimator.GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
                        //{
                        //    m_StudentAnimator.SetBool("IsWalkToSit", false);
                        //}
                        //else if (m_StudentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talking1") || m_StudentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talking3"))
                        //{
                        //    m_StudentAnimator.SetBool("IsWalkToTalk", false);
                        //    m_StudentAnimator.SetInteger("TalkAniNumber", m_RandomNumTalkAniIndex);
                        //}
                        //else if (m_StudentAnimator.GetCurrentAnimatorStateInfo(0).IsName("sittingIdel"))
                        //{
                        //    m_StudentAnimator.SetBool("IsWalkToSit", false);
                        //}
                    }
                }
                break;

            case Doing.InGenreRoom:
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(LookTransform.forward),
                        Time.deltaTime * 4f);
                }
                break;

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

            // ��Ÿ�� ������ �κ�
            case Doing.EndInteracting:
                {
                    InteractingObj = null;
                    m_Doing = Doing.FreeWalk;
                    StartCoroutine(CoolTimeStart());
                }
                break;
        }

        if (InGameTest.Instance.m_ClassState == ClassState.ClassEnd)
        {
            m_IsArrivedClass = false;
        }

        // �л� ü�� 0 ������ �ȶ��������� ����
        if (m_StudentStat.m_Health <= 0)
        {
            m_StudentStat.m_Health = 0;
        }

        if (m_StudentStat.m_Health >= 100)
        {
            m_StudentStat.m_Health = 100;
        }

        if (m_StudentStat.m_Passion <= 0)
        {
            m_StudentStat.m_Passion = 0;
        }

        if (m_StudentStat.m_Passion >= 100)
        {
            m_StudentStat.m_Passion = 100;
        }

        // ü�¿����� �л� �ӵ� ����
        gameObject.GetComponent<NavMeshAgent>().speed = 2.5f + (m_StudentStat.m_Health - m_minusHealth) * m_speedScale;

        if (m_IsArrivedClass)
        {
            Transform lookTransform = (Transform)gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("SeatTransform").GetValue();
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookTransform.forward),
                Time.deltaTime * 10);
        }
    }
    
    public void Initialize(StudentStat _stat) //, StudentCondition _studentCondition
    {
        m_Doing = Doing.EndInteracting;
        m_StudentStat = _stat;
        m_StudentStat.m_StudentID = gameObject.name;
        m_StudentStat.m_UserSettingName = "";
        gameObject.name = _stat.m_StudentName;
    }

    void OnTriggerEnter(Collider other)
    {
        if (InGameTest.Instance == null || InGameTest.Instance.m_ClassState != ClassState.nothing || other.tag != "Interacting")
            return;

        if (GameTime.Instance.FlowTime.NowMonth == 2 && GameTime.Instance.FlowTime.NowWeek == 4)
            return;

        if (Vector3.Distance(this.transform.position, other.transform.position) < 3)
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
                if (RandomSeed(CheckProfFriendship(nowObject.GetComponent<Instructor>())))
                {
                    InteractingObj = nowObject;
                    nowObject.GetComponent<Instructor>().InteractingObj = this.gameObject;
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

        //Debug.Log(gameObject.name + " ��Ÿ�� �ʱ�ȭ");
        m_IsCoolDown = false;    // ��Ÿ�� ����.
    }

    public IEnumerator ClassEndCoolTime()
    {
        yield return new WaitForSeconds(10f);

        m_IsCoolDown = false;    // ��Ÿ�� ����.
    }

    private int CheckFriendship(Student otherStudent)
    {
        int myIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(this.gameObject.GetComponent<Student>()));
        int otherIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(otherStudent));
        int friendShip = ObjectManager.Instance.m_Friendship[myIndex][otherIndex];

        return friendShip;
    }

    private int CheckProfFriendship(Instructor otherProfessor)
    {
        int myIndex = ObjectManager.Instance.m_StudentList.FindIndex(x => x.Equals(this.gameObject.GetComponent<Student>()));
        int otherIndex = ObjectManager.Instance.m_InstructorList.FindIndex(x => x.Equals(otherProfessor));
        int friendShip = ObjectManager.Instance.m_Friendship[myIndex][18 + otherIndex];

        return friendShip;
    }
}

