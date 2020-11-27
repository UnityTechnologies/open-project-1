using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UOP1.StateMachine;

/// <summary>
/// Controls playback of particles connected to movement. Methods invoked by the StateMachine StateActions
/// </summary>
public class DustParticlesController : MonoBehaviour
{
	[SerializeField] ParticleSystem _walkingParticles = default;
	[SerializeField] ParticleSystem _landParticles = default;
	[SerializeField] ParticleSystem _jumpParticles = default;

	public void EnableWalkParticles()
	{
		_walkingParticles.Play();
	}

	public void DisableWalkParticles()
	{
		_walkingParticles.Stop();
	}

	public void PlayJumpParticles()
	{
		_jumpParticles.Play();
	}
	public void PlayLandParticles()
	{
		_landParticles.Play();
	}
}
