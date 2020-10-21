using UnityEngine;

public class CharacterGroundedState : CharacterState
{
	private bool _isGrounded;

    // Input
    protected Vector3 inputVector;
	protected bool jumpInput;

	public CharacterGroundedState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
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

		character.UseJump();
		character.JumpState.ResetAmountOfJumpsLeft();
	}

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

		inputVector = character.InputVector;
		jumpInput = character.JumpInput;

		
		character.SetVelocityY(characterData.fallingVerticalMovement);

		if (jumpInput && character.JumpState.CanJump() && !character.ShouldSlide)
		{
			stateMachine.ChangeState(character.JumpState);
		}
		else if (!_isGrounded)
		{
			stateMachine.ChangeState(character.InAirState);
		}
	}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
