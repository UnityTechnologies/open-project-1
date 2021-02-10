using UnityEngine;
using UnityEditor;


public class PathwayGizmos
{

	[DrawGizmo(GizmoType.Selected)]
	static void DrawGizmosSelected(Pathway pathway, GizmoType gizmoType)
	{
		DrawGizmos(pathway);
	}

	private static void DrawGizmos(Pathway pathway) {

		Gizmos.color = pathway.CubeColor;
		
		for (int i = 0; i < pathway.wayPoints.Count; i++)
		{
			if (pathway.SelectedIndex != i || pathway.SelectedIndex == -1)
			{

				//Draw cubes, labels and meshes
				DrawElements(pathway, i);
			}
			if (i != 0)
			{
				//Draw lines
				using (new Handles.DrawingScope(pathway.LineColor))
				{
					Handles.DrawDottedLine(pathway.wayPoints[i - 1], pathway.wayPoints[i], 2);
				}
			}

		}

		if (pathway.wayPoints.Count > 2)
		{
			//Draw final line between the first and last point
			using (new Handles.DrawingScope(pathway.LineColor))
			{
				Handles.DrawDottedLine(pathway.wayPoints[0], pathway.wayPoints[pathway.wayPoints.Count - 1], 2);
			}
		}

		if (pathway.SelectedIndex != -1)
		{
			//Draw cubes, labels and meshes for the selected index
			Gizmos.color = pathway.SelectedColor;
			DrawElements(pathway, pathway.SelectedIndex);
		}

	}

	private static Quaternion LookAt(Pathway pathway, int index)
	{
		if (index != pathway.wayPoints.Count - 1)
		{
			return LookAtDirection(pathway.wayPoints[index + 1], pathway.wayPoints[index]);
		}
		else
		{
			return LookAtDirection(pathway.wayPoints[0], pathway.wayPoints[index]);
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

	private static void DrawElements(Pathway pathway, int index){
		
		Vector3 vOffset = Vector3.up * pathway.Size / 2;
		Vector3 cubeDim = Vector3.one * pathway.Size;
		Vector3 meshDim = cubeDim / 1.3f;
		GUIStyle style = new GUIStyle();

		style.normal.textColor = pathway.TextColor;
		Handles.Label(pathway.wayPoints[index] + (pathway.Size + 1) * Vector3.up, Pathway.FIELD_LABEL + index, style);
		Gizmos.DrawWireCube(pathway.wayPoints[index] + vOffset, cubeDim);
		Gizmos.DrawMesh(pathway.DrawMesh, pathway.wayPoints[index], LookAt(pathway, index), meshDim);
	}

}
