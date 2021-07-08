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

		Vector3 lastHitProj = _protagonistScript.lastHit.point;
		lastHitProj.y = 0f;

		

		if (_protagonistScript.canMoveForward)
		{

			float isForward = Vector3.Dot(_protagonistScript.transform.forward, lastHitProj.normalized);
			if (isForward >= .9f)
			{
				return false;
			}
			else
			{
				float slope = Vector3.Angle(Vector3.up, _protagonistScript.lastHit.normal);
				return slope >= _characterController.slopeLimit;
			}
		}
		else
		{
			float slope = Vector3.Angle(Vector3.up, _protagonistScript.lastHit.normal);
			return slope >= _characterController.slopeLimit;
		}
	}
}
