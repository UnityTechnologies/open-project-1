using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newLineOfDialogue", menuName = "Dialogues/Line of Dialogue")]
public class DialogueLineSO : ScriptableObject
{
	public Actor Actor { get => _actor; }
	public string Sentence { get => _sentence; }

	[SerializeField] private Actor _actor = default;
	[SerializeField] [TextArea(3, 3)] private string _sentence = default;
}
