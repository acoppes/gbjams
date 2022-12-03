using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class HitPointsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var hitPointsComponents = world.GetComponents<HitPointsComponent>();
            
            foreach (var entity in world.GetFilter<HitPointsComponent>().End())
            {
                ref var hitPoints = ref hitPointsComponents.Get(entity);

                if (hitPoints.hits.Count == 0)
                {
                    continue;
                }
                
                foreach (var hit in hitPoints.hits)
                {
                    hitPoints.current -= hit.hitPoints;
                }
                
                hitPoints.OnHit(world, entity);

                hitPoints.hits.Clear();
            }
        }
    }
}