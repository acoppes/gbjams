using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyFromObstaclePhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var obstacleComponents = world.GetComponents<ObstacleComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<ObstacleComponent>().Inc<PositionComponent>().End())
            {
                // copy from body to position
                var obstacleComponent = obstacleComponents.Get(entity);
                ref var positionComponent = ref positions.Get(entity);

                var position = obstacleComponent.body.position;
                positionComponent.value = new Vector3(position.x, position.y, positionComponent.value.z);
            }
        }
    }
}