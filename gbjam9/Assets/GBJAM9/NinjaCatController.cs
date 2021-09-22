using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class NinjaCatController : MonoBehaviour
    {
        [FormerlySerializedAs("unit")] public UnitComponent unitComponent;

        public UnitState unitState;
        
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
            unitState.walking = false;
            unitState.kunaiAttacking = false;

            if (dashingCurrentTime > 0)
            {
                unitState.dashing = true;
                
                dashingCurrentTime -= Time.deltaTime;
                dashMovement.lookingDirection = dashDirection;
                dashMovement.Move();

                if (dashingCurrentTime <= 0)
                {
                    dashCooldownCurrentTime = dashCooldown;
                    unitState.dashing = false;
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

                unitState.dashing = true;
                
                return;
            }
            
            if (unitInput.movementDirection.SqrMagnitude() > 0)
            {
                unitMovement.lookingDirection = unitInput.movementDirection;
                unitMovement.Move();

                unitState.walking = true;
            }

            unitModel.lookingDirection = unitMovement.lookingDirection;

            if (unitInput.attack && kunaiPrefab != null)
            {
                // fire kunai!!
                var kunaiObject = GameObject.Instantiate(kunaiPrefab);
                var kunai = kunaiObject.GetComponent<KunaiController>();
                kunai.Fire(transform.position, unitMovement.lookingDirection);
                kunai.unitComponent.player = unitComponent.player;

                unitState.kunaiAttacking = true;
            }
        }
    }
}
