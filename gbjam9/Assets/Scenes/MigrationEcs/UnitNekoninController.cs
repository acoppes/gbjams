using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitNekoninController : MonoBehaviour, IController
{
    public void OnUpdate(float dt, World world, int entity)
    {
        if (world.HasComponent<PlayerInputComponent>(entity))
            return;
        
        // ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        // playerInput.disabled = true;
        
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);
        control.direction = Vector2.right;

        // ref var m = ref world.GetComponent<UnitMovementComponent>(entity);
        // m.movingDirection = Vector2.one;
    }
}
