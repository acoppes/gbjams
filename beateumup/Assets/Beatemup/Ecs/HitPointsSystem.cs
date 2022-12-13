using Gemserk.Gameplay.Signals;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class HitPointsSystem : BaseSystem, IEcsRunSystem
    {
        public SignalAsset onEntityDeathSignal;
        
        public void Run(EcsSystems systems)
        {
            var hitPointsComponents = world.GetComponents<HitPointsComponent>();
            
            foreach (var entity in world.GetFilter<HitPointsComponent>().End())
            {
                ref var hitPoints = ref hitPointsComponents.Get(entity);
                var worldEntity = world.GetEntity(entity);

                if (hitPoints.hits.Count == 0)
                {
                    continue;
                }

                var alive = hitPoints.aliveType;
                
                foreach (var hit in hitPoints.hits)
                {
                    hitPoints.current -= hit.hitPoints;
                }

                if (onEntityDeathSignal != null)
                {
                    if (alive == HitPointsComponent.AliveType.Alive &&
                        hitPoints.aliveType == HitPointsComponent.AliveType.Death)
                    {
                        onEntityDeathSignal.Signal(worldEntity);
                    }
                }
                
                hitPoints.OnHit(world, worldEntity);

                hitPoints.hits.Clear();
            }
        }
    }
}