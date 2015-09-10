#if UFPS

using UnityEngine;
using System.Collections;
using Devdog.InventorySystem;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Integration.UFPS
{
    public partial class UnitTypeUFPSInventoryItem : InventoryItemBase
    {
        public vp_UnitType unitType;
        public uint unitAmount = 1;
        public AudioClip pickupSound;

        
        public bool addDirectlyToWeapon = true;

        protected vp_PlayerEventHandler eventHandler
        {
            get
            {
                return InventorySettingsManager.instance.playerObject.GetComponent<vp_PlayerEventHandler>();
            }
        }

        public override int Use()
        {
            int used = base.Use();
            if (used < 0)
                return used;

            bool added = AddToUFPSAmmo();
            if (added)
            {
                currentStackSize = 0; // None left :(
                NotifyItemUsed(currentStackSize);
                return (int)currentStackSize;
            }

            return 0; // 0 we're used, UFPS rejected the ammo
        }

        public override bool PickupItem(bool addToInventory = true)
        {
            currentStackSize = unitAmount;
            if (addDirectlyToWeapon)
            {
                // Add bullets directly to weapon
                //unitType.Space 
                bool added = AddToUFPSAmmo();
                if (added)
                {
                    if (pickupSound != null)
                        InventoryUIUtility.AudioPlayOneShot(pickupSound);
                }
            }
            else
            {
                bool pickedup = base.PickupItem(addToInventory); // Add to inventory instead.
                if (pickedup)
                {
                    if (pickupSound != null)
                        InventoryUIUtility.AudioPlayOneShot(pickupSound);

                    return true;
                }
            }
            
            return false;
        }

        protected virtual bool AddToUFPSAmmo()
        {
            bool added = eventHandler.AddItem.Try(new object[] { unitType, (int)currentStackSize });
            if (added)
            {
                Destroy(gameObject); // No longer need it
                return true;
            }

            return false;
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
    public class UnitTypeUFPSInventoryItem : InventoryItemBase
    {
        // No UFPS, No fun stuff...
    }
}

#endif
