using System;
using UnityEngine;

namespace UOP1.Extensions.UnityEngine
{
	/// <summary>
	///     Extensions for <see cref="LayerMask" />.
	/// </summary>
	public static class LayerMaskExtensions
	{
		/// <summary>
		///     Check if a layer int exists in <see cref="LayerMask" />.
		/// </summary>
		/// <param name="layerMask">The <see cref="LayerMask" /> to check against.</param>
		/// <param name="layer">The layer int to check.</param>
		/// <returns>True if <paramref name="layer" /> is in <paramref name="layerMask" />.</returns>
		public static bool Contains(this LayerMask layerMask, int layer)
		{
			return layerMask == (layerMask | (1 << layer));
		}

		/// <summary>
		///     Check if a <see cref="Layer" /> exists in the <see cref="LayerMask" />.
		/// </summary>
		/// <param name="layerMask">The <see cref="LayerMask" /> to check against.</param>
		/// <param name="layer">The <see cref="Layer" /> to check.</param>
		/// <returns>True if <paramref name="layer" /> is in <paramref name="layerMask" />.</returns>
		public static bool Contains(this LayerMask layerMask, Layer layer)
		{
			return layerMask.Contains((int)layer);
		}

		/// <summary>
		///     Check if a <see cref="GameObject" />'s layer exists in the <see cref="LayerMask" />.
		/// </summary>
		/// <param name="layerMask">The <see cref="LayerMask" /> to check against.</param>
		/// <param name="gameObject">The <see cref="GameObject" />'s layer to check.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="gameObject" /> is null.</exception>
		/// <returns>True if <paramref name="gameObject" />.layer is in <paramref name="layerMask" />.</returns>
		public static bool Contains(this LayerMask layerMask, GameObject gameObject)
		{
			if (gameObject == null) throw new ArgumentNullException(nameof(gameObject));
			return layerMask.Contains(gameObject.layer);
		}
	}
}
