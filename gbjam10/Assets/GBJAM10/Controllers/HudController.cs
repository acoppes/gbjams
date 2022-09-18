using GBJAM10.UI;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class HudController : EntityController
    {
        public HealthUI healthUI;

        public SkillsUI skillsUI;

        public Animator animator;
        
        private static readonly int visibleHash = Animator.StringToHash("visible");
        
        public override void OnWorldUpdate(World world)
        {
            var nekonin = world.GetSingleton("Nekonin");
            
            if (nekonin != null)
            {

                healthUI.SetHealth(nekonin.health);

                // need weapon
                // need attack and dash

                skillsUI.entity = nekonin;
            }

            animator.SetBool(visibleHash, entity.hud.visible && nekonin != null);
        }
    }
}