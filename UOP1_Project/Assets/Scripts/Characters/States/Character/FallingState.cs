using System;

public class FallingState : IState{

	private Character character;

	public FallingState(Character character){
		this.character = character;
	}
	
	public void Tick(){
		// all this does is reduce the gravity effect
		character.ReduceGravityEffect();
	}

	public void OnEnter(){
		
	}

	public void OnExit(){
		
	}
}