using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
public class LocalizeSceneNameFromSO : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent localizationEvent = default;

	[SerializeField] LocationSO SO = default;

	private void Start()
	{
		localizationEvent.StringReference = SO.locationName;
	}
}
