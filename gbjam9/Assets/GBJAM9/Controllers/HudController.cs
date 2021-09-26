using GBJAM9.UI;

namespace GBJAM9.Controllers
{
    public class HudController : EntityController
    {
        public HealthUI healthUI;
        
        public override void OnWorldUpdate(World world)
        {
            var nekonin = world.GetSingleton("Nekonin");

            healthUI.SetHealth(nekonin.health);
        }
    }
}