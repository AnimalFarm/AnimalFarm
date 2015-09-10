using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem
{
    /// <summary>
    /// Used to represent items that can be equipped by the user, this includes armor, weapons, etc.
    /// </summary>
    public partial class EquippableInventoryItem : InventoryItemBase
    {
        [SerializeField]
        [HideInInspector]
        private int _equipType;
        public InventoryEquipType equipType
        {
            get
            {
                return ItemManager.instance.equipTypes[_equipType];
            }
        }

        [InventoryStat]
        public int strength;
    
        [InventoryStat]
        public int agility;
    
        [InventoryStat]
        public int defense;

        public AudioClip playOnEquip;

        //private bool isEquipped = false;

        public override int Use()
        {
            int used = base.Use();
            if (used < 0)
                return used;
        
            if(itemCollection == InventoryManager.instance.character)
            {
                Unequip();
                return 1; // Used from inside the character, move back to inventory.
            }

            var equipSlot = FindBestEquipSlot();
            if (equipSlot == null)
                return -2; // No slot found

            bool handled = HandleLocks(equipSlot, itemCollection);
            if (handled == false)
                return -2; // Other items cannot be unequipped


            if (InventoryManager.instance.character[equipSlot.index].item != null)
            {
                var a = InventoryManager.instance.character[equipSlot.index].item as EquippableInventoryItem;
                if (a != null)
                    a.Unequip();
            }


            bool equipped = Equip(equipSlot);
            if (equipped)
                return 1;

            return -2;
        }

        protected virtual bool Equip(InventoryEquippableField equipSlot)
        {
            // Equip the item -> Will swap as merge is not possible
            bool swapped = itemCollection.SwapOrMerge(index, InventoryManager.instance.character, equipSlot.index);
            if (swapped)
            {
                InventoryManager.instance.character.NotifyItemEquipped(this, itemCollection, (int)equipSlot.index);

                if (playOnEquip)
                    InventoryUIUtility.AudioPlayOneShot(playOnEquip);

                NotifyItemUsed(1);
                return true;
            }

            return false;
        }

        protected virtual bool Unequip()
        {
            bool added = InventoryManager.AddItemAndRemove(this);
            if (added == false)
                return false;

            InventoryManager.instance.character.NotifyItemUnequipped(this, itemCollection, (int)index);

            NotifyItemUsed(1);
            return true;
        }

        protected virtual InventoryEquippableField FindBestEquipSlot()
        {
            if (InventoryManager.instance.character == null)
                return null;

            var equipSlots = InventoryManager.instance.character.GetEquippableSlots(this);
            if (equipSlots.Length == 0)
            {
                Debug.LogWarning("No suitable equip slot found for item " + name, gameObject);
                return null;
            }

            InventoryEquippableField equipSlot = equipSlots[0];
            foreach (var e in equipSlots)
            {
                if (InventoryManager.instance.character[e.index].item == null)
                {
                    equipSlot = e; // Prefer an empty slot over swapping a filled one.
                }
            }

            return equipSlot;
        }

        /// <summary>
        /// Some item's require multiple slots, for example a 2 handed item forces the left handed item to be empty.
        /// </summary>
        /// <returns>true if items were removed, false if items were not removed.</returns>
        public virtual bool HandleLocks(InventoryEquippableField equipSlot, ItemCollectionBase usedFromCollection)
        {
            var toBeRemoved = new List<uint>(8);

            // Loop through things we want to block
            foreach (var blockType in equipType.blockTypes)
            {
                // Check every slot against this block type
                foreach (var field in InventoryManager.instance.character.equipSlotFields)
                {
                    var item = InventoryManager.instance.character[field.index].item;
                    if(item != null)
                    {
                        var eq = (EquippableInventoryItem)item;

                        if(eq.equipType.ID == blockType && field.index != equipSlot.index)
                        {
                            toBeRemoved.Add(field.index);
                            bool canAdd = InventoryManager.CanAddItem(eq);
                            if (canAdd == false)
                                return false;
                        }
                    }
                }
            }

            foreach (uint i in toBeRemoved)
            {
                bool added = InventoryManager.AddItem(InventoryManager.instance.character[i].item);
                if (added == false)
                {
                    Debug.LogError("Item could not be saved, even after check, please report this bug + stacktrace.");
                    return false;
                }

                InventoryManager.instance.character.SetItem(i, null);
                InventoryManager.instance.character[i].Repaint();
            }

            return true;
        }


        public override LinkedList<InfoBox.Row[]> GetInfo()
        {
            var info = base.GetInfo();

            info.AddAfter(info.First, new InfoBox.Row[]
            {
                new InfoBox.Row("Strength", strength.ToString()),
                new InfoBox.Row("Agility", agility.ToString()),
                new InfoBox.Row("Defense", defense.ToString())
            });

            return info;
        }
    }
}