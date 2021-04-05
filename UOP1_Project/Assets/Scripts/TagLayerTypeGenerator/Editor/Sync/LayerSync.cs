using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Editor.Sync
{
	/// <summary>Checks for updates to the Layers in the Project.</summary>
	internal sealed class LayerSync : ISync
	{
		/// <summary>Used to check if what layer strings and IDs are in the Layer type.</summary>
		private readonly HashSet<(string, int)> _inType = new HashSet<(string, int)>();

		/// <summary>Used to check if what layer strings and IDs are in the project.</summary>
		private readonly HashSet<(string, int)> _inUnity = new HashSet<(string, int)>();

		/// <inheritdoc />
		public bool IsInSync(Type generatingType)
		{
			if (generatingType == null) throw new ArgumentNullException(nameof(generatingType));

			_inUnity.Clear();

			foreach (string layer in InternalEditorUtility.layers)
			{
				string layerName = layer.Replace(" ", string.Empty);
				_inUnity.Add((layerName, LayerMask.NameToLayer(layer)));
			}

			_inType.Clear();

			var fields = generatingType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fieldInfo in fields)
				if (fieldInfo.IsLiteral)
					_inType.Add((fieldInfo.Name, (int)fieldInfo.GetValue(null)));

			return _inType.SetEquals(_inUnity);
		}
	}
}
