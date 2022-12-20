using System.Collections.Generic;
using Gemserk.Gameplay;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class EntityCollisionDelegate : MonoBehaviour
    {
        public struct EntityCollision
        {
            public Entity entity;
            public Collision collision;
        }
        
        public delegate void CollisionHandler(EntityCollision entityCollision);
        public CollisionHandler onCollisionEnter;

        private List<PhysicsCollisionsDelegate> physicsDelegates = new List<PhysicsCollisionsDelegate>();

        private void Awake()
        {
            GetComponentsInChildren(physicsDelegates);
        }

        private void OnEnable()
        {
            foreach (var physicsDelegate in physicsDelegates)
            {
                physicsDelegate.onCollisionEnter += OnCollisionEnter;
            }
        }

        private void OnDisable()
        {
            foreach (var physicsDelegate in physicsDelegates)
            {
                physicsDelegate.onCollisionEnter -= OnCollisionEnter;
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter != null)
            {
                var entityReference = collision.gameObject.GetComponentInParent<EntityReference>();
                onCollisionEnter(new EntityCollision()
                {
                    entity = entityReference.entity,
                    collision = collision
                });
            }
        }
    }
}