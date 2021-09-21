using System;
using UnityEngine;

namespace GBJAM9
{
    public class UnitState : MonoBehaviour
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