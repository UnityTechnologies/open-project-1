using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/SpawnPoint")]
public class SpawnPointSO : ScriptableObject
{
    public PointSelection Location = new PointSelection();

	public Vector3[] GetVectors()
	{
		Vector3[] returnArray = new Vector3[2];
		returnArray[0] = LocationDatabaseSO.Instance.Locations[Location.LocationIndex].Points[Location.PointIndex].Position;
		returnArray[1] = LocationDatabaseSO.Instance.Locations[Location.LocationIndex].Points[Location.PointIndex].Position;
		return returnArray;
	}
}
