using UnityEngine;

namespace OP1.Factory
{
	/// <summary>
	/// Implements the IFactory interface for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to create.</typeparam>
	public class ComponentFactory<T> : IFactory<T> where T : Component
	{
		public T Prefab { get; }

		public ComponentFactory(T prefab)
		{
			Prefab = prefab;
		}

		public T Create()
		{
			return GameObject.Instantiate(Prefab);
		}
	} 
}
