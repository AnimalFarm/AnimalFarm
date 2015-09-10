using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Devdog.InventorySystem.Editors
{
    [CustomPropertyDrawer(typeof(InventoryItemBase))]
    public class InventoryItemBasePropertyDrawerEditor : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // TODO, create a custom editor
            EditorGUI.PropertyField(position, property, label);

            EditorGUI.EndProperty();
        }
    }
}