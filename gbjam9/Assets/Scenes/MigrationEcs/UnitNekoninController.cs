using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitNekoninController : MonoBehaviour, IController
{
    public float dashExtraSpeed;
    
    public void OnInit(World world, int entity)
    {
        // get state component for entity
        // register states
        
        // for example, dash logic
        // on update, if (input.secondaryAction pressed) 
        // then enter dashing for dash duration
        
        // on enter dashing, disable control, set visual state
        // on exit dashing, enable control, unset visual state
    }

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
            if (!states.IsActive("Dashing"))
            {
                playerInput.disabled = false;
                states.ExitState("Dashing");
                // model unset dashing
                unitState.dashing = false;
                
                movementComponent.extraSpeed = 0;
            }    
        }
        else
        {
            // secondary action is pressed
            if (control.secondaryAction)
            {
                var dash = abilities.Get("Dash");
                
                states.EnterState("Dashing", dash.duration);
                playerInput.disabled = true;
                unitState.dashing = true;

                control.direction = lookingDirection.value;

                movementComponent.extraSpeed = dashExtraSpeed;
            }            
        }

        // ref var m = ref world.GetComponent<UnitMovementComponent>(entity);
        // m.movingDirection = Vector2.one;
    }
}
