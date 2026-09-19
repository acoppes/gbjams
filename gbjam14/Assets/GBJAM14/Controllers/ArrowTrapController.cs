using Game.Components;
using GBJAM14.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using MyBox;
using UnityEngine;

namespace GBJAM14.Controllers
{
    public class ArrowTrapController : ControllerBase, IUpdate
    {
        [EntityDefinition]
        public Object fireArrowSoundEffect;
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var arrowTrapComponent = ref entity.Get<ArrowTrapComponent>();
            
            arrowTrapComponent.reloadCooldown.Increase(dt);
            if (!arrowTrapComponent.reloadCooldown.IsReady)
            {
                return;
            }
            
            arrowTrapComponent.fireCooldown.Increase(dt);
            if (arrowTrapComponent.fireCooldown.IsReady)
            {
                arrowTrapComponent.fireCooldown.Reset();
                arrowTrapComponent.reloadCooldown.Reset();
                
                var initialOffset = arrowTrapComponent.fireOffset;
                // var spawnOffset = arrowTrapComponent.spawnOffset;
                var direction = entity.Get<LookingDirection>().value;
                var position = entity.Get<PositionComponent>().value + arrowTrapComponent.spawnOffset;
                
                world.CreateEntity(arrowTrapComponent.projectileDefinition, null, e =>
                {
                    e.Get<PositionComponent>().value = position + direction * initialOffset;
                    e.Get<LookingDirection>().value = direction;

                    // e.Get<ProjectileComponent>().initialOffset = initialOffset;
                    e.Add(new ProjectileFireComponent()
                    {
                        direction = e.Get<LookingDirection>().value
                    });
                });

                if (fireArrowSoundEffect)
                {
                    world.CreateEntity(fireArrowSoundEffect, null, (e) =>
                    {
                        e.Get<PositionComponent>().value = position;
                    });
                }
            }
        }
    }
}