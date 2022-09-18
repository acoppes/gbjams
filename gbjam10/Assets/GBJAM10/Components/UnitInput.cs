using System;
using UnityEngine;

namespace GBJAM10.Components
{
    public class UnitInput : MonoBehaviour, IEntityComponent
    {
        [NonSerialized]
        public Vector2 movementDirection;
        
        [NonSerialized]
        public Vector2 attackDirection;

        [NonSerialized]
        public bool attack;

        [NonSerialized]
        public bool dash;
    }
}