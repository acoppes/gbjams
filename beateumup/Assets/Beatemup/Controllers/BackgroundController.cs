using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class BackgroundController : ControllerBase
    {
        public override void OnUpdate(float dt)
        {
            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            movement.movingDirection = Vector2.left;
        }
    }
}