using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>
public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private bool _playOnStart = default;
	[SerializeField] private bool _playOnce = default;
	[SerializeField] private QuestManagerSO _questManager = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO _playSpeceficCutscene = default;

	[Header("Broadcasting on")]
	[SerializeField] private PlayableDirectorChannelSO _playCutsceneEvent = default;

	private PlayableDirector _playableDirector = default;

	private void Start()
	{
		_playableDirector = GetComponent<PlayableDirector>();
		if (_playOnStart)
			if (_playCutsceneEvent != null)
				_playCutsceneEvent.RaiseEvent(_playableDirector);

		//Check if we are playing a new game, we should play the intro cutscene
		if (_questManager)
		{
			if (_questManager.IsNewGame())
			{
				_playableDirector.Play();
			}
		}
}

	private void OnEnable()
	{
		_playSpeceficCutscene.OnEventRaised += PlaySpecificCutscene;
	}
	private void OnDisable()
	{
		_playSpeceficCutscene.OnEventRaised -= PlaySpecificCutscene;
	}

	void PlaySpecificCutscene()
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
	}
}
