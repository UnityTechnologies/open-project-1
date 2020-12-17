using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class to help designers tweak POI data within the scenes.
/// They still need to add the points of interest to the SceneDataSO it belongs to.
/// </summary>
[ExecuteInEditMode]
public class PointEditor : MonoBehaviour
{
	public PointSO PointToEdit;
	private PointSO _currentPoint;

	public string PointName;
	[TextArea]
	public string PointDescription;

#if UNITY_EDITOR
	void Update()
    {

		if (!Application.isPlaying)
		{
			if (PointToEdit != null)
			{
				if (PointToEdit != _currentPoint)
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
		PointToEdit.PointName = this.PointName;
		PointToEdit.Position = this.transform.position;
		PointToEdit.Rotation = this.transform.rotation.eulerAngles;
		PointToEdit.PointDescription = this.PointDescription;
	}
	private void FetchData()
	{
		this.PointName = PointToEdit.PointName;
		this.transform.position = PointToEdit.Position;
		this.transform.rotation = Quaternion.Euler(PointToEdit.Rotation);
		this.PointDescription = PointToEdit.PointDescription;
		_currentPoint = PointToEdit;
	}
}
