using System;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM9.UI
{
    public class HealthSlotUI : MonoBehaviour
    {
        public Image background;
        public Image fill;

        [NonSerialized]
        public bool isFilled;

        private void LateUpdate()
        {
            fill.enabled = isFilled;
        }
    }
}