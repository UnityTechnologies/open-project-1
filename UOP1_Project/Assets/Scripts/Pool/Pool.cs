using System.Collections;
using System.Collections.Generic;
using OP1.Factory;

namespace OP1.Pool
{
	/// <summary>
	/// A generic pool that generates members of type T on-demand via a factory.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>
	public class Pool<T> : IEnumerable where T : IPoolable
	{
		public Stack<T> _available = new Stack<T>();
		public HashSet<T> _unavailable = new HashSet<T>();
		IFactory<T> _factory;

		public Pool(IFactory<T> factory) : this(factory, 0) { }

		public Pool(IFactory<T> factory, int initialPoolSize)
		{
			this._factory = factory;

			for (int i = 0; i < initialPoolSize; i++)
			{
				_available.Push(_factory.Create());
			}
		}

		public virtual T Request()
		{
			if (_available.Count <= 0)
			{
				_available.Push(_factory.Create());
			}
			T member = _available.Pop();
			_unavailable.Add(member);
			member.Initialize();
			return member;
		}

		public void Return(T member)
		{
			member.Reset(() =>
			{
				_unavailable.Remove(member);
				_available.Push(member);
			});
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			HashSet<T> members = new HashSet<T>(_available);
			members.UnionWith(_unavailable);
			return (IEnumerator<T>)members;
		}
	} 
}

