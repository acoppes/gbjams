using Game.Components;
using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GBJAM13.Systems
{
    public class MapDestinationSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MapElementComponent, AnimationsComponent>, Exc<DisabledComponent>> 
            filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var mapElement = ref filter.Pools.Inc1.Get(e);
                ref var animations = ref filter.Pools.Inc2.Get(e);

                if (!animations.IsPlaying(mapElement.element))
                {
                    animations.Play(mapElement.element);
                }
            }
        }


    }
}