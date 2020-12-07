using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Runtime Anchors/Transform")]
public class TransformAnchor : RuntimeAnchorBase
{
	[HideInInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

	private Transform _transform;
	public Transform Transform
	{
		get { return _transform; }
		set
		{
			_transform = value;
			isSet = _transform != null;
		}
	}

	public void OnDisable()
	{
		_transform = null;
		isSet = false;
	}
}
