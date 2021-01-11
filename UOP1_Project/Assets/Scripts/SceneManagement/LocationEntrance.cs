using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntrance : MonoBehaviour
{
	[Header("Asset References")]
	[SerializeField] private PathSO _entrancePath;

	public PathSO EntrancePath => _entrancePath;
}
