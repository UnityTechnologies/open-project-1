using UnityEngine;

namespace OP1.Pool
{
	/// <summary>
	/// Implements a Pool for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to pool.</typeparam>
	public abstract class ComponentPool<T> : Pool<T> where T : Component, IPoolable
	{
		public abstract int InitialPoolSize { get; set; }
		private GameObject _poolRootObject;

		private void InitializePool()
		{
			_poolRootObject = new GameObject(name);
			DontDestroyOnLoad(_poolRootObject);
			for (int i = 0; i < InitialPoolSize; i++)
			{
				_available.Push(Add());
			}
		}

		public override T Request()
		{
			if (_poolRootObject == null)
			{
				InitializePool();
			}
			return base.Request();
		}

		public override void Return(T member)
		{
			if (_poolRootObject == null)
			{
				InitializePool();
			}
			base.Return(member);
		}

		public override T Add()
		{
			T newMember = base.Add();
			newMember.transform.SetParent(_poolRootObject.transform);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			DestroyImmediate(_poolRootObject);
		}
	}
}
