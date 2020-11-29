using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed")]
public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
{
	public float timerLength = .5f;
}

public class TimeElapsedCondition : Condition
{
	private float _startTime;
	private TimeElapsedConditionSO _originSO => (TimeElapsedConditionSO)base.OriginSO; // The SO this Condition spawned from

	public override void OnStateEnter()
	{
		_startTime = Time.time;
	}

	protected override bool Statement() => Time.time >= _startTime + _originSO.timerLength;
}
