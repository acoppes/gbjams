using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJAM9.Components
{
    public class ColliderComponent : MonoBehaviour, IGameComponent
    {
        public Collider2D collider;

        [NonSerialized]
        public List<ContactPoint2D> contactsList = new List<ContactPoint2D>();
        
        [NonSerialized]
        public List<Entity> collidingEntities = new List<Entity>();

        [NonSerialized]
        public bool inCollision;
    }
}