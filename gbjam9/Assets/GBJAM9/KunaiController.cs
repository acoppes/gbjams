using System;
using GBJAM.Commons;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9
{
    public class KunaiController : MonoBehaviour
    {
        [FormerlySerializedAs("unit")] public UnitComponent unitComponent;

        [FormerlySerializedAs("projectile")] public ProjectileComponent projectileComponent;
        
        [SerializeField]
        protected UnitMovement movement;

        [SerializeField]
        protected UnitModel model;

        [FormerlySerializedAs("sfx")] [SerializeField]
        protected SfxVariant fireSfx;

        [SerializeField]
        protected GameObject hitSfxPrefab;
        
        public void Fire(Vector3 position, Vector2 direction)
        {
            transform.position = position;
            movement.lookingDirection = direction;

            if (fireSfx != null)
            {
                fireSfx.Play();
            }
        }

        private void Update()
        {
            movement.Move();
            model.lookingDirection = movement.velocity;
        }

        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     Debug.Log("collision");
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("trigger");

            var unit = other.GetComponent<UnitComponent>();
            if (unit != null)
            {
                if (unit.player != this.unitComponent.player)
                {
                    // perform damage!
                    var health = unit.GetComponent<HealthComponent>();
                    if (health != null)
                    {
                        health.damages += projectileComponent.damage;
                    }

                    // autodamage on hit
                    var myHealth = this.unitComponent.GetComponent<HealthComponent>();
                    if (myHealth != null)
                    {
                        myHealth.damages += projectileComponent.damage;
                    }

                    if (hitSfxPrefab != null)
                    {
                        Instantiate(hitSfxPrefab, transform.position, Quaternion.identity);
                    }
                }
                // check for unit player
            }
        }
    }
}