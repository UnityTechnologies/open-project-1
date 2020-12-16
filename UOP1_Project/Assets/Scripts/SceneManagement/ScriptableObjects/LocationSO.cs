using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
public class LocationSO : ScriptableObject
{
	public string LocationName;
	public Vector3 LocationPosition, LocationRotation;
	[TextArea]
	public string LocationDescription;
}
