using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct KunaiComponent : IEntityComponent
    {
        public Entity stuckEntity;
    }
    
    public class KunaiComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(KunaiComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new KunaiComponent()
            {
                stuckEntity = Entity.NullEntity
            });
        }
    }
}