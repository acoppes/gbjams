using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class CopyPositionToPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<PhysicsComponent>().Inc<PositionComponent>().End())
            {
                // copy from position to body
                ref var physicsComponent = ref physicsComponents.Get(entity);
                var positionComponent = positions.Get(entity);

                physicsComponent.obstacleCollider.enabled = !physicsComponent.disableCollideWithObstacles;
                
                if (physicsComponent.syncType == PhysicsComponent.SyncType.FromPhysics)
                {
                    continue;
                }

                if (physicsComponent.isStatic)
                {
                    physicsComponent.obstacleCollider.transform.position = positionComponent.value;
                }
                else
                {
                    physicsComponent.body.position = positionComponent.value;
                }
            }
        }
    }
}