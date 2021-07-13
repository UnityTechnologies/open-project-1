using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

	public bool IsPressed { get; set; }
    public float _maxDescent;

    void PlayerHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > .95f)
        {
            Debug.Log("player is on top of " + hit.gameObject.name);
            IsPressed = true;
        }
    }

	
	private void OnTriggerEnter(Collider other)
	{
		//if (hit.normal.y > .95f)
		//{
			//Debug.Log("player is on top of " + hit.gameObject.name);
		
			IsPressed = true;
		
			
		//}
	}
	
}
