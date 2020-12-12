using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Land Particles")]
public class PlayLandParticlesActionSO : StateActionSO<PlayLandParticlesAction> { }

public class PlayLandParticlesAction : StateAction
{
	//Component references
	private DustParticlesController _dustController;

	private float _coolDown = 0.3f;
	private float t = 0f;

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<DustParticlesController>();
	}

	public override void OnStateExit()
	{
		if (Time.time >= t + _coolDown)
		{
			_dustController.PlayLandParticles();
			t = Time.time;
		}
	}

	public override void OnUpdate() { }
}
