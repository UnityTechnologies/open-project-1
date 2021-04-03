using UnityEditor;
using UnityEngine;
using UOP1.TagLayerTypeGenerator.Runtime.Attributes;

namespace UOP1.TagLayerTypeGenerator.Editor.Attributes
{
	/// <summary>Converts a <see cref="string" /> property into a <see cref="EditorGUI.TagField(UnityEngine.Rect,string)" />.</summary>
	[CustomPropertyDrawer(typeof(TagAttribute))]
	internal sealed class TagAttributePropertyDrawer : PropertyDrawer
	{
		/// <inheritdoc />
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (property.propertyType != SerializedPropertyType.String)
				EditorGUI.PropertyField(position, property, label);
			else
				property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

			EditorGUI.EndProperty();
		}
	}
}
