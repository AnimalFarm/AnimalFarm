﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Reflection;
using Devdog.InventorySystem;
using Devdog.InventorySystem.Models;
using Devdog.InventorySystem.UI.Models;

namespace Devdog.InventorySystem
{
    public delegate void EquipItem(InventoryItemBase item, ItemCollectionBase fromCollection, int fromSlot);
    public delegate void UnequipItem(InventoryItemBase item, ItemCollectionBase fromCollection, int fromSlot);


    [AddComponentMenu("InventorySystem/Windows/Character")]
    [RequireComponent(typeof(UIWindow))]
    public partial class CharacterUI : ItemCollectionBase
    {
        public partial class StatLookup
        {
            /// <summary>
            /// Name of stat
            /// </summary>
            public string name;

            /// <summary>
            /// The category this stat belongs to.
            /// </summary>
            public string category;

            /// <summary>
            /// The amount of status.
            /// </summary>
            public int amount;

            public StatLookup(string name, string category)
            {
                this.name = name;
                this.category = category;
                this.amount = 0;
            }
        }


        //public EquipBlockLookup[] equipToLookups;
        public event EquipItem OnEquipItem;
        public event UnequipItem OnUnequipItem;

        public RectTransform statsContainer;
        public InventoryEquipStatRowUI statusRowPrefab;
        public InventoryEquipStatCategoryUI statusCategoryPrefab;

        [Range(0, 100)]
        public int equipPriority = 50;

        public InventoryEquippableField[] equipSlotFields { get; private set; }
        public Dictionary<string, List<InventoryEquipStatRowLookup>> characterStats { get; protected set; }

        protected InventoryPool<InventoryEquipStatRowUI> rowsPool;
        protected InventoryPool<InventoryEquipStatCategoryUI> categoryPool;

        protected UIWindow window;

        public override uint initialCollectionSize
        {
            get
            {
                return (uint)equipSlotFields.Length;
            }
        }

        public override void Awake()
        {
            equipSlotFields = container.GetComponentsInChildren<InventoryEquippableField>(true);
            characterStats = new Dictionary<string, List<InventoryEquipStatRowLookup>>(ItemManager.instance.equipStats.Length);
            base.Awake();

            window = GetComponent<UIWindow>();

            if (statusRowPrefab != null)
                rowsPool = new InventoryPool<InventoryEquipStatRowUI>(statusRowPrefab, 64);

            if (statusCategoryPrefab != null)
                categoryPool = new InventoryPool<InventoryEquipStatCategoryUI>(statusCategoryPrefab, 8);

            InventoryManager.AddEquipCollection(this, equipPriority);
            //UpdateCharacterStats();

            window.OnShow += () =>
            {
                RepaintStats();
            };
        }

        /// <summary>
        /// Get all slots where this item can be equipped.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns indices where the item can be equipped. collection[index] ... </returns>
        public InventoryEquippableField[] GetEquippableSlots(EquippableInventoryItem item)
        {
            var equipSlots = new List<InventoryEquippableField>(4);
            foreach (var field in equipSlotFields)
            {
                foreach (var type in field.equipTypes)
                {
                    if (item.equipType.ID == type.ID)
                    {
                        equipSlots.Add(field);
                    }
                }
            }

            return equipSlots.ToArray();
        }

        public void NotifyItemEquipped(InventoryItemBase item, ItemCollectionBase fromCollection, int fromSlot)
        {
            UpdateCharacterStats();

            if (OnEquipItem != null)
                OnEquipItem(item, fromCollection, fromSlot);
        }
        public void NotifyItemUnequipped(InventoryItemBase item, ItemCollectionBase fromCollection, int fromSlot)
        {
            UpdateCharacterStats();

            if (OnUnequipItem != null)
                OnUnequipItem(item, fromCollection, fromSlot);
        }


