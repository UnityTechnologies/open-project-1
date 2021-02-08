using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CharacterMoodBehaviour : PlayableBehaviour
{
	[HideInInspector] public MoodCollectionSO  MoodSet = default;
	[HideInInspector] public ExpressionManager ExpressionManager;
	[HideInInspector] public bool			   PlayRandomAnimation;
	[HideInInspector] public int               AnimationIndex;
	[HideInInspector] public bool              EnableBlinking = true;
	[HideInInspector] public bool              EnablePhonemes = true;
	[HideInInspector] public bool              EnableAnimations = true;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (Application.isPlaying)
		{
			if (playable.GetGraph().IsPlaying())
			{
				if (MoodSet != null)
				{
					// Make sure this actor assigned to this mood set is registed so that their
					// expressions can be modified via ExpressionManager
					if (!ExpressionManager.IsActorRegistered(MoodSet.Actor))
						ExpressionManager.RegisterActor(MoodSet.Actor);

					// Eye Stuff
					if (EnableBlinking)
					{ 
						ExpressionManager.EnableBlinking(MoodSet.Actor, MoodSet.EyeType);
					}
					else
					{
						ExpressionManager.DisableBlinking(MoodSet.Actor);
					}

					// Mouth Stuff
					if (EnablePhonemes)
					{ 
						ExpressionManager.EnablePhonemes(MoodSet.Actor, MoodSet.MouthType);
						ExpressionManager.SetPhonemeLibraryByMood(MoodSet.Actor, MoodSet.Mood);
					}
					else
					{
						ExpressionManager.DisablePhonemes(MoodSet.Actor);
					}

					// Animation Stuff
					ExpressionManager.EnableAnimations(MoodSet.Actor, EnableAnimations);					
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
