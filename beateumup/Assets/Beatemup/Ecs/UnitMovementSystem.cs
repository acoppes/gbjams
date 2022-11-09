using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnitMovementSystem : BaseSystem, IEcsRunSystem
    {
        public Vector2 gamePerspective = new Vector2(1.0f, 0.75f);
        
        public void Run(EcsSystems systems)
        {
            var controls = world.GetComponents<ControlComponent>();
            var movementComponents = world.GetComponents<UnitMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            // foreach (var entity in world.GetFilter<ControlComponent>().Inc<UnitMovementComponent>().End())
            // {
            //     ref var control = ref controls.Get(entity);
            //     ref var movement = ref movementComponents.Get(entity);
            //
            //     movement.movingDirection = control.direction;
            // }

            foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<PositionComponent>().End())
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

                var velocity = Vector3.zero;

                velocity.x = direction.x * (movement.speed + movement.extraSpeed.x);
                velocity.y = direction.y * (movement.speed + movement.extraSpeed.y);

                velocity = new Vector3(
                    velocity.x * gamePerspective.x, 
                    velocity.y * gamePerspective.y, 0);
                    
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