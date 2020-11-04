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
        var scriptPlayable = ScriptPlayable<DialogueControlMix>.Create(graph, inputCount); 
        DialogueControlMix behaviour = scriptPlayable.GetBehaviour();

        behaviour.ClipsEndTime = new List<double>();
        behaviour.ClipsStartTime = new List<double>();

        // Get all the clip information
        foreach (var clip in GetClips())
        {
            if(behaviour.ClipsEndTime.Contains(clip.end) == false)
                behaviour.ClipsEndTime.Add(clip.end);  // Save them in DialogueControlMix 
            if (behaviour.ClipsStartTime.Contains(clip.start) == false)
                behaviour.ClipsStartTime.Add(clip.start); 
        }  

        return scriptPlayable;
    }
}
