using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorSurrogateSO : ScriptableObject
{
	private static SelectorSurrogateSO _instance;
	public static SelectorSurrogateSO Instance { get { return _instance; } }
	public LoadSceneChannelSO LoadChannel;
	public PointSelection Point;

	public SelectorSurrogateSO()
	{
		_instance = this;
	}
	[RuntimeInitializeOnLoadMethod]
	public static void Initialize()
	{
		_instance = Resources.LoadAll<SelectorSurrogateSO>("Editor")[0];
	}
}
