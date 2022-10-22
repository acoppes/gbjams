using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace GBJAM10.Ecs
{
    public class TimeToLiveAbilitySystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            var abilitiesComponents = world.GetComponents<AbilitiesComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>().Inc<AbilitiesComponent>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);

                var timeToLive = abilitiesComponent.GetAbility("TimeToLive");

                if (timeToLive != null && timeToLive.isReady)
                {
                    healthComponent.deathRequest = true;
                }
            }
        }
    }
}