using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// This class contains Settings specific to Locations only
/// </summary>

[CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
public class LocationSO : GameSceneSO
{
	public LocalizedString locationName;
}
