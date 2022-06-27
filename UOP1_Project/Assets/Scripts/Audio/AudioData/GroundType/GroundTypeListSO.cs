using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// List of All ground types available in game.
/// </summary>
[CreateAssetMenu(fileName = "GroundTypeList", menuName = "Ground Type/Ground Type List")]
public class GroundTypeListSO : ScriptableObject
{
	public GroundTypeSO[] groundTypes = default;
}
