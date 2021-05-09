using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class UIDialogueManager : MonoBehaviour
{

	[SerializeField] private LocalizeStringEvent _lineText = default;

	[SerializeField] private LocalizeStringEvent _actorNameText = default;

	[SerializeField] private UIDialogueChoicesManager _choicesManager = default;

	[SerializeField] private DialogueChoicesChannelSO _showChoicesEvent = default;
	private void Start()
	{
		
			_showChoicesEvent.OnEventRaised += ShowChoices;
		
	}
	public void SetDialogue(LocalizedString dialogueLine, ActorSO actor)
	{
		_choicesManager.gameObject.SetActive(false);
		_lineText.StringReference = dialogueLine;
		_actorNameText.StringReference = actor.ActorName;

	}

	void ShowChoices(List<Choice> choices)
	{

		_choicesManager.FillChoices(choices);
		_choicesManager.gameObject.SetActive(true);
	}
	void HideChoices()
	{
		_choicesManager.gameObject.SetActive(false);
	}
}
