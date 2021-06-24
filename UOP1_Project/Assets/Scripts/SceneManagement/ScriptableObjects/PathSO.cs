using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Paths are used to determine the connecting entrances and exits between Locations.
/// They don't have a direction, so the same path SO can be reused both ways, by referencing it in the appropriate <c>LocationEntrance</c> and <c>LocationExit</c> scripts in the scene.
/// </summary>

[CreateAssetMenu(fileName = "BetweenLocation1AndLocation2", menuName = "Scene Data/Path")]
public class PathSO : ScriptableObject { }
