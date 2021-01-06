using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : PlayableTrack
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		
		foreach (TimelineClip clip in GetClips())
		{
			DialogueClip dialogueControlClip = clip.asset as DialogueClip;
		}

		return base.CreateTrackMixer(graph, go, inputCount);
	}
}
