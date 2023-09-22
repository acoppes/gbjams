using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct WallStickComponent: IEntityComponent
    {
        public bool roof;
        public bool wall;
    }
    
    public class WallStickComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(WallStickComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new WallStickComponent());
        }
    }
}