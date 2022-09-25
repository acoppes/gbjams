using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;

namespace GBJAM10.Definitions
{
    public class DamageEffectDefinition : TargetEffectDefinition
    {
        //  more interesting values?
        public float damage;
    
        public override void Apply(World world, Entity entity)
        {
            ref var targetEffects = ref world.GetComponent<TargetEffectsComponent>(entity);
        
            targetEffects.targetEffects.Add(new DamageTargetEffect
            {
                damage = damage
            });
        }
    }
}