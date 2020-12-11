using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using System.Collections;
using Moment = UOP1.StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "PlayParticleEffect", menuName = "State Machines/Actions/Play Particle Effect")]
public class PlayParticleEffectSO : StateActionSO
{
	public ParticleEffectPoolSO particlePool;
	public Moment whenToPlay = default;
	public bool stopOnStateExit = default;
	public Transform anchor = default;
	protected override StateAction CreateAction() => new PlayParticleEffect();
}

public class PlayParticleEffect : StateAction
{
	private ParticleSystem _particles;
	private PlayParticleEffectSO _parentSO;
	private StateMachine _stateMachine;

	public override void Awake(StateMachine stateMachine)
	{
		_parentSO = (PlayParticleEffectSO)OriginSO;
		_stateMachine = stateMachine;
	}
		
	public override void OnUpdate()
	{
	}

	private IEnumerator TriggerEffectCoroutine(ParticleSystem particleSystem)
	{
		particleSystem.Play();

		while (particleSystem.IsAlive())
		{
			yield return null;
		}
		_parentSO.particlePool.Return(particleSystem);
	}

	public override void OnStateEnter()
	{
		if (Moment.OnStateEnter == _parentSO.whenToPlay)
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
		if (_parentSO.stopOnStateExit)
		{
			if (_particles != null)
			{
				_particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	private void TriggerParticuleEffect()
	{
		_particles = _parentSO.particlePool.Request();

		_particles.transform.SetParent(_stateMachine.transform);

		_particles.transform.localPosition = _parentSO.anchor.localPosition;
		_particles.transform.localRotation = _parentSO.anchor.localRotation;
		_particles.transform.localScale = _parentSO.anchor.localScale;

		_stateMachine.StartCoroutine(TriggerEffectCoroutine(_particles));
	}
}
