using UnityEngine;
using UnityEngine.Playables;

public class DialogueControlClip : PlayableAsset
{
    public DialogueControlBehaviour template; 

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph, template);
        DialogueControlBehaviour clone = playable.GetBehaviour();
        // You can edit the clone here
        return playable;
    }
}
