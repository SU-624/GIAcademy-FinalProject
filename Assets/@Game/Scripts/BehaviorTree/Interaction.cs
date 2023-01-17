using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;

public class Interaction : Action
{
	private GameObject m_InterActionObj;
	private bool m_IsPlaying;

	public override void OnStart()
	{
		m_IsPlaying = false;
		SharedGameObject m_Interaction = gameObject.GetComponent<BehaviorTree>().ExternalBehavior.GetVariable("InteractionObject").ConvertTo<SharedGameObject>(); 
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}