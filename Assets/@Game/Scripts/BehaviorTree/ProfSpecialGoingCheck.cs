using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ProfSpecialGoingCheck : Conditional
{
    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart)
        {
            gameObject.GetComponent<Instructor>().DoingValue = Instructor.Doing.FreeWalk;
            return TaskStatus.Failure;
        }
        else if (gameObject.GetComponent<Instructor>().NowRoom != (int)InteractionManager.SpotName.Nothing && (gameObject.GetComponent<Instructor>().DoingValue == Instructor.Doing.InFacility || gameObject.GetComponent<Instructor>().DoingValue == Instructor.Doing.ObjectInteraction))
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}