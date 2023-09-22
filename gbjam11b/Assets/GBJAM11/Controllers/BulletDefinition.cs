using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;

namespace GBJAM11.Controllers
{
    public class BulletDefinition : ControllerBase, IEntityCollisionEvent
    {
        public void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            // Debug.Log("DETECTION!");
            
            if (entity.Get<DestroyableComponent>().destroy)
                return;

            var targetEntity = entityCollision.entity;
            
            if (targetEntity.Exists() && targetEntity.Has<PlayerComponent>())
            {
                if (entity.Get<PlayerComponent>().player == targetEntity.Get<PlayerComponent>().player)
                    return;
            }
            
            // // if static obstacle, then spawn stuck kunai!!
            //
            // var newKunaiEntity = world.CreateEntity(stuckDefinition);
            //
            // if (targetEntity.Exists() && targetEntity.Has<SwapableComponent>())
            // {
            //     var targetPosition = entityCollision.entity.Get<PositionComponent>().value;
            //     targetPosition.y = entity.Get<PositionComponent>().value.y;
            //     newKunaiEntity.Get<KunaiComponent>().stuckEntity = targetEntity;
            //     newKunaiEntity.Get<PositionComponent>().value = targetPosition;
            // }
            // else
            // {
            //     newKunaiEntity.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
            //
            //     if (!entityCollision.isTrigger)
            //     {
            //         var normal = entityCollision.collision2D.contacts[0].normal;
            //         
            //         newKunaiEntity.Get<KunaiComponent>().ceilingCollision = Mathf.Abs(normal.x) < 0.2f && normal.y < -0.8f;
            //         newKunaiEntity.Get<KunaiComponent>().floorCollision = Mathf.Abs(normal.x) < 0.2f && normal.y > 0.8f;
            //         newKunaiEntity.Get<KunaiComponent>().wallCollision = Mathf.Abs(normal.y) < 0.2f && Mathf.Abs(normal.x) > 0.8f;
            //         newKunaiEntity.Get<KunaiComponent>().normal = normal;
            //         
            //         newKunaiEntity.Get<PositionComponent>().value = entityCollision.collision2D.contacts[0].point;
            //     }
            // }
            //
            // newKunaiEntity.Get<LookingDirection>().value = entity.Get<LookingDirection>().value;
            
            entity.Get<DestroyableComponent>().destroy = true;
        }
    }
}