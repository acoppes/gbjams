using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Controllers
{
    public class NictoController : ControllerBase, IInit, IStateChanged
    {
        private static readonly string[] ComboAnimations = 
        {
            "Attack2", "Attack3", "AttackFinisher"
        };

        public float dashFrontTime = 0.1f;
        public float dashBackTime = 0.1f;

        public float dashFrontSpeed = 3.0f;
        public float dashBackSpeed = 3.0f;

        public float dashBackRecoveryTime = 0.5f;

        public float dashFrontCooldown = 5.0f / 15.0f;
        private float dashFrontCooldownCurrent = 0;
        
        public float dashBackCooldown = 5.0f / 15.0f;
        private float dashBackCooldownCurrent = 0;

        public float attackCooldown = 0.1f;
        private float attackCooldownCurrent = 0;
        
        private int comboAttacks => ComboAnimations.Length;
        private int currentComboAttack;

        private Vector3 teleportLastHitPosition;
        
        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
        }
        
        public void OnEnter()
        {
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            
            var states = world.GetComponent<StatesComponent>(entity);
            
            if (states.statesEntered.Contains("Moving"))
            {
                animation.Play("Walk");
            }
            
            if (states.statesEntered.Contains("DashBackRecovery"))
            {
                animation.Play("DashBackRecovery");
            }
            
            if (states.statesEntered.Contains("DashBack"))
            {
                animation.Play("DashBack");
            }
            
            if (states.statesEntered.Contains("DashFront"))
            {
                animation.Play("DashFront");
            }
        }

        public void OnExit()
        {
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);

            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            // ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);
            // ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);

            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationAttackComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            // ref var jump = ref world.GetComponent<JumpComponent>(entity);

            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            
            State state;

            if (states.TryGetState("DashBackRecovery", out state))
            {
                movement.movingDirection = Vector2.zero;
                
                if (state.time > dashBackRecoveryTime)
                {
                    states.ExitState("DashBackRecovery");
                }
                
                return;
            }
            
            if (states.TryGetState("DashBack", out state))
            {
                dashBackCooldownCurrent = dashBackCooldown;
                
                movement.movingDirection = -lookingDirection.value;
                movement.extraSpeed = new Vector2(dashBackSpeed, 0);
                
                if (state.time > dashBackTime)
                {
                    movement.extraSpeed = Vector2.zero;
                    states.ExitState(state.name);
                    
                    states.EnterState("DashBackRecovery");
                }
                
                return;
            }
            
            if (states.TryGetState("DashFront", out state))
            {
                dashFrontCooldownCurrent = dashFrontCooldown;
                
                movement.movingDirection = lookingDirection.value;
                movement.extraSpeed = new Vector2(dashFrontSpeed, 0);
                
                if (state.time > dashFrontTime)
                {
                    movement.extraSpeed = Vector2.zero;
                    states.ExitState(state.name);
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
                    
                    states.EnterState("Attack");
                    currentComboAttack = comboAttacks;
                    animation.Play("TeleportFinisher", 1);

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
                    var hitTargets = HitBoxUtils.GetTargets(world, entity);

                    foreach (var hitTarget in hitTargets)
                    {
                        ref var hitComponent = ref world.GetComponent<HitComponent>(hitTarget);
                        hitComponent.hits.Add(new HitData
                        {
                            position = position.value
                        });
                        
                        var targetPosition = world.GetComponent<PositionComponent>(hitTarget);

                        // position.value = new Vector3(position.value.x, targetPosition.value.y - 0.1f, position.value.z);
                        
                        states.EnterState("Combo");

                        animation.pauseTime = TmntConstants.HitAnimationPauseTime;

                        teleportLastHitPosition = targetPosition.value;
                    }
                }
                
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
                
                if (states.HasState("Combo") && animation.playingTime >= currentAnimationFrame.cancellationTime && 
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

                if (states.HasState("Combo") && animation.playingTime >= currentAnimationFrame.cancellationTime && control.HasBufferedAction(control.button1) 
                    && currentComboAttack < comboAttacks)
                {
                    animation.Play(ComboAnimations[currentComboAttack], 1);

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
                currentComboAttack = 0;
                
                animation.Play("Attack1", 1);
                
                movement.movingDirection = Vector2.zero;
                
                control.ConsumeBuffer();

                states.EnterState("Attack");
                
                // states.EnterState("Combo");
                
                return;
            }

            if (dashBackCooldownCurrent <= 0)
            {
                if (control.HasBufferedActions(control.backward.name, control.button2.name) ||
                    control.HasBufferedActions(control.button2.name, control.backward.name))
                {
                    control.ConsumeBuffer();
                    states.EnterState("DashBack");
                    return;
                }
            }
            
            if (dashFrontCooldownCurrent <= 0)
            {
                if (control.HasBufferedAction(control.button2))
                {
                    control.ConsumeBuffer();
                    states.EnterState("DashFront");
                    return;
                }
            }
            
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