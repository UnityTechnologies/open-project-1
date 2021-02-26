using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ControlWalkingParticlesAction", menuName = "State Machines/Actions/Control Walking Particles")]
public class ControlWalkingParticlesActionSO : StateActionSO<ControlWalkingParticlesAction> { }

public class ControlWalkingParticlesAction : StateAction
{
	//Component references
	private PlayerEffectController _dustController;

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<PlayerEffectController>();
	}

	public override void OnStateEnter()
	{
		_dustController.EnableWalkParticles();
	}

	public override void OnStateExit()
	{
		_dustController.DisableWalkParticles();
	}

	public override void OnUpdate() { }
}
