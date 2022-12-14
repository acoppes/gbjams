using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class CopyPositionFromPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();
            var positions = world.GetComponents<PositionComponent>();
            
            foreach (var entity in world.GetFilter<PhysicsComponent>().Inc<PositionComponent>().End())
            {
                // copy from body to position`
                ref var physicsComponent = ref physicsComponents.Get(entity);

                if (physicsComponent.isStatic)
                {
                    continue;
                }
                
                ref var positionComponent = ref positions.Get(entity);

                positionComponent.value = physicsComponent.body.position;
                physicsComponent.velocity = physicsComponent.body.velocity;
            }
        }
    }
}