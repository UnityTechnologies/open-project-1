using UnityEngine;

public class CritterAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO idleSound, moveSound, attackSound, gettingHitSound, deathSound;

	public void PlayIdleSound() => PlayAudio(idleSound, _audioConfig, transform.position);
	//The move sound will not be called for the plant critter
	public void PlayMoveSound() => PlayAudio(moveSound, _audioConfig, transform.position);
	public void PlayAttackSound() => PlayAudio(attackSound, _audioConfig, transform.position);
	public void PlayGettingHitSound() => PlayAudio(gettingHitSound, _audioConfig, transform.position);
	public void PlayDeathSound() => PlayAudio(deathSound, _audioConfig, transform.position);

}
