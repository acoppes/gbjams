using System;
using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class KunaiController : MonoBehaviour
    {
        [FormerlySerializedAs("unitComponent")] 
        [FormerlySerializedAs("unit")] 
        public EntityComponent entityComponent;

        [FormerlySerializedAs("sfx")] [SerializeField]
        protected SfxVariant fireSfx;

        [SerializeField]
        protected GameObject hitSfxPrefab;

        public float startOffset = 0.4f;
        
        public void Fire(Vector3 position, Vector2 direction)
        {
            var offset = direction.normalized * startOffset;
            transform.position = position + new Vector3(offset.x, offset.y, 0);
            entityComponent.movement.lookingDirection = direction;

            if (fireSfx != null)
            {
                fireSfx.Play();
            }
        }

        private void Update()
        {
            entityComponent.model.lookingDirection = entityComponent.movement.velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("collision");
            
            var unit = other.collider.GetComponent<EntityComponent>();
            if (unit != null)
            {
                if (unit.player != entityComponent.player)
                {
                    // perform damage!
                    var health = unit.GetComponent<HealthComponent>();
                    if (health != null)
                    {
                        health.damages += entityComponent.projectileComponent.damage;
                    }
                }
            }
            
            // autodamage on hit
            var myHealth = entityComponent.GetComponent<HealthComponent>();
            if (myHealth != null)
            {
                myHealth.damages += entityComponent.projectileComponent.damage;
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