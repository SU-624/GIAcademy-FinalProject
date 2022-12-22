using UnityEngine;
using StatData.Runtime;
using Conditiondata.Runtime;
using BT;

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
        Study,
        GoTo,
        Wait,
        Eat,
        AtRest,
    }

    public StudentStat m_StudentData;
    public StudentCondition m_StudentCondition;
    public Doing m_Doing { get; set; }
    public Node m_Node;

    public float m_Time = 0f;
    public float m_CoolTime = 1f;

    public string m_NameStudent;

    public bool isHereRestaurant = false;

    public int m_RestaurantNumOfPeople;

    void Update()
    {
        m_Time += Time.deltaTime;

        if (m_Time > m_CoolTime)
        {
            m_Time = 0f;
            //Debug.Log(m_Node);
            m_Node.Run();
        }
    }

    public void Initialize(StudentStat _stat ,string _name, StudentCondition _studentCondition, Node _node)
    {
        m_Doing = Doing.FreeWalk;
        m_NameStudent = _name;
        gameObject.name = _name;
        m_StudentCondition = _studentCondition;
        m_Node = _node;
        m_StudentData = _stat;
        m_StudentData.m_StudentSystemValue = _stat.m_StudentSystemValue;
        m_StudentData.m_StudentContentsValue = _stat.m_StudentContentsValue;
        m_StudentData.m_StudentBalanceValue = _stat.m_StudentBalanceValue;
    }
}

