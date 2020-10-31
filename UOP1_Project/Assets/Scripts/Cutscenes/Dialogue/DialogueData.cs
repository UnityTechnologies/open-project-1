using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "CutsceneSystem/DialogueData")]
public class DialogueData : ScriptableObject
{
	[Tooltip("Leave this empty if the dialogue doesn't use timeline.")]
	public TimelineAsset TimelineAsset;
	public List<DialogueLine> Conversation; 
}
