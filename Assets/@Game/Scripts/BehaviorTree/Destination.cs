using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Destination : Conditional
{
    private string m_NowDestination = " ";


    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        if (InGameTest.Instance.m_ClassState == ClassState.ClassStart
            && m_NowDestination != "ClassEntrance"
            && m_NowDestination != "ClassSeat")
        {
            m_NowDestination = " ";
        }

        bool _bNewDestination = SetDestination();

        return _bNewDestination ? TaskStatus.Success : TaskStatus.Failure;
    }

    bool SetDestination()
    {
        string _name;

        if (m_NowDestination == " ")
        {
            if (gameObject.GetComponent<Student>().m_DestinationQueue.Count != 0)
            {
                m_NowDestination = gameObject.GetComponent<Student>().m_DestinationQueue.Dequeue();
                _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();
                gameObject.GetComponent<BehaviorTree>().ExternalBehavior.SetVariableValue("Destination", GameObject.Find(_name).transform.position);
                return true;
            }
        }
        else
        {
            _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();

            float _dis = Vector3.Distance(gameObject.transform.position, GameObject.Find(_name).transform.position);

            //// 수업 시작했을 때 
            Vector3 v1 = GameObject.Find(_name).transform.position;
            SharedVector3 sv = (SharedVector3)gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("Destination");
            Vector3 v2 = sv.Value;

            if (v1 != v2)
            {
                gameObject.GetComponent<BehaviorTree>().ExternalBehavior.SetVariableValue("Destination", GameObject.Find(_name).transform.position);
            }

            //Debug.Log(gameObject.GetComponent<NavMeshAgent>().speed);
            //if (gameObject.GetComponent<NavMeshAgent>().velocity.magnitude <= 6f)
            if (_dis < 3)
            {
                if (gameObject.GetComponent<Student>().m_DestinationQueue.Count != 0)
                {
                    m_NowDestination = gameObject.GetComponent<Student>().m_DestinationQueue.Dequeue();

                    if (m_NowDestination == "ClassSeat")
                    {
                        _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();
                    }
                    else
                    {
                        _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();
                        //m_NowDestination = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();
                    }
                    gameObject.GetComponent<BehaviorTree>().ExternalBehavior.SetVariableValue("Destination", GameObject.Find(_name).transform.position);
                    return true;
                }
                else
                {
                    // 수업이 아닌 특정 목적지를 도착했을 때 이동하는 걸로 변경하기
                    gameObject.GetComponent<Student>().m_IsDesSetting = false;

                    if (m_NowDestination == "ClassSeat")
                    {
                        _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();

                        GameObject _tempObj = GameObject.Find(_name);

                        if (this.gameObject.transform.position.z <= _tempObj.transform.position.z)
                        {
                            m_NowDestination = " ";
                            gameObject.GetComponent<Student>().m_IsArrivedClass = true;
                        }
                    }
                }
            }
            //else
            //{
            //    _name = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable(m_NowDestination).ToString();

            //    GameObject _tempObj = GameObject.Find(_name);

            //    if (this.gameObject.transform.position.z <= _tempObj.transform.position.z)
            //    {
            //        m_NowDestination = " ";
            //    }
            //}
        }

        return true;
    }
}