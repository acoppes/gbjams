using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM14.Components
{
    public struct SpikesTrapComponent : IEntityComponent
    {
        public bool active;
        public Cooldown activeCooldown;
    }
    
    public class SpikesTrapComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new SpikesTrapComponent()
            {
            });
        }
    }
}