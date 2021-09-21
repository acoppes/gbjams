using System;
using UnityEngine;

namespace GBJAM9
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField]
        protected Transform transform;

        [SerializeField]
        protected float speed;

        [SerializeField]
        protected Vector2 perspective = new Vector2(1.0f, 0.75f);

        [NonSerialized]
        public Vector2 velocity;

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);
        
        public void Move()
        {
            var newPosition = transform.localPosition;
            velocity = lookingDirection * speed * Time.deltaTime;

            newPosition.x += velocity.x * perspective.x;
            newPosition.y += velocity.y * perspective.y;

            var collider = Physics2D.OverlapPoint( newPosition);
            if (collider == null)
            {
                transform.localPosition = newPosition;
            }
        }
    }
}