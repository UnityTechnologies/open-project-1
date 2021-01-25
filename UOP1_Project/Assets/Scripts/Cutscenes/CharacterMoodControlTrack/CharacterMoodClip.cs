using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CharacterMoodClip : PlayableAsset, ITimelineClipAsset
{
	[SerializeField] private CharacterMoodBehaviour _behaviour = default;
	[SerializeField] private bool _playRandomAnimation = true;
	[SerializeField] private int _animationIndex = 0;

	[HideInInspector] public ExpressionManager ExpressionManager;
	//[HideInInspector] public ExpressionManagerMulti ExpressionManager;

	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		_behaviour.PlayRandomAnimation = _playRandomAnimation;
		_behaviour.AnimationIndex = _animationIndex;

		//_behaviour.ExpressionManager = ExpressionManager;
		_behaviour.ExpressionManager = ExpressionManager;
		ScriptPlayable<CharacterMoodBehaviour> playable = ScriptPlayable<CharacterMoodBehaviour>.Create(graph, _behaviour);

		return playable;
	}
}
