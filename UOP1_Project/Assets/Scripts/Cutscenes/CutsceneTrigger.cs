using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneData CutsceneData;
	[Tooltip("Play the cutscene on the Start, otherwise the cutscene will be triggered by colider. Make sure no overlap")]
	[SerializeField] private bool PlayOnStart;
	[SerializeField] private bool NeedToPressButton;
	[SerializeField] private bool onlyOnce;

	#region Default transform
	private Vector3 position;
	private Quaternion rotation;
	#endregion
	private void Awake()
	{
		position = transform.position;
		rotation = transform.rotation;
	}

	private void Start()
	{
		if (PlayOnStart)
		{ 
			CutsceneManager.Instance.Play(CutsceneData);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!NeedToPressButton)
		{ 
			CutsceneManager.Instance.Play(CutsceneData);
		}
	}

	private void OnTriggerStay(Collider other)
	{ 
		transform.LookAt(other.transform.position);

		if (NeedToPressButton)
		{
			if (!CutsceneManager.Instance.IsInteracting)
			{
				CutsceneManager.Instance.AbleToInteract(other.transform.position, CutsceneData);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		CutsceneManager.Instance.NotAbleToInteract();
		transform.position = position;
		transform.rotation = rotation;
	}
}
