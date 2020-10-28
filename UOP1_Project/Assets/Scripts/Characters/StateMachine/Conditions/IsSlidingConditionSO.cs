using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsSliding", menuName = "State Machines/Conditions/Is Sliding")]
public class IsSlidingConditionSO : StateConditionSO<IsSlidingCondition> { }

public class IsSlidingCondition : Condition
{
	private CharacterController _characterController;
	private Character _characterScript;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
		_characterScript = stateMachine.GetComponent<Character>();
	}

	public override bool Statement()
	{
		if (_characterScript.lastHit == null)
			return false;

		float currentSlope = Vector3.Angle(Vector3.up, _characterScript.lastHit.normal);
		return (currentSlope >= _characterController.slopeLimit);
	}
}
