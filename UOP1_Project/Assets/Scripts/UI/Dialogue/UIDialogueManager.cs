using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class UIDialogueManager : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent _lineText = default;
	[SerializeField] private LocalizeStringEvent _actorNameText = default;
	[SerializeField] private GameObject _actorNamePanel = default;
	[SerializeField] private GameObject _mainProtagonistNamePanel = default;
	[SerializeField] private UIDialogueChoicesManager _choicesManager = default;

	[Header("Listening to")]
	[SerializeField] private DialogueChoicesChannelSO _showChoicesEvent = default;

	private void OnEnable()
	{
		_showChoicesEvent.OnEventRaised += ShowChoices;
	}

	private void OnDisable()
	{
		_showChoicesEvent.OnEventRaised -= ShowChoices;
	}

	public void SetDialogue(LocalizedString dialogueLine, ActorSO actor, bool isMainProtagonist)
	{
		_choicesManager.gameObject.SetActive(false);
		_lineText.StringReference = dialogueLine;

		_actorNamePanel.SetActive(!isMainProtagonist);
		_mainProtagonistNamePanel.SetActive(isMainProtagonist);

		if (!isMainProtagonist)
		{
			_actorNameText.StringReference = actor.ActorName;
		}
		//Protagonist's LocalisedString is provided on the GameObject already
	}

	private void ShowChoices(List<Choice> choices)
	{
		_choicesManager.FillChoices(choices);
		_choicesManager.gameObject.SetActive(true);
	}

	private void HideChoices()
	{
		_choicesManager.gameObject.SetActive(false);
	}
}
