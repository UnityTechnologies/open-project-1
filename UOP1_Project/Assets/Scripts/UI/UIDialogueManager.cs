using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

// NOTE TO PR-MERGER: I have marked a few sections with the region tags "DELETABLE_DEMO_CODE"...this was code specifically used for the demo that is not
// critical to the operation of the Expression System itself...and, as its name suggests, it can be deleted without any loss of core functionality

public class UIDialogueManager : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent lineText = default;
	[SerializeField] LocalizeStringEvent actorNameText = default;

	#region DELETABLE_DEMO_CODE
	[SerializeField] LocalizeStringEvent secondActorNameText = default;
	[SerializeField] LocalizeStringEvent thirdActorNameText  = default;
	[SerializeField] GameObject backgroundPanel;
	#endregion

	public void SetDialogue(DialogueLineSO dialogueLine)
	{
		lineText.StringReference = dialogueLine.Sentence;
		actorNameText.StringReference = dialogueLine.Actor.ActorName;
		secondActorNameText.StringReference = dialogueLine.Actor.ActorName;
		thirdActorNameText.StringReference = dialogueLine.Actor.ActorName;

		#region DELETABLE_DEMO_CODE
		// Extract the localized string
		var stringOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(dialogueLine.Actor.ActorName.TableReference, dialogueLine.Actor.ActorName.TableEntryReference);

		string actorName = null;
		actorName = stringOp.Result;
		if (stringOp.IsDone)
		{
			actorName = stringOp.Result;
		}
		else
		{
			stringOp.Completed += (op) => actorName = op.Result;
		}

		if (actorName == "Hamlet")
		{
			backgroundPanel.GetComponent<Image>().color = Color.white;
			lineText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;

			secondActorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
			thirdActorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
		}

		if (actorName == "Evil Hamlet")
		{
			backgroundPanel.GetComponent<Image>().color = Color.black;
			lineText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;

			actorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
			thirdActorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
		}

		if (actorName == "Townsfolk")
		{
			backgroundPanel.GetComponent<Image>().color = Color.blue;
			lineText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;

			actorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
			secondActorNameText.gameObject.GetComponent<TextMeshProUGUI>().text = "";
		}

		#endregion
	}
}
