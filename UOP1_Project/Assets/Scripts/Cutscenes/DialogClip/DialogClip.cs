using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogClip : PlayableAsset, ITimelineClipAsset
{
    public DialogBehaviour dialog = new DialogBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogBehaviour>.Create (graph, dialog);
        return playable;
    }
}
