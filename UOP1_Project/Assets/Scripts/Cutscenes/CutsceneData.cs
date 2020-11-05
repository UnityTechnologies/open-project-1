using System; 
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CutsceneData
{
	[Tooltip("Event that is called after the Cutscene reached its end")]
	public UnityEvent Event;
}
