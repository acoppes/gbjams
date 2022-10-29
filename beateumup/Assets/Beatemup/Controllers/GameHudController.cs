using Beatemup.Ecs;
using Beatemup.UI;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class GameHudController : ControllerBase, IInit
    {
        private static readonly int visibleHash = Animator.StringToHash("visible");

        private HealthUI heroHealthUI;
        private HealthUI bossHealthUI;
        private WeaponUI weaponUI;

        public void OnInit()
        {
            ref var modelComponent = ref world.GetComponent<UnitModelComponent>(entity);

            var instance = modelComponent.instance;
        
            var healthObject = instance.transform.Find("Canvas/Hero_Health");
        
            if (healthObject != null)
            {
                heroHealthUI = healthObject.GetComponent<HealthUI>();
            }
        
            healthObject = instance.transform.Find("Canvas/Boss");

            if (healthObject != null)
            {
                bossHealthUI = healthObject.GetComponent<HealthUI>();
            }

            var weaponObject = instance.transform.FindInHierarchy("Hero_Weapon");
            if (weaponObject != null)
            {
                weaponUI = weaponObject.GetComponent<WeaponUI>();
            }
        }
    
        public override void OnUpdate(float dt)
        {
            ref var modelComponent = ref world.GetComponent<UnitModelComponent>(entity);

            var instance = modelComponent.instance;
            var animator = instance.GetComponent<Animator>();

            var mainCharacterEntity = world.GetEntityByName("Main_Character");
            var bossEntity = world.GetEntityByName("Main_Enemy");
        
            // if (mainCharacterEntity == Entity.NullEntity)
            // {
            //     animator.SetBool(visibleHash, false);
            //     return;
            // }

            if (heroHealthUI != null)
            { 
                if (mainCharacterEntity != Entity.NullEntity)
                {
                    var healthComponent = world.GetComponent<HealthComponent>(mainCharacterEntity);
                    heroHealthUI.SetHealth(healthComponent.current, healthComponent.total);
                }
            }

            if (bossHealthUI != null)
            {
                if (bossEntity != Entity.NullEntity)
                {
                    var healthComponent = world.GetComponent<HealthComponent>(bossEntity);
                    bossHealthUI.SetHealth(healthComponent.current, healthComponent.total);
                }
            }

            if (weaponUI != null)
            {
                if (mainCharacterEntity != Entity.NullEntity)
                {
                    var specialWeapon = world.GetComponent<SpecialWeaponComponent>(mainCharacterEntity);
                    weaponUI.UpdateWeapon(specialWeapon.special);
                }
                
            }
            
            animator.SetBool(visibleHash, modelComponent.IsVisible);
        }


    }
}