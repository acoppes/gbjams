using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct KunaiComponent : IEntityComponent
    {
        public Entity stuckEntity;
        public bool onRoof;
    }
    
    public class KunaiComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(KunaiComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new KunaiComponent()
            {
                stuckEntity = Entity.NullEntity
            });
        }
    }
}