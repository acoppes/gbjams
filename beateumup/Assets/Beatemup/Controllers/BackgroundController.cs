using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class BackgroundController : ControllerBase
    {
        public override void OnUpdate(float dt)
        {
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
            movement.movingDirection = Vector2.left;
        }
    }
}