using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Devdog.InventorySystem.Dialogs;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    public class SettingsManagerEditor : EditorWindow
    {
        private ItemManager manager;
        private InventorySettingsManager settings;

        private Vector2 settingsScrollPosition = new Vector2();
        private static string[] raritys;
    

        [MenuItem("Tools/InventorySystem/Tools/Settings", false, 10)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<SettingsManagerEditor>("Settings manager", true);
        }

        public void OnEnable()
        {
            InventoryEditorUtil.Init();
            UpdateCategoriesAndRaritys();
         
            manager = InventoryEditorUtil.GetItemManager();
            settings = InventoryEditorUtil.GetSettingsManager();
        }

        private void UpdateCategoriesAndRaritys()
        {
            var a = InventoryEditorUtil.GetCategoriesAndRaritys();
            raritys = a.rarities;
            //colors = a.rarityColors;
            //categories = a.categories;
        }


        public void OnGUI()
        {
            InventoryEditorUtil.Update();
            UpdateCategoriesAndRaritys();
            if (manager == null || settings == null)
                return;

            EditorGUILayout.BeginHorizontal();

            GUILayout.Toolbar(0, new string[] { "Settings" }, InventoryEditorUtil.toolbarStyle);
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal(InventoryEditorUtil.boxStyle);
            EditorGUILayout.Space();

            ShowSettings();

            EditorGUILayout.EndHorizontal();
        }

        private void ShowSettings()
        {
            settingsScrollPosition = EditorGUILayout.BeginScrollView(settingsScrollPosition);

            InventoryEditorUtil.ErrorIfEmpty(EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty, "Item path is not set, items cannot be saved.");
            if(EditorPrefs.GetString("InventorySystem_ItemPrefabPath") != "")
                InventoryEditorUtil.ErrorIfEmpty((EditorPrefs.GetString("InventorySystem_ItemPrefabPath")).StartsWith("Assets/") == false, "Datapath has to be inside the unity asset folder.");
            
            InventoryEditorUtil.ErrorIfEmpty(settings.defaultSlotIcon, "No default slot icon is set");
            InventoryEditorUtil.ErrorIfEmpty(settings.itemButtonPrefab, "No default UI button prefab is set");
            //ErrorIfEmpty(settings.itemButtonLootPrefab, "No default UI button prefab is set");
            InventoryEditorUtil.ErrorIfEmpty(settings.playerObject, "Player object has to be set, if no player is present use the main camera.");

            if (settings._showConfirmationDialogMinRarity >= raritys.Length)
                settings._showConfirmationDialogMinRarity = 0;

            #region Path selector

            EditorGUILayout.LabelField("Items are saved as prefabs in a folder, this allows you to add components to objects and completely manage them.", InventoryEditorUtil.labelStyle);
            if (EditorPrefs.GetString("InventorySystem_ItemPrefabPath") == string.Empty)
                GUI.color = Color.red;
    
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField("Inventory Item prefab folder: " + EditorPrefs.GetString("InventorySystem_ItemPrefabPath"));
            if (GUILayout.Button("Set path", GUILayout.Width(100)))
            {
                string path = EditorUtility.SaveFolderPanel("Choose a folder to save your item prefabs", "", "");


                EditorPrefs.SetString("InventorySystem_ItemPrefabPath", "Assets" + path.Replace(Application.dataPath, ""));
            }
            EditorGUILayout.EndHorizontal();

            GUI.color = Color.white;

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            #endregion



            #region UI settings

            EditorGUILayout.LabelField("UI ELEMENTS ----------------------------------------- ");


            EditorGUILayout.BeginVertical("box");
            settings.defaultSlotIcon = InventoryEditorUtil.SimpleObjectPicker<Sprite>("Default slot icon", settings.defaultSlotIcon, false, true);
            settings.itemButtonPrefab = InventoryEditorUtil.SimpleObjectPicker<GameObject>("Item button prefab", settings.itemButtonPrefab, false, true);
            //settings.itemButtonLootPrefab = SimpleObjectPicker<GameObject>("Item button loot prefab", settings.itemButtonLootPrefab, false, true);
            settings.useContextMenu = EditorGUILayout.Toggle("Use context menu", settings.useContextMenu);
            if (settings.useContextMenu == false)
                GUI.enabled = false;

            settings.contextMenu = InventoryEditorUtil.SimpleObjectPicker<InventoryContextMenu>("Context menu (null allowed)", settings.contextMenu, true, settings.useContextMenu);
            GUI.enabled = true;

            settings.guiRoot = InventoryEditorUtil.SimpleObjectPicker<RectTransform>("GUI root", settings.guiRoot, true, true);
            //settings.defaultFont = InventoryEditorUtil.SimpleObjectPicker<Font>("Default font", settings.defaultFont, false, true);
            settings.useItemButton = (UnityEngine.EventSystems.PointerEventData.InputButton)EditorGUILayout.EnumPopup("Use item button", settings.useItemButton);

            EditorGUILayout.HelpBox("Note that this should be the player in your scene, not a prefab.", MessageType.Info);
            //EditorGUILayout.LabelField("Note that this", InventoryEditorUtil.labelStyle);
            settings.playerObject = InventoryEditorUtil.SimpleObjectPicker<Transform>("Player object", settings.playerObject, true, true);
            settings.useObjectDistance = EditorGUILayout.FloatField("Use object distance", settings.useObjectDistance);

            settings.maxDropDistance = EditorGUILayout.FloatField("Max drop distance", settings.maxDropDistance);
            settings.dropAtMousePosition = EditorGUILayout.Toggle("Drop at mouse position", settings.dropAtMousePosition);
            settings.dropItemRaycastToGround = EditorGUILayout.Toggle("Drop force to ground", settings.dropItemRaycastToGround);
            if (settings.dropAtMousePosition == false)
                GUI.enabled = false;
                
            settings.layersWhenDropping = InventoryEditorUtil.LayerMaskField("Layers when dropping", settings.layersWhenDropping, true);
            GUI.enabled = true;


            if (settings.dropAtMousePosition)
                GUI.enabled = false;

            EditorGUILayout.HelpBox("Only used when drop at mouse position is false.", MessageType.Info);
            settings.dropOffsetVector = EditorGUILayout.Vector3Field("Drop offset", settings.dropOffsetVector);

            GUI.enabled = true;


            EditorGUILayout.LabelField("Mobile --- ");

            settings.mobileDoubleTapTime = EditorGUILayout.FloatField("Mobile DoubleTap timeout", settings.mobileDoubleTapTime);
            settings.mobileLongPressTime = EditorGUILayout.FloatField("Mobile LongPress timeout", settings.mobileLongPressTime);

            settings.mobileUnstackItemKey = (MobileUIActions)EditorGUILayout.EnumPopup("Mobile Unstack trigger", settings.mobileUnstackItemKey);
            settings.mobileUseItemButton = (MobileUIActions)EditorGUILayout.EnumPopup("Mobile Use trigger", settings.mobileUseItemButton);        


            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            settings.showConfirmationDialogWhenDroppingItem = EditorGUILayout.Toggle("Show dialog when dropping item", settings.showConfirmationDialogWhenDroppingItem);
            if (settings.showConfirmationDialogWhenDroppingItem == false)
                GUI.enabled = false;

            settings._showConfirmationDialogMinRarity = EditorGUILayout.Popup("Minimal rarity to show dialog", settings._showConfirmationDialogMinRarity, raritys);
            settings.confirmationDialog = InventoryEditorUtil.SimpleObjectPicker<ConfirmationDialog>("Confirmation dialog", settings.confirmationDialog, true, true);
        

            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            //EditorGUILayout.LabelField("You can use {0} for the name and {1} for the description, if the confirmation is about an item.", InventoryEditorUtil.labelStyle);
            //settings.defaultConfirmationDialogTitle = EditorGUILayout.TextField("Default confirmation dialog title", settings.defaultConfirmationDialogTitle, EditorStyles.textField);



            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            //EditorGUILayout.LabelField("You can use {0} for the name and {1} for the description, if the confirmation is about an item.", InventoryEditorUtil.labelStyle);
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.PrefixLabel("Default confirmation dialog description");
            //settings.defaultConfirmationDialogDescription = EditorGUILayout.TextArea(settings.defaultConfirmationDialogDescription, EditorStyles.textArea);
            //EditorGUILayout.EndHorizontal();

            GUI.enabled = true;


            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            settings.useUnstackDialog = EditorGUILayout.Toggle("Show dialog when unstacking item", settings.useUnstackDialog);
            if (settings.useUnstackDialog == false)
                GUI.enabled = false;
            settings.intValDialog = InventoryEditorUtil.SimpleObjectPicker<IntValDialog>("Unstack dialog", settings.intValDialog, true, false);


            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            //EditorGUILayout.LabelField("You can use {0} for the name and {1} for the description, if the confirmation is about an item.", InventoryEditorUtil.labelStyle);
            //settings.unstackDialogTitle = EditorGUILayout.TextField("Unstack dialog title", settings.unstackDialogTitle, EditorStyles.textField);


            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            //EditorGUILayout.LabelField("You can use {0} for the name and {1} for the description, if the confirmation is about an item.", InventoryEditorUtil.labelStyle);
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.PrefixLabel("Unstack dialog description");
            //settings.unstackDialogDescription = EditorGUILayout.TextArea(settings.unstackDialogDescription, EditorStyles.textArea);
            //EditorGUILayout.EndHorizontal();


            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            settings.unstackItemButton = (UnityEngine.EventSystems.PointerEventData.InputButton)EditorGUILayout.EnumPopup("Unstack mouse button", settings.unstackItemButton);
            settings.unstackItemKey = (KeyCode)EditorGUILayout.EnumPopup("Unstack key", settings.unstackItemKey);

            GUI.enabled = true;

            EditorGUILayout.EndVertical();


            EditorGUILayout.Space();


            //public RectTransform[] disabledWhileDialogActive = new RectTransform[0];

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Disable windows while dialog is open");
            for (int x = 0; x < settings.disabledWhileDialogActive.Length; x++)
            {
                EditorGUILayout.BeginHorizontal("box");

                //EditorGUILayout.ObjectField(null, typeof(MonoScript), false);

                settings.disabledWhileDialogActive[x] = InventoryEditorUtil.SimpleObjectPicker<RectTransform>("", settings.disabledWhileDialogActive[x], true, false);

                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    var list = new List<RectTransform>(settings.disabledWhileDialogActive);
                    list.RemoveAt(x);
                    settings.disabledWhileDialogActive = list.ToArray();
                }
                GUI.color = Color.white;

                EditorGUILayout.EndHorizontal();
            }


            if (GUILayout.Button("Add"))
            {
                var list = new List<RectTransform>(settings.disabledWhileDialogActive);
                list.Add(null);
                settings.disabledWhileDialogActive = list.ToArray();
            }
            EditorGUILayout.EndVertical();

            #endregion

            EditorGUILayout.EndScrollView();

            if(GUI.changed)
                EditorUtility.SetDirty(settings);
        }
    }
}