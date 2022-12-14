using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class PhysicsGravitySystem : BaseSystem, IEcsRunSystem, IInit
    {
        public float distanceToGround = 0.1f;
        public Vector3 gravity = new Vector3(0, -9.81f, 0);
        
        public void OnInit()
        {
            Physics.gravity = Vector3.zero;
        }
        
        public void Run(EcsSystems systems)
        {
            var gravityComponents = world.GetComponents<GravityComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            var verticalMovements = world.GetComponents<VerticalMovementComponent>();
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            
            foreach (var entity in world.GetFilter<GravityComponent>().Inc<PhysicsComponent>().End())
            {
                var gravityComponent = gravityComponents.Get(entity);
                ref var physicsComponent = ref physicsComponents.Get(entity);

                if (gravityComponent.disabled)
                {
                    continue;
                }

                if (physicsComponent.isStatic)
                {
                    continue;
                }

                gravityComponent.inContactWithGround = false;

                if (physicsComponent.body.SweepTest(new Vector3(0, -1, 0), out var hit, 5.0f, QueryTriggerInteraction.Ignore))
                {
                    // don't apply gravity if in contact with ground?
                    gravityComponent.inContactWithGround = hit.distance < distanceToGround;
                }

                if (!gravityComponent.inContactWithGround)
                {
                    physicsComponent.body.AddForce(gravity * gravityComponent.scale, ForceMode.Acceleration);
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