using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class MovementSystem : BaseSystem, IEcsRunSystem
    {
        public Vector2 gamePerspective = new Vector2(1.0f, 0.75f);
        
        public void Run(EcsSystems systems)
        {
            var movementComponents = world.GetComponents<HorizontalMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<HorizontalMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var movement = ref movementComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);
                
                if (movement.disabled)
                {
                    movement.currentVelocity = Vector2.zero;
                    continue;
                }

                var direction = movement.movingDirection;

                var newPosition = position.value;

                var velocity = direction.normalized * movement.speed * movement.speedMultiplier;

                // velocity.x = direction.x * movement.baseSpeed.x * movement.speedMultiplier;
                // velocity.y = direction.y * movement.baseSpeed.y * movement.speedMultiplier;
                
                // velocity.z = direction.z * (movement.speed + movement.extraSpeed.z);

                // velocity = new Vector2(velocity.x, velocity.y);
                    
                // e.collider.rigidbody.velocity = velocity;

                newPosition += new Vector3(velocity.x * gamePerspective.x, velocity.y * gamePerspective.y, 0) * Time.deltaTime;
                
                position.value = newPosition;

                movement.currentVelocity = velocity;
            }
        }
    }
}