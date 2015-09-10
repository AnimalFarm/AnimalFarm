﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.InventorySystem.Models
{
    [System.Serializable]
    public partial class InventoryMessage
    {
        public string title;
        public string message;

        public System.Object[] parameters;

        /// <summary>
        /// Required for PlayMaker...
        /// </summary>
        public InventoryMessage()
        { }

        public InventoryMessage(string title, string message, params System.Object[] parameters)
        {
            this.title = title;
            this.message = message;
            this.parameters = parameters;
        }


        public virtual void Show(params System.Object[] parameters)
        {
            if (parameters.Length > 0)
                this.parameters = parameters;

            // Not much going on here...
        }
    }
}