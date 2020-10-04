using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public Conversation[] Conversations_loop;
    [NonSerialized] public Conversation Conversation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Pig" )
        {
            Conversation = Conversations_loop[UnityEngine.Random.Range(0, Conversations_loop.Length)];
            GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().StartCoroutine(GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().NewChat(Conversation)); ;
         
        }

    }
}
