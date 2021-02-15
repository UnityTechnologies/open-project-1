using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using System.Collections.Generic;


public class PathwayGizmos
{
	public static void DrawHandlesPath(PathwayConfigSO pathway)
	{
		if (pathway.Waypoints.Count != 0)
		{
			DrawElements(pathway, pathway.Waypoints, 0);
		}
		if (pathway.Waypoints.Count > 1)
		{
			for (int index = 0; index < pathway.Waypoints.Count && pathway.Waypoints.Count > 1; index++)
			{
				int nextIndex = (index + 1) % pathway.Waypoints.Count;
				DrawElements(pathway, pathway.Waypoints, index);
				List<Vector3> navMeshPath = new List<Vector3>();
				NavMeshPath navPath = new NavMeshPath();
				NavMesh.CalculatePath(pathway.Waypoints[index], pathway.Waypoints[nextIndex], NavMesh.AllAreas, navPath);
				using (new Handles.DrawingScope(pathway.LineColor))
				{
					for (int j = 0; j < navPath.corners.Length - 1; j++)
					{
						Handles.DrawDottedLine(navPath.corners[j], navPath.corners[j + 1], 2);
					}
				}
			}
		}
	}

	private static void DrawElements(PathwayConfigSO pathway, List<Vector3> path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = Vector3.up;

		style.normal.textColor = pathway.TextColor;
		style.fontSize = pathway.TextSize;

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}
}
