using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class HitEventSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var hitComponents = world.GetComponents<HitComponent>();
            
            foreach (var entity in world.GetFilter<HitComponent>().End())
            {
                ref var hitComponent = ref hitComponents.Get(entity);

                if (hitComponent.hits == 0)
                {
                    continue;
                }

                hitComponent.OnHit();

                hitComponent.hits = 0;
            }
        }
    }
}