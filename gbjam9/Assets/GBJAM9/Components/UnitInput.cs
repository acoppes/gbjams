using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitInput : MonoBehaviour, IGameComponent
    {
        [NonSerialized]
        public Vector2 movementDirection;

        [NonSerialized]
        public bool attack;

        [NonSerialized]
        public bool dash;
    }
}