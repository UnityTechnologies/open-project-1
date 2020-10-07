using UnityEngine;

namespace KarimCastagnini.PluggableFSM
{
	public abstract class Action<T> : ScriptableObject
	{
		//implement this method to perform operations on dataModel
		public abstract void Act(T dataModel);
	}
}
