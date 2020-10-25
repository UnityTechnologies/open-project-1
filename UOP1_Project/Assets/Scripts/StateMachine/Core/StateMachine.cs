using System;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.StateMachine
{
	public class StateMachine : MonoBehaviour
	{
#if UNITY_EDITOR
		public string CurrentState;
		public bool debug;
#endif
		[Tooltip("Set the initial state of this StateMachine")]
		[SerializeField] private ScriptableObjects.StateSO _initialStateSO = null;

		private readonly Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
		private State _currentState;

		private void Awake()
		{
			_currentState = _initialStateSO.GetState(this);
			_currentState.OnStateEnter();
#if UNITY_EDITOR
			CurrentState = _currentState.Name;
#endif
		}

		public new bool TryGetComponent<T>(out T component) where T : Component
		{
			var type = typeof(T);
			if (!_cachedComponents.TryGetValue(type, out var value))
			{
				if (base.TryGetComponent<T>(out component))
					_cachedComponents.Add(type, component);

				return component != null;
			}

			component = (T)value;
			return true;
		}

		public T GetOrAddComponent<T>() where T : Component
		{
			if (!TryGetComponent<T>(out var component))
			{
				component = gameObject.AddComponent<T>();
				_cachedComponents.Add(typeof(T), component);
			}

			return component;
		}

		public new T GetComponent<T>() where T : Component
		{
			return TryGetComponent(out T component)
				? component : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}.");
		}

		private void Update()
		{
			if (_currentState.TryGetTransition(out var transitionState))
				Transition(transitionState);

			_currentState.OnUpdate();
		}

		private void Transition(State transitionState)
		{
			_currentState.OnStateExit();
			_currentState = transitionState;
			_currentState.OnStateEnter();
#if UNITY_EDITOR
			if (debug)
				Debug.Log($"{name} entering state {_currentState.Name}");
			CurrentState = _currentState.Name;
#endif
		}
	}
}
