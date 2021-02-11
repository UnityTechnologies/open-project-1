using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathway : MonoBehaviour
{
	[HideInInspector]
	public Vector3[] wayPoints;
#if UNITY_EDITOR
	[SerializeField]
	private float _cubeSize = 3;
	[SerializeField]
	private Color _cubeColor = Color.red;
	[SerializeField]
	private Color _lineColor = Color.black;
	[SerializeField]
	private Color _selectedColor = Color.white;
	[SerializeField]
	private Color _textColor = Color.white;
	[SerializeField]
	private Mesh _drawMesh;
	
	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color CubeColor { get => _cubeColor; }
	public Color LineColor { get => _lineColor; }
	public Color SelectedColor { get => _selectedColor; }
	public Color TextColor { get => _textColor; }
	public float Size { get => _cubeSize; }
	public Mesh DrawMesh { get => _drawMesh; }
	public int SelectedIndex { get; set; }
	public NavMeshPath Path { get; set; }

#endif


}
