using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct CanBeInteractedComponent : IEntityComponent
    {

    }
    
    public class CanBeInteractedComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new CanBeInteractedComponent() { });
        }
    }
}