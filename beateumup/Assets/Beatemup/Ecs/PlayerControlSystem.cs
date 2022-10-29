using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;

namespace Beatemup.Ecs
{
    public class PlayerControlSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var filter = world
                .GetFilter<UnitControlComponent>()
                .Inc<PlayerInputComponent>()
                .Inc<PlayerComponent>()
                .End();

            var gameData = world.sharedData.sharedData as SharedGameData;

            var controlComponents = world.GetComponents<UnitControlComponent>();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            var playerComponents = world.GetComponents<PlayerComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            foreach (var entity in filter)
            {
                ref var control = ref controlComponents.Get(entity);
                var playerInputComponent = playerInputComponents.Get(entity);
                var playerComponent = playerComponents.Get(entity);

                if (playerComponent.player != gameData.activePlayer)
                {
                    continue;
                }

                if (!playerInputComponent.disabled && !control.locked)
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
            
            // Update looking direction based on controls
            foreach (var entity in world.GetFilter<UnitControlComponent>().Inc<LookingDirection>().End())
            {
                var control = controlComponents.Get(entity);
                ref var lookingDirection = ref lookingDirectionComponents.Get(entity);

                if (control.direction.sqrMagnitude > 0f)
                {
                    lookingDirection.value = control.direction.normalized;
                }
            }
        }
    }
}
