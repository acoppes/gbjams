using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class HorizontalMovementSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var movementComponents = world.GetComponents<HorizontalMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HorizontalMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var movement = ref movementComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);
                
                // if (movement.disabled)
                // {
                //     movement.currentVelocity = Vector2.zero;
                //     continue;
                // }

                var direction = movement.movingDirection;

                var newPosition = position.value;

                var velocity = direction.normalized * movement.speed * movement.speedMultiplier;

                newPosition += velocity * Time.deltaTime;
                
                position.value = newPosition;

                movement.currentVelocity = velocity;
            }
        }
    }
}