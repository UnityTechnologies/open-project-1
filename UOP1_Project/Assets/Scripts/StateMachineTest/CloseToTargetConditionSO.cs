using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseToTarget", menuName = "State Machines/Tests/Conditions/Close To Target")]
public class CloseToTargetConditionSO : StateConditionSO<CloseToTargetCondition> { }

public class CloseToTargetCondition : Condition
{
	private Transform _transform;
	private Transform _target;

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.transform;
		_target = stateMachine.GetComponent<ChaseComponent>().Target;
	}

	public override bool Statement()
	{
		return Vector3.Distance(_transform.position, _target.position) < 1f;
	}
}
