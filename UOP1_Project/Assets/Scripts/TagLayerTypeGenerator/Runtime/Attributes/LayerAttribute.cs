using System;
using UnityEngine;

namespace UOP1.TagLayerTypeGenerator.Runtime.Attributes
{
	/// <summary>Apply to any <see cref="int" /> property and it'll be converted into a layer field in the inspector.</summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class LayerAttribute : PropertyAttribute
	{
	}
}
