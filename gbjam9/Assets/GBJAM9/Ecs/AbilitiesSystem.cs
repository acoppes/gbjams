using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class AbilitiesSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<AbilitiesComponent>().End();
            var abilitiesComponents = world.GetComponents<AbilitiesComponent>();
            
            foreach (var entity in filter)
            {
                ref var abilitiesComponent = ref abilitiesComponents.Get(entity);
                foreach (var ability in abilitiesComponent.abilities)
                {
                    if (!ability.isRunning)
                    {
                        ability.cooldownCurrent += Time.deltaTime;
                    }
                    else
                    {
                        ability.runningTime += Time.deltaTime;
                    }
                }
            }
        }
    }
}