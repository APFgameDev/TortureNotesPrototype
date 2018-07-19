using UnityEngine;
using UnityEditor;
public class LabelOverride : PropertyAttribute
{
    public string label;
    public LabelOverride(string label)
    {
        this.label = label;
    }
}

[CustomPropertyDrawer(typeof(LabelOverride))]
public class ThisPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var propertyAttribute = this.attribute as LabelOverride;
        label.text = propertyAttribute.label;
        EditorGUI.PropertyField(position, property, label);
    }
}