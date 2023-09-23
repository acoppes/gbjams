using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct RoomComponent : IEntityComponent
    {
        // enter, exit 
        public Entity nextRoom;
        public Entity exitDoor;

        public List<Entity> spawners;
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
                nextRoom = Entity.NullEntity,
                exitDoor = Entity.NullEntity,
                spawners = new List<Entity>()
            });
        }
    }
}