using UnityEngine;

public class NPCAudio : CharacterAudio
{
	[SerializeField] private AudioCueSO singShort, singLong, talk, footstep;

	//when we have the ground detector script, we should check the type to know which footstep sound to play
	public void PlayFootstep() => PlayAudio(footstep, _audioConfig, transform.position);
	public void PlayTalk() => PlayAudio(talk, _audioConfig, transform.position);
	//Only bard hare will use the Idle since he sings at that time
	public void PlaySingShort() => PlayAudio(singShort, _audioConfig, transform.position);
	public void PlaySingLong() => PlayAudio(singLong, _audioConfig, transform.position);

}
