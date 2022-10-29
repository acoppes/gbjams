using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class AutoDestroyOutsideCameraSystem : BaseSystem, IEcsRunSystem
    {
        public float maxCameraDistance = 7.0f;

        private Entity cameraEntity;
        
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            if (cameraEntity == Entity.NullEntity)
            {
                cameraEntity = world.GetEntityByName("Main_Camera");
            }
            
            if (cameraEntity == Entity.NullEntity)
            {
                return;
            }

            var cameraPosition = world.GetComponent<PositionComponent>(cameraEntity);
            
            foreach (var entity in world.GetFilter<HealthComponent>().Inc<PositionComponent>().Inc<AutoDestroyOutsideCamera>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                var position = positionComponents.Get(entity);

                if (Mathf.Abs(position.value.x - cameraPosition.value.x) > maxCameraDistance)
                {
                    healthComponent.deathRequest = true;
                }
            }
        }
    }
}