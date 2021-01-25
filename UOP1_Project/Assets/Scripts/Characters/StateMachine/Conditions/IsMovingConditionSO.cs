using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
	public float treshold = 0.02f;
}

public class IsMovingCondition : Condition
{
	private Protagonist _protagonistScript;
	private IsMovingConditionSO _originSO => (IsMovingConditionSO)base.OriginSO; // The SO this Condition spawned from

	public override void Awake(StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		Vector3 movementVector = _protagonistScript.movementInput;
		movementVector.y = 0f;
		return movementVector.sqrMagnitude > _originSO.treshold;
	}
}
