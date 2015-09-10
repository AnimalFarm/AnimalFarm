using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Devdog.InventorySystem
{
    /// <summary>
    /// A page inside an UIWindow. When a tab is clicked the insides of the window are changed, this is a page.
    /// </summary>
    [AddComponentMenu("InventorySystem/UI Helpers/UIWindowPage")]
    public partial class UIWindowPage : UIWindow
    {
        public bool isDefaultPage = true;

        /// <summary>
        /// The button that "triggers" this page. leave empty if there is no button.
        /// </summary>
        public UnityEngine.UI.Button myButton;

        [SerializeField]
        protected bool _isEnabled = true;
        public bool isEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;

                if(isEnabled)
                {
                    myButton.enabled = false;

                }
                else
                {
                    Hide();
                    myButton.enabled = false;
                }
            }
        }

        /// <summary>
        /// Container that olds the items, if any.
        /// </summary>
        public RectTransform itemContainer;

        [InventoryRequired]
        public UIWindow windowParent;

        public override void Awake()
        {
            base.Awake();

            if (transform.IsChildOf(windowParent.transform) == false)
                Debug.LogWarning("This window page is not a child of it's the given window, this is required.", gameObject);

            // Register our page with the window parent
            windowParent.AddPage(this);
        }

        public override void Show()
        {
            if(isEnabled == false)
            {
                Debug.LogWarning("Trying to show a disabled UIWindowPage");
                return;
            }

            base.Show();

            windowParent.NotifyPageShown(this);
        }

        public override void HideFirst()
        {
            isVisible = false;
            SetChildrenActive(false);
        }
    }
}