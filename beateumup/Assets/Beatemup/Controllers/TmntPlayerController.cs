using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Beatemup.Controllers
{
    public class TmntPlayerController : ControllerBase, IInit
    {
        private static readonly string[] ComboAnimations = 
        {
            "Attack2", "Attack3", "AttackFinisher"
        };

        private const string DashState = "Dash";
        private const string DashStopState = "DashStop";
        
        private const string SprintState = "Sprint";

        [FormerlySerializedAs("attackCancelationTime")] 
        public float attackCancellationTime = 0.1f;

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;
        
        public float sprintExtraSpeed = 2.0f;

        public float heavySwingStartTime = 0.5f;
        public float heavySwingChargeTime = 0.25f;

        private float pressedAttackTime = 0;

        private int comboAttacks => ComboAnimations.Length;
        private int currentComboAttack;

        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
            
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            animationComponent.Play("Idle");
            
            // animationComponent.onEvent += OnAnimationEvent;
        }

        // private void OnAnimationEvent(AnimationComponent animationComponent, int animation, int frameNumber)
        // {
        //     ref var states = ref world.GetComponent<StatesComponent>(entity);
        //     var frame = animationComponent.GetFrame(animation, frameNumber);
        //
        //     if (states.HasState("Attack"))
        //     {
        //         if (frame.events.Contains("hit"))
        //         {
        //             // check for enemies
        //             
        //             // if found, hit enemies and enable combo
        //         }
        //     }
        // }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
            // ref var modelState = ref world.GetComponent<ModelStateComponent>(entity);
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationFrameComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            State state;
            
            if (states.TryGetState("HeavySwing", out state))
            {
                if (animation.IsPlaying("HeavySwingAttack"))
                {
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        states.ExitState("HeavySwing");
                    }
                }
                
                if (animation.IsPlaying("HeavySwingFirstStrike"))
                {
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        animation.Play("HeavySwingAttack", 1);
                    }
                }
                
                if (animation.IsPlaying("HeavySwingHold"))
                {
                    if (!control.button1.isPressed)
                    {
                        if (animation.HasAnimation("HeavySwingFirstStrike"))
                        {
                            animation.Play("HeavySwingFirstStrike", 1);
                        }
                        else
                        {
                            animation.Play("HeavySwingAttack", 1);
                        }
                        return;
                    }
                }
                
                if (animation.IsPlaying("HeavySwingCharging"))
                {
                    if (!control.button1.isPressed)
                    {
                        
                        states.ExitState("HeavySwing");
                        return;
                    }

                    if (animation.playingTime > heavySwingChargeTime)
                    {
                        animation.Play("HeavySwingHold");
                    }
                }
                
                if (animation.IsPlaying("HeavySwingStartup"))
                {
                    if (!control.button1.isPressed)
                    {
                        
                        states.ExitState("HeavySwing");
                        return;
                    }
                    
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        animation.Play("HeavySwingCharging");
                    }
                }

                return;
            }

            if (states.HasState("SprintStop"))
            {
                if (animation.state == AnimationComponent.State.Completed || control.HasBufferedAction(control.button1))
                {
                    
                    states.ExitState("SprintStop");
                }
                return;
            }
            
            if (states.HasState(DashStopState))
            {
                if (animation.state == AnimationComponent.State.Completed || control.HasBufferedAction(control.button1))
                {
                    
                    states.ExitState(DashStopState);
                }

                return;
            }

            if (states.HasState("Backkick"))
            {
                if (animation.state == AnimationComponent.State.Completed)
                {
                    lookingDirection.value.x = -lookingDirection.value.x;
                    
                    states.ExitState("Backkick");
                }

                return;
            }
            
            if (states.TryGetState("Attack", out state))
            {
                // var state = states.GetState("Attack");
                
                // if (animation.)

                if (currentAnimationFrame.hit)
                {
                    Debug.Log($"HIT EVENT: {animation.currentFrame}");
                    // detect enemies in hitbox
                    // if enemies, then hit enemy and enter combo
                    
                    // Physics2D.overlap
                    
                    states.EnterState("Combo");
                }

                if (states.HasState("Combo") && animation.playingTime >= attackCancellationTime &&
                    control.HasBufferedActions(control.button1.name, control.backward.name))
                {
                    control.ConsumeBuffer();
                    
                    animation.Play("Backkick", 1);
                    states.ExitState("Attack");
                    states.ExitState("Combo");
                    states.EnterState("Backkick");
                    return;
                }

                if (states.HasState("Combo") && animation.playingTime >= attackCancellationTime && control.HasBufferedAction(control.button1) 
                    && currentComboAttack < comboAttacks)
                {
                    animation.Play(ComboAnimations[currentComboAttack], 1);

                    if (control.HasBufferedActions(control.backward.name, control.button1.name))
                    {
                        lookingDirection.value.x = -lookingDirection.value.x;
                        states.ExitState("Combo");
                    }

                    currentComboAttack++;
                    control.ConsumeBuffer();
                    
                    return;
                }
            
                if (animation.state == AnimationComponent.State.Completed)
                {
                    states.ExitState("Combo");
                    states.ExitState("Attack");
                }

                return;
            }

            if (states.TryGetState(DashState, out state))
            {
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
            
            pressedAttackTime -= dt;

            if (!control.button1.isPressed)
            {
                pressedAttackTime = heavySwingStartTime;
            }

            if (pressedAttackTime <= 0)
            {
                control.ConsumeBuffer();

                animation.Play("HeavySwingStartup", 1);
                states.EnterState("HeavySwing");
                    
                return;
            }
            
            if (control.HasBufferedAction(control.button1))
            {
                currentComboAttack = 0;
                
                if (Mathf.Abs(movement.currentVelocity.x) > Mathf.Epsilon)
                {
                    animation.Play("AttackMoving", 1);
                }
                else
                {
                    animation.Play("Attack", 1);
                }
                
                movement.movingDirection = Vector2.zero;
                
                control.ConsumeBuffer();

                states.EnterState("Attack");
                
                // states.EnterState("Combo");
                
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
                    
                    states.ExitState("Moving");
                    return;
                }

                if (control.direction.y > 0 && !animation.IsPlaying("WalkUp"))
                {
                    animation.Play("WalkUp");
                } else if (control.direction.y <= 0 && !animation.IsPlaying("Walk"))
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

            if (!animation.IsPlaying("Idle"))
            {
                animation.Play("Idle");
            }
        }

    }
}