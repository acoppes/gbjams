using System;
using GBJAM9.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM9.Controllers
{
    public class BasicEnemyController : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Wander,
            FollowingPlayer,
            ReturningToWander
        }
        
        private Entity entity;

        private State state;

        private Vector2 wanderCenter;

        public void Start()
        {
            entity = GetComponent<Entity>();
            state = State.Idle;
            wanderCenter = transform.position;
        }

        private void FixedUpdate()
        {
            // decide stuff
            
            // entity.world.GetEntityList<>()
            entity.input.movementDirection = Vector2.right;
        }
    }
}