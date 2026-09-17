using Game.Components;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM14.Controllers
{
    public class ArrowTrapController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var arrowTrapComponent = ref entity.Get<ArrowTrapComponent>();
            arrowTrapComponent.fireCooldown.Increase(dt);

            if (arrowTrapComponent.fireCooldown.IsReady)
            {
                arrowTrapComponent.fireCooldown.Reset();
                var initialOffset = arrowTrapComponent.fireOffset;
                var direction = entity.Get<LookingDirection>().value;
                
                world.CreateEntity(arrowTrapComponent.projectileDefinition, null, e =>
                {
                    e.Get<PositionComponent>().value = 
                        entity.Get<PositionComponent>().value + direction * initialOffset;
                    e.Get<LookingDirection>().value = direction;

                    // e.Get<ProjectileComponent>().initialOffset = initialOffset;
                    e.Add(new ProjectileFireComponent()
                    {
                        direction = e.Get<LookingDirection>().value
                    });
                });
            }
        }
    }
}