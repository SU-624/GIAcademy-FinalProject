using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckStatus : Conditional
{
    //private int successProbability = 20;

    private GameObject m_InterActionObj;
    //public bool _isSuccess;
    //bool _isTargetObjSuccess;

    public override void OnStart()
    {
        //_isSuccess = false;
        m_InterActionObj = this.gameObject.GetComponent<Student>().InteractingObj;
        //_isSuccess = RandomSeed();
    }

    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance == null)
            return TaskStatus.Failure;

        //if (!_isSuccess)
        //    return TaskStatus.Failure;

        if (InGameTest.Instance.m_ClassState != ClassState.nothing)
            return TaskStatus.Failure;

        if (m_InterActionObj == null)
            return TaskStatus.Failure;

        //if (gameObject.GetComponent<Student>().DoingValue != Student.Doing.FreeWalk)
        //    return TaskStatus.Failure;

        bool _coolTime = this.gameObject.GetComponent<Student>().m_IsCoolDown;
        bool _isInteracting = this.gameObject.GetComponent<Student>().m_IsInteracting;

        bool _isTargetObjCoolTime = m_InterActionObj.GetComponent<Student>().m_IsCoolDown;
        bool _isTargetObjInteracting = m_InterActionObj.GetComponent<Student>().m_IsInteracting;

        if (!_coolTime && !_isInteracting && !_isTargetObjCoolTime && !_isTargetObjInteracting)
        {
            ChangeStatus();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    //private bool RandomSeed()
    //{
    //    int randomValue = Random.Range(0, 101);

    //    if (randomValue < successProbability)
    //    {
    //        return true;
    //    }

    //    this.gameObject.GetComponent<Student>().m_IsCoolDown = true;
    //    this.gameObject.GetComponent<Student>().InteractingObj = null;
    //    m_InterActionObj.GetComponent<Student>().InteractingObj = null;
    //    this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.EndInteracting;
    //    return false;
    //}

    private void ChangeStatus()
    {
        this.gameObject.GetComponent<Student>().m_IsInteracting = true;
        m_InterActionObj.GetComponent<Student>().m_IsInteracting = true;

        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.StartInteracting;
        m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.Idle;
    }
}