using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool> {

}

public class ZoneTriggerController : MonoBehaviour
{
	[SerializeField] private BoolEvent _enterZone = default;
	[SerializeField] private LayerMask _layers = default;

	private void OnTriggerEnter(Collider other)
	{
		if ((1 << other.gameObject.layer & _layers) != 0)
		{
			_enterZone.Invoke(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((1 << other.gameObject.layer & _layers) != 0)
		{
			_enterZone.Invoke(false);
		}
	}
}
