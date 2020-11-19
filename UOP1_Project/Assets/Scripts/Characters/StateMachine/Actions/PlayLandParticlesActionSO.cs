using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Land Particles")]
public class PlayLandParticlesActionSO : StateActionSO<PlayLandParticlesAction> { }

public class PlayLandParticlesAction : StateAction
{
	//Component references
	private DustParticlesController _dustController;

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<DustParticlesController>();
	}

	public override void OnStateExit()
	{
		_dustController.PlayLandParticles();
	}

	public override void OnUpdate() { }
}
