using System;

namespace OP1.Pool
{
	/// <summary>
	/// Represents an object that can be pooled.
	/// </summary>
	public interface IPoolable
	{
		void OnRequest();
		void OnReturn(Action onReturned);
	} 
}
