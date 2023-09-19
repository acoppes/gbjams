using Game.Components;
using Game.Controllers;
using Game.Utilities;
using GBJAM11.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace GBJAM11.Controllers
{
    public class KunaiController : ControllerBase, IEntityCollisionEvent
    {
        public void OnEntityCollision(World world, Entity entity, IEntityCollisionDelegate.EntityCollision entityCollision)
        {
            Debug.Log("COLLISION!!");
            // change state, stop travelling, etc...
            
            // entity.Get<ProjectileComponent>().
        }
    }
}