using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO idleSound, moveSound, attackSound, gettingHitSound, deathSound;

	public void PlayIdleSound() => _sfxEventChannel.RaisePlayEvent(idleSound, _audioConfig, transform.position);
	//The move sound will not be called for the plant critter
	public void PlayMoveSound() => _sfxEventChannel.RaisePlayEvent(moveSound, _audioConfig, transform.position);
	public void PlayAttackSound() => _sfxEventChannel.RaisePlayEvent(attackSound, _audioConfig, transform.position);
	public void PlayGettingHitSound() => _sfxEventChannel.RaisePlayEvent(gettingHitSound, _audioConfig, transform.position);
	public void PlayDeathSound() => _sfxEventChannel.RaisePlayEvent(deathSound, _audioConfig, transform.position);

}
