using Game.Components;
using Game.Utilities;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;

namespace GBJAM11.Controllers
{
    public class BulletController : ControllerBase, IEntityCollisionEvent
    {
        public void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            if (entity.Get<DestroyableComponent>().destroy)
                return;

            var targetEntity = entityCollision.entity;
            
            if (targetEntity.Exists() && targetEntity.Has<PlayerComponent>())
            {
                if (entity.Get<PlayerComponent>().player == targetEntity.Get<PlayerComponent>().player)
                    return;
            }

            if (targetEntity.Exists() && targetEntity.Has<HealthComponent>())
            {
                ref var health = ref targetEntity.Get<HealthComponent>();
                var damage = entity.Get<ProjectileDamageComponent>();
                
                health.damages.Add(new DamageData()
                {
                    value = damage.damage
                });
            }
            
            entity.Get<DestroyableComponent>().destroy = true;
        }
    }
}