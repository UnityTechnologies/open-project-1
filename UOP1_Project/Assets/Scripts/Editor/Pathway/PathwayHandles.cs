using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PathwayHandles
{
	private Pathway _pathway;
	private Vector3 tmp;

	public PathwayHandles(Pathway pathway)
	{
		_pathway = pathway;
	}

	public void DisplayHandles()
	{
		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			EditorGUI.BeginChangeCheck();

			tmp=Handles.PositionHandle(_pathway.Waypoints[i], Quaternion.identity);

			if (EditorGUI.EndChangeCheck())
			{
				_pathway.Waypoints[i] = tmp;
			}
		}
	}

}
