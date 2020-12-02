using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		freeLookVCam.Follow = target;
		freeLookVCam.LookAt = target;
	}

}
