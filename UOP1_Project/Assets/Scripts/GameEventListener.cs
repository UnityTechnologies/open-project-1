using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        //Check if the event exists to avoid errors
        if (Event == null)
        {
            return;
        }
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (Event == null)
        {
            return;
        }
        Event.UnregisterListener(this); 
    }

    public void OnEventRaised()
    {
        if (Response == null)
        {
            return;
        }
        Response.Invoke(); 
    }
}