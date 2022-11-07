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

        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";
        
        private const string SprintState = "Sprint";

        public float attackCancelationTime = 0.1f;

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;
        
        public float sprintExtraSpeed = 2.0f;
        
        private float _dashStopDuration = 0.1f;

        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
            
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            animationComponent.Play("Idle");
        }
        
        public void OnEntityDestroyed(Entity e)
        {

        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
            // ref var modelState = ref world.GetComponent<ModelStateComponent>(entity);
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            
            if (states.HasState("SprintStop"))
            {
                if (animation.state == AnimationComponent.State.Completed || control.HasBufferedAction(control.button1))
                {
                    animation.Play("Idle");
                    states.ExitState("SprintStop");
                }
                return;
            }
            
            if (states.HasState(DashStopState))
            {
                var state = states.GetState(DashStopState);
                if (animation.state == AnimationComponent.State.Completed || control.HasBufferedAction(control.button1))
                {
                    animation.Play("Idle");
                    states.ExitState(DashStopState);
                }

                return;
            }

            for (int i = 0; i < AttackStates.Length; i++)
            {
                if (states.HasState(AttackStates[i]))
                {
                    var state = states.GetState(AttackStates[i]);
                    
                    if (state.time >= attackCancelationTime && control.HasBufferedAction(control.button1) 
                                                            && i < AttackStates.Length - 1)
                    {
                        animation.Play(AttackStates[i + 1], 1);
                        
                        // modelState.attackMoving = false;
                        // modelState.states[AttackStates[i]] = false;
                        // modelState.states[AttackStates[i + 1]] = true;

                        state.time = 0;
                        
                        if (control.HasBufferedActions(control.backward.name, control.button1.name))
                        {
                            lookingDirection.value.x = -lookingDirection.value.x;
                            // TODO: should also stop combo
                        }

                        states.ExitState(AttackStates[i]);
                        states.EnterState(AttackStates[i + 1]);

                        control.ConsumeBuffer();
                        
                        return;
                    }
                
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        animation.Play("Idle");
                        
                        // modelState.attackMoving = false;
                        // modelState.states[AttackStates[i]] = false;
                        
                        states.ExitState(AttackStates[i]);
                    }

                    return;
                }
            }




            if (states.HasState(DashState))
            {
                var state = states.GetState(DashState);
                
                if (state.time > dashDuration)
                {
                    movement.movingDirection = Vector2.zero;
                    // modelState.dashing = false;
                    animation.Play("DashStop", 1);
                    movement.extraSpeed.x = 0;
                    states.ExitState(DashState);
                    states.EnterState(DashStopState);
                }
                
                // ref var position = ref world.GetComponent<PositionComponent>(entity);
                // position.value = new Vector2(-position.value.x, position.value.y);
                
                return;
            }
            
            if (control.HasBufferedAction(control.button1))
            {
                if (Mathf.Abs(movement.currentVelocity.x) > Mathf.Epsilon)
                {
                    // modelState.attackMoving = true;
                    
                    // TODO: recover attack moving
                    
                    animation.Play("AttackMoving", 1);
                    // modelState.attackMoving = true;
                }
                else
                {
                    animation.Play(AttackStates[0], 1);
                    // modelState.states[AttackStates[0]] = true;
                    // _currentAttackDuration = _attackDuration[0];
                }
                
                movement.movingDirection = Vector2.zero;
                
                // lookingDirection.locked = true;
                control.ConsumeBuffer();
                
                states.EnterState(AttackStates[0]);
                return;
            }

            if (control.HasBufferedAction(control.button2))
            {
                control.ConsumeBuffer();
                
                states.ExitState(SprintState);
                movement.extraSpeed.x = 0;
                
                movement.movingDirection = new Vector2(lookingDirection.value.x, 0);
                animation.Play("Dash", 1);
                movement.extraSpeed.x = dashExtraSpeed;
                states.EnterState(DashState);
                
                return;
            }

            if (states.HasState(SprintState))
            {
                if ((!control.right.isPressed && !control.left.isPressed) || 
                    (control.right.isPressed && control.left.isPressed) || control.backward.isPressed)
                {
                    // modelState.sprinting = false;
                    movement.extraSpeed.x = 0;
                    movement.movingDirection = Vector2.zero;
                    
                    animation.Play("SprintStop", 1);
                    states.ExitState(SprintState);
                    states.EnterState("SprintStop");
                    return;
                }
                
                movement.movingDirection = control.direction;

                return;
            }
            else
            {
                if (control.HasBufferedActions(control.forward.name, control.forward.name))
                {
                    control.ConsumeBuffer();
                    
                    animation.Play("Sprint");
                    // modelState.sprinting = true;
                    movement.extraSpeed.x = sprintExtraSpeed;
                    states.EnterState(SprintState);
                    states.ExitState("Moving");
                    return;
                }
                
                // if (control.right.isPressed != control.left.isPressed)
                // {
                //
                // }
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
                    animation.Play("Idle");
                    states.ExitState("Moving");
                    return;
                }

                if (control.direction.y > 0 && animation.IsPlaying("Walk"))
                {
                    animation.Play("WalkUp");
                } else if (control.direction.y <= 0 && animation.IsPlaying("WalkUp"))
                {
                    animation.Play("Walk");
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
                animation.Play("Walk");
                states.EnterState("Moving");
                return;
            }
        }

    }
}