using System;
using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using Moment = UOP1.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
public class AnimatorParameterActionSO : StateActionSO
{
	public ParameterType parameterType = default;
	public string parameterName = default;

	public bool boolValue = default;
	public int intValue = default;
	public float floatValue = default;

	public Moment whenToRun = default; // Allows this StateActionSO type to be reused for all 3 state moments

	protected override StateAction CreateAction() => new AnimatorParameterAction(Animator.StringToHash(parameterName));

	public enum ParameterType
	{
		Bool, Int, Float, Trigger,
	}
}

public class AnimatorParameterAction : StateAction
{
	//Component references
	private Animator _animator;
	private AnimatorParameterActionSO _originSO => (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
	private int _parameterHash;

	public AnimatorParameterAction(int parameterHash)
	{
		_parameterHash = parameterHash;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		if (_originSO.whenToRun == SpecificMoment.OnStateEnter)
			SetParameter();
	}

	public override void OnStateExit()
	{
		if (_originSO.whenToRun == SpecificMoment.OnStateExit)
			SetParameter();
	}

	private void SetParameter()
	{
		switch (_originSO.parameterType)
		{
			case AnimatorParameterActionSO.ParameterType.Bool:
				_animator.SetBool(_parameterHash, _originSO.boolValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Int:
				_animator.SetInteger(_parameterHash, _originSO.intValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Float:
				_animator.SetFloat(_parameterHash, _originSO.floatValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Trigger:
				_animator.SetTrigger(_parameterHash);
				break;
		}
	}

	public override void OnUpdate() { }
}
