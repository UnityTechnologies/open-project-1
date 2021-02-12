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
	private Color _lineColor = Color.black;
	[SerializeField]
	private int _textSize = 20;
	[SerializeField]
	private Color _textColor = Color.white;
	[SerializeField]
	private Mesh _drawMesh;
	[SerializeField]
	private float _meshSize = 3;
	[SerializeField]
	private Color _meshColor = Color.red;
	[SerializeField]
	private Color _selectedColor = Color.white;

	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color LineColor { get => _lineColor; }
	public Color TextColor { get => _textColor; }
	public int TextSize { get => _textSize; }
	public float MeshSize { get => _meshSize; }
	public Color MeshColor { get => _meshColor; }
	public Mesh DrawMesh { get => _drawMesh; }
	public Color SelectedColor { get => _selectedColor; }
	public int SelectedIndex { get; set; }
	public struct HitPoint
	{
		public bool HasHit { get; }
		public Vector3 Position { get; }
		public HitPoint(bool hit, Vector3 position)
		{
			HasHit = hit;
			Position = position;
		}
	}
	public bool DisplayPolls{ get ; set; }
	public NavMeshPath Path { get; set; }
	public List<HitPoint> Hits { get; set; }

#endif


}
