using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    public class EquipManagerEditor : EditorWindow
    {
        private Vector2 itemsScrollPosition = new Vector2();
        private InventorySettingsManager settings;

        private int toolbarIndex = 0;
        private int selectedStatIndexType = -1;

        private InventoryEquipType selectedEquipType;
        private string[] equipTypesStrings;

        [MenuItem("Tools/InventorySystem/Equip manager")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<EquipManagerEditor>("Equip manager", true);
        }

        public void OnEnable()
        {
            InventoryEditorUtil.Init();

            settings = InventoryEditorUtil.GetSettingsManager();

            UpdateEquipTypes();
        }

        private void UpdateEquipTypes()
        {
            equipTypesStrings = new string[InventoryEditorUtil.selectedDatabase.equipTypes.Length];
            for (int i = 0; i < InventoryEditorUtil.selectedDatabase.equipTypes.Length; i++)
            {
                equipTypesStrings[i] = InventoryEditorUtil.selectedDatabase.equipTypes[i].name;
            }
        }

        public void OnGUI()
        {
            InventoryEditorUtil.Update();


            EditorGUILayout.BeginHorizontal();

            GUI.color = Color.grey;
            if (GUILayout.Button("< DB", InventoryEditorUtil.toolbarStyle, GUILayout.Width(60)))
            {
                InventoryEditorUtil.selectedDatabase = null;
            }
            GUI.color = Color.white;

            toolbarIndex = GUILayout.Toolbar(toolbarIndex, new string[] { "Character stats", "Equip types" }, InventoryEditorUtil.toolbarStyle);
            EditorGUILayout.EndHorizontal();

            if (InventoryEditorUtil.selectedDatabase == null)
            {
                InventoryEditorUtil.ShowItemDatabasePicker();
                return;
            }

            switch (toolbarIndex)
            {
                default:
                case 0:
                    ShowItemEditor();
                    break;
                case 1:
                    ShowEquipTypesEditor();
                    break;
            }

            if (GUI.changed)
                EditorUtility.SetDirty(InventoryEditorUtil.selectedDatabase); // To make sure it gets saved.
        }

        private void ShowItemEditor()
        {

            InventoryEditorUtil.ErrorIfEmpty(EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty, "Inventory item prefab folder is not set, items cannot be saved! Please go to settings and define the Inventory item prefab folder.");
            if (EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty)
                return;

        
            EditorGUILayout.BeginVertical("box");




            #region Types picker

            EditorGUILayout.BeginVertical();

            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Step 1: Pick the item types that you want to scan for character stats.", InventoryEditorUtil.labelStyle);
            GUI.color = Color.white;
            EditorGUILayout.LabelField("Note: You only have to pick the top level classes.", InventoryEditorUtil.labelStyle);
            EditorGUILayout.LabelField("If EquippableInventoryItem extends from InventoryItemBase, you don't need to pick base. The system handles inheritance.", InventoryEditorUtil.labelStyle);

            // Object picker response
            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                var selected = EditorGUIUtility.GetObjectPickerObject();
                if (selected != null)
                {
                    InventoryEditorUtil.selectedDatabase.equipStatTypes[selectedStatIndexType] = ((MonoScript)selected).GetClass().AssemblyQualifiedName;
                    Repaint();
                }
            }

            for (int i = 0; i < InventoryEditorUtil.selectedDatabase.equipStatTypes.Length; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.Space();

                //var currentItem = onlyAllowTypes.GetArrayElementAtIndex(i);
                EditorGUILayout.LabelField(InventoryEditorUtil.selectedDatabase.equipStatTypes[i] == null ? "- NOT SET" : InventoryEditorUtil.selectedDatabase.equipStatTypes[i], InventoryEditorUtil.labelStyle);
                GUI.color = Color.green;
                if (GUILayout.Button("Set", GUILayout.Width(80)))
                {
                    EditorGUIUtility.ShowObjectPicker<MonoScript>(null, false, "l:InventoryItemType", 9);
                    selectedStatIndexType = i;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    var l = new List<string>(InventoryEditorUtil.selectedDatabase.equipStatTypes);
                    l.RemoveAt(i);
                    InventoryEditorUtil.selectedDatabase.equipStatTypes = l.ToArray();
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.green;
            if (GUILayout.Button("Add"))
            {
                var l = new List<string>(InventoryEditorUtil.selectedDatabase.equipStatTypes);
                l.Add(null);
                InventoryEditorUtil.selectedDatabase.equipStatTypes = l.ToArray();

                //onlyAllowTypes.InsertArrayElementAtIndex(onlyAllowTypes.arraySize > 0 ? onlyAllowTypes.arraySize - 1 : 0);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        
            #endregion

            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Step 2: Scan the types for stats.", InventoryEditorUtil.labelStyle);
            GUI.color = Color.white;

            if (GUILayout.Button("Scan types"))
            {
                var oldList = new List<InventoryEquipStat>(InventoryEditorUtil.selectedDatabase.equipStats);
                var displayList = new List<InventoryEquipStat>(64);
                foreach (var type in InventoryEditorUtil.selectedDatabase.equipStatTypes)
                {
                    var fields = new List<FieldInfo>();
                    InventoryEditorUtil.GetAllFieldsInherited(System.Type.GetType(type, true), fields);
                    foreach (var field in fields)
                    {
                        var attr = field.GetCustomAttributes(typeof(InventoryStatAttribute), true);
                        if (attr.Length > 0)
                        {
                            var m = (InventoryStatAttribute)attr[0];

                            var old = oldList.FindAll(o => o.fieldInfoNameVisual == field.ReflectedType.Name + "." + field.Name);
                            if(old.Count == 0)
                            {
                                displayList.Add(new InventoryEquipStat() { name = m.name, typeName = type, fieldInfoName = field.Name, fieldInfoNameVisual = field.ReflectedType.Name + "." + field.Name, show = false, category = "Default", formatter = settings != null ? settings.defaultCharacterStatFormatter : null });
                            }
                            else
                            {
                                // Item exists more than once.
                                var already = displayList.Find(o => o.fieldInfoNameVisual == field.ReflectedType.Name + "." + field.Name);
                                if(already == null)
                                {
                                    displayList.Add(old[0]);
                                }
                            }
                        }
                    }
                }

                InventoryEditorUtil.selectedDatabase.equipStats = displayList.ToArray();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Step 3: Choose what you want to display.", InventoryEditorUtil.labelStyle);
            GUI.color = Color.white;

            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField("Show", GUILayout.Width(30));
            EditorGUILayout.LabelField("Code path");
            EditorGUILayout.LabelField("Field name");
            EditorGUILayout.LabelField("Category");
            EditorGUILayout.LabelField("Show type");

            EditorGUILayout.LabelField("Move");

            EditorGUILayout.EndHorizontal();

            int x = 0;
            foreach (var item in InventoryEditorUtil.selectedDatabase.equipStats)
            {
                EditorGUILayout.BeginHorizontal("box");

                if (item.show == false)
                    GUI.enabled = false;

                GUI.enabled = true;
                item.show = EditorGUILayout.Toggle(item.show, GUILayout.Width(30));
                if (item.show == false)
                    GUI.enabled = false;

                EditorGUILayout.LabelField(item.fieldInfoNameVisual);
                item.name = EditorGUILayout.TextField(item.name);
                item.category = EditorGUILayout.TextField(item.category);

                item.formatter = InventoryEditorUtil.SimpleObjectPicker<CharacterStatFormatterBase>("", item.formatter, false, false);

                if (x == 0)
                    GUI.enabled = false;

                if (GUILayout.Button("Up"))
                {
                    var l = new List<InventoryEquipStat>(InventoryEditorUtil.selectedDatabase.equipStats);
                    var temp = l[x - 1];
                    l[x - 1] = l[x];
                    l[x] = temp;

                    InventoryEditorUtil.selectedDatabase.equipStats = l.ToArray();
                }

                if(item.show)
                    GUI.enabled = true;

                if (x == InventoryEditorUtil.selectedDatabase.equipStats.Length - 1)
                    GUI.enabled = false;
            
                if (GUILayout.Button("Down"))
                {
                    var l = new List<InventoryEquipStat>(InventoryEditorUtil.selectedDatabase.equipStats);
                    var temp = l[x + 1];
                    l[x + 1] = l[x];
                    l[x] = temp;

                    InventoryEditorUtil.selectedDatabase.equipStats = l.ToArray();
                }

                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();

                x++;
            }


            EditorGUILayout.EndVertical();
        }

        private void ShowEquipTypesEditor()
        {
            EditorGUILayout.BeginHorizontal();

            InventoryEditorUtil.ErrorIfEmpty(EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty, "Inventory item prefab folder is not set, items cannot be saved! Please go to settings and define the Inventory item prefab folder.");
            if (EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty)
                return;


            itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField("Search...");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Create equip type"))
            {
                // Add one
                var l = new List<InventoryEquipType>(InventoryEditorUtil.selectedDatabase.equipTypes);
                l.Add(new InventoryEquipType());
                InventoryEditorUtil.selectedDatabase.equipTypes = l.ToArray();

                for (int i = 0; i < InventoryEditorUtil.selectedDatabase.equipTypes.Length; i++)
                {
                    InventoryEditorUtil.selectedDatabase.equipTypes[i].ID = i;
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InventoryEditorUtil.selectedDatabase.equipTypes.Length + " Equip types");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int x = 0;
            foreach (var item in InventoryEditorUtil.selectedDatabase.equipTypes)
            {
                if (item == null)
                {
                    continue;
                }
                if (item == selectedEquipType)
                    GUI.color = Color.green;

                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + item.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(item.name);

                //if (item.icon != null)
                //GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetRect(40, 40), item.icon.texture, item.icon.textureRect);

                GUI.color = Color.green;
                if (GUILayout.Button("Edit", GUILayout.Width(60)))
                {
                    selectedEquipType = item;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete item " + item.name + "\"?", "Yes", "NO!"))
                    {
                        selectedEquipType = null;
                        var l = new List<InventoryEquipType>(InventoryEditorUtil.selectedDatabase.equipTypes);
                        l.RemoveAt(x);
                        InventoryEditorUtil.selectedDatabase.equipTypes = l.ToArray();

                        for (int i = 0; i < InventoryEditorUtil.selectedDatabase.equipTypes.Length; i++)
                        {
                            InventoryEditorUtil.selectedDatabase.equipTypes[i].ID = i;
                        }
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
                x++;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

            if (selectedEquipType != null)
            {
                // Show property editor

                EditorGUILayout.BeginVertical(GUILayout.Width(500));

                EditorGUILayout.LabelField("#" + selectedEquipType.ID);
                EditorGUILayout.Space();

                selectedEquipType.name = EditorGUILayout.TextField("Name", selectedEquipType.name);
                EditorGUILayout.Space();


                EditorGUILayout.LabelField("You can force other fields to be empty when you set this. For example when equipping a greatsword, you might want to un-equip the shield.", InventoryEditorUtil.labelStyle);
                if (selectedEquipType.blockTypes != null)
                {
                    for (int i = 0; i < selectedEquipType.blockTypes.Length; i++)
                    {
                        GUILayout.BeginHorizontal("box");

                        int type = selectedEquipType.blockTypes[i];

                        if (type == selectedEquipType.ID)
                            GUI.color = Color.red;

                        int selected = 0;
                        if (type != -1)
                            selected = type;

                        int index = EditorGUILayout.Popup(selected, equipTypesStrings);
                        selectedEquipType.blockTypes[i] = InventoryEditorUtil.selectedDatabase.equipTypes[index].ID;

                        GUI.color = Color.red;
                        if(GUILayout.Button("X", GUILayout.Width(40)))
                        {
                            var l = new List<int>(selectedEquipType.blockTypes);
                            l.RemoveAt(i);
                            i--; // To keep the array working :)
                            selectedEquipType.blockTypes = l.ToArray();
                        }
                        GUI.color = Color.white;

                        GUILayout.EndHorizontal();
                    }

                    UpdateEquipTypes();
                }
                if(GUILayout.Button("Add restriction"))
                {
                    if(selectedEquipType.blockTypes == null)
                        selectedEquipType.blockTypes = new int[0];

                    var l = new List<int>(selectedEquipType.blockTypes);
                    l.Add(-1);
                    selectedEquipType.blockTypes = l.ToArray();

                    UpdateEquipTypes();
                }

                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

    }
}