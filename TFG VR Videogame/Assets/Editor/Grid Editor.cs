using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Default Inspector GUI
        DrawDefaultInspector();

        // Reference to the script overrided
        GridManager myComponent = (GridManager)target;

        // Add Generate Tile Button
        if (GUILayout.Button("Generate Grid"))
        {
            myComponent.GenerateGrid();
        }

        // Add Delete Button
        if (GUILayout.Button("Delete Grid Tiles"))
        {
            // Delete Tiles
            myComponent.DeleteGrid();
        }
    }
}
