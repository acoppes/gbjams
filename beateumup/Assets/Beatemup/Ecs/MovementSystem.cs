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
            var movementComponents = world.GetComponents<UnitMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var movement = ref movementComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);
                
                if (movement.disabled)
                {
                    movement.currentVelocity = Vector3.zero;
                    continue;
                }

                var direction = movement.movingDirection;

                var newPosition = position.value;

                var velocity = Vector3.zero;

                velocity.x = direction.x * (movement.speed + movement.extraSpeed.x);
                velocity.y = direction.y * (movement.speed + movement.extraSpeed.y);
                velocity.z = direction.z * (movement.speed + movement.extraSpeed.z);

                velocity = new Vector3(
                    velocity.x * gamePerspective.x, 
                    velocity.y * gamePerspective.y, 
                    velocity.z);
                    
                // e.collider.rigidbody.velocity = velocity;

                newPosition += velocity * Time.deltaTime;
                
                position.value = newPosition;

                movement.currentVelocity = velocity;
            }
            
            // foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<LookingDirection>().End())
            // {
            //     var movement = movementComponents.Get(entity);
            //     ref var lookingDirection = ref lookingDirectionComponents.Get(entity);
            //
            //     if (Mathf.Abs(movement.currentVelocity.x) > 0)
            //     {
            //         lookingDirection.value.x = Mathf.Abs(movement.currentVelocity.x) / movement.currentVelocity.x;
            //     }
            //
            //     // if (movement.currentVelocity.SqrMagnitude() > 0)
            //     // {
            //     //     lookingDirection.value = movement.currentVelocity.normalized;
            //     // }
            // }
        }
    }
}