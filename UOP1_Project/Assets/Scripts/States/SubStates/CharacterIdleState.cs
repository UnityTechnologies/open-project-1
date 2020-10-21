using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterGroundedState
{
    public CharacterIdleState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
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
			if (inputVector.x != 0 || inputVector.z != 0)
			{
				stateMachine.ChangeState(character.MoveState);
			}
		}
	}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();  
    }
}
