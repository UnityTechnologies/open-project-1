using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwapTriggerParent : MonoBehaviour
{
    public CinemachineVirtualCamera cameraToSwap;
    
    [Header("Asset references")]
    [SerializeField] private VcamEventChannelSO VcamEventChannel = default;

    Dictionary<Collider,bool> colliderDictionary;

    void Start()
    {
        colliderDictionary = new Dictionary<Collider, bool>();
        foreach (Collider col in gameObject.GetComponentsInChildren<Collider>())
        {
            colliderDictionary.Add(col,false);
        }
    }

    //returns true if the player is inside any of the child colliders
    bool IsPlayerInside()
    {
        foreach (KeyValuePair<Collider,bool> entry in colliderDictionary)
        {
            if(entry.Value)
            {
                return true;
            }
        }
        return false;
    }
   
    // called when the player enters any of the child colliders
    public void OnChildTriggerEntry(Collider childCollider)
    {

        if(colliderDictionary.ContainsKey(childCollider))
        {
            
            if(!IsPlayerInside())
            {
                // the player has entered the compound collider.
                VcamEventChannel.OnEventRaised(cameraToSwap);
            }

            colliderDictionary[childCollider] = true;
            
        }
        else
        {
            Debug.LogError(childCollider +"is not a child of "+gameObject);
        }
    }

    public void OnChildTriggerExit(Collider childCollider)
    {

        if(colliderDictionary.ContainsKey(childCollider))
        { 

            colliderDictionary[childCollider] = false;
            if(!IsPlayerInside())
            {
                // the player has left the compound collider.
                VcamEventChannel.OnEventRaised(null);
            }
            
        }
        else
        {
            Debug.LogError(childCollider +"is not a child of "+gameObject);
        }

    }


   

}
