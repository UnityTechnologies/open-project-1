using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class to help designers tweak location data within the scenes.
/// They still need to add the locations to the SceneDataSO it belongs to.
/// </summary>
[ExecuteInEditMode]
public class LocationEditor : MonoBehaviour
{
	public LocationSO LocationToEdit;
	private LocationSO _currentLocation;

	public string LocationName;
	[TextArea]
	public string LocationDescription;

#if UNITY_EDITOR
	void Update()
    {

		if (!Application.isPlaying)
		{
			if (LocationToEdit != null)
			{
				if (LocationToEdit != _currentLocation)
				{
					FetchData();
				}
				else
				{
					FillData();
				}
			}
		}
	}
#endif
	private void FillData()
	{
		LocationToEdit.LocationName = this.LocationName;
		LocationToEdit.LocationPosition = this.transform.position;
		LocationToEdit.LocationRotation = this.transform.rotation.eulerAngles;
		LocationToEdit.LocationDescription = this.LocationDescription;
	}
	private void FetchData()
	{
		this.LocationName = LocationToEdit.LocationName;
		this.transform.position = LocationToEdit.LocationPosition;
		this.transform.rotation = Quaternion.Euler(LocationToEdit.LocationRotation);
		this.LocationDescription = LocationToEdit.LocationDescription;
		_currentLocation = LocationToEdit;
	}
}
