using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class PathwayHandles
{
	private PathwayConfigSO _pathway;

	public PathwayHandles(PathwayConfigSO pathway)
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
		for (int i = 0; i < _pathway.Waypoints.Count; i++)
		{
			NavMesh.SamplePosition(_pathway.Waypoints[i], out NavMeshHit hit, 99.0f, NavMesh.AllAreas);
			_pathway.Waypoints[i] = hit.position;
		}
	}

}
