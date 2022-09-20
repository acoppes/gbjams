using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class CharacterController : ControllerBase
{
    // Read this kind of things from configuration
    public float jumpDuration;

    private readonly StateFilter canDash = new StateFilter(null, "CantDashAgain");

    public override void OnUpdate(float dt)
    {
        // if (world.HasComponent<PlayerInputComponent>(entity))
        //     return;
        
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        ref var player = ref world.GetComponent<PlayerComponent>(entity);
        
        playerInput.disabled = true;
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);

        control.direction.x = 1;
        
        if (playerInput.keyMap == null)
        {
            return;
        }
        
        if (playerInput.keyMap != null)
        {
            control.direction.y = playerInput.keyMap.direction.y;
            control.mainAction = playerInput.keyMap.button1Pressed;
            control.secondaryAction = playerInput.keyMap.button2Pressed;

            movementComponent.extraSpeed = 0;
            
            if (playerInput.keyMap.direction.x > 0)
            {
                movementComponent.extraSpeed = 3;
            } else if (playerInput.keyMap.direction.x < 0)
            {
                movementComponent.extraSpeed = -3;
            }
        }

        // movementComponent.movingDirection = new Vector2(1, movementComponent.movingDirection.y);

        if (states.HasState("Jumping"))
        {
            var jumpingState = states.GetState("Jumping");
            
            // cant do anything while jumping
            
            // if ground toched or jump completed// exit state?
            control.direction.y = 0;

            if (jumpingState.time > jumpDuration)
            {
                unitState.dashing = false;
                states.ExitState("Jumping");
            }
            
            return;
        }

        if (control.mainAction)
        {
            // start jumping 
            unitState.dashing = true;
            states.EnterState("Jumping");
        }
        
    }
}