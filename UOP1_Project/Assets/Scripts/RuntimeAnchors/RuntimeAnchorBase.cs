using System;

/// <summary>Used to loosely couple objects to one another.</summary>
/// <typeparam name="T">The type of anchor to reference.</typeparam>
public abstract class RuntimeAnchorBase<T> : DescriptionBaseSO where T : class
{
	/// <summary>We use a weak reference so we don't hold a strong reference to whatever we're anchoring.</summary>
	private WeakReference<T> _anchor;

	/// <summary>Scripts can check if the anchor is null before using it by checking this bool.</summary>
	public bool IsSet => _anchor != null;

	/// <summary>Gets or sets the name of the <typeparamref name="T" /> anchor.</summary>
	/// <exception cref="InvalidOperationException"></exception>
	public T Anchor
	{
		get
		{
			if (_anchor.TryGetTarget(out T @out))
				return @out;
			throw new InvalidOperationException($"{GetType().Name} anchor has been destroyed.");
		}
		set
		{
			if (IsSet)
				_anchor.SetTarget(value);
			else
				_anchor = new WeakReference<T>(value);
		}
	}

	/// <summary>This function is called when the scriptable object goes out of scope.</summary>
	public void OnDisable()
	{
		_anchor = null;
	}
}
