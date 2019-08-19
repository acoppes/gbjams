﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM7.Scripts
{
    public class MenuOption : MonoBehaviour
    {
        public Text nameText;
        public Image selector;
        
        [NonSerialized]
        public bool selected;

        [NonSerialized]
        public Option option;

        private void LateUpdate()
        {
            nameText.text = option.name;
            selector.enabled = selected;
        }
    }
}
