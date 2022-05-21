using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class SamuraiDogController : MonoBehaviour, IController
{
    // Read this kind of things from configuration
    public float specialAttackExtraSpeed;
    public float specialAttackRecoveryTime;
    
    public void OnUpdate(float dt, World world, int entity)
    {
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
        if (states.HasState("SpecialAttackRecovery"))
        {
            var state = states.GetState("SpecialAttackRecovery");
            
            if (state.time > specialAttackRecoveryTime)
            {
                states.ExitState("SpecialAttackRecovery");
                playerInput.disabled = false;
                unitState.attacking1 = false;
            }

            return;
        }
        

        if (states.HasState("SpecialAttack"))
        {
            var state = states.GetState("SpecialAttack");
            var specialAttack = abilities.Get("SpecialAttack");
            
            if (state.time > specialAttack.duration)
            {
                states.ExitState("SpecialAttack");
                
                // unitState.attacking1 = false;
                movementComponent.extraSpeed = 0;
                control.direction = Vector2.zero;
                
                states.EnterState("SpecialAttackRecovery");
            }

            return;
        }

        if (states.HasState("ChargingAttack"))
        {
            var state = states.GetState("ChargingAttack");
            var chargeSpecialAttack = abilities.Get("ChargeSpecialAttack");
            
            if (!control.secondaryAction)
            {
                movementComponent.disabled = false;
                states.ExitState("ChargingAttack");
                unitState.chargeAttack1 = false;

                if (state.time > chargeSpecialAttack.duration)
                {
                    // enter attacking state
                    states.EnterState("SpecialAttack");
                    // set speed and movement direction like a dash
                    
                    playerInput.disabled = true;
                    unitState.attacking1 = true;

                    control.direction = lookingDirection.value;

                    movementComponent.extraSpeed = specialAttackExtraSpeed;
                }
            }
            return;
        }

        
        // secondary action is pressed
        if (control.secondaryAction)
        {
            movementComponent.disabled = true;
            states.EnterState("ChargingAttack");
            unitState.chargeAttack1 = true;
            return;
        }
    }
}