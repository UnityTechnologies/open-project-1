using UnityEngine;

public class ProtagonistAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO caneSwing, liftoff, land, objectPickup, footstep, getHit, die, talk;

	public void PlayFootstep() => PlayAudio(footstep, _audioConfig, transform.position);
	public void PlayJumpLiftoff() => PlayAudio(liftoff, _audioConfig, transform.position);
	public void PlayJumpLand() => PlayAudio(land, _audioConfig, transform.position);
	public void PlayCaneSwing() => PlayAudio(caneSwing, _audioConfig, transform.position);
	public void PlayObjectPickup() => PlayAudio(objectPickup, _audioConfig, transform.position);
	public void PlayGetHit() => PlayAudio(getHit, _audioConfig, transform.position);
	public void PlayDie() => PlayAudio(die, _audioConfig, transform.position);
	public void PlayTalk() => PlayAudio(talk, _audioConfig, transform.position);
}
