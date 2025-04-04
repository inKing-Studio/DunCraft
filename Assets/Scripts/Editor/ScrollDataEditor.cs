using UnityEditor;
using UnityEngine;

// Target the ScrollData class
[CustomEditor(typeof(ScrollData))]
public class ScrollDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector GUI, which respects attributes like [Header] and [Tooltip]
        DrawDefaultInspector();

        // You could add custom GUI elements here later if needed,
        // but for now, just drawing the default is enough.

        // Apply changes if the inspector was modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}