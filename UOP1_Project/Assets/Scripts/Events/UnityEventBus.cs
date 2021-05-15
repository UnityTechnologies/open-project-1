using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Enables loose coupling of events from publishers to subscribers
///
/// A UnityAction based subscription where the T is event type used to find
/// a suitable UnityAction.
///
/// Note the code is typesafe although somewhat ugly with HashSet<object> holder.
///
/// </summary>
public interface IUnityEventBus
{
	public void Subscribe<T>(UnityAction<T> action);

	public void Unsubscribe<T>(UnityAction<T> action);

	public void Publish<T>(T msg);
}

public class UnityEventBus : IUnityEventBus
{
	private readonly Dictionary<Type, HashSet<object>> _handlers = new Dictionary<Type, HashSet<object>>();

	public void Subscribe<T>(UnityAction<T> action)
	{
		if (action == null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		var type = typeof(T);

		if (!_handlers.TryGetValue(type, out var handlers))
		{
			handlers = new HashSet<object>();
			_handlers[type] = handlers;
		}

		handlers.Add(action);
	}

	public void Unsubscribe<T>(UnityAction<T> action)
	{
		if (action == null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		var type = typeof(T);

		if (_handlers.TryGetValue(type, out var handlers))
		{
			handlers.Remove(action);
		}
	}

	public void Publish<T>(T msg)
	{
		if (msg == null)
		{
			throw new ArgumentNullException(nameof(msg));
		}

		if (_handlers.TryGetValue(msg.GetType(), out var handlers))
		{
			foreach (var handler in handlers.Cast<UnityAction<T>>())
			{
				handler.Invoke(msg);
			}
		}
	}
}
