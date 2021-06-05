using UnityEngine;

/// <summary>
/// This one of a kind SO stores, during gameplay, the path that was
/// </summary>
//[CreateAssetMenu(fileName = "PathStorage", menuName = "Gameplay/Path Storage")]
public class PathStorageSO : DescriptionBaseSO
{
	[HideInInspector] public PathSO lastPathTaken;
}
