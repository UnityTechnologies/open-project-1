using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Dialogue line Channel")]
public class DialogueLineChannelSO : ScriptableObject
{
	public UnityAction<DialogueLineSO> OnEventRaised;
	public void RaiseEvent(DialogueLineSO line)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(line);
	}
}
