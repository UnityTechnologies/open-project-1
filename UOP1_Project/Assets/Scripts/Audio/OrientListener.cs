using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orient the listener to point in the same direction as the camera.
public class OrientListener : MonoBehaviour
{
	// Reference to the camera transform determine listener orientation
	[SerializeField]
	private TransformAnchor _cameraTransform;

	void LateUpdate()
	{
		transform.forward = _cameraTransform.Transform.forward;
	}
}
