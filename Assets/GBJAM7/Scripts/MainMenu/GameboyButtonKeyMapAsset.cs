using System;
using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    [CreateAssetMenu(menuName = "GameboyKeyMap")]
    public class GameboyButtonKeyMapAsset : ScriptableObject
    {
        public KeyCode leftKey;
        public KeyCode rigthKey;
        public KeyCode upKey;
        public KeyCode downKey;

        public KeyCode button1KeyCode;
        public KeyCode button2KeyCode;
        
        public KeyCode startKeyCode;
        public KeyCode selectKeyCode;
        
        [NonSerialized]
        public bool leftPressed;
        
        [NonSerialized]
        public bool rightPressed;

        [NonSerialized]
        public bool upPressed;

        [NonSerialized]
        public bool downPressed;
        
        [NonSerialized]
        public bool button1Pressed;
        
        [NonSerialized]
        public bool button2Pressed;

        [NonSerialized]
        public bool startPressed;

        public KeyCode[] GetAllKeyCodes()
        {
            return new[]
            {
                leftKey,
                rigthKey,
                upKey,
                downKey,
                button1KeyCode,
                button2KeyCode,
                startKeyCode,
                selectKeyCode
            };
        }

        public bool AnyButtonPressed()
        {
            return leftPressed || rightPressed || upPressed || downPressed ||
                   button1Pressed || button2Pressed || startPressed;
        }

        public void UpdateControlState()
        {
            leftPressed = Input.GetKeyDown(leftKey);
            rightPressed = Input.GetKeyDown(rigthKey);
            
            upPressed = Input.GetKeyDown(upKey);
            downPressed = Input.GetKeyDown(downKey);

            startPressed = Input.GetKeyDown(startKeyCode);

            button1Pressed = Input.GetKeyDown(button1KeyCode);
            button2Pressed = Input.GetKeyDown(button2KeyCode);
        }
        
        
    }
}