using System.Collections.Generic;
using UnityEngine;


public class Pathway : MonoBehaviour
{
	[HideInInspector]
	public List<Vector3> Waypoints;

#if UNITY_EDITOR

	[SerializeField]
	private Color _lineColor = Color.black;

	[SerializeField, Range(0, 100)]
	private int _textSize = 20;

	[SerializeField]
	private Color _textColor = Color.white;

	[SerializeField]
	[Tooltip("Add a mesh on each point of the drawn paths")]
	private Mesh _drawMesh;

	[SerializeField, Range(0, 100)]
	[Tooltip("The poll uses twice the height of the mesh specified for the search radius")]
	private float _meshSize = 3;

	[SerializeField]
	private Color _meshColor = Color.red;

	[SerializeField]
	[Tooltip("color of the selected scene view element by clicking on the list")]
	private Color _selectedColor = Color.white;

	[HideInInspector]
	public bool DisplayPolls;

	[HideInInspector]
	public bool TogglePathDisplay;

	[HideInInspector]
	public List<Vector3> Path;

	[HideInInspector]
	public List<HitPoint> Hits;

	[System.Serializable]
	public struct HitPoint
	{
		public bool hasHit;
		public Vector3 position;
		
	}

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

	
	

#endif

}
