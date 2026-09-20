using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct PlatesTrapComponent : IEntityComponent
    {
        public int pressCount;
        public bool pressed => pressCount > 0;
    }
    
    public class PlateTrapComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new PlatesTrapComponent()
            {
            });
        }
    }
}