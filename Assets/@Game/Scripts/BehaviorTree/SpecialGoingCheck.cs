using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SpecialGoingCheck : Conditional
{
	public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart)
        {
            gameObject.GetComponent<Student>().DoingValue = Student.Doing.FreeWalk;
            return TaskStatus.Failure;
        }
        else if (gameObject.GetComponent<Student>().NowRoom != (int)InteractionManager.SpotName.Nothing && (gameObject.GetComponent<Student>().DoingValue == Student.Doing.InGenreRoom || gameObject.GetComponent<Student>().DoingValue == Student.Doing.InFacility || gameObject.GetComponent<Student>().DoingValue == Student.Doing.ObjectInteraction))
        {
		    return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
	}
}