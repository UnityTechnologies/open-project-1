using System.Collections.Generic;
using OP1.Factory;

namespace OP1.Pool
{
	/// <summary>
	/// A generic pool that generates members of type T on-demand via a factory.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>
	public class Pool<T> : IPool<T> where T : IPoolable
	{
		private readonly Stack<T> _available = new Stack<T>();
		private readonly IFactory<T> _factory;

		public Pool(IFactory<T> factory) : this(factory, 0) { }

		public Pool(IFactory<T> factory, int initialPoolSize)
		{
			this._factory = factory;

			for (int i = 0; i < initialPoolSize; i++)
			{
				_available.Push(_factory.Create());
			}
		}

		public T Request()
		{
			if (_available.Count <= 0)
			{
				_available.Push(_factory.Create());
			}
			T member = _available.Pop();
			member.Initialize();
			return member;
		}

		public IEnumerable<T> Request(int num=1)
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
			foreach(T member in members)
			{
				Return(member);
			}
		}

	} 
}

