using Game.Components;
using Game.Utilities;
using GBJAM11.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

namespace GBJAM11.Controllers
{
    public class KunaiTunnelController : ControllerBase, IEntityCollisionEvent
    {
        public void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            if (entityCollision.entity.Exists())
            {
                if (entityCollision.entity.Has<KunaiComponent>())
                {
                    ref var kunai = ref entityCollision.entity.Get<KunaiComponent>();
                    if (kunai.lastTeleportLocation != entity)
                    {
                        var exitEntity = entity.Get<KunaiTunnelComponent>().exitEntity;
                        
                        entityCollision.entity.Get<PositionComponent>().value = exitEntity.Get<PositionComponent>().value;

                        entityCollision.entity.Get<Physics2dComponent>().body.position = exitEntity.Get<PositionComponent>().value;

                        kunai.lastTeleportLocation = exitEntity;
                    }

                }
            }
        }
    }
}