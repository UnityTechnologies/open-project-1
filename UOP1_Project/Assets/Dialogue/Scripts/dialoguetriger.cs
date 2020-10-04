using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialoguetriger : MonoBehaviour
{
  
    [SerializeField] public conversation Conversation;
 
   
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Pig" )
        {
            GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().StartCoroutine(GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().NewChat(Conversation)); ;
         
        }

    }
}
