using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO caneSwing, liftoff, land, objectPickup, footstep, getHit, die;

	public void PlayFootstep() => _sfxEventChannel.RaisePlayEvent(footstep, _audioConfig, transform.position);
	public void PlayJumpLiftoff() => _sfxEventChannel.RaisePlayEvent(liftoff, _audioConfig, transform.position);
	public void PlayJumpLand() => _sfxEventChannel.RaisePlayEvent(land, _audioConfig, transform.position);
	public void PlayCaneSwing() => _sfxEventChannel.RaisePlayEvent(caneSwing, _audioConfig, transform.position);
	public void PlayObjectPickup() => _sfxEventChannel.RaisePlayEvent(objectPickup, _audioConfig, transform.position);
	public void PlayGetHit() => _sfxEventChannel.RaisePlayEvent(getHit, _audioConfig, transform.position);
	public void PlayDie() => _sfxEventChannel.RaisePlayEvent(die, _audioConfig, transform.position);

}
