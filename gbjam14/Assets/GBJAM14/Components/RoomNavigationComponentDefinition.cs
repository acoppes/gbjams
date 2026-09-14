using GBJAM14.Data;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct RoomNavigationComponent : IEntityComponent
    {
        // public int/string currentRoom;

        public RoomExitData currentExit;
    }
    
    public class RoomNavigationComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new RoomNavigationComponent() { });
        }
    }
}