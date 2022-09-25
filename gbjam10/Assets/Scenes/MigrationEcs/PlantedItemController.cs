using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;

public class PlantedItemController : ControllerBase
{
    public float damage;
    
    public override void OnUpdate(float dt)
    {
        ref var health = ref world.GetComponent<HealthComponent>(entity);
        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var damageOnImpact = abilities.GetTargeting("DamageOnImpact");

        foreach (var target in damageOnImpact.targets)
        {
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
    }
}