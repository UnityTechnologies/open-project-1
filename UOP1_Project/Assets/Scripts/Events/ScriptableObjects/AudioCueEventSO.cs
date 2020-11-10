using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Event on which <c>AudioCue</c> components send a message to play SFX and music. <c>AudioManager</c> listens on these events, and actually plays the sound.
/// </summary>
[CreateAssetMenu(menuName = "Game Event/Audio Cue")]
public class AudioCueEventSO : ScriptableObject
{
	public UnityAction<AudioCueSO, AudioConfigurationSO, Vector3> eventRaised;

	public void Raise(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
	{
		eventRaised.Invoke(audioCue, audioConfiguration, positionInSpace);
	}
}
