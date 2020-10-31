using UnityEngine;

namespace OP1.Factory
{
	public class FactorySO<T> : ScriptableObject, IFactory<T> where T : new()
	{
		public virtual T Create()
		{
			return new T();
		}
	}
}
