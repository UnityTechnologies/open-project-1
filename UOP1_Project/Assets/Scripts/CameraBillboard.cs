using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 camForward = mainCam.transform.forward;
        transform.forward = new Vector3(camForward.x, transform.forward.y, camForward.z);
    }
 }
