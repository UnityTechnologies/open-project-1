using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Fade Channel")]
public class FadeChannelSO : ScriptableObject
{
	public UnityAction<bool, float, Color> OnEventRaised;
	/// <summary>
	/// Generic fade function. Communicates with <seealso cref="FadeManager.cs"/>.
	/// </summary>
	/// <param name="fadeIn">If true, the rectangle fades in. If false, the rectangle fades out.</param>
	/// <param name="duration">How long it takes to the image to fade in/out.</param>
	/// <param name="color">Target color for the image to reach. Disregarded when fading out.</param>
	public void Fade(bool fadeIn, float duration, Color color)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(fadeIn, duration, color);
	}

	/// <summary>
	/// Generic fade function. Communicates with <seealso cref="FadeManager.cs"/>.
	/// </summary>
	/// <param name="fadeIn">If true, the rectangle fades in. If false, the rectangle fades out.</param>
	/// <param name="duration">How long it takes to the image to fade in/out.</param>
	public void Fade(bool fadeIn, float duration)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(fadeIn, duration, new Color(0, 0, 0, 1));
	}

	/// <summary>
	/// Fade helper function to simplify usage. Fades in the rectangle.
	/// </summary>
	/// <param name="duration">How long it takes to the image to fade in.</param>
	/// <param name="color">Target color for the image to reach.</param>
	public void FadeIn(float duration, Color color)
	{
		Fade(true, duration, color);
	}
	/// <summary>
	/// Fade helper function to simplify usage. Fades in the rectangle.
	/// </summary>
	/// <param name="duration">How long it takes to the image to fade in.</param>
	public void FadeIn(float duration)
	{
		Fade(true, duration, new Color(0, 0, 0, 1));
	}
	/// <summary>
	/// Fade helper function to simplify usage. Fades out the rectangle.
	/// </summary>
	/// <param name="duration">How long it takes to the image to fade out.</param>
	public void FadeOut(float duration)
	{
		Fade(false, duration, new Color(0, 0, 0, 1));
	}
}
