using UnityEngine;
using StatData.Runtime;
using Conditiondata.Runtime;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 각 학생들이 가지고 있어야하는 정보들을 담은 클래스
/// 
/// 2022.11.04
/// </summary>
public class Student : MonoBehaviour
{
    public enum Doing
    {
        FreeWalk,
        AtRest,
        Studying,
        StartInteracting,
        EndInteracting,
    }

    public StudentStat m_StudentData;
    public StudentCondition m_StudentCondition;

    private Doing m_Doing;
    public Doing DoingValue { get => m_Doing;
        set{
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
                    m_IsCoolDown = true;   // 쿨타임 시작
                }
                break;
            }
        }
    }
    //public Node m_Node;

    public float m_Time = 0f;       // 쿨타임의 타이머
    public float m_CoolTime;   // 멈추길 원하는 만큼의 시간

    public string m_NameStudent;

    public bool m_IsArrivedClass;
    public bool m_IsDesSetting;
    public bool m_IsInteracting;
    public bool m_IsCoolDown;           // 쿹타임 중인가 판별
    public bool m_IsDialoguePlaying;
    public int m_RestaurantNumOfPeople;

    public string m_Roll;

    public Queue<string> m_DestinationQueue = new Queue<string>();

    private void Awake()
    {
        m_CoolTime = 10f;
        m_IsArrivedClass = false;
        m_IsDesSetting = false;
        m_IsInteracting = false;
        m_IsCoolDown = false;
        m_IsDialoguePlaying = false;
    }

    void Update()
    {
        switch (m_Doing)
        {
            // 쿨타임 적용할 부분
            case Doing.EndInteracting:
            {
                m_Time += Time.deltaTime;

                if (m_Time > m_CoolTime)
                {
                    m_Time = 0f;
                    m_IsCoolDown = false;    // 쿨타임 끝남.
                    m_IsInteracting = false;
                    DoingValue = Doing.FreeWalk;
                }
            }
            break;
        }
    }


    public void Initialize(StudentStat _stat, string _name) //, StudentCondition _studentCondition
    {
        m_Doing = Doing.FreeWalk;
        m_NameStudent = _name;
        gameObject.name = _name;
        m_StudentData = _stat;
    }
}

