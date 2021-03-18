using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwapTriggerChild : MonoBehaviour
{
    CameraSwapTriggerParent parent;
    Collider collider;
    void Start()
    {
        parent = GetComponentInParent<CameraSwapTriggerParent>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            parent.OnChildTriggerEntry(collider);
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
