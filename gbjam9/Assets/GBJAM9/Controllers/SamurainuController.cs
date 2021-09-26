using System;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public class SamurainuController : EntityController
    {
        public enum State
        {
            Wander,
            Chasing,
            Charging,
            Attacking,
            Recovering
        }

        private State state;

        private Vector2 attackDirection;

        public float chargeAttackDistance = 2.0f;
        public float chaseMaxDistance = 6.0f;
        
        public float chargeDuration;
        public float recoverDuration;

        private float chargeTime;
        private float recoverTime;

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

            var nekonin = world.GetSingleton("Nekonin");
            
            // raycast nekonin, if obstacle, ignore it, continue wandering

            var canAttack = false;
            
            attackDirection = nekonin.transform.position - entity.transform.position;
            var distance = 10;

            var myLayerMask = entity.player.layerMask;
            // all layers but mine
            var layerMask = Physics2D.AllLayers & ~myLayerMask;
            
            var hit = Physics2D.Raycast(entity.transform.position, attackDirection,
                distance, layerMask);

            var nekoninDistance = 1000.0f;
            
            if (hit.collider != null)
            {
                nekoninDistance = hit.distance;
                var hitEntity = hit.collider.GetComponent<Entity>();
                if (hitEntity != null && hitEntity == nekonin && 
                    hitEntity.health != null && hitEntity.health.alive)
                {
                    canAttack = true;
                }
            }

            entity.input.attack = false;
            
            entity.state.chargeAttack1 = false;
            
            if (state == State.Wander)
            {
                if (canAttack)
                {
                    state = State.Chasing;
                }
            } 
            
            if (state == State.Chasing)
            {
                entity.input.movementDirection = attackDirection.normalized;

                if (!canAttack)
                {
                    state = State.Wander;
                    return;
                }

                if (nekoninDistance < chargeAttackDistance)
                {
                    state = State.Charging;
                    chargeTime = chargeDuration;
                } else if (nekoninDistance > chaseMaxDistance)
                {
                    state = State.Wander;
                    entity.input.movementDirection = Vector2.zero;
                }

                // check if attack cooldown ready

                // run at least one chase frame
                return;
            }

            if (state == State.Charging)
            {
                entity.input.movementDirection = Vector2.zero;
                
                // if (!canAttack)
                // {
                //     state = State.Wander;
                //     return;
                // }

                entity.state.chargeAttack1 = true;

                chargeTime -= Time.deltaTime;

                if (chargeTime <= 0)
                {
                    state = State.Attacking;
                }

                // if charge duration completed, attack
            }

            if (state == State.Attacking)
            {
                // press attack button
                entity.input.attack = true;
                state = State.Recovering;
                recoverTime = recoverDuration;

                // wait for next frame
                return;
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
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (state == State.Wander)
            {
                Gizmos.color = Color.blue;
                var a = attackDirection.normalized * 2.0f;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3) a);
            }

            if (state == State.Attacking)
            {
                Gizmos.color = Color.red;
                var a = attackDirection.normalized * 2.0f;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3) a);
            }
        }
    }
}