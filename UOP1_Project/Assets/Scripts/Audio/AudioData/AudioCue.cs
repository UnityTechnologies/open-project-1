using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple implementation of a MonoBehaviour that is able to request a sound being played by the <c>AudioManager</c>.
/// It fires an event on an <c>AudioCueEventSO</c> which acts as a channel, that the <c>AudioManager</c> will pick up and play.
/// </summary>
public class AudioCue : MonoBehaviour
{
	[SerializeField] private AudioCueSO _audioCue = default;
	[SerializeField] private AudioCueEventSO _audioCueEventChannel = default;
	[SerializeField] private AudioConfigurationSO _audioConfiguration = default;

	public void PlayAudioCue()
	{
		_audioCueEventChannel.Raise(_audioCue, _audioConfiguration, transform.position);
	}
}
