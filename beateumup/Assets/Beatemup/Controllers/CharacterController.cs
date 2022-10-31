using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;

        public void OnInit()
        {
            
        }
        
        public void OnEntityDestroyed(Entity e)
        {

        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
            ref var modelState = ref world.GetComponent<ModelStateComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            var lookingDirection = world.GetComponent<LookingDirection>(entity);
            
            // if (states.HasState(DashState))
            // {
            //     var state = states.GetState(DashState);
            //     if (state.time > dashDuration)
            //     {
            //         modelState.dashing = false;
            //         movement.extraSpeed = 0;
            //         states.ExitState(DashState);
            //     }
            //     
            //     // ref var position = ref world.GetComponent<PositionComponent>(entity);
            //     // position.value = new Vector2(-position.value.x, position.value.y);
            //     
            //     return;
            // }

            if (states.HasState(DashState))
            {
                var state = states.GetState(DashState);
                if (state.time > dashDuration)
                {
                    modelState.dashing = false;
                    movement.extraSpeed = 0;
                    states.ExitState(DashState);
                }
                
                // ref var position = ref world.GetComponent<PositionComponent>(entity);
                // position.value = new Vector2(-position.value.x, position.value.y);
                
                return;
            }

            if (control.button1.wasPressed)
            {
                movement.movingDirection = new Vector2(lookingDirection.value.x, 0);
                modelState.dashing = true;
                movement.extraSpeed = dashExtraSpeed;
                states.EnterState(DashState);
                return;
            }
            
            movement.movingDirection = control.direction;
        }

    }
}