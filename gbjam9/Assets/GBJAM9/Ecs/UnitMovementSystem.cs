using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class UnitMovementSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var inputs = world.GetComponents<UnitInputComponent>();
            var movementComponents = world.GetComponents<UnitMovementComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            foreach (var entity in world.GetFilter<UnitInputComponent>().Inc<UnitMovementComponent>().End())
            {
                ref var input = ref inputs.Get(entity);
                ref var movement = ref movementComponents.Get(entity);

                if (!input.disabled)
                {
                    movement.movingDirection = input.movementDirection;
                }
                else
                {
                    movement.movingDirection = Vector2.zero;
                }
            }

            foreach (var entity in world.GetFilter<UnitMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var movement = ref movementComponents.Get(entity);
                ref var position = ref positionComponents.Get(entity);

                var speed = movement.speed;
                var direction = movement.movingDirection;

                // if (e.state != null && e.dash != null && e.state.dashing)
                // {
                //     speed = e.dash.speed;
                //     // direction = e.movement.lookingDirection;
                //     direction = e.dash.direction;
                // }
                    
                var newPosition = position.value;

                var velocity = direction * speed;

                velocity = new Vector2(
                    velocity.x * movement.perspective.x, 
                    velocity.y * movement.perspective.y);
                    
                // e.collider.rigidbody.velocity = velocity;

                newPosition += velocity * Time.deltaTime;
                
                position.value = newPosition;

                movement.currentVelocity = velocity;

                // if (velocity.SqrMagnitude() > 0)
                // {
                //     // var movingDirection = velocity.normalized;
                //     movement.lookingDirection = velocity.normalized;
                //         
                //     // if (e.attack != null)
                //     // {
                //     //     e.attack.direction = movingDirection;
                //     // }
                //         
                //     // if (e.sfxContainer != null && e.sfxContainer.walkSfx != null)
                //     // {
                //     //     e.sfxContainer.walkSfx.Play();
                //     // }    
                // }
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