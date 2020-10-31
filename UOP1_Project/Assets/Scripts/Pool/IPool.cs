using System.Collections.Generic;

namespace OP1.Pool
{
	/// <summary>
	/// Represents a collection that pools objects of T
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>
	public interface IPool<T>
	{
		T Request();
		IEnumerable<T> Request(int num);
		void Return(T member);
		void Return(IEnumerable<T> members);
	}
}
