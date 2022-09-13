
using System.Linq;

namespace GBJAM9.Controllers
{
    public class DummyController : EntityController
    {
        public override void OnWorldUpdate(World world)
        {
            if (entity.health.damages > 0)
            {
                // look at player, we assume only the player can hit
                var ninjaCat = world.entities.FirstOrDefault(e => e.mainUnit != null);
                if (ninjaCat != null)
                {
                    entity.model.lookingDirection = ninjaCat.transform.position - entity.transform.position;
                    entity.model.lookingDirection.Normalize();
                }
            }
        }
    }
}