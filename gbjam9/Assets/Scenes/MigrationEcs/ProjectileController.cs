using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IController
{
    public float damage;
    
    public void OnUpdate(float dt, World world, Entity entity)
    {
        ref var health = ref world.GetComponent<HealthComponent>(entity);
            
        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var damageOnImpact = abilities.GetTargeting("DamageOnImpact");

        if (damageOnImpact.targets.Count > 0)
        {
            var target = damageOnImpact.targets[0];

            ref var targetHealth = ref world.GetComponent<HealthComponent>(target.entity);
            targetHealth.pendingDamages.Add(new Damage
            {
                value = damage
            });
            health.deathRequest = true;
        }
    }
}