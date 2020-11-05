using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UOP1.Cutscenes
{
	[Serializable]
	public class DialogueClip : PlayableAsset, ITimelineClipAsset
	{
		public DialogueBehaviour dialogue = new DialogueBehaviour();

		public ClipCaps clipCaps
		{
			get { return ClipCaps.None; }
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, dialogue);
			return playable;
		}
	}
}
