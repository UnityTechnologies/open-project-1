using UnityEngine;

/// <summary>
/// This class contains Settings specific to Locations only
/// </summary>

[CreateAssetMenu(fileName = "NewLocationData", menuName = "Scene Data/LocationData")]
public class LocationSO : BaseSceneSO
{
	[Header("Points")]
	public PointSO[] Points;
}
