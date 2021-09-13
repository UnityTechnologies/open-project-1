using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one gameobject argument.
/// Example: A game object pick up event event, where the GameObject is the object we are interacting with.
/// </summary>

[CreateAssetMenu(menuName = "Events/GameObject Event Channel")]
public class GameObjectEventChannelSO : DescriptionBaseSO
{
	public UnityAction<GameObject> OnEventRaised;
	
	public void RaiseEvent(GameObject value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
