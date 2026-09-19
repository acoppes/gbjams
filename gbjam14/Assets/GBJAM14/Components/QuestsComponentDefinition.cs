using System.Collections.Generic;
using System.Linq;
using Gemserk.Leopotam.Ecs;

namespace GBJAM14.Components
{
    public struct QuestsComponent : IEntityComponent
    {
        public List<string> quests;
    }
    
    public class QuestsComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        public List<string> quests = new List<string>();
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new QuestsComponent()
            {
                quests = new List<string>(quests.Select(q => q.Trim()))
            });
        }
    }
}