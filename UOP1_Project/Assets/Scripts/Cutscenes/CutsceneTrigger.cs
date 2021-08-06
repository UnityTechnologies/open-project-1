using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	private PlayableDirector _playableDirector = default;
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;

	[Header("Listening to channels")]
	[SerializeField] private VoidEventChannelSO _playSpeceficCutscene = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private PlayableDirectorChannelSO _playCutsceneEvent = default;


	private void Start()
	{
		_playableDirector = GetComponent<PlayableDirector>();
		if (_playOnStart)
			if (_playCutsceneEvent != null)
				_playCutsceneEvent.RaiseEvent(_playableDirector);
	}

	private void OnEnable()
	{
		_playSpeceficCutscene.OnEventRaised += PlaySpeceficCutscene;
	}
	private void OnDisable()
	{
		_playSpeceficCutscene.OnEventRaised -= PlaySpeceficCutscene;
	}

	void PlaySpeceficCutscene()
	{
		if (_playCutsceneEvent != null)
			_playCutsceneEvent.RaiseEvent(_playableDirector);

		if (_playOnce)
			Destroy(this);
	}

	//THIS WILL BE REMOVED LATER WHEN WE HAVE ALL EVENTS SET UP, NOW WE ONLY NEED IT TO TEST CUTSCENE WITH TRIGGER
	//Remember to remove collider componenet when we remove this
	private void OnTriggerEnter(Collider other)
	{
		//Fake event raise to test quicker
		_playSpeceficCutscene.RaiseEvent();

		//if (_playCutsceneEvent != null)
		//	_playCutsceneEvent.RaiseEvent(_playableDirector);

		//if (_playOnce)
		//	Destroy(this);
	}
}
