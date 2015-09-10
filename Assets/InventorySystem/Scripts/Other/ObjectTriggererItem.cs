using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem.Dialogs;

namespace Devdog.InventorySystem
{
    /// <summary>
    /// Used to trigger item pickup, modify the settings in ShowObjectTriggerer.
    /// </summary>
    [AddComponentMenu("InventorySystem/Triggers/Object triggerer item")]
    [RequireComponent(typeof(InventoryItemBase))]
    [RequireComponent(typeof(Rigidbody))]
    public partial class ObjectTriggererItem : MonoBehaviour
    {

        public InventoryItemBase item { get; protected set; }

        public bool inRange
        {
            get
            {
                return Vector3.Distance(InventorySettingsManager.instance.playerObject.transform.position, transform.position) < InventorySettingsManager.instance.useObjectDistance;
            }
        }


        public void Awake()
        {
            item = GetComponent<InventoryItemBase>();            
        }

        public virtual void OnMouseDown()
        {
            if (ShowObjectTriggerer.instance.itemTriggerMouseClick && InventoryUIUtility.clickedUIElement == false)
            {
                if (inRange)
                    Use();
                else
                {
                    InventoryManager.instance.lang.itemCannotBePickedUpToFarAway.Show(item.name, item.description);
                }
            }
        }

        //// For collider based character stuff
        //public virtual void OnCollisionEnter(Collision col)
        //{
        //    if (ShowObjectTriggerer.instance.itemTriggerOnPlayerCollision)
        //    {
        //        if (col.transform.IsChildOf(InventorySettingsManager.instance.playerObject.transform) ||
        //            col.transform == InventorySettingsManager.instance.playerObject.transform)
        //        {
        //            // Collided with player
        //            Use();
        //        }
        //    }
        //}

        protected virtual void Use()
        {
            item.PickupItem();
        }
    }
}