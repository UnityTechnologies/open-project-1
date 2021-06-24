using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += PlayMusic;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= PlayMusic;
	}

	private void PlayMusic()
	{
		_playMusicOn.RaisePlayEvent(_thisSceneSO.musicTrack, _audioConfig);

	}
}
