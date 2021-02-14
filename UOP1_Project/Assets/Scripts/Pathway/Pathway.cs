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
	private int _textSize = 10;

	[SerializeField]
	private Color _textColor = Color.white;

	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color LineColor { get => _lineColor; }
	public Color TextColor { get => _textColor; }
	public int TextSize { get => _textSize; }
	

#endif

}
