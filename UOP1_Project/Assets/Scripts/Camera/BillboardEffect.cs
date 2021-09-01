using UnityEngine;

namespace Assets.Scripts.Camera
{
	public class BillboardEffect : MonoBehaviour
	{
		private Transform cam;

		private void Start()
		{
			cam = GameObject.Find("Main Camera").transform;
		}

		void LateUpdate()
		{
			transform.LookAt(transform.position + cam.forward);
		}
	}
}
