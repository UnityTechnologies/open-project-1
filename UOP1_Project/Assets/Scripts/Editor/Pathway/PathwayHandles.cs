using UnityEngine;
using UnityEditor;


public class PathwayHandles 
{
	private Pathway _pathway;
	private Vector3 _newPosition;

	public PathwayHandles(Pathway pathway) {
		_pathway = pathway;
	}

	public void DispalyHandles()
	{
		EditorGUI.BeginChangeCheck();
		_newPosition = _pathway.transform.position - _newPosition;

		for (int i = 0; i < _pathway.wayPoints.Length; i++)
		{
			_pathway.wayPoints[i] = Handles.PositionHandle(_pathway.wayPoints[i] + _newPosition, Quaternion.identity);
		}

		_newPosition = _pathway.transform.position;
	}

}
