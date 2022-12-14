using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyPositionFromPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<PhysicsComponent>().Inc<PositionComponent>().End())
            {
                // copy from body to position
                ref var physicsComponent = ref physicsComponents.Get(entity);

                if (physicsComponent.isStatic)
                {
                    continue;
                }
                
                ref var positionComponent = ref positions.Get(entity);

                var position = physicsComponent.body.position;
                positionComponent.value = new Vector3(position.x, position.z, position.y);

                var velocity = physicsComponent.body.velocity;
                physicsComponent.velocity = new Vector3(velocity.x, velocity.z, velocity.y);
            }
        }
    }
}