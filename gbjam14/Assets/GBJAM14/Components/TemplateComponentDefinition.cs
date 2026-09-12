using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public class TemplateComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new TemplateComponent() { });
        }
    }
}