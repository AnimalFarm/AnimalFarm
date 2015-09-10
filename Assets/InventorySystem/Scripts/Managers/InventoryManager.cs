using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem
{
    [RequireComponent(typeof(ItemManager))]
    [RequireComponent(typeof(InventorySettingsManager))]
    [AddComponentMenu("InventorySystem/Managers/InventoryManager")]
    public partial class InventoryManager : MonoBehaviour
    {
        #region Variables

        public InventoryUI inventory;
        public SkillbarUI skillbar;
        public BankUI bank;
        public CharacterUI character;
        public LootUI loot;
        public VendorUI vendor;
        public NoticeUI notice;
        public CraftingWindowStandardUI craftingStandard;
        // Add crafting -> layout
        public InventoryLangDatabase lang; // All languages, notifications, stuff like that.


        /// <summary>
        /// The parent holds all collection's objects to keep the scene clean.
        /// </summary>
        public Transform collectionObjectsParent { get; private set; } 

        /// <summary>
        /// Collections such as the Inventory are used to loot items.
        /// When an item is picked up the item will be moved to the inventory. You can create multiple Inventories and limit types per inventory.
        /// </summary>
        private static List<InventoryCollectionLookup> lootToCollections = new List<InventoryCollectionLookup>(4);
        private static List<InventoryCollectionLookup> equipToCollections = new List<InventoryCollectionLookup>(4);
        private static List<ItemCollectionBase> bankCollections = new List<ItemCollectionBase>(4);

        #endregion
    

        private static InventoryManager _instance;
        public static InventoryManager instance {
            get {
                return _instance;
            }
        }


        public void Awake()
        {
            _instance = this;
            collectionObjectsParent = new GameObject("__COLLECTION_OBJECTS").transform;
        }


        protected virtual InventoryCollectionLookup GetBestLootCollectionForItem(InventoryItemBase item)
        {
            InventoryCollectionLookup best = null;

            foreach (var lookup in lootToCollections)
            {
                if (lookup.collection.CanAddItem(item))
                {
                    if (best == null)
                        best = lookup;
                    else if (lookup.priority > best.priority)
                        best = lookup;
                }
            }

            return best;
        }

        /// <summary>
        /// Get the item count of all items in the lootable collections.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns>Item count in all lootable collections.</returns>
        public static uint GetItemCount(uint itemID, bool checkBank)
        {
            uint count = 0;
            foreach (var collection in lootToCollections)
                count += collection.collection.GetItemCount(itemID);

            if(checkBank)
            {
                foreach (var collection in bankCollections)
                    count += collection.GetItemCount(itemID);
            }

            return count;
        }

        /// <summary>
        /// Get the first item from all lootable collections.
        /// </summary>
        /// <param name="itemID">ID of the object your searching for</param>
        /// <returns></returns>
        public static InventoryItemBase Find(uint itemID, bool checkBank)
        {
            foreach (var col in lootToCollections)
            {
                var item = col.collection.Find(itemID);
                if(item != null)
                    return item;   
            }

            if(checkBank)
            {
                foreach (var col in bankCollections)
                {
                    var item = col.Find(itemID);
                    if (item != null)
                        return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Get all items with a given ID
        /// </summary>
        /// <param name="itemID">ID of the object your searching for</param>
        /// <returns></returns>
        public static List<InventoryItemBase> FindAll(uint itemID, bool checkBank)
        {
            var list = new List<InventoryItemBase>(8);
            foreach (var col in lootToCollections)
            {
                // Linq.Concat doesn't seem to work.. :/
                foreach (var item in col.collection.FindAll(itemID))
                {
                    list.Add(item);
                }
            }
        
            if(checkBank)
            {
                foreach (var col in bankCollections)
                {
                    // Linq.Concat doesn't seem to work.. :/
                    foreach (var item in col.FindAll(itemID))
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Add an item to an inventory.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns></returns>
        public static bool AddItem(InventoryItemBase item, bool repaint = true)
        {
            //if(item.currentStackSize > item.maxStackSize)
            //{
            //    // Stack is to large, split it up
            //    var list = new List<InventoryItemBase>(Mathf.RoundToInt(Mathf.Ceil(item.currentStackSize / item.maxStackSize)));
            //    uint counter = item.currentStackSize;
            //    while(counter > item.maxStackSize)
            //    {
            //        counter -= item.currentStackSize;

            //        var copy = GameObject.Instantiate<InventoryItemBase>(item);
            //        copy.currentStackSize = item.maxStackSize;
            //        list.Add(copy);
            //    }

            //    item.currentStackSize = counter;
            //    list.Add(item); // Remainder

            //    return AddItems(list, false, repaint);
            //}

            var best = instance.GetBestLootCollectionForItem(item);

            if (best != null)
            {
                return best.collection.AddItem(item, repaint);
            }

            InventoryManager.instance.lang.collectionFull.Show(item.name, item.description, instance.inventory.collectionName);
            return false;
        }

        /// <summary>
        /// Add items to an inventory.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns></returns>
        public static bool AddItems(IEnumerable<InventoryItemBase> items, bool storeAsMuchAsPossible, bool repaint = true)
        {
            var toDict = new Dictionary<ItemCollectionBase, List<InventoryItemBase>>();

            foreach (var item in items)
            {
                var best = instance.GetBestLootCollectionForItem(item);
                if(best != null)
                {
                    if (toDict.ContainsKey(best.collection) == false)
                        toDict.Add(best.collection, new List<InventoryItemBase>());

                    toDict[best.collection].Add(item);
                }
                else if (storeAsMuchAsPossible == false)
                {
                    InventoryManager.instance.lang.collectionFull.Show(item.name, item.description, instance.inventory.collectionName);
                    return false; // Not all items can be stored.
                }
            }

            // Collection is filled
            foreach (var item in toDict)
            {
                item.Key.AddItems(item.Value, repaint);
            }
        
            return true;
        }

        /// <summary>
        /// Add an item to an inventory and remove it from the collection it was previously in.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns></returns>
        public static bool AddItemAndRemove(InventoryItemBase item, bool repaint = true)
        {
            var best = instance.GetBestLootCollectionForItem(item);

            if (best != null)
            {
                return best.collection.AddItemAndRemove(item, repaint);
            }

            InventoryManager.instance.lang.collectionFull.Show(item.name, item.description, instance.inventory.collectionName);
            return false;
        }

        public static bool CanAddItem(InventoryItemBase item)
        {
            foreach (var lookup in lootToCollections)
            {
                if (lookup.collection.CanAddItem(item))
                {
                    return true;
                }
            }

            return false;
        }


        public static void RemoveItem(uint itemID, uint amount, bool checkBank)
        {
            var allItems = FindAll(itemID, checkBank); // All the items in all looting collections
            uint itemsRemovedSoFar = 0; // Counts the items removed from the collections so far...
        
            foreach (var singleItem in allItems)
            {
                if (itemsRemovedSoFar + singleItem.currentStackSize <= amount)
                {
                    // Take some of the stack or all if it's available

                    singleItem.itemCollection.SetItem(singleItem.index, null);
                    singleItem.itemCollection.NotifyItemRemoved(singleItem.ID, singleItem.index, (uint)singleItem.currentStackSize);
                    singleItem.itemCollection[singleItem.index].Repaint();

                    Destroy(singleItem.gameObject); // Item is no longer needed
                    itemsRemovedSoFar += singleItem.currentStackSize;
                }
                else if (itemsRemovedSoFar < amount)
                {
                    // Remove that's left

                    // Going over, take just a few of the stack
                    uint toRemove = amount - itemsRemovedSoFar;
                    singleItem.currentStackSize -= toRemove;
                    singleItem.itemCollection[singleItem.index].Repaint();
                    itemsRemovedSoFar += toRemove;
                    break; // We're done our stack is complete
                }
            }
        }


        /// <summary>
        /// Add a collection that functions as an Inventory. Items will be looted to this collection.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        /// <param name="priority">
        /// How important is the collection, if you 2 collections can hold the item, which one should be chosen?
        /// Range of 0 to 100
        /// </param>
        public static void AddInventoryCollection(ItemCollectionBase collection, int priority)
        {
            lootToCollections.Add(new InventoryCollectionLookup(collection, priority));
        }


        /// <summary>
        /// Check if a given collection is a loot to collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsInventoryCollection(ItemCollectionBase collection)
        {
            foreach (var col in lootToCollections)
            {
                if (col.collection == collection)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a collection that functions as an Equippable collection. Items can be equipped to this collection.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        /// <param name="priority">
        /// How important is the collection, if you 2 collections can hold the item, which one should be chosen?
        /// Range of 0 to 100
        /// 
        /// Note: This method is not used yet, it only registers the Equippable collection, that's it.
        /// </param>
        public static void AddEquipCollection(ItemCollectionBase collection, int priority)
        {
            equipToCollections.Add(new InventoryCollectionLookup(collection, priority));
        }


        public static void AddBankCollection(ItemCollectionBase collection)
        {
            bankCollections.Add(collection);
        }

        /// <summary>
        /// Get all bank collections
        /// I casted it to an array (instead of list) to avoid messing with the internal list.
        /// </summary>
        /// <returns></returns>
        public static ItemCollectionBase[] GetBankCollections()
        {
            return bankCollections.ToArray();
        }

        public static ItemCollectionBase[] GetLootToCollections()
        {
            var l = new List<ItemCollectionBase>(lootToCollections.Count);
            foreach (var item in lootToCollections)
                l.Add(item.collection);

            return l.ToArray();
        }
    }
}