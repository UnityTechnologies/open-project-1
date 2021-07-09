using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCritterEyesEmission : MonoBehaviour
{
	public Material Material1;
	public GameObject critter;

	private void Start()
	{
		//critter.GetComponent<MeshRenderer>().material = Material1;
	}
	private void OnTriggerEnter(Collider other)
	{
		eyesEmission();
	}

	public void eyesEmission()
	{
		critter.GetComponent<SkinnedMeshRenderer>().material = Material1;
	}
}
