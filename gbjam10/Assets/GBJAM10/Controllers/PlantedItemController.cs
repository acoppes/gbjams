using GBJAM10.Definitions;
using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class PlantedItemController : ControllerBase
    {
        public float damage;

        public GameObject explosionVfxDefinition;
    
        public override void OnUpdate(float dt)
        {
            ref var health = ref world.GetComponent<HealthComponent>(entity);

            if (health.deathRequest)
            {
                return;
            }
        
            ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
            var damageOnImpact = abilities.GetTargeting("DamageOnImpact");

            foreach (var target in damageOnImpact.targets)
            {
                if (target.entity == Entity.NullEntity)
                    continue;

                if (world.HasComponent<UnitTypeComponent>(target.entity))
                {
                    var unitTypeComponent = world.GetComponent<UnitTypeComponent>(target.entity);
                    if ((unitTypeComponent.type & (int) UnitDefinition.UnitType.Unit) == 0)
                    {
                        continue;
                    }
                }
            
                ref var targetHealth = ref world.GetComponent<HealthComponent>(target.entity);
                targetHealth.pendingDamages.Add(new Damage
                {
                    value = damage
                });
                health.deathRequest = true;
            }

            if (health.deathRequest && explosionVfxDefinition != null)
            {
                var vfxEntity = world.CreateEntity(explosionVfxDefinition.GetInterface<IEntityDefinition>(), null);
                ref var vfxPosition = ref world.GetComponent<PositionComponent>(vfxEntity);
                vfxPosition.value = world.GetComponent<PositionComponent>(entity).value;
            }
        }
    }
}