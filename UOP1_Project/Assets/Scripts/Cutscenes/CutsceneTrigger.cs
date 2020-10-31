using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private CutsceneData CutsceneData;
	[Tooltip("Play the cutscene on the Start, otherwise the cutscene will be triggered by colider. Make sure no overlap")]
	[SerializeField] private bool PlayOnStart;
	[SerializeField] private bool NeedToPressButton;
	[Tooltip("Only once only work if NeedToPressButton false")]
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

			if (onlyOnce)
			{
				Destroy(this);
			}
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if(NeedToPressButton)
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
