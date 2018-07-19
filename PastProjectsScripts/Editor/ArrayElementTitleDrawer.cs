using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
public class ArrayElementTitleDrawer : PropertyDrawer
{
    protected virtual ArrayElementTitleAttribute Attribute
    {
        get { return (ArrayElementTitleAttribute)attribute; }
    }

    SerializedProperty TitleNameProp;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string numericValue = new String(label.text.Where(Char.IsDigit).ToArray());

        int indexValue = int.Parse(numericValue);

        string newlabel =  "";

        if (indexValue < Attribute.m_elementTitles.Length)
            newlabel = Attribute.m_elementTitles[indexValue];

        EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
    }
}

