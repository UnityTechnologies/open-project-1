using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
	[Header("Fade Event")]
	[SerializeField] private FadeChannelSO _fadeChannelSO;

	[Header("Fade Image")]
	[SerializeField] private Image _imageComponent;

	private bool IsCurrentlyFading = false;

	/// <summary>
	/// Enumerators that fades in the canvas's imageComponent to turn the screen to a flat color over time. Fadeins called simeutaneously will only fade in the earliest call and discard any others.
	/// </summary>
	/// <param name="duration">How long it takes to the image to fade in.</param>
	/// <param name="color">Target color for the image to reach.</param>
	/// <returns></returns>
	private IEnumerator FadeInEnum(float duration, Color color)
	{
		float totalTime = 0f; // Total amount of time this coroutine has taken. Determines when the fadein will end and what color the imageComponent should be at every frame.
		while (totalTime <= duration)
		{
			totalTime += Time.deltaTime;
			_imageComponent.color = Color.Lerp(new Color(0, 0, 0, 0), color, totalTime/duration); // Sets the image's color to a mixture between total transparency and the target color, and interpolates based on the amount of time to completion.
			yield return null;
		}
		_imageComponent.color = color; // Here to guarentee the image is exactly the requested color at the end of the loop.
		IsCurrentlyFading = false;
		yield return null;
	}

	/// <summary>
	/// Enumerators that fades out the canvas's imageComponent to turn the screen to normal gameplay color over time. Fadeouts called simeutaneously will only fade out the earliest call and discard any others.
	/// </summary>
	/// <param name="duration">How long it takes to the image to fade out.</param>
	/// <returns></returns>
	private IEnumerator FadeOutEnum(float duration, Color color = default)
	{
		if (color == default)
			color = _imageComponent.color; // Stores the old color of the image component, as we can't assume the image will always be black, if no color is specified.
		float totalTime = 0f; // Total amount of time this coroutine has taken. Determines when the fadeout will end and what color the imageComponent should be at every frame.
		while (totalTime <= duration)
		{
			totalTime += Time.deltaTime;
			_imageComponent.color = Color.Lerp(color, new Color(0, 0, 0, 0), totalTime / duration); // Sets the image's color to a mixture between the old color and  total transparency, and interpolates based on the amount of time to completion.
			yield return null;
		}
		_imageComponent.color = new Color(0, 0, 0, 0); // Here to guarentee the image is fully transparent at the end of the loop.
		IsCurrentlyFading = false;
		yield return null;
	}

	private void OnEnable()
	{
		if (_fadeChannelSO != null)
		{
			_fadeChannelSO.OnEventRaised += fadeGeneral;
		}
	}

	private void OnDisable()
	{
		if (_fadeChannelSO != null)
		{
			_fadeChannelSO.OnEventRaised += fadeGeneral;
		}
	}

	/// <summary>
	/// Controls the fade-in and fade-out.
	/// </summary>
	/// <param name="fadeIn">If true, the rectangle fades in. If false, the rectangle fades out.</param>
	/// <param name="duration">How long it takes to the image to fade in/out.</param>
	/// <param name="color">Target color for the image to reach. Disregarded when fading out.</param>
	private void fadeGeneral(bool fadeIn, float duration, Color color)
	{
		if (!IsCurrentlyFading) // Makes sure multiple fade-ins or outs don't happen at the same time. Note this will mean fadeouts called at the same time will be discarded.
		{
			IsCurrentlyFading = true;
			if (fadeIn)
			{
				StartCoroutine(FadeInEnum(duration, color));
			}
			else
			{
				StartCoroutine(FadeOutEnum(duration, color)); // Fadeout doesn't need color, so the color parameter is disregarded.
			}
		}
	}
}
