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
        
        public Cooldown reloadCooldown;

        public Vector3 spawnOffset;

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

        public float fireCooldownStart;
        public float fireCooldown;
        public float fireOffset;
        public float reloadCooldown;

        public Vector2 spawnOffset;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new ArrowTrapComponent()
            {
                projectileDefinition = projectileDefinition,
                fireCooldown = new Cooldown(fireCooldown)
                {
                    current = fireCooldownStart
                },
                fireOffset = fireOffset,
                reloadCooldown = new Cooldown(reloadCooldown)
                {
                    current = reloadCooldown
                },
                spawnOffset = new Vector3(spawnOffset.x, spawnOffset.y, 0)
            });
        }
    }
}