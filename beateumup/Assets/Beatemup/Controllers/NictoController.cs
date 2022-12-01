using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using UnityEngine.Serialization;
using LookingDirection = Beatemup.Ecs.LookingDirection;
using TargetingUtils = Beatemup.Ecs.TargetingUtils;

namespace Beatemup.Controllers
{
    public class NictoController : ControllerBase, IInit, IStateChanged
    {
        public float baseSpeed = 8.0f;
        
        public float dashFrontTime = 0.1f;
        public float dashBackTime = 0.1f;

        public AnimationCurve dashHeightCurve = AnimationCurve.Constant(0, 1, 0);

        public float dashSpeed = 20.0f;
        
        public Vector2 dashBackJumpSpeed = new Vector2(10f, 10f);
        
        public float dashBackJumpMaxHeight = 3;

        private Vector2 dashRecoveryDirection;
        
        public float dashRecoverySpeed = 10.0f;
        
        public float dashRecoveryTime = 0.5f;
        
        public AnimationCurve dashRecoverySpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        private bool dashBackRecoveryCanFlip = true;
        
        public float dashFrontCooldown = 5.0f / 15.0f;
        private float dashFrontCooldownCurrent = 0;
        
        public float dashBackCooldown = 5.0f / 15.0f;
        private float dashBackCooldownCurrent = 0;

        public List<string> comboAnimations = new List<string>()
        {
            "Attack2", "Attack3", "AttackFinisher"
        };
        
        public float attackCooldown = 0.1f;
        private float attackCooldownCurrent = 0;
        
        private int comboAttacks => comboAnimations.Count;
        private int currentComboAttack;

        private Vector3 teleportLastHitPosition;
        
        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
            
            ref var hitComponent = ref world.GetComponent<HitComponent>(entity);
            hitComponent.OnHitEvent += OnHit;
        }
        
        private void OnHit(World world, Entity entity, HitComponent hitComponent)
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            var position = world.GetComponent<PositionComponent>(entity);
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            if (states.HasState("DashFront") || states.HasState("DashBack"))
            {
                return;
            }

            states.EnterState("HitStun");

