using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Jump Particles")]
public class PlayJumpParticlesActionSO : StateActionSO<PlayJumpParticlesAction> { }

public class PlayJumpParticlesAction : StateAction
{
	//Component references
	private PlayerEffectController _dustController;

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<PlayerEffectController>();
	}

	public override void OnStateEnter()
	{
		_dustController.PlayJumpParticles();
	}

	public override void OnUpdate() { }
}
