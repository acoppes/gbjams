using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct ForcedGroundComponent : IEntityComponent
    {
        
    }
    
    public class ForcedGroundComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(ForcedGroundComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ForcedGroundComponent());
        }
    }
}