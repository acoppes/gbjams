using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM10.Ecs
{
    public class UnitMovementSystem : BaseSystem, IEcsRunSystem
    {
        public Vector2 gamePerspective = new Vector2(1.0f, 0.75f);
        
        public void Run(EcsSystems systems)
        {
            var controls = world.GetComponents<UnitControlComponent>();
            var movementComponents = world.GetComponents<UnitMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            foreach (var entity in world.GetFilter<UnitControlComponent>().Inc<UnitMovementComponent>().End())
            {
                ref var control = ref controls.Get(entity);
                ref var movement = ref movementComponents.Get(entity);

                movement.movingDirection = control.direction;
            }

            foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var movement = ref movementComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);
                
                if (movement.disabled)
                {
                    movement.currentVelocity = Vector2.zero;
                    continue;
                }

                var speed = movement.speed + movement.extraSpeed;
                var direction = movement.movingDirection;

                var newPosition = position.value;

                var velocity = direction * speed;

                velocity = new Vector2(
                    velocity.x * gamePerspective.x, 
                    velocity.y * gamePerspective.y);
                    
                // e.collider.rigidbody.velocity = velocity;

                newPosition += velocity * Time.deltaTime;
                
                position.value = newPosition;

                movement.currentVelocity = velocity;
            }
            
            foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<LookingDirection>().End())
            {
                var movement = movementComponents.Get(entity);
                ref var lookingDirection = ref lookingDirectionComponents.Get(entity);

                if (movement.currentVelocity.SqrMagnitude() > 0)
                {
                    lookingDirection.value = movement.currentVelocity.normalized;
                }
            }
        }
    }
}