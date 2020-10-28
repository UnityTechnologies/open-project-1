using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HasHitHead", menuName = "State Machines/Conditions/Has Hit the Head")]
public class HasHitHeadConditionSO : StateConditionSO<HasHitHeadCondition> { }

public class HasHitHeadCondition : Condition
{
	//Component references
	private Character _characterScript;
	private CharacterController _characterController;
	private Transform _transform;

	public override void Awake(StateMachine stateMachine)
	{
		_transform = stateMachine.GetComponent<Transform>();
		_characterScript = stateMachine.GetComponent<Character>();
		_characterController = stateMachine.GetComponent<CharacterController>();
	}

	public override bool Statement()
	{
		bool isMovingUpwards = _characterScript.movementVector.y > 0f;
		if (isMovingUpwards)
		{
			// Making sure the collision is near the top of the head
			float permittedDistance = _characterController.radius / 2f;
			float topPositionY = _transform.position.y + _characterController.height;
			float distance = Mathf.Abs(_characterScript.lastHit.point.y - topPositionY);
			if (distance <= permittedDistance)
			{
				_characterScript.jumpInput = false;
				_characterScript.movementVector.y = 0f;

				return true;
			}
		}

		return false;
	}
}
