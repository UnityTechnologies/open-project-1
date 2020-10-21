using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidGameEvent", menuName = "Game Event/Void")]
public class VoidGameEvent : ScriptableObject
{
    public UnityAction eventRaised;
	public void Raise()
	{
		if(eventRaised != null) eventRaised.Invoke();
	}
}


