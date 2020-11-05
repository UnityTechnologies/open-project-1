using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;
	private bool mouseCamEnabled;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
	}

	private void OnEnable()
	{
		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.mouseCamEnabledEvent += OnMouseCamEnabled;
		inputReader.mouseCamDisabledEvent += OnMouseCamDisabled;
	}

	private void OnMouseCamDisabled()
	{
		mouseCamEnabled = false;
		freeLookVCam.m_XAxis.m_InputAxisValue = 0;
		freeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	private void OnMouseCamEnabled() => mouseCamEnabled = true;

	private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool mouseMovement)
	{
		if (mouseMovement && !mouseCamEnabled) return;

		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime;
	}
}
