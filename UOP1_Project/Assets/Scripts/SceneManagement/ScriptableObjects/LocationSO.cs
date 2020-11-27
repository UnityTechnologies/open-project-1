using UnityEngine;

using UnityEngine.Localization;
/// <summary>
/// This class contains Settings specific to Locations only
/// </summary>

[CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
public class LocationSO : GameSceneSO
{
	[Header("Location specific")]
	public LocalizedString locationName;
	public int enemiesCount; //Example variable, will change later
}
