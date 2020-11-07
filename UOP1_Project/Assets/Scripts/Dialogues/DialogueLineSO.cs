using System;
using UnityEngine;

/// <summary>
/// DialogueLine is a Scriptable Object that represents one line of spoken dialogue.
/// It references the Actor that speaks it.
/// </summary>
[CreateAssetMenu(fileName = "newLineOfDialogue", menuName = "Dialogues/Line of Dialogue")]
public class DialogueLineSO : ScriptableObject
{
	public ActorSO Actor { get => _actor; }
	public string Sentence { get => _sentence; }

	[SerializeField] private ActorSO _actor = default;
	[SerializeField] [TextArea(3, 3)] private string _sentence = default; //TODO: Connect this with localisation
}
