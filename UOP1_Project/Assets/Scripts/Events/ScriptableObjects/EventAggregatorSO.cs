using UnityEngine;

[CreateAssetMenu(fileName = "EventAggregatorSO", menuName = "Events/Event Aggregator")]
public class EventAggregatorSO : ScriptableObject, IEventAggregator
{
	private readonly EventAggregator _eventAggregator = new EventAggregator();

	public void Subscribe(object observer)
	{
		_eventAggregator.Subscribe(observer);
	}

	public void Unsubscribe(object observer)
	{
		_eventAggregator.Unsubscribe(observer);
	}

	public void Publish(object msg)
	{
		_eventAggregator.Publish(msg);
	}
}
