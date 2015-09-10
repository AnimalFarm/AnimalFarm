using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(InventoryItemBase), true)]
    [CanEditMultipleObjects()]
    public class InventoryItemBaseEditor : Editor
    {
        //private InventoryItemBase item;
        protected SerializedProperty id;
        protected SerializedProperty itemName; // Name is used by Editor.name...
        protected SerializedProperty description;
        protected SerializedProperty properties;
        protected SerializedProperty useCategoryCooldown;
        protected SerializedProperty category;
        protected SerializedProperty icon;
        protected SerializedProperty weight;
        protected SerializedProperty requiredLevel;
        protected SerializedProperty rarity;
        protected SerializedProperty buyPrice;
        protected SerializedProperty sellPrice;
        protected SerializedProperty isDroppable;
        protected SerializedProperty isSellable;
        protected SerializedProperty isStorable;
        protected SerializedProperty maxStackSize;
        protected SerializedProperty cooldownTime;

        protected static ItemManager itemManager;
        protected static bool propertiesFoldout = true;

        protected static string[] raritys;
        protected static Color[] colors;
        protected static string[] categories;
        protected static string[] propertiesArr;

        public virtual void OnEnable()
        {
            //item = (InventoryItemBase)target;

            id = serializedObject.FindProperty("_id");
            itemName = serializedObject.FindProperty("_name");
            description = serializedObject.FindProperty("_description");
            properties = serializedObject.FindProperty("_properties");
            useCategoryCooldown = serializedObject.FindProperty("_useCategoryCooldown");
            category = serializedObject.FindProperty("_category");
            icon = serializedObject.FindProperty("_icon");
            weight = serializedObject.FindProperty("_weight");
            requiredLevel = serializedObject.FindProperty("_requiredLevel");
            rarity = serializedObject.FindProperty("_rarity");
            buyPrice = serializedObject.FindProperty("_buyPrice");
            sellPrice = serializedObject.FindProperty("_sellPrice");
            isDroppable = serializedObject.FindProperty("_isDroppable");
            isSellable = serializedObject.FindProperty("_isSellable");
            isStorable = serializedObject.FindProperty("_isStorable");
            maxStackSize = serializedObject.FindProperty("_maxStackSize");
            cooldownTime = serializedObject.FindProperty("_cooldownTime");

            itemManager = Editor.FindObjectOfType<ItemManager>();
            if (itemManager == null)
                Debug.LogError("No item manager found in scene, cannot edit item.");
        }

        protected void UpdateProperties()
        {
            propertiesArr = new string[itemManager.properties.Length];
            for (int i = 0; i < itemManager.properties.Length; i++)
                propertiesArr[i] = itemManager.properties[i].key;

            raritys = new string[itemManager.itemRaritys.Length];
            for (int i = 0; i < raritys.Length; i++)
                raritys[i] = itemManager.itemRaritys[i].name;

            colors = new Color[itemManager.itemRaritys.Length];
            for (int i = 0; i < colors.Length; i++)
                colors[i] = itemManager.itemRaritys[i].color;

            categories = new string[itemManager.itemCategories.Length];
            for (int i = 0; i < categories.Length; i++)
                categories[i] = itemManager.itemCategories[i].name;
        }

        public override void OnInspectorGUI()
        {

            OnCustomInspectorGUI(new List<string>(){ });

        }

        protected virtual void OnCustomInspectorGUI(List<string> doNotDraw)
        {
            serializedObject.Update();
            UpdateProperties();

            if(itemManager == null)
                EditorGUILayout.HelpBox("ItemManager not found!", MessageType.Error);

            if(buyPrice.intValue < sellPrice.intValue)
                EditorGUILayout.HelpBox("Buy price is lower than the sell price, are you sure?", MessageType.Warning);
                
            // Can't go below 0
            if (cooldownTime.floatValue < 0.0f)
                cooldownTime.floatValue = 0.0f;

            // Just a safety precaution
            if (rarity.intValue >= raritys.Length)
                rarity.intValue = 0;

            // Just a safety precaution
            if (category.intValue >= categories.Length)
                category.intValue = 0;




            var excludeList = new List<string>()
            {
                "_id",
                "_name",
                "_description",
                "_properties",
                "_category",
                "_useCategoryCooldown",
                "_icon",
                "_weight",
                "_requiredLevel",
                "_rarity",
                "_buyPrice",
                "_sellPrice",
                "_isDroppable",
                "_isSellable",
                "_isStorable",
                "_maxStackSize",
                "_cooldownTime"
            };

            foreach (var item in doNotDraw)
            {
                excludeList.Add(item);
            }


            // Draws remaining items
            EditorGUILayout.BeginVertical("box");
            DrawPropertiesExcluding(serializedObject, excludeList.ToArray());
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("box");
            if (doNotDraw.Contains("_id"))
                GUI.enabled = false;

            EditorGUILayout.LabelField("ID: ", id.intValue.ToString());
            GUI.enabled = true;

            if (doNotDraw.Contains("_name"))
                GUI.enabled = false;

            EditorGUILayout.PropertyField(itemName);
            GUI.enabled = true;

            if (doNotDraw.Contains("_description"))
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Description");
            var style = EditorStyles.textArea;
            style.richText = true;
            style.wordWrap = true;
            style.fixedHeight = 50.0f;

            description.stringValue = EditorGUILayout.TextArea(description.stringValue, style);
            //EditorGUILayout.PropertyField(description, GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            
            EditorGUILayout.EndVertical();

            #region Properties

            EditorGUILayout.BeginVertical("box");
            propertiesFoldout = EditorGUILayout.Foldout(propertiesFoldout, "Properties (" + properties.arraySize + ")");
            if(propertiesFoldout)
            {

                var item = (InventoryItemBase)target;

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);

                EditorGUILayout.LabelField("Key", GUILayout.Width(180));
                EditorGUILayout.LabelField("Value", GUILayout.Width(180));
                //EditorGUILayout.LabelField("Show?", GUILayout.Width(50));
                //EditorGUILayout.LabelField("Color", GUILayout.Width(50));

                EditorGUILayout.LabelField("Up", GUILayout.Width(30));
                EditorGUILayout.LabelField("Down", GUILayout.Width(50));

                EditorGUILayout.LabelField("Delete", GUILayout.Width(50));

                GUILayout.EndHorizontal();


                int x = 0;
                foreach (var i in item.properties)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);

                    // Variables
                    i.ID = EditorGUILayout.Popup(i.ID, propertiesArr, GUILayout.Width(180));
                    if (i.ID >= itemManager.properties.Length)
                        i.ID = itemManager.properties.Length - 1;

                    i.ID = Mathf.Max(i.ID, 0);

                    if (itemManager.properties.Length > 0)
                    {
                        i.key = itemManager.properties[i.ID].key;
                        i.showInUI = itemManager.properties[i.ID].showInUI;
                        i.uiColor = itemManager.properties[i.ID].uiColor;                        
                    }
                    

                    i.value = EditorGUILayout.TextField(i.value, GUILayout.Width(180));

                    // Moving
                    if (x == 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("Up", GUILayout.Width(30)))
                    {
                        properties.MoveArrayElement(x, x - 1);
                    }

                    GUI.enabled = true;

                    if (x >= properties.arraySize - 1)
                        GUI.enabled = false;

                    if (GUILayout.Button("Down", GUILayout.Width(50)))
                    {
                        properties.MoveArrayElement(x, x + 1);
                    }

                    GUI.enabled = true;


                    // Deleting
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        properties.DeleteArrayElementAtIndex(x);
                    }
                    GUI.color = Color.white;



                    GUILayout.EndHorizontal();
                    x++;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUILayout.Button("Add property"))
                {
                    properties.InsertArrayElementAtIndex(properties.arraySize > 0 ? properties.arraySize - 1 : 0);
                }

                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            #endregion



            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(icon);
            EditorGUILayout.PropertyField(weight);
            EditorGUILayout.PropertyField(requiredLevel);

            if(colors.Length > 0)
                GUI.color = colors[rarity.intValue];
            
            rarity.intValue = EditorGUILayout.Popup("Rarity", rarity.intValue, raritys);
            GUI.color = Color.white;

            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical("box");
            category.intValue = EditorGUILayout.Popup("Category", category.intValue, categories);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useCategoryCooldown);
            if(useCategoryCooldown.boolValue)
                EditorGUILayout.LabelField(string.Format("({0} seconds)", itemManager.itemCategories[category == null ? 0 : category.intValue].cooldownTime));
        
            EditorGUILayout.EndHorizontal();
            if (useCategoryCooldown.boolValue == false)
                EditorGUILayout.PropertyField(cooldownTime);

            EditorGUILayout.EndVertical();
        

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(buyPrice);
            EditorGUILayout.PropertyField(sellPrice);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(isDroppable);
            EditorGUILayout.PropertyField(isSellable);
            EditorGUILayout.PropertyField(isStorable);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(maxStackSize);
        
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}