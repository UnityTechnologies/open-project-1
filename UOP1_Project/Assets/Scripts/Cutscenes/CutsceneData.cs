using System; 
using UnityEngine;

[Serializable]
public class CutsceneData
{
	// We can make this a list
	public DialogueDataSO DialogueData;
	[Tooltip("Event placeholder that will be called after DialogueData reached its end.")]
	public string Event; 
}
