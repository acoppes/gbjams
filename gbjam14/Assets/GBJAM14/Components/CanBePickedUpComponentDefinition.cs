using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct CanBePickedUpComponent : IEntityComponent
    {
        
    }

    public struct PickupActionComponent : IActionComponent
    {
        public Entity picker;
        public Entity pickup;
    }
    
    public class CanBePickedUpComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new CanBePickedUpComponent()
            {
            });
        }
    }
}