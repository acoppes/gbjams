using System.Linq;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

public class UnitNekoninController : MonoBehaviour, IController
{
    // Read this kind of things from configuration
    public float dashExtraSpeed;

    private readonly StateFilter canDash = new StateFilter(null, "CantDashAgain");

    public void OnUpdate(float dt, World world, int entity)
    {
        // if (world.HasComponent<PlayerInputComponent>(entity))
        //     return;
        
        ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        ref var player = ref world.GetComponent<PlayerComponent>(entity);
        // playerInput.disabled = true;
        
        ref var movementComponent = ref world.GetComponent<UnitMovementComponent>(entity);
        
        ref var unitState = ref world.GetComponent<UnitStateComponent>(entity);
        
        ref var states = ref world.GetComponent<StatesComponent>(entity);
        ref var control = ref world.GetComponent<UnitControlComponent>(entity);

        ref var abilities = ref world.GetComponent<AbilitiesComponent>(entity);
        
        var lookingDirection = world.GetComponent<LookingDirection>(entity);
        
        var attack = abilities.GetAbility("MainAbility");
        var dash = abilities.GetAbility("SecondaryAbility");
        
        if (dash.isRunning)
        {
            // var state = states.GetState("Dashing");

            if (dash.isComplete)
            {
                playerInput.disabled = false;
                states.ExitState("Dashing");
                // model unset dashing
                unitState.dashing = false;
                
                movementComponent.extraSpeed = 0;
                
                dash.Stop();
            }

            return;
        }

        // if (states.HasState("Attacking"))
        if (attack.isRunning)
        {
            // var state = states.GetState("Attacking");
            
            // if (state.time > attack.duration)
            if (attack.isComplete) 
            {
                // FIRE KUNAI PROJECTILE! (depends on current weapon/etc)

                var projectileEntity = ProjectileUtils.Fire(world, new ProjectileParameters
                {
                    definition = attack.projectileDefinition,
                    position = attack.position, // attachpoints.Get("").position
                    direction = attack.direction, // lookingdirection.value
                    player = player.player
                });
                
                // override something for that projectile?
                
                // states.ExitState("Attacking");
                unitState.attacking1 = false;

                attack.Stop();
            }    
        }

        if (control.mainAction && attack.isReady)
        {
            // states.EnterState("Attacking");
            unitState.attacking1 = true;

            attack.StartRunning();
            
            return;
        }

        if (!control.secondaryAction)
        {
            states.ExitState("CantDashAgain");
        }
        
        if (dash.isReady && canDash.Match(states) && control.secondaryAction)
        {
            // states.EnterState("Dashing");
            
            dash.StartRunning();
            
            playerInput.disabled = true;
            unitState.dashing = true;

            control.direction = lookingDirection.value;

            movementComponent.extraSpeed = dashExtraSpeed;
            
            states.EnterState("CantDashAgain");
                
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