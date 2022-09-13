using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace GBJAM10.UI
{
    public class SkillsUI : MonoBehaviour
    {
        public List<Image> weaponImages;
        
        public Image weaponCooldown;

        public Image dashCooldown;

        [NonSerialized]
        public Entity entity;

        public void SetAbilities(float mainAbilityCooldown, float secondaryAbilityCooldown)
        {
            weaponCooldown.fillAmount = 1.0f - mainAbilityCooldown;
            dashCooldown.fillAmount = 1.0f - secondaryAbilityCooldown;
        }
    }
}