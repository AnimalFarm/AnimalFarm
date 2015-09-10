using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(InventoryItemDatabase), true)]
    public class InventoryItemDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            if (GUILayout.Button("Open item editor"))
            {
                InventoryEditorUtil.selectedDatabase = (InventoryItemDatabase) target;
                ItemManagerEditor.ShowWindow();
            }
            if (GUILayout.Button("Open equipment editor"))
            {
                InventoryEditorUtil.selectedDatabase = (InventoryItemDatabase)target;
                EquipManagerEditor.ShowWindow();
            }
            if (GUILayout.Button("Open crafting editor"))
            {
                InventoryEditorUtil.selectedDatabase = (InventoryItemDatabase)target;
                CraftingManagerEditor.ShowWindow();
            }
        }
    }
}