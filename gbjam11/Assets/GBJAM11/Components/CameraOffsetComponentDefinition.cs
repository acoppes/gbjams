using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM11.Components
{
    public struct CameraOffsetComponent: IEntityComponent
    {
        public float xMax, yMax;
        public Vector3 offset;
    }
    
    public class CameraOffsetComponentDefinition : ComponentDefinitionBase
    {
        public float xMax, yMax;
        
        public override string GetComponentName()
        {
            return nameof(CameraOffsetComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CameraOffsetComponent()
            {
                xMax = xMax,
                yMax = yMax
            });
        }
    }
}