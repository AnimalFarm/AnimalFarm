#if UFPS

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using Devdog.InventorySystem.Integration.UFPS;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(EquippableUFPSInventoryItem), true)]
    [CanEditMultipleObjects()]
    public class EquippableUFPSInventoryItemEditor : EquippableInventoryItemEditor
    {

        private EquippableUFPSInventoryItem tar;


        public override void OnEnable()
        {
            base.OnEnable();


            tar = (EquippableUFPSInventoryItem) target;

        }

        protected override void OnCustomInspectorGUI(List<string> doNotDraw)
        {
            serializedObject.Update();
            
            

            serializedObject.ApplyModifiedProperties();


            var l = new List<string>();
            if (tar.useUFPSItemData)
            {
                l.Add("_id");
                l.Add("_name");
                l.Add("_description");
            }

            base.OnCustomInspectorGUI(l);
        }
    }
}

#endif