using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Beatemup.Controllers
{
    public class FootSoldierMotorcycleController : ControllerBase, IInit
    {
        public void OnInit()
        {
            ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            lookingDirection.locked = true;
            
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            animationComponent.Play("MotorcycleRoll");
        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
            ref var animation = ref world.GetComponent<AnimationComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            // ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
            
            movement.movingDirection = control.direction;

            if (states.TryGetState("Wheelie", out var state))
            {
                if (animation.IsPlaying("MotorcycleWheelieStart") && animation.state == AnimationComponent.State.Completed)
                {
                    animation.Play("MotorcycleWheelieLoop");
                    return;
                }
                
                if (animation.IsPlaying("MotorcycleWheelieLoop") && !control.button1.isPressed)
                {
                    animation.Play("MotorcycleWheelieEnd", 1);
                    return;
                }
                
                if (animation.IsPlaying("MotorcycleWheelieEnd") && animation.state == AnimationComponent.State.Completed)
                {
                    states.ExitState("Wheelie");
                    return;
                }

                return;
            }

            if (control.HasBufferedAction(control.button1))
            {
                animation.Play("MotorcycleWheelieStart", 1);
                states.EnterState("Wheelie");
                return;
            }
            
            if (!animation.IsPlaying("MotorcycleRoll"))
            {
                animation.Play("MotorcycleRoll");
            }
        }

    }
}