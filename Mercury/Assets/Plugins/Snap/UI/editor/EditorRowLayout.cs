using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RowLayout))]
public class EditorRowLayout : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RowLayout layout = (RowLayout)target;
        if (GUILayout.Button("Do Layout"))
        {
            Undo.RegisterFullObjectHierarchyUndo(layout.gameObject, "Layout");
            layout.InvalidateLayout();
            layout.ValidateLayout();
            EditorUtility.SetDirty(layout.gameObject);
        }
    }
}
