using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.Scriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseToTarget", menuName = "State Machines/Tests/Conditions/Close To Target")]
public class ScriptableCloseToTargetCondition : ScriptableStateCondition
{
	[SerializeField] private ChaseDataObject _chaseData = null;

	protected override Condition CreateCondition() => new CloseToTargetCondition(_chaseData);
}

public class CloseToTargetCondition : Condition
{
	private Transform _transform;
	private Transform _chaseTransform;
	private ChaseDataObject _chaseData;

	public CloseToTargetCondition(ChaseDataObject chaseData) => _chaseData = chaseData;

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.transform;

		if (stateMachine.TryGetScriptableObject<ChaseDataObject>(out var chaseData))
			_chaseData = chaseData;

		if (_chaseData == null)
			throw new ArgumentNullException(nameof(_chaseData));

		if (string.IsNullOrEmpty(_chaseData.TargetName))
			throw new ArgumentNullException(nameof(_chaseData.TargetName));

		_chaseTransform = GameObject.Find(_chaseData.TargetName).transform;
	}

	public override bool Statement() => Vector3.Distance(_transform.position, _chaseTransform.position) < 1f;
}
