using UnityEngine;

public class CharacterState
{
    protected Character character;
    protected CharacterStateMachine stateMachine;
    protected CharacterData characterData;

    protected bool isExitingState;

    protected float startTime;

    public CharacterState(Character character, CharacterStateMachine stateMachine, CharacterData characterData)
    {
        this.character = character;
        this.stateMachine = stateMachine;
        this.characterData = characterData;
    }

    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate(){
		DoChecks();
	}

    public virtual void PhysicsUpdate() {}

    public virtual void DoChecks(){ }
}
