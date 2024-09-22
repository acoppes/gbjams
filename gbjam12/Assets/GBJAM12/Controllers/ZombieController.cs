using Game;
using Game.Components;
using GBJAM12.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM12.Controllers
{
    public class ZombieController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var animations = ref entity.Get<AnimationsComponent>();
            
            var danceMoves = world.GetSingleton<DanceMovesComponent>();

            if (danceMoves.d1)
            {
                if (!animations.IsPlaying("D1"))
                {
                    animations.Play("D1");
                }
            }
            else if (danceMoves.d2)
            {
                if (!animations.IsPlaying("D2"))
                {
                    animations.Play("D2");
                }
            }
            else if (danceMoves.d3)
            {
                if (!animations.IsPlaying("D3"))
                {
                    animations.Play("D3");
                }
            }
            else if (!animations.IsPlaying("Idle"))
            {
                animations.Play("Idle");
            }
        }
    }
}