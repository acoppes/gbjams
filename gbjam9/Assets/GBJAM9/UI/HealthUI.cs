using System;
using System.Collections.Generic;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject healthSlotPrefab;

        public RectTransform container;

        private List<HealthSlotUI> healthSlots = new List<HealthSlotUI>();

        private int current;
        
        public void SetHealth(HealthComponent health)
        {
            current = health.current;
            
            // regenerate slots if different total
            if (healthSlots.Count != health.total)
            {
                // regenerate all sub 

                foreach (var healthSlot in healthSlots)
                {
                    Destroy(healthSlot.gameObject);
                }
                
                healthSlots.Clear();

                for (var i = 0; i < health.total; i++)
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
