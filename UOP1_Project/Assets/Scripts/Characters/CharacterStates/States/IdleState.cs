using System;

public class IdleState : State
{

	private Character character;

	public IdleState(Character character)
	{
		this.character = character;
	}

	public override void Tick()
	{
		// character.ApplyMovementAndRotate();
	}

	public override void OnEnter()
	{

	}

	public override void OnExit()
	{

	}
}
