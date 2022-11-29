using Beatemup.Development;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;
using TargetingUtils = Beatemup.Ecs.TargetingUtils;

namespace Beatemup.Controllers
{
    public class FootSoldierAIController : ControllerBase, IInit, IEntityDestroyed
    {
        public float targetReachedDelayToAttack = 0.5f;
        
        public float timeToTryAttack = 0.25f;
        
        public HitboxAsset attackDetection;

        private DebugHitBoxSystem debugHitBoxes;

        private DebugHitBox debugHitBox;

        private TargetingUtils.Target mainTarget;
        
        public void OnInit()
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            states.EnterState("MovingDown");

            debugHitBoxes = FindObjectOfType<DebugHitBoxSystem>();

            if (debugHitBoxes != null)
            {
                debugHitBox = debugHitBoxes.CreateDebugHitBox(2);
            }
        }
        
        public void OnEntityDestroyed(Entity e)
        {
            if (mainTarget != null && mainTarget.entity == e)
            {
                mainTarget = null;
            }
        }
        
        public override void OnUpdate(float dt)
        {
            // var mainPlayer = world.GetEntityByName("Character_Player_0");

            ref var states = ref world.GetComponent<StatesComponent>(entity);
            ref var control = ref world.GetComponent<ControlComponent>(entity);
            
            var position = world.GetComponent<PositionComponent>(entity);
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            var player = world.GetComponent<PlayerComponent>(entity);

            State state;

            control.button1.isPressed = false;
            
            var hitBox = attackDetection.GetHitBox(position, lookingDirection);
            
            if (debugHitBox != null)
            {
                debugHitBox.UpdateHitBox(hitBox);
            }
            
            if (mainTarget == null)
            {
                var targets = TargetingUtils.GetTargets(new TargetingUtils.TargetingParameters
                {
                    sourcePlayer = player.player,
                    area = new HitBox
                    {
                        position = position.value,
                        depth = 100,
                        offset = Vector2.zero,
                        size = new Vector2(100, 100)
                    }
                });

                if (targets.Count > 0)
                {
                    mainTarget = targets[0];
                }
            }
            
            if (states.HasState("HitStun") || states.HasState("Down"))
            {
                control.direction = Vector2.zero;
                return;
            }
            
            if (states.TryGetState("TargetReached", out state))
            {
                control.direction = Vector2.zero;

                if (state.time > targetReachedDelayToAttack)
                {
                    states.ExitState(state.name);
                }
                return;
            }

            if (states.TryGetState("FollowingTarget", out state))
            {
                var side = Mathf.Sign(position.value.x - mainTarget.position.x);
                
                var desiredPosition = mainTarget.position + new Vector3(side * attackDetection.offset.x, 0, 0);
                control.direction = (desiredPosition - position.value).normalized;
                
                // lookingDirection.value.x = Mathf.Sign(mainTarget.position.x - position.value.x);
                
                if ((desiredPosition - position.value).sqrMagnitude < 0.1f)
                {
                    control.direction = Vector2.zero;
                    lookingDirection.value.x = -side;
                    // control.direction = (mainTarget.position - position.value).normalized;
                    
                    states.EnterState("TargetReached");
                    states.ExitState(state.name);
                }

                return;
            }
            
            var baseAttackTargets = TargetingUtils.GetTargets(new TargetingUtils.TargetingParameters
            {
                sourcePlayer = player.player,
                area = hitBox
            });

            if (states.TryGetState("TryingAttack", out state))
            {
                if (state.time > timeToTryAttack)
                {
                    states.ExitState("TryingAttack");
                }
            }
            
            if (!states.HasState("TryingAttack"))
            {
                if (baseAttackTargets.Count > 0)
                {
                    states.EnterState("TryingAttack");
                    control.button1.isPressed = true;
                    
                    control.InsertInBuffer(control.button1.name);
                    return;
                }
            }
            
            control.direction = Vector2.zero;

            if (mainTarget != null && baseAttackTargets.Count == 0)
            {
                states.EnterState("FollowingTarget");
                return;
            }
            
            // if (states.TryGetState("MovingDown", out state))
            // {
            //     control.direction = Vector2.down;
            //
            //     if (state.time > timeToChangeDirection)
            //     {
            //         states.ExitState("MovingDown");
            //         states.EnterState("MovingUp");
            //     }
            //     
            //     return;
            // }
            //
            // if (states.TryGetState("MovingUp", out state))
            // {
            //     control.direction = Vector2.up;
            //
            //     if (state.time > timeToChangeDirection)
            //     {
            //         states.ExitState("MovingUp");
            //         states.EnterState("MovingDown");
            //     }
            //     
            //     return;
            // }
        }

    }
}