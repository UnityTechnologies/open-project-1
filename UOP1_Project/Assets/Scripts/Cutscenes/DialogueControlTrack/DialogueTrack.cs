using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(CutsceneManager))]
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : PlayableTrack
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		CutsceneManager cutsceneManagerRef = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as CutsceneManager;

		foreach (TimelineClip clip in GetClips())
		{
			DialogueClip dialogueControlClip = clip.asset as DialogueClip;
			dialogueControlClip.cutsceneManager = cutsceneManagerRef;
		}

		return base.CreateTrackMixer(graph, go, inputCount);
	}
}
