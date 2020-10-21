using UnityEngine;

public class CharacterInAirState : CharacterState
{
	// Input
	private Vector3 inputVector;
	private bool jumpInput;

	// Checks
	private bool _isGrounded;
	private bool _isJumping;

	private float gravityContributionMultiplier = 0f; //The factor which determines how much gravity is affecting verticalMovement
	private float verticalMovement = 0f; //Represents how much a player will move vertically in a frame. Affected by gravity * gravityContributionMultiplier

	public CharacterInAirState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
	{
	}

	public override void DoChecks()
	{
		base.DoChecks();

		_isGrounded = character.CheckIfGrounded();
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		GetInput();

		if (_isGrounded)
		{
			stateMachine.ChangeState(character.IdleState);
		}
		else if (jumpInput && character.JumpState.CanJump()) {
			stateMachine.ChangeState(character.JumpState);
		}
		else
		{
			gravityContributionMultiplier += Time.deltaTime * characterData.gravityComebackMultiplier;

			if (_isJumping)
			{
				if (Time.time >= startTime + characterData.jumpInputDuration)
				{
					_isJumping = false;
					gravityContributionMultiplier = 1f; //Gravity influence is reset to full effect
				}
				else
				{

					gravityContributionMultiplier *= characterData.gravityDivider; //Reduce the gravity effect
				}
			}

			if (character.CollisionTop)
			{
				character.ResetCollisionTop();
				_isJumping = false;
				gravityContributionMultiplier = 1f;
				verticalMovement = 0f;
			}

			//Less control in mid-air, conserving momentum from previous frame
			character.SetVelocityX(inputVector.x * characterData.speed);
			character.SetVelocityZ(inputVector.z * characterData.speed);

			//The character is either jumping or in freefall, so gravity will add up
			gravityContributionMultiplier = Mathf.Clamp01(gravityContributionMultiplier);

			//Add gravity contribution
			//Note that even if it's added, the above value is negative due to Physics.gravity.y
			//Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
			verticalMovement = character.CurrentVelocity.y + Physics.gravity.y * characterData.gravityMultiplier * Time.deltaTime * gravityContributionMultiplier;


			verticalMovement = Mathf.Clamp(verticalMovement, -characterData.maxFallSpeed, 100f);

			character.SetVelocityY(verticalMovement);
		}
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	private void GetInput()
	{
		inputVector = character.InputVector;
		jumpInput = character.JumpInput;
	}

	public void SetIsJumping(bool isJumping) => _isJumping = isJumping;
}
