using UnityEngine;

namespace UOP1.Pool
{
	/// <summary>
	/// Implements a Pool for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to pool.</typeparam>
	public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
	{
		public abstract int InitialPoolSize { get; set; }
		private GameObject _poolRootObject;

		private void InitializePool()
		{
			_poolRootObject = new GameObject(name);
			DontDestroyOnLoad(_poolRootObject);
			for (int i = 0; i < InitialPoolSize; i++)
			{
				_available.Push(Create());
			}
		}

		public override T Request()
		{
			if (_poolRootObject == null)
			{
				InitializePool();
			}
			T member = base.Request();
			member.gameObject.SetActive(true);
			return member;
		}

		public override void Return(T member)
		{
			if (_poolRootObject == null)
			{
				InitializePool();
			}
			member.transform.SetParent(_poolRootObject.transform);
			member.gameObject.SetActive(false);
			base.Return(member);
		}

		protected override T Create()
		{
			T newMember = base.Create();
			newMember.transform.SetParent(_poolRootObject.transform);
			newMember.gameObject.SetActive(false);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
#if UNITY_EDITOR
			DestroyImmediate(_poolRootObject);
#else
			Destroy(_poolRootObject);
#endif
		}
	}
}
