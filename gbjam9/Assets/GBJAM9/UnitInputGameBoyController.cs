using GBJAM.Commons;
using UnityEngine;

namespace GBJAM9
{
    public class UnitInputGameBoyController : UnitInput
    {
        [SerializeField]
        protected GameboyButtonKeyMapAsset gameboyKeyMap;
        
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