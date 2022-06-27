using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ground Type Properties. This will use for detect ground type for footsteps sound.
/// </summary>
[CreateAssetMenu(fileName = "newGroundType", menuName = "Ground Type/Ground Type")]

[SerializeField]
public class GroundTypeSO : ScriptableObject
{
	public string Title;

	[Space]
	public bool hasGroundTag;

	[Space]
	[Header("If it has Ground tag")]
	[Tooltip("This helps to detect the type of ground.")]
	public Vector3 vertexColorRGB;

	[Space]
	[Header("If it doesn't have Ground tag")]
	[Tooltip("This helps to detect the type of ground.")]
	public string gameObjectTag;
	
}
