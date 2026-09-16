using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct MainCharacterComponent : IEntityComponent
    {
        
    }
    
    public class MainCharacterComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new MainCharacterComponent() { });
        }
    }
}