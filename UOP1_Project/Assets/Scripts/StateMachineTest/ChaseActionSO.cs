using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase", menuName = "State Machines/Tests/Actions/Chase")]
public class ChaseActionSO : StateActionSO
{
	[Tooltip("Object that specifies the speed and the target to chase. This can be overriden in the StateMachine.")]
	[SerializeField] private ChaseDataSO _chaseData = null;

	protected override StateAction CreateAction() => new ChaseAction(_chaseData);
}

public class ChaseAction : StateAction
{
	private Transform _transform;
	private Transform _chaseTransform;
	private ChaseDataSO _chaseData;

	public ChaseAction(ChaseDataSO chaseData) => _chaseData = chaseData;

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

	public override void OnUpdate()
	{
		_transform.position = Vector3.MoveTowards(
			_transform.position,
			_chaseTransform.position,
			_chaseData.Speed * Time.deltaTime);
	}
}
