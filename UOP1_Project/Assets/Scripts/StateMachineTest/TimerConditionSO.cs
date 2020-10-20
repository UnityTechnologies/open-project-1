using DeivSky.StateMachine;
using DeivSky.StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Timer", menuName = "State Machines/Tests/Conditions/Timer")]
public class TimerConditionSO : StateConditionSO
{
	public float Duration = 10f;

	protected override Condition CreateCondition() => new TimerCondition(Duration);
}

public class TimerCondition : Condition
{
	private float _duration = 10f;
	private float _time;

	public TimerCondition(float duration) => _duration = duration;

	public override void OnStateEnter() => _time = Time.time + _duration;

	public override bool Statement() => Time.time >= _time;
}
