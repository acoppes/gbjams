using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GBJAM11.Systems
{
    public class CameraOffsetSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, CameraOffsetComponent>, Exc<DisabledComponent>> filter = default;

        public Object cameraOffsetDefinition;
        public string defaultName = "Player_Camera_Offset";

        private Entity cameraOffsetEntity;
        
        public void Init(EcsSystems systems)
        {
            cameraOffsetEntity = world.CreateEntity(cameraOffsetDefinition);
            cameraOffsetEntity.Get<NameComponent>().name = defaultName;
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var position = ref filter.Pools.Inc1.Get(entity);
                ref var cameraOffset = ref filter.Pools.Inc2.Get(entity);

                cameraOffsetEntity.Get<PositionComponent>().value = position.value + cameraOffset.offset;
            }
        }

    }
}