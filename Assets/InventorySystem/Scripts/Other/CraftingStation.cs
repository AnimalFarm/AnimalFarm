using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem
{
    /// <summary>
    /// A physical representation of a crafting station.
    /// </summary>
    [AddComponentMenu("InventorySystem/Triggers/Crafting station")]
    [RequireComponent(typeof(ObjectTriggerer))]
    public class CraftingStation : MonoBehaviour
    {
        public int categoryID = 0; // What category can we craft from?
        protected InventoryCraftingCategory category
        {
            get
            {
                return ItemManager.instance.craftingCategories[categoryID];
            }
        }

        [NonSerialized]
        protected UIWindow window;
        protected static CraftingStation currentCraftingStation;

        [NonSerialized]
        protected ObjectTriggerer triggerer;

        public void Awake()
        {
            window = InventoryManager.instance.craftingStandard.window;
            triggerer = GetComponent<ObjectTriggerer>();
            triggerer.window = window;
            triggerer.handleWindowDirectly = false; // We're in charge now :)

            window.OnHide += () =>
            {
                currentCraftingStation = null;
            };

            triggerer.OnTriggerUse += () =>
            {
                window.Toggle();

                if (window.isVisible)
                {
                    currentCraftingStation = this;
                    InventoryManager.instance.craftingStandard.SetCraftingCategory(category);
                }
            };
            triggerer.OnTriggerUnUse += () =>
            {
                if (window.isVisible && currentCraftingStation == this)
                    window.Hide();
            };
        }
    }
}