using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public GameEvent onLevelEnd;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onLevelEnd.Raise();
        }
    }
}
