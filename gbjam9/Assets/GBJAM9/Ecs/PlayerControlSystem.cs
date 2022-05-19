using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace GBJAM9.Ecs
{
    public class PlayerControlSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world.GetFilter<UnitControlComponent>()
                .Inc<PlayerInputComponent>().End();
            
            var controlComponents = world.GetComponents<UnitControlComponent>();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            
            foreach (var entity in filter)
            {
                ref var control = ref controlComponents.Get(entity);
                var playerInputComponent = playerInputComponents.Get(entity);

                if (!playerInputComponent.disabled)
                {
                    control.direction = playerInputComponent.keyMap.direction;

                    // if (playerInputComponent.keyMap.direction.SqrMagnitude() > 0)
                    // {
                    //     control.attackDirection = playerInputComponent.keyMap.direction;
                    // }

                    control.mainAction = playerInputComponent.keyMap.button1Pressed;
                    control.secondaryAction = playerInputComponent.keyMap.button2Pressed;
                }
            }
        }
    }
}
