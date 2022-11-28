using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class QueryCacheSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var queryComponents = world.GetComponents<QueryComponent>();
            var playerComponents = world.GetComponents<PlayerComponent>();
            var nameComponents = world.GetComponents<NameComponent>();
            
            foreach (var entity in world.GetFilter<QueryComponent>().Inc<NameComponent>().End())
            {
                ref var queryComponent = ref queryComponents.Get(entity);
                var nameComponent = nameComponents.Get(entity);
                queryComponent.name = nameComponent.name;
            }
            
            foreach (var entity in world.GetFilter<QueryComponent>().Inc<PlayerComponent>().End())
            {
                ref var queryComponent = ref queryComponents.Get(entity);
                var playerComponent = playerComponents.Get(entity);
                queryComponent.player = playerComponent.player;
            }
        }
    }
}