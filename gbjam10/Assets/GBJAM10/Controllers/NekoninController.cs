using UnityEngine;

namespace GBJAM10.Controllers
{
    public class NekoninController : EntityController
    {
        [SerializeField]
        protected ParticleSystem dashParticles;

        public override void OnWorldUpdate(World world)
        {
            var dash = entity.dash;
            
            if (dash.durationCurrent > 0)
            {
                entity.state.dashing = true;
                
                dash.durationCurrent -= Time.deltaTime;
                
                if (dash.durationCurrent <= 0)
                {
                    dash.cooldownCurrent = dash.cooldown;
                    entity.state.dashing = false;
                    
                    entity.gameObject.layer = LayerMask.NameToLayer("Player");
                }

                return;
            }
            
            if (dashParticles != null)
            {
                dashParticles.Stop();
            }

            dash.cooldownCurrent -= Time.deltaTime;
            
            if (entity.input.enabled && entity.input.dash && dash.cooldownCurrent <= 0)
            {
                dash.durationCurrent = dash.duration;
                dash.direction = entity.movement.lookingDirection;
                
                if (dashParticles != null)
                {
                    dashParticles.Play();
                }

                if (dash.sfx != null)
                {
                    dash.sfx.Play();
                }

                entity.state.dashing = true;

                // change the collider layer while dashing...
                entity.gameObject.layer = LayerMask.NameToLayer("IgnoreAttacks");
            }
        }

    }
}
