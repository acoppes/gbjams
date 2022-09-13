using System;
using UnityEngine;

namespace GBJAM9.Components
{
    public class PlayerComponent : MonoBehaviour, IEntityComponent
    {
        public int player;

        [NonSerialized]
        public int layer;
        
        [NonSerialized]
        public int enemyLayer;

        [NonSerialized]
        public int projectileLayer;

        [NonSerialized]
        public int layerMask;
        
        [NonSerialized]
        public int enemyLayerMask;
    }
}