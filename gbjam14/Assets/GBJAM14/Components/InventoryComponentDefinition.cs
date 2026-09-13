using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct InventoryComponent : IEntityComponent
    {
        public List<string> items;
    }
    
    public class InventoryComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public List<string> defaultItems;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new InventoryComponent()
            {
                items = new List<string>(defaultItems)
            });
        }
    }
}