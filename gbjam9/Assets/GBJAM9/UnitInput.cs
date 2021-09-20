using System;
using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class UnitInput : MonoBehaviour
    {
        [SerializeField]
        protected GameboyButtonKeyMapAsset gameboyKeyMap;

        [NonSerialized]
        public Vector2 movementDirection;

        [NonSerialized]
        public bool fireKunai;
        
        private void Update()
        {
            movementDirection = gameboyKeyMap.direction;
            fireKunai = gameboyKeyMap.button2Pressed;
        }
    }
}