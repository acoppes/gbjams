using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct ItemComponent : IEntityComponent
    {
        public string itemId;
    }
    
    public class ItemComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public string itemId;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new ItemComponent()
            {
                itemId = itemId.Trim()
            });
        }
    }
}