using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera cameraToSwap;
    [Tooltip("if this is set, the camera will be swapped when the player enters the volume of the trigger and go back to default when he exist the volume. Otherwise the camera swap will take place based on whether the player exits along or opposye to local z axis")]
    public bool UseVolume = false;
    
    [Header("Asset references")]
    [SerializeField] private VcamEventChannelSO VcamEventChannel = default;

    Collider Collider;

    void Start()
    {
        Collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            if(UseVolume){
                //player entered the volume so switch to our camera
                VcamEventChannel.OnEventRaised(cameraToSwap);
            }
        }
    }


   void OnTriggerExit(Collider other)
   {
        // check if it is the player
        if(other.CompareTag("Player")){
            
            if(UseVolume){
                //player exited the volume so switch back to default cam
                VcamEventChannel.OnEventRaised(null);
            }
            else{
                //check which side the player exited. we consider only the z axis so the enter and exit faces should be along the local z axis.
                Vector3 playerPosInLocalCoordinates = transform.InverseTransformPoint(other.transform.position);
                
                if(transform.InverseTransformPoint(Collider.bounds.center).z > playerPosInLocalCoordinates.z){
                    
                    //player exited on -ve z side which means we switch to the default camera
                    VcamEventChannel.OnEventRaised(null);

                }
                else{

                    //player exited on +ve z side which means we switch to our camera
                    VcamEventChannel.OnEventRaised(cameraToSwap);
                
                }
            }
        }
   }

}
