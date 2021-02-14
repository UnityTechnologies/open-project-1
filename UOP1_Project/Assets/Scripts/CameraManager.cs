using System;
using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
	public InputReader inputReader;
	public Camera mainCamera;
	public CinemachineFreeLook freeLookVCam;

	private CinemachineBrain cinemachineBrain;

	private List<CinemachineVirtualCameraBase> vcamsInScene;
	private bool _isRMBPressed;

	[SerializeField, Range(.5f, 3f)]
	private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											
	[SerializeField] private TransformAnchor _cameraTransformAnchor = default;

	[Header("Listening on channels")]
	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	[SerializeField] private TransformEventChannelSO _frameObjectChannel = default;

	[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to swap the active virtual camera")]
	[SerializeField] private VcamEventChannelSO _VcamEventChannel = default;


	private bool _cameraMovementLock = false;

	public void SetupProtagonistVirtualCamera(Transform target)
	{
		foreach (CinemachineVirtualCameraBase vcam in vcamsInScene)
		{
			vcam.LookAt = target;
			vcam.Follow = target;
		}
		freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
	}

	private void Awake()
	{
		cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
		vcamsInScene = new List<CinemachineVirtualCameraBase>();
	}

	private void OnEnable()
	{
		//? should this just be populated in inspector? we may find virtual cameras which are not for the player
		foreach (CinemachineVirtualCameraBase vcam in FindObjectsOfType<CinemachineVirtualCameraBase>())
		{
			vcamsInScene.Add(vcam);
		}

		inputReader.cameraMoveEvent += OnCameraMove;
		inputReader.enableMouseControlCameraEvent += OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent += OnDisableMouseControlCamera;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;

		if(_VcamEventChannel != null)
			_VcamEventChannel.OnEventRaised += OnCameraSwapEvent;

		_cameraTransformAnchor.Transform = mainCamera.transform;
	}


    private void OnDisable()
	{
		inputReader.cameraMoveEvent -= OnCameraMove;
		inputReader.enableMouseControlCameraEvent -= OnEnableMouseControlCamera;
		inputReader.disableMouseControlCameraEvent -= OnDisableMouseControlCamera;

		if (_frameObjectChannel != null)
			_frameObjectChannel.OnEventRaised -= OnFrameObjectEvent;

		if(_VcamEventChannel != null)
			_VcamEventChannel.OnEventRaised -= OnCameraSwapEvent;

		vcamsInScene.Clear();
	}

	private void OnEnableMouseControlCamera()
	{
		_isRMBPressed = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(DisableMouseControlForFrame());
	}

	IEnumerator DisableMouseControlForFrame()
	{
		_cameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		_cameraMovementLock = false;
	}

	private void OnDisableMouseControlCamera()
	{
		_isRMBPressed = false;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		freeLookVCam.m_XAxis.m_InputAxisValue = 0;
		freeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (_cameraMovementLock)
			return;

		if (isDeviceMouse && !_isRMBPressed)
			return;

		freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime * _speedMultiplier;
		freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime * _speedMultiplier;
	}

	private void OnFrameObjectEvent(Transform value)
	{
		SetupProtagonistVirtualCamera(value);
	}

	private void OnCameraSwapEvent(CinemachineVirtualCamera vcam)
    {
        if(vcam != null){
			if(!cinemachineBrain.IsLive(vcam)){
				cinemachineBrain.ActiveVirtualCamera.Priority = 0;
				vcam.Priority = 100;
			}
		}
		// null indicates default camera. in our case the free look camera
		else if(vcam == null){
			if(!cinemachineBrain.IsLive(freeLookVCam)){
				cinemachineBrain.ActiveVirtualCamera.Priority = 0;
				freeLookVCam.Priority = 100;
			}
		}
		
    }
}
