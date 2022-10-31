using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;

namespace Beatemup.Controllers
{
    public class CharacterController : ControllerBase, IInit, IEntityDestroyed
    {
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
            
            movement.movingDirection = control.direction;
        }

    }
}