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

                var horizontal = playerInput.actions["Horizontal"];
                var vertical = playerInput.actions["Vertical"];

                ref var controlComponent = ref controlComponents.Get(entity);
                
                controlComponent.direction = 
                    new Vector2(horizontal.ReadValue<float>(), vertical.ReadValue<float>());

                var button1 = playerInput.actions["Button1"];
                controlComponent.button1.UpdatePressed(button1.IsPressed());

                var button2 = playerInput.actions["Button2"];
                controlComponent.button2.UpdatePressed(button2.IsPressed());
                
                // var startRunning = playerInput.actions["StartRunning"];
                // if (startRunning.)
                // {
                //     Debug.Log("start running");
                // } 
            }
        }
    }
}