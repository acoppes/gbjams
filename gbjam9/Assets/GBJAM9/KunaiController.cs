using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class KunaiController : MonoBehaviour
    {
        public Unit unit;

        public Projectile projectile;
        
        [SerializeField]
        protected UnitMovement movement;

        [SerializeField]
        protected UnitModel model;

        [SerializeField]
        protected SfxVariant sfx;

        public void Fire(Vector3 position, Vector2 direction)
        {
            transform.position = position;
            movement.lookingDirection = direction;

            if (sfx != null)
            {
                sfx.Play();
            }
        }

        private void Update()
        {
            movement.Move();
            model.lookingDirection = movement.velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("collision");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("trigger");

            var unit = other.GetComponent<Unit>();
            if (unit != null)
            {
                if (unit.player != this.unit.player)
                {
                    // perform damage!
                    var health = unit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.damages += projectile.damage;
                    }
                }
                // check for unit player
            }
        }
    }
}