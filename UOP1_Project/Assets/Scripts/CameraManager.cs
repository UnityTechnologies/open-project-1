using Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraManager : MonoBehaviour
    {
        public InputReader inputReader;
        public CinemachineFreeLook freeLookVCam;

        [Tooltip("General multiplier for camera sensitivity/speed")]
        [Range(1.0f, 20.0f)]
        [SerializeField]
        private float cameraSensitivity = 7.0f;

        public void SetupProtagonistVirtualCamera(Transform target)
        {
            freeLookVCam.Follow = target;
            freeLookVCam.LookAt = target;
        }

        private void OnEnable()
        {
            inputReader.cameraMoveEvent += OnCameraMove;
        }

        private void OnDisable()
        {
            inputReader.cameraMoveEvent -= OnCameraMove;
        }

        private void OnCameraMove(Vector2 cameraMovement)
        {
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime;
        }
    }
}