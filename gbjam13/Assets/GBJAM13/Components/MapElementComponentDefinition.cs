using Gemserk.Leopotam.Ecs;

namespace GBJAM13.Components
{
    public class MapElementComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new MapElementComponent());
        }
    }
}