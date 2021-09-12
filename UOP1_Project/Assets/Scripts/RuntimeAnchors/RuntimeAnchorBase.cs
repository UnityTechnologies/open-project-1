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
		set
		{
			_value = value;
			isSet = _value != null;
			
			if(OnAnchorProvided != null
				&& isSet)
			{
				OnAnchorProvided.Invoke();
			}
			
		}
	}

	private void OnDisable()
	{
		_value = null;
		isSet = false;
	}
}