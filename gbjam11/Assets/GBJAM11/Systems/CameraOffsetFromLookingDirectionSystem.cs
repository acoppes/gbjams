using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GBJAM11.Systems
{
    public class CameraOffsetFromLookingDirectionSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<LookingDirection, CameraOffsetComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var lookingDirection = filter.Pools.Inc1.Get(entity);
                ref var cameraOffset = ref filter.Pools.Inc2.Get(entity);

                var offset = Vector2.zero;

                offset.x = lookingDirection.value.x * cameraOffset.xMax;
                offset.y = lookingDirection.value.y * cameraOffset.yMax;
                
                cameraOffset.offset = offset;
            }
        }

    }
}