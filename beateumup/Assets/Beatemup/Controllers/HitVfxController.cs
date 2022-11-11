using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class HitVfxController : ControllerBase, IInit
    {
        public void OnInit()
        {
            var vfx = world.GetComponent<VfxComponent>(entity);

            if (vfx.delay > 0)
            {
                ref var states = ref world.GetComponent<StatesComponent>(entity);
                states.EnterState("Delay");    
            }
            else
            {
                ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
                animationComponent.Play("Idle", 1);
            }
        }
        
        public override void OnUpdate(float dt)
        {
            var vfx = world.GetComponent<VfxComponent>(entity);
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            ref var states = ref world.GetComponent<StatesComponent>(entity);

            if (states.TryGetState("Delay", out var state))
            {
                if (state.time > vfx.delay)
                {
                    animationComponent.Play("Idle", 1);   
                    states.ExitState(state.name);
                }
                return;
            }

            if (animationComponent.state == AnimationComponent.State.Completed)
            {
                world.DestroyEntity(entity);
            }
        }

    }
}