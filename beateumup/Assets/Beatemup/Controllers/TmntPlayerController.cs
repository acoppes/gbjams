using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Controllers
{
    public class TmntPlayerController : ControllerBase, IInit
    {
        private static readonly string[] ComboAnimations = 
        {
            "Attack2", "Attack3", "AttackFinisher"
        };

        // private const string DashState = "Dash";
        // private const string DashStopState = "DashStop";
        
        private const string SprintState = "Sprint";

        public float attackCancellationTime = 0.1f;

        // public float dashDuration = 1.0f;
        // public float dashExtraSpeed = 10.0f;
        
        public float sprintExtraSpeed = 2.0f;

        public float heavySwingStartTime = 0.5f;
        public float heavySwingChargeTime = 0.25f;

        public float hitStunTime = 0.25f;

        public float hitAnimationPauseTime = 1.0f;

        private float pressedAttackTime = 0;

        private int comboAttacks => ComboAnimations.Length;
        private int currentComboAttack;

        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
            
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            animationComponent.Play("Idle");
            
            ref var hitComponent = ref world.GetComponent<HitComponent>(entity);
            hitComponent.OnHitEvent += OnHit;
        }

        private void OnHit(World world, Entity entity, HitComponent hitComponent)
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            var position = world.GetComponent<PositionComponent>(entity);
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            states.EnterState("HitStun");

            if (hitComponent.hits.Count > 0)
            {
                var hitPosition = hitComponent.hits[0].position;

                lookingDirection.value = hitPosition - position.value;
            }
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);

            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);

            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationFrameComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            ref var jump = ref world.GetComponent<JumpComponent>(entity);

            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            State state;

            if (states.TryGetState("HitStun", out state))
            {
                if (!animation.IsPlaying("HitStun"))
                {
                    animation.Play("HitStun");
                }

                if (state.time > hitStunTime)
                {
                    states.ExitState("HitStun");
                }
                
                return;
            }

            if (states.TryGetState("DiveKick", out state))
            {
                if (animation.IsPlaying("DivekickStartup"))
                {
                    animation.Play("DivekickLoop");
                }

                if (currentAnimationFrame.hit)
                {
                    var hitTargets = HitBoxUtils.GetTargets(world, entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value
                        });
                            
                        animation.pauseTime = hitAnimationPauseTime;
                    }
                }
                
                movement.movingDirection.y = 0;
                movement.movingDirection.x = lookingDirection.value.x;
                
                // movement.movingDirection.z = -2;
                    
                if (verticalMovement.isOverGround)
                {
                    states.ExitState("DiveKick");
                    gravityComponent.disabled = false;
                }

                return;
            }

            if (states.TryGetState("Jump", out state))
            {
                movement.movingDirection = control.direction;
                
                if (control.backward.isPressed)
                {
                    lookingDirection.value.x = control.direction.x;
                }
                
                if (animation.IsPlaying("JumpUp"))
                {
                    if (!control.button2.isPressed)
                    {
                        jump.isActive = false;
                        gravityComponent.disabled = false;
                        
                        animation.Play("JumpRoll", 1);
                    }

                    return;
                }
                
                if (animation.IsPlaying("JumpRoll"))
                {
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        animation.Play("JumpFall");
                        return;
                    }

                    if (control.HasBufferedAction(control.button1))
                    {
                        control.ConsumeBuffer();
                        
                        states.ExitState("Jump");
                        states.EnterState("DiveKick");
                        
                        animation.Play("DivekickStartup", 1);
                        return;
                    }
                    
                    if (verticalMovement.isOverGround)
                    {
                        states.ExitState("Jump");
                    }

                    return;
                }
                
                if (animation.IsPlaying("JumpFall"))
                {
                    if (control.HasBufferedAction(control.button1))
                    {
                        control.ConsumeBuffer();

                        states.ExitState("Jump");
                        states.EnterState("DiveKick");
                        
                        animation.Play("DivekickStartup", 1);
                    }

                    if (verticalMovement.isOverGround)
                    {
                        states.ExitState("Jump");
                    }

                    return;
                }
                
                return;
            }

            if (states.TryGetState("HeavySwing", out state))
            {
                if (animation.IsPlaying("HeavySwingAttack"))
                {
                    if (currentAnimationFrame.hit)
                    {
                        var hitTargets = HitBoxUtils.GetTargets(world, entity);

                        foreach (var hitTarget in hitTargets)
                        {
                            ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                            hitComponent.hits.Add(new HitData
                            {
                                position = position.value
                            });
                            
                            animation.pauseTime = hitAnimationPauseTime;
                        }
                    }
                    
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        states.ExitState("HeavySwing");
                    }
                }
                
                if (animation.IsPlaying("HeavySwingFirstStrike"))
                {
                    if (currentAnimationFrame.hit)
                    {
                        var hitTargets = HitBoxUtils.GetTargets(world, entity);

                        foreach (var hitTarget in hitTargets)
                        {
                            ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                            hitComponent.hits.Add(new HitData
                            {
                                position = position.value
                            });
                            
                            animation.pauseTime = hitAnimationPauseTime;
                        }
                    }
                    
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
            
            // if (states.HasState(DashStopState))
            // {
            //     if (animation.state == AnimationComponent.State.Completed || control.HasBufferedAction(control.button1))
            //     {
            //         states.ExitState(DashStopState);
            //     }
            //
            //     return;
            // }

            if (states.HasState("Backkick"))
            {
                if (currentAnimationFrame.hit)
                {
                    var hitTargets = HitBoxUtils.GetTargets(world, entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value
                        });
                        
                        animation.pauseTime = hitAnimationPauseTime;
                    }
                }
                
                if (animation.state == AnimationComponent.State.Completed)
                {
                    lookingDirection.value.x = -lookingDirection.value.x;
                    states.ExitState("Backkick");
                }

                return;
            }
            
            if (states.TryGetState("Attack", out state))
            {
                if (currentAnimationFrame.hit)
                {
                    var hitTargets = HitBoxUtils.GetTargets(world, entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value
                        });
                        
                        var targetPosition = world.GetComponent<PositionComponent>(hitTarget);

                        position.value = new Vector3(position.value.x, targetPosition.value.y - 0.1f, position.value.z);
                        
                        states.EnterState("Combo");

                        animation.pauseTime = hitAnimationPauseTime;
                    }
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

            // if (states.TryGetState(DashState, out state))
            // {
            //     if (state.time > dashDuration)
            //     {
            //         movement.movingDirection = Vector2.zero;
            //         // modelState.dashing = false;
            //         animation.Play("DashStop", 1);
            //         movement.extraSpeed.x = 0;
            //         states.ExitState(DashState);
            //         states.EnterState(DashStopState);
            //     }
            //     
            //     // ref var position = ref world.GetComponent<PositionComponent>(entity);
            //     // position.value = new Vector2(-position.value.x, position.value.y);
            //     
            //     return;
            // }
            
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
            
            if (verticalMovement.isOverGround && control.HasBufferedAction(control.button2))
            {
                control.ConsumeBuffer();
                
                states.ExitState(SprintState);
                movement.extraSpeed.x = 0;
                
                animation.Play("JumpUp", 1);
                states.EnterState("Jump");

                jump.isActive = true;
                gravityComponent.disabled = true;
                
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
                
                if (!animation.IsPlaying("Sprint"))
                {
                    animation.Play("Sprint");
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