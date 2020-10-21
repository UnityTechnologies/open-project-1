using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityState : CharacterState
{
	private bool _isGrounded;

    protected bool isAbilityDone;

    protected Vector3 inputVector;
    protected float yInput;

    public CharacterAbilityState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
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
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

		inputVector = character.InputVector;

		if (isAbilityDone)
		{
			if (_isGrounded && character.CurrentVelocity.y < 0.01f)
			{
				if (inputVector.x != 0 || inputVector.z != 0)
				{
					stateMachine.ChangeState(character.MoveState);
				}
				else
				{
					stateMachine.ChangeState(character.IdleState);
				}
			}
			else
			{
				stateMachine.ChangeState(character.InAirState);
			}
		}
	}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

		
    }
}
