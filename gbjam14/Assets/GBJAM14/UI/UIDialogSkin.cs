using System;
using Game.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM14.UI
{
    public class UIDialogSkin : MonoBehaviour
    {
        public RectTransform container;
        public TextView dialogTextView;
        // public Image[] portraits;
        public Image[] indicators;
        
        [NonSerialized]
        public bool active;

        private void LateUpdate()
        {
            container.gameObject.SetActive(active);
        }
    }
}