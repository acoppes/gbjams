using System;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private static readonly string[] AttackStates = new string []
        {
            "attack", "attack2", "attack3", "attackFinisher"
        };
        
        // private static readonly string[] Combo2States = new string []
        // {
        //     "AttackMoving", "Attack2" //, "Attack3", "AttackFinisher"
        // };

        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";
        
        private const string SprintState = "Sprint";
        // private const string DashStopState = "DashStop";

        public float attackCancelationTime = 0.1f;

        private float[] _attackDuration;
        
        private float _attackMovingDuration = 1.0f;

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;
        
        public float sprintExtraSpeed = 2.0f;
        
        private float _dashStopDuration = 0.1f;

        public void OnInit()
        {
            var model = world.GetComponent<UnitModelComponent>(entity);
            var animator = model.instance.GetComponent<Animator>();

            var allClips = animator.runtimeAnimatorController.animationClips;

            _attackDuration = new float[AttackStates.Length];

            foreach (var clip in allClips)
            {
                for (var i = 0; i < AttackStates.Length; i++)
                {
                    var attackState = AttackStates[i];
                    
                    if (clip.name.Equals(attackState, StringComparison.OrdinalIgnoreCase))
                    {
                        _attackDuration[i] = clip.length;
                    }
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
            
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
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

            for (int i = 0; i < AttackStates.Length; i++)
            {
                if (states.HasState(AttackStates[i]))
                {
                    var state = states.GetState(AttackStates[i]);

                    if (state.time >= attackCancelationTime && control.HasBufferedAction(control.button1, 1) 
                                                            && i < AttackStates.Length - 1)
                    {
                        modelState.attackMoving = false;
                     
                        modelState.states[AttackStates[i]] = false;
                        modelState.states[AttackStates[i + 1]] = true;

                        state.time = 0;
                    
                        states.ExitState(AttackStates[i]);
                        states.EnterState(AttackStates[i + 1]);

                        control.ConsumeBuffer();
                        
                        return;
                    }
                
                    if (state.time >= _attackDuration[i])
                    {
                        modelState.attackMoving = false;
                        modelState.states[AttackStates[i]] = false;
                        states.ExitState(AttackStates[i]);
                    }

                    return;
                }
            }


            if (states.HasState(DashStopState))
            {
                var state = states.GetState(DashStopState);
                if (state.time >= _dashStopDuration || control.HasBufferedAction(control.button1, 1))
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
                    movement.extraSpeed.x = 0;
                    states.ExitState(DashState);
                    states.EnterState(DashStopState);
                }
                
                // ref var position = ref world.GetComponent<PositionComponent>(entity);
                // position.value = new Vector2(-position.value.x, position.value.y);
                
                return;
            }
            
            if (control.HasBufferedAction(control.button1, 1))
            {
                if (Mathf.Abs(movement.currentVelocity.x) > Mathf.Epsilon)
                {
                    // modelState.attackMoving = true;
                    
                    // TODO: recover attack moving
                    
                    modelState.attackMoving = true;
                }
                else
                {
                    modelState.states[AttackStates[0]] = true;
                    // _currentAttackDuration = _attackDuration[0];
                }
                
                movement.movingDirection = Vector2.zero;
                
                // lookingDirection.locked = true;
                control.ConsumeBuffer();
                
                states.EnterState(AttackStates[0]);
                return;
            }

            if (control.HasBufferedAction(control.button2, 1))
            {
                // exit sprint
                states.ExitState(SprintState);
                modelState.sprinting = false;
                movement.extraSpeed.x = 0;
                
                movement.movingDirection = new Vector2(lookingDirection.value.x, 0);
                modelState.dashing = true;
                movement.extraSpeed.x = dashExtraSpeed;
                // lookingDirection.locked = true;
                states.EnterState(DashState);
                
                return;
            }

            if (states.HasState(SprintState))
            {
                if ((!control.right.isPressed && !control.left.isPressed) || 
                    (control.right.isPressed && control.left.isPressed) || control.backward.isPressed)
                {
                    modelState.sprinting = false;
                    movement.extraSpeed.x = 0;
                    states.ExitState(SprintState);
                    return;
                }
            }
            else
            {
                if (control.right.isPressed != control.left.isPressed)
                {
                    if ((control.right.isPressed && control.HasBufferedAction(control.right.name, 2)) ||
                        (control.left.isPressed && control.HasBufferedAction(control.left.name, 2)))
                    {
                        modelState.sprinting = true;
                        movement.extraSpeed.x = sprintExtraSpeed;
                        states.EnterState(SprintState);
                        return;
                    }
                }
            }
            
            movement.movingDirection = control.direction;

            if (states.HasState("Moving"))
            {
                if (control.backward.isPressed)
                {
                    lookingDirection.value.x = control.direction.x;
                }
                
                if (control.direction.sqrMagnitude < Mathf.Epsilon)
                {
                    // lookingDirection.locked = false;
                    states.ExitState("Moving");
                }

                return;
            }

            if (control.direction.sqrMagnitude > Mathf.Epsilon)
            {
                if (control.backward.isPressed)
                {
                    lookingDirection.value.x = control.direction.x;
                }
                
                // lookingDirection.locked = true;
                states.EnterState("Moving");
                return;
            }
        }

    }
}