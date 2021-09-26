using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9.Controllers
{
    public class NinjaCatController : MonoBehaviour
    {
        [FormerlySerializedAs("unit")] 
        [FormerlySerializedAs("unitComponent")] 
        public Entity entity;

        [SerializeField]
        protected ParticleSystem dashParticles;

        [SerializeField]
        protected SfxVariant dashSfx;

        // Update is called once per frame
        private void Update()
        {
            var dash = entity.dash;
            
            if (dash.durationCurrent > 0)
            {
                entity.state.dashing = true;
                
                dash.durationCurrent -= Time.deltaTime;
                entity.movement.lookingDirection = dash.direction;
                
                if (dash.durationCurrent <= 0)
                {
                    dash.cooldownCurrent = dash.cooldown;
                    entity.state.dashing = false;
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

                if (dashSfx != null)
                {
                    dashSfx.Play();
                }

                entity.state.dashing = true;
            }



        }
    }
}
