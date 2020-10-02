using UnityEngine;

public class WalkingState : IState{

	private Character character;

	public WalkingState(Character character){
		this.character = character;
	}
	
	public void Tick(){
		character.ApplyMovementAndRotate();
	}

	public void OnEnter(){
		character.ResetVerticalMovement();
	}

	public void OnExit(){
		
	}
}