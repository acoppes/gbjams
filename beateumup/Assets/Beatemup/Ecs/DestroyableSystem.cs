using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class DestroyableSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var destroyables = world.GetComponents<DestroyableComponent>();
            
            foreach (var entity in world.GetFilter<DestroyableComponent>().End())
            {
                var destroyable = destroyables.Get(entity);
                if (destroyable.destroy)
                {
                    world.DestroyEntity(world.GetEntity(entity));
                }
            }
        }
    }
}