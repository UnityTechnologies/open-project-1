using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDatabase", menuName = "Scene Data/Surrogate")]
public class SelectorSurrogateSO : ScriptableObject
{
	private static SelectorSurrogateSO _instance;
	public static SelectorSurrogateSO Instance { get { return _instance; } }
	public LoadSceneChannelSO LoadChannel;
	public LocationSelection Location;

	public SelectorSurrogateSO()
	{
		_instance = this;
	}
	[RuntimeInitializeOnLoadMethod]
	private static void Initialize()
	{
		_instance = Resources.LoadAll<SelectorSurrogateSO>("Editor")[0];
	}
}
