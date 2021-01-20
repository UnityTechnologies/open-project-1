using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple implementation of a MonoBehaviour that is able to request a sound being played by the <c>AudioManager</c>.
/// It fires an event on an <c>AudioCueEventSO</c> which acts as a channel, that the <c>AudioManager</c> will pick up and play.
/// </summary>
public class AudioCue : MonoBehaviour
{
	[Header("Sound definition")]
	[SerializeField] private AudioCueSO _audioCue = default;
	[SerializeField] private bool _playOnStart = false;

	[Header("Configuration")]
	[SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;
	[SerializeField] private AudioConfigurationSO _audioConfiguration = default;

	private void Start()
	{
		if (_playOnStart)
			StartCoroutine(PlayDelayed());
	}

	private IEnumerator PlayDelayed()
	{
		yield return new WaitForSeconds(.1f);

		PlayAudioCue();
	}

	public void PlayAudioCue()
	{
		_audioCueEventChannel.RaiseEvent(_audioCue, _audioConfiguration, transform.position);
	}
}
