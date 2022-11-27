using Beatemup.Development;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;
using TargetingUtils = Beatemup.Ecs.TargetingUtils;

namespace Beatemup.Controllers
{
    public class FootSoldierAIController : ControllerBase, IInit
    {
        public float timeToChangeDirection = 0.25f;
        
        public float timeToTryAttack = 0.25f;
        
        public HitboxAsset attackDetection;

        private DebugHitBoxSystem debugHitBoxes;

        private DebugHitBox debugHitBox;
        
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
        
        public override void OnUpdate(float dt)
        {
            // var mainPlayer = world.GetEntityByName("Character_Player_0");

            ref var states = ref world.GetComponent<StatesComponent>(entity);
            ref var control = ref world.GetComponent<ControlComponent>(entity);
            
            var position = world.GetComponent<PositionComponent>(entity);
            var lookingDirection = world.GetComponent<LookingDirection>(entity);

            var player = world.GetComponent<PlayerComponent>(entity);

            State state;

            control.button1.isPressed = false;
            
            var hitBox = attackDetection.GetHitBox(position, lookingDirection);
            
            if (debugHitBox != null)
            {
                debugHitBox.UpdateHitBox(hitBox);
            }
            
            if (states.HasState("HitStun"))
            {
                control.direction = Vector2.zero;
                return;
            }

            if (states.TryGetState("TryingAttack", out state))
            {
                if (state.time > timeToTryAttack)
                {
                    states.ExitState("TryingAttack");
                }
            }
            
            if (!states.HasState("TryingAttack"))
            {
                

                var targets = TargetingUtils.GetTargets(world, new TargetingUtils.TargetingParameters
                {
                    player = player.player,
                    area = hitBox
                });

             
                
                if (targets.Count > 0)
                {
                    states.EnterState("TryingAttack");
                    control.button1.isPressed = true;
                    
                    control.InsertInBuffer(control.button1.name);
                    return;
                }

                // control.direction = Vector2.zero;
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