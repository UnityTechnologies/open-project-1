using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCaveLight : MonoBehaviour
{
	private IEnumerator coroutine1;
	private IEnumerator coroutine2;
	private IEnumerator coroutine3;
	public GameObject[] rockCritterClusterA;
	public Light light1;
	public GameObject[] rockCritterClusterB;
	public Light light2;
	public GameObject[] rockCritterClusterC;
	public Light light3;
	public GameObject[] rockCritterClusterD;
	public Light light4;
	public GameObject[] rockCritterClusterE;

	private void OnTriggerEnter(Collider other)
	{
		foreach (GameObject critter in rockCritterClusterA)
		{
			critter.GetComponentInChildren<RockCritterEyesEmission>().eyesEmission();
		}
		foreach (GameObject critter in rockCritterClusterB)
		{
			critter.GetComponentInChildren<RockCritterEyesEmission>().eyesEmission();
		}
		foreach (GameObject critter in rockCritterClusterC)
		{
			critter.GetComponentInChildren<RockCritterEyesEmission>().eyesEmission();
		}
		foreach (GameObject critter in rockCritterClusterD)
		{
			critter.GetComponentInChildren<RockCritterEyesEmission>().eyesEmission();
		}
		foreach (GameObject critter in rockCritterClusterE)
		{
			critter.GetComponentInChildren<RockCritterEyesEmission>().eyesEmission();
		}
		coroutine1 = IncreaseLightIntensity(light1, 10, 5f);
		coroutine2 = IncreaseLightIntensity(light2, 10, 5f);
		coroutine3 = IncreaseLightIntensity(light3, 10, 5f);
		StartCoroutine(coroutine1);
		StartCoroutine(coroutine2);
		StartCoroutine(coroutine3);

	}

	private IEnumerator IncreaseLightIntensity(Light targetObj, float toIntensity, float duration)
	{
		float counter = 0;

		//Get the current intensity of the Light 
		float startIntensity = targetObj.intensity;

		while (counter < duration)
		{
			counter += Time.deltaTime;
			targetObj.intensity = Mathf.Lerp(startIntensity, toIntensity, counter / duration);
			yield return null;
		}
	}
}
