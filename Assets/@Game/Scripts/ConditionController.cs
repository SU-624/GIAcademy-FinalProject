using System.Collections.Generic;
using UnityEngine;
using StatData.Runtime;
using Conditiondata.Runtime;

/// <summary>
/// 만들어진 컨디션 ScriptableObject의 정보를 가져오는 클래스
/// 
/// 2022.11.05
/// </summary>
public class ConditionController : MonoBehaviour
{
    [SerializeField] private DataBase m_ConditionData;

    private Dictionary<string, StudentCondition> m_StudentCondtion = new Dictionary<string, StudentCondition>();
    public Dictionary<string, StudentCondition> studentCondition => m_StudentCondtion;

    private bool m_IsInitialized = false;
    public bool isInitialized => m_IsInitialized;
    public DataBase dataBase => m_ConditionData;

    protected virtual void Awake()
    {
        //if (m_IsInitialized == false)
        //{
        //    Initialized();
        //    m_IsInitialized = true;
        //}
    }

    //private void Initialized()
    //{
    //    foreach (ConditionData condition in m_ConditionData.studentCondition)
    //    {
    //        m_StudentCondtion.Add(condition.name, new StudentCondition(condition));
    //    }
    //}
}
