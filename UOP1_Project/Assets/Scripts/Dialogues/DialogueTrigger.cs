using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private DialogueManager _dialogueManager;
	[SerializeField] private DialogueDataSO _dialogueData;

	private void OnTriggerEnter(Collider other)
	{
		_dialogueManager.DisplayDialogueData(_dialogueData);
	}
}
