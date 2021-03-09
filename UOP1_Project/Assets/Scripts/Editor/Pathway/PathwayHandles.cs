using UnityEngine;
using UnityEditor;


public class PathwayHandles
{
	private Pathway _pathway;
	private Vector3 _tmp;

	public PathwayHandles(Pathway pathway)
	{
		_pathway = pathway;
	}

	public int DisplayHandles()
	{
		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			EditorGUI.BeginChangeCheck();

			_tmp = Handles.PositionHandle(_pathway.Waypoints[i].waypoint, Quaternion.identity);

			if (EditorGUI.EndChangeCheck())
			{
				_pathway.Waypoints[i].waypoint = _tmp;
				return i;
			}
		}
		return -1;
	}
}
