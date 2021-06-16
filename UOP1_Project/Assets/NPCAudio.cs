using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO idleSound, talk, footstep;

	//when we have the ground detector script, we should check the type to know which footstep sound to play
	public void PlayFootstep() => _sfxEventChannel.RaisePlayEvent(footstep, _audioConfig, transform.position);
	public void PlayTalk() => _sfxEventChannel.RaisePlayEvent(talk, _audioConfig, transform.position);
	//Only bard hare will use the Idle since he sings at that time
	public void PlayIdle() => _sfxEventChannel.RaisePlayEvent(idleSound, _audioConfig, transform.position);

}
