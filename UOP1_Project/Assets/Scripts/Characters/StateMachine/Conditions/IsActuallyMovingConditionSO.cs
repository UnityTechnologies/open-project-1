using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsActuallyMoving", menuName = "State Machines/Conditions/Is Actually Moving")]
public class IsActuallyMovingConditionSO : StateConditionSO
{
	[SerializeField] private float _treshold = 0.02f;

	protected override Condition CreateCondition() => new IsActuallyMovingCondition(_treshold);
}

public class IsActuallyMovingCondition : Condition
{
	private float _treshold;
	private CharacterController _characterController;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	public IsActuallyMovingCondition(float treshold)
	{
		_treshold = treshold;
	}

	protected override bool Statement()
	{
		return _characterController.velocity.sqrMagnitude > _treshold * _treshold;
	}
}
