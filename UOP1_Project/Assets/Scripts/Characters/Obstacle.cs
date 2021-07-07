using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // This collider is disabled while the player is in the trigger area.
    public Collider collider;

    public void OnObstacleTriggerChange(bool entered, GameObject who)
	{
        collider.isTrigger = entered;
	}
}
