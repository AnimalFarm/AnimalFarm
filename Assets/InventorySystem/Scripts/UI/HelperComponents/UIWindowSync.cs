using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventorySystem;
using UnityEngine;


namespace Devdog.InventorySystem
{
    public partial class UIWindowSync : MonoBehaviour
    {

        public UIWindow[] otherWindows = new UIWindow[0];

        private UIWindow window;

        public void Awake()
        {
            window = GetComponent<UIWindow>();

            window.OnHide += () =>
            {
                foreach (var w in otherWindows)
                {
                    if(w.isVisible)
                        w.Hide();
                }
            };

            window.OnShow += () =>
            {
                foreach (var w in otherWindows)
                {
                    if(w.isVisible == false)
                        w.Show();
                }
            };
        }

    }    
}
