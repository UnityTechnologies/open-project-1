using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CharacterMoodBehaviour : PlayableBehaviour
{
	[SerializeField] MoodCollectionSO _moodSet = default;

	[HideInInspector] public bool PlayRandomAnimation;
	[HideInInspector] public int AnimationIndex;

	[HideInInspector] public ExpressionManager ExpressionManager;
	//[HideInInspector] public ExpressionManagerMulti ExpressionManager;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (Application.isPlaying)
		{
			if (playable.GetGraph().IsPlaying())
			{
				if (_moodSet != null)
				{
					ExpressionManager.SetMood(_moodSet.Mood);
				//	ExpressionManager.SetPhonemeLibraryByMood(_moodSet.Actor, _moodSet.Mood);

					ExpressionManager.playRandomAnimation = PlayRandomAnimation;
					ExpressionManager.animationIndex = AnimationIndex;

				//	ActorAnimationSettings settings = new ActorAnimationSettings();
				//	settings.PlayRandomAnimation = PlayRandomAnimation;
				//	settings.ForcedAnimationIndex = AnimationIndex;
				//	ExpressionManager.SetActorAnimationSettings(_moodSet.Actor, settings);
				}
				else
				{
					Debug.LogWarning("This clip doesn't contain a Mood!");
				}
			}
			else
			{
				ExpressionManager.PlayDefaultAnimClip();
			//	if (_moodSet != null)
			//		ExpressionManager.ForceDefaultAnimation(_moodSet.Actor);
			}
		}
	}
}
