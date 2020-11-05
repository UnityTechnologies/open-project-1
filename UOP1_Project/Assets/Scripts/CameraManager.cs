using System;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;
	private bool isMouseControlled;

	[SerializeField, Range(1f, 5f)]
	private float speed;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
	}

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.mouseControlCameraEnableEvent += OnMouseControlCameraEnable;
		inputReader.mouseControlCameraDisableEvent += OnMouseControlCameraDisable;
	}

	private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
		inputReader.mouseControlCameraEnableEvent -= OnMouseControlCameraEnable;
		inputReader.mouseControlCameraDisableEvent -= OnMouseControlCameraDisable;
	}

	private void OnMouseControlCameraDisable()
	{
		isMouseControlled = false;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		freeLookVCam.m_XAxis.m_InputAxisValue = 0;
		freeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	private void OnMouseControlCameraEnable()
	{
		isMouseControlled = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool mouseMovement)
	{
		if (mouseMovement && !isMouseControlled) return;

		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime * speed;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime * speed;
	}
}
