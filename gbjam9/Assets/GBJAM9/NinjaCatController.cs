using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class NinjaCatController : MonoBehaviour
    {
        public Unit unit;
        
        [SerializeField]
        protected UnitInput unitInput;

        [SerializeField]
        protected UnitMovement unitMovement;
        
        [SerializeField]
        protected UnitMovement dashMovement;
        
        [SerializeField]
        UnitModel unitModel;

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
            unitModel.velocity = Vector2.zero;

            if (dashingCurrentTime > 0)
            {
                dashingCurrentTime -= Time.deltaTime;
                dashMovement.lookingDirection = dashDirection;
                dashMovement.Move();

                unitModel.velocity = dashMovement.velocity;
                    
                if (dashingCurrentTime <= 0)
                {
                    dashCooldownCurrentTime = dashCooldown;
                }
                return;
            }
            
            if (dashParticles != null)
            {
                dashParticles.Stop();
            }

            dashCooldownCurrentTime -= Time.deltaTime;
            
            if (dashMovement != null && unitInput.dash && dashCooldownCurrentTime <= 0)
            {
                dashingCurrentTime = dashingTime;
                dashDirection = unitInput.dashDirection;
                
                if (dashParticles != null)
                {
                    dashParticles.Play();
                }

                if (dashSfx != null)
                {
                    dashSfx.Play();
                }
                
                return;
            }
            
            if (unitInput.movementDirection.SqrMagnitude() > 0)
            {
                unitMovement.lookingDirection = unitInput.movementDirection;
                unitMovement.Move();
                
                unitModel.velocity = unitMovement.velocity;
            }

            unitModel.lookingDirection = unitMovement.lookingDirection;

            if (unitInput.attack && kunaiPrefab != null)
            {
                // fire kunai!!
                var kunaiObject = GameObject.Instantiate(kunaiPrefab);
                var kunai = kunaiObject.GetComponent<KunaiController>();
                kunai.Fire(transform.position, unitMovement.lookingDirection);
                kunai.unit.player = unit.player;
            }
        }
    }
}
