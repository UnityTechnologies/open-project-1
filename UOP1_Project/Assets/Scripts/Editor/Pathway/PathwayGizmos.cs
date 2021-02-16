using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;


public class PathwayGizmos
{
	public static void DrawHandlesPath(PathwayConfigSO pathway)
	{
		EditorGUI.BeginChangeCheck();

		List<Vector3> pathwayEditorDisplay = new List<Vector3>();

		// Snap the waypoints on the NavMesh
		for (int i = 0; i < pathway.Waypoints.Count; i++)
		{
			NavMesh.SamplePosition(pathway.Waypoints[i], out NavMeshHit hit, 99.0f, NavMesh.AllAreas);
			pathwayEditorDisplay.Add(hit.position);
		}

		// Only one waypoint use case.
		if (pathway.Waypoints.Count == 1)
		{
			DrawWaypointLabel(pathway, pathway.Waypoints, 0);
			pathway.Waypoints[0] = Handles.PositionHandle(pathway.Waypoints[0], Quaternion.identity);
		}
		// All the other use cases where a path exists.
		for (int index = 0; index < pathway.Waypoints.Count && pathway.Waypoints.Count > 1; index++)
		{
			int nextIndex = (index + 1) % pathway.Waypoints.Count;
			DrawWaypointLabel(pathway, pathway.Waypoints, index);
			List<Vector3> navMeshPath = new List<Vector3>();
			NavMeshPath navPath = new NavMeshPath();
			NavMesh.CalculatePath(pathwayEditorDisplay[index], pathwayEditorDisplay[nextIndex], NavMesh.AllAreas, navPath);
			using (new Handles.DrawingScope(pathway.LineColor))
			{
				for (int j = 0; j < navPath.corners.Length - 1; j++)
				{
					Handles.DrawDottedLine(navPath.corners[j], navPath.corners[j + 1], 2);
				}
			}
			// Display handles pointing into the path forward direction (Blue handle)
			if (navPath.corners.Length > 1)
			{
				pathway.Waypoints[index] = Handles.PositionHandle(pathway.Waypoints[index], Quaternion.LookRotation(navPath.corners[1] - navPath.corners[0]));
			}
			else
			{
				pathway.Waypoints[index] = Handles.PositionHandle(pathway.Waypoints[index], Quaternion.identity);
			}
		}
	}

	private static void DrawWaypointLabel(PathwayConfigSO pathway, List<Vector3> path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = Vector3.up;

		style.normal.textColor = pathway.TextColor;
		style.fontSize = pathway.TextSize;

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}
}
