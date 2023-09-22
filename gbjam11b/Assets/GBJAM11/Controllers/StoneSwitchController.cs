using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM11.Controllers
{
    public class StoneSwitchController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var states = ref entity.Get<StatesComponent>();
            ref var animations = ref entity.Get<AnimationComponent>();
            
            var physics = entity.Get<PhysicsComponent>();

            if (states.TryGetState("Pressed", out var pressedState))
            {
                if (physics.contactsCount == 0)
                {
                    states.ExitState(pressedState.name);
                    animations.Play("Idle", 0);
                }
            }
            else
            {
                if (physics.contactsCount > 0)
                {
                    states.EnterState("Pressed");
                    animations.Play("Pressed", 0);
                }
            }

        }
        
        
    }
}