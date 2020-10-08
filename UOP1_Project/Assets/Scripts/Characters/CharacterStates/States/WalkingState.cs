using UnityEngine;

public class WalkingState : State
{

	private Character character;

	public WalkingState(Character character)
	{
		this.character = character;
	}

	public override void Tick()
	{
		character.ApplyMovementAndRotate();
	}

	public override void OnEnter()
	{
		character.ResetVerticalMovement();
	}

	public override void OnExit()
	{

	}
}
