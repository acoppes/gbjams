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
        private List<FieldInfo> _controlActions; 
        
        public void Init(EcsSystems systems)
        {
            var allFields = typeof(ControlComponent).GetFields(BindingFlags.Public | BindingFlags.Instance);
            _controlActions = allFields.Where(f => f.FieldType == typeof(Button)).ToList();
        }
        
        private static void UpdateFromInput(ref ControlComponent controlComponent, FieldInfo field, PlayerInput playerInput)
        {
            var inputAction = playerInput.actions.FindAction(field.Name);
            
            if (inputAction == null)
                return;
            
            UpdateFromInput(ref controlComponent, field, inputAction.IsPressed());
        }
        
        private static void UpdateFromInput(ref ControlComponent controlComponent, FieldInfo field, bool pressed)
        {
            var button = (Button) field.GetValue(controlComponent);
            button.UpdatePressed(pressed);

            object boxed = controlComponent;
            field.SetValue(boxed, button);
            controlComponent = (ControlComponent) boxed;
            
            if (button.wasPressedThisFrame)
            {
                controlComponent.InsertInBuffer(field.Name);
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();
            var controlComponents = world.GetComponents<ControlComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            foreach (var entity in world.GetFilter<ControlComponent>().End())
            {
                ref var controlComponent = ref controlComponents.Get(entity);
                controlComponent.bufferTime -= Time.deltaTime;
            }

            foreach (var entity in world.GetFilter<PlayerInputComponent>().Inc<ControlComponent>().End())
            {
                ref var playerInputComponent = ref playerInputComponents.Get(entity);

                var playerInput = PlayerInput.GetPlayerByIndex(playerInputComponent.playerInput);

                if (playerInput == null)
                {
                    playerInputComponent.isControlled = false;
                    continue;
                }

                playerInputComponent.isControlled = true;
                
                ref var controlComponent = ref controlComponents.Get(entity);
                
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

                if (playerInput.currentControlScheme.Equals("Gamepad"))
                {
                    var movement = playerInput.actions.FindAction("Movement");
                    controlComponent.direction = movement.ReadValue<Vector2>();
                }
                else
                {
                    controlComponent.direction = direction;
                }
                    
            }
            
            foreach (var entity in world.GetFilter<ControlComponent>().End())
            {
                ref var controlComponent = ref controlComponents.Get(entity);
                if (controlComponent.bufferTime < 0 && controlComponent.buffer.Count > 0)
                {
                    controlComponent.buffer.Clear();
                }
            }
            
            foreach (var entity in world.GetFilter<ControlComponent>().Inc<LookingDirection>().End())
            {
                var lookingDirection = lookingDirectionComponents.Get(entity);

                ref var controlComponent = ref controlComponents.Get(entity);

                controlComponent.forward.UpdatePressed(false);
                controlComponent.backward.UpdatePressed(false);
                
                if (controlComponent.direction.x > 0 && lookingDirection.value.x > 0)
                {
                    controlComponent.forward.name = controlComponent.right.name;
                    controlComponent.backward.name = controlComponent.left.name;
                    controlComponent.forward.UpdatePressed(true);
                } else if (controlComponent.direction.x < 0 && lookingDirection.value.x < 0)
                {
                    controlComponent.forward.name = controlComponent.left.name;
                    controlComponent.backward.name = controlComponent.right.name;
                    controlComponent.forward.UpdatePressed(true);
                } else if (controlComponent.direction.x < 0 && lookingDirection.value.x > 0)
                {
                    controlComponent.backward.name = controlComponent.left.name;
                    controlComponent.forward.name = controlComponent.right.name;
                    controlComponent.backward.UpdatePressed(true);
                } else if (controlComponent.direction.x > 0 && lookingDirection.value.x < 0)
                {
                    controlComponent.backward.name = controlComponent.right.name;
                    controlComponent.forward.name = controlComponent.left.name;
                    controlComponent.backward.UpdatePressed(true);
                }
                
                // controlComponent.forward.UpdatePressed(
                //     (controlComponent.direction.x > 0 && lookingDirection.value.x > 0) || 
                //     (controlComponent.direction.x < 0 && lookingDirection.value.x < 0)); 
                //
                // controlComponent.backward.UpdatePressed(
                //     (controlComponent.direction.x < 0 && lookingDirection.value.x > 0) || 
                //     (controlComponent.direction.x > 0 && lookingDirection.value.x < 0)); 
            }
        }

    }
}