using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.0500623f, 0.3788047f, 0.7075472f)]
[TrackClipType(typeof(DialogClip))]
[TrackBindingType(typeof(DialogBinder))]
public class DialogTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var trackMixer = ScriptPlayable<DialogMixerBehaviour>.Create (graph, inputCount);
        trackMixer.GetBehaviour().SetClips(GetClips() as TimelineClip[]);
        return trackMixer;
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        DialogBinder trackBinding = director.GetGenericBinding(this) as DialogBinder;
        if (trackBinding == null)
            return;

        var serializedObject = new UnityEditor.SerializedObject (trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;

            driver.AddFromName<DialogBinder>( trackBinding.gameObject, iterator.propertyPath );
        }
#endif
        base.GatherProperties(director, driver);
    }
}
