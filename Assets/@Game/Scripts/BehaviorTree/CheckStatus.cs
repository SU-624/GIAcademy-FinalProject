using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckStatus : Conditional
{
    private GameObject m_InterActionObj;

    public override void OnStart()
    {
        SharedGameObject m_Interaction = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("InteractionObject").ConvertTo<SharedGameObject>();
        m_InterActionObj = m_Interaction.Value;
    }

    public override TaskStatus OnUpdate()
    {
        bool _coolTime = this.gameObject.GetComponent<Student>().m_IsCoolDown;
        bool _isInteracting = this.gameObject.GetComponent<Student>().m_IsInteracting;

        bool _isTargetObjCoolTime = m_InterActionObj.GetComponent<Student>().m_IsCoolDown;
        bool _isTargetObjInteracting = m_InterActionObj.GetComponent<Student>().m_IsInteracting;

        if (!_coolTime && !_isInteracting && !_isTargetObjCoolTime && !_isTargetObjInteracting)
        {
            ChangeStatus();
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    private void ChangeStatus()
    {
        this.gameObject.GetComponent<Student>().m_IsInteracting = true;
        m_InterActionObj.GetComponent<Student>().m_IsInteracting = true;

        this.gameObject.GetComponent<Student>().m_IsDialoguePlaying = true;
        m_InterActionObj.GetComponent<Student>().m_IsDialoguePlaying = true;

        this.gameObject.GetComponent<Student>().DoingValue = Student.Doing.StartInteracting;
        m_InterActionObj.GetComponent<Student>().DoingValue = Student.Doing.StartInteracting;
    }
}