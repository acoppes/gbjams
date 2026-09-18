using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM14.Controllers
{
    public class ArrowController : ControllerBase, IInit
    {
        public void OnInit(World world, Entity entity)
        {
            var physics2DComponent = entity.Get<Physics2dComponent>();
            physics2DComponent.collisionsEventsDelegate.onCollisionEnter += OnEntityCollision;
        }

        private void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            if (entity.Get<ProjectileComponent>().impacted)
            {
                return;
            }
            
            if (entityCollision.entity)
            {
                if (entityCollision.entity.Has<HealthComponent>())
                {
                    entity.Get<ProjectileComponent>().impacted = true;
                    entity.Get<ProjectileComponent>().impactEntity = entityCollision.entity;
                    
                    entityCollision.entity.Get<HealthComponent>().damages.Add(new HealthChangeData()
                    {
                        position = entityCollision.collider2D.transform.position,
                        player = 0,
                        knockback = true,
                        source = entity,
                        value = 1,
                        vfxDefinition = null
                    });
                    
                    entity.Get<DestroyableComponent>().destroy = true;
                }
            }
            else
            {
                entity.Get<ProjectileComponent>().impacted = true;
                entity.Get<DestroyableComponent>().destroy = true;
            }
        }
    }
}