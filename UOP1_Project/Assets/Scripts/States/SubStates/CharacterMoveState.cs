using System.Collections;
using UnityEngine;

public class CharacterMoveState : CharacterGroundedState
{
    public CharacterMoveState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

		if (!isExitingState)
		{
			character.SetVelocityX(characterData.speed * inputVector.x);
			character.SetVelocityZ(characterData.speed * inputVector.z);

			if (!isExitingState)
			{
				if (inputVector.x == 0 && inputVector.y == 0 && inputVector.z == 0)
				{
					stateMachine.ChangeState(character.IdleState);
				}
			}
		}
	}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
