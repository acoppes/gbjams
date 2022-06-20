using Gemserk.Leopotam.Ecs.Gameplay;

public class ProjectileController : ControllerBase
{
    public float damage;
    
    public override void OnUpdate(float dt)
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