using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM10.Ecs
{
    public class KeepUnitInsideCameraBoundsSystem : BaseSystem, IEcsRunSystem
    {
        public float maxCameraBehind = 4.0f;
        public float maxCameraAhead = 2.0f;

        private Entity cameraEntity;
        
        public void Run(EcsSystems systems)
        {
            var positionComponents = world.GetComponents<PositionComponent>();

            // var mainCamera = Camera.main;
            
            if (cameraEntity == Entity.NullEntity)
            {
                cameraEntity = world.GetEntityByName("Main_Camera");
            }
            
            if (cameraEntity == Entity.NullEntity)
            {
                return;
            }

            var cameraPosition = world.GetComponent<PositionComponent>(cameraEntity);
            
            foreach (var entity in world.GetFilter<PositionComponent>().Inc<KeepInsideCameraComponent>().End())
            {
                ref var position = ref positionComponents.Get(entity);

                if (position.value.x < cameraPosition.value.x - maxCameraBehind)
                {
                    position.value.x = cameraPosition.value.x - maxCameraBehind;
                }
                
                if (position.value.x > cameraPosition.value.x + maxCameraAhead)
                {
                    position.value.x = cameraPosition.value.x + maxCameraAhead;
                }
            }
        }
    }
}