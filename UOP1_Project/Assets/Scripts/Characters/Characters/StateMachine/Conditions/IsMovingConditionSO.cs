using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "StartedMoving", menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO
{
	[SerializeField] private float _treshold = 0.02f;

	protected override Condition CreateCondition() => new IsMovingCondition(_treshold);
}

public class IsMovingCondition : Condition
{
	private float _treshold;
	private Character _characterScript;

	public override void Awake(StateMachine stateMachine)
	{
		_characterScript = stateMachine.GetComponent<Character>();
	}

	public IsMovingCondition(float treshold)
	{
		_treshold = treshold;
	}

	public override bool Statement()
	{
		Vector3 movementVector = _characterScript.movementInput;
		movementVector.y = 0f;
		return movementVector.sqrMagnitude > _treshold;
	}

	public override void OnStateExit()
	{
		_characterScript.movementVector = Vector3.zero;
	}
}
