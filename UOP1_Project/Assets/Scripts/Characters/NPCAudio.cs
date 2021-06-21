using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO singShort, singLong, talk, footstep;

	//when we have the ground detector script, we should check the type to know which footstep sound to play
	public void PlayFootstep() => _sfxEventChannel.RaisePlayEvent(footstep, _audioConfig, transform.position);
	public void PlayTalk() => _sfxEventChannel.RaisePlayEvent(talk, _audioConfig, transform.position);
	//Only bard hare will use the Idle since he sings at that time
	public void PlaySingShort() => _sfxEventChannel.RaisePlayEvent(singShort, _audioConfig, transform.position);
	public void PlaySingLong() => _sfxEventChannel.RaisePlayEvent(singLong, _audioConfig, transform.position);

}
