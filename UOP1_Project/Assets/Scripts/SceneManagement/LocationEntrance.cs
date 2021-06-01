using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntrance : MonoBehaviour
{
	[SerializeField] private PathSO _entrancePath;

	public PathSO EntrancePath => _entrancePath;
}
