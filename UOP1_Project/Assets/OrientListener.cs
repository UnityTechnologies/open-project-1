using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orient the listener to point in the same direction as the camera.
public class OrientListener : MonoBehaviour
{
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
