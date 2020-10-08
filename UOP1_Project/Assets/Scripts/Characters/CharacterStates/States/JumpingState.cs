using UnityEngine;

public class JumpingState : State
{

	private Character character;

	public JumpingState(Character character)
	{
		this.character = character;
	}

	public override void Tick()
	{
		character.ApplyGravityComeback();

		if (character.IsJumpingTooLong(Time.time))
		{
			character.SetJumpingState(false);
			character.ResetGravityContributorMultiplier();
		}
		else
		{
			character.ReduceGravityEffect();
		}

		character.ApplyMovementAndRotate();
	}

	public override void OnEnter()
	{
		character.InitJumpingValues();
	}

	public override void OnExit()
	{

	}
}
