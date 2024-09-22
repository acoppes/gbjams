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
            
            // if (danceMoves.n1 && danceMoves.n2 && danceMoves.n3)
            // {
            //     if (!animations.IsPlaying("D1-D2-D3"))
            //     {
            //         animations.Play("D1-D2-D3");
            //     }
            // }
            // else if (danceMoves.n1 && danceMoves.n2)
            // {
            //     if (!animations.IsPlaying("D1-D2"))
            //     {
            //         animations.Play("D1-D2");
            //     }
            // }
            // else if (danceMoves.n2 && danceMoves.n3)
            // {
            //     if (!animations.IsPlaying("D2-D3"))
            //     {
            //         animations.Play("D2-D3");
            //     }
            // }
            // else if (danceMoves.n1 && danceMoves.n3)
            // {
            //     if (!animations.IsPlaying("D1-D3"))
            //     {
            //         animations.Play("D1-D3");
            //     }
            // }

            // var note = danceMoves.GetLongerNote();
            //
            // if (note == 0)
            // {
            //     if (!animations.IsPlaying("D1"))
            //     {
            //         animations.Play("D1");
            //     }
            // }
            // else if (note == 1)
            // {
            //     if (!animations.IsPlaying("D2"))
            //     {
            //         animations.Play("D2");
            //     }
            // }
            // else if (note == 2)
            // {
            //     if (!animations.IsPlaying("D3"))
            //     {
            //         animations.Play("D3");
            //     }
            // }
            // else if (!animations.IsPlaying("Idle"))
            // {
            //     animations.Play("Idle");
            // }
            
            if (danceMoves.n1)
            {
                if (!animations.IsPlaying("D1"))
                {
                    animations.Play("D1");
                }
            }
            else if (danceMoves.n2)
            {
                if (!animations.IsPlaying("D2"))
                {
                    animations.Play("D2");
                }
            }
            else if (danceMoves.n3)
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