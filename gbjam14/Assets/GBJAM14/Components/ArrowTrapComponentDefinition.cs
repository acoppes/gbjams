using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM14.Components
{
    public struct ArrowTrapComponent : IEntityComponent
    {
        public Object projectileDefinition;
        public Cooldown fireCooldown;
        public float fireOffset;

        // options:
        // initial delay
        // fire speed override
        // switch directions 
        // fire multiple arrows at the same time
    }
    
    public class ArrowTrapComponentDefinition : ComponentDefinitionBase
    {
        [EntityDefinition]
        public Object projectileDefinition;

        public float fireCooldown;
        public float fireOffset;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new ArrowTrapComponent()
            {
                projectileDefinition = projectileDefinition,
                fireCooldown = new Cooldown(fireCooldown),
                fireOffset = fireOffset
            });
        }
    }
}