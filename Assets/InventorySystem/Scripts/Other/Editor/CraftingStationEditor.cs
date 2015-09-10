using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(CraftingStation), true)]
    public class CraftingStationEditor : Editor
    {
        //private CraftingStation item;
        private SerializedObject serializer;

        private SerializedProperty craftingCategoryID;
        private static ItemManager itemManager;
    

        public virtual void OnEnable()
        {
            //item = (CraftingStation)target;
            serializer = serializedObject;

            craftingCategoryID = serializer.FindProperty("categoryID");

            itemManager = Editor.FindObjectOfType<ItemManager>();
            if (itemManager == null)
                Debug.LogError("No item manager found in scene, cannot edit item.");
        }


        public override void OnInspectorGUI()
        {
            if (itemManager == null)
                return;


            serializedObject.Update();

            var l = new List<string>(itemManager.craftingCategories.Length);
            foreach (var item in itemManager.craftingCategories)
            {
                l.Add(item.name);
            }



            // Draws remaining items
            EditorGUILayout.BeginVertical("box");
            DrawPropertiesExcluding(serializer, new string[]
            {
                "categoryID",
            });
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            craftingCategoryID.intValue = EditorGUILayout.Popup("Crafting category", craftingCategoryID.intValue, l.ToArray());

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}