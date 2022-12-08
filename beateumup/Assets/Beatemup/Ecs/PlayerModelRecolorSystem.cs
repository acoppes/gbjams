using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class PlayerModelRecolorSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();

            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PlayerInputComponent>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var playerInputComponent = playerInputComponents.Get(entity);

                if (modelComponent.instance.remapShader != null && modelComponent.remapTexturesPerPlayer.Length > 0)
                {
                    modelComponent.instance.remapShader.lutTexture = 
                        modelComponent.remapTexturesPerPlayer.GetItemOrLast(playerInputComponent.playerInput);
                }
            }
        }
    }
}