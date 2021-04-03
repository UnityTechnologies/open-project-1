using UnityEditor;
using UnityEngine;
using UOP1.TagLayerTypeGenerator.Runtime.Attributes;

namespace UOP1.TagLayerTypeGenerator.Editor.Attributes
{
	/// <summary>Converts an <see cref="int" /> property into a <see cref="EditorGUI.LayerField(UnityEngine.Rect,int)" />.</summary>
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	internal sealed class LayerAttributePropertyDrawer : PropertyDrawer
	{
		/// <inheritdoc />
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (property.propertyType != SerializedPropertyType.Integer)
				EditorGUI.PropertyField(position, property, label);
			else
				property.intValue = EditorGUI.LayerField(position, label, property.intValue);


			EditorGUI.EndProperty();
		}
	}
}
