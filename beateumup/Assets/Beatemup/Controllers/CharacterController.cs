using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
        private const string DashingState = "Dashing";

        public float dashDuration = 1.0f;
        public float dashExtraSpeed = 10.0f;

        public void OnInit()
        {
            
        }
        
        public void OnEntityDestroyed(Entity e)
        {

        }

        public override void OnUpdate(float dt)
        {
            var control = world.GetComponent<ControlComponent>(entity);
            ref var movement = ref world.GetComponent<UnitMovementComponent>(entity);

            ref var states = ref world.GetComponent<StatesComponent>(entity);
            
            if (states.HasState(DashingState))
            {
                var state = states.GetState(DashingState);
                if (state.time > dashDuration)
                {
                    movement.extraSpeed = 0;
                    states.ExitState(DashingState);
                }
                
                // ref var position = ref world.GetComponent<PositionComponent>(entity);
                // position.value = new Vector2(-position.value.x, position.value.y);
                
                return;
            }
            
            movement.movingDirection = control.direction;

            if (control.button1.wasPressed)
            {
                movement.extraSpeed = dashExtraSpeed;
                states.EnterState(DashingState);
                return;
            }
        }

    }
}