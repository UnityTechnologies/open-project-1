using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Dialogue line Channel")]
public class DialogueLineChannelSO : ScriptableObject
{
	public UnityAction<DialogueLineSO, ActorSO> OnEventRaised;
	public void RaiseEvent(DialogueLineSO line, ActorSO actor)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(line, actor);
	}
}
