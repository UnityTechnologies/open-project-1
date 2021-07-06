using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsSliding", menuName = "State Machines/Conditions/Is Sliding")]
public class IsSlidingConditionSO : StateConditionSO<IsSlidingCondition> { }

public class IsSlidingCondition : Condition
{
	private CharacterController _characterController;
	private Protagonist _protagonistScript;

	public override void Awake(StateMachine stateMachine)
	{
		_characterController = stateMachine.GetComponent<CharacterController>();
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	protected override bool Statement()
	{
		float raycastSlope = Vector3.Angle(Vector3.up, _protagonistScript.rayGroundNormal);
		if (raycastSlope >= _characterController.slopeLimit)
		{
			float spherecastSlope = Vector3.Angle(Vector3.up, _protagonistScript.spherecastGroundNormal);
			return spherecastSlope >= _characterController.slopeLimit;
		}
		else
		{
			return false;
		}
	}
}
