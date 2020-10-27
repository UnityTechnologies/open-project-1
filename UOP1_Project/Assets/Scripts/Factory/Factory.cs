namespace OP1.Factory
{
	/// <summary>
	/// Implements the IFactory interface for non-abstract types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Factory<T> : IFactory<T> where T : new()
	{
		public T Create()
		{
			return new T();
		}

	} 
}
