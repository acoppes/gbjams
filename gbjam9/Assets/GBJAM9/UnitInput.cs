using System;
using UnityEngine;

namespace GBJAM9
{
    public class UnitInput : MonoBehaviour
    {
        [NonSerialized]
        public Vector2 movementDirection;

        [NonSerialized]
        public bool attack;

        [NonSerialized]
        public bool dash;
        
        [NonSerialized]
        public Vector2 dashDirection;
    }
}