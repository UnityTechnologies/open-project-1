using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapTriggerChild : MonoBehaviour
{
    CameraSwapTriggerParent parent;
    Collider collider;

    [Tooltip("camera used on enter if player enters this trigger")]
    public CinemachineVirtualCamera cameraToSwapOnEnter;

    void Start()
    {
        parent = GetComponentInParent<CameraSwapTriggerParent>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            parent.OnChildTriggerEntry(collider,cameraToSwapOnEnter);
        }
    }

    void OnTriggerExit(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            parent.OnChildTriggerExit(collider);
        }
    }
}
