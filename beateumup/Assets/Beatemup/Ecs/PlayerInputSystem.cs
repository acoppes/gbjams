using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Beatemup.Ecs
{
    public class PlayerInputSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            var controlComponents = world.GetComponents<ControlComponent>();

            foreach (var entity in world.GetFilter<PlayerInputComponent>().Inc<ControlComponent>().End())
            {
                var playerInputComponent = playerInputComponents.Get(entity);

                var playerInput = PlayerInput.GetPlayerByIndex(playerInputComponent.playerInput);

                if (playerInput == null)
                {
                    break;
                }

                var move = playerInput.actions["Move"];

                ref var controlComponent = ref controlComponents.Get(entity);
                controlComponent.direction = move.ReadValue<Vector2>();

                var button1 = playerInput.actions["Button1"];
                controlComponent.button1.UpdatePressed(button1.IsPressed());

                // var startRunning = playerInput.actions["StartRunning"];
                // if (startRunning.)
                // {
                //     Debug.Log("start running");
                // } 
            }
        }
    }
}