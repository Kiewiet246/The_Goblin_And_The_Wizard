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
         gridManager.ClearGrid();
      }
   }
}
