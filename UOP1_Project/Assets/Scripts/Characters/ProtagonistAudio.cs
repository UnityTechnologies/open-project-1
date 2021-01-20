using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistAudio : MonoBehaviour
{
	[SerializeField] private AudioCueEventChannelSO _sfxEventChannel = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	[SerializeField] private AudioCueSO caneSwing, objectPickup, footstep;

	public void PlayFootstep() => _sfxEventChannel.RaiseEvent(footstep, _audioConfig, transform.position);
	public void PlayCaneSwing() => _sfxEventChannel.RaiseEvent(caneSwing, _audioConfig, transform.position);
	public void PlayObjectPickup() => _sfxEventChannel.RaiseEvent(objectPickup, _audioConfig, transform.position);
}
