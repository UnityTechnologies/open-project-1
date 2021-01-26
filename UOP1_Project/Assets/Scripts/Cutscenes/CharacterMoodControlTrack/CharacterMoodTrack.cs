using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackBindingType(typeof(ExpressionManager))]
[TrackClipType(typeof(CharacterMoodClip))]
[TrackColor(0.0f, 0.0f, 0.0f)]
public class CharacterMoodTrack : PlayableTrack
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ExpressionManager em = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as ExpressionManager;

		foreach (TimelineClip clip in GetClips())
		{
			CharacterMoodClip moodClip = clip.asset as CharacterMoodClip;

			moodClip.ExpressionManager = em;
		}

		return base.CreateTrackMixer(graph, go, inputCount);
	}
}
