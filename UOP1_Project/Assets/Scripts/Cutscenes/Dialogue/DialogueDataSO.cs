using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "newDialogue", menuName = "CutsceneSystem/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
	[Tooltip("Leave this empty if the dialogue doesn't use timeline.")]
	public List<DialogueLineSO> Conversation;
}
