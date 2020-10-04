using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //  [Tooltip("leave empty if using distancetrigger")] [SerializeField] private   bool usingcollidertrigger;
    //  [Tooltip("leave at 0 if using colidertrigger")] [SerializeField] private float distancetrigger;
    [SerializeField] public Conversation Conversation;
    // Start is called before the first frame update
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
            GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().StartCoroutine(GameObject.FindGameObjectWithTag("subtitles").GetComponent<DialogueRendererer>().NewChat(Conversation)); ;
         
        }

    }
}
