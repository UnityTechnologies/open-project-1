using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathwayConfig", menuName = "EntityConfig/Pathway Config")]
public class PathwayConfigSO : NPCMovementConfigSO
{
	[HideInInspector] public List<Vector3> Waypoints;

#if UNITY_EDITOR

	[SerializeField]
	private Color _pathwayEditorColor = Color.cyan;
	private int _textSize = 10;
	private Color _textColor = Color.white;

	public const string FIELD_LABEL = "Point ";
	public const string TITLE_LABEL = "Waypoints";

	public Color LineColor { get => _pathwayEditorColor; }
	public Color TextColor { get => _textColor; }
	public int TextSize { get => _textSize; }

#endif
}
