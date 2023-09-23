using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;

namespace GBJAM11.Controllers
{
    public class SwitchController : ControllerBase, IUpdate
    {
        // on entity collision with player, activate and open door
        
        public void OnUpdate(World world, Entity entity, float dt)
        {
            
        }
    }
}