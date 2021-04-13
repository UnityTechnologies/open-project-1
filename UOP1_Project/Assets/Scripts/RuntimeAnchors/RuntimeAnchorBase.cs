using System;

/// <summary>Used to loosely couple objects to one another.</summary>
/// <typeparam name="T">The type of anchor to encapsulate.</typeparam>
public abstract class RuntimeAnchorBase<T> : DescriptionBaseSO where T : class
{
	/// <summary>We use a weak reference so we don't hold a strong reference to whatever we're anchoring.</summary>
	private WeakReference<T> _anchor;

	/// <summary>Scripts can check if the anchor is null before using it by checking this bool.</summary>
	public bool IsSet => _anchor != null;

	/// <summary>Gets or sets the <typeparamref name="T" /> anchor.</summary>
	/// <exception cref="InvalidOperationException">If no anchor has been set or the encapsulated object has been destroyed.</exception>
	public T Anchor
	{
		get
		{
			if (_anchor == null)
				throw new InvalidOperationException($"{nameof(Anchor)} has not been set.");

			if (_anchor.TryGetTarget(out T target))
				return target;

			throw new InvalidOperationException($"Anchor to {typeof(T)} has been destroyed.");
		}
		set
		{
			if (_anchor != null)
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

	/// <summary>Returns the <typeparamref name="T" /> via implicitly casting the <see cref="RuntimeAnchorBase{T}" /> to its <typeparamref name="T" />.</summary>
	/// <param name="anchor">The anchor being cast to it's encapsulating type.</param>
	/// <exception cref="InvalidOperationException">If the anchor being cast is <see langword="null" />.</exception>
	/// <returns>The <typeparamref name="T" /> being anchored.</returns>
	public static implicit operator T(RuntimeAnchorBase<T> anchor)
	{
		if (anchor == null)
			throw new InvalidOperationException("Cannot cast a 'null' anchor.");

		return anchor.Anchor;
	}
}
