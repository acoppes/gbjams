using System.Collections.Generic;
using UnityEngine;

namespace Beatemup.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject healthSlotPrefab;

        public RectTransform container;

        private List<HealthSlotUI> healthSlots = new List<HealthSlotUI>();

        public float factor = 1.0f;

        public void SetHealth(float current, float total)
        {
            var newCurrent = current * factor;
            var newTotal = total * factor;

            var totalInt = Mathf.RoundToInt(newTotal);
            var currentInt = Mathf.RoundToInt(newCurrent);
            
            // regenerate slots if different total
            if (healthSlots.Count != totalInt)
            {
                // regenerate all sub 
                foreach (var healthSlot in healthSlots)
                {
                    Destroy(healthSlot.gameObject);
                }
                
                healthSlots.Clear();

                for (var i = 0; i < totalInt; i++)
                {
                    var healthSlotObject = Instantiate(healthSlotPrefab, container);
                    healthSlots.Add(healthSlotObject.GetComponent<HealthSlotUI>());
                }
            }
            
            for (var i = 0; i < healthSlots.Count; i++)
            {
                if (i == Mathf.FloorToInt(newCurrent))
                {
                    var difference = newCurrent - i;
                    healthSlots[i].fillAmount = difference;
                }
                else
                {
                    healthSlots[i].fillAmount = i < newCurrent ? 1 : 0;
                }
            }
        }
    }
}
