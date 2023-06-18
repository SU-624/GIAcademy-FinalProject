using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ProfCheckClass : Conditional
{
	public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance == null)
        {
            return TaskStatus.Failure;
        }

        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart && (this.gameObject.GetComponent<Instructor>().DoingValue == Instructor.Doing.FreeWalk || this.gameObject.GetComponent<Instructor>().DoingValue == Instructor.Doing.Idle))
        {
            return TaskStatus.Success;
        }

        if (InGameTest.Instance.m_ClassState == ClassState.Studying)
        {
            //gameObject.GetComponent<Animator>().SetBool("isStudying", true);
            return TaskStatus.Success;
        }
        else if (InGameTest.Instance.m_ClassState == ClassState.ClassEnd)
        {
            //this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.FreeWalk;
            //gameObject.GetComponent<Animator>().SetBool("isStudying", false);
            this.gameObject.GetComponent<Instructor>().m_IsCoolDown = false;    // ÄðÅ¸ÀÓ ³¡³².
            this.gameObject.GetComponent<Instructor>().m_IsInteracting = false;

            return TaskStatus.Success;

        }
        return TaskStatus.Failure;

    }
}