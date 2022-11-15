using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class GravitySystem : BaseSystem, IEcsRunSystem
    {
        public float maxFallingSpeed = -20;
        public float g = -9.81f;
        
        public void Run(EcsSystems systems)
        {
            var gravityComponents = world.GetComponents<GravityComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var verticalMovements = world.GetComponents<VerticalMovementComponent>();
            
            foreach (var entity in world.GetFilter<GravityComponent>().Inc<VerticalMovementComponent>().End())
            {
                ref var gravity = ref gravityComponents.Get(entity);
                ref var vertical = ref verticalMovements.Get(entity);

                if (gravity.disabled)
                {
                    continue;
                }

                vertical.speed += g * gravity.scale * Time.deltaTime;

                if (vertical.speed < maxFallingSpeed)
                {
                    vertical.speed = maxFallingSpeed;
                }
            }

            foreach (var entity in world.GetFilter<VerticalMovementComponent>().Inc<PositionComponent>().End())
            {
                ref var position = ref positionComponents.Get(entity);
                ref var vertical = ref verticalMovements.Get(entity);

                position.value.z += vertical.speed * Time.deltaTime;
                
                if (position.value.z <= 0)
                {
                    position.value.z = 0;
                    vertical.speed = 0;
                }
                
                vertical.isOverGround = position.value.z <= Mathf.Epsilon;
            }
        }
    }
}