            // if (hitComponent.hits.Count > 0)
            // {
            //     var hitPosition = hitComponent.hits[0].position;
            //
            //     lookingDirection.value = hitPosition - position.value;
            //     lookingDirection.value.Normalize();
            // }
        }
        
        public void OnEnter()
        {
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            
            var states = world.GetComponent<StatesComponent>(entity);
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);
            
            ref var horizontalMovement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);

            var lookingDirection = world.GetComponent<LookingDirection>(entity);
            var control = world.GetComponent<ControlComponent>(entity);

            if (states.statesEntered.Contains("Moving"))
            {
                animation.Play("Walk");
            }
            
            if (states.statesEntered.Contains("DashBackRecovery"))
            {
                dashRecoveryDirection = horizontalMovement.movingDirection;
                animation.Play("DashRecovery");
            }
            
            if (states.statesEntered.Contains("DashBack"))
            {
                gravityComponent.disabled = true;
                animation.Play("DashBack", 1);
                
                horizontalMovement.movingDirection = new Vector2(-lookingDirection.value.x, control.direction.y);
            }
            
            if (states.statesEntered.Contains("DashBackJump"))
            {
                // jump.isActive = true;
                verticalMovement.speed = dashBackJumpSpeed.y;
                
                gravityComponent.disabled = true;
                animation.Play("DashBack", 1);
                states.EnterState("DashBackJump.Up");
            }
            
            if (states.statesEntered.Contains("DashFront"))
            {
                gravityComponent.disabled = true;
                animation.Play("DashFront", 1);

                // var directionX = control.direction.x;
                //
                // if (Mathf.Abs(directionX) < 0.01f)
                // {
                //     directionX = lookingDirection.value.x;
                // }
                
                horizontalMovement.movingDirection = new Vector2(control.direction.x, control.direction.y);

                if (horizontalMovement.movingDirection.SqrMagnitude() < 0.01f)
                {
                    horizontalMovement.movingDirection = lookingDirection.value;
                }
            }
            
            if (states.statesEntered.Contains("HitStun"))
            {
                states.ExitState("Attack");
                states.ExitState("Combo");
                
                // states.ExitState("DashFront");
                // states.ExitState("DashBack");
            }
        }

        public void OnExit()
        {
            var states = world.GetComponent<StatesComponent>(entity);
            
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);
            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            
            if (states.statesExited.Contains("DashBackJump"))
            {
                position.value.z = 0;
                movement.baseSpeed = 0;
                gravityComponent.disabled = false;
                
                // exit all sub states, for now manually
                states.ExitState("DashBackJump.Up");
                states.ExitState("DashBackJump.Fall");
            }
            
            if (states.statesExited.Contains("DashBack"))
            {
                position.value.z = 0;
                movement.baseSpeed = 0;
                gravityComponent.disabled = false;

                //movement.movingDirection.y = 0;
            }
            
            if (states.statesExited.Contains("DashFront"))
            {
                position.value.z = 0;
                movement.baseSpeed = 0;
                gravityComponent.disabled = false;
                
                // movement.movingDirection.y = 0;
            }
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);

            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);

            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationAttackComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            // ref var jump = ref world.GetComponent<JumpComponent>(entity);

            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            State state;
            
            if (states.TryGetState("HitStun", out state))
            {
                movement.movingDirection = Vector2.zero;
                
                if (!animation.IsPlaying("HitStun"))
                {
                    animation.Play("HitStun", 1);
                }

                if (animation.state == AnimationComponent.State.Completed)
                {
                    states.ExitState("HitStun");
                }
                
                return;
            }

            if (states.TryGetState("DashBackRecovery", out state))
            {
                // movement.movingDirection = Vector2.zero;
                
                // TODO: set direction from caller 

                movement.movingDirection = dashRecoveryDirection;
                movement.baseSpeed = dashRecoverySpeedCurve.Evaluate(state.time / dashRecoveryTime) * dashRecoverySpeed;
                
                if (dashBackRecoveryCanFlip && control.backward.isPressed)
                {
                    lookingDirection.value.x = -lookingDirection.value.x;
                }
                
                if (state.time > dashRecoveryTime)
                {
                    states.ExitState("DashBackRecovery");
                    dashBackRecoveryCanFlip = true;
                }

                if (control.HasBufferedAction(control.button1) && attackCooldownCurrent <= 0)
                {
                    states.ExitState("DashBackRecovery");
                    dashBackRecoveryCanFlip = true;

                    // if (control.backward.isPressed)
                    // {
                    //     lookingDirection.value.x = control.direction.x;
                    // }
                    
                    currentComboAttack = 0;
                    animation.Play("Attack1", 1);
                    movement.movingDirection = Vector2.zero;
                    control.ConsumeBuffer();
                    states.EnterState("Attack");
                
                    return;
                }
                
                dashBackCooldownCurrent -= Time.deltaTime;
                dashFrontCooldownCurrent -= Time.deltaTime;
                
                return;
            }
            
            if (states.TryGetState("DashBackJump", out state))
            {
                if (states.HasState("DashBackJump.Attack"))
                {
                    movement.movingDirection = -lookingDirection.value;
                    movement.baseSpeed = dashBackJumpSpeed.x;

                    // check for event to fire kunais!

                    if (position.value.z >= dashBackJumpMaxHeight)
                    {
                        position.value.z = dashBackJumpMaxHeight;
                        gravityComponent.disabled = false;
                        verticalMovement.speed = 0;
                    }
                    
                    if (animation.state == AnimationComponent.State.Completed)
                    {
                        animation.Play("BackJump");
                        
                        states.ExitState("DashBackJump.Attack");
                        states.EnterState("DashBackJump.Fall");
                    }

                    return;
                }
                
                if (states.HasState("DashBackJump.Up"))
                {
                    movement.movingDirection = -lookingDirection.value;
                    movement.baseSpeed = dashBackJumpSpeed.x;

                    if (control.HasBufferedActions(control.button1.name))
                    {
                        animation.Play("AirAttack", 1);
                        states.ExitState("DashBackJump.Up");
                        states.EnterState("DashBackJump.Attack");
                        
                        return;
                    }
                    
                    if (position.value.z >= dashBackJumpMaxHeight)
                    {
                        position.value.z = dashBackJumpMaxHeight;
                        
                        gravityComponent.disabled = false;
                        verticalMovement.speed = 0;

                        animation.Play("BackJump");
                        
                        states.ExitState("DashBackJump.Up");
                        states.EnterState("DashBackJump.Fall");
                    }

                    return;
                }
                
                if (states.HasState("DashBackJump.Fall"))
                {
                    movement.movingDirection = -lookingDirection.value;
                    movement.baseSpeed = dashBackJumpSpeed.x * 0.75f;
                    
                    if (verticalMovement.isOverGround)
                    {
                        states.EnterState("DashBackRecovery");
                        states.ExitState("DashBackJump");
                        states.ExitState("DashBackJump.Fall");
                    }

                    return;
                }
                
                return;
            }
            
            if (states.TryGetState("DashBack", out state))
            {
                dashBackCooldownCurrent = dashBackCooldown;
                
                // movement.movingDirection = new Vector2(-lookingDirection.value.x, control.direction.y);
                movement.baseSpeed = dashSpeed;
                
                position.value.z = dashHeightCurve.Evaluate(state.time / dashBackTime);

                if (state.time > dashBackTime)
                {
                    states.ExitState(state.name);
                    states.EnterState("DashBackRecovery");
                }
                
                return;
            }
            
            if (states.TryGetState("DashFront", out state))
            {
                dashFrontCooldownCurrent = dashFrontCooldown;
                
                // movement.movingDirection = new Vector2(lookingDirection.value.x, control.direction.y);
                movement.baseSpeed = dashSpeed;
                
                position.value.z = dashHeightCurve.Evaluate(state.time / dashFrontTime);

                if (state.time > dashFrontTime)
                {
                    states.ExitState(state.name);
                    states.EnterState("DashBackRecovery");
                }
                
                return;
            }

            dashBackCooldownCurrent -= Time.deltaTime;
            dashFrontCooldownCurrent -= Time.deltaTime;

            if (states.TryGetState("HiddenAttack", out state))
            {
                if (animation.IsPlaying("TeleportOut") && animation.state == AnimationComponent.State.Completed)
                {
                    position.value = teleportLastHitPosition;
                    position.value.x += lookingDirection.value.x * 3;
                    
                    animation.Play("TeleportIn", 1);
                    return;
                }
                
                if (animation.IsPlaying("TeleportIn") && animation.state == AnimationComponent.State.Completed)
                {
                    lookingDirection.value.x = -lookingDirection.value.x;
                    
                    states.ExitState("HiddenAttack");
                    
                    // states.EnterState("Attack");
                    // currentComboAttack = comboAttacks;
                    // animation.Play("TeleportFinisher", 1);
                    
                    // dont allow flip! 
                    dashBackRecoveryCanFlip = false;
                    states.EnterState("DashBackRecovery");

                    // reset dash cooldown
                    dashFrontCooldownCurrent = dashFrontCooldown;
                    
                    // if (states.HasState("Combo"))
                    // {
                    //
                    //     
                    //     // animation.Play(ComboAnimations[currentComboAttack], 1);
                    //     // currentComboAttack++;
                    // }
                    
                    return;
                }

                return;
            }

            if (states.TryGetState("Attack", out state))
            {
                if (currentAnimationFrame.currentFrameHit)
                {
                    var hitTargets = world.GetTargets(entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget.entity);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value,
                            finisher = currentComboAttack >= comboAttacks
                        });
                        
                        var targetPosition = world.GetComponent<PositionComponent>(hitTarget.entity);

                        // position.value = new Vector3(position.value.x, targetPosition.value.y - 0.1f, position.value.z);
                        
                        states.EnterState("Combo");

                        animation.pauseTime = TmntConstants.HitAnimationPauseTime;

                        teleportLastHitPosition = targetPosition.value;
                    }
                }
                
                // if (animation.playingTime >= currentAnimationFrame.cancellationTime 
                //     && currentComboAttack < comboAttacks
                //     && dashBackCooldownCurrent <= 0 
                //     && (control.HasBufferedActions(control.up.name, control.button2.name) ||
                //         control.HasBufferedActions(control.button2.name, control.up.name)))
                // {
                //     control.ConsumeBuffer();
                //     states.ExitState("Attack");
                //     states.ExitState("Combo");
                //     states.EnterState("DashBackJump");
                //     return;
                // }
                
                if (animation.playingTime >= currentAnimationFrame.cancellationTime 
                    && currentComboAttack < comboAttacks
                    && dashBackCooldownCurrent <= 0 
                    && (control.HasBufferedActions(control.backward.name, control.button2.name) ||
                    control.HasBufferedActions(control.button2.name, control.backward.name)))
                {
                    control.ConsumeBuffer();
                    states.ExitState("Attack");
                    states.ExitState("Combo");
                    states.EnterState("DashBack");
                    return;
                }

                /*if (states.HasState("Combo") && animation.playingTime >= attackCancellationTime && 
                    (control.HasBufferedActions(control.forward.name, control.button2.name) ||
                    control.HasBufferedActions(control.button2.name, control.forward.name))*/
                
                if (animation.HasAnimation("TeleportOut") && states.HasState("Combo") && animation.playingTime >= currentAnimationFrame.cancellationTime && 
                    control.HasBufferedActions(control.button2.name)
                     && dashFrontCooldownCurrent <= 0
                    && currentComboAttack < comboAttacks)
                {
                    control.ConsumeBuffer();
                    
                    animation.Play("TeleportOut", 1);
                    states.ExitState("Attack");
                    states.ExitState("Combo");
                    
                    states.EnterState("HiddenAttack");
                    return;
                }
                
                if (animation.playingTime >= currentAnimationFrame.cancellationTime 
                    && currentComboAttack < comboAttacks
                    && dashFrontCooldown <= 0 
                    && control.HasBufferedActions(control.button2.name))
                {
                    control.ConsumeBuffer();
                    states.ExitState("Attack");
                    states.ExitState("Combo");
                    states.EnterState("DashFront");
                    return;
                }

                if (states.HasState("Combo") && animation.playingTime >= currentAnimationFrame.cancellationTime && control.HasBufferedAction(control.button1) 
                    && currentComboAttack < comboAttacks)
                {
                    animation.Play(comboAnimations[currentComboAttack], 1);

                    // if (control.HasBufferedActions(control.backward.name, control.button1.name))
                    // {
                    //     lookingDirection.value.x = -lookingDirection.value.x;
                    //     states.ExitState("Combo");
                    // }

                    currentComboAttack++;
                    control.ConsumeBuffer();
                    
                    return;
                }
            
                if (animation.state == AnimationComponent.State.Completed)
                {
                    states.ExitState("Combo");
                    states.ExitState("Attack");

                    // if combo completed, then reset attack cooldown
                    if (currentComboAttack >= comboAttacks)
                    {
                        attackCooldownCurrent = attackCooldown;
                    }
                }

                return;
            }

            attackCooldownCurrent -= Time.deltaTime;
            
            if (control.HasBufferedAction(control.button1) && attackCooldownCurrent <= 0)
            {
                if (control.backward.isPressed)
                {
                    lookingDirection.value.x = -lookingDirection.value.x;
                }
                
                currentComboAttack = 0;
                
                animation.Play("Attack1", 1);
                movement.movingDirection = Vector2.zero;
                control.ConsumeBuffer();
                states.EnterState("Attack");
                
                // states.EnterState("Combo");
                
                return;
            }
            
            // if (dashBackCooldownCurrent <= 0)
            // {
            //     if (control.HasBufferedActions(control.up.name, control.button2.name) ||
            //         control.HasBufferedActions(control.button2.name, control.up.name))
            //     {
            //         control.ConsumeBuffer();
            //         states.EnterState("DashBackJump");
            //         return;
            //     }
            // }
            
            // var validDashFrontDirections =
            //     control.forward.isPressed || control.up.isPressed || control.down.isPressed;
            
            if (dashFrontCooldownCurrent <= 0)
            {
                
                if (control.HasBufferedAction(control.button2))
                {
                    control.ConsumeBuffer();
                    states.EnterState("DashFront");
                    return;
                }
            }

            // if (dashBackCooldownCurrent <= 0)
            // {
            //     if (control.HasBufferedAction(control.button2) && !validDashFrontDirections)
            //     {
            //         control.ConsumeBuffer();
            //         states.EnterState("DashBack");
            //         return;
            //     }
            // }
            


            movement.baseSpeed = baseSpeed;
            movement.movingDirection = control.direction;

            if (states.HasState("Moving"))
            {
                if (!animation.IsPlaying("Walk"))
                {
                    animation.Play("Walk");
                }
                
                if (control.backward.isPressed)
                {
                    lookingDirection.value.x = control.direction.x;
                }
                
                if (control.direction.sqrMagnitude < Mathf.Epsilon)
                {
                    states.ExitState("Moving");
                    return;
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