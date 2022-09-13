using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM10.Components
{
    public class ColliderComponent : MonoBehaviour, IEntityComponent
    {
        public Collider2D collider;

        public Rigidbody2D rigidbody;

        [NonSerialized]
        public List<ContactPoint2D> contactsList = new List<ContactPoint2D>();
        
        [NonSerialized]
        public List<Entity> collidingEntities = new List<Entity>();

        [NonSerialized]
        public bool inCollision;

        private void Awake()
        {
            if (collider == null)
            {
                collider = GetComponent<Collider2D>();
            }
            
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody2D>();
            }
        }
    }
}