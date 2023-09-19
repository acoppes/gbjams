using Game.Utilities;
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
            if (entity.Get<DestroyableComponent>().destroy)
                return;
            
            // Debug.Log("COLLISION!!");
            // change state, stop travelling, etc...
            
            // entity.Get<ProjectileComponent>().
            
            // if static obstacle, then spawn stuck kunai!!

            var targetPosition = entityCollision.entity.Get<PositionComponent>().value;
            targetPosition.y = entity.Get<PositionComponent>().value.y;

            var newKunaiEntity = world.CreateEntity(stuckDefinition);
            newKunaiEntity.Get<PositionComponent>().value = targetPosition;
            newKunaiEntity.Get<LookingDirection>().value = entity.Get<LookingDirection>().value;

            entity.Get<DestroyableComponent>().destroy = true;
        }
    }
}