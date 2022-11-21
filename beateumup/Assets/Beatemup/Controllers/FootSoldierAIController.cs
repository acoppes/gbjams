using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using UnityEngine;

namespace Beatemup.Controllers
{
    public class FootSoldierAIController : ControllerBase, IInit
    {
        public float timeToChangeDirection = 0.25f;
        
        public void OnInit()
        {
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            states.EnterState("MovingDown");
        }
        
        public override void OnUpdate(float dt)
        {
            // var mainPlayer = world.GetEntityByName("Character_Player_0");
            
            ref var states = ref world.GetComponent<StatesComponent>(entity);
            ref var control = ref world.GetComponent<ControlComponent>(entity);
            
            State state;
            
            if (states.TryGetState("MovingDown", out state))
            {
                control.direction = Vector2.down;

                if (state.time > timeToChangeDirection)
                {
                    states.ExitState("MovingDown");
                    states.EnterState("MovingUp");
                }
                
                return;
            }
            
            if (states.TryGetState("MovingUp", out state))
            {
                control.direction = Vector2.up;

                if (state.time > timeToChangeDirection)
                {
                    states.ExitState("MovingUp");
                    states.EnterState("MovingDown");
                }
                
                return;
            }
        }
    }
}