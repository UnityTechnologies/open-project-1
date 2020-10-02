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
		// TODO: should this be called on every tick? It actually appears to be called each frame
		character.ResetVerticalMovement();
	}

	public void OnExit(){
		
	}
}