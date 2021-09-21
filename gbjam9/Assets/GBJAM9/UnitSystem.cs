using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM9
{
    public class UnitSystem : MonoBehaviour
    {
        [NonSerialized]
        public List<Unit> units = new List<Unit>();
        
        public void FixedUpdate()
        {
            // perform general logics in order
            var toDestroyUnits = new List<Unit>();

            foreach (var unit in units)
            {
                var receivedDamage = false;
                
                var health = unit.GetComponent<Health>();
                if (health != null)
                {
                    receivedDamage = health.damages > 0;
                    
                    if (receivedDamage)
                    {
                        health.current -= health.damages;
                        health.damages = 0;
                    }
                }
                
                // TODO: blink animation state
                
                
                
                if (health != null)
                {
                    if (health.current <= 0)
                    {
                        toDestroyUnits.Add(unit);
                    }
                }
            }

            foreach (var unit in toDestroyUnits)
            {
                Destroy(unit.gameObject);
            }
        }
    }
}