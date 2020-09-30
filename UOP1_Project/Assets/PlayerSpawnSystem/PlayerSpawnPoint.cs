using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public string label;
    
    void Start()
    {
        RegisterAsSpawnPoint();
    }

    void RegisterAsSpawnPoint()
    {
        PlayerLevelChange.main.AddSpawnPoint(this);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}