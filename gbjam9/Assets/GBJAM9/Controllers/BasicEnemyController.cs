using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class BasicEnemyController : MonoBehaviour
    {
        public enum State
        {
            Wander,
            AttackingPlayer,
            FollowingPlayer,
            ReturningToWander
        }
        
        private Entity entity;

        private State state;

        private Vector2 wanderCenter;

        public Vector2 startingWanderDirection;

        private float wanderSwitchDirectionCooldown;

        private float attackingPlayerCooldown;
        
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
            // TODO: configure mask in player component

            var playerMask = entity.player.enemyLayerMask;
            
            var hit = Physics2D.Raycast(transform.position, entity.input.movementDirection, 
                7, playerMask);

            var playerDetected = hit.collider != null && hit.distance > 0;
            
            entity.input.attack = false;
            
            if (state == State.Wander)
            {
                wanderSwitchDirectionCooldown -= Time.deltaTime;
                
                entity.input.movementDirection = startingWanderDirection;
                if (entity.colliderComponent.inCollision && wanderSwitchDirectionCooldown < 0)
                {
                    startingWanderDirection *= -1;
                    wanderSwitchDirectionCooldown = 0.2f;
                }

                if (playerDetected)
                {
                    state = State.AttackingPlayer;
                }
            } 
            
            if (state == State.AttackingPlayer)
            {
                entity.input.movementDirection = Vector2.zero;
                
                if (playerDetected)
                {
                    entity.input.attack = true;
                    attackingPlayerCooldown = 0.3f;
                }

                attackingPlayerCooldown -= Time.deltaTime;

                if (attackingPlayerCooldown < 0)
                {
                    state = State.Wander;
                }
            }

        }
    }
}