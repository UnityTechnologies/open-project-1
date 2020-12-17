using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Load Scene Channel")]
public class LoadSceneChannelSO : ScriptableObject
{
	public UnityAction<PointSelection, bool> OnLoadingRequested;

	public void RaiseEvent(PointSelection locationToLoad, bool showLoadingScreen)
	{
		if (OnLoadingRequested != null)
		{
			OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen);
		}
		else
		{
			Debug.LogWarning("A Game Scene loading was requested, but nobody picked it up." +
				"Check why there is no SceneLoader already present, " +
				"and make sure it's listening on this Load Event channel.");
		}
	}
}
