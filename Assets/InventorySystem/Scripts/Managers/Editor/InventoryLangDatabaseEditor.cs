using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(InventoryLangDatabase), true)]
    public class InventoryLangDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            if (GUILayout.Button("Open language editor"))
            {
                InventoryEditorUtil.selectedLangDatabase = (InventoryLangDatabase) target;
                LangManagerEditor.ShowWindow();
            }
        }
    }
}