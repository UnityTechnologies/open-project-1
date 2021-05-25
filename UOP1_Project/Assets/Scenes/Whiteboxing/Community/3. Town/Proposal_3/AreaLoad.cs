using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoad : MonoBehaviour
{
    public GameObject[] areaToHide;
	public GameObject[] areaToShow;
	public bool area1Hidden = false;
	public bool area2Shown = false;

	private void Awake()
	{
		for (int i = 0; i < areaToShow.Length; i++)
		{
			areaToShow[i].SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
    {
		for (int i = 0; i < areaToHide.Length; i++)
		{
			areaToHide[i].SetActive(false);
		}
		for (int i = 0; i < areaToShow.Length; i++)
		{
			areaToShow[i].SetActive(true);
		}
		area1Hidden = true;
		area2Shown = true;
	}
	private void OnTriggerExit(Collider other)
	{
		for (int i = 0; i < areaToHide.Length; i++)
		{
			areaToHide[i].SetActive(true);
		}
		for (int i = 0; i < areaToShow.Length; i++)
		{
			areaToShow[i].SetActive(false);
		}
		area1Hidden = false;
		area2Shown = false;
	}

}
