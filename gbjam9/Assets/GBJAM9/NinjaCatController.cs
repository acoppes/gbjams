using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class NinjaCatController : MonoBehaviour
    {
        [FormerlySerializedAs("unitComponent")] 
        public UnitComponent unit;

        [SerializeField]
        protected UnitInput unitInput;

        [SerializeField]
        protected UnitMovement unitMovement;
        
        [SerializeField]
        protected UnitMovement dashMovement;
        
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
            unit.unitState.walking = false;
            unit.unitState.kunaiAttacking = false;

            if (dashingCurrentTime > 0)
            {
                unit.unitState.dashing = true;
                
                dashingCurrentTime -= Time.deltaTime;
                dashMovement.lookingDirection = dashDirection;
                dashMovement.Move();

                if (dashingCurrentTime <= 0)
                {
                    dashCooldownCurrentTime = dashCooldown;
                    unit.unitState.dashing = false;
                }

                return;
            }
            
            if (dashParticles != null)
            {
                dashParticles.Stop();
            }

            dashCooldownCurrentTime -= Time.deltaTime;
            
            if (unitInput.enabled && dashMovement != null && unitInput.dash && dashCooldownCurrentTime <= 0)
            {
                dashingCurrentTime = dashingTime;
                dashDirection = unitMovement.lookingDirection;
                
                if (dashParticles != null)
                {
                    dashParticles.Play();
                }

                if (dashSfx != null)
                {
                    dashSfx.Play();
                }

                unit.unitState.dashing = true;
                
                return;
            }
            
            if (unitInput.enabled && unitInput.movementDirection.SqrMagnitude() > 0)
            {
                unitMovement.lookingDirection = unitInput.movementDirection;
                unitMovement.Move();

                unit.unitState.walking = true;
            }

            unit.unitModel.lookingDirection = unitMovement.lookingDirection;

            if (unitInput.enabled && unitInput.attack && kunaiPrefab != null)
            {
                // fire kunai!!
                var kunaiObject = GameObject.Instantiate(kunaiPrefab);
                var kunai = kunaiObject.GetComponent<KunaiController>();
                kunai.Fire(transform.position, unitMovement.lookingDirection);
                kunai.unitComponent.player = unit.player;

                unit.unitState.kunaiAttacking = true;
            }
        }
    }
}
