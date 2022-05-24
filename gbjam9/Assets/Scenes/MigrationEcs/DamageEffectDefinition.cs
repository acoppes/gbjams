using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;

public class DamageEffectDefinition : TargetEffectDefinition
{
    //  more interesting values?
    public float damage;
    
    public override void Apply(World world, int entity)
    {
        ref var targetEffects = ref world.GetComponent<TargetEffectsComponent>(entity);
        
        targetEffects.targetEffects.Add(new DamageTargetEffect
        {
            damage = damage
        });
    }
}