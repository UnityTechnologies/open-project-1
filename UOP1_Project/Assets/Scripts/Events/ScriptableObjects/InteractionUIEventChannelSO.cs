using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events to toggle the interaction UI.
/// Example: Dispaly or hide the interaction UI via a bool and the interaction type from the enum via int
/// </summary>

[CreateAssetMenu(menuName = "Events/Toogle Interaction UI Event Channel")]
public class InteractionUIEventChannelSO : ScriptableObject
{
	public UnityAction<bool, Interaction> OnEventRaised;
	public void RaiseEvent(bool state, Interaction interactionType)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(state, interactionType);
	}

}

//maybe make a SO for the interaction type so we can access it from both the UI script
//and the Interaction script
//we have a field for the current interaction so no need to pass it as paramater
//in the event...

