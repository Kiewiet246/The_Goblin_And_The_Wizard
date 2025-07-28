using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GM_EditorSct : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();
      
      GridManager gridManager = (GridManager)target;

      if (GUILayout.Button("Clear Grid"))
      {
         gridManager.ClearGridEditor();
      }

      if (GUILayout.Button("Generate Grid"))
      {
         gridManager.CreateGrid();
      }
      
      if (GUILayout.Button("Find Path"))
      {
         gridManager.FindPath();
      }

      if (GUILayout.Button("Generate Path"))
      {
         gridManager.RandomLocations();
      }
   }
}
