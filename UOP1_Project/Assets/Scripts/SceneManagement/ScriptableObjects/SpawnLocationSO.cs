using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/SpawnLocation")]
public class SpawnLocationSO : ScriptableObject
{
    public LocationSelection Location = new LocationSelection();

	public Vector3[] GetVectors()
	{
		Vector3[] returnArray = new Vector3[2];
		returnArray[0] = SceneDatabaseSO.Instance.Scenes[Location.SceneIndex].Locations[Location.LocationIndex].LocationPosition;
		returnArray[1] = SceneDatabaseSO.Instance.Scenes[Location.SceneIndex].Locations[Location.LocationIndex].LocationPosition;
		return returnArray;
	}
}
