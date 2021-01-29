using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;


[CreateAssetMenu(fileName = "IsRoamingStopTimeElapsed", menuName = "State Machines/Conditions/Is Roaming Stop Time Elapsed")]
public class IsRoamingStopTimeElapsedSO : StateConditionSO
{
	protected override Condition CreateCondition() => new IsRoamingStopTimeElapsed();
}

public class IsRoamingStopTimeElapsed : Condition
{
	private float _stopDuration;
	private float _startTime;

	public override void Awake(StateMachine stateMachine)
	{
		_stopDuration = 0.0f;
		MovementConfigSO config = stateMachine.GetComponent<NpcEntity>().MovementConfig;
		if (typeof(RoamingAroundSpawningPositionConfigSO).IsInstanceOfType(config))
		{
			_stopDuration = ((RoamingAroundSpawningPositionConfigSO)config).StopDuration;
		}
	}

	public override void OnStateEnter()
	{
		_startTime = Time.time;
	}

	protected override bool Statement()
	{
		return Time.time >= _startTime + _stopDuration;
	}
}
