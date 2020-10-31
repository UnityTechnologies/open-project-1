﻿using UnityEngine;

namespace OP1.Factory
{
	/// <summary>
	/// Implements the IFactory interface for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to create.</typeparam>
	public abstract class ComponentFactorySO<T> : ScriptableObject,IFactory<T> where T : Component
	{
		public abstract T Prefab
		{
			get;
		}

		public T Create()
		{
			return Instantiate(Prefab);
		}
	} 
}
