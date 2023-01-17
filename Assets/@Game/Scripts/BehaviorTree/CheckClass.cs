using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckClass : Conditional
{
    bool isClassDesSetting = false;

    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart)
        {
            if (isClassDesSetting == false)
            {
                SetClassDestination();
            }
            return TaskStatus.Success;
        }
        else if (InGameTest.Instance.m_ClassState == ClassState.Studying)
        {
            return TaskStatus.Success;
        }
        else if(InGameTest.Instance.m_ClassState == ClassState.ClassEnd )
        {
            if(gameObject.GetComponent<Student>().isDesSetting == false)
            {
                SetClassEndDestination();
            }
            return TaskStatus.Success;

        }
        return TaskStatus.Failure;

    }

    void SetClassDestination()
    {
        gameObject.GetComponent<Student>().m_DestinationQueue.Clear();
        gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("ClassEntrance");
        gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("ClassSeat");
        gameObject.GetComponent<Student>().isDesSetting = true;
        isClassDesSetting = true;
    }

    void SetClassEndDestination()
    {
        gameObject.GetComponent<Student>().m_DestinationQueue.Clear();

        int _rand = Random.Range(0, 3);

        if (_rand == 0)
        {
            gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk1");
        }
        else if (_rand == 1)
        {
            gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk2");
        }
        else if (_rand == 2)
        {
            gameObject.GetComponent<Student>().m_DestinationQueue.Enqueue("FreeWalk3");
        }
        gameObject.GetComponent<Student>().isDesSetting = true;
        isClassDesSetting = false;
    }
}