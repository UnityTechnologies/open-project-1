using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO caneSwing, objectPickup, footstep, getHit, die;

	public void PlayFootstep() => _sfxEventChannel.RaisePlayEvent(footstep, _audioConfig, transform.position);
	public void PlayCaneSwing() => _sfxEventChannel.RaisePlayEvent(caneSwing, _audioConfig, transform.position);
	public void PlayObjectPickup() => _sfxEventChannel.RaisePlayEvent(objectPickup, _audioConfig, transform.position);
	public void PlayGetHit() => _sfxEventChannel.RaisePlayEvent(getHit, _audioConfig, transform.position);
	public void PlayDie() => _sfxEventChannel.RaisePlayEvent(die, _audioConfig, transform.position);

}
