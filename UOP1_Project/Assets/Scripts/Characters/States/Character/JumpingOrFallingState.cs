using UnityEngine;

public class JumpingOrFallingState : IState{

	private Character character;
	
	public JumpingOrFallingState(Character character){
		this.character = character;
	}
	
	public void Tick(){
		character.ApplyGravityComeback();

		if (character.IsJumpingTooLong(Time.time)){
			character.SetJumpingState(false);
			character.ResetGravityContributorMultiplier();
		}
		else{
			character.ReduceGravityEffect();
		}

		character.ApplyMovementAndRotate();

		character.CalculateFinalAirMovement();
	}

	public void OnEnter(){
		// TODO: reduce movement speeds to create slower movement in mid-air
	}

	public void OnExit(){
		// TODO: 
	}
	
	// TODO: add a method to calculate vertical movement for character when jumping
}