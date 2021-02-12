using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class PathwayGizmos 
{
	
	[DrawGizmo(GizmoType.Selected)]
	private static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{
		Gizmos.color = pathway.MeshColor;
		
		if (pathway.Path.Count== 0)
		{
			DrawHandlesLines(pathway);
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

		style.normal.textColor = pathway.SelectedIndex==index? pathway.SelectedColor : pathway.TextColor;
		style.fontSize = pathway.TextSize;
		Vector3 textHeight = ( pathway.MeshSize * 1.5f + pathway.TextSize * 0.1f) * Vector3.up;

		if (pathway.DrawMesh != null)
		{
			Vector3 meshDim = Vector3.one * pathway.MeshSize;
			
			Gizmos.DrawMesh(pathway.DrawMesh, path[index], LookAt(path, index), meshDim);
		}

		Handles.Label(path[index] + textHeight, index.ToString(), style);
	}

	private static void DrawHandlesLines(Pathway pathway)
	{
		for (int i = 0; i < pathway.WayPoints.Count; i++)
		{
			if (pathway.SelectedIndex != i || pathway.SelectedIndex == -1)
			{
				DrawElements(pathway, pathway.WayPoints, i);
			}

			if (i != 0)
			{
				using (new Handles.DrawingScope(pathway.LineColor))
				{
					Handles.DrawDottedLine(pathway.WayPoints[i - 1], pathway.WayPoints[i], 2);
				}
			}
		}

		if (pathway.WayPoints.Count > 2)
		{
			using (new Handles.DrawingScope(pathway.LineColor))
			{
				Handles.DrawDottedLine(pathway.WayPoints[0], pathway.WayPoints[pathway.WayPoints.Count - 1], 2);
			}
		}

		if (pathway.SelectedIndex != -1)
		{
			Gizmos.color = pathway.SelectedColor;
			DrawElements(pathway, pathway.WayPoints, pathway.SelectedIndex);
		}
	}

	private static void DrawNavMeshPath(Pathway pathway)
	{
		
		for (int i = 0; i < pathway.Path.Count- 1; i++)
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
			for (int i = 0; i < pathway.WayPoints.Count; i++)
			{
				if (pathway.Hits[i].HasHit)
				{
					Gizmos.color = new Color(0, 255, 0, 0.5f);
					Gizmos.DrawSphere(pathway.Hits[i].Position, pathway.MeshSize*2);
				}
				else
				{
					Gizmos.color = new Color(255, 0, 0, 0.5f);
					Gizmos.DrawSphere(pathway.WayPoints[i], pathway.MeshSize*2);
				}
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
