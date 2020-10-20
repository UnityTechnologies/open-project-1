using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeivSky.StateMachine
{
	public class StateMachine : MonoBehaviour
	{
#if UNITY_EDITOR
		public string CurrentState;
		public bool debug;
#endif
		[SerializeField] private ScriptableObjects.StateSO _initialStateSO = null;
		[SerializeField] private ScriptableObject[] _scriptableObjects = null;
		private List<Type> _scriptableObjectsTypes = null;
		private readonly Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();

		private State _currentState;

		protected void Awake()
		{
			_scriptableObjectsTypes = GetObjectTypes(_scriptableObjects);
			_currentState = _initialStateSO.GetState(this);
			_currentState.OnStateEnter();
#if UNITY_EDITOR
			CurrentState = _currentState.Name;
#endif
		}

		private static List<Type> GetObjectTypes(object[] objects)
		{
			int count = objects.Length;
			var types = new List<Type>(count);
			for (int i = 0; i < count; i++)
				types.Add(objects[i].GetType());

			return types;
		}

		public bool TryGetScriptableObject<T>(out T sObject) where T : ScriptableObject
		{
			int i = _scriptableObjectsTypes.IndexOf(typeof(T));
			sObject = i >= 0 ? (T)_scriptableObjects[i] : null;
			return i >= 0;
		}

		public T GetScriptableObject<T>() where T : ScriptableObject
		{
			return TryGetScriptableObject<T>(out var sObject) ? sObject : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}");
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
			=> TryGetComponent(out T component) ? component : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}.");

		protected void Update()
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

	public abstract class StateMachine<T> : StateMachine
	{
		public abstract T GetContext();
	}
}
