using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(CutsceneManager))]
[TrackClipType(typeof(DialogueControlClip))]
public class DialogueControlTrack : PlayableTrack
{  
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var scriptPlayable = ScriptPlayable<DialogueControlMixerBehaviour>.Create(graph, inputCount); 
        DialogueControlMixerBehaviour behaviour = scriptPlayable.GetBehaviour();

        // Get all the clip information
        foreach (TimelineClip clip in GetClips())
        {
           // Needed?
        }  

        return scriptPlayable;
    }
}
