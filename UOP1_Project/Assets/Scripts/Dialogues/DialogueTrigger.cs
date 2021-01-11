using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private DialogueManager _dialogueManager = default;
	[SerializeField] private DialogueDataSO _dialogueData = default;

	private void OnTriggerEnter(Collider other)
	{
		_dialogueManager.DisplayDialogueData(_dialogueData);
	}
}
