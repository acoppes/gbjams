using Gemserk.Leopotam.Ecs;

namespace GBJAM12.Components
{
    public struct DanceMovesComponent : IEntityComponent
    {
        public bool d1;
        public bool d2;
        public bool d3;
    }
    
    public class DanceMovesComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new DanceMovesComponent()
            {
                
            });
        }
    }
}