using UOP1.Factory;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.Pool
{
	/// <summary>
	/// A generic pool that generates members of type T on-demand via a factory.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements to pool.</typeparam>
	public abstract class PoolSO<T> : ScriptableObject, IPool<T>
	{
		protected readonly Stack<T> Available = new Stack<T>();
		/// <summary>
		/// The factory which will be used to create <typeparamref name="T"/> on demand.
		/// </summary>
		public abstract IFactory<T> Factory { get; set; }
		protected bool HasBeenPrewarmed { get; set; }

		protected virtual T Create()
		{
			return Factory.Create();
		}

		/// <summary>
		/// Prewarms the pool with a <paramref name="num"/> of <typeparamref name="T"/>.
		/// </summary>
		/// <param name="num">The number of members to create as a part of this pool.</param>
		/// <remarks>NOTE: This method can be called at any time, but only once for the lifetime of the pool.</remarks>
		public virtual void Prewarm(int num)
		{
			if (HasBeenPrewarmed)
			{
				Debug.LogWarning($"Pool {name} has already been prewarmed.");
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Available.Push(Create());
			}
			HasBeenPrewarmed = true;
		}

		/// <summary>
		/// Requests a <typeparamref name="T"/> from this pool.
		/// </summary>
		/// <returns>The requested <typeparamref name="T"/>.</returns>
		public virtual T Request()
		{
			return Available.Count > 0 ? Available.Pop() : Create();
		}

		/// <summary>
		/// Batch requests a <typeparamref name="T"/> collection from this pool.
		/// </summary>
		/// <returns>A <typeparamref name="T"/> collection.</returns>
		public virtual IEnumerable<T> Request(int num = 1)
		{
			List<T> members = new List<T>(num);
			for (int i = 0; i < num; i++)
			{
				members.Add(Request());
			}
			return members;
		}

		/// <summary>
		/// Returns a <typeparamref name="T"/> to the pool.
		/// </summary>
		/// <param name="member">The <typeparamref name="T"/> to return.</param>
		public virtual void Return(T member)
		{
			Available.Push(member);
		}

		/// <summary>
		/// Returns a <typeparamref name="T"/> collection to the pool.
		/// </summary>
		/// <param name="members">The <typeparamref name="T"/> collection to return.</param>
		public virtual void Return(IEnumerable<T> members)
		{
			foreach (T member in members)
			{
				Return(member);
			}
		}

		public virtual void OnDisable()
		{
			Available.Clear();
			HasBeenPrewarmed = false;
		}
	}
}
