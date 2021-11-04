using UnityEngine;

public class ProtagonistAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO _caneSwing;
	[SerializeField] private AudioCueSO _liftoff;
	[SerializeField] private AudioCueSO _land;
	[SerializeField] private AudioCueSO _objectPickup;
	[SerializeField] private AudioCueSO _footsteps;
	[SerializeField] private AudioCueSO _getHit;
	[SerializeField] private AudioCueSO _die;
	[SerializeField] private AudioCueSO _talk;

	public void PlayFootstep() => PlayAudio(_footsteps, _audioConfig, transform.position);
	public void PlayJumpLiftoff() => PlayAudio(_liftoff, _audioConfig, transform.position);
	public void PlayJumpLand() => PlayAudio(_land, _audioConfig, transform.position);
	public void PlayCaneSwing() => PlayAudio(_caneSwing, _audioConfig, transform.position);
	public void PlayObjectPickup() => PlayAudio(_objectPickup, _audioConfig, transform.position);
	public void PlayGetHit() => PlayAudio(_getHit, _audioConfig, transform.position);
	public void PlayDie() => PlayAudio(_die, _audioConfig, transform.position);
	public void PlayTalk() => PlayAudio(_talk, _audioConfig, transform.position);
}
