using GBJAM10.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

public class CharacterController : ControllerBase
{
    private const string StateJumping = "Jumping";
    private const string StateFalling = "Falling";
    private const string StatePickingTrap = "PickingTrap";
    
    // Read this kind of things from configuration
    public float jumpMaxHeight;
    public float jumpSpeed = 1;
    public float fallSpeed = 1;

    public float slowExtraSpeed = -3;
    public float fastExtraSpeed = 3;

    public GameObject bulletDefinition;

    public override void OnUpdate(float dt)
    {
        // if (world.HasComponent<PlayerInputComponent>(entity))
        //     return;
        
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        ref var jumpComponent = ref world.GetComponent<JumpComponent>(entity);
        ref var playerComponent = ref world.GetComponent<PlayerComponent>(entity);
        
        playerInput.disabled = true;
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        unitState.disableAutoUpdate = true;
        unitState.walking = false;
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        var pickTrapAbility = abilities.GetAbility("PickTrap");
        
        var autoAttackAbility = abilities.GetAbility("AutoAttack");

        var position = world.GetComponent<PositionComponent>(entity);
        var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
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

        if (states.HasState(StatePickingTrap))
        {
            movementComponent.extraSpeed += slowExtraSpeed;
            
            // control.direction.x = 0;
            
            control.direction.y = 0;

            if (pickTrapAbility.isComplete)
            {
                unitState.attacking1 = false;
                
                pickTrapAbility.isRunning = false;
                states.ExitState(StatePickingTrap);
            }
            
            return;
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

            if (jumpComponent.y >= jumpMaxHeight || !control.secondaryAction)
            {
                unitState.dashing = false;
                states.ExitState(StateJumping);
                states.EnterState(StateFalling);
            }
            
            return;
        }
        
        if (control.mainAction && pickTrapAbility.isCooldownReady)
        {
            pickTrapAbility.isRunning = true;
            
            unitState.walking = false;
            
            // control.direction.x = 0;
            
            unitState.attacking1 = true;
            states.EnterState(StatePickingTrap);
            return;
        }

        if (control.secondaryAction)
        {
            // start jumping 
            jumpComponent.y = 0;
            unitState.dashing = true;
            states.EnterState(StateJumping);
            return;
        }
        
        if (autoAttackAbility.isCooldownReady)
        {
            var bulletEntity = world.CreateEntity(bulletDefinition.GetInterface<IEntityDefinition>(), null);
            ref var bulletPosition = ref world.GetComponent<PositionComponent>(bulletEntity);
            
            ref var bulletPlayerComponent = ref world.GetComponent<PlayerComponent>(bulletEntity);
            bulletPlayerComponent.player = playerComponent.player;
            
            bulletPosition.value = position.value + lookingDirection.value.normalized * 0.5f;
            autoAttackAbility.cooldownCurrent = 0;
        }

        if (movementComponent.totalSpeed > 0)
        {
            unitState.walking = true;    
        }
    }
}