using System;
using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class KunaiController : MonoBehaviour
    {
        [FormerlySerializedAs("entityComponent")]
        [FormerlySerializedAs("unitComponent")] 
        [FormerlySerializedAs("unit")] 
        public Entity entity;

        [FormerlySerializedAs("sfx")] [SerializeField]
        protected SfxVariant fireSfx;

        [SerializeField]
        protected GameObject hitSfxPrefab;

        public float startOffset = 0.4f;
        
        public void Fire(Vector3 position, Vector2 direction)
        {
            var offset = direction.normalized * startOffset;
            transform.position = position + new Vector3(offset.x, offset.y, 0);
            
            // entityComponent.movement.lookingDirection = direction;
            entity.movement.movingDirection = direction;

            if (fireSfx != null)
            {
                fireSfx.Play();
            }
        }

        private void Update()
        {
            entity.model.lookingDirection = entity.movement.velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("collision");
            
            var otherEntity = other.collider.GetComponent<Entity>();
            if (otherEntity != null)
            {
                if (otherEntity.player.player == entity.player.player)
                    return;
                
                var health = otherEntity.GetComponent<HealthComponent>();
                if (health != null)
                {
                    health.damages += entity.projectileComponent.damage;
                }
            }
            
            // autodamage on hit
            var myHealth = entity.GetComponent<HealthComponent>();
            if (myHealth != null)
            {
                myHealth.damages += entity.projectileComponent.damage;
            }

            if (hitSfxPrefab != null)
            {
                Instantiate(hitSfxPrefab, transform.position, Quaternion.identity);
            }
            // check for unit player
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("trigger");


        }
    }
}