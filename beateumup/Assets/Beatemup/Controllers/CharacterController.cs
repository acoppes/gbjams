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
            // if (world.HasComponent<PlayerInputComponent>(entity))
            //     return;
        
            // ref var playerInput = ref world.GetComponent<PlayerInputComponent>(entity);
        }

    }
}