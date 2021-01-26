using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CharacterMoodBehaviour : PlayableBehaviour
{
	[HideInInspector] public MoodCollectionSO MoodSet = default;
	[HideInInspector] public ExpressionManager ExpressionManager;
	[HideInInspector] public bool PlayRandomAnimation;
	[HideInInspector] public int AnimationIndex;
	[HideInInspector] public bool EnableBlinking = true;
	[HideInInspector] public bool EnablePhonemes = true;
	[HideInInspector] public bool EnableAnimations = true;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (Application.isPlaying)
		{
			if (playable.GetGraph().IsPlaying())
			{
				if (MoodSet != null)
				{
					if (!ExpressionManager.IsActorRegistered(MoodSet.Actor))
						ExpressionManager.RegisterActor(MoodSet.Actor);

					ExpressionManager.EnableBlinking(MoodSet.Actor, EnableBlinking);
					ExpressionManager.EnablePhonemes(MoodSet.Actor, EnablePhonemes);
					ExpressionManager.EnableAnimations(MoodSet.Actor, EnableAnimations);

					ExpressionManager.SetPhonemeLibraryByMood(MoodSet.Actor, MoodSet.Mood);
					
					ActorAnimationSettings settings = new ActorAnimationSettings();
					settings.PlayRandomAnimation = PlayRandomAnimation;
					settings.ForcedAnimationIndex = AnimationIndex;
					ExpressionManager.SetActorAnimationSettings(MoodSet.Actor, settings);
				}
				else
				{
					Debug.LogWarning("This clip doesn't contain a Mood!");
				}
			}
			else
			{
				if (MoodSet != null)
					ExpressionManager.ForceDefaultAnimation(MoodSet.Actor);
			}
		}
	}
}
