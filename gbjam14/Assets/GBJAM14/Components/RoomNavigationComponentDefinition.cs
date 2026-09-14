using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct RoomNavigationComponent : IEntityComponent
    {
        // public int/string currentRoom;

        public string roomId;
        public string startId;

        public string nextRoomId;
        public string nextStartId;
    }
    
    public class RoomNavigationComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new RoomNavigationComponent() { });
        }
    }
}