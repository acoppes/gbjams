using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct DoorComponent : IEntityComponent
    {
        public bool startsOpen;
        public bool isOpen;
        public bool isClosed;
    }
    
    public class DoorComponentDefinition : ComponentDefinitionBase
    {
        public bool startsOpen;
        
        public override string GetComponentName()
        {
            return nameof(DoorComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            if (!world.HasComponent<DoorComponent>(entity))
            {
                world.AddComponent(entity, new DoorComponent());
            }

            world.GetComponent<DoorComponent>(entity).startsOpen = startsOpen;
        }
    }
}