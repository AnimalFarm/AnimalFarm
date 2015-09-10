using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(ItemCollectionBase), true)]
    [CanEditMultipleObjects()]
    public class ItemCollectionBaseEditor : Editor
    {
        private ItemCollectionBase item;
        private SerializedObject serializer;

        private SerializedProperty items;
        private SerializedProperty useReferences;
        private SerializedProperty canDropFromCollection;
        private SerializedProperty canUseFromCollection;
        private SerializedProperty canDragInCollection;
        private SerializedProperty canPutItemsInCollection;
        private SerializedProperty canStackItemsInCollection;
        private SerializedProperty manuallyDefineCollection;
        private SerializedProperty container;
        private SerializedProperty onlyAllowTypes;

        private static ItemManager itemManager;

        // Script selector
        private int lastObjectPickerIndex;
        private bool showAllowItemsOfType;
        private bool showManuallySetItems;


        public virtual void OnEnable()
        {
            item = (ItemCollectionBase)target;
            //serializer = new SerializedObject(target);
            serializer = serializedObject;

            items = serializer.FindProperty("_items");
            useReferences = serializer.FindProperty("useReferences");
            canDropFromCollection = serializer.FindProperty("canDropFromCollection");
            canUseFromCollection = serializer.FindProperty("canUseFromCollection");
            canDragInCollection = serializer.FindProperty("canDragInCollection");
            canPutItemsInCollection = serializer.FindProperty("canPutItemsInCollection");
            canStackItemsInCollection = serializer.FindProperty("canStackItemsInCollection");
            manuallyDefineCollection = serializer.FindProperty("manuallyDefineCollection");
            container = serializer.FindProperty("container");
            onlyAllowTypes = serializer.FindProperty("_onlyAllowTypes");

            itemManager = Editor.FindObjectOfType<ItemManager>();
            if (itemManager == null)
                Debug.LogError("No item manager found in scene, cannot edit item.");
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();



            // Draws remaining items
            EditorGUILayout.BeginVertical("box");
            DrawPropertiesExcluding(serializer, new string[]
            {
                "_items",
                "useReferences",
                "canDropFromCollection",
                "canUseFromCollection",
                "canDragInCollection",
                "canPutItemsInCollection",
                "canStackItemsInCollection",
                "manuallyDefineCollection",
                "container",
                "onlyAllowTypes"
            });
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(useReferences);
            EditorGUILayout.PropertyField(container);
            //EditorGUILayout.PropertyField(onlyAllowTypes);


            #region Manually define collection

            EditorGUILayout.PropertyField(manuallyDefineCollection);
            if (manuallyDefineCollection.boolValue)
            {
                //EditorGUILayout.PropertyField();

                showManuallySetItems = EditorGUILayout.Foldout(showManuallySetItems, "Manually define items", EditorStyles.foldout);
                if (showManuallySetItems)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical();

                    for (int i = 0; i < items.arraySize; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        items.GetArrayElementAtIndex(i).objectReferenceValue = EditorGUILayout.ObjectField("Item " + i, items.GetArrayElementAtIndex(i).objectReferenceValue, typeof(InventoryUIItemWrapperBase), true);

                        GUI.color = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(30)))
                        {
                            items.DeleteArrayElementAtIndex(i);
                        }
                        GUI.color = Color.white;

                        EditorGUILayout.EndHorizontal();
                    }

                    GUI.color = Color.green;
                    if (GUILayout.Button("Add item"))
                    {
                        if (items.arraySize == 0)
                            items.InsertArrayElementAtIndex(0);
                        else
                            items.InsertArrayElementAtIndex(items.arraySize - 1);
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();


                }
            }

            #endregion


            #region Only allow items of type

            showAllowItemsOfType = EditorGUILayout.Foldout(showAllowItemsOfType, "Only allow items of type", EditorStyles.foldout);
            if (showAllowItemsOfType)
            {
                EditorGUILayout.BeginVertical();
                // Object picker response
                if (Event.current.commandName == "ObjectSelectorClosed")
                {
                    var selected = EditorGUIUtility.GetObjectPickerObject();
                    if(selected != null)
                        item._onlyAllowTypes[lastObjectPickerIndex] = ((MonoScript)selected).GetClass().AssemblyQualifiedName;
                }

                for (int i = 0; i < onlyAllowTypes.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.Space();
                
                    //var currentItem = onlyAllowTypes.GetArrayElementAtIndex(i);
                    EditorGUILayout.LabelField(item._onlyAllowTypes[i]);
                    GUI.color = Color.green;
                    if (GUILayout.Button("Set", GUILayout.Width(80)))
                    {
                        EditorGUIUtility.ShowObjectPicker<MonoScript>(null, false, "l:InventoryItemType", 9);
                        lastObjectPickerIndex = i;
                    }

                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(30)))
                    {
                        onlyAllowTypes.DeleteArrayElementAtIndex(i);
                    }
                    GUI.color = Color.white;

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                GUI.color = Color.green;
                if (GUILayout.Button("Add"))
                {
                    onlyAllowTypes.InsertArrayElementAtIndex(onlyAllowTypes.arraySize > 0 ? onlyAllowTypes.arraySize - 1 : 0);
                }
                GUI.color = Color.white;
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
            }

            #endregion



            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(canDropFromCollection);
            EditorGUILayout.PropertyField(canUseFromCollection);
            EditorGUILayout.PropertyField(canDragInCollection);
            EditorGUILayout.PropertyField(canPutItemsInCollection);
            EditorGUILayout.PropertyField(canStackItemsInCollection);        
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}