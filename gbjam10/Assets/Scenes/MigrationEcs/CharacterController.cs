using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class CharacterController : ControllerBase
{
    private const string StateJumping = "Jumping";
    private const string StateFalling = "Falling";
    
    // Read this kind of things from configuration
    public float jumpMaxHeight;
    public float jumpSpeed = 1;
    public float fallSpeed = 1;

    public float slowExtraSpeed = -3;
    public float fastExtraSpeed = 3;

    public override void OnUpdate(float dt)
    {
        // if (world.HasComponent<PlayerInputComponent>(entity))
        //     return;
        
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        ref var jumpComponent = ref world.GetComponent<JumpComponent>(entity);
        ref var player = ref world.GetComponent<PlayerComponent>(entity);
        
        playerInput.disabled = true;
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        unitState.disableAutoUpdate = true;
        unitState.walking = false;
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        // ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        // var lookingDirection = world.GetComponent<LookingDirection>(entity);

        control.direction.x = 1;
        
        if (playerInput.keyMap != null)
        {
            control.direction.y = playerInput.keyMap.direction.y;
            control.mainAction = playerInput.keyMap.button1Pressed;
            control.secondaryAction = playerInput.keyMap.button2Pressed;

            movementComponent.extraSpeed = 0;
            
            if (playerInput.keyMap.direction.x > 0)
            {
                movementComponent.extraSpeed += fastExtraSpeed;
            } else if (playerInput.keyMap.direction.x < 0)
            {
                movementComponent.extraSpeed += slowExtraSpeed;
            }
        }

        // movementComponent.movingDirection = new Vector2(1, movementComponent.movingDirection.y);
        if (states.HasState(StateFalling))
        {
            // var state = states.GetState(StateFalling);
            
            control.direction.y = 0;
            jumpComponent.y -= dt * fallSpeed;
            
            if (jumpComponent.y <= 0)
            {
                unitState.dashing = false;
                states.ExitState(StateFalling);
                jumpComponent.y = 0;
            }
            
            return;
        }

        if (states.HasState(StateJumping))
        {
            // var state = states.GetState(StateJumping);
            
            control.direction.y = 0;

            jumpComponent.y += dt * jumpSpeed;

            if (jumpComponent.y >= jumpMaxHeight || !control.mainAction)
            {
                unitState.dashing = false;
                states.ExitState(StateJumping);
                states.EnterState(StateFalling);
            }
            
            return;
        }

        if (control.mainAction)
        {
            // start jumping 
            jumpComponent.y = 0;
            unitState.dashing = true;
            states.EnterState(StateJumping);
            return;
        }

        if (movementComponent.totalSpeed > 0)
        {
            unitState.walking = true;    
        }
    }
}