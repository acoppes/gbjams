using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class UnitMovement : MonoBehaviour, IEntityComponent
    {
        public float speed;

        public Vector2 perspective = new Vector2(1.0f, 0.75f);

        [NonSerialized]
        public Vector2 lookingDirection = new Vector2(1, 0);
        
        [NonSerialized]
        public Vector2 movingDirection = new Vector2(0, 0);
    }
}