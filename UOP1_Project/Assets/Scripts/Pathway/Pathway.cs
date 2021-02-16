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

	[SerializeField, Range(0, 100)]
	[Tooltip("")]
	private float _probeRadius = 3;

	[HideInInspector]
	public bool DisplayPolls;

	[HideInInspector]
	public bool TogglePathDisplay;

	private List<Vector3> _path;
	private List<bool> _hits;

	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color LineColor { get => _lineColor; }
	public Color TextColor { get => _textColor; }
	public int TextSize { get => _textSize; }
	public float ProbeRadius { get => _probeRadius; }
	public List<Vector3> Path { get => _path; set => _path = value; }
	public List<bool> Hits { get => _hits; set => _hits = value; }

	public bool RealTimeEnabled;

#endif

}
