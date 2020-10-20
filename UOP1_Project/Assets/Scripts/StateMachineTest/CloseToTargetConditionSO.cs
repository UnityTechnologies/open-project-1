using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseToTarget", menuName = "State Machines/Tests/Conditions/Close To Target")]
public class CloseToTargetConditionSO : StateConditionSO
{
	[SerializeField] private ChaseDataSO _chaseData = null;

	protected override Condition CreateCondition() => new CloseToTargetCondition(_chaseData);
}

public class CloseToTargetCondition : Condition
{
	private Transform _transform;
	private Transform _chaseTransform;
	private ChaseDataSO _chaseData;

	public CloseToTargetCondition(ChaseDataSO chaseData) => _chaseData = chaseData;

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.transform;

		if (stateMachine.TryGetScriptableObject<ChaseDataSO>(out var chaseData))
			_chaseData = chaseData;

		if (_chaseData == null)
			throw new ArgumentNullException(nameof(_chaseData));

		if (string.IsNullOrEmpty(_chaseData.TargetName))
			throw new ArgumentNullException(nameof(_chaseData.TargetName));

		_chaseTransform = GameObject.Find(_chaseData.TargetName).transform;
	}

	public override bool Statement() => Vector3.Distance(_transform.position, _chaseTransform.position) < 1f;
}
