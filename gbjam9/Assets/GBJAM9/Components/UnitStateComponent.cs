using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class UnitStateComponent : MonoBehaviour, IGameComponent
    {
        [NonSerialized]
        public bool walking;

        [NonSerialized]
        public bool kunaiAttacking;

        [NonSerialized]
        public bool dashing;
        
        [NonSerialized]
        public bool hit;
    }
}