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
        
        // 쿨다운이 끝났고 상호작용 중이 아닐 때 
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