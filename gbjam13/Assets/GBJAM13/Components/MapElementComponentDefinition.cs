using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM13.Components
{
    public class MapElementComponentDefinition : ComponentDefinitionBase
    {
        public Vector3 shipOffset;
        
        public override void Apply(World world, Entity entity)
        {
            entity.Add(new MapElementComponent()
            {
                shipOffset = shipOffset
            });
        }
    }
}