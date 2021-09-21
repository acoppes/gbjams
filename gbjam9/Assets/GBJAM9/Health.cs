using System;
using UnityEngine;

namespace GBJAM9
{
    public class Health : MonoBehaviour
    {
        public int total;
        public int current;
        
        [NonSerialized]
        public int damages;
    }
}