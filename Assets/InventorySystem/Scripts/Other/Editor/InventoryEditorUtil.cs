using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Devdog.InventorySystem.Editors
{
    public static class InventoryEditorUtil
    {
        public class InventoryEditorCategoriesAndRaritiesLookup
        {
            public string[] rarities;
            public Color[] rarityColors;

            public string[] categories;
        }


        public static GUIStyle labelStyle { get; private set; }
        public static GUIStyle boxStyle { get; private set; }
        public static GUIStyle toolbarStyle { get; private set; }


        public static InventoryItemDatabase selectedDatabase { get; set; }
        public static InventoryLangDatabase selectedLangDatabase { get; set; }


        public static void Init()
        {
            Update();

            if (GetItemManager() != null)
                selectedDatabase = GetItemManager().itemDatabase;
        }

        public static void Update()
        {
            toolbarStyle = new GUIStyle(EditorStyles.toolbarButton);
            toolbarStyle.fixedHeight = 40;

            boxStyle = new GUIStyle();
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
            boxStyle.stretchWidth = true;

            labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.wordWrap = true;
        }


        public static InventoryItemDatabase GetItemDatabase()
        {
            var manager = GameObject.FindObjectOfType<ItemManager>();
            if (manager != null)
                return manager.itemDatabase;

            return null;
        }
        public static InventoryLangDatabase GetLangDatabase()
        {
            var manager = GameObject.FindObjectOfType<InventoryManager>();
            if (manager != null)
                return manager.lang;

            return null;
        }


        public static void ShowLangDatabasePicker()
        {
            EditorGUILayout.LabelField("Found the following databases in your project folder:", EditorStyles.largeLabel);

            var dbs = AssetDatabase.FindAssets("t:" + typeof(InventoryLangDatabase).Name);
            foreach (var db in dbs)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(AssetDatabase.GUIDToAssetPath(db), InventoryEditorUtil.labelStyle);
                if (GUILayout.Button("Select"))
                    selectedLangDatabase = (InventoryLangDatabase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(db), typeof(InventoryLangDatabase));

                EditorGUILayout.EndHorizontal();
            }

            if (dbs.Length == 0)
            {
                EditorGUILayout.LabelField("No Lang databases found, first create one in your assets folder.");
            }

            return;
        }
        public static void ShowItemDatabasePicker()
        {
            EditorGUILayout.LabelField("Found the following databases in your project folder:", EditorStyles.largeLabel);

            var dbs = AssetDatabase.FindAssets("t:" + typeof(InventoryItemDatabase).Name);
            foreach (var db in dbs)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(AssetDatabase.GUIDToAssetPath(db), InventoryEditorUtil.labelStyle);
                if (GUILayout.Button("Select"))
                {
                    selectedDatabase = (InventoryItemDatabase) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(db), typeof (InventoryItemDatabase));
                }

                EditorGUILayout.EndHorizontal();
            }

            if (dbs.Length == 0)
            {
                EditorGUILayout.LabelField("No Item databases found, first create one in your assets folder.");
            }
        }


        public static ItemManager GetItemManager()
        {
            return GameObject.FindObjectOfType<ItemManager>();
        }

        public static InventoryManager GetInventoryManager()
        {
            return GameObject.FindObjectOfType<InventoryManager>();
        }


        public static InventorySettingsManager GetSettingsManager()
        {
            // Already dropped error on GetItemManager()
            return GameObject.FindObjectOfType<InventorySettingsManager>();
        }

        public static InventoryEditorCategoriesAndRaritiesLookup GetCategoriesAndRaritys()
        {
            var a = new InventoryEditorCategoriesAndRaritiesLookup();
            var manager = GetItemManager();
            a.rarities = new string[manager.itemRaritys.Length];
            for (int i = 0; i < a.rarities.Length; i++)
                a.rarities[i] = manager.itemRaritys[i].name;

            a.rarityColors = new Color[manager.itemRaritys.Length];
            for (int i = 0; i < a.rarityColors.Length; i++)
                a.rarityColors[i] = manager.itemRaritys[i].color;

            a.categories = new string[manager.itemCategories.Length];
            for (int i = 0; i < a.categories.Length; i++)
                a.categories[i] = manager.itemCategories[i].name;

            return a;
        }


        public static void GetAllFieldsInherited(System.Type startType, List<FieldInfo> appendList)
        {
            if (startType == typeof(UnityEngine.MonoBehaviour) || startType == null)
                return;

            // Copied fields can be restricted with BindingFlags
            FieldInfo[] fields = startType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                appendList.Add(field);
            }

            // Keep going untill we hit UnityEngine.MonoBehaviour type.
            GetAllFieldsInherited(startType.BaseType, appendList);
        }


        #region UI helpers

        public static T SimpleObjectPicker<T>(string name, UnityEngine.Object o, bool sceneObjects, bool required) where T : UnityEngine.Object
        {
            if (o == null && required == true && GUI.enabled)
                GUI.color = Color.red;

            var item = (T)EditorGUILayout.ObjectField(name, o, typeof(T), sceneObjects);
            GUI.color = Color.white;

            return item;
        }
        public static void ErrorIfEmpty(System.Object o, string msg)
        {
            if (o == null)
            {
                EditorGUILayout.HelpBox(msg, MessageType.Error);
            }
        }

        public static void ErrorIfEmpty(UnityEngine.Object o, string msg)
        {
            if (o == null)
            {
                EditorGUILayout.HelpBox(msg, MessageType.Error);
            }
        }

        public static void ErrorIfEmpty(bool o, string msg)
        {
            if (o)
            {
                EditorGUILayout.HelpBox(msg, MessageType.Error);
            }
        }

        public static LayerMask LayerMaskField(string label, LayerMask selected, bool showSpecial)
        {

            List<string> layers = new List<string>();
            List<int> layerNumbers = new List<int>();

            string selectedLayers = "";

            for (int i = 0; i < 32; i++)
            {

                string layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {
                    if (selected == (selected | (1 << i)))
                    {

                        if (selectedLayers == "")
                        {
                            selectedLayers = layerName;
                        }
                        else
                        {
                            selectedLayers = "Mixed";
                        }
                    }
                }
            }

            //EventType lastEvent = Event.current.type;

            if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.ExecuteCommand)
            {
                if (selected.value == 0)
                {
                    layers.Add("Nothing");
                }
                else if (selected.value == -1)
                {
                    layers.Add("Everything");
                }
                else
                {
                    layers.Add(selectedLayers);
                }
                layerNumbers.Add(-1);
            }

            if (showSpecial)
            {
                layers.Add((selected.value == 0 ? "[X] " : "     ") + "Nothing");
                layerNumbers.Add(-2);

                layers.Add((selected.value == -1 ? "[X] " : "     ") + "Everything");
                layerNumbers.Add(-3);
            }

            for (int i = 0; i < 32; i++)
            {

                string layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {
                    if (selected == (selected | (1 << i)))
                    {
                        layers.Add("[X] " + layerName);
                    }
                    else
                    {
                        layers.Add("     " + layerName);
                    }
                    layerNumbers.Add(i);
                }
            }

            bool preChange = GUI.changed;

            GUI.changed = false;

            int newSelected = 0;

            if (Event.current.type == EventType.MouseDown)
            {
                newSelected = -1;
            }

            newSelected = EditorGUILayout.Popup(label, newSelected, layers.ToArray(), EditorStyles.layerMaskField);

            if (GUI.changed && newSelected >= 0)
            {
                //newSelected -= 1;
                if (showSpecial && newSelected == 0)
                    selected = 0;
                else if (showSpecial && newSelected == 1)
                    selected = -1;
                else
                {

                    if (selected == (selected | (1 << layerNumbers[newSelected])))
                        selected &= ~(1 << layerNumbers[newSelected]);
                    else
                        selected = selected | (1 << layerNumbers[newSelected]);
                }
            }
            else
                GUI.changed = preChange;

            return selected;
        }


        #endregion

    }
}