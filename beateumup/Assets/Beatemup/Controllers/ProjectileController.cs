using Beatemup.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;

namespace Beatemup.Controllers
{
    public class ProjectileController : ControllerBase, IInit, IUpdate
    {
        public void OnInit()
        {
            // var gravity = world.GetComponent<GravityComponent>(entity);
            // var gravity = GetEntityComponent<GravityComponent>();
        }
        
        public void OnUpdate(float dt)
        {
            
        }
    }
}