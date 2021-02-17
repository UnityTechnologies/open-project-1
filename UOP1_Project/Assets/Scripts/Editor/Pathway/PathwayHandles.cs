using UnityEngine;
using UnityEditor;


public class PathwayHandles
{
	private Pathway _pathway;

	public PathwayHandles(Pathway pathway)
	{
		_pathway = pathway;
	}

	public void DispalyHandles()
	{
		EditorGUI.BeginChangeCheck();

		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			_pathway.Waypoints[i] = Handles.PositionHandle(_pathway.Waypoints[i], Quaternion.identity);
		}
	}

}
