using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ArrivedCheck : Conditional
{
	public SharedString _destination;

	public SharedTransform _target;

	public override TaskStatus OnUpdate()
	{
		if(ArrivedGoal(_target.Value) == false && 
			_destination.Value == "FreeWalk1" || 
			_destination.Value == "FreeWalk2" || 
			_destination.Value == "FreeWalk3")
        {
			return TaskStatus.Success;
        }

		//_destination.Value = "Base";

		return TaskStatus.Failure;
	}

	public bool ArrivedGoal(Transform _targetTransform)
    {
		float _dis = Vector3.Distance(_targetTransform.position, transform.position);

		return _dis < 1;
    }
}