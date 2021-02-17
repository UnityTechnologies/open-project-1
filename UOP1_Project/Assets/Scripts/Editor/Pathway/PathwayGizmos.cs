using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class PathwayGizmos 
{
	[DrawGizmo(GizmoType.Selected)]
	private static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{	
		if (!pathway.ToggledNavMeshDisplay)
		{
			DrawHandlesPath(pathway);
		}
		else
		{
			DrawNavMeshPath(pathway);
		}

		DrawHitPoints(pathway);
	}


	private static void DrawLabel(Pathway pathway, List<Vector3> path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = Vector3.up;

		style.normal.textColor = pathway.TextColor;
		style.fontSize = pathway.TextSize;

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}

	private static void DrawHandlesPath(Pathway pathway)
	{
		Handles.color = pathway.LineColor;

		if (pathway.Waypoints.Count != 0)
		{
			DrawLabel(pathway, pathway.Waypoints, 0);
		}

		if (pathway.Waypoints.Count > 1)
		{
			for (int i = 1; i < pathway.Waypoints.Count; i++)
			{
				DrawLabel(pathway, pathway.Waypoints, i);
				Handles.DrawDottedLine(pathway.Waypoints[i - 1], pathway.Waypoints[i], 2);
			}
			
			if (pathway.Waypoints.Count > 2)
			{
				Handles.DrawDottedLine(pathway.Waypoints[0], pathway.Waypoints[pathway.Waypoints.Count - 1], 2);
			}
		}
	}

	private static void DrawNavMeshPath(Pathway pathway)
	{
		Handles.color = pathway.LineColor;
		
		for (int i = 0 ; i < pathway.Path.Count - 1; i++)
		{
			Handles.DrawLine(pathway.Path[i], pathway.Path[i + 1]);
			if (i<pathway.Waypoints.Count) {
				DrawLabel(pathway, pathway.Waypoints, i);
			}
		}
	}

	private static void DrawHitPoints(Pathway pathway)
	{
		if (pathway.DisplayProbes)
		{
			float sphereRadius = pathway.ProbeRadius;

			for (int i = 0; i < pathway.Hits.Count; i++)
			{
				if (pathway.Hits[i])
				{
					Gizmos.color = new Color(0, 255, 0, 0.5f);
					Gizmos.DrawSphere(pathway.Waypoints[i], sphereRadius);
				}
				else
				{
					Gizmos.color = new Color(255, 0, 0, 0.5f);
					Gizmos.DrawSphere(pathway.Waypoints[i], sphereRadius);
				}
			}
		}
	}
}
