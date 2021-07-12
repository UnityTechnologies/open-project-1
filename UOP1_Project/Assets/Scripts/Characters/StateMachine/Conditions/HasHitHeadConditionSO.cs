using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Has Hit the Head")]
public class HasHitHeadConditionSO : StateConditionSO<HasHitHeadCondition> { }

public class HasHitHeadCondition : Condition
{
	//Component references
	private Protagonist _protagonistScript;
	private CharacterController _characterController;
	private Transform _transform;

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.GetComponent<Transform>();
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	protected override bool Statement()
	{
		bool isMovingUpwards = _protagonistScript.movementVector.y > 0f;
		if (isMovingUpwards)
		{
			if(_characterController.collisionFlags == CollisionFlags.Above)
			{
				_protagonistScript.jumpInput = false;
				_protagonistScript.movementVector.y = 0f;

				return true;
			}
		}

		return false;
	}
}
