using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct RoomComponent : IEntityComponent
    {
        
    }
    
    public class RoomComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(RoomComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new RoomComponent()
            {

            });
        }
    }
}