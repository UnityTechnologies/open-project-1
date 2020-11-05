using UnityEngine;

/// <summary>
/// Class to trigger a cutscene.
/// </summary>

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneData CutsceneData;
	[Tooltip("Play the cutscene on the Start, otherwise the cutscene will be triggered by colider. Make sure no overlap")]
	[SerializeField] private bool _playOnStart;
	[SerializeField] private bool _needToPressButton;
	[Tooltip("Only once only work if NeedToPressButton false")]
	[SerializeField] private bool _onlyOnce; 

	private Vector3 position;
	private Quaternion rotation;

	private void Awake()
	{
		position = transform.position;
		rotation = transform.rotation;
	}

	private void Start()
	{
		if (_playOnStart)
		{ 
			//CutsceneManager.Instance.Play(CutsceneData);
		}
	}

	private void OnTriggerEnter(Collider other)
	{ 
		if (!_needToPressButton)
		{ 
			//CutsceneManager.Instance.Play(CutsceneData);

			if (_onlyOnce)
			{
				Destroy(this);
			}
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if(_needToPressButton)
			transform.LookAt(other.transform.position);

		if (_needToPressButton)
		{
			/*if (!CutsceneManager.Instance.IsInteracting)
			{
				CutsceneManager.Instance.AbleToInteract(other.transform.position, CutsceneData);
			}*/
		}
	}

	private void OnTriggerExit(Collider other)
	{
		//CutsceneManager.Instance.NotAbleToInteract();
		transform.position = position;
		transform.rotation = rotation;
	}
}
