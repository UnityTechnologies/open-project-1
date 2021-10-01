using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] protected AudioCueEventChannelSO _sfxEventChannel = default;
	[SerializeField] protected AudioConfigurationSO _audioConfig = default;
	[SerializeField] protected GameStateSO _gameState = default;
	
	protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
	{
		if (_gameState.CurrentGameState != GameState.Cutscene)
			_sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
	}
}
