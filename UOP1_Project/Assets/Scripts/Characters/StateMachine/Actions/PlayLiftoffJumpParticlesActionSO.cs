using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

//TODO: Cleanup all the Actions related to jump particles, there are too many specific scripts now

[CreateAssetMenu(fileName = "PlayLiftoffJumpParticlesAction", menuName = "State Machines/Actions/Play Liftoff Jump Particles Action")]
public class PlayLiftoffJumpParticlesActionSO : StateActionSO<PlayLiftoffJumpParticlesAction> { }

public class PlayLiftoffJumpParticlesAction : StateAction
{
	//Component references
	private PlayerEffectController _dustController;

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<PlayerEffectController>();
	}

	public override void OnStateEnter()
	{
		_dustController.PlayLandParticles(1f); //Same particles as the landing, but with full power
	}

	public override void OnUpdate() { }
}
