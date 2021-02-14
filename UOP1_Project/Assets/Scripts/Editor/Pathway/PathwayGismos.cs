using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class PathwayGizmos 
{
	
	[DrawGizmo(GizmoType.Selected)]
	private static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{
		Gizmos.color = pathway.MeshColor;
		
		if (!pathway.TogglePathDisplay)
		{
			DrawHandlesPath(pathway);
		}
		else
		{
			DrawNavMeshPath(pathway);
		}

		DrawHitPoints(pathway);
	}

	private static void DrawElements(Pathway pathway, List<Vector3> path, int index)
	{
		GUIStyle style = new GUIStyle();
		Vector3 textHeight = (pathway.MeshSize * 1.5f + pathway.TextSize * 0.1f) * Vector3.up;

		style.normal.textColor = pathway.SelectedIndex==index && pathway.Path.Count == 0 ? pathway.SelectedColor : pathway.TextColor;
		style.fontSize = pathway.TextSize;
		
		if (pathway.DrawMesh != null)
		{
			Vector3 meshDim = Vector3.one * pathway.MeshSize;
			Gizmos.DrawMesh(pathway.DrawMesh, path[index], LookAt(path, index), meshDim);
		}

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}

	private static void DrawHandlesPath(Pathway pathway)
	{
		for (int i = 0; i < pathway.Waypoints.Count; i++)
		{
			if (pathway.SelectedIndex != i || pathway.SelectedIndex == -1)
			{
				DrawElements(pathway, pathway.Waypoints, i);
			}

			if (i != 0 && pathway.Waypoints.Count > 1)
			{
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

		if (pathway.SelectedIndex != -1)
		{
			Gizmos.color = pathway.SelectedColor;
			DrawElements(pathway, pathway.Waypoints, pathway.SelectedIndex);
		}
	}

	private static void DrawNavMeshPath(Pathway pathway)
	{
		for (int i = 0; i < pathway.Path.Count - 1; i++)
		{
			DrawElements(pathway, pathway.Path, i);
			using (new Handles.DrawingScope(pathway.LineColor))
			{
				Handles.DrawLine(pathway.Path[i], pathway.Path[i + 1]);
			}
		}

		DrawElements(pathway, pathway.Path, pathway.Path.Count - 1);
		using (new Handles.DrawingScope(pathway.LineColor))
		{
			Handles.DrawLine(pathway.Path[0], pathway.Path[pathway.Path.Count - 1]);
		}
	}

	private static void DrawHitPoints(Pathway pathway)
	{
		if (pathway.DisplayPolls)
		{
			if (pathway.Hits.Count == pathway.Waypoints.Count)
			{
				float sphereRadius = pathway.MeshSize * 2;

				for (int i = 0; i < pathway.Hits.Count; i++)
				{
					if (pathway.Hits[i].HasHit)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawLine(pathway.Hits[i].Position, pathway.Waypoints[i]);
						if (Mathf.Abs(Vector3.Distance(pathway.Hits[i].Position,pathway.Waypoints[i])) <= 0.2f)
						{
							Gizmos.color = new Color(0, 0, 255, 1f);
							Gizmos.DrawSphere(pathway.Waypoints[i], 0.2f);
						}
						
						Debug.Log(pathway.Hits[i].Position);
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
			else
			{
				Debug.LogError("Polls need to be updated");
			}
		}
	}

	private static Quaternion LookAt(List<Vector3> path, int index)
	{
		if (index != path.Count - 1)
		{
			return LookAtDirection(path[index + 1], path[index]);
		}
		else
		{
			return LookAtDirection(path[0], path[index]);
		}
	}

	private static Quaternion LookAtDirection(Vector3 a, Vector3 b)
	{
		if (a != b)
		{
			return Quaternion.LookRotation(a - b);
		}
		return Quaternion.identity;
	}

}
