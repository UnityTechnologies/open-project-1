using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private PlayableDirector _playableDirector = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;

	[SerializeField] private PlayableDirectorChannelSO _playCutsceneEvent = default;

	private void Start()
	{
		if (_playOnStart)
			if (_playCutsceneEvent != null)
				_playCutsceneEvent.RaiseEvent(_playableDirector);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_playCutsceneEvent != null)
			_playCutsceneEvent.RaiseEvent(_playableDirector);

		if (_playOnce)
			Destroy(this);
	}
}
