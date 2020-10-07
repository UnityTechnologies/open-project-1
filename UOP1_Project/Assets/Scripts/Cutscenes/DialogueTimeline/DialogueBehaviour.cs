using System;
using UnityEngine;
using UnityEngine.Playables;

namespace UOP1.Cutscenes
{
	[Serializable]
	public class DialogueBehaviour : PlayableBehaviour
	{
		[Tooltip("If enabled the graph will be stopped when this clip finished")] public bool stopGraphOnClipEnd;
		[Tooltip("Id of dialogue line to be assigned to the DialogBinder")] public string dialogueID;

		public override void OnPlayableCreate(Playable playable)
		{

		}
	}
}
