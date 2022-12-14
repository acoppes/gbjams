using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using MyBox;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class CopyPositionToPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<PhysicsComponent>().Inc<PositionComponent>().End())
            {
                // copy from position to body
                ref var physicsComponent = ref physicsComponents.Get(entity);
                var positionComponent = positions.Get(entity);

                if (physicsComponent.syncType == PhysicsComponent.SyncType.FromPhysics)
                {
                    continue;
                }
                
                physicsComponent.collider.enabled = !physicsComponent.disabled;
                
                if (physicsComponent.isStatic)
                {
                    physicsComponent.collider.transform.position = new Vector3(positionComponent.value.x, positionComponent.value.z, 
                        positionComponent.value.y);
                }
                else
                {
                    physicsComponent.body.position = new Vector3(positionComponent.value.x, positionComponent.value.z, 
                        positionComponent.value.y);
                }
            }
        }
    }
}