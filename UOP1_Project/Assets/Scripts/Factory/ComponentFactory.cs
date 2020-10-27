using UnityEngine;

namespace OP1.Factory
{
	/// <summary>
	/// Implements the IFactory interface for Component types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ComponentFactory<T> : IFactory<T> where T : Component
	{
		string _name;
		GameObject _parent;
		GameObject _prefab;

		int _index = 0;

		public ComponentFactory() : this($"{typeof(T).Name}Pool") { }
		public ComponentFactory(string name) : this(name, new GameObject(name)) { }
		public ComponentFactory(string name, GameObject parent) : this(name, parent, new GameObject(name)) { }
		public ComponentFactory(string name, GameObject parent, GameObject prefab)
		{
			this._name = name;
			_parent = parent != null ? parent : new GameObject($"{this._name}Pool");
			_prefab = prefab;
		}

		public T Create()
		{
			GameObject tempGameObject = GameObject.Instantiate(_prefab, _parent.transform, true);
			tempGameObject.name += _index;
			_index++;
			tempGameObject.SetActive(false);
			return tempGameObject.GetComponent<T>() ?? tempGameObject.AddComponent<T>();
		}

	} 
}
