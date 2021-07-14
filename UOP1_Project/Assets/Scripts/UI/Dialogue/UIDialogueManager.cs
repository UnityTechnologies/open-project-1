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

	[SerializeField] private LocalizeStringEvent _mainProtagonistNameText = default;

	[SerializeField] private GameObject _actorNamePanel = default;

	[SerializeField] private GameObject _mainProtagonistNamePanel = default;

	[SerializeField] private UIDialogueChoicesManager _choicesManager = default;

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
		else
		{
			_mainProtagonistNameText.StringReference = actor.ActorName;
		}
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
