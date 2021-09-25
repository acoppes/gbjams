using UnityEngine;

namespace GBJAM9.Controllers
{
    public class BasicEnemyController : EntityController
    {
        public enum State
        {
            Wander,
            AttackingPlayer,
            FollowingPlayer,
            ReturningToWander
        }

        private State state;

        private Vector2 wanderCenter;

        public Vector2 startingWanderDirection;

        private float wanderSwitchDirectionCooldown;

        private float attackingPlayerCooldown;
        
        // TODO: use spawn parameters for this, like the spawn point or something
        // or use variants

        public override void OnInit(World world)
        {
            state = State.Wander;
            wanderCenter = transform.position;
        }

        public override void OnWorldUpdate(World world)
        {

            var playerMask = entity.player.enemyLayerMask;

            var distance = 4;
            var directions = new Vector2[] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };

            var attackDirection = new Vector2();

            var playerDetected = false;
            
            foreach (var direction in directions)
            {
                var hit = Physics2D.Raycast(transform.position, direction,
                    distance, playerMask);
                playerDetected = hit.collider != null && hit.distance > 0;
                if (playerDetected)
                {
                    attackDirection = direction;
                    break;
                }
            }

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
                    if (entity.attack.cooldown <= 0)
                    {
                        entity.input.movementDirection = attackDirection;
                        entity.input.attack = true;
                    }
                    
                    // entity.input.movementDirection = 
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