using GBJAM.Commons;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class PlayerInputSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<PlayerInputComponent>().End();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            
            var gameboyKeyMap = GameboyInput.Instance.current;

            foreach (var entity in filter)
            {
                ref var playerInputComponent = ref playerInputComponents.Get(entity);
                playerInputComponent.keyMap = gameboyKeyMap;
            }
        }
    }
}