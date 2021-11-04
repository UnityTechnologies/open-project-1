using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizeSceneNameFromSO : MonoBehaviour
{
	[SerializeField] LocalizeStringEvent _localizationEvent = default;
	[SerializeField] LocationSO _location = default;

	private void Start()
	{
		_localizationEvent.StringReference = _location.locationName;
	}
}
