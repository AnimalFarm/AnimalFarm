using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Devdog.InventorySystem.UI.Models;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Devdog.InventorySystem
{
    [RequireComponent(typeof(UIWindow))]
    [AddComponentMenu("InventorySystem/Windows Other/Infobox")]
    public partial class InfoBox : MonoBehaviour
    {
        public partial class Row
        {
            /// <summary>
            /// Title of the label.
            /// </summary>
            public string title;
            public Color titleColor;
        
            /// <summary>
            /// Text of the label.
            /// </summary>
            public string text;
            public Color textColor;
        
        
            public Row()
            {
        
            }

            public Row(string title, Color color)
                : this(title, string.Empty, color, Color.white)
            { }

            public Row(string title, string text)
                : this(title, text, Color.white, Color.white)
            { }
        
            public Row(string title, string text, Color titleColor, Color textColor)
            {
                this.title = title;
                this.text = text;
                this.titleColor = titleColor;
                this.textColor = textColor;
            }
        }

        /// <summary>
        /// All information will be appended to the container.
        /// </summary>
        public RectTransform container;

        /// <summary>
        /// When the InfoBox hits the right or left part of the screen it will move to the other side.
        /// </summary>
        public bool moveWhenHitBorderHorizontal = true;

        /// <summary>
        /// When the InfoBox hits the top or bottom part of the screen it will move to the other side.
        /// </summary>
        public bool moveWhenHitBorderVertical = true;

        // Default fields
        public Image uiIcon;
        public Text uiName;
        public Text uiDescription;

        /// <summary>
        /// Used to define extra margin on the corners of the screen.
        /// If the item falls of the screen it will be shown on the other side of the cursor.
        /// </summary>
        public Vector2 cornerMargins;

        /// <summary>
        /// Height used between lines of text.
        /// </summary>
        public float lineHeight = 20.0f;

        /// <summary>
        /// The title text width.
        /// </summary>
        public float titleTextWidth = 90.0f;

        public GameObject infoBoxCategory;
        public GameObject separatorPrefab;
        public InfoBoxRowUI infoBoxRowPrefab; // 1 item (row) inside the infobox

        [NonSerialized]
        private RectTransform currentTransform;

        [NonSerialized]
        private Vector2 defaultPivot;

        // Used to avoid continous repainting.
        [NonSerialized]
        protected InventoryUIItemWrapper cacheItem;

        [NonSerialized]
        private UIWindow window;

        [NonSerialized]
        private InventoryPool<InfoBoxRowUI> pool;
        
        [NonSerialized]
        private InventoryPool poolObjs;

        [NonSerialized]
        private InventoryPool poolCategoryBoxes;


        void Awake()
        {
            currentTransform = GetComponent<RectTransform>();
            defaultPivot = currentTransform.pivot;
            window = GetComponent<UIWindow>();

            pool = new InventoryPool<InfoBoxRowUI>(infoBoxRowPrefab, 32);
            poolObjs = new InventoryPool(separatorPrefab, 8);
            poolCategoryBoxes = new InventoryPool(infoBoxCategory, 8);
        }

        public virtual void Update()
        {
            // If the item is no longer visible but still hovering
            if (InventoryUIUtility.hoveringItem != null && InventoryUIUtility.hoveringItem.gameObject.activeInHierarchy == false)
                InventoryUIUtility.ExitItem(InventoryUIUtility.hoveringItem, InventoryUIUtility.hoveringItem.index, InventoryUIUtility.hoveringItem.itemCollection, null);

            if (InventoryUIUtility.hoveringItem != null && InventoryUIUtility.hoveringItem.item != null)
            {

                #region Handling borders

                if(InventorySettingsManager.instance.isUIWorldSpace == false)
                {
                    if (moveWhenHitBorderHorizontal)
                    {
                        // Change the box if its about to fall of the screen
                        if (Input.mousePosition.x + currentTransform.sizeDelta.x > Screen.width - cornerMargins.x)
                        {
                            // Falls of the right
                            currentTransform.pivot = new Vector2(defaultPivot.y, currentTransform.pivot.x); // Swap
                        }
                        else
                        {
                            currentTransform.pivot = new Vector2(defaultPivot.x, currentTransform.pivot.y); // Swap
                        }
                    }
                    if (moveWhenHitBorderVertical)
                    {
                        if (Input.mousePosition.y - currentTransform.sizeDelta.y < 0.0f - cornerMargins.y)
                        {
                            // Falls of the bottom
                            currentTransform.pivot = new Vector2(currentTransform.pivot.x, defaultPivot.x); // Swap                
                        }
                        else
                        {
                            currentTransform.pivot = new Vector2(currentTransform.pivot.x, defaultPivot.y); // Swap
                        }
                    }
                }           

                #endregion

                if (InventorySettingsManager.instance.isUIWorldSpace)
                {
                    var r = InventorySettingsManager.instance.guiRoot.GetComponent<UnityEngine.UI.GraphicRaycaster>();
                    var l = new List<RaycastResult>();
                    Vector3 p = Input.mousePosition;
                    if (Application.isMobilePlatform && Input.touchCount > 0)
                        p = Input.GetTouch(0).position;

                    r.Raycast(new PointerEventData(null) { position = p, pressPosition = p }, l);
                    p = Camera.main.ScreenToWorldPoint(p);
                    if(l.Count > 0)
                    {
                        transform.position = new Vector3(p.x, p.y, l[0].gameObject.transform.position.z);
                    }
                }
                else
                    transform.position = Input.mousePosition;


                if (InventoryUIUtility.hoveringItem != cacheItem)
                {
                    cacheItem = InventoryUIUtility.hoveringItem;
                    Repaint(cacheItem, cacheItem.item.GetInfo());
                }

                //if (lastWindow != null)
                //{
                //    if (lastWindow.animator.enabled || lastWindow.isVisible == false)
                //        return;
                //}

                if (window.isVisible == false)
                {
                    window.Show();
                }
            }
            else
            {
                if(window.isVisible)
                    window.Hide();
            }
        }


        /// <summary>
        /// Repaint the infobox with the given data.
        /// </summary>
        /// <param name="data">
        /// An array of all the rows and it's data to be displayed.
        /// </param>
        public virtual void Repaint(InventoryUIItemWrapper item, LinkedList<InfoBox.Row[]> rows)
        {
            pool.DestroyAll();
            poolObjs.DestroyAll();
            poolCategoryBoxes.DestroyAll();

            // The usual stuff
            uiIcon.sprite = item.item.icon;
            uiName.text = item.item.name;
            uiName.color = (item.item.rarity != null) ? item.item.rarity.color : uiName.color;
            uiDescription.text = item.item.description;

            int i = 0;        
            foreach (var box in rows)
            {
                i++;

                var boxObj = poolCategoryBoxes.Get();
                //var boxLayout = boxObj.AddComponent<VerticalLayoutGroup>();
                //boxLayout.childForceExpandHeight = false;
                //boxLayout.childForceExpandWidth = true;

                foreach (var row in box)
                {
                    var rowObj = pool.Get();
                    //var rowObj = GameObject.Instantiate<InfoBoxRowUI>(infoBoxRowPrefab);
                    rowObj.transform.SetParent(boxObj.transform);

                    rowObj.title.text = row.title;
                    rowObj.title.color = row.titleColor;

                    rowObj.message.text = row.text;
                    rowObj.message.color = row.textColor;
                }

                boxObj.transform.SetParent(container);
                boxObj.transform.localScale = Vector3.one;
                boxObj.transform.localPosition = new Vector3(boxObj.transform.localPosition.x, boxObj.transform.localPosition.y, 0.0f);

                if(i < rows.Count && separatorPrefab != null)
                {
                    // Add a separator
                    if (separatorPrefab != null)
                    {
                        var separator = poolObjs.Get();
                        //var separator = GameObject.Instantiate<GameObject>(separatorPrefab);
                        separator.transform.SetParent(container);
                        separator.transform.localScale = Vector3.one;
                        separator.transform.localPosition = new Vector3(separator.transform.localPosition.x, separator.transform.localPosition.y, 0.0f);
                    }
                }
            }
        }
    }
}