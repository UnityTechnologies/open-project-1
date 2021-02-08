using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem _slashEffect;
	[SerializeField]
	private ParticleSystem _reverseSlashEffect;

	private void Start()
	{
		_slashEffect.Stop();
		_reverseSlashEffect.Stop();
	}

	public void PlaySlash()
	{
		_slashEffect.Play();
	}

	public void PlayReverseSlash()
	{
		_reverseSlashEffect.Play();
	}

}
