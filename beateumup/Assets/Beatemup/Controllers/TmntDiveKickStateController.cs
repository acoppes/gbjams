using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;
using LookingDirection = Beatemup.Ecs.LookingDirection;

namespace Beatemup.Controllers
{
    public class TmntDiveKickStateController : ControllerBase, IStateChanged
    {
        public Vector3 diveKickSpeed;

        public void OnEnter()
        {
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);
            
            var states = world.GetComponent<StatesComponent>(entity);

            if (states.statesEntered.Contains("DiveKick"))
            {
                animation.Play("DivekickStartup", 1);
                gravityComponent.disabled = true;
            }
        }

        public void OnExit()
        {
            ref var gravityComponent = ref world.GetComponent<GravityComponent>(entity);;
            
            var states = world.GetComponent<StatesComponent>(entity);

            if (states.statesExited.Contains("DiveKick"))
            {
                gravityComponent.disabled = false;
            }
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);

            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            ref var verticalMovement = ref world.GetComponent<VerticalMovementComponent>(entity);

            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            var currentAnimationFrame = world.GetComponent<CurrentAnimationAttackComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            ref var position = ref world.GetComponent<PositionComponent>(entity);

            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);

            State state;
            
            if (states.TryGetState("DiveKick", out state))
            {
                if (animation.IsPlaying("DivekickStartup") && animation.state == AnimationComponent.State.Completed)
                {
                    animation.Play("DivekickLoop");
                    return;
                }

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
                            
                        animation.pauseTime = TmntConstants.HitAnimationPauseTime;
                    }
                }

                // gravityComponent.disabled = true;
                
                movement.movingDirection.y = diveKickSpeed.y;
                movement.movingDirection.x = lookingDirection.value.x * diveKickSpeed.x;

                verticalMovement.speed = diveKickSpeed.z;
                
                if (control.HasBufferedAction(control.button2))
                {
                    control.ConsumeBuffer();
                        
                    states.EnterState("Jump");
                    states.ExitState("DiveKick");
                        
                    return;
                }
                
                if (verticalMovement.isOverGround)
                {
                    states.ExitState("DiveKick");
                    // gravityComponent.disabled = false;
                }

                return;
            }
        }
    }
}