using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationDatabase", menuName = "Scene Data/LocationDatabase")]
public class LocationDatabaseSO : ScriptableObject
{
	private static LocationDatabaseSO _instance;
	public static LocationDatabaseSO Instance { get { return _instance; } }
	public LocationSO[] Locations;

	public LocationDatabaseSO()
	{
		_instance = this;
	}
	[RuntimeInitializeOnLoadMethod]
	public static void Initialize ()
	{
		_instance = Resources.LoadAll<LocationDatabaseSO>("Databases")[0];
	}
}
