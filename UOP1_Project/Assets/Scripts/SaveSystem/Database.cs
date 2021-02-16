using System.Collections.Generic;
using UnityEngine;

public abstract class Database<T> : ScriptableObject where T : IDesc
{
	[SerializeField] protected List<T> _descs = new List<T>();

	public virtual object FindByUuid(string uuid)
	{
		foreach (var desc in _descs)
		{
			if (desc.DescId.uuid == uuid)
			{
				return desc;
			}
		}

		return null;
	}
}
