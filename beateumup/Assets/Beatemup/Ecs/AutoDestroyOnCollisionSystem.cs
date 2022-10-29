using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class AutoDestroyOnCollisionSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            var abilitiesComponents = world.GetComponents<AbilitiesComponent>();
            var colliderComponents = world.GetComponents<ColliderComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>().Inc<AbilitiesComponent>().Inc<ColliderComponent>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                var abilitiesComponent = abilitiesComponents.Get(entity);
                var colliderComponent = colliderComponents.Get(entity);

                var autoDestroyOnCollision = abilitiesComponent.GetAbility("AutoDestroyOnCollision");

                if (colliderComponent.collisionCount > 0 && 
                    autoDestroyOnCollision != null && autoDestroyOnCollision.isReady)
                {
                    healthComponent.deathRequest = true;
                }
            }
        }
    }
}