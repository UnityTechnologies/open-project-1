using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts
{
    public class RotatorScript : MonoBehaviour
    {
        [SerializeField] public float rotationSpeedDegreesPerSecond;
        private float currentAngle = 0;//current rotation angle in degrees
        private Quaternion startRotation = Quaternion.identity;
        // Start is called before the first frame update
        void OnEnable()
        {
            startRotation = gameObject.transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            currentAngle += rotationSpeedDegreesPerSecond * Time.deltaTime;
            if (currentAngle > 360f)
            {
                currentAngle -= 360f;
            }
            gameObject.transform.rotation = Quaternion.Euler(0, currentAngle, 0) * startRotation;
        }
    }
}
