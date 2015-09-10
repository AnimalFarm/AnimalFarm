using System;
using UnityEngine;
using System.Collections;

namespace Devdog.InventorySystem.Models
{
    [RequireComponent(typeof(InventoryUIItemWrapper))]
    [AddComponentMenu("InventorySystem/UI Helpers/Equippable field")]
    public class InventoryEquippableField : MonoBehaviour
    {
        /// <summary>
        /// Index of this field
        /// </summary>
        [HideInInspector]
        public uint index
        {
            get
            {
                return itemWrapper.index;
            }
        }

        [SerializeField]
        private int[] _equipTypes;
        public InventoryEquipType[] equipTypes
        {
            get
            {
                InventoryEquipType[] types = new InventoryEquipType[_equipTypes.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    types[i] = ItemManager.instance.equipTypes[_equipTypes[i]];
                }

                return types;
            }
        }

        [NonSerialized]
        private InventoryUIItemWrapper itemWrapper;

        public void Awake()
        {
            itemWrapper = GetComponent<InventoryUIItemWrapper>();
        }
    }
}