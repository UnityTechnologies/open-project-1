using UnityEngine;

[CreateAssetMenu(fileName = "New PathAnchor", menuName = "Runtime Anchors/Path")]
public class PathAnchor : RuntimeAnchorBase
{
	[HideInInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

	private PathSO _Path;
	public PathSO Path
	{
		get { return _Path; }
		set
		{
			_Path = value;
			isSet = _Path != null;
		}
	}

	public void OnDisable()
	{
		_Path = null;
		isSet = false;
	}
}
