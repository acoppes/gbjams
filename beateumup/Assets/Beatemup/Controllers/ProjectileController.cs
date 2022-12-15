using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class ProjectileController : ControllerBase, IInit, IUpdate, IStateChanged
    {
        public void OnInit()
        {
            // var gravity = world.GetComponent<GravityComponent>(entity);
            // var gravity = GetEntityComponent<GravityComponent>();
            
            ref var states = ref GetComponent<StatesComponent>();
            states.EnterState("Travel");
        }
        
        public void OnEnterState()
        {
            ref var states = ref GetComponent<StatesComponent>();
            ref var movement = ref GetComponent<HorizontalMovementComponent>();
            
            if (states.statesEntered.Contains("Travel"))
            {
                ref var gravity = ref GetComponent<GravityComponent>();
                gravity.disabled = true;

                movement.speed = movement.baseSpeed;
            }
        }

        public void OnExitState()
        {
            
        }
        
        public void OnUpdate(float dt)
        {
            ref var states = ref GetComponent<StatesComponent>();
            ref var movement = ref GetComponent<HorizontalMovementComponent>();
            ref var lookingDirection = ref GetComponent<LookingDirection>();
            
            State state;
            
            if (states.TryGetState("Travel", out state))
            {
                movement.movingDirection = lookingDirection.value;
            }
        }


    }
}