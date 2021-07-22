using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class DialoguePreview : MonoBehaviour
{
	private VisualElement CreateDialoguePreviewWithBranching(DialogueDataSO dialogueDataSO)
	{
		VisualElement dialoguePreviewVE = new VisualElement();

		/*DialogueDataSO currentDialogue = dialogueDataSO;

		foreach (LocalizedString localizedString in currentDialogue.DialogueLines)
		{
			Label dialogueLine = new Label();
			dialogueLine.text = localizedString.GetLocalizedStringImmediateSafe();
			dialoguePreviewVE.Add(dialogueLine);
		}
		if (currentDialogue.Choices != null)
		{
			foreach (Choice choice in currentDialogue.Choices)
			{
				Button dialogueButton = new Button();
				dialogueButton.text = choice.Response.GetLocalizedStringImmediateSafe();
				dialoguePreviewVE.Add(dialogueButton);
				dialoguePreviewVE.Add(CreateDialoguePreviewWithBranching(choice.NextDialogue));
			}
		}
		*/

		return dialoguePreviewVE;
	}

}
