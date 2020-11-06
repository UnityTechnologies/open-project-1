using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueClip : PlayableAsset, ITimelineClipAsset
{
	[SerializeField] private DialogueBehaviour _template = default;
	[HideInInspector] public CutsceneManager cutsceneManager;

	// Having ClipCaps set to None makes sure that the clips can't be blended, extrapolated, looped, etc.
	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		_template.cutsceneManager = cutsceneManager;
		ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, _template);

		return playable;
	}
}
