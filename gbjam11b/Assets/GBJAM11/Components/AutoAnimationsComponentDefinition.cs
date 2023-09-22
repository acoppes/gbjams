using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct AutoAnimationComponent : IEntityComponent
    {
        public bool disabled;
    }
    
    public class AutoAnimationsComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(AutoAnimationComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new AutoAnimationComponent());
        }
    }
}