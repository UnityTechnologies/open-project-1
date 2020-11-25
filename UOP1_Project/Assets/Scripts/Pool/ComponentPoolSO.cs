using UnityEngine;

namespace UOP1.Pool
{
	/// <summary>
	/// Implements a Pool for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to pool.</typeparam>
	public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
	{
		private GameObject _poolRootObject;
		private GameObject PoolRootObject
		{
			get
			{
				if (!Application.isPlaying)
				{
					return null;
				}
				if (_poolRootObject == null)
				{
					_poolRootObject = new GameObject(name);
					//DontDestroyOnLoad(_poolRootObject);
				}
				return _poolRootObject;
			}
		}

		public override T Request()
		{
			T member = base.Request();
			member.gameObject.SetActive(true);
			return member;
		}

		public override void Return(T member)
		{
			member.transform.SetParent(PoolRootObject.transform);
			member.gameObject.SetActive(false);
			base.Return(member);
		}

		protected override T Create()
		{
			T newMember = base.Create();
			newMember.transform.SetParent(PoolRootObject.transform);
			newMember.gameObject.SetActive(false);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
#if UNITY_EDITOR
			DestroyImmediate(PoolRootObject);
#else
			Destroy(PoolRootObject);
#endif
		}
	}
}
