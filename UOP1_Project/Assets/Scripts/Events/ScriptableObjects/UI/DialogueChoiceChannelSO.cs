using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Dialogue Choice Channel")]
public class DialogueChoiceChannelSO : ScriptableObject
{
	public UnityAction<Choice> OnEventRaised;
	public void RaiseEvent(Choice choice)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(choice);
	}
}
