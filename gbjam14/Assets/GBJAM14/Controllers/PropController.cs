using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM14.Controllers
{
    public class PropController : ControllerBase, IUpdate, IHealthStateChanged
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var animations = ref entity.Get<AnimationsComponent>();
            if (animations.isCompleted && animations.IsPlaying("death"))
            {
                entity.Get<DestroyableComponent>().destroy = true;
            }
        }

        public void OnHealthStateChanged(World world, Entity entity)
        {
            if (entity.Get<HealthComponent>().wasKilledLastFrame)
            {
                ref var animations = ref entity.Get<AnimationsComponent>();
                animations.Play("death", 1);
            }
        }
    }
}