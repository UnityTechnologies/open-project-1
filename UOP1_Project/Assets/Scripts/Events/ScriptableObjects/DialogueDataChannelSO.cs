using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for talk interaction events.
/// Example: start talking to an actor passed as paramater
/// </summary>

[CreateAssetMenu(menuName = "Events/Dialogue Data Channel")]
public class DialogueDataChannelSO : ScriptableObject
{
	public UnityAction<DialogueDataSO> OnEventRaised;
	public void RaiseEvent(DialogueDataSO dialogue)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(dialogue);
	}
}

