using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class PhysicsGravitySystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public float distanceToGround = 0.1f;
        public Vector3 gravity = new Vector3(0, -9.81f, 0);
        
        private LayerMask groundContactLayerMask;
        
        public void Init(EcsSystems systems)
        {
            Physics.gravity = Vector3.zero;
            groundContactLayerMask = LayerMask.GetMask("StaticObstacle");
        }

        public void Run(EcsSystems systems)
        {
            var gravityComponents = world.GetComponents<GravityComponent>();
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            
            foreach (var entity in world.GetFilter<GravityComponent>().Inc<PhysicsComponent>().End())
            {
                ref var gravityComponent = ref gravityComponents.Get(entity);
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

                var position3d = physicsComponent.body.position;
                
                var ray = new Ray(position3d + new Vector3(0, 0.1f, 0), Vector3.down);
                
                if (Physics.Raycast(ray, out var hit, 2f, groundContactLayerMask, QueryTriggerInteraction.Ignore))
                {
                    // don't apply gravity if in contact with ground?
                    gravityComponent.inContactWithGround = hit.distance < distanceToGround;
                }

                if (!gravityComponent.inContactWithGround)
                {
                    physicsComponent.body.AddForce(gravity * gravityComponent.scale, ForceMode.Acceleration);
                }
            }
        }
    }
}