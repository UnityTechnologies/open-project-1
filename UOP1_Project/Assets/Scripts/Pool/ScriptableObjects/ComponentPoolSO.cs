using UnityEngine;

namespace OP1.Pool
{
	public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component,IPoolable
	{
		[SerializeField]
		private string _name = default;

		private GameObject _poolRootObject;

		public override void OnEnable()
		{
			if (_poolRootObject == null)
			{
				_poolRootObject = new GameObject(_name);
			}
			base.OnEnable();
		}

		public override T Create()
		{
			T newMember = base.Create();
			newMember.transform.SetParent(_poolRootObject.transform);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			DestroyImmediate(_poolRootObject);
		}
	}
}
