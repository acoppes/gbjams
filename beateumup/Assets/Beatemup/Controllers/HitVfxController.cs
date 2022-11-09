using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class HitVfxController : ControllerBase, IInit
    {
        public void OnInit()
        {
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);
            animationComponent.Play("Idle", 1);
        }
        
        public override void OnUpdate(float dt)
        {
            ref var animationComponent = ref world.GetComponent<AnimationComponent>(entity);

            if (animationComponent.state == AnimationComponent.State.Completed)
            {
                world.DestroyEntity(entity);
            }
        }

    }
}