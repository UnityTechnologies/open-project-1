using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public CinemachineFreeLook freeLookVCam;

	[Tooltip("General multiplier for camera sensitivity/speed")]
	[Range(1.0f, 20.0f)]
	[SerializeField] private float cameraSensitivity = 7.0f;

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		//...
	}

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
		//...
	}

	private void OnCameraMove(Vector2 cameraMovement)
	{
		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime * cameraSensitivity;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime * cameraSensitivity;
	}
}
