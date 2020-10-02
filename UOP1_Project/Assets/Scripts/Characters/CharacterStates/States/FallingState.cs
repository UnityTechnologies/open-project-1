using System;

public class FallingState : State{

	private Character character;

	public FallingState(Character character){
		this.character = character;
	}
	
	public override void Tick(){
		character.ApplyGravityComeback();
		character.ApplyMovementAndRotate();
	}

	public override void OnEnter(){
		
	}

	public override void OnExit(){
		
	}
}