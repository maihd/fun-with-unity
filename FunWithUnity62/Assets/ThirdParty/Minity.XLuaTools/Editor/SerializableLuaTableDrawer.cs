using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(SerializableLuaTable))]
public class SerializableLuaTableDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keysProperty = property.FindPropertyRelative("Keys");

        var defaultHeight = base.GetPropertyHeight(property, label);

        var height = defaultHeight;
        for (int i = 0, n = keysProperty.arraySize; i < n; i++)
        {
            height += defaultHeight;
            height += defaultHeight;
        }
        height += defaultHeight;
        height += defaultHeight;
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var keysProperty = property.FindPropertyRelative("Keys");
        var valuesProperty = property.FindPropertyRelative("Values");

        if (keysProperty.arraySize != valuesProperty.arraySize)
        {
            valuesProperty.arraySize = keysProperty.arraySize;
        }

        var elementHeight = base.GetPropertyHeight(property, label);
        var elementRect = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
        var fullRect = new Rect(position.x, position.y, position.width, elementHeight * (2 * keysProperty.arraySize + 3));

        EditorGUI.BeginProperty(fullRect, label, property);

        EditorGUI.PrefixLabel(fullRect, GUIUtility.GetControlID(FocusType.Passive), label);

        var indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        for (int i = 0, n = keysProperty.arraySize; i < n; i++)
        {
            EditorGUI.PropertyField(elementRect, keysProperty.GetArrayElementAtIndex(i), GUIContent.none);
            elementRect.y += elementRect.height;

            EditorGUI.PropertyField(elementRect, valuesProperty.GetArrayElementAtIndex(i), GUIContent.none);
            elementRect.y += elementRect.height;
        }

        if (GUI.Button(elementRect, "+"))
        {
            Debug.Log("Add more items");

            int index = keysProperty.arraySize;

            keysProperty.arraySize++;
            valuesProperty.arraySize++;

            keysProperty.GetArrayElementAtIndex(index).stringValue = "";
        }
        
        elementRect.y += elementRect.height;
        if (GUI.Button(elementRect, "-"))
        {
            Debug.Log("Remove one items");

            keysProperty.arraySize--;
            valuesProperty.arraySize--;
        }

        EditorGUI.indentLevel = indentLevel;

        EditorGUI.EndProperty();
    }
}