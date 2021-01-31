using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRotation : MonoBehaviour
{
	public float rotationSpeed=0.5f;

    // Update is called once per frame
    void Update()
    {
		transform.RotateAroundLocal(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
