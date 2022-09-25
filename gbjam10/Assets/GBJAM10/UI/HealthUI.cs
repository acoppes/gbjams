using System.Collections.Generic;
using GBJAM10.Components;
using UnityEngine;

namespace GBJAM10.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject healthSlotPrefab;

        public RectTransform container;

        private List<HealthSlotUI> healthSlots = new List<HealthSlotUI>();

        public void SetHealth(HealthComponent health)
        {
            SetHealth(health.current, health.total);
        }
        
        public void SetHealth(float current, float total)
        {
            // regenerate slots if different total
            if (healthSlots.Count != total)
            {
                // regenerate all sub 
                foreach (var healthSlot in healthSlots)
                {
                    Destroy(healthSlot.gameObject);
                }
                
                healthSlots.Clear();

                for (var i = 0; i < total; i++)
                {
                    var healthSlotObject = Instantiate(healthSlotPrefab, container);
                    healthSlots.Add(healthSlotObject.GetComponent<HealthSlotUI>());
                }
            }
            
            for (var i = 0; i < healthSlots.Count; i++)
            {
                healthSlots[i].isFilled = i < current;
            }
        }
    }
}
