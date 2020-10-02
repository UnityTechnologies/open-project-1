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

		character.ApplyMovementAndRotate();
	}

	public void OnEnter(){
		// TODO: move setup code for jumping here
	}

	public void OnExit(){
		// TODO: move exit code for jumping here
	}
}