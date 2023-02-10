using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InteractionCoolTime : Conditional
{
    bool m_CoolTime;
    bool m_IsInteracting;

    public override TaskStatus OnUpdate()
    {
        m_CoolTime = this.gameObject.GetComponent<Student>().m_IsCoolDown;
        m_IsInteracting = this.gameObject.GetComponent<Student>().m_IsInteracting;
        
        // ��ٿ��� ������ ��ȣ�ۿ� ���� �ƴ� �� 
        if (!m_CoolTime && !m_IsInteracting)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }

    }
}