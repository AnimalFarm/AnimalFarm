using UnityEngine;
using UnityEditor;
using System.Collections;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(InventoryEquippableField))]
    public class InventoryEquippableFieldEditor : Editor
    {

        //private SerializedProperty id;
        private SerializedProperty equipTypes;
        private ItemManager manager;

        private string[] equipTypesStrings = new string[0];

        public void OnEnable()
        {
            //item = (InventoryItemBase)target;

            //id = serializedObject.FindProperty("ID");
            equipTypes = serializedObject.FindProperty("_equipTypes");


            manager = Editor.FindObjectOfType<ItemManager>();
            if (manager == null)
                Debug.LogError("No item manager found in scene, cannot edit item.");

            UpdateEquipTypes();
        }

        private void UpdateEquipTypes()
        {
            equipTypesStrings = new string[manager.equipTypes.Length];
            for (int i = 0; i < manager.equipTypes.Length; i++)
            {
                equipTypesStrings[i] = manager.equipTypes[i].name;
            }
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UpdateEquipTypes();


            EditorGUILayout.BeginVertical("box");

            DrawPropertiesExcluding(serializedObject, new string[]
            {
                "ID",
                "_equipTypes"
            });
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Which types can be placed in this field?");
            EditorGUILayout.LabelField("Edit types at Tools/InventorySystem/Equip manager");


            for (int i = 0; i < equipTypes.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                var e = equipTypes.GetArrayElementAtIndex(i);
                e.intValue = EditorGUILayout.Popup(e.intValue, equipTypesStrings);
                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    equipTypes.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            if(GUILayout.Button("Add"))
            {
                equipTypes.InsertArrayElementAtIndex(equipTypes.arraySize == 0 ? 0 : equipTypes.arraySize - 1);
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

    }
}