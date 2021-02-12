using UnityEngine;

[CreateAssetMenu(fileName = "New LocationAnchor", menuName = "Runtime Anchors/Location")]
public class LocationAnchor : RuntimeAnchorBase
{
	[HideInInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

	private LocationSO _Location;
	public LocationSO Location
	{
		get { return _Location; }
		set
		{
			_Location = value;
			isSet = _Location != null;
		}
	}

	public void OnDisable()
	{
		_Location = null;
		isSet = false;
	}
}