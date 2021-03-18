using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamRequiresPlayerTarget : MonoBehaviour
{
    public bool followsPlayer = true;
    public bool looksAtPlayer = true;

    CinemachineVirtualCameraBase vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCameraBase>();        
    }

    public void SetPlayerTarget(Transform playerTransform){
        
        if(followsPlayer){
            vcam.Follow = playerTransform;
        }

        if(looksAtPlayer){
            vcam.LookAt = playerTransform;
        }
        
    }

}
