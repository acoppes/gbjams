using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class UnitMovementSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            var inputs = world.GetComponents<UnitInputComponent>();
            var movements = world.GetComponents<UnitMovementComponent>();
            var positions = world.GetComponents<PositionComponent>();

            foreach (var entity in world.GetFilter<UnitInputComponent>().Inc<UnitMovementComponent>().End())
            {
                ref var input = ref inputs.Get(entity);
                ref var movement = ref movements.Get(entity);

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
                ref var movement = ref movements.Get(entity);
                ref var position = ref positions.Get(entity);

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

                if (velocity.SqrMagnitude() > 0)
                {
                    // var movingDirection = velocity.normalized;
                    movement.lookingDirection = velocity.normalized;
                        
                    // if (e.attack != null)
                    // {
                    //     e.attack.direction = movingDirection;
                    // }
                        
                    // if (e.sfxContainer != null && e.sfxContainer.walkSfx != null)
                    // {
                    //     e.sfxContainer.walkSfx.Play();
                    // }    
                }
            }
        }
    }
}