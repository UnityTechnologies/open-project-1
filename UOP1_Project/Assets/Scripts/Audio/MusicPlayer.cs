using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;
	[SerializeField] private AudioCueEventChannelSO _playMusicOn = default;
	[SerializeField] private GameSceneSO _thisSceneSO = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

	[Header("Pause menu music")]
	[SerializeField] private AudioCueSO _pauseMusic = default;
	[SerializeField] private BoolEventChannelSO _onPauseOpened = default; 

	private void OnEnable()
	{
		_onPauseOpened.OnEventRaised += PlayPauseMusic;
		_onSceneReady.OnEventRaised += PlayMusic;
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= PlayMusic;
		_onPauseOpened.OnEventRaised -= PlayPauseMusic;
	}

	private void PlayMusic()
	{
		_playMusicOn.RaisePlayEvent(_thisSceneSO.musicTrack, _audioConfig);
	}

	private void PlayPauseMusic(bool open)
	{
		if (open)
			_playMusicOn.RaisePlayEvent(_pauseMusic, _audioConfig);
		else
			PlayMusic();
	}
}
