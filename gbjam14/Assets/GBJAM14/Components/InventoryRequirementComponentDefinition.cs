using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct InventoryRequirementComponent : IEntityComponent
    {
        public List<string> requirements;
    }
    
    public class InventoryRequirementComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public List<string> requirements;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new InventoryRequirementComponent()
            {
                requirements = new List<string>(requirements.Select(r => r.Trim()))
            });
        }
    }
}