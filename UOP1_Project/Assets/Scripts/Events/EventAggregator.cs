using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


/// <summary>
/// Enables loose coupling of events from publishers to subscribers
///
/// inspired from https://github.com/Caliburn-Micro/Caliburn.Micro/blob/master/src/Caliburn.Micro.Core/IEventAggregator.cs
/// and also https://github.com/google/guava/blob/master/guava/src/com/google/common/eventbus/EventBus.java
///
/// Runs within single threaded unity environment so no need to lock any structures.
/// There is no current requirement to dispatch or consume events asynchronously and sync with unity main thread.
/// </summary>
public interface IEventAggregator
{
	void Subscribe(object observer);

	void Unsubscribe(object observer);

	void Publish(object msg);
}

public class EventAggregator : IEventAggregator
{
	private readonly Dictionary<Type, HashSet<Handler>> _handlers = new Dictionary<Type, HashSet<Handler>>();
	private readonly Dictionary<object, Subscriptions> _subByObserver = new Dictionary<object, Subscriptions>();
	private readonly object[] _paramHolder = {null};

	public void Subscribe(object observer)
	{
		if (observer == null)
		{
			throw new ArgumentNullException(nameof(observer));
		}

		if (_subByObserver.ContainsKey(observer))
		{
			return;
		}

		var subs = new Subscriptions();

		var interfaces = observer.GetType().GetTypeInfo().ImplementedInterfaces
			.Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));

		foreach (var @interface in interfaces)
		{
			var type = @interface.GetTypeInfo().GenericTypeArguments[0];
			var method = @interface.GetRuntimeMethod("Handle", new[] {type});

			if (method != null)
			{
				var handler = new Handler(method, observer);

				if (!_handlers.TryGetValue(type, out var handlers))
				{
					handlers = new HashSet<Handler>();
					_handlers[type] = handlers;
				}

				handlers.Add(handler);
				subs.Interested[type] = handler;
			}
		}

		if (subs.Interested.Count > 0)
		{
			_subByObserver[observer] = subs;
		}
	}

	public void Unsubscribe(object observer)
	{
		if (observer == null)
		{
			throw new ArgumentNullException(nameof(observer));
		}


		if (_subByObserver.TryGetValue(observer, out var subscriptions))
		{
			_subByObserver.Remove(observer);

			foreach (var entry in subscriptions.Interested)
			{
				if (_handlers.TryGetValue(entry.Key, out var handlers))
				{
					handlers.Remove(entry.Value);
				}
			}
		}
	}

	public void Publish(object msg)
	{
		if (msg == null)
		{
			throw new ArgumentNullException(nameof(msg));
		}

		if (_handlers.TryGetValue(msg.GetType(), out var handlers))
		{
			_paramHolder[0] = msg;

			foreach (var handler in handlers)
			{
				handler.Notify(_paramHolder);
			}
		}
	}

	private class Subscriptions
	{
		private readonly Dictionary<Type, Handler> _interested = new Dictionary<Type, Handler>();

		public Dictionary<Type, Handler> Interested => _interested;
	}

	private class Handler
	{
		private readonly MethodInfo _method;
		private readonly object _observer;

		public Handler(MethodInfo method, object observer)
		{
			_method = method;
			_observer = observer;
		}

		public void Notify(object[] param)
		{
			_method.Invoke(_observer, param);
		}
	}

	public interface IHandle<T>
	{
		void Handle(T msg);
	}
}
