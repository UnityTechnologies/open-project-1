namespace OP1.Factory
{
	/// <summary>
	/// Represents a factory.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IFactory<T>
	{
		T Create();

	} 
}
