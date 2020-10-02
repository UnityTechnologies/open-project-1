using System;

public class FallingState : IState{

	private Character character;

	public FallingState(Character character){
		this.character = character;
	}
	
	public void Tick(){
		character.ApplyGravityComeback();
		character.ApplyMovementAndRotate();
	}

	public void OnEnter(){
		
	}

	public void OnExit(){
		
	}
}