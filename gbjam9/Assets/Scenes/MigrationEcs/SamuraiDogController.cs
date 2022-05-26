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
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
        var attack = abilities.GetAbility("MainAbility");
        var chargeSpecialAttack = abilities.GetAbility("SecondaryAbility");
        
        if (states.HasState("SpecialAttackRecovery"))
        {
            var state = states.GetState("SpecialAttackRecovery");
            
            if (state.time > specialAttackRecoveryTime)
            {
                states.ExitState("SpecialAttackRecovery");
                // playerInput.disabled = false;
                control.locked = false;
                unitState.attacking1 = false;
                
                // control.directionLocked = false;
            }

            return;
        }
        

        if (states.HasState("SpecialAttack"))
        {
            var state = states.GetState("SpecialAttack");
            var specialAttack = abilities.GetAbility("SpecialAttack");
            
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
          
            
            if (!control.secondaryAction)
            {
                movementComponent.disabled = false;
                states.ExitState("ChargingAttack");
                unitState.chargeAttack1 = false;

                // chargeSpecialAttack.Stop(); // didnt execute, dont reset cooldown
                chargeSpecialAttack.Cancel();
                
                if (state.time > chargeSpecialAttack.duration)
                {
                    chargeSpecialAttack.Stop();
                    
                    // enter attacking state
                    states.EnterState("SpecialAttack");
                    // set speed and movement direction like a dash
                    
                    unitState.attacking1 = true;

                    control.locked = true;
                    control.direction = lookingDirection.value;

                    movementComponent.extraSpeed = specialAttackExtraSpeed;
                    
                    // control.directionLocked = true;
                }
            }
            return;
        }
        
        // if (states.HasState("Attacking"))
        // {
        //     var state = states.GetState("Attacking");
        //     
        //     if (state.time > attack.duration)
        //     {
        //         states.ExitState("Attacking");
        //         unitState.attacking1 = false;
        //     }
        //
        //     return;
        // }
        
        // if (states.HasState("Attacking"))
        if (attack.isRunning)
        {
            // var state = states.GetState("Attacking");
            
            // if (state.time > attack.duration)
            if (attack.isComplete) 
            {
                // states.ExitState("Attacking");
                unitState.attacking1 = false;

                attack.Stop();
            }    
        }

        if (!control.mainAction)
        {
            states.ExitState("CantAttackAgain");
        }

        if (!states.HasState("CantAttackAgain") && control.mainAction && attack.isReady)
        {
            // states.EnterState("Attacking");
            unitState.attacking1 = true;

            attack.StartRunning();
            
            states.EnterState("CantAttackAgain");
            
            return;
        }

        // secondary action is pressed
        if (control.secondaryAction && chargeSpecialAttack.isReady)
        {
            movementComponent.disabled = true;
            states.EnterState("ChargingAttack");
            unitState.chargeAttack1 = true;
            chargeSpecialAttack.StartRunning();
            return;
        }
        
        // if (control.mainAction)
        // {
        //     states.EnterState("Attacking");
        //     unitState.attacking1 = true;
        //     return;
        // } 
    }
}