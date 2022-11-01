using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private const string Attack1State = "Attack1";
        
        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";

        public float attack1Duration = 1.0f;

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;
        
        public float dashStopDuration = 0.1f;

        public void OnInit()
        {
            var model = world.GetComponent<UnitModelComponent>(entity);
            var animator = model.instance.GetComponent<Animator>();

            var allClips = animator.runtimeAnimatorController.animationClips;

            foreach (var clip in allClips)
            {
                if (clip.name.Equals("Attack"))
                {
                    attack1Duration = clip.length;
                }
            }
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
            
            if (states.HasState(Attack1State))
            {
                var state = states.GetState(Attack1State);
                if (state.time >= attack1Duration)
                {
                    modelState.attack1 = false;
                    states.ExitState(Attack1State);
                }

                return;
            }
            
            
            if (states.HasState(DashStopState))
            {
                var state = states.GetState(DashStopState);
                if (state.time >= dashStopDuration)
                {
                    states.ExitState(DashStopState);
                }

                return;
            }

            if (states.HasState(DashState))
            {
                var state = states.GetState(DashState);
                
                if (state.time > dashDuration)
                {
                    movement.movingDirection = Vector2.zero;
                    modelState.dashing = false;
                    movement.extraSpeed = 0;
                    states.ExitState(DashState);
                    states.EnterState(DashStopState);
                }
                
                // ref var position = ref world.GetComponent<PositionComponent>(entity);
                // position.value = new Vector2(-position.value.x, position.value.y);
                
                return;
            }
            
            if (control.button1.wasPressed)
            {
                movement.movingDirection = Vector2.zero;
                modelState.attack1 = true;
                states.EnterState(Attack1State);
                return;
            }

            if (control.button2.wasPressed)
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