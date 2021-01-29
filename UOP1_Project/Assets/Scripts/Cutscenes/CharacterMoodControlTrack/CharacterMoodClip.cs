using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CharacterMoodClip : PlayableAsset, ITimelineClipAsset
{
	[HideInInspector] private CharacterMoodBehaviour _behaviour           = default;

	[SerializeField] private MoodCollectionSO        _moodSet             = default;
	[SerializeField] private bool					 _playRandomAnimation = true;
	[SerializeField] private int					 _animationIndex      = 0;
	[SerializeField] private bool				     _enableBlinking      = true;
	[SerializeField] private bool					 _enablePhonemes      = true;
	[SerializeField] private bool					 _enableAnimations    = true;

	[HideInInspector] public ExpressionManager ExpressionManager;

	public ClipCaps clipCaps
	{
		get { return ClipCaps.None; }
	}

	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		// Transfer clip settings over to the behaviour
		_behaviour.MoodSet = _moodSet;
		_behaviour.ExpressionManager = ExpressionManager;
		_behaviour.PlayRandomAnimation = _playRandomAnimation;
		_behaviour.AnimationIndex = _animationIndex;
		_behaviour.EnableBlinking= _enableBlinking;
		_behaviour.EnablePhonemes = _enablePhonemes;
		_behaviour.EnableAnimations = _enableAnimations;

		ScriptPlayable<CharacterMoodBehaviour> playable = ScriptPlayable<CharacterMoodBehaviour>.Create(graph, _behaviour);

		return playable;
	}
}
