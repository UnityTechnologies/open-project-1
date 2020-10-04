using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogBehaviour : PlayableBehaviour
{
    [Tooltip("If enabled the graph will be stopped when this clip finished")] public bool stopGraphOnClipEnd;
    [Tooltip("Id of dialog line to be assigned to the DialogBinder")] public string dialogID;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
