using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class HealthSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var healthComponents = world.GetComponents<HealthComponent>();
            
            foreach (var entity in world.GetFilter<HealthComponent>().End())
            {
                ref var healthComponent = ref healthComponents.Get(entity);
                
                // TODO: process pending damages

                if (healthComponent.current <= 0)
                {
                    world.DestroyEntity(entity);
                }
            }
        }
    }
}