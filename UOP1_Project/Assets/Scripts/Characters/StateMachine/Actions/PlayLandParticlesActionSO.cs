using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Actions/Play Land Particles")]
public class PlayLandParticlesActionSO : StateActionSO<PlayLandParticlesAction> { }

public class PlayLandParticlesAction : StateAction
{
	//Component references
	private PlayerEffectController _dustController;
	private Transform _transform;

	private float _coolDown = 0.3f;
	private float t = 0f;

	private float _fallStartY = 0f;
	private float _fallEndY = 0f;
	private float _maxFallDistance = 4f; //Used to adjust particle emission intensity

	public override void Awake(StateMachine stateMachine)
	{
		_dustController = stateMachine.GetComponent<PlayerEffectController>();
		_transform = stateMachine.transform;
	}

	public override void OnStateEnter()
	{
		_fallStartY = _transform.position.y;
	}

	public override void OnStateExit()
	{
		_fallEndY = _transform.position.y;
		float dY = Mathf.Abs(_fallStartY - _fallEndY);
		float fallIntensity = Mathf.InverseLerp(0, _maxFallDistance, dY);

		if (Time.time >= t + _coolDown)
		{
			_dustController.PlayLandParticles(fallIntensity);
			t = Time.time;
		}
	}

	public override void OnUpdate() { }
}
