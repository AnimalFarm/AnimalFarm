using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Devdog.InventorySystem.Dialogs;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    public class ItemManagerEditor : EditorWindow
    {
        private InventoryItemBase selectedItem;
        private InventoryItemProperty selectedProperty;
        private InventoryItemCategory selectedCategory;
        private InventoryItemRarity selectedRarity;

        private Vector2 itemsScrollPosition = new Vector2();
        private Vector2 propertiesScrollPosition = new Vector2();

        private int toolbarIndex = 0;
        //private int lastObjectPickerIndex;
        //private bool showAllowItemsOfType = true;


        //private static Color[] colors;
        //private static string[] categories;
        private bool doneCreatingItem = true;
        private Editor itemEditorInspector;

        [MenuItem("Tools/InventorySystem/Item manager")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ItemManagerEditor>("Item manager", true);
        }

        public void OnEnable()
        {
            InventoryEditorUtil.Init();
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
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, new string[] { "Item editor", "Item property editor", "Category editor", "Rarity editor" }, InventoryEditorUtil.toolbarStyle);
            EditorGUILayout.EndHorizontal();


            if (InventoryEditorUtil.selectedDatabase == null)
            {
                InventoryEditorUtil.ShowItemDatabasePicker();
                return;
            }


            EditorGUILayout.BeginHorizontal(InventoryEditorUtil.boxStyle);
            EditorGUILayout.Space();

            switch (toolbarIndex)
            {
                default:
                case 0:
                    ShowItemEditor();
                    break;
                case 1:
                    ShowItemPropertyEditor();
                    break;
                case 2:
                    ShowCategoryEditor();
                    break;
                case 3:
                    ShowRarityEditor();
                    break;
            }

            EditorGUILayout.EndHorizontal();


            if (GUI.changed)
                EditorUtility.SetDirty(InventoryEditorUtil.selectedDatabase); // To make sure it gets saved.
        }


        //private void UpdateItemIDs()
        //{
        //    var l = new List<InventoryItemBase>(InventoryEditorUtil.selectedDatabase.items);
        //    l.RemoveAll(o => o == null);
        //    InventoryEditorUtil.selectedDatabase.items = l.ToArray();

        //    //for (uint i = 0; i < InventoryEditorUtil.selectedDatabase.items.Length; i++)
        //    //{
        //    //    InventoryEditorUtil.selectedDatabase.items[i].ID = i;
        //    //    EditorUtility.SetDirty(InventoryEditorUtil.selectedDatabase.items[i]);
        //    //}
        //    AssetDatabase.SaveAssets();
        //}

        private void ShowItemEditor()
        {
            InventoryEditorUtil.ErrorIfEmpty(EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty, "Inventory item prefab folder is not set, items cannot be saved! Please go to settings and define the Inventory item prefab folder.");
            if (EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty)
                return;


            // When creating a new item
            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                var selected = EditorGUIUtility.GetObjectPickerObject();
                if (selected != null && doneCreatingItem == false)
                {
                    // Add the item
                    Debug.Log("Lets create that object");
                
                    string prefabPath = EditorPrefs.GetString("InventorySystem_ItemPrefabPath") + "/item_" + System.DateTime.Now.ToFileTimeUtc() + "_PFB.prefab";
                    var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    var prefab = PrefabUtility.CreatePrefab(prefabPath, obj);
                    AssetDatabase.SetLabels(prefab, new string[] { "InventoryItemPrefab" });
                    var script = (MonoScript)selected;
                    System.Type scriptType = script.GetClass();
                    var items = new List<InventoryItemBase>(InventoryEditorUtil.selectedDatabase.items);
                    var comp = (InventoryItemBase)prefab.AddComponent(scriptType);
                    comp.ID = items.Count == 0 ? 0 : items[items.Count - 1].ID + 1;

                    items.Add(comp);
                    InventoryEditorUtil.selectedDatabase.items = items.ToArray();
                    Object.DestroyImmediate(obj);
                    
                    Debug.Log("Item created at " + prefabPath);
                    doneCreatingItem = true;

                    //UpdateItemIDs();
                    Repaint();
                }
                //else if(selected == null)
                //EditorGUIUtility.ShowObjectPicker<MonoScript>(null, false, "l:InventoryItemType", 0); // Pick something god damn it..                
            }


            itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField("Search...");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Create item"))
            {
                doneCreatingItem = false;
                EditorGUIUtility.ShowObjectPicker<MonoScript>(null, false, "l:InventoryItemType", 0);          
            }
            //if (GUILayout.Button("Force update ID's"))
            //{
            //    UpdateItemIDs();
            //}
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InventoryEditorUtil.selectedDatabase.items.Length + " Items");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int x = 0;
            foreach (var item in InventoryEditorUtil.selectedDatabase.items)
            {
                if (item == null)
                    continue;            

                if (item == selectedItem)
                    GUI.color = Color.green;
          
                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + item.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(item.name);
                EditorGUILayout.LabelField(item.GetType().Name.ToString().Replace("InventoryItem", ""), GUILayout.Width(100));

                //if (item.icon != null)
                //GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetRect(40, 40), item.icon.texture, item.icon.textureRect);

                GUI.color = Color.green;
                if (GUILayout.Button("Edit", GUILayout.Width(60)))
                {
                    selectedItem = item;

                    itemEditorInspector = Editor.CreateEditor(selectedItem);

                    AssetDatabase.OpenAsset(selectedItem);
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete item #" + item.ID + " with the name \"" + item.name + "\"?", "Yes", "NO!"))
                    {
                        selectedItem = null;

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(InventoryEditorUtil.selectedDatabase.items[x]));

                        InventoryEditorUtil.selectedDatabase.items[x] = null;
                        var l = new List<InventoryItemBase>(InventoryEditorUtil.selectedDatabase.items);
                        l.RemoveAll(o => o == null);
                        InventoryEditorUtil.selectedDatabase.items = l.ToArray();

                        //UpdateItemIDs();
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
                x++;
            }

            EditorGUILayout.EndScrollView();
        
            EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

            if (selectedItem != null)
            {
                GUILayout.Label("Use the inspector if you want to add custom components.");

                itemEditorInspector.OnInspectorGUI();

                string newName = "Item_" + selectedItem.name == null || selectedItem.name == string.Empty ? string.Empty : selectedItem.name.ToLower().Replace(" ", "_") + "_#" + selectedItem.ID + "_" + InventoryEditorUtil.selectedDatabase.name + "_PFB";
                if (AssetDatabase.GetAssetPath(selectedItem).EndsWith(newName + ".prefab") == false)
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(selectedItem), newName);                    
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void UpdatePropertyIDs()
        {
            var l = new List<InventoryItemProperty>(InventoryEditorUtil.selectedDatabase.properties);
            l.RemoveAll(o => o == null);
            InventoryEditorUtil.selectedDatabase.properties = l.ToArray();

            for (int i = 0; i < InventoryEditorUtil.selectedDatabase.properties.Length; i++)
                InventoryEditorUtil.selectedDatabase.properties[i].ID = i;
        }

        private void ShowItemPropertyEditor()
        {
            propertiesScrollPosition = EditorGUILayout.BeginScrollView(propertiesScrollPosition);

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField("Search...");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Create property"))
            {
                var l = new List<InventoryItemProperty>(InventoryEditorUtil.selectedDatabase.properties);
                l.Add(new InventoryItemProperty());
                InventoryEditorUtil.selectedDatabase.properties = l.ToArray();

                UpdatePropertyIDs();
            }

            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InventoryEditorUtil.selectedDatabase.properties.Length + " Properties");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int x = 0;
            foreach (var property in InventoryEditorUtil.selectedDatabase.properties)
            {
                if (property == selectedProperty)
                    GUI.color = Color.green;

                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + property.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(property.key);

                //if (item.icon != null)
                //GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetRect(40, 40), item.icon.texture, item.icon.textureRect);

                GUI.color = Color.green;
                if (GUILayout.Button("Edit", GUILayout.Width(60)))
                {
                    selectedProperty = property;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete property #" + property.ID + " with the key \"" + property.key + "\"?", "Yes", "NO!"))
                    {
                        InventoryEditorUtil.selectedDatabase.properties[x] = null;
                        UpdatePropertyIDs();
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
                x++;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

            if (selectedProperty != null)
            {
                // Show property editor

                EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

                EditorGUILayout.LabelField("ID", selectedProperty.ID.ToString());
                EditorGUILayout.Space();

                selectedProperty.key = EditorGUILayout.TextField("Key", selectedProperty.key);
                EditorGUILayout.Space();

                selectedProperty.uiColor = EditorGUILayout.ColorField("UI Color", selectedProperty.uiColor);
                EditorGUILayout.Space();

                selectedProperty.showInUI = EditorGUILayout.Toggle("Show in UI", selectedProperty.showInUI);
                EditorGUILayout.Space();

                //selectedProperty.value = EditorGUILayout.TextField("Default value", selectedProperty.value);


                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        private void UpdateCategoryIDs()
        {
            var l = new List<InventoryItemCategory>(InventoryEditorUtil.selectedDatabase.itemCategories);
            l.RemoveAll(o => o == null);
            InventoryEditorUtil.selectedDatabase.itemCategories = l.ToArray();

            for (uint i = 0; i < InventoryEditorUtil.selectedDatabase.itemCategories.Length; i++)
                InventoryEditorUtil.selectedDatabase.itemCategories[i].ID = i;
        }

        private void ShowCategoryEditor()
        {
            itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField("Search...");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Add new category"))
            {
                Debug.Log("Adding a category");
                var items = new List<InventoryItemCategory>(InventoryEditorUtil.selectedDatabase.itemCategories);
                var cat = new InventoryItemCategory();
                cat.ID = items[items.Count - 1].ID + 1;
                items.Add(cat);
                InventoryEditorUtil.selectedDatabase.itemCategories = items.ToArray();

                UpdateCategoryIDs();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InventoryEditorUtil.selectedDatabase.itemCategories.Length + " Categories");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int i = 0;
            foreach (var category in InventoryEditorUtil.selectedDatabase.itemCategories)
            {
                if (category == null)
                    continue;


                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + category.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(category.name);

                // First item cannot be removed it's always "none"

                if(i != 0)
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button("Edit"))
                    {
                        selectedCategory = category;
                    }
                    GUI.color = Color.red;
                    if (GUILayout.Button("Delete"))
                    {
                        if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete category #" + category.ID + " with the name \"" + category.name + "\"?\n\nAll items with this given category will have their category removed and reset to none.", "Yes", "NO!"))
                        {
                            // Find all items with this category and s
                            selectedCategory = null;

                            InventoryEditorUtil.selectedDatabase.itemCategories[i] = null;
                            UpdateCategoryIDs();
                        }
                    }
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = Color.gray;
                    GUILayout.Button("Edit");
                    GUILayout.Button("Delete");
                    GUI.color = Color.white;
                }
            

                EditorGUILayout.EndHorizontal();
                i++;
            }

            EditorGUILayout.EndScrollView();

            if(selectedCategory != null)
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

                EditorGUILayout.LabelField("ID", selectedCategory.ID.ToString());
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("The name of the category, is displayed in the tooltip in UI elements.", InventoryEditorUtil.labelStyle);
                selectedCategory.name = EditorGUILayout.TextField("Category name", selectedCategory.name);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


                EditorGUILayout.LabelField("Items can have a 'global' cooldown. Whenever an item of this category is used, all items with the same category will go into cooldown.", InventoryEditorUtil.labelStyle);
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Note, that items can individually override the timeout.", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;
                selectedCategory.cooldownTime = EditorGUILayout.Slider("Cooldown time (seconds)", selectedCategory.cooldownTime, 0.0f, 999.0f);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateRarityIDs()
        {
            var l = new List<InventoryItemRarity>(InventoryEditorUtil.selectedDatabase.itemRaritys);
            l.RemoveAll(o => o == null);
            InventoryEditorUtil.selectedDatabase.itemRaritys = l.ToArray();

            for (uint i = 0; i < InventoryEditorUtil.selectedDatabase.itemRaritys.Length; i++)
                InventoryEditorUtil.selectedDatabase.itemRaritys[i].ID = i;
        }

        private void ShowRarityEditor()
        {
            itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField("Search...");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Add new rarity"))
            {
                Debug.Log("Adding a rarity");
                var raritys = new List<InventoryItemRarity>(InventoryEditorUtil.selectedDatabase.itemRaritys);
                var rarity = new InventoryItemRarity();
                rarity.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                rarity.ID = raritys.Count == 0 ? 0 : raritys[raritys.Count - 1].ID + 1;
                raritys.Add(rarity);

                InventoryEditorUtil.selectedDatabase.itemRaritys = raritys.ToArray();
                UpdateRarityIDs();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InventoryEditorUtil.selectedDatabase.itemRaritys.Length + " Raritys");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int i = 0;
            foreach (var rarity in InventoryEditorUtil.selectedDatabase.itemRaritys)
            {
                if (rarity == null)
                    continue;


                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + rarity.ID, GUILayout.Width(40));

                GUI.color = rarity.color;
                EditorGUILayout.LabelField(rarity.name);
                GUI.color = Color.white;

                if (i == 0)
                    GUI.enabled = false;

                if (GUILayout.Button("Up", GUILayout.Width(50)))
                {
                    var temp = InventoryEditorUtil.selectedDatabase.itemRaritys[i - 1];
                    InventoryEditorUtil.selectedDatabase.itemRaritys[i - 1] = InventoryEditorUtil.selectedDatabase.itemRaritys[i];
                    InventoryEditorUtil.selectedDatabase.itemRaritys[i] = temp;
                }
                GUI.enabled = true;

                if (i == InventoryEditorUtil.selectedDatabase.itemRaritys.Length - 1)
                    GUI.enabled = false;

                if (GUILayout.Button("Down", GUILayout.Width(50)))
                {
                    var temp = InventoryEditorUtil.selectedDatabase.itemRaritys[i + 1];
                    InventoryEditorUtil.selectedDatabase.itemRaritys[i + 1] = InventoryEditorUtil.selectedDatabase.itemRaritys[i];
                    InventoryEditorUtil.selectedDatabase.itemRaritys[i] = temp;
                }

                GUI.enabled = true;
                GUI.color = Color.green;
                if (GUILayout.Button("Edit"))
                {
                    selectedRarity = rarity;
                }
                GUI.color = Color.red;
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete rarity #" + rarity.ID + " with the name \"" + rarity.name + "\"?", "Yes", "NO!"))
                    {
                        selectedRarity = null;

                        InventoryEditorUtil.selectedDatabase.itemRaritys[i] = null;
                        UpdateRarityIDs();
                    }
                }
                GUI.color = Color.white;
     

                EditorGUILayout.EndHorizontal();
                i++;
            }

            EditorGUILayout.EndScrollView();

            if (selectedRarity != null)
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

                EditorGUILayout.LabelField("ID", selectedRarity.ID.ToString());
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("The name of the rarity, is displayed in the tooltip in UI elements.", InventoryEditorUtil.labelStyle);
                selectedRarity.name = EditorGUILayout.TextField("Rarity name", selectedRarity.name);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("The color displayed in the UI.", InventoryEditorUtil.labelStyle);
                selectedRarity.color = EditorGUILayout.ColorField("Rarity color", selectedRarity.color);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


                EditorGUILayout.LabelField("A custom object used when dropping this item like a pouch or chest.", InventoryEditorUtil.labelStyle);
                selectedRarity.dropObject = (GameObject)EditorGUILayout.ObjectField("Drop object", selectedRarity.dropObject, typeof(GameObject), false);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
            }
        }
    }
}