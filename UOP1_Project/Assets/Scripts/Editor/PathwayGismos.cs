using UnityEngine;
using UnityEditor;


public class PathwayGizmos
{
	private const string FIELD_LABEL="Waypoints ";

	[DrawGizmo(GizmoType.Selected)]
	static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{
		DrawGizmos(pathway);
	}
	
	private static void DrawGizmos(Pathway pathway) {

		Quaternion lookAt;
		Vector3 vOffset = Vector3.up * pathway.Size / 2;
		Vector3 cubeDim = Vector3.one * pathway.Size;
		Vector3 meshDim = cubeDim / 1.3f;
		Gizmos.color = pathway.CubeColor;
		GUIStyle style = new GUIStyle();
		style.normal.textColor = pathway.TextColor;

		for (int i = 0; i < pathway.wayPoints.Count; i++)
		{
			if (pathway.SelectedIndex != i || pathway.SelectedIndex == -1)
			{
				if (i != pathway.wayPoints.Count - 1)
				{
					lookAt = Quaternion.LookRotation(pathway.wayPoints[i + 1] - pathway.wayPoints[i]);
				}
				else
				{
					lookAt = Quaternion.LookRotation(pathway.wayPoints[0] - pathway.wayPoints[i]);
				}
				//Draw cubes, labels and mesh
				Handles.Label(pathway.wayPoints[i] + (pathway.Size + 2) * Vector3.up, FIELD_LABEL + i, style);
				Gizmos.DrawWireCube(pathway.wayPoints[i] + vOffset, cubeDim);
				Gizmos.DrawMesh(pathway.DrawMesh, pathway.wayPoints[i], lookAt, meshDim);
			}
			if (i != 0) 
			{
				//Draw lines
				Handles.color = pathway.LineColor;
				Handles.DrawDottedLine(pathway.wayPoints[i - 1], pathway.wayPoints[i], 2);
				Handles.color = pathway.CubeColor;
			}
		}

		if (pathway.wayPoints.Count > 2)
		{
			//draw final line between the first and last point
			Handles.color = pathway.LineColor;
			Handles.DrawDottedLine(pathway.wayPoints[0], pathway.wayPoints[pathway.wayPoints.Count - 1], 2);
			Handles.color = pathway.CubeColor;
		}

		if (pathway.SelectedIndex != -1)
		{
			if (pathway.SelectedIndex != pathway.wayPoints.Count - 1)
			{
				lookAt = Quaternion.LookRotation(pathway.wayPoints[pathway.SelectedIndex + 1] - pathway.wayPoints[pathway.SelectedIndex]);
			}
			else
			{
				lookAt = Quaternion.LookRotation(pathway.wayPoints[0] - pathway.wayPoints[pathway.SelectedIndex]);
			}
			//Draw cubes, labels and mesh for the selected index
			Gizmos.color = pathway.SelectedColor;
			Gizmos.DrawWireCube(pathway.wayPoints[pathway.SelectedIndex] + vOffset, cubeDim);
			Gizmos.DrawMesh(pathway.DrawMesh, pathway.wayPoints[pathway.SelectedIndex], lookAt, meshDim);
			Gizmos.color = pathway.CubeColor;
		}

	}
	
}
