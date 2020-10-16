using System;
using DeivSky.StateMachine;
using DeivSky.StateMachine.Scriptables;
using UnityEngine;

[CreateAssetMenu(fileName = "Timer", menuName = "State Machines/Tests/Conditions/Timer")]
public class ScriptableTimedCondition : SerializableStateCondition<TimedCondition> { }

[Serializable]
public class TimedCondition : Condition
{
	[SerializeField] private float _duration = 10f;
	private float _time;

	public override void OnStateEnter() => _time = Time.time + _duration;

	public override bool Statement() => Time.time >= _time;
}
