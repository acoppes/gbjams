using Game.Utilities;
using GBJAM11.Components;
using GBJAM11.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class KunaiController : ControllerBase, IEntityCollisionEvent
    {
        public Object stuckDefinition;
        
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
            
            // TODO: ignore nekosama collision
            
            // if static obstacle, then spawn stuck kunai!!
            
            var newKunaiEntity = world.CreateEntity(stuckDefinition);
            
            if (targetEntity.Exists() && targetEntity.Has<SwapableComponent>())
            {
                var targetPosition = entityCollision.entity.Get<PositionComponent>().value;
                targetPosition.y = entity.Get<PositionComponent>().value.y;
                newKunaiEntity.Get<KunaiComponent>().stuckEntity = targetEntity;
                newKunaiEntity.Get<PositionComponent>().value = targetPosition;
            }
            else
            {
                newKunaiEntity.Get<PositionComponent>().value = entity.Get<PositionComponent>().value;
            }
            
            newKunaiEntity.Get<LookingDirection>().value = entity.Get<LookingDirection>().value;
            
            entity.Get<DestroyableComponent>().destroy = true;
        }
    }
}