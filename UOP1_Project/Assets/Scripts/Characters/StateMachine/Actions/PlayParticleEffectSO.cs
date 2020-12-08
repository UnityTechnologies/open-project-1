using UnityEngine;
using UOP1.StateMachine;
using UOP1.StateMachine.ScriptableObjects;
using System.Collections;
using Moment = UOP1.StateMachine.StateAction.SpecificMoment;

[CreateAssetMenu(fileName = "PlayParticleEffect", menuName = "State Machines/Actions/Play Particle Effect")]
public class PlayParticleEffectSO : StateActionSO
{
	public ParticleSystem particlesPrefab;
	public Moment whenToRun = default;
	public bool stopOnStateExit = default;
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
		GameObject.Destroy(particleSystem.gameObject);
	}

	public override void OnStateEnter()
	{
		if (Moment.OnStateEnter == _parentSO.whenToRun)
		{
			TriggerParticuleEffect();
		}
	}

	public override void OnStateExit()
	{
		if (Moment.OnStateExit == _parentSO.whenToRun)
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
		_particles = GameObject.Instantiate(_parentSO.particlesPrefab, _stateMachine.transform);
		_stateMachine.StartCoroutine(TriggerEffectCoroutine(_particles));
	}
}
