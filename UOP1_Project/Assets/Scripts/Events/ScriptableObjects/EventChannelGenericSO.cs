using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannelGenericSO <T> : EventChannelBaseSO
{
	protected UnityAction<T> action;

	public void RegisterEvent(UnityAction<T> action) {
		this.action += action;
	}

	public void UnregisterEvent(UnityAction<T> action)
	{
		this.action -= action;
	}

	public void RaiseEvent(T value)
	{
		if (action != null)
			action.Invoke(value);
	}

}
