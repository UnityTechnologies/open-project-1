using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Task Channel")]
public class TaskChannelSO : ScriptableObject
{
	public UnityAction<TaskSO> OnEventRaised;
	public void RaiseEvent(TaskSO task)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(task);
	}
}
