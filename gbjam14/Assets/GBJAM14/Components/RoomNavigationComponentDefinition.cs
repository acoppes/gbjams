using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct RoomNavigationComponent : IEntityComponent
    {
        public string nextRoomId;
        public string nextEnterId;
    }
    
    public class RoomNavigationComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new RoomNavigationComponent() { });
        }
    }
}