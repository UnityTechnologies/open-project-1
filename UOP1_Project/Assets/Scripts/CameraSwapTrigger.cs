using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera cameraToSwap;
    
    [Header("Asset references")]
    [SerializeField] private VcamEventChannelSO VcamEventChannel = default;

    BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


   void OnTriggerExit(Collider other)
   {
        // check if it is the player
        if(other.CompareTag("Player")){
            
            //check which side the player exited. we consider only the z axis so the enter and exit faces should be along the local z axis.
            
            Vector3 playerPosInLocalCoordinates = transform.InverseTransformPoint(other.transform.position);
            
            if(boxCollider.center.z > playerPosInLocalCoordinates.z){
                
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
