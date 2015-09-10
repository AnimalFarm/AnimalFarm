using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem.Dialogs;

namespace Devdog.InventorySystem
{
    /// <summary>
    /// Used to trigger a physical object such as vendor, treasure chests etc.
    /// </summary>
    [AddComponentMenu("InventorySystem/Triggers/Object triggerer")]
    public partial class ObjectTriggerer : MonoBehaviour
    {
        #region Events

        public delegate void TriggerUse();
        public delegate void TriggerUnUse();

        public event TriggerUse OnTriggerUse;
        public event TriggerUnUse OnTriggerUnUse;

        #endregion

        /// <summary>
        /// When the item is clicked, should it trigger?
        /// </summary>
        [Header("Triggers")]
        public bool triggerMouseClick = true;

        /// <summary>
        /// When the item is hovered over (center of screen) and a certain key is tapped, should it trigger?
        /// </summary>
        public KeyCode triggerHoverKeyCode = KeyCode.None;

        /// <summary>
        /// When true the window will be triggered directly, if false, a 2nd party will have to handle it through events.
        /// </summary>
        [HideInInspector]
        [NonSerialized]
        public bool handleWindowDirectly = true;

        /// <summary>
        /// Toggle when triggered
        /// </summary>
        public bool toggleWhenTriggered = true;

        /// <summary>
        /// Only required if handling the window directly
        /// </summary>
        [Header("The window")]
        public UIWindow window;

        [Header("Animations & Audio")]
        public AnimationClip closeAnimation;
        public AnimationClip openAnimation;

        public AudioClip openAudioClip;
        public AudioClip closeAudioClip;


        public Animator animator { get; protected set; }

        public bool isOpen { get; protected set; }


        public bool inRange
        {
            get
            {
                return Vector3.Distance(InventorySettingsManager.instance.playerObject.transform.position, transform.position) < InventorySettingsManager.instance.useObjectDistance;
            }
        }


        public void Awake()
        {
            animator = GetComponent<Animator>();

            if (window != null)
            {
                window.OnHide += () =>
                {
                    Hide();
                };                
            }

            StartCoroutine(DistanceCheck());
        }

        //public void Update()
        //{
        //    if (triggerKeyCode)
        //    {
        //        if (Input.GetKeyDown(triggerHoverKeyCode))
        //        {
        //            // Raycast from center of screen
        //            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward * InventorySettingsManager.instance.useObjectDistance), Color.red, 1.0f, true);

        //            RaycastHit hit;
        //            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, InventorySettingsManager.instance.useObjectDistance))
        //            {
        //                var colliders = hit.transform.GetComponentsInChildren<Collider>();
        //                foreach (var col in colliders)
        //                {
        //                    if (col.transform.IsChildOf(transform))
        //                    {
        //                        if ((window != null && window.isVisible) && toggleWhenTriggered && isOpen)
        //                            Hide();
        //                        else
        //                            Show();

        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// An infinite loop that checks the distance to the object every 0.5 seconds.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DistanceCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);

                if (inRange == false)
                {
                    Hide();
                }
            }
        }

        public virtual void Show()
        {
            if (isOpen)
                return;

            if (handleWindowDirectly)
            {
                if(toggleWhenTriggered)
                    window.Toggle();
                else if (window.isVisible == false)
                    window.Show();
            }

            if (openAnimation != null)
                animator.Play(openAnimation.name);

            if (openAudioClip != null)
                InventoryUIUtility.AudioPlayOneShot(openAudioClip);

            isOpen = true;

            if (OnTriggerUse != null)
                OnTriggerUse();
        }

        public virtual void OnMouseDown()
        {
            if (triggerMouseClick && InventoryUIUtility.clickedUIElement == false && inRange)
            {
                if (toggleWhenTriggered)
                    Toggle();
                else
                    Show();
            }
        }

        public virtual void Toggle()
        {
            if (window != null && window.isVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public virtual void Hide()
        {
            if (isOpen == false)
                return;

            if (handleWindowDirectly)
            {
                if (window.isVisible)
                    window.Hide();
            }

            if (closeAnimation != null && animator != null)
                animator.Play(closeAnimation.name);

            if (closeAudioClip != null)
                InventoryUIUtility.AudioPlayOneShot(closeAudioClip);

            isOpen = false;

            if (OnTriggerUnUse != null)
                OnTriggerUnUse();
        }
        
    }
}