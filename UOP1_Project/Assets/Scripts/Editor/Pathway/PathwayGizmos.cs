using UnityEngine;
using UnityEditor;


public class PathwayGizmos
{
	public static void DrawGizmosSelected(PathwayConfigSO pathway)
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


	private static void DrawLabel(PathwayConfigSO pathway, Vector3 path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = Vector3.up;

		style.normal.textColor = pathway.TextColor;
		style.fontSize = pathway.TextSize;

		Handles.Label(path + textHeight, index.ToString(), style);
	}

	private static void DrawHandlesPath(PathwayConfigSO pathway)
	{
		Handles.color = pathway.LineColor;

		if (pathway.Waypoints.Count != 0)
		{
			DrawLabel(pathway, pathway.Waypoints[0].waypoint, 0);

			if (pathway.Waypoints.Count > 1)
			{
				for (int i = 1; i < pathway.Waypoints.Count; i++)
				{
					DrawLabel(pathway, pathway.Waypoints[i].waypoint, i);
					Handles.DrawDottedLine(pathway.Waypoints[i - 1].waypoint, pathway.Waypoints[i].waypoint, 2);
				}

				if (pathway.Waypoints.Count > 2)
				{
					Handles.DrawDottedLine(pathway.Waypoints[0].waypoint, pathway.Waypoints[pathway.Waypoints.Count - 1].waypoint, 2);
				}
			}
		}
	}

	private static void DrawNavMeshPath(PathwayConfigSO pathway)
	{
		Handles.color = pathway.LineColor;

		for (int i = 0; i < pathway.Path.Count - 1; i++)
		{
			Handles.DrawLine(pathway.Path[i], pathway.Path[i + 1]);

			if (i < pathway.Waypoints.Count)
			{
				DrawLabel(pathway, pathway.Waypoints[i].waypoint, i);
			}
		}
	}

	private static void DrawHitPoints(PathwayConfigSO pathway)
	{
		if (pathway.DisplayProbes)
		{
			float sphereRadius = pathway.ProbeRadius;

			for (int i = 0; i < pathway.Hits.Count; i++)
			{
				if (pathway.Hits[i])
				{
					Handles.color = new Color(0, 255, 0, 0.1f);
					Handles.SphereHandleCap(0, pathway.Waypoints[i].waypoint, Quaternion.identity, sphereRadius, EventType.Repaint);
				}
				else
				{
					Handles.color = new Color(255, 0, 0, 0.1f);
					Handles.SphereHandleCap(0, pathway.Waypoints[i].waypoint, Quaternion.identity, sphereRadius, EventType.Repaint);
				}
			}
		}
	}
}
