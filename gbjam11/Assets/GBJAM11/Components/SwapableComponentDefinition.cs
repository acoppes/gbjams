using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct SwapableComponent : IEntityComponent
    {
        
    }
    
    public class SwapableComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(SwapableComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new SwapableComponent());
        }
    }
}