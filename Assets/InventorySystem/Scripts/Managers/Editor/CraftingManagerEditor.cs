using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    public class CraftingManagerEditor : EditorWindow
    {
        private InventoryCraftingCategory selectedCategory;
        private InventoryCraftingBlueprint selectedBlueprint;

        private Vector2 blueprintScrollPosition = new Vector2();
        private Vector2 itemsScrollPosition = new Vector2();

        private int toolbarIndex = 0;
        //private static string[] categories;
        private static string[] craftingCategories;
        private static Vector2 selctedCategoryScrollPos = new Vector2();
        private InventoryCraftingBlueprintLayout.Row.Column layoutObjectPickerSetFor;
        private InventoryCraftingBlueprintItemRow selectedBlueprintRequiredItemLookup;

        [MenuItem("Tools/InventorySystem/Crafting manager")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<CraftingManagerEditor>("Crafting manager", true);
        }

        public void OnEnable()
        {
            InventoryEditorUtil.Init();            
        }

        public void OnGUI()
        {
            InventoryEditorUtil.Update();
            if (InventoryEditorUtil.selectedDatabase == null)
            {
                InventoryEditorUtil.ShowItemDatabasePicker();
                return;
            }
            
            EditorGUILayout.BeginHorizontal();

            GUI.color = Color.grey;
            if (GUILayout.Button("< DB", InventoryEditorUtil.toolbarStyle, GUILayout.Width(60)))
            {
                InventoryEditorUtil.selectedDatabase = null;
                EditorGUILayout.EndHorizontal();
                return;
            }
            GUI.color = Color.white;
            craftingCategories = new string[InventoryEditorUtil.selectedDatabase.craftingCategories.Length + 1];
            for (int i = 0; i < InventoryEditorUtil.selectedDatabase.craftingCategories.Length; i++)
            {
                craftingCategories[i] = InventoryEditorUtil.selectedDatabase.craftingCategories[i].name + " Editor";
            }
            craftingCategories[craftingCategories.Length - 1] = "Crafting category editor";

            //int toolbarIndexCache = toolbarIndex;
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, craftingCategories, InventoryEditorUtil.toolbarStyle);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(InventoryEditorUtil.boxStyle);
            EditorGUILayout.Space();



            if(toolbarIndex == craftingCategories.Length - 1)
            {
                CraftingCategoryEditor();
            }
            else if(toolbarIndex < craftingCategories.Length - 1)
            {
                CraftingCategoryBlueprintEditor(InventoryEditorUtil.selectedDatabase.craftingCategories[toolbarIndex]);
            }

            EditorGUILayout.EndHorizontal();


            if (GUI.changed)
                EditorUtility.SetDirty(InventoryEditorUtil.selectedDatabase); // To make sure it gets saved.
        }

        private void CraftingCategoryBlueprintEditor(InventoryCraftingCategory category)
        {
            blueprintScrollPosition = EditorGUILayout.BeginScrollView(blueprintScrollPosition, GUILayout.Width(440));


            #region Dialog for selecting result item
            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == 7)
                {
                    var selected = EditorGUIUtility.GetObjectPickerObject();
                    if (selected != null)
                    {
                        if (selectedBlueprint != null)
                        {
                            selectedBlueprint.itemResult = ((GameObject)selected).GetComponent<InventoryItemBase>();
                            Repaint();
                        }
                    }
                    else
                    {
                        if (selectedBlueprint != null)
                        {
                            selectedBlueprint.itemResult = null;
                            Repaint();
                        }
                    }
                }
            }
            #endregion

            #region Dialog for selecting a required item

            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == 62)
                {
                    var selected = EditorGUIUtility.GetObjectPickerObject();
                    if (selected != null)
                    {
                        if (selectedBlueprintRequiredItemLookup  != null)
                        {
                            selectedBlueprintRequiredItemLookup.item = ((GameObject)selected).GetComponent<InventoryItemBase>();
                            Repaint();
                        }
                    }
                    else
                    {
                        if (selectedBlueprintRequiredItemLookup  != null)
                        {
                            selectedBlueprintRequiredItemLookup.item = null;
                            Repaint();
                        }
                    }
                }
            }

            #endregion

            #region Dialog for selecting an item inside the layout manager

            if (Event.current.commandName == "ObjectSelectorClosed")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == 61)
                {
                    var selected = EditorGUIUtility.GetObjectPickerObject();
                    if (selected != null)
                    {
                        if (layoutObjectPickerSetFor != null)
                        {
                            layoutObjectPickerSetFor.item = ((GameObject)selected).GetComponent<InventoryItemBase>();
                            if (layoutObjectPickerSetFor.amount <= 0)
                                layoutObjectPickerSetFor.amount = 1;

                            Repaint();
                        }
                    }
                    else
                    {
                        if (layoutObjectPickerSetFor != null)
                        {
                            layoutObjectPickerSetFor.item = null;
                            Repaint();
                        }
                    }
                }
            }

            #endregion




            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Create new " + craftingCategories[toolbarIndex].ToLower().Replace(" editor", "") + " blueprint"))
            {
                int highestID = 0;
                foreach (var cat in InventoryEditorUtil.selectedDatabase.craftingCategories)
                {
                    foreach (var blue in cat.blueprints)
                    {
                        if (blue.ID > highestID)
                            highestID = blue.ID;
                    }
                }
                highestID++;

                var l = new List<InventoryCraftingBlueprint>(category.blueprints);
                l.Add(new InventoryCraftingBlueprint(){ ID = highestID });
                category.blueprints = l.ToArray();

                for (int i = 0; i < category.blueprints.Length; i++)
                {
                    category.blueprints[i].indexInCategory = i;
                }
                AssetDatabase.SaveAssets();
            }

            if(GUILayout.Button("Re-order items"))
            {
                var ordered = category.blueprints.OrderBy(o => o.itemResult != null ? o.itemResult._category.ToString() : string.Empty);
                category.blueprints = ordered.ToArray();
            }

            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(category.blueprints.Length + " blueprints");
            EditorGUILayout.EndHorizontal();
            // END ROW

            int x = 0;
            foreach (var blueprint in category.blueprints)
            {
                if (blueprint == null)
                    continue;

                if (blueprint == selectedBlueprint)
                    GUI.color = Color.green;

                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + blueprint.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(blueprint.name);

                if(blueprint.itemResult == null)
                {
                    GUI.color = Color.yellow;
                    EditorGUILayout.HelpBox("No result selected", MessageType.Warning);
                    GUI.color = Color.white;
                }

                GUI.color = Color.green;
                if (GUILayout.Button("Edit", GUILayout.Width(60)))
                {
                    selectedBlueprint = blueprint;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete category #" + blueprint.ID + " with the name \"" + blueprint.name + "\"?\n\nAll blueprints in this category will also be deleted!", "Yes", "NO!"))
                    {
                        var l = new List<InventoryCraftingBlueprint>(category.blueprints);
                        l.RemoveAt(blueprint.indexInCategory);
                        category.blueprints = l.ToArray();

                        selectedBlueprint = null;

                        for (int i = 0; i < category.blueprints.Length; i++)
                        {
                            category.blueprints[i].indexInCategory = i;
                        }

                        AssetDatabase.SaveAssets();
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
                x++;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginVertical("box");

            if (selectedBlueprint != null)
            {
                selctedCategoryScrollPos = EditorGUILayout.BeginScrollView(selctedCategoryScrollPos);

                #region About craft

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Step 1. What are we crafting?", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                selectedBlueprint.useItemResultNameAndDescription = EditorGUILayout.Toggle("Use result item's name", selectedBlueprint.useItemResultNameAndDescription);
                if (selectedBlueprint.useItemResultNameAndDescription == false)
                {
                    selectedBlueprint.customName = EditorGUILayout.TextField("Blueprint name", selectedBlueprint.customName);
                    selectedBlueprint.customDescription = EditorGUILayout.TextField("Blueprint description", selectedBlueprint.customDescription);
                    GUI.enabled = false;
                    EditorGUILayout.TextField("Category", InventoryEditorUtil.selectedDatabase.itemCategories[selectedBlueprint.itemResult != null ? selectedBlueprint.itemResult._category : 0].name);
                }
                else
                {
                    GUI.enabled = false;
                    EditorGUILayout.TextField("Blueprint name", selectedBlueprint.itemResult != null ? selectedBlueprint.itemResult.name : string.Empty);
                    EditorGUILayout.TextField("Blueprint description", selectedBlueprint.itemResult != null ? selectedBlueprint.itemResult.description : string.Empty);
                    EditorGUILayout.TextField("Category", InventoryEditorUtil.selectedDatabase.itemCategories[selectedBlueprint.itemResult != null ? selectedBlueprint.itemResult._category : 0].name);                
                }
                GUI.enabled = true;


                #endregion

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                #region Crafting process

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Step 2. How are we crafting it?", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                selectedBlueprint.successChanceFactor = EditorGUILayout.Slider("Chance factor", selectedBlueprint.successChanceFactor, 0.0f, 1.0f);
                selectedBlueprint.craftingTimeDuration = EditorGUILayout.FloatField("Crafting time duration (seconds)", selectedBlueprint.craftingTimeDuration);
                selectedBlueprint.craftingTimeSpeedupFactor = EditorGUILayout.FloatField("Speedup factor", selectedBlueprint.craftingTimeSpeedupFactor);
                selectedBlueprint.craftingTimeSpeedupMax = EditorGUILayout.FloatField("Max speedup", selectedBlueprint.craftingTimeSpeedupMax);



                if (selectedBlueprint.craftingTimeSpeedupFactor != 1.0f)
                {
                    EditorGUILayout.Space();

                    for (int i = 1; i < 16; i++)
                    {
                        float f = Mathf.Clamp(Mathf.Pow(selectedBlueprint.craftingTimeSpeedupFactor, i * 5), 0.0f, selectedBlueprint.craftingTimeSpeedupMax);
                    
                        if(f != selectedBlueprint.craftingTimeSpeedupMax)
                            EditorGUILayout.LabelField("Speedup after \t" + (i * 5) + " crafts \t" + System.Math.Round(f, 2) + "x \t(" + System.Math.Round(selectedBlueprint.craftingTimeDuration / f, 2) + "s per item)");
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Reached max after " + 1.0f / Mathf.Log(selectedBlueprint.craftingTimeSpeedupFactor, selectedBlueprint.craftingTimeSpeedupMax) + " crafts");
                }
            
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Step 2.5. What items does the user need? (Ignore if using layouts)", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                EditorGUILayout.BeginVertical();
                foreach (var item in selectedBlueprint.requiredItems)
                {
                    EditorGUILayout.BeginHorizontal("box");

                    if (item.item == null)
                        GUI.color = Color.red;
                    else
                        GUI.color = Color.green;

                    EditorGUILayout.LabelField(item.item != null ? "#" + item.item.ID + " - " + item.item.name : "No item selected");
                    if (item.amount < 1)
                        item.amount = 1;

                    item.amount = EditorGUILayout.IntField(item.amount, GUILayout.Width(80));

                    if (GUILayout.Button("Select object", EditorStyles.objectField))
                    {
                        selectedBlueprintRequiredItemLookup = item;

                        // Need to do it manually because we can't apply filters in the default object picker
                        EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(null, false, "l:InventoryItemPrefab", 62);
                    }
                    if (GUILayout.Button("X", GUILayout.Width(40)))
                    {
                        var l = new List<InventoryCraftingBlueprintItemRow>(selectedBlueprint.requiredItems);
                        l.Remove(item);
                        selectedBlueprint.requiredItems = l.ToArray();
                    }

                    GUI.color = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Add required item"))
                {
                    var l = new List<InventoryCraftingBlueprintItemRow>(selectedBlueprint.requiredItems);
                    l.Add(new InventoryCraftingBlueprintItemRow());
                    selectedBlueprint.requiredItems = l.ToArray();
                }

                selectedBlueprint.craftCostPrice = EditorGUILayout.FloatField("Craft cost gold", selectedBlueprint.craftCostPrice);

                #endregion


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


                #region Craft result

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Step 3. What's the result?", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                EditorGUILayout.BeginHorizontal();
            
                if (selectedBlueprint.itemResult == null)
                    GUI.color = Color.red;
                else
                    GUI.color = Color.green;

                EditorGUILayout.LabelField(selectedBlueprint.itemResult != null ? "#" + selectedBlueprint.itemResult.ID + " - " + selectedBlueprint.itemResult.name : "No item selected", GUILayout.Width(120));

                if (GUILayout.Button("Select object", EditorStyles.objectField))
                {
                    // Need to do it manually because we can't apply filters in the default object picker
                    EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(null, false, "l:InventoryItemPrefab", 7);
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();

                selectedBlueprint.itemResultCount = EditorGUILayout.IntField("Result count", selectedBlueprint.itemResultCount);

                selectedBlueprint.playerLearnedBlueprint = EditorGUILayout.Toggle("Player learned blueprint", selectedBlueprint.playerLearnedBlueprint);

                #endregion


                //EditorGUILayout.Space();
                //EditorGUILayout.Space();
                //EditorGUILayout.Space();
                //EditorGUILayout.Space();


                #region Layouts

                //GUI.color = Color.yellow;
                //EditorGUILayout.LabelField("Step 4 (optional). Layouts are used in games like Minecraft, how should the user place the items to craft this item?", InventoryEditorUtil.labelStyle);
                //GUI.color = Color.white;

                //EditorGUILayout.LabelField("Sometimes you might want your layout to be valid in a mirrored image. For example creating a stairs using 3x3 blocks from left to right, could also be valid from right to left.", InventoryEditorUtil.labelStyle);
            
                //selectedBlueprint.checkMirroredLayoutHorizontal = EditorGUILayout.Toggle("Allow mirroring horizontal", selectedBlueprint.checkMirroredLayoutHorizontal);
                //selectedBlueprint.checkMirroredLayoutVertical = EditorGUILayout.Toggle("Allow mirroring vertical", selectedBlueprint.checkMirroredLayoutVertical);


                #endregion


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();


                #region Layout editor

                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Step 4 (optional). Define the layouts to use", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                int counter = 0;
                foreach (var l in selectedBlueprint.blueprintLayouts)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();

                    l.enabled = EditorGUILayout.BeginToggleGroup("Layout #" + l.ID + "-" + (l.enabled ? "(enabled)" : "(disabled)"), l.enabled);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Up", GUILayout.Width(80)))
                    {

                    }
                    if (GUILayout.Button("Down", GUILayout.Width(80)))
                    {

                    }
                    GUI.color = Color.red;
                    if (GUILayout.Button("Delete"))
                    {
                        var t = new List<InventoryCraftingBlueprintLayout>(selectedBlueprint.blueprintLayouts);
                        t.RemoveAt(counter);
                        selectedBlueprint.blueprintLayouts = t.ToArray();

                        AssetDatabase.SaveAssets();
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                    //EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginVertical();
                    if (l.enabled)
                    {
                        foreach (var r in l.rows)
                        {
                            EditorGUILayout.BeginHorizontal();
                            foreach (var c in r.columns)
                            {
                                if (c.item != null)
                                    GUI.color = Color.green;

                                EditorGUILayout.BeginVertical("box", GUILayout.Width(80), GUILayout.Height(80));

                                EditorGUILayout.LabelField((c.item != null) ? c.item.name : string.Empty, InventoryEditorUtil.labelStyle);
                                c.amount = EditorGUILayout.IntField(c.amount);

                                if (GUILayout.Button("Set", GUILayout.Width(80)))
                                {
                                    layoutObjectPickerSetFor = c;
                                    EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(null, false, "l:InventoryItemPrefab", 61);
                                }
                                if (GUILayout.Button("Clear", EditorStyles.miniButton))
                                {
                                    c.amount = 0;
                                    c.item = null;
                                }

                                EditorGUILayout.EndVertical();

                                GUI.color = Color.white;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndToggleGroup();

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    counter++;
                }


                if(GUILayout.Button("Add layout"))
                {
                    var l = new List<InventoryCraftingBlueprintLayout>(selectedBlueprint.blueprintLayouts);
                    var obj = new InventoryCraftingBlueprintLayout();

                    obj.ID = l.Count;
                    obj.rows = new InventoryCraftingBlueprintLayout.Row[category.rows];
                    for (int i = 0; i < obj.rows.Length; i++)
                    {
                        obj.rows[i] = new InventoryCraftingBlueprintLayout.Row();
                        obj.rows[i].index = i;
                        obj.rows[i].columns = new InventoryCraftingBlueprintLayout.Row.Column[category.cols];

                        for (int j = 0; j < obj.rows[i].columns.Length; j++)
                        {
                            obj.rows[i].columns[j] = new InventoryCraftingBlueprintLayout.Row.Column();
                            obj.rows[i].columns[j].index = j;
                        }
                    }

                    l.Add(obj);
                    selectedBlueprint.blueprintLayouts = l.ToArray();
                }

                #endregion


                GUI.enabled = true; // From layouts


                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();
        }


        private void CraftingCategoryEditor()
        {
            itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

            // BEGIN ROW
            EditorGUILayout.BeginHorizontal("box");
            GUI.color = Color.green;
            if (GUILayout.Button("Create item"))
            {
                var l = new List<InventoryCraftingCategory>(InventoryEditorUtil.selectedDatabase.craftingCategories);
                l.Add(new InventoryCraftingCategory());
                InventoryEditorUtil.selectedDatabase.craftingCategories = l.ToArray();
                toolbarIndex = InventoryEditorUtil.selectedDatabase.craftingCategories.Length;

                for (int i = 0; i < InventoryEditorUtil.selectedDatabase.craftingCategories.Length; i++)
                {
                    InventoryEditorUtil.selectedDatabase.craftingCategories[i].ID = i;
                }
                AssetDatabase.SaveAssets();
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            // END ROW

            // BEGIN ROW
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.Label(manager.craftingCategories.Length + " categories");
            //EditorGUILayout.EndHorizontal();
            // END ROW

            int x = 0;
            foreach (var item in InventoryEditorUtil.selectedDatabase.craftingCategories)
            {
                if (item == null)
                {
                    continue;
                }
                if (item == selectedCategory)
                    GUI.color = Color.green;
          
                EditorGUILayout.BeginHorizontal("box");

                EditorGUILayout.LabelField("#" + item.ID, GUILayout.Width(40));
                EditorGUILayout.LabelField(item.name);

                GUI.color = Color.green;
                if (GUILayout.Button("Edit", GUILayout.Width(60)))
                {
                    selectedCategory = item;
                }

                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete category #" + item.ID + " with the name \"" + item.name + "\"?\n\nAll blueprints in this category will also be deleted!", "Yes", "NO!"))
                    {
                        if (EditorUtility.DisplayDialog("Very sure?", "Once you delete this category all blueprints in it will also be removed!", "Yes", "NO!"))
                        {
                            var l = new List<InventoryCraftingCategory>(InventoryEditorUtil.selectedDatabase.craftingCategories);
                            l.RemoveAt(item.ID);
                            InventoryEditorUtil.selectedDatabase.craftingCategories = l.ToArray();
                        
                            selectedCategory = null;
                            toolbarIndex = InventoryEditorUtil.selectedDatabase.craftingCategories.Length;

                            for (int i = 0; i < InventoryEditorUtil.selectedDatabase.craftingCategories.Length; i++)
                            {
                                InventoryEditorUtil.selectedDatabase.craftingCategories[i].ID = i;
                            }

                            AssetDatabase.SaveAssets();
                        }
                    }
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
                x++;
            }

            EditorGUILayout.EndScrollView();
        
            EditorGUILayout.BeginVertical("box", GUILayout.Width(500));

            if (selectedCategory != null)
            {
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("Note that this is not used for item but categories but rather category types such as Smithing, Tailoring, etc.", InventoryEditorUtil.labelStyle);
                GUI.color = Color.white;

                selectedCategory.name = EditorGUILayout.TextField("Category name", selectedCategory.name);
                selectedCategory.description = EditorGUILayout.TextField("Category description", selectedCategory.description);

                EditorGUILayout.Space();
                selectedCategory.alsoScanBankForRequiredItems = EditorGUILayout.Toggle("Scan bank for craft items", selectedCategory.alsoScanBankForRequiredItems);
                EditorGUILayout.Space();


                selectedCategory.rows = (uint)EditorGUILayout.IntField("Layout rows", (int)selectedCategory.rows);
                selectedCategory.cols = (uint)EditorGUILayout.IntField("Layout cols", (int)selectedCategory.cols);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Forces the result item to be saved in a collection, leave empty to auto. detect the best collection.", InventoryEditorUtil.labelStyle);
                selectedCategory.forceSaveInCollection = (ItemCollectionBase)EditorGUILayout.ObjectField("Force save in collection", selectedCategory.forceSaveInCollection, typeof(ItemCollectionBase), true);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Category contains " + selectedCategory.blueprints.Length + " blueprints.", InventoryEditorUtil.labelStyle);
            }

            EditorGUILayout.EndVertical();
        }
    }
}