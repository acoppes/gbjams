using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class PlayerIndicatorModelSystem : BaseSystem, IEcsRunSystem
    {
        public Color[] playerColors;
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PlayerInputComponent>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var playerInputComponent = playerInputComponents.Get(entity);

                if (modelComponent.instance.playerIndicator != null)
                {
                    modelComponent.instance.playerIndicator.enabled = playerInputComponent.isControlled;
                    modelComponent.instance.playerIndicator.color = playerColors[playerInputComponent.playerInput];
                }
            }
        }
    }
}