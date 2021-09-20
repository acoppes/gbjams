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
        public bool attack;

        [NonSerialized]
        public bool dash;
        
        [NonSerialized]
        public Vector2 dashDirection;

        private void Update()
        {
            dash = false;
            dashDirection = Vector2.zero;
            
            movementDirection = gameboyKeyMap.direction;
            attack = gameboyKeyMap.button1Pressed;
            dash = gameboyKeyMap.button2Pressed;

            dashDirection = movementDirection;
        }
    }
}