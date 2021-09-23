using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class NinjaCatController : MonoBehaviour
    {
        [FormerlySerializedAs("unit")] [FormerlySerializedAs("unitComponent")] 
        public EntityComponent entity;
        
        [SerializeField]
        protected GameObject kunaiPrefab;

        [SerializeField]
        protected float dashingTime = 0.15f;

        [SerializeField]
        protected float dashCooldown = 1.0f;

        private float dashingCurrentTime;
        private float dashCooldownCurrentTime;
        private Vector2 dashDirection;

        [SerializeField]
        protected ParticleSystem dashParticles;

        [SerializeField]
        protected SfxVariant dashSfx;

        // Update is called once per frame
        private void Update()
        {
            // entity.state.walking = false;
            
            entity.state.kunaiAttacking = false;

            if (dashingCurrentTime > 0)
            {
                entity.state.dashing = true;
                
                dashingCurrentTime -= Time.deltaTime;
                entity.movement.lookingDirection = dashDirection;
                
                // dashMovement.lookingDirection = dashDirection;

                // dashMovement.Move();

                if (dashingCurrentTime <= 0)
                {
                    dashCooldownCurrentTime = dashCooldown;
                    entity.state.dashing = false;
                }

                return;
            }
            
            if (dashParticles != null)
            {
                dashParticles.Stop();
            }

            dashCooldownCurrentTime -= Time.deltaTime;
            
            if (entity.input.enabled && entity.input.dash && dashCooldownCurrentTime <= 0)
            {
                dashingCurrentTime = dashingTime;
                dashDirection = entity.movement.lookingDirection;
                
                if (dashParticles != null)
                {
                    dashParticles.Play();
                }

                if (dashSfx != null)
                {
                    dashSfx.Play();
                }

                entity.state.dashing = true;
                
                return;
            }

            // entity.model.lookingDirection = unitMovement.lookingDirection;

            if (entity.input.enabled && entity.input.attack && kunaiPrefab != null)
            {
                // fire kunai!!
                var kunaiObject = GameObject.Instantiate(kunaiPrefab);
                var kunai = kunaiObject.GetComponent<KunaiController>();
                kunai.Fire(transform.position, entity.movement.lookingDirection);
                kunai.entityComponent.player.player = entity.player.player;

                entity.state.kunaiAttacking = true;
            }
        }
    }
}
