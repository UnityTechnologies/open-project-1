using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.Localization.Components;
public class UIDialogueManager : MonoBehaviour
{

	[SerializeField]  private LocalizeStringEvent _lineText = default;

	[SerializeField] private LocalizeStringEvent _actorNameText = default;

	[SerializeField] private UIDialogueChoicesManager _choicesManager = default;

	[SerializeField] private DialogueChoicesChannelSO ShowChoicesEvent; 
	private void Start()
	{
		if (ShowChoicesEvent != null)
		{
			ShowChoicesEvent.OnEventRaised += ShowChoices;
		}
	}
	public void SetDialogue(DialogueLineSO dialogueLine, ActorSO actor)
	{
		_choicesManager.gameObject.SetActive(false);
		_lineText.StringReference = dialogueLine.Sentence;
		_actorNameText.StringReference = actor.ActorName;

	}

     void ShowChoices(List<Choice> choices) {
		
		_choicesManager.FillChoices(choices);
		_choicesManager.gameObject.SetActive(true);
	}
	void HideChoices()
	{
		_choicesManager.gameObject.SetActive(false);
	}
}
