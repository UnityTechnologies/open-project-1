using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control the amount of dash movement
/// </summary>
public class Dasher : MonoBehaviour
{
	/// <summary>
	/// The dash speed, controlled by an animation clip
	/// </summary>
	public float dashMagnitude;

	/// <summary>
	/// The dash movement vector to be applied
	/// </summary>
	public Vector3 DashMovementVector => transform.forward * dashMagnitude;

	private void OnDrawGizmos()
	{
		Vector3 ray = DashMovementVector * Time.deltaTime;

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, ray);
		Gizmos.DrawWireSphere(transform.position + ray, 0.1f);
	}
}
