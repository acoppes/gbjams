using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitNekoninController : MonoBehaviour, IController
{
    // Read this kind of things from configuration
    public float dashExtraSpeed;

    public void OnUpdate(float dt, World world, int entity)
    {
        // if (world.HasComponent<PlayerInputComponent>(entity))
        //     return;
        
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        // playerInput.disabled = true;
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);

        if (states.HasState("Dashing"))
        {
            var state = states.GetState("Dashing");
            var dash = abilities.Get("Dash");
            
            if (state.time > dash.duration)
            {
                playerInput.disabled = false;
                states.ExitState("Dashing");
                // model unset dashing
                unitState.dashing = false;
                
                movementComponent.extraSpeed = 0;
            }

            return;
        }

        if (states.HasState("Attacking"))
        {
            var state = states.GetState("Attacking");
            var attack = abilities.Get("Attack");
            
            if (state.time > attack.duration)
            {
                states.ExitState("Attacking");
                unitState.attacking1 = false;
            }    
        }

        if (control.mainAction)
        {
            states.EnterState("Attacking");
            unitState.attacking1 = true;
            return;
        } 
        
        if (control.secondaryAction)
        {
            states.EnterState("Dashing");
            playerInput.disabled = true;
            unitState.dashing = true;

            control.direction = lookingDirection.value;

            movementComponent.extraSpeed = dashExtraSpeed;
                
            // states.EnterState("Dashing", dash.duration)
            // {
            //     onActivate =
            //     {
            //         
            //     },
            //     onDeactivate =
            //     {
            //         
            //     }
            // };
            
            return;
        }    
    }
}