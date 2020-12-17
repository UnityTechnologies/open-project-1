using UnityEngine;

[CreateAssetMenu(fileName = "NewPoint", menuName = "Scene Data/Point")]
public class PointSO : ScriptableObject
{
	public string PointName;
	public Vector3 Position, Rotation;
	[TextArea]
	public string PointDescription;
}
