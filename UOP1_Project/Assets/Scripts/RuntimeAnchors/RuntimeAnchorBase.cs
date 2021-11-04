using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.Events;

public class RuntimeAnchorBase<T> : DescriptionBaseSO where T : UnityEngine.Object
{
	public UnityAction OnAnchorProvided;

	[Header("Debug")]
	[ReadOnly] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

	[ReadOnly] [SerializeField] private T _value;
	public T Value
	{
		get { return _value; }
	}

	public void Provide(T value)
	{
		if(value == null)
		{
			Debug.LogError("A null value was provided to the " + this.name + " runtime anchor.");
			return;
		}

		_value = value;
		isSet = true;
		
		if(OnAnchorProvided != null)
			OnAnchorProvided.Invoke();
	}

	public void Unset()
	{
		_value = null;
		isSet = false;
	}

	private void OnDisable()
	{
		Unset();
	}
}