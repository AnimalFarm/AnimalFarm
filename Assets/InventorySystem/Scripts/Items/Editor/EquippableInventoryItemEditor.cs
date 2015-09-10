using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(EquippableInventoryItem), true)]
    [CanEditMultipleObjects()]
    public class EquippableInventoryItemEditor : InventoryItemBaseEditor
    {
        protected SerializedProperty equipType;


        protected static string[] equipTypesStrings = new string[0];


        public override void OnEnable()
        {
            base.OnEnable();
            equipType = serializedObject.FindProperty("_equipType");

            UpdateEquipTypes();
        }

        public void UpdateEquipTypes()
        {
            equipTypesStrings = new string[itemManager.equipTypes.Length];
            for (int i = 0; i < itemManager.equipTypes.Length; i++)
            {
                equipTypesStrings[i] = itemManager.equipTypes[i].name;
            }
        }


        protected override void OnCustomInspectorGUI(List<string> doNotDraw)
        {
            serializedObject.Update();
            UpdateEquipTypes();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("What kind of item is this?");
            EditorGUILayout.LabelField("Edit types at Tools/InventorySystem/Equip manager");

            equipType.intValue = EditorGUILayout.Popup(equipType.intValue, equipTypesStrings);
        
            EditorGUILayout.EndVertical();


            serializedObject.ApplyModifiedProperties();

            doNotDraw.Add("_equipType");
            base.OnCustomInspectorGUI(doNotDraw);
        }
    }
}