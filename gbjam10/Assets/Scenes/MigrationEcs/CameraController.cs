using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class CameraController : ControllerBase
{
    public override void OnUpdate(float dt)
    {
        // ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        // movementComponent.movingDirection = Vector2.right;

        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        playerInput.disabled = true;
        
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        control.direction.x = 1;
    }
}