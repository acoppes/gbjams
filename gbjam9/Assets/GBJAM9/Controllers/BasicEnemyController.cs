using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class BasicEnemyController : MonoBehaviour
    {
        public enum State
        {
            Wander,
            FollowingPlayer,
            ReturningToWander
        }
        
        private Entity entity;

        private State state;

        private Vector2 wanderCenter;

        public Vector2 startingWanderDirection;

        private float wanderSwitchDirectionCooldown;
        
        // TODO: use spawn parameters for this, like the spawn point or something
        // or use variants

        public void Start()
        {
            entity = GetComponent<Entity>();
            state = State.Wander;
            wanderCenter = transform.position;
        }

        private void FixedUpdate()
        {
            // decide stuff
            
            // entity.world.GetEntityList<>()
            if (state == State.Wander)
            {
                wanderSwitchDirectionCooldown -= Time.deltaTime;
                
                entity.input.movementDirection = startingWanderDirection;
                if (entity.colliderComponent.inCollision && wanderSwitchDirectionCooldown < 0)
                {
                    startingWanderDirection *= -1;
                    wanderSwitchDirectionCooldown = 0.2f;
                }
            }

            // var playerLayer = LayerMask.NameToLayer("Player");
            var playerMask = LayerMask.GetMask("Player");
            // var playerLayerMask = Physics2D.GetLayerCollisionMask(playerLayer);

            var hit = Physics2D.Raycast(transform.position, entity.input.movementDirection, 
                7, playerMask);
            entity.input.attack = false;
            if (hit.collider != null && hit.distance > 0)
            {
                // hit.collider.GetComponent<>()
                entity.input.attack = true;
            }

        }
    }
}