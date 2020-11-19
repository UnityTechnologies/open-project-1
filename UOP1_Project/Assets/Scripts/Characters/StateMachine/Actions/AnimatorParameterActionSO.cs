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
	[SerializeField] private ParameterType _parameterType = default;
	[SerializeField] private string _parameterName = default;

	[SerializeField] private bool _boolValue = default;
	[SerializeField] private int _intValue = default;
	[SerializeField] private float _floatValue = default;

	[SerializeField] private Moment _whenToRun = default; // Allows this StateActionSO type to be reused for all 3 state moments.

	// Bit of a waste to send all three parameters to the StateAction, but it's a small price to pay for a lot of convenience
	protected override StateAction CreateAction() => new AnimatorParameterAction(_whenToRun, _parameterName, _parameterType,
																				 _boolValue, _intValue, _floatValue);

	public enum ParameterType
	{
		Bool, Int, Float, Trigger,
	}
}

public class AnimatorParameterAction : StateAction
{
	//Component references
	private Animator _animator;

	private int _parameterHash;
	private AnimatorParameterActionSO.ParameterType _parameterType;

	private bool _boolValue;
	private int _intValue;
	private float _floatValue;

	private SpecificMoment _whenToRun;

	public AnimatorParameterAction(SpecificMoment whenToRun, string parameterName,
								   AnimatorParameterActionSO.ParameterType parameterType,
								   bool newBoolValue, int newIntValue, float newFloatValue)
	{
		_whenToRun = whenToRun;
		_parameterHash = Animator.StringToHash(parameterName);
		_parameterType = parameterType;

		_boolValue = newBoolValue;
		_intValue = newIntValue;
		_floatValue = newFloatValue;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		if (_whenToRun == SpecificMoment.OnStateEnter)
			SetParameter();
	}

	public override void OnStateExit()
	{
		if (_whenToRun == SpecificMoment.OnStateExit)
			SetParameter();
	}

	private void SetParameter()
	{
		switch (_parameterType)
		{
			case AnimatorParameterActionSO.ParameterType.Bool:
				_animator.SetBool(_parameterHash, _boolValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Int:
				_animator.SetInteger(_parameterHash, _intValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Float:
				_animator.SetFloat(_parameterHash, _floatValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Trigger:
				_animator.SetTrigger(_parameterHash);
				break;
		}
	}

	public override void OnUpdate() { }
}
