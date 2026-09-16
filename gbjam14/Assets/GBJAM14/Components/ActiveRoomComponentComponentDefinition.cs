using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct ActiveRoomComponent : IEntityComponent
    {
        public string roomId;
        public string doorId;
    }
    
    public class ActiveRoomComponentComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new ActiveRoomComponent()
            {
                
            });
        }
    }
}