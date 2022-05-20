using System;
using UnityEngine;

namespace GBJAM.Commons
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
        public bool button1JustPressed;
        
        [NonSerialized]
        public bool button2JustPressed;

        [NonSerialized]
        public bool startPressed;

        [NonSerialized]
        public Vector2 direction;

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
        
        public float keyRepeatCooldown = 0.5f;
        private float keyRepeatCurrent = 1000.0f;
        
        public bool AnyButtonPressed()
        {
            return leftPressed || rightPressed || upPressed || downPressed ||
                   button1JustPressed || button2JustPressed || startPressed;
        }

        public void UpdateControlState()
        {
            direction = Vector2.zero;
            
            keyRepeatCurrent += Time.deltaTime;
            
            leftPressed = false;
            rightPressed = false;
            
            upPressed = false;
            downPressed = false;

            startPressed = false;
            
            leftPressed = false;
            rightPressed = false;

            button1Pressed = false;
            button2Pressed = false;
            
            startPressed = Input.GetKeyDown(startKeyCode);

            button1JustPressed = Input.GetKeyDown(button1KeyCode);
            button2JustPressed = Input.GetKeyDown(button2KeyCode);
            
            if (keyRepeatCurrent > keyRepeatCooldown)
            {
                button1Pressed = Input.GetKey(button1KeyCode);
                button2Pressed = Input.GetKey(button2KeyCode);
                
                leftPressed = Input.GetKey(leftKey);
                rightPressed = Input.GetKey(rigthKey);

                upPressed = Input.GetKey(upKey);
                downPressed = Input.GetKey(downKey);

                direction.x += leftPressed ? -1 : 0;
                direction.x += rightPressed ? 1 : 0;
                direction.y += downPressed ? -1 : 0;
                direction.y += upPressed ? 1 : 0;

                if (AnyButtonPressed())
                {
                    keyRepeatCurrent = 0;
                }
            }

            foreach (var keyCode in GetAllKeyCodes())
            {
                if (Input.GetKeyUp(keyCode))
                {
                    keyRepeatCurrent = 1000;
                }
            }

        }
        
        
    }
}