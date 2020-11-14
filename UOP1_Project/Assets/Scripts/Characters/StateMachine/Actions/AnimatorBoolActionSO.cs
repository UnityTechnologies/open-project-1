using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "SetAnimatorBoolAction", menuName = "State Machines/Actions/Set Animator Bool")]
public class AnimatorBoolActionSO : StateActionSO
{
	[SerializeField] private string _parameterName = default;
	[SerializeField] private bool _newValue = default;

	protected override StateAction CreateAction() => new AnimatorParameterAction(_parameterName, _newValue);
}

public class AnimatorParameterAction : StateAction
{
	//Component references
	private Animator _animator;

	private int _parameterHash;
	private bool _newValue;

	public AnimatorParameterAction(string parameterName, bool newValue)
	{
		_parameterHash = Animator.StringToHash(parameterName);
		_newValue = newValue;
	}

	public override void Awake(StateMachine stateMachine)
	{
		_animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		_animator.SetBool(_parameterHash, _newValue);
	}

	public override void OnUpdate() { }
}
