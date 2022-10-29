using System;
using UnityEngine;
using UnityEngine.UI;

namespace Beatemup.UI
{
    public class HealthSlotUI : MonoBehaviour
    {
        public Image background;
        public Image fill;

        [NonSerialized]
        public float fillAmount = 1;

        private void LateUpdate()
        {
            fill.fillAmount = fillAmount;
        }
    }
}