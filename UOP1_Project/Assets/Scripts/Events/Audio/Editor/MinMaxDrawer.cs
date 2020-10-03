#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        var minMax = attribute as MinMaxAttribute;
        var range = minMax.range;

        var value = property.vector2Value;
        var minValue = value.x;
        var maxValue = value.y;

        var valueRect = new Rect(position) { width = 40 };
        var sliderRect = new Rect(position);
        const int padding = 6;

        EditorGUI.BeginChangeCheck();
        value.x = EditorGUI.FloatField(valueRect, Mathf.Round(value.x * 100) / 100);
        if(EditorGUI.EndChangeCheck())
            property.vector2Value = new Vector2(Mathf.Round(value.x * 100) / 100, value.y);
        
        EditorGUI.BeginChangeCheck();
        sliderRect.x += valueRect.width + padding;
        sliderRect.width -= valueRect.width * 2 + padding * 2;
        EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.x, range.y);

        if(EditorGUI.EndChangeCheck())
            property.vector2Value = new Vector2(minValue, maxValue);
        
        EditorGUI.BeginChangeCheck();
        valueRect.x += position.width - valueRect.width;
        value.y = EditorGUI.FloatField(valueRect, Mathf.Round(maxValue * 100) / 100);
        if(EditorGUI.EndChangeCheck())
            property.vector2Value = new Vector2(value.x, Mathf.Round(value.y * 100) / 100);

        EditorGUI.EndProperty();
    }
/*
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        var minMax = attribute as MinMaxAttribute;
        var range = minMax.range;

        var value = property.vector2Value;
        
        var minMaxSlider = new MinMaxSlider(value.x, value.y, range.x, range.y);
        minMaxSlider.RegisterValueChangedCallback(evt => property.vector2Value = evt.newValue);
        
        root.Add(minMaxSlider);

        return root;
    }
    */
}
#endif