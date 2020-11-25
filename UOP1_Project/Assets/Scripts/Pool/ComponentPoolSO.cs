using UnityEngine;

namespace UOP1.Pool
{
	/// <summary>
	/// Implements a Pool for Component types.
	/// </summary>
	/// <typeparam name="T">Specifies the component to pool.</typeparam>
	public abstract class ComponentPoolSO<T> : PoolSO<T> where T : Component
	{
		private Transform _poolRoot;
		private Transform PoolRoot
		{
			get
			{
				if (_poolRoot == null)
				{
					_poolRoot = new GameObject(name).transform;
				}
				return _poolRoot;
			}
		}

		private Transform _parent;
		private Transform Parent
		{
			get
			{
				if (_parent == null)
				{
					_parent = PoolRoot;
				}
				return _parent;
			}
		}

		public void SetParent(Transform t)
		{
			Transform newParent = t != null ? t : PoolRoot;
			if (_parent != null && _parent != newParent)
			{
				foreach (T member in Available)
				{
					member.transform.SetParent(newParent);
				}
				CheckCleanPoolRoot();
			}
			_parent = newParent;
		}

		void CheckCleanPoolRoot()
		{
			if (_poolRoot != null && _poolRoot != _parent && _poolRoot.childCount == 0)
			{
				Destroy(_poolRoot.gameObject);
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
			member.transform.SetParent(Parent.transform);
			member.gameObject.SetActive(false);
			CheckCleanPoolRoot();
			base.Return(member);
		}

		protected override T Create()
		{
			T newMember = base.Create();
			newMember.transform.SetParent(Parent.transform);
			newMember.gameObject.SetActive(false);
			return newMember;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			if (_poolRoot != null)
			{
#if UNITY_EDITOR
				DestroyImmediate(_poolRoot.gameObject);
#else
				Destroy(_poolRoot.gameObject);
#endif
			}
		}
	}
}
