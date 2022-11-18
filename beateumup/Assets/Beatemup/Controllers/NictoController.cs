using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Controllers
{
    public class NictoController : ControllerBase, IInit
    {
        private static readonly string[] ComboAnimations = 
        {
            "Attack2", "Attack3", "AttackFinisher"
        };
        
        public float attackCancellationTime = 0.1f;
        
        private int comboAttacks => ComboAnimations.Length;
        private int currentComboAttack;
        
        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);

            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            // ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);
            // ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);

            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationFrameComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var position = ref world.GetComponent<PositionComponent>(entity);
            // ref var jump = ref world.GetComponent<JumpComponent>(entity);

            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            
            State state;

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

                        animation.pauseTime = TmntConstants.HitAnimationPauseTime;
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
            
            if (control.HasBufferedAction(control.button1))
            {
                currentComboAttack = 0;
                
                animation.Play("Attack1", 1);
                
                movement.movingDirection = Vector2.zero;
                
                control.ConsumeBuffer();

                states.EnterState("Attack");
                
                // states.EnterState("Combo");
                
                return;
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