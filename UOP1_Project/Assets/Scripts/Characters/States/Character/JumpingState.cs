using UnityEngine;

public class JumpingState : IState{

	private Character character;
	
	public JumpingState(Character character){
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
	}

	public void OnEnter(){
		character.InitJumpingValues();
	}

	public void OnExit(){
		
	}
}