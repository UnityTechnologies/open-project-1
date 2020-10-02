using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UOP1.Cutscenes
{
    [TrackColor(0.0500623f, 0.3788047f, 0.7075472f)]
    [TrackClipType(typeof(DialogueClip))]
    [TrackBindingType(typeof(DialogueBinder))]
    public class DialogueTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var trackMixer = ScriptPlayable<DialogueMixerBehaviour>.Create (graph, inputCount);
            trackMixer.GetBehaviour().SetClips(GetClips() as TimelineClip[]);
            return trackMixer;
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            DialogueBinder trackBinding = director.GetGenericBinding(this) as DialogueBinder;
            if (trackBinding == null)
                return;

            var serializedObject = new UnityEditor.SerializedObject (trackBinding);
            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                if (iterator.hasVisibleChildren)
                    continue;

                driver.AddFromName<DialogueBinder>( trackBinding.gameObject, iterator.propertyPath );
            }
#endif
            base.GatherProperties(director, driver);
        }
    }
}
