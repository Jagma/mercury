using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ColumnLayout))]
public class EditorColumnLayout : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColumnLayout layout = (ColumnLayout)target;
        if (GUILayout.Button("Do Layout"))
        {
            Undo.RegisterFullObjectHierarchyUndo(layout.gameObject, "Layout");
            layout.InvalidateLayout();
            layout.ValidateLayout();
            EditorUtility.SetDirty(layout.gameObject);
        }
    }
}
