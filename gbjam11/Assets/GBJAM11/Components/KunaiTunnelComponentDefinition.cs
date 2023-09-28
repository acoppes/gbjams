using Gemserk.Leopotam.Ecs;

namespace GBJAM11.Components
{
    public struct KunaiTunnelComponent : IEntityComponent
    {
        public Entity exitEntity;
    }
    
    public class KunaiTunnelComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(KunaiTunnelComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new KunaiTunnelComponent());
        }
    }
}