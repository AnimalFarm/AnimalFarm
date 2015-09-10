using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    public class LangManagerEditor : EditorWindow
    {
        private Editor editor;
        private Vector2 scrollPos = new Vector2();

        [MenuItem("Tools/InventorySystem/Language manager")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<LangManagerEditor>("Language manager", true);
        }

        public void OnEnable()
        {
            InventoryEditorUtil.Init();


            if (InventoryEditorUtil.GetInventoryManager() != null)
                InventoryEditorUtil.selectedLangDatabase = InventoryEditorUtil.GetInventoryManager().lang;
        }


        public void OnGUI()
        {
            InventoryEditorUtil.Update();
            if (InventoryEditorUtil.selectedLangDatabase == null)
            {
                InventoryEditorUtil.ShowLangDatabasePicker();
                return;
            }

            if (editor == null)
            {
                editor = Editor.CreateEditor(InventoryEditorUtil.selectedLangDatabase);
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            editor.DrawHeader();
            editor.DrawDefaultInspector();


            EditorGUILayout.EndScrollView();

            if (GUI.changed)
                EditorUtility.SetDirty(InventoryEditorUtil.selectedLangDatabase); // To make sure it gets saved.
        }
    }
}