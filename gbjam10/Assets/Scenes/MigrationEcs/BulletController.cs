using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class BulletController : ControllerBase
{
    public override void OnUpdate(float dt)
    {
        ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);
        movement.movingDirection = Vector2.right;
    }
}