        /// <summary>
        /// Show the status
        /// </summary>
        public virtual void UpdateCharacterStats(bool repaint = true)
        {
            characterStats.Clear();
            //characterStats = new Dictionary<string, List<KeyValuePair<string, string>>>(ItemManager.instance.equipStats.Length); 
            foreach (var stat in ItemManager.instance.equipStats)
            {
                if (stat.show == false)
                    continue;

                var l = new List<float>(items.Length);
                foreach (var item in items)
                {
                    // Handle the stats by attribute and category.
                    if (item.item != null)
                    {
                        var field = FindFieldInherited(item.item.GetType(), stat.fieldInfoName);
                        if(field != null)
                        {
                            // First cast it to string, later we'll see if we can parse it to float / int. If not it's probably a different type of stat.
                            string value = field.GetValue(item.item).ToString();
                            float val = float.Parse(value); // Assuming we're using floats / ints or uints here...
                            l.Add(val);
                        }
                    }
                }

                if (characterStats.ContainsKey(stat.category) == false)
                    characterStats.Add(stat.category, new List<InventoryEquipStatRowLookup>());

                // The stat
                string st = stat.formatter != null ? stat.formatter.FormatStat(l) : InventorySettingsManager.instance.defaultCharacterStatFormatter.FormatStat(l);
                float total = 0.0f;
                foreach (var item in l)
                    total += item;

                characterStats[stat.category].Add(new InventoryEquipStatRowLookup(stat.name, total, st));
            }

            if (repaint)
                RepaintStats();
        }

        protected virtual void RepaintStats()
        {
            if (window.isVisible == false || statusRowPrefab == null || statusCategoryPrefab == null)
                return;

            // Get rid of the old
            categoryPool.DestroyAll();
            rowsPool.DestroyAll();

            // Maybe make a pool for the items? See some spikes...
            foreach (var stat in characterStats)
            {
                // stat.Key is category
                // stat.Value is all items in category 
                var cat = categoryPool.Get();
                //cat.gameObject.SetActive(window.isVisible);
                cat.SetCategory(stat.Key);
                cat.transform.SetParent(statsContainer);
                cat.transform.localPosition = new Vector3(cat.transform.localPosition.x, cat.transform.localPosition.y, 0.0f);

                foreach (var s in stat.Value)
                {
                    var obj = rowsPool.Get();
                    //obj.gameObject.SetActive(window.isVisible);
                    //var obj = GameObject.Instantiate<InventoryEquipStatRow>(statusRowPrefab);
                    obj.SetRow(s.statName, s.finalValueString);

                    obj.transform.SetParent(cat.container);
                    obj.transform.localPosition = Vector3.zero; // UI Layout will handle it.
                }
            }
        }

        private FieldInfo FindFieldInherited(System.Type startType, string fieldName)
        {
            if (startType == typeof(UnityEngine.MonoBehaviour) || startType == null)
                return null;

            // Copied fields can be restricted with BindingFlags
            var field = startType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                return field;

            // Keep going untill we hit UnityEngine.MonoBehaviour type.
            return FindFieldInherited(startType.BaseType, fieldName);
        }

        public override bool SetItem(uint slot, InventoryItemBase item)
        {
            ItemCollectionBase prevCollection = null;
            int prevIndex = -1;
            if(item != null)
            {
                prevCollection = item.itemCollection;
                prevIndex = (int)item.index;
            }

            bool set = base.SetItem(slot, item);
            if (set == false)
                return false;

            if (item == null)
                return true;

            var equippable = (EquippableInventoryItem)item;
            bool handled = equippable.HandleLocks(InventoryManager.instance.character.equipSlotFields[slot], null);
            if (handled)
            {
                NotifyItemEquipped(item, prevCollection, prevIndex);
            }

            return handled;
        }

        public override void SetItems(InventoryItemBase[] toSet, bool setParent, bool repaint = true)
        {
            base.SetItems(toSet, setParent, repaint);

            UpdateCharacterStats(window.isVisible);
        }

        public override bool CanSetItem(uint slot, InventoryItemBase item)
        {
            bool set = base.CanSetItem(slot, item);
            if (set == false)
                return false;

            if (item == null)
                return true;


            var equippable = (EquippableInventoryItem)item;

            var slots = GetEquippableSlots(equippable);
            if (slots.Length == 0)
                return false;

            // An acceptable slot?
            foreach (var s in slots)
            {
                if (s.index == slot)
                    return true;
            }
        
            return false;
        }

        public override bool CanMergeSlots(uint slot1, ItemCollectionBase collection2, uint slot2)
        {
            return false;
        }
        public override bool SwapOrMerge(uint slot1, ItemCollectionBase handler2, uint slot2, bool repaint = true)
        {
            return SwapSlots(slot1, handler2, slot2, repaint);    
        }
    }
}