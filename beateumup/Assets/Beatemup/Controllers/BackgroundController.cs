using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class BackgroundController : ControllerBase, IUpdate
    {
        public void OnUpdate(float dt)
        {
            ref var movement = ref world.GetComponent<HorizontalMovementComponent>(entity);
            movement.speed = movement.baseSpeed;
            movement.movingDirection = Vector3.left;
        }
    }
}