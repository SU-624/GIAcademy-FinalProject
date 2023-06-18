using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckClass : Conditional
{
    bool isClassDesSetting = false;

    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance == null)
        {
            return TaskStatus.Failure;
        }

        //if (InGameTest.Instance.m_ClassState == ClassState.ClassStart && (this.gameObject.GetComponent<Student>().DoingValue == Student.Doing.FreeWalk || this.gameObject.GetComponent<Student>().DoingValue == Student.Doing.Idle))
        //{
        //    return TaskStatus.Success;
        //}

        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart && this.gameObject.GetComponent<Student>().DoingValue != Student.Doing.StartInteracting)
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
            this.gameObject.GetComponent<Student>().m_IsCoolDown = false;    // ��Ÿ�� ����.
            this.gameObject.GetComponent<Student>().m_IsInteracting = false;

            if (gameObject.GetComponent<Student>().m_IsDesSetting == false)
            {
                //SetClassEndDestination();
            }
            return TaskStatus.Success;

        }
        return TaskStatus.Failure;

    }

    //void SetClassDestination()
    //{
    //    gameObject.GetComponent<Student>().m_DestinationQueue.Clear();
    //    gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("ClassEntrance");
    //    gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("ClassSeat");
    //    gameObject.GetComponent<Student>().m_IsDesSetting = true;
    //    isClassDesSetting = true;
    //    gameObject.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    //}

    //void SetClassEndDestination()
    //{
    //    gameObject.GetComponent<Student>().m_DestinationQueue.Clear();
    //    //gameObject.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;

    //    int _rand = Random.Range(0, 3);

    //    if (_rand == 0)
    //    {
    //        gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk1");
    //    }
    //    else if (_rand == 1)
    //    {
    //        gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk2");
    //    }
    //    else if (_rand == 2)
    //    {
    //        gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk3");
    //    }
    //    gameObject.GetComponent<Student>().m_IsDesSetting = true;
    //    isClassDesSetting = false;
    //}
}