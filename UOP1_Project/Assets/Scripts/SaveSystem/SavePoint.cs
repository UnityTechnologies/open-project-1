using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [Header("The Save Game Event")]
    [SerializeField] private GameObjectEventChannelSO saveGameEvent;

    [Header("Player Spawn Point")]
    [Tooltip("The point where the player is spawned when game is loaded")]
    public Transform playerSpawnPoint;

    public void Save()
    {
        if (saveGameEvent != null)
        {
            saveGameEvent.RaiseEvent(this.gameObject);
        }
    }
}
