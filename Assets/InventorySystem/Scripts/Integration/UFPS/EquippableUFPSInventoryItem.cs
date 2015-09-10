#if UFPS

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Integration.UFPS
{
    public partial class EquippableUFPSInventoryItem : EquippableInventoryItem
    {
        public bool useUFPSItemData = true;
        public vp_ItemType itemType;
        
        protected vp_PlayerEventHandler eventHandler
        {
            get
            {
                return InventorySettingsManager.instance.playerObject.GetComponent<vp_PlayerEventHandler>();
            }
        }

        public override string name
        {
            get
            {
                if (useUFPSItemData && itemType != null)
                    return itemType.DisplayName;
                else
                    return base.name;
            }
            set { base.name = value; }
        }

        public override string description
        {
            get
            {
                if (useUFPSItemData && itemType != null)
                    return itemType.Description;
                else
                    return base.description;
            }
            set { base.description = value; }
        }

        public override LinkedList<InfoBox.Row[]> GetInfo()
        {
            var basic = base.GetInfo();
            basic.Remove(basic.First.Next);
            basic.AddAfter(basic.First, new InfoBox.Row[]
            {
                new InfoBox.Row("Ammo", "abc")
            });


            return basic;
        }

        protected override bool Equip(InventoryEquippableField equipSlot)
        {
            bool equipped = base.Equip(equipSlot);
            if (equipped == false)
                return false;

            bool added = eventHandler.AddItem.Try(new object[] { itemType });
            
            return added;
        }

        protected override bool Unequip()
        {
            bool unequipped = base.Unequip();
            if (unequipped == false)
                return false;

            bool removed = eventHandler.RemoveItem.Try(new object[] { itemType });
            
            return removed;
        }
    }
}

#else

using UnityEngine;
using System.Collections;
using Devdog.InventorySystem;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Integration.UFPS
{
    public class EquippableUFPSInventoryItem : EquippableInventoryItem
    {
        // No UFPS, No fun stuff...
    }
}

#endif
