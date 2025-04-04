using UnityEditor;
using UnityEngine;

// Associate this drawer with the MaterialCategory enum
[CustomPropertyDrawer(typeof(MaterialCategory))]
public class MaterialCategoryDrawer : PropertyDrawer
{
    // Override the OnGUI method to draw the custom inspector field
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin drawing the property field
        EditorGUI.BeginProperty(position, label, property);

        // Draw the EnumFlagsField, which provides the multi-select dropdown for flags enums
        // property.intValue holds the combined bitmask value of the selected enum flags
        // The third argument ensures the dropdown shows the enum type correctly
        property.intValue = (int)(MaterialCategory)EditorGUI.EnumFlagsField(position, label, (MaterialCategory)property.intValue);

        // End drawing the property field
        EditorGUI.EndProperty();
    }
}