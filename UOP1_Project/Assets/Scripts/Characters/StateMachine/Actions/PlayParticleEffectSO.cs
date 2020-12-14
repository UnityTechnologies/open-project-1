using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using System.Collections;
using Moment = UOP1.StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "PlayParticleEffect", menuName = "State Machines/Actions/Play Particle Effect")]
public class PlayParticleEffectSO : StateActionSO
{
	public ParticleEffectSO particleEffect;
	public Moment whenToPlay = default;
	public bool stopOnStateExit = default;
	public float coolDown = 0.0f;
	protected override StateAction CreateAction() => new PlayParticleEffect();
}

public class PlayParticleEffect : StateAction
{

	private PlayParticleEffectSO _parentSO;
	private EffectController _effectController;
	private float t = 0f;

	public override void Awake(StateMachine stateMachine)
	{
		_parentSO = (PlayParticleEffectSO)OriginSO;
		_effectController = stateMachine.GetComponent<EffectController>();
	}
		
	public override void OnUpdate()
	{

	}

	public override void OnStateEnter()
	{
		if (Moment.OnStateEnter == _parentSO.whenToPlay && _effectController != null)
		{
			TriggerParticuleEffect();
		}
	}

	public override void OnStateExit()
	{
		if (Moment.OnStateExit == _parentSO.whenToPlay)
		{
			TriggerParticuleEffect();
		}
		if (_parentSO.stopOnStateExit && _effectController != null)
		{
			_effectController.StopEffect(_parentSO.particleEffect);
		}
	}

	private void TriggerParticuleEffect()
	{
		if (Time.time >= t + _parentSO.coolDown)
		{
			_effectController.PlayEffect(_parentSO.particleEffect);
		}
	}
}
