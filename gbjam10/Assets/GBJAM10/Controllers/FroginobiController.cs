using GBJAM10.Components;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class FroginobiController : EntityController
    {
        public enum State
        {
            Wander,
            Charging,
            Attacking,
            Recovering
        }

        private State state;

        public Vector2 startingWanderDirection;

        private float wanderSwitchDirectionCooldown;

        private float attackingPlayerCooldown;

        public float waitForWanderDuration = 2.0f;

        public float chargeDuration = 0.25f;
        public float recoverDuration = 0.5f;

        private float waitForWanderTime;
        private float chargeTime;
        private float recoverTime;

        private Vector2 attackDirection;
        
        // TODO: use spawn parameters for this, like the spawn point or something
        // or use variants

        public override void OnInit(World world)
        {
            state = State.Wander;
        }

        public override void OnWorldUpdate(World world)
        {
            if (!entity.health.alive)
            {
                return;
            }

            var gameEntity = world.GetSingleton("Game");
            if (gameEntity != null)
            {
                if (gameEntity.game.state == GameComponent.State.TransitionToRoom)
                {
                    return;
                }
            }
                
            var playerMask = entity.player.enemyLayerMask;

            var distance = 4;
            var directions = new Vector2[] { Vector2.right, Vector2.left, Vector2.down, Vector2.up };

            var currentAttackDirection = new Vector2();

            var canAttack = false;
            
            foreach (var direction in directions)
            {
                var hit = Physics2D.Raycast(transform.position, direction,
                    distance, playerMask);

                if (hit.collider != null)
                {
                    var health = hit.collider.GetComponent<HealthComponent>();
                    canAttack = health != null && health.current > 0 && hit.distance > 0;    
                }
                
                if (canAttack)
                {
                    currentAttackDirection = direction;
                    break;
                }
            }

            entity.state.chargeAttack2 = false;
            entity.input.attack = false;
            
            if (state == State.Wander)
            {
                if (!canAttack)
                {
                    waitForWanderTime -= Time.deltaTime;
                    
                    if (waitForWanderTime < 0)
                    {
                        wanderSwitchDirectionCooldown -= Time.deltaTime;

                        entity.input.movementDirection = startingWanderDirection;
                        if (entity.collider.inCollision && wanderSwitchDirectionCooldown < 0)
                        {
                            startingWanderDirection *= -1;
                            wanderSwitchDirectionCooldown = 0.2f;
                        }
                    }
                }

                if (canAttack)
                {
                    state = State.Charging;
                    chargeTime = chargeDuration;
                    attackDirection = currentAttackDirection;
                }
            } 
            
            if (state == State.Charging)
            {
                entity.input.movementDirection = Vector2.zero;
                entity.state.chargeAttack2 = true;

                chargeTime -= Time.deltaTime;

                if (chargeTime <= 0)
                {
                    state = State.Attacking;
                }
                
                // if charge duration completed, attack
            }
            
            if (state == State.Attacking)
            {
                entity.input.movementDirection = Vector2.zero;
                
                // press attack button
                // entity.input.movementDirection = attackDirection;
                
                entity.input.attackDirection = attackDirection;
                entity.input.attack = true;
                state = State.Recovering;
                recoverTime = recoverDuration;

                return;

                // if (canAttack)
                // {
                //     if (entity.attack.cooldown <= 0)
                //     {
                //         entity.input.movementDirection = attackDirection;
                //         entity.input.attack = true;
                //     }
                //     
                //     // entity.input.movementDirection = 
                //     attackingPlayerCooldown = 0.3f;
                // }

                // attackingPlayerCooldown -= Time.deltaTime;
                //
                // if (attackingPlayerCooldown < 0)
                // {
                //     state = State.Wander;
                // }
            }
            
            if (state == State.Recovering)
            {
                // wait idle for a while before doing any logic again...
                entity.input.movementDirection = Vector2.zero;
                entity.input.attack = false;
                entity.input.dash = false;

                recoverTime -= Time.deltaTime;

                if (recoverTime < 0)
                {
                    state = State.Wander;
                    waitForWanderTime = waitForWanderDuration;
                }
            }
        }
    }
}