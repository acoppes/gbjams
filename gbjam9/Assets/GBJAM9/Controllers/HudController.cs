using GBJAM9.UI;

namespace GBJAM9.Controllers
{
    public class HudController : EntityController
    {
        public HealthUI healthUI;

        public SkillsUI skillsUI;
        
        public override void OnWorldUpdate(World world)
        {
            var nekonin = world.GetSingleton("Nekonin");

            healthUI.SetHealth(nekonin.health);

            // need weapon
            // need attack and dash

            skillsUI.entity = nekonin;

        }
    }
}