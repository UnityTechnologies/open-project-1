using UOP1.Factory;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.Pool
{
	/// <summary>
	/// A generic pool that generates members of type T on-demand via a factory.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements to pool.</typeparam>
	public abstract class PoolSO<T> : ScriptableObject, IPool<T> where T : IPoolable
	{
		protected readonly Stack<T> _available = new Stack<T>();
		public abstract IFactory<T> Factory { get; set; }

		protected virtual T Create()
		{
			return Factory.Create();
		}

		public virtual T Request()
		{
			T member = _available.Count > 0 ? _available.Pop() : Create();
			member.OnRequest();
			return member;
		}

		public virtual IEnumerable<T> Request(int num = 1)
		{
			List<T> members = new List<T>(num);
			for (int i = 0; i < num; i++)
			{
				members.Add(Request());
			}
			return members;
		}

		public virtual void Return(T member)
		{
			member.OnReturn(() =>
			{
				_available.Push(member);
			});
		}

		public virtual void Return(IEnumerable<T> members)
		{
			foreach (T member in members)
			{
				Return(member);
			}
		}

		public virtual void OnDisable()
		{
			_available.Clear();
		}
	}
}
