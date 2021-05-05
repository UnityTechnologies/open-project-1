using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UnityEventBusSO", menuName = "Events/Unity Event Bus")]
public class UnityEventBusSO : ScriptableObject, IUnityEventBus
{
	private readonly UnityEventBus _unityEventBus = new UnityEventBus();

	public void Subscribe<T>(UnityAction<T> action)
	{
		_unityEventBus.Subscribe(action);
	}

	public void Unsubscribe<T>(UnityAction<T> action)
	{
		_unityEventBus.Unsubscribe(action);
	}

	public void Publish<T>(T msg)
	{
		_unityEventBus.Publish(msg);
	}
}
