using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Collections;

[CustomPropertyDrawer(typeof(SerializableLuaValue))]
public class SerializableLuaValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var typeRect = new Rect(position.x, position.y, 85, position.height);
        var valueRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        var type = (SerializableLuaValue.ValueType)property.FindPropertyRelative("Type").intValue;
        var fieldName = "";
        switch (type)
        {
            case SerializableLuaValue.ValueType.Nil:
                break;

            case SerializableLuaValue.ValueType.Bool:
                fieldName = "AsBool";
                break;

            case SerializableLuaValue.ValueType.Number:
                fieldName = "AsNumber";
                break;

            case SerializableLuaValue.ValueType.String:
                fieldName = "AsString";
                break;

            case SerializableLuaValue.ValueType.Table:
                fieldName = "AsTable";
                break;
        }

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("Type"), GUIContent.none);
        if (type != SerializableLuaValue.ValueType.Nil)
        {
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative(fieldName), GUIContent.none);
        }

        EditorGUI.indentLevel = indentLevel;

        EditorGUI.EndProperty();
    }
}