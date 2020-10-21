using UnityEngine;

public class CharacterJumpState : CharacterAbilityState
{
	private int amountOfJumpsLeft;

	public CharacterJumpState(Character character, CharacterStateMachine stateMachine, CharacterData characterData) : base(character, stateMachine, characterData)
	{
		amountOfJumpsLeft = characterData.numberOfJumps;
	}

	public override void Enter()
	{
		base.Enter();

		character.SetVelocityY(characterData.initialJumpForce);
		isAbilityDone = true;
		amountOfJumpsLeft--;
		character.InAirState.SetIsJumping();
	}

	public override void Exit()
	{
		base.Exit();
	}
	public bool CanJump()
	{
		return amountOfJumpsLeft > 0;
	}

	public void ResetAmountOfJumpsLeft()
	{
		amountOfJumpsLeft = characterData.numberOfJumps;
	}

	public void DecreaseAmountOfJumpsLeft()
	{
		amountOfJumpsLeft--;
	}
}
