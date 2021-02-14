using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class PathwayGizmos
{
	[DrawGizmo(GizmoType.Selected)]
	private static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{

		DrawHandlesPath(pathway);
	}

	private static void DrawElements(Pathway pathway, List<Vector3> path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = Vector3.up;

		style.normal.textColor = pathway.TextColor;
		style.fontSize = pathway.TextSize;

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}

	private static void DrawHandlesPath(Pathway pathway)
	{
		if (pathway.Waypoints.Count != 0)
		{
			DrawElements(pathway, pathway.Waypoints, 0);
		}

		for (int i = 0; i < pathway.Waypoints.Count; i++)
		{
			if (i != 0 && pathway.Waypoints.Count > 1)
			{
				DrawElements(pathway, pathway.Waypoints, i);
				using (new Handles.DrawingScope(pathway.LineColor))
				{
					Handles.DrawDottedLine(pathway.Waypoints[i - 1], pathway.Waypoints[i], 2);
				}

			}
		}

		if (pathway.Waypoints.Count > 2)
		{
			using (new Handles.DrawingScope(pathway.LineColor))
			{
				Handles.DrawDottedLine(pathway.Waypoints[0], pathway.Waypoints[pathway.Waypoints.Count - 1], 2);
			}
		}

	}

}
