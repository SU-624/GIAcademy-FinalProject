using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FreeWalkDesCheck : Conditional
{
    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance.m_ClassState == ClassState.nothing)
        {
            if (gameObject.GetComponent<Student>().isDesSetting == false)
            {
                SetDestinationFreeWalk();
            }
        }
        return TaskStatus.Success;
    }

    void SetDestinationFreeWalk()
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
    }

}