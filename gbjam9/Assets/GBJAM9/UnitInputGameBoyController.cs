using GBJAM.Commons;
using GBJAM9.Components;
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
            
            movementDirection = gameboyKeyMap.direction;
            attack = gameboyKeyMap.button1Pressed;
            dash = gameboyKeyMap.button2Pressed;
        }
    }
}