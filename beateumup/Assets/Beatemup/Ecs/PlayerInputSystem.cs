using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Beatemup.Ecs
{
    public class PlayerInputSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        public float defaultBufferTime = 1.0f;
        
        private List<FieldInfo> _controlActions; 
        
        public void Init(EcsSystems systems)
        {
            var allFields = typeof(ControlComponent).GetFields(BindingFlags.Public | BindingFlags.Instance);
            _controlActions = allFields.Where(f => f.FieldType == typeof(Button)).ToList();
        }
        
        private void UpdateFromInput(ref ControlComponent controlComponent, FieldInfo field, PlayerInput playerInput)
        {
            var inputAction = playerInput.actions.FindAction(field.Name);
            
            if (inputAction == null)
                return;

            var pressed = inputAction.IsPressed();

            var button = (Button) field.GetValue(controlComponent);
            button.UpdatePressed(pressed);

            object boxed = controlComponent;
            field.SetValue(boxed, button);
            controlComponent = (ControlComponent) boxed;
            
            if (button.wasPressedThisFrame)
            {
                controlComponent.buffer.Add(field.Name);
                controlComponent.bufferTime = defaultBufferTime;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            var controlComponents = world.GetComponents<ControlComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            foreach (var entity in world.GetFilter<PlayerInputComponent>().Inc<ControlComponent>().End())
            {
                var playerInputComponent = playerInputComponents.Get(entity);

                var playerInput = PlayerInput.GetPlayerByIndex(playerInputComponent.playerInput);

                if (playerInput == null)
                {
                    break;
                }
                
                ref var controlComponent = ref controlComponents.Get(entity);

                controlComponent.bufferTime -= Time.deltaTime;
                
                var direction = Vector2.zero;

                foreach (var controlAction in _controlActions)
                {
                    UpdateFromInput(ref controlComponent, controlAction, playerInput);
                }

                if (controlComponent.up.isPressed)
                {
                    direction.y += 1.0f;
                } 
                
                if (controlComponent.down.isPressed)
                {
                    direction.y -= 1.0f;
                }
                
                if (controlComponent.right.isPressed)
                {
                    direction.x += 1.0f;
                } 
                
                if (controlComponent.left.isPressed)
                {
                    direction.x -= 1.0f;
                }

                controlComponent.direction = direction;

                if (controlComponent.bufferTime < 0 && controlComponent.buffer.Count > 0)
                {
                    controlComponent.buffer.Clear();
                }

                // if (controlComponent.buffer.Count > ControlComponent.MaxBufferCount)
                // {
                //     controlComponent.buffer.remo
                // }
            }
            
            foreach (var entity in world.GetFilter<ControlComponent>().Inc<LookingDirection>().End())
            {
                var lookingDirection = lookingDirectionComponents.Get(entity);

                ref var controlComponent = ref controlComponents.Get(entity);
                
                controlComponent.forward.UpdatePressed(
                    (controlComponent.direction.x > 0 && lookingDirection.value.x > 0) || 
                    (controlComponent.direction.x < 0 && lookingDirection.value.x < 0)); 
                
                controlComponent.backward.UpdatePressed(
                    (controlComponent.direction.x < 0 && lookingDirection.value.x > 0) || 
                    (controlComponent.direction.x > 0 && lookingDirection.value.x < 0)); 
            }
        }

    }
}