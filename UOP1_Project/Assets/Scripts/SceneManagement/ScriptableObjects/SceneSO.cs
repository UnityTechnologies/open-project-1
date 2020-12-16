using UnityEngine;

/// <summary>
/// This class contains Settings specific to Locations only
/// </summary>

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data/SceneData")]
public class SceneSO : BaseSceneSO
{
	[Header("Locations")]
	public LocationSO[] Locations;
}
