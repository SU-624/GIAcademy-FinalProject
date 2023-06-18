using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class StudentColliderCheck : Conditional
{
    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance == null)
            return TaskStatus.Failure;

        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart || InGameTest.Instance.m_ClassState == ClassState.ClassEnd)
            return TaskStatus.Failure;

        if (gameObject.GetComponent<Student>().InteractingObj == null)
            return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}