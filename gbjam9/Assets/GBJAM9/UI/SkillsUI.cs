using System;
using System.Collections.Generic;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM9.UI
{
    public class SkillsUI : MonoBehaviour
    {
        public List<Image> weaponImages;
        
        public Image weaponCooldown;

        public Image dashCooldown;

        [NonSerialized]
        public Entity entity;

        private void LateUpdate()
        {
            if (entity == null)
            {
                return;
            }
            
            if (entity.attack.weaponData != null)
            {
                var attackType = entity.attack.weaponData.attackType.ToLowerInvariant();
                
                foreach (var weaponImage in weaponImages)
                {
                    weaponImage.enabled = weaponImage.name.ToLowerInvariant().Contains(attackType);
                }
                
                weaponCooldown.fillAmount = entity.attack.cooldown / entity.attack.weaponData.cooldown;
            }
            else
            {
                foreach (var weaponImage in weaponImages)
                {
                    weaponImage.enabled = false;
                }
                weaponCooldown.fillAmount = 0;
            }
            
            dashCooldown.fillAmount = entity.dash.cooldownCurrent / entity.dash.cooldown;
        }

        public void SetAbilities(float mainAbilityCooldown, float secondaryAbilityCooldown)
        {
            weaponCooldown.fillAmount = 1.0f - mainAbilityCooldown;
            dashCooldown.fillAmount = 1.0f - secondaryAbilityCooldown;
        }
    }
}