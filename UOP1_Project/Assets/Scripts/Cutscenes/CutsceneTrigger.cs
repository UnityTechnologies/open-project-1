using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneManager _cutSceneManager;
	[SerializeField] private PlayableDirector _playableDirector;
	[SerializeField] private bool _playOnStart;
	[SerializeField] private bool _playOnce;

	private void Start()
	{
		if(_playOnStart)
			_cutSceneManager.Play(_playableDirector);
	}

	private void OnTriggerEnter(Collider other)
	{
		_cutSceneManager.Play(_playableDirector);

		if(_playOnce)
			Destroy(this);
	}
}
