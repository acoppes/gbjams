using Game.Components;
using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace GBJAM13.Systems
{
    public class MapSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MapElementComponent, AnimationsComponent>, Exc<DisabledComponent>> 
            filter = default;
        
        readonly EcsFilterInject<Inc<MapElementComponent, ModelComponent>, Exc<DisabledComponent>> 
            debugTempFilter = default;
        
        readonly EcsFilterInject<Inc<MapElementComponent, ModelComponent>, Exc<DisabledComponent>> 
            mapVisitFilter = default;
        
        // readonly EcsFilterInject<Inc<MapElementComponent, MapSelectedComponent>, Exc<DisabledComponent, HasSelectionIndicatorComponent>> 
        //     mapSelectedFilter = default;

        public bool mainPathDebugEnabled;

        public Color colorForDebug;
        public Color colorOutsideTravelPath;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var mapElement = ref filter.Pools.Inc1.Get(e);
                ref var animations = ref filter.Pools.Inc2.Get(e);

                if (!animations.IsPlaying(mapElement.eventVariant))
                {
                    animations.Play(mapElement.eventVariant);
                }
            }
            
            foreach (var e in mapVisitFilter.Value)
            {
                ref var mapElement = ref mapVisitFilter.Pools.Inc1.Get(e);
                ref var model = ref mapVisitFilter.Pools.Inc2.Get(e);

                if (mapElement.outsideTravelPath)
                {
                    model.color = colorOutsideTravelPath;
                }
                else
                {
                    model.color = Color.white;
                }
            }
            
            if (mainPathDebugEnabled)
            {
                foreach (var e in debugTempFilter.Value)
                {
                    ref var mapElement = ref debugTempFilter.Pools.Inc1.Get(e);
                    ref var model = ref debugTempFilter.Pools.Inc2.Get(e);

                    if (mapElement.mainPath)
                    {
                        model.color = colorForDebug;
                    }
                    else
                    {
                        model.color = Color.white;
                    }
                }
            }
            
            // foreach (var e in mapSelectedFilter.Value)
            // {
            //     ref var mapElement = ref mapSelectedFilter.Pools.Inc1.Get(e);
            //     ref var model = ref mapSelectedFilter.Pools.Inc2.Get(e);
            //
            //     if (mapElement.mainPath)
            //     {
            //         model.color = colorForDebug;
            //     }
            //     else
            //     {
            //         model.color = Color.white;
            //     }
            // }
        }


    }
}