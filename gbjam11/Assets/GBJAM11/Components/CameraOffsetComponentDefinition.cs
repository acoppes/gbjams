using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct CameraOffsetComponent: IEntityComponent
    {
        public Vector3 offset;
    }
    
    public class CameraOffsetComponentDefinition : ComponentDefinitionBase
    {
        public override string GetComponentName()
        {
            return nameof(CameraOffsetComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CameraOffsetComponent());
        }
    }
}