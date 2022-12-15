using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;

namespace Beatemup.Controllers
{
    public class HitVfxController : ControllerBase, IInit, IUpdate
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
        
        public void OnUpdate(float dt)
        {
            ref var destroyable = ref world.GetComponent<DestroyableComponent>(entity);
            if (destroyable.destroy)
            {
                return;
            }
            
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
                destroyable.destroy = true;
            }
        }

    }
}