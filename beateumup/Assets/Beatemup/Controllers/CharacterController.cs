using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;

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

            if (control.button1.wasPressed)
            {
                ref var position = ref world.GetComponent<PositionComponent>(entity);
                position.value = new Vector2(-position.value.x, position.value.y);
            }
        }

    }
}