using UnityEngine;
using UnityEngine.Playables;

public class DialogueControlClip : PlayableAsset
{
    [SerializeField] private DialogueControlBehaviour _template; 

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph, _template);

        return playable;
    }
}
