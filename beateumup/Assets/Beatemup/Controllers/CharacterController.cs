using System;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private const string AttackState = "Attack";
        
        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";
        
        private const string SprintState = "Sprint";
        // private const string DashStopState = "DashStop";

        private float _attackDuration = 1.0f;
        private float _attackMovingDuration = 1.0f;

        private float _currentAttackDuration;

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;
        
        public float sprintExtraSpeed = 2.0f;
        
        private float _dashStopDuration = 0.1f;

        public void OnInit()
        {
            var model = world.GetComponent<UnitModelComponent>(entity);
            var animator = model.instance.GetComponent<Animator>();

            var allClips = animator.runtimeAnimatorController.animationClips;

            foreach (var clip in allClips)
            {
                if (clip.name.Equals("Attack", StringComparison.OrdinalIgnoreCase))
                {
                    _attackDuration = clip.length;
                }
                
                if (clip.name.Equals("AttackMoving", StringComparison.OrdinalIgnoreCase))
                {
                    _attackMovingDuration = clip.length;
                }
                
                if (clip.name.Equals("DashStop", StringComparison.OrdinalIgnoreCase))
                {
                    _dashStopDuration = clip.length;
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
            
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            
            if (states.HasState(AttackState))
            {
                var state = states.GetState(AttackState);
                
                if (state.time >= _currentAttackDuration)
                {
                    modelState.attack = false;
                    modelState.attackMoving = false;
                    
                    lookingDirection.locked = false;
                    states.ExitState(AttackState);
                }

                return;
            }
            
            
            if (states.HasState(DashStopState))
            {
                var state = states.GetState(DashStopState);
                if (state.time >= _dashStopDuration)
                {
                    lookingDirection.locked = false;
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
            
            if (control.button1.isPressed)
            {
                if (control.forward.isPressed)
                {
                    modelState.attackMoving = true;
                    _currentAttackDuration = _attackMovingDuration;
                }
                else
                {
                    modelState.attack = true;
                    _currentAttackDuration = _attackDuration;
                }
               
                
                movement.movingDirection = Vector2.zero;
                
                
                lookingDirection.locked = true;
                states.EnterState(AttackState);
                return;
            }

            if (control.button2.isPressed)
            {
                // exit sprint
                states.ExitState(SprintState);
                modelState.sprinting = false;
                movement.extraSpeed = 0;
                
                movement.movingDirection = new Vector2(lookingDirection.value.x, 0);
                modelState.dashing = true;
                movement.extraSpeed = dashExtraSpeed;
                lookingDirection.locked = true;
                states.EnterState(DashState);
                
                return;
            }
            
            movement.movingDirection = control.direction;

            if (states.HasState(SprintState))
            {
                if (!control.forward.isPressed || control.backward.isPressed)
                {
                    modelState.sprinting = false;
                    movement.extraSpeed = 0;
                    states.ExitState(SprintState);
                    return;
                }
            }
            else
            {
                if (control.forward.isPressed && control.forward.doubleTap)
                {
                    modelState.sprinting = true;
                    movement.extraSpeed = sprintExtraSpeed;
                    states.EnterState(SprintState);
                    return;
                }
            }


        }

    }
}