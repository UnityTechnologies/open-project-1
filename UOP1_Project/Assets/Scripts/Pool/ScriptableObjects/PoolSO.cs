using OP1.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OP1.Pool
{

	public abstract class PoolSO<T> : ScriptableObject, IPool<T> where T : IPoolable
	{
		private readonly Stack<T> _available = new Stack<T>();
		public abstract IFactory<T> Factory { get; }
		[SerializeField]
		private int _initialPoolSize = default;

		public virtual void OnEnable()
		{
			for (int i = 0; i < _initialPoolSize; i++)
			{
				_available.Push(Create());
			}
		}

		public virtual void OnDisable()
		{
			_available.Clear();
		}

		public virtual T Create()
		{
			return Factory.Create();
		}

		public T Request()
		{
			if (_available.Count <= 0)
			{
				_available.Push(Create());
			}
			T member = _available.Pop();
			member.Initialize();
			return member;
		}

		public IEnumerable<T> Request(int num = 1)
		{
			List<T> members = new List<T>();
			for (int i = 0; i < num; i++)
			{
				members.Add(Request());
			}
			return members;
		}

		public void Return(T member)
		{
			member.Reset(() =>
			{
				_available.Push(member);
			});
		}

		public void Return(IEnumerable<T> members)
		{
			foreach (T member in members)
			{
				Return(member);
			}
		}
	}
}
