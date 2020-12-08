using System;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// DialogueLine is a Scriptable Object that represents one line of spoken dialogue.
/// It references the Actor that speaks it.
/// </summary>
[CreateAssetMenu(fileName = "newLineOfDialogue", menuName = "Dialogues/Line of Dialogue")]
public class DialogueLineSO : ScriptableObject
{
	public ActorSO Actor { get => _actor; }
	public LocalizedString Sentence { get => _sentence; }

	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private LocalizedString _sentence = default; //TODO: Connect this with localisation
}
